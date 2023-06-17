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
		public static void Postfix(CosmeticsController __instance, ref List<CosmeticsController.CosmeticItem>[] ___itemLists, ref List<CosmeticsController.CosmeticItem> ___unlockedCosmetics, ref List<CosmeticsController.CosmeticItem> ___unlockedHats, ref List<CosmeticsController.CosmeticItem> ___unlockedFaces, ref List<CosmeticsController.CosmeticItem> ___unlockedBadges, ref List<CosmeticsController.CosmeticItem> ___unlockedHoldable)
		{
			___unlockedCosmetics = __instance.allCosmetics;
			Logging.log.LogInfo("All cosmetics (unlocked): "+___unlockedCosmetics.ToString());
			foreach (CosmeticsController.CosmeticItem cosmeticItem in ___unlockedCosmetics)
			{
				object[] args = {cosmeticItem.itemName};
				Traverse.Create<CosmeticsController>().Method("UnlockItem", args);
			}
			__instance.UpdateShoppingCart();
		}
	}
}
