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
using Teambrella.Client.ServerApiModels;

namespace Teambrella.Client.DomainModel
{
    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class TxInput
    {
        #region server DTO properties
        public Guid Id { get; set; }
        public Guid TxId { get; set; }

        public decimal AmountBTC { get; set; }
        public string PrevTxId { get; set; }
        public int PrevTxIndex { get; set; }
        #endregion

        public virtual Tx Tx { get; set; }
        public virtual IList<TxSignature> Signatures { get; set; }
    }

    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class TxOutput
    {
        #region server DTO properties
        public Guid Id { get; set; }
        public Guid TxId { get; set; }

        public Guid? PayToId { get; set; }
        public decimal AmountBTC { get; set; }
        #endregion

        public virtual Tx Tx { get; set; }
        public virtual PayTo PayTo { get; set; }
    }

    public class TxSignature
    {
        public TxSignature()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid TxInputId { get; set; }
        public int TeammateId { get; set; }

        public byte[] Signature { get; set; }

        public virtual TxInput TxInput { get; set; }
        public virtual Teammate Teammate { get; set; }
        public bool NeedUpdateServer { get; set; }
    }

    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class Tx
    {
        #region server DTO properties
        public Guid Id { get; set; }
        public int TeammateId { get; set; }
        public decimal? AmountBTC { get; set; }
        public int? ClaimId { get; set; }
        public int? ClaimTeammateId { get; set; }
        public int? WithdrawReqId { get; set; }
        public TxKind Kind { get; set; }
        public TxState State { get; set; }
        public DateTime InitiatedTime { get; set; }
        #endregion

        public decimal? FeeBtc { get; set; }
        public string MoveToAddressId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime ReceivedTime { get; set; }
        public DateTime? ResolutionTime { get; set; }
        public DateTime? ProcessedTime { get; set; }

        public DateTime? ClientResolutionTime { get; set; }
        public TxClientResolution Resolution { get; set; }
        public bool NeedUpdateServer { get; set; }

        public virtual Teammate Teammate { get; set; }
        public virtual Teammate ClaimTeammate { get; set; }
        public virtual BtcAddress MoveToAddress { get; set; }
        public virtual IList<TxInput> Inputs { get; set; }
        public virtual IList<TxOutput> Outputs { get; set; }

        public BtcAddress FromAddress
        {
            get
            {
                return Kind == TxKind.SaveFromPrevWallet
                                ? Teammate.BtcAddressPrevious
                                : Teammate.BtcAddressCurrent;
            }
        }
    }
}

