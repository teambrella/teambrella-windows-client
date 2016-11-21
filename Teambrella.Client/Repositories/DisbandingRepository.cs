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
using Teambrella.Client.Dal;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.Repositories
{
    public class DisbandingRepository : RepositoryBase
    {
        public DisbandingRepository(TeambrellaContext context)
            : base(context)
        {
        }


        public Disbanding Add(Disbanding disbanding)
        {
            return Add<Disbanding>(disbanding);
        }

        public void Update(Disbanding disbanding)
        {
            Update<Disbanding>(disbanding);
        }
    }
}


