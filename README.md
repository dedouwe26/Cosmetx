`Thanks! For everyone that supported this little mod, we have hit more than 10,000 VIEWS! For everyone that still wants a new update, there is a small chance that there will be one. Anyways, thanks alot!`

# Cosmetx 🧢
This allows you to gain access to all cosmetics, including the unobtainable ones! <br />
`Client-side only!`
## Requires 📦
Utilla 1.6.13
## How it works! ⚙️
In DnSpy you can find out where the cosmetics are handled. <br />
So in GorillaNetworking.CosmeticsController.GetUserCosmeticsAllowed() you can apply a patch that gives you all cosmetics.

## How to build! 🔨
First you will need to install an .NET Core SDK or framework <br />
Then you download the project and change in the `Directory.Build.props` the Game path. <br />
Then you run the `dotnet build` command and the DLL file is in `bin/Debug/netstandard2/Cosmetx.dll` <br />
If you get an error, you will need to run `dotnet restore` first.

