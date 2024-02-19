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
        public static string EmergencyTpStartMessage = "Emergency Teleport Started...";
        public static string EmergencyTpEndMessage = "Emergency Teleport Complete...";

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
                if (nameOfUserWhoTyped == "" && chatMessage == EmergencyTpStartMessage)
                {
                    if (commanderSource.EmergencyTPInUse) { return; }//blocks double call on server.
                    Terminal t = FindActiveObject<Terminal>();
                    ShipTeleporter[] teleporters = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();

                    logSource.LogInfo($"Setting emergency TP count.");
                    commanderSource.EmergencyTPCount++;

                    //IF HOST
                    t.terminalAudio.PlayOneShot(commanderSource.Audio.emergencyAudio);
                    logSource.LogInfo($"Emergency TP in use: true.");
                    commanderSource.EmergencyTPInUse = true;

                }
                if (nameOfUserWhoTyped == "" && chatMessage == EmergencyTpEndMessage)
                {
                    logSource.LogInfo($"Emergency TP in use: false.");
                    commanderSource.EmergencyTPInUse = false;

                }
            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogError($"Sync Config Error: {ex.Message}");
            };
        }

        static T FindActiveObject<T>() where T : UnityEngine.Object
        {
            T[] unityObjects = UnityEngine.Object.FindObjectsOfType<T>();

            if (unityObjects.Length > 0)
            {
                // For simplicity, just return the first found.
                //May need to expand later if first object is not desired object.
                return unityObjects[0];
            }

            return null;
        }

    }
}
