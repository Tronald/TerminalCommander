using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using TerminalCommander.Patches;
using UnityEngine;
using UnityEngine.Networking;

using System;


namespace TerminalCommander
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Commander : BaseUnityPlugin
    {
        public const string modGUID = "Tronald.TerminalCommander";
        public const string modName = "TerminalCommander";
        public const string modVersion = "1.9.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Commander instance;

        public TerminalCommanderConfiguration Configs = new TerminalCommanderConfiguration();
       
        public bool StartOfRound = true;

        public DateTime LastJamEvent = new DateTime();
        public DateTime LastDoorEvent = new DateTime();
        public bool EmergencyTPUsed = false;

        internal ManualLogSource log;
        public AudioManager Audio;

        void Awake()
        {
            if (instance == null) { instance = this; }

            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            log.LogInfo($"{modName} is loaded!");

            Configs.Set_Configs(this);

            TerminalHotkeys.SetSource(this);
            TerminalCommands.SetSource(this);
            RoundManagerPatch.SetSource(this);
            ChatManagerPatch.SetSource(this);

            Audio = new AudioManager();
            Audio.LoadAudio();

            harmony.PatchAll(typeof(Commander));
            harmony.PatchAll(typeof(TerminalHotkeys));
            harmony.PatchAll(typeof(TerminalCommands));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(ChatManagerPatch));
        }
    }    
}
