using System.Collections.Generic;
using HarmonyLib;
using GorillaNetworking;

namespace Cosmetx.Patches
{
    /// <summary>
    /// This is an example patch, made to demonstrate how to use Harmony. You should remove it if it is not used.
    /// </summary>
    [HarmonyPatch(typeof(CosmeticsController))]
    [HarmonyPatch("GetUserCosmeticsAllowed", MethodType.Normal)]
    internal class CosmeticsAllowedPatch
    {

        private static bool Prefix
            (CosmeticsController __instance, ref List<CosmeticsController.CosmeticItem> ___unlockedCosmetics, ref List<CosmeticsController.CosmeticItem> ___unlockedHats, ref List<CosmeticsController.CosmeticItem> ___unlockedBadges,
            ref List<CosmeticsController.CosmeticItem> ___unlockedFaces, ref List<CosmeticsController.CosmeticItem> ___unlockedHoldable, ref List<CosmeticsController.CosmeticItem> ___allCosmetics, ref Dictionary<string, CosmeticsController.CosmeticItem> ___allCosmeticsDict, ref Dictionary<string, string> ___allCosmeticsItemIDsfromDisplayNamesDict,
            ref string ___concatStringCosmeticsAllowed, ref bool ___playedInBeta, ref int ___currencyBalance, ref List<CosmeticsController.CosmeticItem>[] ___itemLists)
        {
            new CosmetxController(__instance, ref ___unlockedCosmetics, ref ___unlockedHats, ref ___unlockedBadges,
            ref ___unlockedFaces, ref ___unlockedHoldable, ref ___allCosmetics, ref ___allCosmeticsDict, ref ___allCosmeticsItemIDsfromDisplayNamesDict,
            ref ___concatStringCosmeticsAllowed, ref ___playedInBeta, ref ___currencyBalance, ref ___itemLists).GetUserCosmeticsAllowed();
            return false;
        }
    }
    [HarmonyPatch(typeof(CosmeticsController))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class CosmeticsAwakeningPatch {
        private static void Postfix (CosmeticsController __instance, ref List<CosmeticsController.CosmeticItem>[] ___itemLists, ref List<CosmeticsController.CosmeticItem> ___unlockedHats, ref List<CosmeticsController.CosmeticItem> ___unlockedBadges,
            ref List<CosmeticsController.CosmeticItem> ___unlockedFaces, ref List<CosmeticsController.CosmeticItem> ___unlockedHoldable) 
        {
            ___itemLists[0] = ref ___unlockedHats;
		    ___itemLists[1] = ref ___unlockedFaces;
		    ___itemLists[2] = ref ___unlockedBadges;
		    ___itemLists[3] = ref ___unlockedHoldable;
        }
    }
}