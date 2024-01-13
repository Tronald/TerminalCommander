using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using TerminalCommander.Patches;
using BepInEx.Bootstrap;
using System.Security.Cryptography;


namespace TerminalCommander
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Commander : BaseUnityPlugin
    {
        public const string modGUID = "Tronald.TerminalCommander";
        public const string modName = "TerminalCommander";
        public const string modVersion = "1.7.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Commander instance;

        public static TerminalCommanderConfiguration Configs = new TerminalCommanderConfiguration();

        internal ManualLogSource log;

        void Awake()
        {
            if (instance == null) { instance = this; }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            log.LogInfo($"{modName} is loaded!");

            //Set_Configs();

            harmony.PatchAll(typeof(Commander));
            harmony.PatchAll(typeof(TerminalHotkeys));
            harmony.PatchAll(typeof(TerminalCommands));

            TerminalHotkeys.SetSource(this);
            TerminalCommands.SetSource(this);
        }      
    }    
}
