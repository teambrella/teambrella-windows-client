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
    public class DisbandingTxSignatureRepository : RepositoryBase
    {
        public DisbandingTxSignatureRepository(TeambrellaContext context)
            : base(context)
        {
        }

        public DisbandingTxSignature Add(DisbandingTxSignature signature)
        {
            return Add<DisbandingTxSignature>(signature);
        }

        public DisbandingTxSignature Get(Cosigner cosigner, int utxoPos)
        {
            return _context.DisbandingTxSignature.FirstOrDefault(x => x.AddressId == cosigner.AddressId && x.KeyOrder == cosigner.KeyOrder && x.UtxoPos == utxoPos);
        }

        public void Update(DisbandingTxSignature signature)
        {
            Update<DisbandingTxSignature>(signature);
        }
    }
}

