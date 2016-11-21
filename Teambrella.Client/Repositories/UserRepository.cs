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
    public class UserRepository : RepositoryBase
    {
        public UserRepository(TeambrellaContext context) : base(context)
        {
        }

        public User GetUser()
        {
            var user = _context.User.FirstOrDefault();
            return user;
        }

        public User Create(User user)
        {
            return Add<User>(user);
        }

        public void Update(User user)
        {
            Update<User>(user);
        }
    }
}
