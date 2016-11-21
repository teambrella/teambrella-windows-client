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
using System.Windows.Forms;
using NBitcoin;
using Teambrella.Client.Services;

namespace Teambrella.Client.App
{
    public partial class DisbandAddressForm : Form
    {
        private Network _network;

        public DisbandAddressForm(Network network)
        {
            _network = network;
            InitializeComponent();
            buttonOK.Enabled = false;
            labelBadAddress.Visible = false;
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var addr = _network.CreateBitcoinAddress(textBoxAddress.Text);
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "Couldn't create Bitcoin address.");
                buttonOK.Enabled = false;
                labelBadAddress.Visible = true;
                return;
            }
            buttonOK.Enabled = true;
            labelBadAddress.Visible = false;
        }
    }
}
