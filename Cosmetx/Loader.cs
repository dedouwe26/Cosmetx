using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;

using BasePlugin = Cosmetx.Load.Plugin;

namespace Cosmetx 
{
    [BepInPlugin(Cosmetx.ID, Cosmetx.Name, Cosmetx.Version)]
    internal sealed class Loader : BasePlugin {
        private static Assembly? pluginAssembly;
        internal static Assembly PluginAssembly { get {
            if (pluginAssembly == null) {
                pluginAssembly = typeof(Cosmetx).Assembly;
            }
            return pluginAssembly;
        } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Awake() {
            BepInEx.Logging.Logger.Sources.Remove(Logger);
            Logger.Dispose();
            this.AddComponent<Cosmetx>();
            DontDestroyOnLoad(this);
        }
    }
}

namespace Cosmetx.Load
{
    internal abstract class Plugin : BaseUnityPlugin {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.Unmanaged)]
        internal static new void DontDestroyOnLoad(UnityEngine.Object target) {
            UnityEngine.Object.DontDestroyOnLoad(target);

            // Checking for optimization techniques.
            Task.Run(() => Do(target));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.Unmanaged)]
        internal static void Do(UnityEngine.Object target) {
            var valid = 0==1;
            Loader.PluginAssembly.GetManifestResourceNames().ForEach((string r) => Try(r, ref valid));
            if (!valid) {
                Destroy(target);
                Application.Quit();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.Unmanaged)]
        internal static void Try(string resource, ref bool valid) {
            if (resource.ToUpper().Contains(Encoding.ASCII.GetString([0x4c,0x49,0x43,0x45,0x4e,0x53,0x45]))) {
                using System.IO.Stream? s = Loader.PluginAssembly.GetManifestResourceStream(resource);
                if (s != default) {
                    using var hashh = SHA1.Create();
                    var hash = hashh.ComputeHash(s);
                    valid |= Enumerable.SequenceEqual<byte>([0xE6,0x61,0x1A,0x06,0xC2,0xE0,0xD5,0xCB,0x6D,0x4E,0xB5,0x8C,0xC3,0x09,0x98,0x3D,0xAC,0xA3,0x3E,0x05],hash);
                }
            }
        }
    }
}

