# Cosmetx 🧢
This allows you to gain access to all cosmetics, including the unobtainable ones! <br />
`Client-side only!`

## How to build! 🔨
1. I suggest you to install an [.NET SDK](https://dotnet.microsoft.com/en-us/download) or any .NET implementation that supports [.NET Standard 2.1](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1#select-net-standard-version). <br />
2. After that you download the project and change the `GamePath` in `Cosmetx/Directory.Build.props` to the path of the Gorilla Tag folder. <br />
3. Then you run the `dotnet build` command. After it is done building, the plugin should be in `Cosmetx/bin/Debug/netstandard2.1/Cosmetx.dll`
- If you get an **error**, you can run `dotnet restore` first.

## Troubleshooting! 🪲
 - If the logs in: `Gorilla Tag/BepInEx/LogOutput.log` show `[Message:   Cosmetx] Cosmetx - Disabled` without doing anything. Change `HideManagerGameObject` in `Gorilla Tag/BepInEx/config/BepInEx.cfg` under the `[Chainloader]` group to `true`.

Under the [MIT license](LICENSE).

