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
using System.Windows.Forms;
using Teambrella.Client.DomainModel;
using Teambrella.Client.Services;

namespace Teambrella.Client.App
{
    public partial class SettingsForm : Form
    {
        private AccountService _accountService;

        public SettingsForm()
        {
            _accountService = new AccountService();
            InitializeComponent();

            _listBoxTeams.DataSource = _accountService.GetAllTeams()
                .Where(x => x.DisbandState != DisbandState.Invalid_NotMyTeam)
                .OrderBy(x => x.Testnet ? 1 : 0)
                .ThenBy(x => x.Name)
                .ToList();
            _listBoxTeams.DisplayMember = "DisplayName";

            _labelPubKey.Text = _accountService.GetUser().BitcoinPrivateKey.PubKey.ToString();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_accountService != null && !_accountService.Closed)
                _accountService.Close();
            Close();
        }

        private void listBoxTeams_SelectedValueChanged(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }

            _comboBoxPayToAddressOkAge.SelectedIndex = team.PayToAddressOkAge / 2 - 1;

            // -1 is for "Do not auto-approve"
            // 0 is for "Right away"
            _comboBoxAutoApprovalMyGoodAddress.SelectedIndex = team.AutoApprovalMyGoodAddress + 1;
            _comboBoxAutoApprovalMyNewAddress.SelectedIndex = team.AutoApprovalMyNewAddress + 1;
            _comboBoxAutoApprovalCosignGoodAddress.SelectedIndex = team.AutoApprovalCosignGoodAddress + 1;
            _comboBoxAutoApprovalCosignNewAddress.SelectedIndex = team.AutoApprovalCosignNewAddress + 1;

            ShowDisbanding(team.DisbandState);
        }

        private void comboBoxPayToAddressOkAge_SelectedValueChanged(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }
            team.PayToAddressOkAge = (_comboBoxPayToAddressOkAge.SelectedIndex + 1) * 2;
            _accountService.SaveChanges();
        }

        private void comboBoxAutoApprovalMyGoodAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }
            team.AutoApprovalMyGoodAddress = _comboBoxAutoApprovalMyGoodAddress.SelectedIndex - 1;
            _accountService.SaveChanges();
        }

        private void comboBoxAutoApprovalMyNewAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }
            team.AutoApprovalMyNewAddress = _comboBoxAutoApprovalMyNewAddress.SelectedIndex - 1;
            _accountService.SaveChanges();
        }

        private void comboBoxAutoApprovalCosignGoodAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }
            team.AutoApprovalCosignGoodAddress = _comboBoxAutoApprovalCosignGoodAddress.SelectedIndex - 1;
            _accountService.SaveChanges();
        }

        private void comboBoxAutoApprovalCosignNewAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }
            team.AutoApprovalCosignNewAddress = _comboBoxAutoApprovalCosignNewAddress.SelectedIndex - 1;
            _accountService.SaveChanges();
        }

        private void buttonDisband_Click(object sender, EventArgs e)
        {
            var team = (Team)_listBoxTeams.SelectedValue;
            if (team == null)
            {
                return;
            }

            if (team.DisbandState == DisbandState.Invalid_NoAddress)
            {
                return; // actually we can't get here, right?
            }

            Teammate me = team.GetMe(_accountService.GetUser());
            if (team.DisbandState == DisbandState.Normal || team.DisbandState == DisbandState.CosignerInitiated)
            {
                var disbandAddressForm = new DisbandAddressForm(me.Team.Network);
                var result = disbandAddressForm.ShowDialog();

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                var disbanding = new Disbanding
                {
                    Teammate = me,
                    RequestDate = DateTime.UtcNow,
                    WithdrawAddr = disbandAddressForm.GetAddress(),
                    UtxoCurAddrNum = -1,
                    UtxoPrevAddrNum = -1
                };
                _accountService.AddDisbanding(disbanding);
                team.DisbandState = DisbandState.Initiated;
                _accountService.UpdateTeam(team);
                _accountService.SaveChanges();
                ShowDisbanding(team.DisbandState);
                // ask blockchain service to handle current status of disbanding
            }

            // show dialog
            var disbandForm = new DisbandForm(me.Id);
            disbandForm.ShowDialog();
        }

        private void ShowDisbanding(DisbandState disbandState)
        {
            _accountService.DetectChanges();

            _buttonDisband.Enabled = true;
            if (disbandState == DisbandState.Normal)
            {
                _panelStatus.BackColor = System.Drawing.Color.LightGray;
                _labelStatusValue.ForeColor = System.Drawing.Color.Green;
                _labelStatusValue.Text = "Normal";
                _buttonDisband.Text = "&Disband...";
            }
            else if (disbandState == DisbandState.CosignerInitiated)
            {
                _panelStatus.BackColor = System.Drawing.Color.AntiqueWhite;
                _labelStatusValue.ForeColor = System.Drawing.Color.SaddleBrown;
                _labelStatusValue.Text = "Disdband Request Received";
                _buttonDisband.Text = "&Disband...";
            }
            else if (disbandState == DisbandState.Invalid_NoAddress)
            {
                _panelStatus.BackColor = System.Drawing.Color.AntiqueWhite;
                _labelStatusValue.ForeColor = System.Drawing.Color.SaddleBrown;
                _labelStatusValue.Text = "No Wallet Created Yet";
                _buttonDisband.Text = "&Disband...";
                _buttonDisband.Enabled = false;
            }
            else
            {
                _panelStatus.BackColor = System.Drawing.Color.AntiqueWhite;
                _labelStatusValue.ForeColor = System.Drawing.Color.SaddleBrown;
                _labelStatusValue.Text = "Disbanding";
                _buttonDisband.Text = "&Details...";
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_labelPubKey.Text);
            }
            catch (Exception) { }
        }
    }
}
