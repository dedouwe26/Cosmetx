using System;
using System.Collections.Generic;
using System.Linq;
using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;

namespace Cosmetx.Patches
{
    [HarmonyPatch(typeof(PlayFabClientAPI), nameof(PlayFabClientAPI.GetUserInventory))]
    internal class UserInventoryPatch {
        private static bool Prefix(GetUserInventoryRequest request, Action<GetUserInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData, Dictionary<string, string> extraHeaders) {
            Cosmetx.Log?.LogDebug("Called!!!!!!!!!!!!!!!!!!!!!!!!!!");
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = CosmeticsController.instance.catalog },
                (GetCatalogItemsResult result) => {
                    GetUserInventoryResult getUserInventoryResult = new() {
                        Inventory = result.Catalog.Select((CatalogItem item) => {
                            ItemInstance instance = new() {
                                CatalogVersion = item.CatalogVersion,
                                ItemId = item.ItemId,
                                ItemClass = item.ItemClass,
                                DisplayName = item.DisplayName,
                                UnitCurrency = Cosmetx.CurrencyName
                            };

                            return instance;
                        }).ToList()
                    };
                    resultCallback(getUserInventoryResult);
			    },
            errorCallback);

            // Dont want to call the original.
            return false;
        }
    }
}