[Teambrella](https://teambrella.com)'s client is a bitcoin wallet software for the p2p insurance system.

Your funds are stored in a special personal Bitcoin wallet. The funds in the wallet can only be spent if both you and three out of eight semi-randomly selected members of your team co-sign a transaction. The private keys to the wallet are stored on respective client systems only (i.e., yours and those of your cosigners) and are never transmitted outside.


## Features
- Automatic payment of reimbursements and withdrawals
- Control and prevention of suspicious transactions
- Team disbanding: When you and your teammates disband the team, everyone gets their funds back with no help from the server.

## Build Procedure
- Install .NET 4.5.1 Dev Pack https://www.microsoft.com/en-US/download/details.aspx?id=40772
- Install Microsoft Visual Studio 2015 Community Edition (VS) https://www.visualstudio.com/post-download-vs/?sku=community
- Open Teambrella.Cleint.sln in VS. Build (Ctrl+Shift+B).

## Components
| Filename | Description|
|---|---|
| teambrella.exe | Windows tray application |
| TeambrellaService.exe | Windows service: pings and fetches the latest data from teambrella.com server |
| Teambrella.Client.dll | Shared code |

