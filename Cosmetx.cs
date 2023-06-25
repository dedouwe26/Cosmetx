using System;
using System.Threading;
using System.Reflection;
using BepInEx;
using UnityEngine;
using GorillaNetworking;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace Cosmetx
{
    public class HarmonyPatches
    {
        private static Harmony instance;

        public static bool IsPatched { get; private set; }
        public const string InstanceId = "com.dedouwe26.gorillatag.cosmetx";

        internal static void ApplyHarmonyPatches()
        {
            if (instance == null)
            {
                instance = new Harmony(InstanceId);
            }
            if (!IsPatched)
            {
                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }

    /// <summary>
    /// mods main class
    /// </summary>

    [BepInPlugin("com.dedouwe26.gorillatag.cosmetx", "Cosmetx", "1.0.0")]
    public class Cosmetx : BaseUnityPlugin
    {
        public static CosmeticsController cosmeticsControllerInstance;
        public static bool isUnlocked = false;

        void Awake()
        {
            BepInEx.Logging.Logger.Sources.Remove(Logger);
            Logging.init();
        }
        void Start()
        {
            // NOT HERE
        }

        void OnEnable()
        {
            Logging.log.LogInfo("Plugin is enabled");
            Logging.log.LogMessage("Patching Now...");
            HarmonyPatches.ApplyHarmonyPatches();
            // SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            // SceneManager.sceneLoaded -= OnSceneLoaded;
            HarmonyPatches.RemoveHarmonyPatches();
            cosmeticsControllerInstance.GetUserCosmeticsAllowed();
        }

        // void OnSceneLoaded(Scene s, LoadSceneMode sm)
        // {
        //     if (s.name == "GorillaTagSJR")
        //     {
        //     }
        // }
    }
}
