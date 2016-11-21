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
    public class TeamRepository : RepositoryBase
    {
        public TeamRepository(TeambrellaContext context)
            : base(context)
        {
        }

        public Team Get(int id)
        {
            return _context.Team.FirstOrDefault(x => x.Id == id);
        }

        public List<Team> GetAll()
        {
            return _context.Team.ToList();
        }

        public void Update(Team team)
        {
            Update<Team>(team);
        }

        public Team Add(Team team)
        {
            return Add<Team>(team);
        }

    }
}


