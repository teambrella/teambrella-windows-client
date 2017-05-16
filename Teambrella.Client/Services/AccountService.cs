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
using System.Collections.Generic;
using System.Linq;
using NBitcoin;
using Teambrella.Client.Dal;
using Teambrella.Client.DomainModel;
using Teambrella.Client.Repositories;
using Teambrella.Client.ServerApiModels;
using Teambrella.Client.Signing;
using System.Threading;


/*
    TX processing:
    1. Tx & TxOutputs (w/o change) are obtained from the server
    2. User approves the Tx or N days pass
    3. Client notifies server on approval
    4. TxInputs and a TxOutput for change are obtained from the server
    5. Tx is signed/co-signed
*/

namespace Teambrella.Client.Services
{
    public class AccountService : IDisposable
    {
        private const decimal NoAutoApproval = 1000000;

        private UserRepository _userRepo;
        private ConnectionRepository _connectionRepo;
        private TeamRepository _teamRepo;
        private TeammateRepository _teammateRepo;
        private BTCAddressRepository _addressRepo;
        private CosignerRepository _cosignerRepo;
        private DisbandingRepository _disbandingRepo;
        private DisbandingTxSignatureRepository _disbandingTxSignatureRepo;
        private PayToRepository _payToRepo;
        private TxRepository _txRepo;

        private TeambrellaContext _context;

        private Server _server;

        private Connection _connection;

        public bool Closed { get; set; }

        public AccountService()
        {
            _server = new Server();

            _context = new TeambrellaContext();
            Closed = false;

            _userRepo = new UserRepository(_context);
            _connectionRepo = new ConnectionRepository(_context);
            _teamRepo = new TeamRepository(_context);
            _teammateRepo = new TeammateRepository(_context);
            _addressRepo = new BTCAddressRepository(_context);
            _cosignerRepo = new CosignerRepository(_context);
            _disbandingRepo = new DisbandingRepository(_context);
            _disbandingTxSignatureRepo = new DisbandingTxSignatureRepository(_context);
            _payToRepo = new PayToRepository(_context);
            _txRepo = new TxRepository(_context);
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Closed = true;
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void DetectChanges()
        {
            _context.ChangeTracker.DetectChanges();
        }

        public void UpdateUser(User user)
        {
            _userRepo.Update(user);
        }

        public User GetUser()
        {
            User user = _userRepo.GetUser();
            if (null == user)
            {
                user = CreateUser();
            }

            return user;
        }

        private User CreateUser()
        {
            Mutex m = new Mutex(false, "Teambrella_CreateUser_Mutex");
            try
            {
                m.WaitOne();

                _context.ChangeTracker.DetectChanges();

                // Double check pattern (second check is always inside mutex)
                User user = _userRepo.GetUser();
                if (user == null)
                {
                    var key = new Key();
                    user = new User
                    {
                        PrivateKey = key.GetBitcoinSecret(Network.Main).ToString()
                    };

                    user = _userRepo.Create(user);
                    _context.SaveChanges();

                }

                return user;
            }
            finally
            {
                m.ReleaseMutex();
            }
        }

        public Teammate GetTeammate(int teammateId)
        {
            return _teammateRepo.Get(teammateId);
        }

        public LoginQuery GetLoginQueryObject()
        {
            _server.TryInitTimestamp();
            return _server.CreateBrowseOpenModel(GetUser().BitcoinPrivateKey);
        }

        public void UpdateTeam(Team team)
        {
            _teamRepo.Update(team);
        }

        public List<Team> GetAllTeams()
        {
            return _teamRepo.GetAll();
        }

        public void UpdateDisbanding(Disbanding disbanding)
        {
            _disbandingRepo.Update(disbanding);
        }

        public Disbanding AddDisbanding(Disbanding disbanding)
        {
            return _disbandingRepo.Add(disbanding);
        }

        public DisbandingTxSignature GetDisbandingTxSignature(Cosigner cosigner, int input)
        {
            return _disbandingTxSignatureRepo.Get(cosigner, input);
        }

        public DisbandingTxSignature AddDisbandingTxSignature(DisbandingTxSignature signature)
        {
            return _disbandingTxSignatureRepo.Add(signature);
        }

        public TxSignature GetSignature(Guid input, int teammateId)
        {
            return _txRepo.GetSignature(input, teammateId);
        }

        public TxSignature AddSignature(TxSignature signature)
        {
            return _txRepo.AddSignature(signature);
        }

        public List<Tx> GetCoSignableTxs()
        {
            return _txRepo.GetCoSignable();
        }

        public List<Tx> GetApprovedAndCosignedTxs(User user)
        {
            string key = user.PrivateKey;
            if (string.IsNullOrEmpty(key))
                return new List<Tx>(0);

            string pubkey = new BitcoinSecret(user.PrivateKey).PubKey.ToString();
            return _txRepo.GetApprovedAndCosigned(pubkey);
        }

        public List<Tx> GetChanged(DateTime time)
        {
            return _txRepo.GetChanged(time);
        }

        public void UpdateTx(Tx tx)
        {
            _txRepo.Update(tx);
        }

        public Tx GetTx(Guid txId)
        {
            return _txRepo.Get(txId);
        }

        public bool IsMyTx(int txTeammateId)
        {
            return GetMyTeammateAccounts().Any(x => x.Id == txTeammateId);
        }

        public void ChangeTxResolution(Tx tx, TxClientResolution resolution)
        {
            tx.Resolution = resolution;
            tx.ClientResolutionTime = DateTime.UtcNow;
            tx.NeedUpdateServer = true;
            _txRepo.Update(tx);
        }

        public bool UpdateData()
        {
            try
            {
                if (_connection == null)
                {
                    if (!ServerInitToConnectionRepo())
                    {
                        return false;
                    }
                }

                AutoApproveTxs();

                if (!ServerUpdatesToLocalDb())
                {
                    return false;
                }
                UpdateAddresses();

                return true;
            }
            catch(Exception ex)
            {
                Logger.WriteException(ex);
                ForceServerUpdatesReload();

                return false;
            }
        }

        public void ForceServerUpdatesReload()
        {
            try
            {
                // corrupted state. Refresh all updates:
                var con = _connection ?? _connectionRepo.GetConnection();

                if (con != null)
                {
                    con.LastUpdated = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    _connectionRepo.Update(con);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "Cannot reset Last Updated server date.");
            }
        }

        private bool ServerInitToConnectionRepo()
        {
            ApiResult serverResult = _server.InitClient(GetUser().BitcoinPrivateKey);
            if (serverResult != null)
            {
                _connection = _connectionRepo.GetConnection();
                if (_connection == null)
                {
                    _connection = new Connection
                    {
                        LastConnected = DateTime.UtcNow,
                        NeedShowBrowser = true
                    };

                    _connection = _connectionRepo.Create(_connection);
                }
                else
                {
                    _connection.LastConnected = DateTime.UtcNow;
                    _connectionRepo.Update(_connection);
                }

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        private void AutoApproveTxs()
        {
            var txs = _txRepo.GetResolvable();
            foreach (var tx in txs)
            {
                var daysLeft = DaysToApproval(tx, IsMyTx(tx.Teammate.Id));
                if (daysLeft <= 0)
                {
                    tx.Resolution = TxClientResolution.Approved;
                    tx.NeedUpdateServer = true;
                    _txRepo.Update(tx);
                }
            }
        }

        private void UpdateAddresses()
        {
            foreach (var teammate in _teammateRepo.GetAll())
            {
                if (teammate.Addresses == null)
                {
                    continue;
                }

                if (teammate.BtcAddressCurrent == null)
                {
                    var curServerAddr = teammate.Addresses.FirstOrDefault(x => x.Status == UserAddressStatus.ServerCurrent);
                    if (curServerAddr != null)
                    {
                        curServerAddr.Status = UserAddressStatus.Current;
                        _addressRepo.Update(curServerAddr);
                    }
                }
            }

            // Check teams
            foreach (var team in _teamRepo.GetAll())
            {
                var teammateMe = team.GetMe(GetUser());
                if (teammateMe == null)
                {
                    if (team.DisbandState != DisbandState.Invalid_NotMyTeam)
                    {
                        team.DisbandState = DisbandState.Invalid_NotMyTeam;
                        _teamRepo.Update(team);
                    }
                    continue;
                }

                if (teammateMe.BtcAddressCurrent == null && team.DisbandState != DisbandState.Invalid_NoAddress)
                {
                    team.DisbandState = DisbandState.Invalid_NoAddress;
                    _teamRepo.Update(team);
                }
                if (teammateMe.BtcAddressCurrent != null && team.DisbandState == DisbandState.Invalid_NoAddress)
                {
                    team.DisbandState = DisbandState.Normal;
                    _teamRepo.Update(team);
                }
            }
            _context.SaveChanges();
        }


        private bool ServerUpdatesToLocalDb()
        {
            Key key = GetUser().BitcoinPrivateKey;
            long since = _connection.LastUpdated.Ticks;
            var txsToUpdate = _txRepo.GetTxsNeedServerUpdate();
            var txSignatures = _txRepo.GetTxsSignaturesNeedServerUpdate();

            GetUpdatesResultData serverResponse = _server.GetUpdates(key, since, txsToUpdate.Where(x => x.Teammate.Team.IsInNormalState), txSignatures);
            if (null == serverResponse)
            {
                // todo: log record
                return false;
            }

            txsToUpdate.ForEach(x =>
            {
                x.NeedUpdateServer = false;
                _txRepo.Update(x);
            });
            txSignatures.ForEach(x =>
            {
                x.NeedUpdateServer = false;
                _txRepo.Update(x);
            });

            serverResponse.Teams.ForEach(team =>
            {
                var existingTeam = _teamRepo.Get(team.Id);
                if (existingTeam == null)
                {
                    team.PayToAddressOkAge = 14;
                    team.AutoApprovalMyGoodAddress = 3;
                    team.AutoApprovalMyNewAddress = 7;
                    team.AutoApprovalCosignGoodAddress = 3;
                    team.AutoApprovalCosignNewAddress = 7;
                    existingTeam = _teamRepo.Add(team);
                }
                else
                {
                    // Can change Name
                    if (existingTeam.Name != team.Name)
                    {
                        existingTeam.Name = team.Name;
                        _teamRepo.Update(existingTeam);
                    }
                }
            });

            serverResponse.Teammates.ForEach(teammate =>
            {
                var existingTeammate = _teammateRepo.Get(teammate.Id);

                bool okToInsertOrUpdate = true;
                if (existingTeammate != null)
                {
                    if (existingTeammate.PublicKey != null && teammate.PublicKey != existingTeammate.PublicKey
                        || teammate.FBName != existingTeammate.FBName
                        || teammate.TeamId != existingTeammate.TeamId)
                    {
                        okToInsertOrUpdate = false; // we don't allow to change basic info
                    }
                }
                if (okToInsertOrUpdate)
                {
                    _teammateRepo.InsertOrUpdateDetached(teammate);
                }
            });

            serverResponse.PayTos.ForEach(payTo =>
            {
                var existingPayTo = _payToRepo.Get(payTo.TeammateId, payTo.Address);
                if (existingPayTo == null)
                {
                    payTo.KnownSince = DateTime.UtcNow;
                    existingPayTo = _payToRepo.Add(payTo);
                }
                if (existingPayTo.IsDefault)
                {
                    _payToRepo.GetForTeammate(existingPayTo.TeammateId).ForEach(x =>
                    {
                        if (x.Address != existingPayTo.Address)
                        {
                            x.IsDefault = false;
                            _payToRepo.Update(x);
                        }
                    });
                }
            });

            serverResponse.BTCAddresses.ForEach(address =>
            {
                if (address.Address != null && _addressRepo.Get(address.Address) == null)
                {
                    if (address.Status == UserAddressStatus.Previous)
                    {
                        address.Status = UserAddressStatus.ServerPrevious;
                    }
                    else if (address.Status == UserAddressStatus.Current)
                    {
                        address.Status = UserAddressStatus.ServerCurrent;
                    }
                    else if (address.Status == UserAddressStatus.Next)
                    {
                        address.Status = UserAddressStatus.ServerNext;
                    }
                    _addressRepo.Add(address);
                }
            });

            serverResponse.Cosigners.ForEach(cosigner =>
            {
                if (cosigner.AddressId != null)
                {
                    if (_addressRepo.Get(cosigner.AddressId) == null) // can't add cosigners to existing addresses
                    {
                        _cosignerRepo.Add(cosigner);
                    }
                }
            });

            // Rules for setting new current address
            // ok to set first address
            // ok to change to next address if:
            // -- no funds on existing current address
            // -- or a real Tx from current to next occurred

            serverResponse.Txs.ForEach(tx =>
            {
                var existingTx = _txRepo.Get(tx.Id);
                if (existingTx == null)
                {
                    tx.ReceivedTime = DateTime.UtcNow;
                    tx.UpdateTime = DateTime.UtcNow;
                    tx.Resolution = TxClientResolution.None;
                    tx.ClientResolutionTime = null;
                    tx.NeedUpdateServer = false;
                    _txRepo.Add(tx);
                }
                else
                {
                    existingTx.State = tx.State;
                    existingTx.UpdateTime = DateTime.UtcNow;
                    existingTx.ResolutionTime = tx.ResolutionTime;
                    existingTx.ProcessedTime = tx.ProcessedTime;
                    _txRepo.Update(existingTx);
                }
            });

            foreach (var txInput in serverResponse.TxInputs)
            {
                if (_txRepo.GetInput(txInput.Id) != null)
                {
                    continue; // can't change inputs
                }

                var tx = _txRepo.Get(txInput.TxId);
                if (tx == null)
                {
                    tx = serverResponse.Txs.FirstOrDefault(x => x.Id == txInput.TxId);
                }
                if (tx == null)
                {
                    continue; // malformed TX, todo: add log record
                }
                _txRepo.AddInput(txInput);
            }

            foreach (var txOutput in serverResponse.TxOutputs)
            {
                var tx = _txRepo.Get(txOutput.TxId);
                if (tx != null)
                {
                    continue; // can't add outputs to existing txs
                }
                tx = serverResponse.Txs.FirstOrDefault(x => x.Id == txOutput.TxId);
                if (tx == null)
                {
                    continue; // malformed TX, todo: add log record
                }
                _txRepo.AddOutput(txOutput);
            }

            foreach (var txSignatureInfo in serverResponse.TxSignatures)
            {
                if (_txRepo.GetSignature(txSignatureInfo.TxInputId, txSignatureInfo.TeammateId) != null)
                {
                    continue; // can't change signatures
                }

                var txInput = _txRepo.GetInput(txSignatureInfo.TxInputId);
                if (txInput == null)
                {
                    txInput = serverResponse.TxInputs.FirstOrDefault(x => x.Id == txSignatureInfo.TxInputId);
                }
                if (txInput == null)
                {
                    continue; // malformed TX, todo: add log record
                }
                var txSignature = new TxSignature()
                {
                    TxInputId = txSignatureInfo.TxInputId,
                    TeammateId = txSignatureInfo.TeammateId,
                    Signature = Convert.FromBase64String(txSignatureInfo.Signature),
                    NeedUpdateServer = false
                };
                _txRepo.AddSignature(txSignature);
            }

            _context.SaveChanges();

            // Check outputs
            foreach (var arrivingTx in serverResponse.Txs)
            {
                var tx = _txRepo.Get(arrivingTx.Id);
                bool isWalletMove = (tx.Kind == TxKind.MoveToNextWallet || tx.Kind == TxKind.SaveFromPrevWallet);

                // Outputs are required unless it's a wallet update
                if (!isWalletMove && tx.Outputs == null)
                {
                    ChangeTxResolution(tx, TxClientResolution.ErrorBadRequest);
                    continue;
                }

                // AmountBTC sum must match total unless it's a wallet update
                if (!isWalletMove && Math.Abs(tx.Outputs.Sum(x => x.AmountBTC) - tx.AmountBTC.Value) > 0.000001M)
                {
                    ChangeTxResolution(tx, TxClientResolution.ErrorBadRequest);
                    continue;
                }

                if (tx.Resolution == TxClientResolution.None)
                {
                    ChangeTxResolution(tx, TxClientResolution.Received);
                }
            }

            _context.SaveChanges();

            // Check addresses
            serverResponse.BTCAddresses.ForEach(address =>
            {
                var addressSaved = _addressRepo.Get(address.Address);
                if (addressSaved != null)
                {
                    if (SignHelper.GenerateStringAddress(addressSaved) != addressSaved.Address)
                    {
                        addressSaved.Status = UserAddressStatus.Invalid;
                        _addressRepo.Update(addressSaved);
                    }
                }
            });

            // process inputs

            // verify amounts if inputs are set

            _connection.LastUpdated = new DateTime(serverResponse.LastUpdated, DateTimeKind.Utc);
            _connectionRepo.Update(_connection);

            _context.SaveChanges();

            return true;
        }

        public BitcoinAddress GetAuxWallet(Network network)
        {
            return GetUser().BitcoinPrivateKey.GetBitcoinSecret(network).GetAddress();
        }


        public bool IsInChangableState(Tx tx)
        {
            if (tx.State == TxState.ErrorBadRequest
                || tx.State == TxState.ErrorCosignersTimeout
                || tx.State == TxState.ErrorOutOfFunds
                || tx.State == TxState.ErrorSubmitToBlockchain
                || tx.State == TxState.Published
                || tx.State == TxState.Confirmed)
            {
                return false;
            }
            return true;
        }

        public bool CanApproveTx(Tx tx)
        {
            if (!tx.Teammate.Team.IsInNormalState)
            {
                return false;
            }
            if (tx.Resolution == TxClientResolution.Received && IsInChangableState(tx))
            {
                return true;
            }
            return false;
        }

        public bool CanBlockTx(Tx tx)
        {
            if (!tx.Teammate.Team.IsInNormalState)
            {
                return false;
            }
            if (tx.Resolution == TxClientResolution.Received && IsInChangableState(tx))
            {
                return true;
            }
            return false;
        }

        public bool CanUnblockTx(Tx tx)
        {
            if (!tx.Teammate.Team.IsInNormalState)
            {
                return false;
            }
            if (tx.Resolution == TxClientResolution.Blocked && IsInChangableState(tx))
            {
                return true;
            }
            return false;
        }

        public decimal DaysToApproval(Tx tx, bool isMyTx)
        {
            decimal daysToApproval;
            if (!tx.Teammate.Team.IsInNormalState)
            {
                return NoAutoApproval;
            }

            bool goodPayToAddresses = true;
            foreach (var txOutput in tx.Outputs)
            {
                goodPayToAddresses = goodPayToAddresses && IsPayToAddressOkAge(txOutput);
            }
            decimal daysPassed = (decimal)(DateTime.UtcNow - tx.ReceivedTime).TotalDays;
            int autoApproval = goodPayToAddresses ? tx.Teammate.Team.AutoApprovalCosignGoodAddress : tx.Teammate.Team.AutoApprovalCosignNewAddress;
            if (isMyTx)
            {
                autoApproval = goodPayToAddresses ? tx.Teammate.Team.AutoApprovalMyGoodAddress : tx.Teammate.Team.AutoApprovalMyNewAddress;
            }
            if (autoApproval == -1)
            { // no auto-approval 
                return NoAutoApproval;
            }
            daysToApproval = (autoApproval - daysPassed);
            return daysToApproval;
        }

        public string GetTxStatusText(Tx tx, bool isMyTx)
        {
            decimal daysToApproval = DaysToApproval(tx, isMyTx);
            if (tx.Teammate.Team.DisbandState == DisbandState.Invalid_NoAddress)
            {
                return "Error: You don't have a valid wallet in this team";
            }
            if (tx.Teammate.Team.DisbandState == DisbandState.Invalid_NotMyTeam)
            {
                return "Error: You're not a member of the team";
            }
            if (!tx.Teammate.Team.IsInNormalState)
            {
                return "Team is disbanding. The transaction won't be processed.";
            }
            var timeToApprovalText = daysToApproval.ToString("N0");
            timeToApprovalText += (timeToApprovalText == "1") ? " day" : " days";
            if (daysToApproval < 1)
            {
                decimal hoursLeft = daysToApproval * 24;
                if (hoursLeft < 1)
                {
                    timeToApprovalText = " < 1 hour";
                }
                else
                {
                    timeToApprovalText = hoursLeft.ToString("N0");
                    timeToApprovalText += (timeToApprovalText == "1") ? " hour" : " hours";
                }
            }

            var resDate = tx.ResolutionTime == null ? "" : "  (" + tx.ResolutionTime.Value.ToShortDateString() + ")";
            var clientResDate = tx.ClientResolutionTime == null ? "" : "  (" + tx.ClientResolutionTime.Value.ToShortDateString() + ")";
            var processedDate = tx.ProcessedTime == null ? "" : "  (" + tx.ProcessedTime.Value.ToShortDateString() + ")";


            // 1.  Blocking
            if (tx.State == TxState.BlockedMaster)
            {
                if (isMyTx)
                {
                    return "Blocked by You" + clientResDate;
                }
                else
                {
                    return "Blocked by " + tx.Teammate.Name + resDate;
                }
            }
            if (tx.State == TxState.BlockedCosigners)
            {
                return "Declined by Cosigners" + resDate;
            }

            if (tx.Resolution == TxClientResolution.Blocked)
            {
                if (isMyTx)
                {
                    return "Blocked by You" + clientResDate;
                }
                else if (tx.State == TxState.Confirmed)
                {
                    return "Confirmed" + processedDate + " / Declined by You " + clientResDate;
                }
                else
                {
                    return "Declined by You" + clientResDate;
                }
            }

            // 2. Errors
            if (tx.State == TxState.ErrorBadRequest || tx.Resolution == TxClientResolution.ErrorBadRequest)
            {
                return "Error: Malformed request from server";
            }
            if (tx.State == TxState.ErrorOutOfFunds || tx.Resolution == TxClientResolution.ErrorOutOfFunds)
            {
                return "Error: Out of funds";
            }
            if (tx.State == TxState.ErrorSubmitToBlockchain || tx.Resolution == TxClientResolution.ErrorSubmitToBlockchain)
            {
                return "Error: Submit to blockchain failed";
            }
            if (tx.State == TxState.ErrorCosignersTimeout || tx.Resolution == TxClientResolution.ErrorCosignersTimeout)
            {
                return "Error: Not enough active cosigners";
            }

            // 3. Processed
            //    no need to check for approval - we don't ask client to sign tx after this
            if (tx.State == TxState.Confirmed)
            {
                return "Processed (" + ToShortDateString(tx.ProcessedTime) + ")";
            }
            if (tx.Resolution == TxClientResolution.Published)
            {
                var time = isMyTx ? tx.ClientResolutionTime : tx.ProcessedTime;
                return "Published (" + ToShortDateString(time) + ")";
            }


            // 4. Waiting for client approval (even if tx.state == TxState.ApprovedCosigners)
            if (tx.Resolution == TxClientResolution.Received)
            {
                if (isMyTx)
                {
                    if (daysToApproval == NoAutoApproval)
                    {
                        return "Needs Your Approval";
                    }
                    else
                    {
                        return "Auto-Approval in " + timeToApprovalText;
                    }
                }
                else
                {
                    if (daysToApproval == NoAutoApproval)
                    {
                        return "Not Yet Approved";
                    }
                    else
                    {
                        return "Auto-Approval in " + timeToApprovalText;
                    }
                }
            }

            // 5. Got client approval
            if (tx.Resolution == TxClientResolution.Approved || tx.Resolution == TxClientResolution.Signed)
            {
                if (tx.State == TxState.ApprovedAll)
                {
                    return "Fully Approved";
                }
                if (tx.State == TxState.SelectedForCosigning)
                {
                    return "Cosigning...";
                }
                if (tx.State == TxState.Cosigned || tx.State == TxState.Published)
                {
                    return "Publishing...";
                }
                if (isMyTx)
                {
                    return "Approved by You / Waiting for Cosigners";
                }
                else
                {
                    return "Approved by You";
                }
            }
            return null;
        }

        public bool IsPayToAddressOkAge(TxOutput output)
        {
            return output.Tx.Teammate.Team.PayToAddressOkAge <= (DateTime.UtcNow - output.PayTo.KnownSince).TotalDays;
        }

        public List<Teammate> GetMyTeammateAccounts()
        {
            return _teammateRepo.GetWithPubKey(GetUser().BitcoinPrivateKey.PubKey);
        }

        private static string ToShortDateString(DateTime? d)
        {
            return d.HasValue
                ? d.Value.ToShortDateString()
                : "-";
        }
    }
}
