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
using System.ComponentModel.DataAnnotations.Schema;
using NBitcoin;

namespace Teambrella.Client.DomainModel
{
    public enum DisbandState : int
    {
        Normal = 0,            // no disband
        CosignerInitiated = 1, // our cosigner or someone who we are cosigner of started disbanding
        Initiated = 2,         // we initiated disbanding
        NotifiedCosigners = 3, // our disbanding request is posted
        ProcessedPrev = 4,
        ProcessedCur = 5,
        ProcessedAll = 6,
        Invalid_NoAddress = 7,
        Invalid_NotMyTeam = 8
    }

    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class Team
    {
        public const int AutoApprovalOff = -1;

        #region server DTO properties
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   // ID - comes from server; we don't want local DB auto-generates them on add.
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Testnet { get; set; }
        #endregion


        public DisbandState DisbandState { get; set; }

        public int PayToAddressOkAge { get; set; }
        public int AutoApprovalMyGoodAddress { get; set; }
        public int AutoApprovalCosignGoodAddress { get; set; }
        public int AutoApprovalMyNewAddress { get; set; }
        public int AutoApprovalCosignNewAddress { get; set; }

        public virtual IList<Teammate> Teammates { get; set; }

        public Teammate GetMe(User user)
        {
            var pubkey = new BitcoinSecret(user.PrivateKey).PubKey.ToString();
            var teammates = Teammates;
            return (null == teammates)
                    ? null
                    : Teammates.FirstOrDefault(x => x.PublicKey == pubkey);
        }

        public bool IsInNormalState
        {
            get
            {
                return
                    DisbandState == DomainModel.DisbandState.Normal
                    || DisbandState == DomainModel.DisbandState.CosignerInitiated;
            }
        }

        public NBitcoin.Network Network
        {
            get
            {
                return Testnet ? NBitcoin.Network.TestNet : NBitcoin.Network.Main;
            }
        }

        public string DisplayName
        {
            get
            {
                if (Testnet)
                {
                    return "[testnet] " + Name;
                }
                return Name;
            }
        }

        [NotMapped]
        public DateTime? DisbandingLastCheckedDate { get; set; }

        [NotMapped]
        public bool FetchedMyDisbandingStatus { get; set; }
    }
}


