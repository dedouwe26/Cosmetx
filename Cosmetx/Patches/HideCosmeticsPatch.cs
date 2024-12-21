using GorillaNetworking;
using HarmonyLib;

namespace Cosmetx.Patches 
{
    [HarmonyPatch(typeof(CosmeticsController), nameof(CosmeticsController.SetHideCosmeticsFromRemotePlayers))]
    internal class HideCosmeticsPatch {
        private static bool Prefix(ref bool hideCosmetics) {
            hideCosmetics = true;
            return true;
        }
    }
}