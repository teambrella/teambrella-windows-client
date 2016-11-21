Components:
    -- teambrella.exe               - windows tray application.
	-- TeambrellaService.exe		- windows service that pings and fetches the latest data from teambrella.com server.
    -- Teambrella.Client.dll        - shared code, that is reused by teambrella.exe and TeambrellaService.exe.


==================
HOW TO BUILD:
==================
PREREQUISITES:

 - .NET 4.5.1 Dev Pack https://www.microsoft.com/en-US/download/details.aspx?id=40772
 - Microsoft Visual Studio 2015 Community Edition (VS) https://www.visualstudio.com/post-download-vs/?sku=community

BUILD:
 - Open Teambrella.Cleint.sln in VS. Build (Ctrl+Shift+B).


==================
HOW TO RUN:
==================
Run teambrella.exe (F5)               - this will also start TeambrellaService in an emulation mode.


==================
WHAT TO DO AFTER RUN:
==================
 - right-click on the tray icon. Select "My Account" context menu item.
                                      - this will forward you to teambrella.com login page.
									    After authentication on teambrella.com the local wallet will be confirmed and linked to your login.
 - right-click on the tray icon. Select "Team Settings " context menu item.
									  - you'll see client settings you can configure for your teams 
 - right-click on the tray icon. Select "Check Transactions" context menu item.
									  - you'll see the list of bitcoin transaction to be approved by you, other have been already approved by you.
