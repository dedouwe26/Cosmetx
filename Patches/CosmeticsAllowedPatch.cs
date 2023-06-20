using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using GorillaNetworking;
using Cosmetx;

namespace Cosmetx.Patches
{
    /// <summary>
    /// This is an example patch, made to demonstrate how to use Harmony. You should remove it if it is not used.
    /// </summary>
    [HarmonyPatch(typeof(CosmeticsController))]
    [HarmonyPatch("GetUserCosmeticsAllowed", MethodType.Normal)]
    class CosmeticsAllowedPatch
    {
        public static void Postfix(CosmeticsController __instance, ref string ___concatStringCosmeticsAllowed, ref List<CosmeticsController.CosmeticItem> ___unlockedCosmetics, ref List<CosmeticsController.CosmeticItem> ___unlockedHats, ref List<CosmeticsController.CosmeticItem> ___unlockedFaces, ref List<CosmeticsController.CosmeticItem> ___unlockedBadges, ref List<CosmeticsController.CosmeticItem> ___unlockedHoldable)
        {
            ___unlockedCosmetics = __instance.allCosmetics;
            foreach (CosmeticsController.CosmeticItem cosmeticItem in __instance.allCosmetics)
            {	
				Logging.log.LogInfo("Unlocking "+cosmeticItem.displayName);
				___concatStringCosmeticsAllowed +=cosmeticItem.itemName;
                switch (cosmeticItem.itemCategory)
                {
                    case CosmeticsController.CosmeticCategory.Hat:
                        if (!___unlockedHats.Contains(cosmeticItem))
                        {
                            ___unlockedHats.Add(cosmeticItem);
                            return;
                        }
                        break;
                    case CosmeticsController.CosmeticCategory.Badge:
                        if (!___unlockedBadges.Contains(cosmeticItem))
                        {
                            ___unlockedBadges.Add(cosmeticItem);
                            return;
                        }
                        break;
                    case CosmeticsController.CosmeticCategory.Face:
                        if (!___unlockedFaces.Contains(cosmeticItem))
                        {
                            ___unlockedFaces.Add(cosmeticItem);
                            return;
                        }
                        break;
                    case CosmeticsController.CosmeticCategory.Holdable:
                    case CosmeticsController.CosmeticCategory.Gloves:
                    case CosmeticsController.CosmeticCategory.Slingshot:
                        if (!___unlockedHoldable.Contains(cosmeticItem))
                        {
                            ___unlockedHoldable.Add(cosmeticItem);
                        }
                        break;
                    case CosmeticsController.CosmeticCategory.Count:
                    case CosmeticsController.CosmeticCategory.Set:
                        break;
                    default:
                        return;
                }
			}
			__instance.UpdateShoppingCart();
        }
    }
}
