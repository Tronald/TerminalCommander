using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine.Diagnostics;

namespace TerminalCommander.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;
       
        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
           
        }

        [HarmonyPatch("SetLevelObjectVariables")]
        [HarmonyPostfix]
        static void ResetVariable()
        {
            try
            {
                if (!commanderSource.Configs.SyncHost) { return; } // Host has disabled gameplay sync

                commanderSource.StartOfRound = true;
                commanderSource.LastJamEvent = new DateTime();
                commanderSource.LastDoorEvent = new DateTime();
                commanderSource.EmergencyTPCount = 0;

                //SYNC SETTINGS IF SYNC TURNED ON
                if (RoundManager.Instance.IsHost)
                {                  
                    HUDManager.Instance.AddTextToChatOnServer(commanderSource.Configs.SyncMessage());
                }
            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogError($"ROUND MANAGER ERROR: {ex.Message}");
            };
        }
 
    }
}
