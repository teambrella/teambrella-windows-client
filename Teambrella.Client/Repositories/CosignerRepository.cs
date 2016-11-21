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
using System.Linq;
using Teambrella.Client.Dal;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.Repositories
{
    public class CosignerRepository : RepositoryBase
    {
        public CosignerRepository(TeambrellaContext context)
            : base(context)
        {
        }

        public Cosigner Get(string address, int teammateId)
        {
            return _context.Cosigner.FirstOrDefault(x => x.AddressId == address && x.TeammateId == teammateId);
        }

        public Cosigner Add(Cosigner cosigner)
        {
            return Add<Cosigner>(cosigner);
        }
    }
}

