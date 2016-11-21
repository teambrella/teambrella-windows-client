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

namespace Teambrella.Client.DomainModel
{
    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class PayTo
    {
        #region server DTO properties
        public Guid Id { get; set; }
        public int TeammateId { get; set; }
        public DateTime KnownSince { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
        #endregion

        public virtual Teammate Teammate { get; set; }
    }
}
