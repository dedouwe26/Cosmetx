using System.Reflection;
using BepInEx;

using HarmonyLib;

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
    /// mod's main class
    /// </summary>

    [BepInPlugin(HarmonyPatches.InstanceId, "Cosmetx", "1.2.1")]
    public class Cosmetx : BaseUnityPlugin
    {
        public static bool isUnlocked = false;

        void Awake()
        {
            BepInEx.Logging.Logger.Sources.Remove(Logger);
            Logging.init();
        }

        void OnEnable()
        {
            Logging.log.LogInfo("Plugin is enabled");
            Logging.log.LogMessage("Patching Now...");
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

    }
}
