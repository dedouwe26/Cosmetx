using System;
using System.Reflection;
using BepInEx;
using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;

namespace Cosmetx
{
    public class Patcher : IDisposable {
        private static Harmony? instance;

        public static bool IsPatched { get; private set; }

        // private static readonly Assembly[] assemblies = [typeof(CosmeticsController).Assembly, typeof(PlayFabClientAPI).Assembly]
        
        public const string InstanceId = "com.dedouwe26.gorillatag.cosmetx";
        internal static void ApplyPatches()
        {
            instance ??= new Harmony(InstanceId);
            if (!IsPatched)
            {
                instance.PatchAll(typeof(Cosmetx).Assembly);
                IsPatched = true;
            }
        }

        internal static void RemovePatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }

        public void Dispose() {
            RemovePatches();
        }
    }

    [BepInPlugin(Patcher.InstanceId, "Cosmetx", "1.2.2")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.14")]
    public class Cosmetx : BaseUnityPlugin {
        public static BepInEx.Logging.ManualLogSource? Log;
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
                        currencyName = (string?)CosmeticsController.instance.GetType().GetField("currencyName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(CosmeticsController.instance);
                    } catch { }
                }
                currencyName ??= "SR";
            }
            return currencyName;
        } }

        public void Awake() {
            Log = Logger;
            Log.LogMessage("Patching Now...");
            Patcher.ApplyPatches();
            CosmeticsController.instance?.SetHideCosmeticsFromRemotePlayers(true);
            enabled = true;
            // MethodInfo unlockItem = typeof(CosmeticsController).GetMethod("UnlockItem", BindingFlags.NonPublic | BindingFlags.Instance);
            // foreach (string cosmetic in controller.allCosmeticsDict.Keys) {
            //     unlockItem.Invoke(CosmeticsController.instance, new object[] {cosmetic});
            // }
            
        }

        public void OnEnable() {
            // Log?.LogInfo("Plugin is enabled ------------");

            // if (!Patcher.IsPatched) {
            //     Patcher.ApplyPatches();
            //     CosmeticsController.instance?.GetCosmeticsPlayFabCatalogData();
            //     CosmeticsController.instance?.SetHideCosmeticsFromRemotePlayers(true);
            // }
        }

        public void OnDisable() {
            // Log?.LogInfo("Plugin is disabled");
            // if (Patcher.IsPatched) {
            //     Patcher.RemovePatches();
            //     CosmeticsController.instance?.GetCosmeticsPlayFabCatalogData();
            //     CosmeticsController.instance?.SetHideCosmeticsFromRemotePlayers(false);
            // }
                
        }

    }
}
