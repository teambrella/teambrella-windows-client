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
using System.Linq;
using System.Numerics;
using NBitcoin;
using Teambrella.Client.DomainModel;

namespace Teambrella.Client.Signing
{
    class SignHelper
    {
        public static Script GetRedeemScript(BtcAddress address)
        {
            var ownerPublicKey = new PubKey(address.Teammate.PublicKey);
            var cosignersPublicKeys = address.Cosigners.OrderBy(x => x.KeyOrder).Select(x => new PubKey(x.Teammate.PublicKey)).ToList();
            var n = cosignersPublicKeys.Count;

            Op[] ops = new Op[7 + n];

            ops[0] = Op.GetPushOp(ownerPublicKey.ToBytes());
            ops[1] = OpcodeType.OP_CHECKSIGVERIFY;
            if (n > 6)
                ops[2] = OpcodeType.OP_3;
            else if (n > 3)
                ops[2] = OpcodeType.OP_2;
            else if (n > 0)
                ops[2] = OpcodeType.OP_1;
            else
                ops[2] = OpcodeType.OP_0;

            for (int i = 0; i < n; i++)
            {
                var pubKey = cosignersPublicKeys[i];
                ops[3 + i] = Op.GetPushOp(pubKey.ToBytes());
            }
            ops[3 + n] = (OpcodeType)(80 + n);
            ops[4 + n] = OpcodeType.OP_CHECKMULTISIG;
            ops[5 + n] = Op.GetPushOp(new BigInteger(address.Teammate.Team.Id));
            ops[6 + n] = OpcodeType.OP_DROP;

            Script redeemScript = new Script(ops);
            return redeemScript;
        }


        public static string GenerateStringAddress(BtcAddress address)
        {
            var redeemScript = GetRedeemScript(address);
            String addrString = redeemScript.GetScriptAddress(address.Teammate.Team.Network).ToString();
            return addrString;
        }


        public static Script GetPubKeyScript(BtcAddress address)
        {
            var redeemScript = GetRedeemScript(address);
            Script pubKeyScript = new Script(
                OpcodeType.OP_HASH160,
                Op.GetPushOp(redeemScript.GetScriptAddress(address.Teammate.Team.Network).ToBytes()),
                OpcodeType.OP_EQUAL
                );
            return pubKeyScript;
        }


        public static byte[] Cosign(Script redeemScript, Key key, Transaction transaction, int inputNum)
        {
            uint256 hash = redeemScript.SignatureHash(transaction, inputNum, SigHash.All);
            return key.Sign(hash).ToDER();
        }
    }
}
