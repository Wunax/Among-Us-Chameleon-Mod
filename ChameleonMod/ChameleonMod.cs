using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace ChameleonMod
{
    [BepInPlugin("org.bepinex.plugins.ChameleonMod", "Chameleon Mod", version)]
    public class ChameleonMod : BasePlugin
    {
        public const string version = "1.0";

        public static ManualLogSource log;
        private readonly Harmony harmony;

        public ChameleonMod()
        {
            ChameleonMod.log = base.Log;
            this.harmony = new Harmony("Chameleon Mod");
        }

        public override void Load()
        {
            ChameleonMod.log.LogMessage($"Chameleon Mod loaded - v{version} by Wunax");
            this.harmony.PatchAll();
        }
    }
}
