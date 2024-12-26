using System;
using BepInEx.Logging;
using ExitGames.Client.Photon.StructWrapping;
using GorillaNetworking;
using HarmonyLib;
using UnityEngine;

namespace Cosmetx
{
    public sealed class Patcher : IDisposable {
        private static Harmony? instance;

        public static bool IsPatched { get; private set; }

        internal static void ApplyPatches() {
            Cosmetx.Log?.LogInfo($"{Cosmetx.Name} - Patching");

            instance ??= new Harmony(Cosmetx.ID);
            if (!IsPatched) {
                instance.PatchAll(Loader.PluginAssembly);
                IsPatched = true;
            }
        }

        internal static void RemovePatches() {
            Cosmetx.Log?.LogInfo($"{Cosmetx.Name} - Removing patches");

            if (instance != null && IsPatched) {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }

        public void Dispose() {
            RemovePatches();
        }
    }

    public sealed class Cosmetx : MonoBehaviour {
        public const string ID = "com.oxded.gorillatag.cosmetx";
        public const string Version = "1.3.0";
        public const string Name = "Cosmetx";
        public const string Owner = "0xDED";
        public const string License = "MIT License";

        public static ManualLogSource? Log;
        private static string? catalogName;
        public static string CatalogName { get {
            catalogName ??= CosmeticsController.instance?.catalog ?? "DLC";
            return catalogName;
        } }
        private static string? currencyName;
        public static string CurrencyName { get {
            if (currencyName == null) {
                if (CosmeticsController.instance != null) {
                    try {
                        currencyName = Traverse.Create(CosmeticsController.instance)?.Field("currencyName")?.GetValue() as string;
                    } catch { }
                }
                currencyName ??= "SR";
            }
            return currencyName;
        } }

        void Awake() {
            Log = BepInEx.Logging.Logger.CreateLogSource(Name);
            Log.LogMessage($"{Name} {Version} by {Owner}. Under the {License}.");
        }
        void Start() {
            Log?.LogMessage($"{Name} - Started");
        }

        private void PokeWardrobes() {
            foreach (CosmeticWardrobe wardrobe in FindObjectsOfType<CosmeticWardrobe>()) {
                AccessTools.Method(wardrobe.GetType(), "HandleCosmeticsUpdated").Invoke(wardrobe, []);
            }
        }

        void OnEnable() {
            Log?.LogMessage($"{Name} - Enabled");
            if (!Patcher.IsPatched) {
                Patcher.ApplyPatches();
                CosmeticsController.instance?.GetCosmeticsPlayFabCatalogData();
                CosmeticsController.instance?.SetHideCosmeticsFromRemotePlayers(true);
                PokeWardrobes();
            }
        }

        void OnDisable() {
            Log?.LogMessage($"{Name} - Disabled");
            if (Patcher.IsPatched) {
                Patcher.RemovePatches();
                CosmeticsController.instance?.GetCosmeticsPlayFabCatalogData();
                CosmeticsController.instance?.SetHideCosmeticsFromRemotePlayers(false);
                PokeWardrobes();
            }
                
        }

    }
}
