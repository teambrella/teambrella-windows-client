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
using System.Data.Entity;
using Teambrella.Client.Dal;

namespace Teambrella.Client.Repositories
{
    public class RepositoryBase
    {
        protected readonly TeambrellaContext _context;

        public RepositoryBase(TeambrellaContext context)
        {
            _context = context;
        }

        public T Add<T>(T entity) where T : class
        {
            var dbSet = _context.Set<T>();
            var newProxy = dbSet.Create();
            newProxy = dbSet.Add(newProxy);
            var entry = _context.Entry(newProxy);
            entry.CurrentValues.SetValues(entity);
            return newProxy;
        }

        public void Update<T>(T entity) where T : class
        {
            if (_context.Entry(entity).State != EntityState.Added)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
