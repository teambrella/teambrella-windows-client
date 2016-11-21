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
using NBitcoin;
using Teambrella.Client.Dal;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.Repositories
{
    public class TeammateRepository : RepositoryBase
    {
        public TeammateRepository(TeambrellaContext context)
            : base(context)
        {
        }

        public List<Teammate> GetWithPubKey(PubKey pubKey)
        {
            string pubKeyHex = pubKey.ToHex();
            return _context.Teammate.Where(x => x.PublicKey == pubKeyHex).ToList();
        }

        public List<Teammate> GetAll()
        {
            return _context.Teammate.ToList();
        }


        public Teammate Get(int teammateId)
        {
            return _context.Teammate.FirstOrDefault(x => x.Id == teammateId);
        }

        public Teammate InsertOrUpdateDetached(Teammate entity)
        {
            var attachedTeammate = _context.Teammate.FirstOrDefault(x => x.Id == entity.Id);
            if (attachedTeammate == null)
            {
                attachedTeammate = _context.Teammate.Create();
                if (entity.GetType().Equals(attachedTeammate.GetType()))
                {
                    _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    return entity;

                }
                else
                {
                    attachedTeammate = _context.Teammate.Add(attachedTeammate);
                }
            }
            var attachedEntry = _context.Entry(attachedTeammate);
            attachedEntry.CurrentValues.SetValues(entity);
            return attachedTeammate;
        }
    }
}

