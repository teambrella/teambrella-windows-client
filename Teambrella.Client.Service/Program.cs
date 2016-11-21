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

namespace Teambrella.Client.Service
{
    static class Program
    {
        /// <summary>
        /// Entry point for the manual run of the windows service.
        /// </summary>
        static void Main()
        {
            ServiceBase[] servicesToRun =
            {
                new TeambrellaService()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
