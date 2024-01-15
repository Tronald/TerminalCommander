﻿using BepInEx.Logging;
using HarmonyLib;
using LethalNetworkAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TerminalApi.Classes;
using Unity.Netcode;

namespace TerminalCommander.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class GameManagement
    {
        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;
        private static LethalClientMessage<TerminalCommanderConfiguration> customClientMessage = new LethalClientMessage<TerminalCommanderConfiguration>("config");

        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
            customClientMessage.OnReceived += CustomClientMessage_OnReceived;
        }

      

        [HarmonyPatch("OnClientConnect")]
        [HarmonyPostfix]
        static void FollowHostConfigurationPatch(StartOfRound __instance, ulong clientId)
        {
            try
            {
                if(__instance.IsHost)
                {
                    logSource.LogInfo($"{Commander.modName} syncing configurations for connected player: clientId {clientId}.");
                    customClientMessage.SendServer(commanderSource.Configs);
                }

                //Sync Configs
             
               // commanderSource.NetworkHandler.SyncConfigs();

            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogInfo($"{Commander.modName} GAME MANAGEMENT ERROR (CONFIGS NOT SYNCED): {ex.Message}");
            };
        }
        private static void CustomClientMessage_OnReceived(TerminalCommanderConfiguration obj)
        {
            logSource.LogInfo($"{Commander.modName} syncing configurations.");
            commanderSource.Configs.AllowBigDoors = obj.AllowBigDoors;
            commanderSource.Configs.AllowJamming = obj.AllowJamming;
        }
    }

}
