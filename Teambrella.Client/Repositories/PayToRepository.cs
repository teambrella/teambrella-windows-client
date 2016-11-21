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
using System.Collections.Generic;
using System.Linq;
using Teambrella.Client.Dal;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.Repositories
{
    public class PayToRepository : RepositoryBase
    {
        public PayToRepository(TeambrellaContext context)
            : base(context)
        {
        }

        public List<PayTo> GetForTeammate(int teammateId)
        {
            return _context.PayTo.Where(x => x.TeammateId == teammateId).ToList();
        }

        public PayTo Get(int teammateId, string address)
        {
            return _context.PayTo.FirstOrDefault(x => x.TeammateId == teammateId && x.Address == address);
        }

        public PayTo Add(PayTo payTo)
        {
            return Add<PayTo>(payTo);
        }

        public void Update(PayTo payTo)
        {
            Update<PayTo>(payTo);
        }
    }
}

