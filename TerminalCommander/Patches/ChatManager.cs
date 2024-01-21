using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalCommander.Patches
{
    /// <summary>
    /// Used to pass and read chat commands sent/received for syncing settings.
    /// This allows us to work around server requirements that would otherwise block vanilla compatibility. 
    /// This is hacky, but works well
    /// </summary>
    [HarmonyPatch(typeof(HUDManager))]
    internal class ChatManagerPatch
    {
        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;
        public static string EmergencyTpMessage = "Emergency Teleport Started...";

        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
        }

        [HarmonyPatch("AddChatMessage")]
        [HarmonyPostfix]
        static void SyncConfigs(string chatMessage, string nameOfUserWhoTyped = "")
        {
            try
            {
                if (nameOfUserWhoTyped=="" && chatMessage.StartsWith("tsync") && !RoundManager.Instance.IsHost)
                {
                    logSource.LogInfo($"Syncing host configurations {chatMessage}");
                    commanderSource.Configs.Set_Configs(chatMessage.Trim());
                    
                }
                if (nameOfUserWhoTyped == "" && chatMessage == EmergencyTpMessage)
                {
                    logSource.LogInfo($"Setting emergency TP used.");
                    commanderSource.EmergencyTPUsed = true;

                }
            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogError($"Sync Config Error: {ex.Message}");
            };
        }

    }
}
