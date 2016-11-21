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

namespace Teambrella.Client.DomainModel
{
    /// <summary>
    /// Status of the wallet address for the local user
    /// </summary>
    public enum UserAddressStatus : int
    {
        #region incoming server DTO values
        Previous = 0,
        Current = 1,
        Next = 2,
        Archive = 3,
        #endregion

        #region extra values, that are valid for local DB only
        Invalid = 4,
        ServerPrevious = 10,
        ServerCurrent = 11,
        ServerNext = 12,
        #endregion
    }

    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class BtcAddress
    {
        #region server DTO properties
        public string Address { get; set; }
        public int TeammateId { get; set; }
        public UserAddressStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        #endregion

        public virtual Teammate Teammate { get; set; }
        public virtual IList<Cosigner> Cosigners { get; set; }
        public virtual IList<Tx> MoveFundsTxs { get; set; }
    }
}
