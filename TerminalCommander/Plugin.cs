using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using TerminalCommander.Patches;
using BepInEx.Bootstrap;
using System.Security.Cryptography;
using System;


namespace TerminalCommander
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Commander : BaseUnityPlugin
    {
        public const string modGUID = "Tronald.TerminalCommander";
        public const string modName = "TerminalCommander";
        public const string modVersion = "1.8.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Commander instance;

        public TerminalCommanderConfiguration Configs = new TerminalCommanderConfiguration();
       
        public bool StartOfRound = true;

        public DateTime LastJamEvent = new DateTime();
        public DateTime LastDoorEvent = new DateTime();

        internal ManualLogSource log;

        void Awake()
        {
            if (instance == null) { instance = this; }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            log.LogInfo($"{modName} is loaded!");

            Configs.Set_Configs(this);           

            TerminalHotkeys.SetSource(this);
            TerminalCommands.SetSource(this);
            GameManagement.SetSource(this);
            RoundManagerPatch.SetSource(this);

            harmony.PatchAll(typeof(Commander));
            harmony.PatchAll(typeof(TerminalHotkeys));
            harmony.PatchAll(typeof(TerminalCommands));
            harmony.PatchAll(typeof(GameManagement));
            harmony.PatchAll(typeof (RoundManagerPatch));
        }      
    }    
}
