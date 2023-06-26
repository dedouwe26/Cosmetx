# Cosmetx
This allows you to gain access to all cosmetics, including the unobtainable ones! \ `Client-side only!`

## How it works!
In DnSpy you can find out where the cosmetics are handled.
So in GorillaNetworking.CosmeticsController.GetUserCosmeticsAllowed() you can apply a patch that gives you all cosmetics.

## How to build!
First you will need to install an .NET Core SDK or framework \ 
Then you download the project and change in the `Directory.Build.props` the Game path. \ 
Then you run the `dotnet build` command and the DLL file is in `bin/Debug/netstandard2/Cosmetx.dll` \ 
If you get an error, you will need to run `dotnet restore` first.

