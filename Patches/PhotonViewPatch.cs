using System;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using HarmonyLib;
using GorillaNetworking;

namespace Cosmetx.Patches
{
	/// <summary>
	/// This is an example patch, made to demonstrate how to use Harmony. You should remove it if it is not used.
	/// </summary>
	[HarmonyPatch(typeof(PhotonView), nameof(PhotonView.RPC), MethodType.Normal)]
	internal class PhotonViewPatch
	{
		private static bool Prefix(PhotonView __instance, ref string methodName)
		{
			if (methodName=="UpdateCosmeticsWithTryon" || methodName=="UpdatePlayerCosmetic") {
                return false;
            }
			return true;
		}
	}
}
