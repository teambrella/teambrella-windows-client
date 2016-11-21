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
using NBitcoin;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.ServerApiModels
{
    public class ApiResult
    {
        public long Timestamp { get; set; }
    }

    public class ApiQuery
    {
        public long Timestamp { get; set; }
        public string Signature { get; set; }
        public string PublicKey { get; set; }

        public ApiQuery AddSignature(long timestamp, Key key)
        {
            Timestamp = timestamp;
            Signature = key.SignMessage(timestamp.ToString());
            PublicKey = key.PubKey.ToString();
            return this;
        }
    }

    public class GetUpdatesApiQuery : ApiQuery
    {
        public long LastUpdated { get; set; }
        public List<TxClientInfoApi> TxInfos { get; set; }
        public List<TxSignatureClientInfoApi> TxSignatures { get; set; }
    }

    public class GetUpdatesApiResult : ApiResult
    {
        public List<Team> Teams { get; set; }
        public List<Teammate> Teammates { get; set; }
        public List<PayTo> PayTos { get; set; }
        public List<Tx> Txs { get; set; }
        public List<TxInput> TxInputs { get; set; }
        public List<TxOutput> TxOutputs { get; set; }
        public List<TxSignatureClientInfoApi> TxSignatures { get; set; }
        public List<BtcAddress> BTCAddresses { get; set; }
        public List<Cosigner> Cosigners { get; set; }
    }

    public class LoginQuery : ApiQuery
    {
        public int? TeamId { get; set; }
        public int? ClaimId { get; set; }
        public int? TeammateId { get; set; }
        public bool IsWithdrawal { get; set; }
    }

    public class TxSignatureClientInfoApi
    {
        public Guid TxInputId { get; set; }
        public int TeammateId { get; set; }
        public string Signature { get; set; }
    }

    public class TxClientInfoApi
    {
        public Guid Id { get; set; }
        public DateTime? ResolutionTime { get; set; }
        public TxClientResolution Resolution { get; set; }
    }

    public enum TxKind : int
    {
        Payout = 0, // voting compensation or reimbursement 
        Withdraw = 1,
        MoveToNextWallet = 2,
        SaveFromPrevWallet = 3
    }

    public enum TxState : int
    {
        Created = 0,
        ApprovedMaster = 1,
        ApprovedCosigners = 2,
        ApprovedAll = 3, // =?>  SelectedForCosigning (select by date)
        BlockedMaster = 4,
        BlockedCosigners = 5,
        SelectedForCosigning = 6, // => BeingCosigned (after at least half co-signers got signature tasks)
        BeingCosigned = 7,
        Cosigned = 8,
        Published = 9,
        Confirmed = 10,
        ErrorCosignersTimeout = 100,
        ErrorSubmitToBlockchain = 101,
        ErrorBadRequest = 102, // bad id, kind or amounts
        ErrorOutOfFunds = 103,
        ErrorTooManyUtxos = 104,
    }

    public enum TxClientResolution : int
    {
        None = 0,
        Received = 1,
        Approved = 2,
        Blocked = 3,
        Signed = 4,
        Published = 5,
        ErrorCosignersTimeout = 100,
        ErrorSubmitToBlockchain = 101,
        ErrorBadRequest = 102, // bad id, kind or amounts
        ErrorOutOfFunds = 103
    }
    public enum TxSigningState : int
    {
        Created = 0,
        TakenForApproval = 1,
        Approved = 2,
        Blocked = 3,
        NeedsSigning = 4,
        Signed = 5
    }
}
