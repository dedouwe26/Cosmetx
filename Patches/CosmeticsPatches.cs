using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using GorillaNetworking;
using Cosmetx;
using System.Threading;
using System.Threading.Tasks;
using PlayFab.ClientModels;
using PlayFab;

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
            ref string ___concatStringCosmeticsAllowed, ref bool ___playedInBeta, ref int ___currencyBalance)
        {
            CosmetxController cc = new CosmetxController(__instance, ref ___unlockedCosmetics, ref ___unlockedHats, ref ___unlockedBadges,
            ref ___unlockedFaces, ref ___unlockedHoldable, ref ___allCosmetics, ref ___allCosmeticsDict, ref ___allCosmeticsItemIDsfromDisplayNamesDict,
            ref ___concatStringCosmeticsAllowed, ref ___playedInBeta, ref ___currencyBalance);
            cc.GetUserCosmeticsAllowed();
            return false;
        }
    }
}