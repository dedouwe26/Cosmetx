using System.Collections.Generic;
using HarmonyLib;
using GorillaNetworking;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Cosmetx.Patches
{
    /// <summary>
    /// This is an example patch, made to demonstrate how to use Harmony. You should remove it if it is not used.
    /// </summary>
    [HarmonyPatch(typeof(CosmeticsController))]
    [HarmonyPatch("GetUserCosmeticsAllowed", MethodType.Normal)]
    internal class CosmeticsAllowedPatch
    {
        private class CosmetxController
        {
            CosmeticsController instance;
            List<CosmeticsController.CosmeticItem> unlockedCosmetics;
            List<CosmeticsController.CosmeticItem> unlockedHats;
            List<CosmeticsController.CosmeticItem> unlockedBadges;
            List<CosmeticsController.CosmeticItem> unlockedFaces;
            List<CosmeticsController.CosmeticItem> unlockedHoldable;
            List<CosmeticsController.CosmeticItem> allCosmetics;
            Dictionary<string, CosmeticsController.CosmeticItem> allCosmeticsDict;
            Dictionary<string, string> allCosmeticsItemIDsfromDisplayNamesDict;
            string concatStringCosmeticsAllowed;
            bool playedInBeta;
            int currencyBalance;
            List<CosmeticsController.CosmeticItem>[] itemLists;
            int searchIndex;
            CosmeticsController.CosmeticItem tempItem;

            internal CosmetxController(CosmeticsController instance, ref List<CosmeticsController.CosmeticItem> unlockedCosmetics, ref List<CosmeticsController.CosmeticItem> unlockedHats, ref List<CosmeticsController.CosmeticItem> unlockedBadges,
                ref List<CosmeticsController.CosmeticItem> unlockedFaces, ref List<CosmeticsController.CosmeticItem> unlockedHoldable, ref List<CosmeticsController.CosmeticItem> allCosmetics, ref Dictionary<string, CosmeticsController.CosmeticItem> allCosmeticsDict,
                ref Dictionary<string, string> allCosmeticsItemIDsfromDisplayNamesDict, ref string concatStringCosmeticsAllowed, ref bool playedInBeta, ref int currencyBalance, ref List<CosmeticsController.CosmeticItem>[] itemLists)
            {
                this.instance = instance;
                this.unlockedCosmetics = unlockedCosmetics;
                this.unlockedHats = unlockedHats;
                this.unlockedBadges = unlockedBadges;
                this.unlockedFaces = unlockedFaces;
                this.unlockedHoldable = unlockedHoldable;
                this.allCosmetics = allCosmetics;
                this.allCosmeticsDict = allCosmeticsDict;
                this.allCosmeticsItemIDsfromDisplayNamesDict = allCosmeticsItemIDsfromDisplayNamesDict;
                this.concatStringCosmeticsAllowed = concatStringCosmeticsAllowed;
                this.playedInBeta = playedInBeta;
                this.currencyBalance = currencyBalance;
                this.itemLists = itemLists;
            }
            internal void GetUserCosmeticsAllowed()
            {
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), delegate (GetUserInventoryResult result)
                {
                    PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
                    {
                        CatalogVersion = CosmeticsController.instance.catalog
                    }, delegate (GetCatalogItemsResult result2)
                    {
                        this.unlockedCosmetics.Clear();
                        this.unlockedHats.Clear();
                        this.unlockedBadges.Clear();
                        this.unlockedFaces.Clear();
                        this.unlockedHoldable.Clear();
                        List<CatalogItem> catalogItems = result2.Catalog;
                        using (List<CatalogItem>.Enumerator enumerator = catalogItems.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                CatalogItem catalogItem = enumerator.Current;
                                this.searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => catalogItem.DisplayName == x.displayName);
                                if (this.searchIndex > -1)
                                {
                                    string[] tempStringArray = null;
                                    bool hasPrice = false;
                                    if (catalogItem.Bundle != null)
                                    {
                                        tempStringArray = catalogItem.Bundle.BundledItems.ToArray();
                                    }
                                    uint cost;
                                    if (catalogItem.VirtualCurrencyPrices.TryGetValue(CosmeticsController.instance.currencyName, out cost))
                                    {
                                        hasPrice = true;
                                    }
                                    this.allCosmetics[this.searchIndex] = new CosmeticsController.CosmeticItem
                                    {
                                        itemName = catalogItem.ItemId,
                                        displayName = catalogItem.DisplayName,
                                        cost = (int)cost,
                                        itemPicture = this.allCosmetics[this.searchIndex].itemPicture,
                                        itemCategory = this.allCosmetics[this.searchIndex].itemCategory,
                                        bundledItems = tempStringArray,
                                        canTryOn = hasPrice,
                                        bothHandsHoldable = this.allCosmetics[this.searchIndex].bothHandsHoldable,
                                        overrideDisplayName = this.allCosmetics[this.searchIndex].overrideDisplayName
                                    };
                                    this.allCosmeticsDict[this.allCosmetics[this.searchIndex].itemName] = this.allCosmetics[this.searchIndex];
                                    this.allCosmeticsItemIDsfromDisplayNamesDict[this.allCosmetics[this.searchIndex].displayName] = this.allCosmetics[this.searchIndex].itemName;
                                }
                            }
                        }
                        for (int i = this.allCosmetics.Count - 1; i > -1; i--)
                        {
                            this.tempItem = this.allCosmetics[i];
                            if (this.tempItem.itemCategory == CosmeticsController.CosmeticCategory.Set && this.tempItem.canTryOn)
                            {
                                string[] bundledItems = this.tempItem.bundledItems;
                                for (int j = 0; j < bundledItems.Length; j++)
                                {
                                    string setItemName = bundledItems[j];
                                    this.searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => setItemName == x.itemName);
                                    if (this.searchIndex > -1)
                                    {
                                        this.tempItem = new CosmeticsController.CosmeticItem
                                        {
                                            itemName = this.allCosmetics[this.searchIndex].itemName,
                                            displayName = this.allCosmetics[this.searchIndex].displayName,
                                            cost = this.allCosmetics[this.searchIndex].cost,
                                            itemPicture = this.allCosmetics[this.searchIndex].itemPicture,
                                            itemCategory = this.allCosmetics[this.searchIndex].itemCategory,
                                            overrideDisplayName = this.allCosmetics[this.searchIndex].overrideDisplayName,
                                            bothHandsHoldable = this.allCosmetics[this.searchIndex].bothHandsHoldable,
                                            canTryOn = true
                                        };
                                        this.allCosmetics[this.searchIndex] = this.tempItem;
                                        this.allCosmeticsDict[this.allCosmetics[this.searchIndex].itemName] = this.allCosmetics[this.searchIndex];
                                        this.allCosmeticsItemIDsfromDisplayNamesDict[this.allCosmetics[this.searchIndex].displayName] = this.allCosmetics[this.searchIndex].itemName;
                                    }
                                }
                            }
                        }
                        this.searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => "Slingshot" == x.itemName);
                        this.allCosmeticsDict["Slingshot"] = this.allCosmetics[this.searchIndex];
                        this.allCosmeticsItemIDsfromDisplayNamesDict[this.allCosmetics[this.searchIndex].displayName] = this.allCosmetics[this.searchIndex].itemName;
                        foreach (CosmeticsController.CosmeticItem cosmeticItem in this.allCosmetics)
                        {
                            if (cosmeticItem.itemName == "null" || cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Set)
                            {
                                continue;
                            }
                            this.unlockedCosmetics.Add(cosmeticItem);
                            if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Hat && !this.unlockedHats.Contains(cosmeticItem))
    						{
    							this.unlockedHats.Add(cosmeticItem);
    						}
    						else if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Face && !this.unlockedFaces.Contains(cosmeticItem))
    						{
    							this.unlockedFaces.Add(cosmeticItem);
    						}
    						else if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Badge && !this.unlockedBadges.Contains(cosmeticItem))
    						{
    							this.unlockedBadges.Add(cosmeticItem);
    						}
    						else if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Skin && !this.unlockedBadges.Contains(cosmeticItem))
    						{
    							this.unlockedBadges.Add(cosmeticItem);
    						}
    						else if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Holdable && !this.unlockedHoldable.Contains(cosmeticItem))
    						{
    							this.unlockedHoldable.Add(cosmeticItem);
    						}
    						else if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Gloves && !this.unlockedHoldable.Contains(cosmeticItem))
    						{
    							this.unlockedHoldable.Add(cosmeticItem);
    						}
    						else if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Slingshot && !this.unlockedHoldable.Contains(cosmeticItem))
    						{
    							this.unlockedHoldable.Add(cosmeticItem);
    						}
                            this.concatStringCosmeticsAllowed += cosmeticItem.itemName;
                        }
                        foreach (CosmeticStand cosmeticStand in this.instance.cosmeticStands)
                        {
                            if (cosmeticStand != null)
                            {
                                cosmeticStand.InitializeCosmetic();
                            }
                        }
                        this.currencyBalance = result.VirtualCurrency[this.instance.currencyName];
                        int num;
                        this.playedInBeta = (result.VirtualCurrency.TryGetValue("TC", out num) && num > 0);
                        this.instance.currentWornSet.LoadFromPlayerPreferences(this.instance);
                        this.instance.SwitchToStage(CosmeticsController.ATMStages.Begin);
                        this.instance.ProcessPurchaseItemState(null, false);
                        this.instance.UpdateShoppingCart();
                        this.instance.UpdateCurrencyBoard();
                    }, (PlayFabError a) =>
                    {
                    });
                }, (PlayFabError a) =>
                {
                });
            }
        }

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
    internal class CosmeticsAwakeningPatch
    {
        private static bool Prefix(CosmeticsController __instance, ref int[] ___cosmeticsPages, ref string ___catalog, ref string ___currencyName, ref CosmeticsController.CosmeticItem ___nullItem,
            ref List<CosmeticsController.CosmeticItem> ___allCosmetics, ref Dictionary<string, CosmeticsController.CosmeticItem> ___allCosmeticsDict, ref Dictionary<string, string> ___allCosmeticsItemIDsfromDisplayNamesDict,
            ref CosmeticsController.CosmeticSet ___tryOnSet, ref List<CosmeticsController.CosmeticItem> ___unlockedHats, ref List<CosmeticsController.CosmeticItem> ___unlockedBadges,
            ref List<CosmeticsController.CosmeticItem> ___unlockedFaces, ref List<CosmeticsController.CosmeticItem> ___unlockedHoldable, ref List<CosmeticsController.CosmeticItem>[] ___itemLists)
        {
            if (CosmeticsController.instance == null)
            {
                CosmeticsController.instance = __instance;
            }
            else if (CosmeticsController.instance != __instance)
            {
                Object.Destroy(__instance.gameObject);
            }
            if (__instance.gameObject.activeSelf)
            {
                ___catalog = "DLC";
                ___currencyName = "SR";
                ___nullItem = ___allCosmetics[0];
                ___nullItem.isNullItem = true;
                ___allCosmeticsDict[___nullItem.itemName] = ___nullItem;
                ___allCosmeticsItemIDsfromDisplayNamesDict[___nullItem.displayName] = ___nullItem.itemName;
                for (int i = 0; i < 10; i++)
                {
                    ___tryOnSet.items[i] = ___nullItem;
                }
                ___cosmeticsPages[0] = 0;
                ___cosmeticsPages[1] = 0;
                ___cosmeticsPages[2] = 0;
                ___cosmeticsPages[3] = 0;
                ___itemLists[0] = new List<CosmeticsController.CosmeticItem>();
                ___itemLists[1] = new List<CosmeticsController.CosmeticItem>();
                ___itemLists[2] = new List<CosmeticsController.CosmeticItem>();
                ___itemLists[3] = new List<CosmeticsController.CosmeticItem>();
                __instance.SwitchToStage(CosmeticsController.ATMStages.Unavailable);
                __instance.StartCoroutine((IEnumerator<object>)(AccessTools.Method(typeof(CosmeticsController), "CheckCanGetDaily", null, null).Invoke(__instance, null)));
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("IsItemAllowed", MethodType.Normal)]
    internal class ItemAllowedPatch
    {
        private static bool Prefix(VRRig __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("GetUserCosmeticsAllowed", MethodType.Normal)]
    internal class GetUserCosmeticsAllowedVRRig
    {
        
        private class GetUserCosmeticsVRRig
        {
            VRRig instance;
            string concatStringOfCosmeticsAllowed;
            bool initializedCosmetics;
            internal GetUserCosmeticsVRRig(VRRig instance, ref string concatStringOfCosmeticsAllowed, ref bool initializedCosmetics)
            {
                this.instance = instance;
                this.concatStringOfCosmeticsAllowed = concatStringOfCosmeticsAllowed;
                this.initializedCosmetics = initializedCosmetics;
            }
            internal void GetUserCosmeticsAllowedVRRig()
            {
                if (CosmeticsController.instance != null)
                {
                    PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest (), delegate (GetCatalogItemsResult result)
                    {
                        foreach (CatalogItem itemInstance in result.Catalog)
                        {
                            if (itemInstance.CatalogVersion == CosmeticsController.instance.catalog)
                            {
                                this.concatStringOfCosmeticsAllowed += itemInstance.ItemId;
                            }
                        }
                        AccessTools.Method(typeof(VRRig), "CheckForEarlyAccess").Invoke(this.instance, null);
                        this.instance.SetCosmeticsActive();
                    }, delegate (PlayFabError error)
                    {
                        Debug.Log("Got error retrieving user data:");
                        Debug.Log(error.GenerateErrorReport());
                        this.initializedCosmetics = true;
                        this.instance.SetCosmeticsActive();
                    }, null, null);
                }
                this.concatStringOfCosmeticsAllowed += "Slingshot";
            }
        }
        private static bool Prefix(VRRig __instance, ref string ___concatStringOfCosmeticsAllowed, ref bool ___initializedCosmetics)
        {
            new GetUserCosmeticsVRRig(__instance, ref ___concatStringOfCosmeticsAllowed, ref ___initializedCosmetics).GetUserCosmeticsAllowedVRRig();
            return false;
        }
    }
}
