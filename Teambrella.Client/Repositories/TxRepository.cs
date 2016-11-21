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
using System.Linq;
using Teambrella.Client.Dal;
using Teambrella.Client.DomainModel;
using Teambrella.Client.ServerApiModels;

namespace Teambrella.Client.Repositories
{
    public class TxRepository : RepositoryBase
    {
        public TxRepository(TeambrellaContext context)
            : base(context)
        {
        }

        public Tx Get(Guid id)
        {
            return _context.Tx.FirstOrDefault(x => x.Id == id);
        }

        public TxInput GetInput(Guid id)
        {
            return _context.TxInput.FirstOrDefault(x => x.Id == id);
        }

        public TxSignature GetSignature(Guid txInputId, int teammateId)
        {
            return _context.TxSignature.FirstOrDefault(x => x.TxInputId == txInputId && x.TeammateId == teammateId);
        }

        public List<Tx> GetChanged(DateTime time)
        {
            return _context.Tx.Where(x => x.UpdateTime.Value > time).ToList();
        }

        public List<Tx> GetTxsNeedServerUpdate()
        {
            return _context.Tx.Where(x => x.NeedUpdateServer).ToList();
        }

        public List<TxSignature> GetTxsSignaturesNeedServerUpdate()
        {
            return _context.TxSignature.Where(x => x.NeedUpdateServer).ToList();
        }

        public List<Tx> GetResolvable()
        {
            return _context.Tx.Where(x => x.Resolution == TxClientResolution.Received).ToList();
        }

        public List<Tx> GetCoSignable()
        {
            return _context.Tx
                .Where(x => x.Resolution == TxClientResolution.Approved)
                .Where(x => x.State == TxState.SelectedForCosigning)
                .Where(x => x.Inputs.Count > 0)
                .ToList();
        }

        public List<Tx> GetApprovedAndCosigned()
        {
            return _context.Tx
                .Where(x => x.Resolution == TxClientResolution.Approved)
                .Where(x => x.State == TxState.Cosigned)
                .Where(x => x.Inputs.Count > 0)
                .ToList();
        }

        public Tx Add(Tx tx)
        {
            return Add<Tx>(tx);
        }

        //public void SetInputs(Tx tx, List<TxInput> inputs)
        //{
        //    foreach (var input in inputs) { 
        //        Add<TxInput>(input);
        //    }
        //}

        public TxInput AddInput(TxInput input)
        {
            return Add<TxInput>(input);
        }

        public TxOutput AddOutput(TxOutput output)
        {
            return Add<TxOutput>(output);
        }

        public TxSignature AddSignature(TxSignature signature)
        {
            TxSignature res = GetSignature(signature.TxInput.Id, signature.Teammate.Id);
            if (res != null)
            {
                return res;
            }
            return Add<TxSignature>(signature);
        }

        public void Update(Tx tx)
        {
            Update<Tx>(tx);
        }

        public void Update(TxSignature signature)
        {
            Update<TxSignature>(signature);
        }
    }
}


