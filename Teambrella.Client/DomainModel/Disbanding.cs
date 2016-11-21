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
    public class Disbanding
    {
        public int Id { get; set; }
        public int TeammateId { get; set; }
        public string WithdrawAddr { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? SignatureDate { get; set; }
        public int? UtxoCurAddrNum { get; set; }
        public int? UtxoPrevAddrNum { get; set; }
        public decimal? CurAddrBTCAmount { get; set; }
        public decimal? PrevAddrBTCAmount { get; set; }

        public virtual Teammate Teammate { get; set; }
    }
}

