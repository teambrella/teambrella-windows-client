/* Copyright(C) 2016  Teambrella, Inc.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License(version 3) as published
 * by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see<http://www.gnu.org/licenses/>.
 */
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teambrella.Client.Services;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.App
{
    public partial class DisbandForm : Form
    {
        private AccountService _accountService;

        const string ColOwner = "Owner";
        const string ColCosigner = "Cosigner";
        const string ColStatus = "Status";
        const string ColOwnerStatus = "OwnerStatus";

        private const decimal MinAuxWalletBTC = 0.003M;
        private const decimal RecomendedAuxWalletBTC = 0.004M;

        private Teammate _me;

        private decimal _amountSatoshisCurent = -1;
        private decimal _amountSatoshisPrevious = -1;

        private BtcAddress _curAddress;
        private BtcAddress _prevAddress;
        private User _user;

        private bool _closing = false;

        public DisbandForm(int myTeammateId)
        {
            _accountService = new AccountService();
            _me = _accountService.GetTeammate(myTeammateId);
            _curAddress = _me.BtcAddressCurrent;
            _prevAddress = _me.BtcAddressPrevious;
            _user = _accountService.GetUser();

            InitializeComponent();

            _textBoxAuxAddress.Text = _accountService.GetAuxWallet(_me.Team.Network).ToString();
            _textBoxAddressTo.Text = _me.CurDisbanding.WithdrawAddr;

            UpdateStatuses();
            UpdateWithdrawalControls();
            UpdateCosignerOfControls();

            Task.Factory.StartNew(() =>
            {
                if (!_accountService.Closed) // todo: change it to separate contexts
                {
                    var blockchainService = new BlockchainService(_accountService);
                    blockchainService.TryFetchAuxWalletUtxos(true, _me.Team.Network);
                    if (!_closing)
                    {
                        try
                        {
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                UpdateStatuses();
                            });
                        }
                        catch (Exception ex) {
                            Logger.WriteException(ex);
                        }
                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                if (!_accountService.Closed) // todo: change it to separate contexts
                {
                    var blockchainService = new BlockchainService(_accountService);
                    if (_curAddress != null)
                    {
                        _amountSatoshisCurent = blockchainService.FetchBalance(_curAddress);
                    }
                    if (_prevAddress != null)
                    {
                        _amountSatoshisPrevious = blockchainService.FetchBalance(_prevAddress);
                    }
                    if (!_closing)
                    {
                        try
                        {
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                UpdateWithdrawalControls();
                            });
                        }
                        catch (Exception ex) {
                            Logger.WriteException(ex);
                        }
                    }
                }
            });
        }


        private void UpdateStatuses()
        {
            bool auxHasFunds = (_user.AuxWalletAmount >= MinAuxWalletBTC);
            _labelAuxFunds.Text = (_user.AuxWalletAmount * 1000).ToString("N2") + " mBTC";
            if (!auxHasFunds)
            {
                _labelAuxFunds.Text += " - fund the wallet with at least " + (RecomendedAuxWalletBTC * 1000).ToString("N1") + " mBTC";
            }

            if (auxHasFunds)
            {
                _labelAuxWalletStatus.Text = "Funded";
                _labelAuxWalletStatus.ForeColor = System.Drawing.Color.Green;
                _labelAuxFunds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
            else
            {
                _labelAuxWalletStatus.Text = "Needs To Be Funded";
                _labelAuxWalletStatus.ForeColor = System.Drawing.Color.Brown;
                _labelWithdrawalStatus.Text = "Not Started Yet";
                _labelAuxFunds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
        }


        private void UpdateWithdrawalControls()
        {
            _accountService.DetectChanges();

            bool auxHasFunds = (_user.AuxWalletAmount >= 0.0001M);
            bool isCurrentSelected = (tabControlWithdrawal.SelectedTab.Name == "Current");
            var address = isCurrentSelected ? _curAddress : _prevAddress;
            var disbanding = (address == null) ? null : address.Teammate.CurDisbanding;
            var utxoNum = 0;
            if (disbanding != null)
            {
                utxoNum = isCurrentSelected ? disbanding.UtxoCurAddrNum.Value : disbanding.UtxoPrevAddrNum.Value;
            }

            if (_curAddress != null)
            {
                tabControlWithdrawal.TabPages["Current"].Text = "Your Current Wallet: " + (_amountSatoshisCurent / BlockchainService.SatoshisInBTC * 1000).ToString("N2") + " mBTC";
            }
            if (_prevAddress != null)
            {
                tabControlWithdrawal.TabPages["Previous"].Text = "Your Previous Wallet: " + (_amountSatoshisPrevious / BlockchainService.SatoshisInBTC * 1000).ToString("N2") + " mBTC";
            }

            _textBoxAddressFrom.Text = address != null ? address.Address : "n/a";
            var amountBTC = 0M;
            if (disbanding != null)
            {
                amountBTC = isCurrentSelected ? disbanding.CurAddrBTCAmount ?? 0 : disbanding.PrevAddrBTCAmount ?? 0;
            }
            _labelFunds.Text = (amountBTC >= 0 && address != null) ? (amountBTC / 1000).ToString("N2") + " mBTC" : "-";
            bool hasFundsToWithdraw = (amountBTC / BlockchainService.SatoshisInBTC >= BlockchainService.MinWithdrawInputBTC);

            _labelWithdrawalStatus.ForeColor = System.Drawing.Color.DimGray;


            if (auxHasFunds)
            {
                if (address != null && disbanding != null)
                {
                    var needSignatures = (address.Cosigners.Count + 1) / 2;

                    var withdrawProcessed =
                        address.Teammate.Team.DisbandState == (isCurrentSelected ? DisbandState.ProcessedCur : DisbandState.ProcessedPrev)
                        || address.Teammate.Team.DisbandState == DisbandState.ProcessedAll;

                    if (withdrawProcessed)
                    {
                        _labelWithdrawalStatus.ForeColor = System.Drawing.Color.Green;
                        _labelWithdrawalStatus.Text = "Successful";
                    }
                    else if (!hasFundsToWithdraw)
                    {
                        _labelWithdrawalStatus.Text = "No Funds To Withdraw";
                    }
                    else
                    {
                        _labelWithdrawalStatus.Text = "Waiting For Cosignatures ("
                            + Math.Min(needSignatures, address.Cosigners.Count(x =>
                                x.Teammate.CurDisbanding != null
                                && x.DisbandingTxSignatures.Count == utxoNum
                              ))
                            + "/" + needSignatures + ")";
                    }
                }
                else
                {
                    _labelWithdrawalStatus.Text = "n/a";
                }
            }

            _dataGridViewCosigners.Rows.Clear();
            if (address != null && disbanding != null)
            {
                foreach (var cosigner in address.Cosigners)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(_dataGridViewCosigners);
                    _dataGridViewCosigners.Rows.Insert(0, row);
                    row = _dataGridViewCosigners.Rows[0];
                    row.Cells[ColCosigner].Value = cosigner.Teammate.Name;
                    row.Cells[ColCosigner].Tag = cosigner.Teammate.FBName;
                    if (!hasFundsToWithdraw)
                    {
                        row.Cells[ColStatus].Value = "n/a";
                        row.Cells[ColStatus].Style.ForeColor = System.Drawing.Color.Gray;
                    }
                    else if (cosigner.Teammate.CurDisbanding == null)
                    {
                        row.Cells[ColStatus].Value = "Not Disbanding Yet";
                        row.Cells[ColStatus].Style.ForeColor = System.Drawing.Color.Gray;
                    }
                    else if (cosigner.DisbandingTxSignatures.Count < utxoNum)
                    {
                        row.Cells[ColStatus].Value = "Waiting for Cosignature";
                    }
                    else
                    {
                        row.Cells[ColStatus].Value = "Cosigned";
                    }
                }
            }
        }

        private void UpdateCosignerOfControls()
        {
            _accountService.DetectChanges();

            foreach (var cosigner in _me.CosignerOf)
            {
                var row = new DataGridViewRow();
                row.CreateCells(_dataGridViewCosignerOf);
                _dataGridViewCosignerOf.Rows.Insert(0, row);
                row = _dataGridViewCosignerOf.Rows[0];
                Teammate owner = cosigner.Address.Teammate;
                row.Cells[ColOwner].Value = owner.Name;
                row.Cells[ColOwner].Tag = owner.FBName;
                if (owner.CurDisbanding == null)
                {
                    row.Cells[ColOwnerStatus].Value = "Not Disbanding Yet";
                    row.Cells[ColOwnerStatus].Style.ForeColor = System.Drawing.Color.Gray;
                }
                else if (owner.CurDisbanding.SignatureDate == null)
                {
                    row.Cells[ColOwnerStatus].Value = "Requested Cosignature";
                }
                else
                {
                    row.Cells[ColOwnerStatus].Value = "Cosigned";
                }
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_textBoxAddressFrom.Text);
            }
            catch (Exception) { }
        }

        private void buttonAuxCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_textBoxAuxAddress.Text);
            }
            catch (Exception) { }
        }

        private void buttonCopyTo_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_textBoxAddressTo.Text);
            }
            catch (Exception) { }
        }

        private void tabControlWithdrawal_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWithdrawalControls();
        }

        private void _dataGridViewCosigners_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var tag = _dataGridViewCosigners[e.ColumnIndex, e.RowIndex].Tag;
            if (tag != null)
            {
                System.Diagnostics.Process.Start("https://facebook.com/" + (string)tag);
            }
        }

        private void _dataGridViewCosignerOf_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var tag = _dataGridViewCosignerOf[e.ColumnIndex, e.RowIndex].Tag;
            if (tag != null)
            {
                System.Diagnostics.Process.Start("https://facebook.com/" + (string)tag);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            _closing = true;
            if (_accountService != null && !_accountService.Closed)
                _accountService.Close();
            Close();
        }
    }
}
