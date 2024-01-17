using BepInEx.Logging;
using HarmonyLib;
using System;

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
                commanderSource.StartOfRound = true;
                commanderSource.LastJamEvent = new DateTime();
                commanderSource.LastDoorEvent = new DateTime();
            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogError($"ROUND MANAGER ERROR: {ex.Message}");
            };
        }
 
    }
}
