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
    class Test {
        private void Tast(string a) {
            Logging.log.LogInfo("YESSSSSSSSSSSSSSSSSSSSSSSSSSSSSS "+a);
        }
    }
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

    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin("com.dedouwe26.gorillatag.cosmetx", "Cosmetx", "1.0.0")]
    public class Cosmetx : BaseUnityPlugin
    {   
        public static CosmeticsController cosmeticsControllerInstance;

        void Awake()
        {   
            BepInEx.Logging.Logger.Sources.Remove(Logger);
            Logging.init();
        }
        void Start()
        {   
            // NOT HERE
            // Utilla.Events.GameInitialized += OnGameInitialized;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnEnable()
        {   
            Logging.log.LogInfo("Plugin is enabled");
            object[] args = {"test"};
            Traverse.Create(typeof(Test)).Method("Tast", args);
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            Thread.Sleep(1000);
            cosmeticsControllerInstance.GetUserCosmeticsAllowed();
        }

        // void OnGameInitialized(object sender, EventArgs e)
		// {
			
		// }

        void OnSceneLoaded(Scene s, LoadSceneMode sm)
        {   
            if (s.name == "GorillaTagSJR") {
                Logging.log.LogMessage("Patching Now...");
                HarmonyPatches.ApplyHarmonyPatches();
                cosmeticsControllerInstance = GameObject.Find("Global/Photon Manager/CosmeticsController").GetComponent<CosmeticsController>();
                Thread.Sleep(1000);
                cosmeticsControllerInstance.GetUserCosmeticsAllowed();
                Logging.log.LogInfo(cosmeticsControllerInstance.allCosmetics);
            }
        }

        void Update()
        {
        }
    }
}
