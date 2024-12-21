using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;

namespace Cosmetx.Patches 
{
    [HarmonyPatch(typeof(CosmeticsController), nameof(CosmeticsController.Initialize))]
    internal class InitializePatch {
        private static void Postfix() {
            PlayFabClientAPI.GetUserInventory(new(), (GetUserInventoryResult result) => {
                Cosmetx.Log?.LogInfo($"Gained access to {result.Inventory.Count} items.");
            }, (PlayFabError err) => { });
        }
    }
}