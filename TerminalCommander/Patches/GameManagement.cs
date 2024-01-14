using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using Unity.Netcode;

namespace TerminalCommander.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class GameManagement
    {
        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;

        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
        }

        [HarmonyPatch("OnPlayerConnectedClientRpc")]
        [HarmonyPostfix]
        static void FollowHostConfigurationPatch(StartOfRound __instance, ulong clientId)
        {
            try
            {
                //Sync Configs
                logSource.LogInfo($"{Commander.modName} syncing configurations for new player.");
                commanderSource.NetworkHandler.SyncConfigs(commanderSource.Configs);

            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogInfo($"{Commander.modName} GAME MANAGEMENT ERROR (CONFIGS NOT SYNCED): {ex.Message}");
            };
        }

        //[HarmonyPatch("PlayerLoadedServerRpc")]
        //[HarmonyPostfix]
        //static void PushHostConfigurationPatch(StartOfRound __instance, ulong clientId)
        //{
        //    try
        //    {              
        //        //Sync Configs
        //        logSource.LogInfo($"{Commander.modName} syncing configurations to server.");
        //        commanderSource.NetworkHandler.SyncConfigs(commanderSource.Configs);

        //    }
        //    catch (Exception ex)
        //    {
        //        //Configs not synced
        //        logSource.LogInfo($"{Commander.modName} GAME MANAGEMENT ERROR (CONFIGS NOT SYNCED TO SERVER): {ex.Message}");
        //    };
        //}

    }
}
