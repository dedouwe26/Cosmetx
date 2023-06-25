using System.Collections.Generic;
using System.Threading.Tasks;
using GorillaNetworking;
using PlayFab;
using PlayFab.ClientModels;

namespace Cosmetx
{
    public class CosmetxController
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

        public CosmetxController(CosmeticsController instance, ref List<CosmeticsController.CosmeticItem> unlockedCosmetics, ref List<CosmeticsController.CosmeticItem> unlockedHats, ref List<CosmeticsController.CosmeticItem> unlockedBadges,
            ref List<CosmeticsController.CosmeticItem> unlockedFaces, ref List<CosmeticsController.CosmeticItem> unlockedHoldable, ref List<CosmeticsController.CosmeticItem> allCosmetics, ref Dictionary<string, CosmeticsController.CosmeticItem> allCosmeticsDict,
            ref Dictionary<string, string> allCosmeticsItemIDsfromDisplayNamesDict, ref string concatStringCosmeticsAllowed, ref bool playedInBeta, ref int currencyBalance)
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
        }
        public void GetUserCosmeticsAllowed()
        {
            int searchIndex = -1;
            this.unlockedCosmetics.Clear();
            this.unlockedHats.Clear();
            this.unlockedBadges.Clear();
            this.unlockedFaces.Clear();
            this.unlockedHoldable.Clear();
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
            {
                CatalogVersion = "DLC"
            }, (GetCatalogItemsResult catalog) =>
            {
                using (List<CatalogItem>.Enumerator enumerator = catalog.Catalog.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        CatalogItem catalogItem = enumerator.Current;
                        searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => catalogItem.DisplayName == x.displayName);
                        if (searchIndex > -1)
                        {
                            string[] tempStringArray = null;
                            bool hasPrice = false;
                            if (catalogItem.Bundle != null)
                            {
                                tempStringArray = catalogItem.Bundle.BundledItems.ToArray();
                            }
                            uint cost = 0;
                            if (catalogItem.VirtualCurrencyPrices.TryGetValue("SR", out cost))
                            {
                                hasPrice = true;
                            }
                            this.allCosmetics[searchIndex] = new CosmeticsController.CosmeticItem
                            {
                                itemName = catalogItem.ItemId,
                                displayName = catalogItem.DisplayName,
                                cost = (int)cost,
                                itemPicture = this.allCosmetics[searchIndex].itemPicture,
                                itemCategory = this.allCosmetics[searchIndex].itemCategory,
                                bundledItems = tempStringArray,
                                canTryOn = hasPrice,
                                bothHandsHoldable = this.allCosmetics[searchIndex].bothHandsHoldable,
                                overrideDisplayName = this.allCosmetics[searchIndex].overrideDisplayName
                            };
                            this.allCosmeticsDict[this.allCosmetics[searchIndex].itemName] = this.allCosmetics[searchIndex];
                            this.allCosmeticsItemIDsfromDisplayNamesDict[this.allCosmetics[searchIndex].displayName] = this.allCosmetics[searchIndex].itemName;
                        }
                    }
                }
            }, (PlayFabError a) =>
            {
            });
            for (int i = this.allCosmetics.Count - 1; i > -1; i--)
            {
                CosmeticsController.CosmeticItem tempItem = this.allCosmetics[i];
                if (tempItem.itemCategory == CosmeticsController.CosmeticCategory.Set && tempItem.canTryOn)
                {
                    string[] bundledItems = tempItem.bundledItems;
                    for (int j = 0; j < bundledItems.Length; j++)
                    {
                        string setItemName = bundledItems[j];
                        searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => setItemName == x.itemName);
                        if (searchIndex > -1)
                        {
                            tempItem = new CosmeticsController.CosmeticItem
                            {
                                itemName = this.allCosmetics[searchIndex].itemName,
                                displayName = this.allCosmetics[searchIndex].displayName,
                                cost = this.allCosmetics[searchIndex].cost,
                                itemPicture = this.allCosmetics[searchIndex].itemPicture,
                                itemCategory = this.allCosmetics[searchIndex].itemCategory,
                                overrideDisplayName = this.allCosmetics[searchIndex].overrideDisplayName,
                                bothHandsHoldable = this.allCosmetics[searchIndex].bothHandsHoldable,
                                canTryOn = true
                            };
                            this.allCosmetics[searchIndex] = tempItem;
                            this.allCosmeticsDict[this.allCosmetics[searchIndex].itemName] = this.allCosmetics[searchIndex];
                            this.allCosmeticsItemIDsfromDisplayNamesDict[this.allCosmetics[searchIndex].displayName] = this.allCosmetics[searchIndex].itemName;
                        }
                    }
                }
            }
            searchIndex = this.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => "Slingshot" == x.itemName);
            this.allCosmeticsDict["Slingshot"] = this.allCosmetics[searchIndex];
            this.allCosmeticsItemIDsfromDisplayNamesDict[this.allCosmetics[searchIndex].displayName] = this.allCosmetics[searchIndex].itemName;
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
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult inv) =>
            {
                this.currencyBalance = inv.VirtualCurrency["SR"];
                int num = 0;
                this.playedInBeta = (inv.VirtualCurrency.TryGetValue("TC", out num) && num > 0);
            }, (PlayFabError a) =>
            {
            });
            this.instance.currentWornSet.LoadFromPlayerPreferences(this.instance);
            this.instance.SwitchToStage(CosmeticsController.ATMStages.Begin);
            this.instance.ProcessPurchaseItemState(null, false);
            this.instance.UpdateShoppingCart();
            this.instance.UpdateCurrencyBoard();
        }
    }
}