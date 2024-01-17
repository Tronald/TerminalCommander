using BepInEx.Logging;
using HarmonyLib;
using LethalNetworkAPI;
using System;

namespace TerminalCommander.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class GameManagement
    {
        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;
        private static LethalClientMessage<TerminalCommanderConfiguration> clientMessage = new LethalClientMessage<TerminalCommanderConfiguration>("config");
        private static LethalServerMessage<TerminalCommanderConfiguration> serverMessage = new LethalServerMessage<TerminalCommanderConfiguration>("config");
        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
            clientMessage.OnReceived += CustomClientMessage_OnReceived;
            serverMessage.OnReceived += ServerMessage_OnReceived;
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
                    serverMessage.SendAllClients(commanderSource.Configs);
                }
            }
            catch (Exception ex)
            {
                //Configs not synced
                logSource.LogError($"GAME MANAGEMENT ERROR (CONFIGS NOT SYNCED): {ex.Message}");
            };
        }
        private static void CustomClientMessage_OnReceived(TerminalCommanderConfiguration obj)
        {
            logSource.LogInfo($"Host configurations received.");
            commanderSource.Configs.AllowBigDoors = obj.AllowBigDoors;
            commanderSource.Configs.AllowJamming = obj.AllowJamming;
        }

        private static void ServerMessage_OnReceived(TerminalCommanderConfiguration arg1, ulong arg2)
        {
            logSource.LogInfo($"Sending host configurations to clients.");
            clientMessage.SendServer(commanderSource.Configs);
        }
    }

}
