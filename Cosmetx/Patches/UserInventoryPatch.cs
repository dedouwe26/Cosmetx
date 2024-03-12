using System;
using System.Collections.Generic;
using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;

namespace Cosmetx
{
    [HarmonyPatch(typeof(PlayFabClientAPI), nameof(PlayFabClientAPI.GetUserInventory))]
    internal class UserInventoryPatch
    {
        private static bool Prefix(GetUserInventoryRequest request, Action<GetUserInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData, Dictionary<string, string> extraHeaders) {
            Logging.log.LogInfo("Called!!!!!!!!!!!!!!!!!!!!!!!!!!");
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
			{
				CatalogVersion = CosmeticsController.instance.catalog
			}, delegate(GetCatalogItemsResult result)
			{
                GetUserInventoryResult getUserInventoryResult = new GetUserInventoryResult
                {
                    Inventory = (from x in result.Catalog
                                 select new ItemInstance
                                 {
                                     CatalogVersion = x.CatalogVersion,
                                     ItemId = x.ItemId,
                                     ItemClass = x.ItemClass,
                                     DisplayName = x.DisplayName,
                                     ItemInstanceId = default(Guid).ToString()
                                 }).ToList()
                };
                resultCallback(getUserInventoryResult);
			}, errorCallback, null, null);
            return false;
        }
    }
}