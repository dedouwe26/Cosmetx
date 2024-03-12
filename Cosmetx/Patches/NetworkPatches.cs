using System;
using Photon.Pun;
using HarmonyLib;

namespace Cosmetx
{
    [HarmonyPatch(typeof(PhotonView), "RPC", new Type[] { typeof(string), typeof(RpcTarget), typeof(object[]) })]
    internal class PhotonViewPatch
    {
        private static bool Prefix(PhotonView __instance, ref string methodName)
        {
            if (methodName == "UpdateCosmeticsWithTryon" || methodName == "UpdatePlayerCosmetic")
            {
                return false;
            }
            return true;
        }
    }
}
