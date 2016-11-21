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
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Teambrella.Client.Services;

namespace Teambrella.Client.Service
{
    public partial class TeambrellaService : ServiceBase
    {
        private volatile bool _isStopped;

        public TeambrellaService()
        {
            InitializeComponent();
        }

        public void Run()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                while (!_isStopped)
                {
                    using (var localAccountService = new AccountService())
                    {
                        if (!localAccountService.UpdateData())
                        {
                            Thread.Sleep(1000 * 60 * 1);
                        }
                        else
                        {
                            Thread.Sleep(1000 * 60 * 10);
                        }
                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                while (!_isStopped)
                {
                    using (var localAccountService = new AccountService())
                    {
                        var localBlockchainService = new BlockchainService(localAccountService);
                        if (!localBlockchainService.UpdateData())
                        {
                            Thread.Sleep(1000 * 60 / 6);
                        }
                        else
                        {
                            Thread.Sleep(1000 * 60);
                        }
                    }
                }
            });
        }

        protected override void OnStop()
        {
            _isStopped = true;
        }
    }
}
