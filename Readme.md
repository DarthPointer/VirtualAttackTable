# Welcome to the VirtualAttackTable repository

VirtualAttackTable is deigned to store a table of target vessels when playing subsims. As of now it only "supports" [Wolfpack](https://store.steampowered.com/app/490920/) with target identification filters and ship data based on the assets of this game. It is supplied with ship images from the extracted game assets.

In order to use the application:
- Install [ASP.NET Core Runtime 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) (if you don't have it installed yet).
- Download and unpack built server zip (Named VirtualAttackTableServerX.X.X) from the latest [Release](https://github.com/DarthPointer/VirtualAttackTable/releases/latest).
- Run `000_run_*.cmd`. As of now there is not much point in running the `open` one because each client will have a separate attack table and won't be able to interact with others. If you want to change the port the server will use, edit the correspinding .cmd file.
- Connect to the server with your browser (type `[server IP/localhost]:[server port]`).
