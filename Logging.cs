namespace Cosmetx
{
    public static class Logging
    {
        public static BepInEx.Logging.ManualLogSource log;
        public static void init() 
        {
            log = BepInEx.Logging.Logger.CreateLogSource("Cosmetx");
        }
    }
}
