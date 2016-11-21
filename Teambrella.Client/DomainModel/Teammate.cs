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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Teambrella.Client.DomainModel
{
    /// <summary>
    /// Dual responsibility entity:
    /// 1) Incoming DTO (Data Transfer Object) from teambrella server.
    /// 2) Local DB entity for that server DTO.
    /// </summary>
    public class Teammate
    {
        #region server DTO properties
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   // ID - comes from server; we don't want local DB auto-generates them on add.
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string FBName { get; set; }
        public string PublicKey { get; set; }
        #endregion

        public virtual Team Team { get; set; }
        public virtual IList<PayTo> PayTos { get; set; }
        public virtual IList<BtcAddress> Addresses { get; set; }
        public BtcAddress BtcAddressPrevious
        {
            get { return Addresses == null ? null : Addresses.FirstOrDefault(addr => addr.Status == UserAddressStatus.Previous); }
        }
        public BtcAddress BtcAddressCurrent
        {
            get
            {
                return Addresses == null ? null : Addresses.FirstOrDefault(addr => addr.Status == UserAddressStatus.Current);
            }
        }
        public BtcAddress BtcAddressNext
        {
            get { return Addresses == null ? null : Addresses.FirstOrDefault(addr => addr.Status == UserAddressStatus.Next); }
        }

        public virtual IList<Cosigner> CosignerOf { get; set; }

        public virtual IList<Disbanding> Disbandings { get; set; }
        public Disbanding CurDisbanding
        {
            get
            {
                var disbanding = Disbandings.OrderByDescending(x => x.RequestDate).FirstOrDefault();
                if (disbanding == null || disbanding.RequestDate == null || (DateTime.UtcNow - disbanding.RequestDate.Value).TotalDays > 30)
                    return null;

                return disbanding;
            }
        }
    }
}

