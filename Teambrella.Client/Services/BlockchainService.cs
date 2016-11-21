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
using Newtonsoft.Json;
using NBitcoin;
using NBitcoin.DataEncoders;
using Teambrella.Client.DomainModel;
using Teambrella.Client.ServerApiModels;
using Teambrella.Client.Signing;

namespace Teambrella.Client.Services
{
    public class BlockchainService
    {
        private const byte OpCodeDisband = 0x01;
        private const byte OpCodeSignatureCur = 0x02;
        private const byte OpCodeSignaturePrev = 0x03;

        public const decimal MinWithdrawInputBTC = 0.001M;
        public const decimal NormalFeeBTC = 0.0001M;
        public const int TopUtxosNum = 10;
        public const int SatoshisInBTC = 100000000;

        private const long TestingBlocktime = 1445350680;

        private List<string> _testNetServers = new List<string> { "https://test-insight.bitpay.com", "https://testnet.blockexplorer.com"};
        private List<string> _mainNetServers = new List<string> { "https://insight.bitpay.com", "https://blockexplorer.com", "https://blockchain.info" };


        private AccountService _accountService { get; set; }

        public BlockchainService(AccountService accountService)
        {
            _accountService = accountService;
        }


        private class ExplorerUtxo
        {
            public string address { get; set; }
            public string txid { get; set; }
            public int vout { get; set; }
            public long ts { get; set; }
            public string scriptPubKey { get; set; }
            public decimal amount { get; set; }
            public int confirmation { get; set; }
        }

        private class ExplorerTxRes
        {
            public string txid { get; set; }
        }

        private class ExplorerTxOuter
        {
            public int pagesTotal { get; set; }
            public List<ExplorerTx> txs { get; set; }
        }

        private class ExplorerScriptPubKey
        {
            public string asm { get; set; }
            public string hex { get; set; }
            public string type { get; set; }
        }

        private class ExplorerTxVout
        {
            public ExplorerScriptPubKey scriptPubKey { get; set; }
        }

        private class ExplorerTx
        {
            public string txid { get; set; }
            public long blocktime { get; set; }
            public List<ExplorerTxVout> vout { get; set; }
        }

        private class OpReturnInfo
        {
            public string hex { get; set; }
            public long blocktime { get; set; }
        }

        public decimal FetchBalance(BtcAddress address)
        {
            if (address == null || address.Address == null)
            {
                return -1;
            }

            var webClient = new System.Net.WebClient();
            string reply = null;
            string query = "/api/addr/" + address.Address + "/balance";
            decimal balance = -1;

            var serverList = (address.Teammate.Team.Testnet ? _testNetServers : _mainNetServers);
            foreach (var server in serverList)
            {
                try
                {
                    reply = webClient.DownloadString(server + query);
                }
                catch (Exception ex)
                {
                    Logger.WriteException(ex, "Couldn't get balance from server.");
                    reply = null;
                }
                if (reply != null)
                {
                    try
                    {
                        balance = decimal.Parse(reply);
                    }
                    catch (Exception)
                    {
                        Logger.WriteFormatMessage("Incorrect balance '{0}'", balance);
                        balance = -1;
                    }
                }
                if (balance >= 0)
                {
                    break;
                }
            }

            return balance;
        }

        private List<ExplorerUtxo> FetchUtxos(BtcAddress address, decimal minAmount)
        {
            if (address == null)
            {
                return new List<ExplorerUtxo>();
            }
            return FetchUtxos(address.Address, minAmount);
        }

        private List<ExplorerUtxo> FetchUtxos(string address, decimal minAmount)
        {
            if (address == null)
            {
                return new List<ExplorerUtxo>();
            }
            var network = Network.GetNetworkFromBase58Data(address);

            var webClient = new System.Net.WebClient();
            string reply = null;
            string query = "/api/addr/" + address + "/utxo";
            List<ExplorerUtxo> utxos = null;

            var serverList = (network == Network.TestNet ? _testNetServers : _mainNetServers);
            foreach (var server in serverList)
            {
                try
                {
                    reply = webClient.DownloadString(server + query);
                }
                catch (Exception ex)
                {
                    Logger.WriteException(ex, "Couldn't get utxo from server.");
                    reply = null;
                }
                if (reply != null)
                {
                    try
                    {
                        utxos = JsonConvert.DeserializeObject<List<ExplorerUtxo>>(reply);
                    }
                    catch (Exception)
                    {
                        Logger.WriteFormatMessage("Incorrect utxo '{0}'", reply);
                        utxos = null;
                    }
                }
                if (utxos != null)
                {
                    break;
                }
            }

            // utox may be null when no interenet connection.
            return utxos == null ? null : utxos.Where(x => x.amount >= minAmount).ToList();
        }

        private List<OpReturnInfo> FetchOpReturns(string address)
        {
            if (address == null)
            {
                return new List<OpReturnInfo>();
            }
            var network = Network.GetNetworkFromBase58Data(address);

            var webClient = new System.Net.WebClient();
            string reply = null;
            string query = "/api/txs/?address=" + address;
            List<ExplorerTx> txs = null;

            var serverList = (network == Network.TestNet ? _testNetServers : _mainNetServers);
            foreach (var server in serverList)
            {
                try
                {
                    reply = webClient.DownloadString(server + query);
                }
                catch (Exception ex)
                {
                    Logger.WriteException(ex, "Couldn't get OpReturns from server.");
                    reply = null;
                }
                if (reply != null)
                {
                    try
                    {
                        txs = JsonConvert.DeserializeObject<ExplorerTxOuter>(reply).txs;
                    }
                    catch (Exception)
                    {
                        Logger.WriteFormatMessage("Incorrect OpReturns '{0}'", reply);
                        txs = null;
                    }
                }
                if (txs != null)
                {
                    break;
                }
            }

            var opRets = new List<OpReturnInfo>();
            if (txs != null)
            {
                foreach (var tx in txs)
                {
                    foreach (var vout in tx.vout)
                    {
                        if (vout.scriptPubKey.asm.StartsWith("OP_RETURN"))
                        {
                            opRets.Add(new OpReturnInfo
                            {
                                blocktime = tx.blocktime,
                                hex = vout.scriptPubKey.hex
                            });
                        }
                    }
                }
            }
            return opRets.Where(x => x.blocktime > TestingBlocktime).ToList();
        }


        public bool TryFetchAuxWalletUtxos(bool forceUpdate, Network network)
        {
            var user = _accountService.GetUser();
            if (!forceUpdate && user.AuxWalletChecked != null && (DateTime.UtcNow - user.AuxWalletChecked.Value).TotalHours < 1)
            {
                return true;
            }

            var utxos = FetchUtxos(_accountService.GetAuxWallet(network).ToString(), 0.0001M);
            if (null == utxos) return false;

            decimal amount = 0;
            foreach (var utxo in utxos)
            {
                amount += utxo.amount;
            }

            user.AuxWalletAmount = amount;
            user.AuxWalletChecked = DateTime.UtcNow;
            _accountService.UpdateUser(user);
            _accountService.SaveChanges();
            return true;
        }


        private bool PostTx(string tx, Network network)
        {
            if (!PostTxBlockr(tx, network))
            {
                return PostTxExplorer(tx, network);
            }
            return true;
        }

        private bool PostTxBlockr(string tx, Network network)
        {
            var webClient = new System.Net.WebClient();
            string reply = null;
            string server = (network == Network.TestNet ? "http://tbtc.blockr.io" : "http://btc.blockr.io");
            try
            {
                reply = webClient.UploadString(server + "/api/v1/tx/push", "{\"hex\":\"" + tx + "\"}");
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "Couldn't push Tx.");
                reply = null;
            }
            if (reply != null && !reply.Contains("fail"))
            {
                return true;
            }
            Logger.WriteFormatMessage("Couldn't push Tx. Reply: {0}", reply);
            return false;
        }


        private bool PostTxExplorer(string tx, Network network)
        {
            var webClient = new System.Net.WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("Accept", "application/json, text/plain, * / *");
            string reply = null;
            string query = "/api/tx/send";

            var serverList = (network == Network.TestNet ? _testNetServers : _mainNetServers);
            foreach (var server in serverList)
            {
                try
                {
                    reply = webClient.UploadString(server + query, "{\"rawtx\":\"" + tx + "\"}");
                }
                catch (Exception ex)
                {
                    Logger.WriteException(ex, "Couldn't upload rawtx.");
                    reply = null;
                }
                if (reply != null)
                {
                    try
                    {
                        JsonConvert.DeserializeObject<ExplorerTxRes>(reply);
                    }
                    catch (Exception)
                    {
                        Logger.WriteFormatMessage("Couldn't push rawtx. Reply: {0}", reply);
                        continue;
                    }
                    return true;
                }
            }

            return false;
        }


        public bool UpdateData()
        {
            bool done = TryFetchAuxWalletUtxos(false, Network.Main) && TryFetchAuxWalletUtxos(false, Network.TestNet);
            if (!done)
            {
                // no internet connection so no utxo was fetched
                return false;
            }

            // Check for new disbanding requests from cosigners
            // get the op_return with the max cur_utxo, then max prev_utxo
            RestoreMyDisbandingState();
            PostDisbandingRequests();
            CheckDisbandingMessages();
            TryDisbanding();

            CosignApprovedTxs();
            PublishApprovedAndCosignedTxs();

            _accountService.SaveChanges();

            return true;
        }


        private void CosignApprovedTxs()
        {
            var user = _accountService.GetUser();
            var txs = _accountService.GetCoSignableTxs();
            foreach (var tx in txs)
            {
                var blockchainTx = GetTx(tx);
                var redeemScript = SignHelper.GetRedeemScript(tx.FromAddress);
                var txInputs = tx.Inputs.OrderBy(x => x.Id).ToList();
                for (int input = 0; input < txInputs.Count; input++)
                {
                    var txInput = txInputs[input];
                    var signature = SignHelper.Cosign(redeemScript, user.BitcoinPrivateKey, blockchainTx, input);
                    var txSignature = new TxSignature
                    {
                        TxInput = txInput,
                        Teammate = tx.Teammate.Team.GetMe(user),
                        NeedUpdateServer = true,
                        Signature = signature
                    };
                    _accountService.AddSignature(txSignature);
                }
                tx.Resolution = TxClientResolution.Signed;
                _accountService.UpdateTx(tx);
            }
            _accountService.SaveChanges();
        }


        private void PublishApprovedAndCosignedTxs()
        {
            var user = _accountService.GetUser();
            var txs = _accountService.GetApprovedAndCosignedTxs();
            foreach (var tx in txs)
            {
                var blockchainTx = GetTx(tx);
                var redeemScript = SignHelper.GetRedeemScript(tx.FromAddress);
                var txInputs = tx.Inputs.OrderBy(x => x.Id).ToList();

                List<Op>[] ops = new List<Op>[tx.Inputs.Count];
                foreach (var cosigner in tx.FromAddress.Cosigners.OrderBy(x => x.KeyOrder))
                {
                    for (int input = 0; input < txInputs.Count; input++)
                    {
                        var txInput = txInputs[input];
                        var txSignature = _accountService.GetSignature(txInput.Id, cosigner.Teammate.Id);
                        if (txSignature == null)
                        {
                            break;
                        }
                        if (ops[input] == null)
                        {
                            ops[input] = new List<Op>();
                        }
                        if (ops[input].Count == 0)
                        {
                            ops[input].Add(OpcodeType.OP_0);
                        }

                        var vchSig = txSignature.Signature.ToList();
                        vchSig.Add((byte)SigHash.All);
                        ops[input].Add(Op.GetPushOp(vchSig.ToArray()));
                    }
                }

                for (int input = 0; input < txInputs.Count; input++)
                {
                    // add self-signatures
                    var signature = SignHelper.Cosign(redeemScript, user.BitcoinPrivateKey, blockchainTx, input);
                    var vchSig = signature.ToList();
                    var txSignature = new TxSignature
                    {
                        TxInput = txInputs[input],
                        Teammate = tx.Teammate.Team.GetMe(user),
                        NeedUpdateServer = true,
                        Signature = signature
                    };
                    _accountService.AddSignature(txSignature);


                    vchSig.Add((byte)SigHash.All);
                    ops[input].Add(Op.GetPushOp(vchSig.ToArray()));
                    ops[input].Add(Op.GetPushOp(redeemScript.ToBytes()));
                    blockchainTx.Inputs[input].ScriptSig = new Script(ops[input].ToArray());
                }

                string strTx = blockchainTx.ToHex();

                if (PostTx(strTx, tx.Teammate.Team.Network))
                {
                    _accountService.ChangeTxResolution(tx, TxClientResolution.Published);
                }
            }
        }


        private void RestoreMyDisbandingState()
        {
            var user = _accountService.GetUser();
            foreach (var team in _accountService.GetAllTeams())
            {
                if (team.FetchedMyDisbandingStatus)
                {
                    continue;
                }
                if (!team.IsInNormalState)
                {
                    continue;
                }
                Teammate me = team.GetMe(user);
                if (me == null)
                {
                    continue;
                }

                PubKey pubKey = new PubKey(me.PublicKey);
                var opReturns = FetchOpReturns(pubKey.GetAddress(team.Network).ToString())
                    .OrderByDescending(x => x.blocktime).ToList(); // process most recent first
                foreach (var opReturn in opReturns)
                {
                    byte[] data = ParseHex(opReturn.hex);
                    int pos = 1;
                    byte len = data[pos];
                    pos++;
                    if (len == 0x4c) // OP_PUSHDATA1 
                    {
                        len = data[pos];
                        pos++;
                    }
                    byte code = data[pos];
                    pos++;

                    if (code == OpCodeDisband)
                    {
                        ProcessDisbandMessage(me, data, pos);
                        if (me.CurDisbanding != null)
                        {
                            // Restore withdrawal amounts
                            var utxosPrev = FetchUtxos(me.BtcAddressPrevious, MinWithdrawInputBTC)
                                .OrderByDescending(x => x.amount).Take(me.CurDisbanding.UtxoPrevAddrNum ?? 0).ToList();
                            var utxosCur = FetchUtxos(me.BtcAddressCurrent, MinWithdrawInputBTC)
                                .OrderByDescending(x => x.amount).Take(me.CurDisbanding.UtxoCurAddrNum ?? 0).ToList();
                            me.CurDisbanding.CurAddrBTCAmount = utxosCur.Sum(x => x.amount / BlockchainService.SatoshisInBTC);
                            me.CurDisbanding.PrevAddrBTCAmount = utxosPrev.Sum(x => x.amount / BlockchainService.SatoshisInBTC);
                            _accountService.UpdateDisbanding(me.CurDisbanding);
                        }
                        team.DisbandState = DisbandState.NotifiedCosigners;
                        _accountService.UpdateTeam(team);
                        _accountService.SaveChanges();
                        break; // we're done with this team
                    }
                }

                team.FetchedMyDisbandingStatus = true;
            }
        }

        private void PostDisbandingRequests()
        {
            var user = _accountService.GetUser();
            foreach (var team in _accountService.GetAllTeams())
            {
                if (team.DisbandState == DisbandState.Initiated)
                {
                    if (PostDisbandingRequest(team))
                    {
                        team.DisbandState = DisbandState.NotifiedCosigners;
                        _accountService.UpdateTeam(team);
                        user.AuxWalletChecked = null;
                        _accountService.UpdateUser(user);
                    }
                }
            }
        }


        private void CheckDisbandingMessages()
        {
            var user = _accountService.GetUser();

            foreach (var team in _accountService.GetAllTeams())
            {
                Teammate me = team.GetMe(user);
                if (me == null)
                {
                    continue;
                }

                if (me.BtcAddressCurrent != null)
                {
                    foreach (Cosigner cosigner in me.BtcAddressCurrent.Cosigners)
                    {
                        CheckTeammateDisbandingMessages(cosigner.Teammate);
                    }
                }

                if (me.BtcAddressPrevious != null)
                {
                    foreach (Cosigner cosigner in me.BtcAddressPrevious.Cosigners)
                    {
                        CheckTeammateDisbandingMessages(cosigner.Teammate);
                    }
                }

                foreach (Cosigner cosigner in me.CosignerOf)
                {
                    CheckTeammateDisbandingMessages(cosigner.Address.Teammate);
                }
            }
        }


        private void TryDisbanding()
        {
            var user = _accountService.GetUser();
            foreach (var team in _accountService.GetAllTeams())
            {
                var me = team.GetMe(user);
                if (me == null)
                {
                    continue;
                }
                var disbanding = me.CurDisbanding;
                if (disbanding == null)
                {
                    continue;
                }

                if (team.DisbandState != DisbandState.NotifiedCosigners
                    && team.DisbandState != DisbandState.ProcessedCur
                    && team.DisbandState != DisbandState.ProcessedPrev)
                {
                    continue; // not disbanding at all or not ready yet
                }

                if (team.DisbandState != DisbandState.ProcessedPrev)
                {
                    if (TryDisbandingTeam(disbanding, false))
                    {
                        team.DisbandState = (team.DisbandState == DisbandState.ProcessedCur) ? DisbandState.ProcessedAll : DisbandState.ProcessedPrev;
                        _accountService.UpdateTeam(team);
                        user.AuxWalletChecked = null;
                        _accountService.UpdateUser(user);
                    }
                }

                if (team.DisbandState != DisbandState.ProcessedCur
                    && team.DisbandState != DisbandState.ProcessedAll)
                {
                    if (TryDisbandingTeam(disbanding, true))
                    {
                        team.DisbandState = (team.DisbandState == DisbandState.ProcessedPrev) ? DisbandState.ProcessedAll : DisbandState.ProcessedCur;
                        _accountService.UpdateTeam(team);
                        user.AuxWalletChecked = null;
                        _accountService.UpdateUser(user);
                    }
                }
            }
        }


        private bool TryDisbandingTeam(Disbanding disbanding, bool useCurAddress)
        {
            var user = _accountService.GetUser();
            var address = useCurAddress ? disbanding.Teammate.BtcAddressCurrent : disbanding.Teammate.BtcAddressPrevious;
            if (address == null)
            {
                return false; // nothing to withdraw
            }
            var utxoNum = useCurAddress ? disbanding.UtxoCurAddrNum.Value : disbanding.UtxoPrevAddrNum.Value;
            if (utxoNum == 0)
            {
                return false; // nothing to withdraw
            }

            // get cosigners that provided the signatures
            var needSignatures = (address.Cosigners.Count + 1) / 2;
            var canCosignNum = 0;
            foreach (var cosigner in address.Cosigners.OrderBy(x => x.KeyOrder))
            {
                if (cosigner.DisbandingTxSignatures.Count == utxoNum)
                {
                    canCosignNum++;
                }
            }
            if (canCosignNum < needSignatures)
            {
                return false;
            }

            var utxos = FetchUtxos(address.Address, MinWithdrawInputBTC)
                .OrderByDescending(x => x.amount).Take(utxoNum).ToList();
            var withdrawalTx = GetWithdrawalTx(address, disbanding, utxos);

            var redeemScript = SignHelper.GetRedeemScript(address);

            int signaturesUsed = 0;
            List<Op>[] ops = new List<Op>[withdrawalTx.Inputs.Count];
            foreach (var cosigner in address.Cosigners.OrderBy(x => x.KeyOrder))
            {
                if (cosigner.DisbandingTxSignatures.Count < utxoNum)
                {
                    continue;
                }

                for (int input = 0; input < withdrawalTx.Inputs.Count; input++)
                {
                    if (ops[input] == null)
                    {
                        ops[input] = new List<Op>();
                    }
                    if (ops[input].Count == 0)
                    {
                        ops[input].Add(OpcodeType.OP_0);
                    }
                    var vchSig = _accountService.GetDisbandingTxSignature(cosigner, input).Signature.ToList();
                    vchSig.Add((byte)SigHash.All);
                    ops[input].Add(Op.GetPushOp(vchSig.ToArray()));
                }

                if (signaturesUsed == needSignatures)
                {
                    break;
                }
            }

            for (int input = 0; input < withdrawalTx.Inputs.Count; input++)
            {
                // add self-signatures
                var vchSig = SignHelper.Cosign(redeemScript, user.BitcoinPrivateKey, withdrawalTx, input).ToList();
                vchSig.Add((byte)SigHash.All);
                ops[input].Add(Op.GetPushOp(vchSig.ToArray()));

                ops[input].Add(Op.GetPushOp(redeemScript.ToBytes()));
                withdrawalTx.Inputs[input].ScriptSig = new Script(ops[input].ToArray());
            }

            string strTx = withdrawalTx.ToHex();

            if (!PostTx(strTx, address.Teammate.Team.Network))
            {
                return false;
            }

            return true;
        }


        private void CheckTeammateDisbandingMessages(Teammate teammate)
        {
            var user = _accountService.GetUser();

            if (teammate.Team.DisbandState == DisbandState.Invalid_NoAddress
                || teammate.Team.DisbandState == DisbandState.Invalid_NotMyTeam)
            {
                return;
            }

            Teammate me = teammate.Team.GetMe(user);
            if (me == null)
            {
                return;
            }

            var myDisbanding = me.CurDisbanding;
            if (myDisbanding == null)
            {
                return;
            }

            Cosigner myPrevCosigner = null;
            Cosigner myCurCosigner = null;
            if (me.BtcAddressCurrent != null && myDisbanding != null)
            {
                myCurCosigner = me.BtcAddressCurrent.Cosigners.FirstOrDefault(x => x.TeammateId == teammate.Id);
            }
            if (me.BtcAddressPrevious != null && myDisbanding != null)
            {
                myPrevCosigner = me.BtcAddressPrevious.Cosigners.FirstOrDefault(x => x.TeammateId == teammate.Id);
            }

            var teammateDisbanding = teammate.CurDisbanding;
            var hoursSinceLastCheck = (teammate.Team.DisbandingLastCheckedDate != null)
                ? (DateTime.UtcNow - teammate.Team.DisbandingLastCheckedDate.Value).TotalHours
                : -1;

            bool teamNeedCheck =
                teammate.Team.DisbandingLastCheckedDate == null
                || hoursSinceLastCheck > 12 && teammate.Team.DisbandState == DisbandState.Normal
                || hoursSinceLastCheck > 1 && teammate.Team.DisbandState != DisbandState.Normal;

            bool waitingForTeammateSignatures =
                myPrevCosigner != null && myPrevCosigner.DisbandingTxSignatures.Count < myDisbanding.UtxoPrevAddrNum
                || myCurCosigner != null && myCurCosigner.DisbandingTxSignatures.Count < myDisbanding.UtxoCurAddrNum;

            bool canCosignCur = false;
            bool canCosignPrev = false;
            if (teammate.BtcAddressCurrent != null && teammate.BtcAddressCurrent.Cosigners.FirstOrDefault(x => x.TeammateId == me.Id) != null)
            {
                canCosignCur = true;
            }
            if (teammate.BtcAddressPrevious != null && teammate.BtcAddressPrevious.Cosigners.FirstOrDefault(x => x.TeammateId == me.Id) != null)
            {
                canCosignPrev = true;
            }
            bool waitingForDisbandingMessage = (canCosignCur || canCosignPrev)
                && (teammateDisbanding == null // not started
                    || teammateDisbanding.SignatureDate == null);     // started but not finished

            bool teammateNeedCheck = waitingForDisbandingMessage || waitingForTeammateSignatures;

            if (teammateNeedCheck && teamNeedCheck)
            {
                PubKey pubKey = new PubKey(teammate.PublicKey);
                var opReturns = FetchOpReturns(pubKey.GetAddress(me.Team.Network).ToString())
                    .OrderByDescending(x => x.blocktime).ToList(); // process most recent first
                foreach (var opReturn in opReturns)
                {
                    byte[] data = ParseHex(opReturn.hex);
                    int pos = 1;
                    byte len = data[pos];
                    pos++;
                    if (len == 0x4c) // OP_PUSHDATA1 
                    {
                        len = data[pos];
                        pos++;
                    }
                    byte code = data[pos];
                    pos++;

                    switch (code)
                    {
                        case OpCodeDisband:
                            if (waitingForDisbandingMessage)
                            {
                                ProcessDisbandMessage(teammate, data, pos);
                                if (PostDisbandingSignatures(teammate.CurDisbanding, canCosignCur, canCosignPrev))
                                {
                                    teammate.CurDisbanding.SignatureDate = DateTime.UtcNow;
                                    _accountService.UpdateDisbanding(teammate.CurDisbanding);
                                }
                            }
                            break;

                        case OpCodeSignatureCur:
                            if (myCurCosigner != null)
                            {
                                ProcessDisbandingTxSignature(myCurCosigner, me.CurDisbanding, true, data, pos);
                            }
                            break;

                        case OpCodeSignaturePrev:
                            if (myCurCosigner != null)
                            {
                                ProcessDisbandingTxSignature(myCurCosigner, me.CurDisbanding, false, data, pos);
                            }
                            break;
                    }
                }
            }
        }


        private void ProcessDisbandMessage(Teammate teammate, byte[] data, int pos)
        {
            if (teammate.CurDisbanding != null && teammate.CurDisbanding.RequestDate != null)
            {
                return;
            }

            int teamId = BytesToInt(data, pos);
            pos += sizeof(int);
            if (teamId != teammate.Team.Id)
            {
                return;
            }

            var utxoPrevAddrNum = BytesToInt(data, pos);
            pos += sizeof(int);
            var utxoCurAddrNum = BytesToInt(data, pos);
            pos += sizeof(int);
            var addrLen = data[pos];
            pos++;
            if (addrLen + pos > data.Length)
            {
                return;
            }
            string withdrawAddress = System.Text.Encoding.UTF8.GetString(data, pos, addrLen);

            Disbanding disbanding = new Disbanding
            {
                Teammate = teammate,
                UtxoPrevAddrNum = utxoPrevAddrNum,
                UtxoCurAddrNum = utxoCurAddrNum,
                RequestDate = DateTime.UtcNow,
                WithdrawAddr = withdrawAddress
            };

            _accountService.AddDisbanding(disbanding);
            _accountService.SaveChanges();
        }


        private void ProcessDisbandingTxSignature(Cosigner myCosigner, Disbanding disbanding, bool forCurAddress, byte[] data, int pos)
        {
            int teammateId = BytesToInt(data, pos);
            pos += sizeof(int);
            if (teammateId != myCosigner.Address.Teammate.Id)
            {
                return; // this message is not for me
            }

            var utxoPos = data[pos];
            pos++;

            if (_accountService.GetDisbandingTxSignature(myCosigner, utxoPos) != null)
            {
                return; // we've got this one already
            }

            var signature = new DisbandingTxSignature();
            signature.UtxoPos = utxoPos;
            signature.Cosigner = myCosigner;
            signature.Signature = new byte[data.Length - pos];
            Array.Copy(data, pos, signature.Signature, 0, data.Length - pos);

            // Verify the signatute
            var me = myCosigner.Address.Teammate;
            var utxos = FetchUtxos(myCosigner.Address, MinWithdrawInputBTC);
            var utxoNum = forCurAddress ? me.CurDisbanding.UtxoCurAddrNum.Value : me.CurDisbanding.UtxoPrevAddrNum.Value;
            utxos = utxos.OrderByDescending(x => x.amount).Take(utxoNum).ToList();
            var withdrawalTx = GetWithdrawalTx(myCosigner.Address, disbanding, utxos);

            var redeemScript = SignHelper.GetRedeemScript(myCosigner.Address);
            uint256 hash = redeemScript.SignatureHash(withdrawalTx, utxoPos, SigHash.All);
            bool isGoodSignature = new PubKey(myCosigner.Teammate.PublicKey).Verify(hash, signature.Signature);
            if (!isGoodSignature)
            {
                return; // only store good ones
            }

            _accountService.AddDisbandingTxSignature(signature);
            _accountService.SaveChanges();

        }


        private static byte[] IntToBytes(int value)
        {
            byte[] intBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            return intBytes;
        }

        private static int BytesToInt(byte[] data, int pos)
        {
            byte[] intBytes = new byte[4];
            Array.Copy(data, pos, intBytes, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            return BitConverter.ToInt32(intBytes, 0);
        }

        private static byte[] ParseHex(string data)
        {
            return Encoders.Hex.DecodeData(data);
        }


        private bool PostDisbandingRequest(Team team)
        {
            var user = _accountService.GetUser();
            var disbanding = team.GetMe(user).CurDisbanding;

            if (disbanding == null)
            {
                return false;
            }

            List<ExplorerUtxo> utxosAux = null;
            List<ExplorerUtxo> utxosPrev = null;
            List<ExplorerUtxo> utxosCur = null;
            try
            {
                utxosAux = FetchUtxos(_accountService.GetAuxWallet(team.Network).ToString(), NormalFeeBTC);
                utxosPrev = FetchUtxos(team.GetMe(user).BtcAddressPrevious, MinWithdrawInputBTC)
                    .OrderByDescending(x => x.amount).Take(TopUtxosNum).ToList();
                utxosCur = FetchUtxos(team.GetMe(user).BtcAddressCurrent, MinWithdrawInputBTC)
                    .OrderByDescending(x => x.amount).Take(TopUtxosNum).ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "Couldn't fetch Utxos for disbanding.");
                return false;
            }
            var utxoAux = utxosAux.FirstOrDefault();
            if (utxoAux == null)
            {
                return false;
            }

            // Prepare OP_RETURN data
            var scriptParts = new List<byte>();
            scriptParts.AddRange(new byte[] { OpCodeDisband });
            scriptParts.AddRange(IntToBytes(team.Id));
            scriptParts.AddRange(IntToBytes(utxosPrev.Count));
            scriptParts.AddRange(IntToBytes(utxosCur.Count));
            scriptParts.AddRange(new byte[] { (byte)disbanding.WithdrawAddr.Length });
            scriptParts.AddRange(System.Text.Encoding.ASCII.GetBytes(disbanding.WithdrawAddr));

            Script scriptDisbandRequest = TxNullDataTemplate.Instance.GenerateScriptPubKey(scriptParts.ToArray());

            // Check if it's posted already
            var fetchedOpReturns = FetchOpReturns(_accountService.GetAuxWallet(team.Network).ToString());
            if (fetchedOpReturns.FirstOrDefault(x => x.hex == scriptDisbandRequest.ToHex()) != null)
            {
                //return false; 
            }

            // Build the Tx
            Coin coin = new Coin
            {
                Outpoint = new OutPoint
                {
                    Hash = uint256.Parse(utxoAux.txid),
                    N = (uint)utxoAux.vout
                },
                TxOut = new TxOut(new Money(utxoAux.amount, MoneyUnit.BTC), _accountService.GetAuxWallet(team.Network))
            };

            var txBuilder = new TransactionBuilder();
            var tx = txBuilder
                .AddCoins(new List<Coin> { coin })
                .AddKeys(user.BitcoinPrivateKey)
                .Send(scriptDisbandRequest, new Money(0))
                .SendFees(new Money(NormalFeeBTC, MoneyUnit.BTC))
                .SetChange(user.BitcoinPrivateKey.GetBitcoinSecret(team.Network).GetAddress())
                .BuildTransaction(true);

            if (tx == null || !txBuilder.Verify(tx))
            {
                return false;
            }

            string strTx = tx.ToHex();

            // Post the Tx
            if (!PostTx(strTx, team.Network))
            {
                return false;
            }

            disbanding.UtxoCurAddrNum = utxosCur.Count;
            disbanding.UtxoPrevAddrNum = utxosPrev.Count;
            disbanding.CurAddrBTCAmount = utxosCur.Sum(x => x.amount / BlockchainService.SatoshisInBTC);
            disbanding.PrevAddrBTCAmount = utxosPrev.Sum(x => x.amount / BlockchainService.SatoshisInBTC);

            team.DisbandState = DisbandState.NotifiedCosigners;

            _accountService.UpdateDisbanding(disbanding);
            _accountService.UpdateTeam(team);

            return true;
        }


        private bool PostDisbandingSignatures(Disbanding disbanding, bool needCosignCur, bool needCosignPrev)
        {
            // Check if we are a cosigner for the teammate
            var addressCur = disbanding.Teammate.BtcAddressCurrent;
            var addressPrev = disbanding.Teammate.BtcAddressCurrent;

            // Check if signatures's been posted already
            if (disbanding.SignatureDate != null)
            {
                return false;
            }

            var network = disbanding.Teammate.Team.Network;

            // Get aux wallet utxo
            int totalUtxos = 0;
            if (disbanding.Teammate.BtcAddressCurrent != null)
            {
                totalUtxos += disbanding.UtxoCurAddrNum.Value;
            }
            if (disbanding.Teammate.BtcAddressPrevious != null)
            {
                totalUtxos += disbanding.UtxoPrevAddrNum.Value;
            }
            if (totalUtxos == 0)
            {
                return true;
            }

            List<ExplorerUtxo> utxosAux = null;
            try
            {
                utxosAux = FetchUtxos(_accountService.GetAuxWallet(network).ToString(), NormalFeeBTC * totalUtxos);
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "Couldn't fetch Utxos for disbanding signatures.");
                return false;
            }
            var utxoAux = utxosAux.FirstOrDefault();
            if (utxoAux == null)
            {
                return false;
            }

            // Use aux money
            Coin coin = new Coin
            {
                Outpoint = new OutPoint
                {
                    Hash = uint256.Parse(utxoAux.txid),
                    N = (uint)utxoAux.vout
                },
                TxOut = new TxOut(new Money(utxoAux.amount, MoneyUnit.BTC), _accountService.GetAuxWallet(network))
            };


            // Get previously sent OpReturns
            var fetchedOpReturns = FetchOpReturns(_accountService.GetAuxWallet(network).ToString());

            // Get cosignature Txs for Current address
            var signatureTxs = new List<Transaction>();
            if (needCosignCur && disbanding.UtxoCurAddrNum > 0)
            {
                var utxos = FetchUtxos(addressCur.Address, MinWithdrawInputBTC)
                    .OrderByDescending(x => x.amount).Take(disbanding.UtxoCurAddrNum.Value).ToList();
                var withdrawalTx = GetWithdrawalTx(addressCur, disbanding, utxos);
                var cosignatures = GetUserCosignatures(addressCur, withdrawalTx);
                var signatureTxsCur = GetDisbandingSignaturesTxs(addressCur, true, cosignatures, coin, null, fetchedOpReturns);
                if (signatureTxsCur == null)
                {
                    return false;
                }
                signatureTxs.AddRange(signatureTxsCur);
            }

            // Get cosignature Txs for Previous address
            if (needCosignPrev && disbanding.UtxoCurAddrNum > 0)
            {
                var utxos = FetchUtxos(addressPrev.Address, MinWithdrawInputBTC)
                    .OrderByDescending(x => x.amount).Take(disbanding.UtxoCurAddrNum.Value).ToList();
                var withdrawalTx = GetWithdrawalTx(addressPrev, disbanding, utxos);
                var cosignatures = GetUserCosignatures(addressPrev, withdrawalTx);
                var signatureTxsPrev = GetDisbandingSignaturesTxs(addressPrev, false, cosignatures, coin, signatureTxs.LastOrDefault(), fetchedOpReturns);
                if (signatureTxsPrev == null)
                {
                    return false;
                }
                signatureTxs.AddRange(signatureTxsPrev);
            }


            foreach (var tx in signatureTxs)
            {
                string strTx = tx.ToHex();

                // Post the Tx
                if (!PostTx(strTx, network))
                {
                    return false;
                }
            }


            return true;
        }


        private Transaction GetWithdrawalTx(BtcAddress address, Disbanding disbanding, List<ExplorerUtxo> utxos)
        {
            decimal totalBTCAmount = 0;

            var tx = new Transaction();
            for (int input = 0; input < utxos.Count; input++)
            {
                var utxo = utxos[input];
                totalBTCAmount += utxo.amount;
                tx.Inputs.Add(new TxIn());
                tx.Inputs[input].PrevOut.N = (uint)utxo.vout;
                tx.Inputs[input].PrevOut.Hash = uint256.Parse(utxo.txid);
            }

            totalBTCAmount -= NormalFeeBTC;

            var bitcoinAddress = address.Teammate.Team.Network.CreateBitcoinAddress(disbanding.WithdrawAddr);
            tx.Outputs.Add(new TxOut(new Money(totalBTCAmount, MoneyUnit.BTC), bitcoinAddress));

            return tx;
        }


        private Transaction GetTx(Tx tx)
        {
            decimal totalBTCAmount = 0;

            var address = tx.Teammate.BtcAddressCurrent;
            var resTx = new Transaction();
            var txInputs = tx.Inputs.OrderBy(x => x.Id).ToList();
            for (int input = 0; input < txInputs.Count; input++)
            {
                var txInput = txInputs[input];
                totalBTCAmount += txInput.AmountBTC;
                resTx.Inputs.Add(new TxIn());
                resTx.Inputs[input].PrevOut.N = (uint)txInput.PrevTxIndex;
                resTx.Inputs[input].PrevOut.Hash = uint256.Parse(txInput.PrevTxId);
            }

            totalBTCAmount -= tx.FeeBtc ?? NormalFeeBTC;
            if (totalBTCAmount < tx.AmountBTC)
            {
                return null;
            }

            if (tx.Kind == TxKind.Payout || tx.Kind == TxKind.Withdraw)
            {
                var txOutputs = tx.Outputs.OrderBy(x => x.Id).ToList();
                var outputSum = 0M;
                for (int output = 0; output < txOutputs.Count; output++)
                {
                    var txOutput = txOutputs[output];
                    var bitcoinAddress = tx.Teammate.Team.Network.CreateBitcoinAddress(txOutput.PayTo.Address);
                    resTx.Outputs.Add(new TxOut(new Money(txOutput.AmountBTC, MoneyUnit.BTC), bitcoinAddress));
                    outputSum += txOutput.AmountBTC;
                }
                var changeAmount = totalBTCAmount - outputSum;
                if (changeAmount > NormalFeeBTC)
                {
                    var bitcoinAddressChange = tx.Teammate.Team.Network.CreateBitcoinAddress(tx.Teammate.BtcAddressCurrent.Address);
                    resTx.Outputs.Add(new TxOut(new Money(changeAmount, MoneyUnit.BTC), bitcoinAddressChange));
                }
            }
            else if (tx.Kind == TxKind.MoveToNextWallet)
            {
                var bitcoinAddress = tx.Teammate.Team.Network.CreateBitcoinAddress(tx.Teammate.BtcAddressNext.Address);
                resTx.Outputs.Add(new TxOut(new Money(totalBTCAmount, MoneyUnit.BTC), bitcoinAddress));
            }
            else if (tx.Kind == TxKind.SaveFromPrevWallet)
            {
                var bitcoinAddress = tx.Teammate.Team.Network.CreateBitcoinAddress(tx.Teammate.BtcAddressCurrent.Address);
                resTx.Outputs.Add(new TxOut(new Money(totalBTCAmount, MoneyUnit.BTC), bitcoinAddress));
            }

            return resTx;
        }


        private List<byte[]> GetUserCosignatures(BtcAddress address, Transaction tx)
        {
            var user = _accountService.GetUser();
            var redeemScript = SignHelper.GetRedeemScript(address);

            var cosignatures = new List<byte[]>();
            for (int input = 0; input < tx.Inputs.Count; input++)
            {
                var cosignature = SignHelper.Cosign(redeemScript, user.BitcoinPrivateKey, tx, input);
                cosignatures.Add(cosignature);
            }
            return cosignatures;
        }


        private List<Transaction> GetDisbandingSignaturesTxs(BtcAddress address, bool isCurAddress, List<byte[]> cosignatures, Coin coin, Transaction prevTx, List<OpReturnInfo> fetchedOpReturns)
        {
            var disbandingSignaturesTxs = new List<Transaction>();
            var user = _accountService.GetUser();
            for (int cosignature = 0; cosignature < cosignatures.Count; cosignature++)
            {
                // Prepare OP_RETURN data
                var scriptParts = new List<byte>();
                scriptParts.AddRange(new byte[] { isCurAddress ? OpCodeSignatureCur : OpCodeSignaturePrev });
                scriptParts.AddRange(IntToBytes(address.Teammate.Id));
                scriptParts.AddRange(new byte[] { (byte)cosignature });
                scriptParts.AddRange(cosignatures[cosignature]);

                Script scriptDisbandingSignature = TxNullDataTemplate.Instance.GenerateScriptPubKey(scriptParts.ToArray());

                if (fetchedOpReturns.FirstOrDefault(x => x.hex == scriptDisbandingSignature.ToHex()) != null)
                {
                    continue; // already posted 
                }

                var txBuilder = new TransactionBuilder();
                if (prevTx == null)
                {
                    txBuilder = txBuilder.AddCoins(new List<Coin> { coin });
                }
                else
                {
                    txBuilder = txBuilder.AddCoins(prevTx);
                }

                var tx = txBuilder
                    .AddKeys(user.BitcoinPrivateKey)
                    .Send(scriptDisbandingSignature, new Money(0))
                    .SendFees(new Money(NormalFeeBTC, MoneyUnit.BTC))
                    .SetChange(user.BitcoinPrivateKey.GetBitcoinSecret(address.Teammate.Team.Network).GetAddress())
                    .BuildTransaction(true);

                if (tx == null || !txBuilder.Verify(tx))
                {
                    return null;
                }

                prevTx = tx;
                disbandingSignaturesTxs.Add(tx);
            }

            return disbandingSignaturesTxs;
        }
    }
}
