using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TerminalCommander.Patches;


namespace TerminalCommander
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Commander : BaseUnityPlugin
    {
        public const string modGUID = "Tronald.TerminalCommander";
        public const string modName = "TerminalCommander";
        public const string modVersion = "1.3.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Commander instance;

        internal ManualLogSource log;

        void Awake()
        {
            if (instance == null) { instance = this; }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            log.LogInfo($"{modName} is loaded!");

            harmony.PatchAll(typeof(Commander));
            harmony.PatchAll(typeof(TerminalPatch));

            TerminalPatch.SetSource(this);
        }

    }
}
