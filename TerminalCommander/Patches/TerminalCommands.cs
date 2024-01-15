using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TerminalCommander.Patches
{
    /// <summary>
    /// Patches in game terminal. Client side execution
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]

    internal class TerminalCommands
    {
        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;
       
        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
            SetCommands();
        }
        static void SetCommands()
        {
            AddCommand("Commander", new CommandInfo
            {
                Category = "help",
                Description = "View the Terminal Commander hotkeys and commands.",
                DisplayTextSupplier = OnHotKeyHelpCommand
            });
            AddCommand("tp", new CommandInfo
            {
                Category = "Commander",              
                DisplayTextSupplier = TeleportCommand
            });
            AddCommand("itp", new CommandInfo
            {
                Category = "Commander",
                DisplayTextSupplier = InverseTeleportCommand
            });
            AddCommand("sync", new CommandInfo
            {
                Category = "Commander",
                DisplayTextSupplier = SyncCommand
            });
        }
        private static string OnHotKeyHelpCommand()
        {
            return "HOTKEYS\n\n" +
                ">Ctrl+D\nOpen / close all doors.\n\n" +
                ">Ctrl+J\nJam all turrets and land mines.\n\n" +
                ">Ctrl+M\nTurn monitor on and off.\n\n" +
                ">Ctrl+S\nQuickly switch players on the monitor.\n\n" +
                ">Ctrl+T\nBegin a signal transmission command.\n\n" +
                "COMMANDS\n\n" +
                ">TP\nTeleport currently viewed player on monitor.\n\n" +
                ">ITP\nActivate inverse teleporter.\n\n";

        }
        private static string TeleportCommand()
        {
            ShipTeleporter[] teleporters = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();
            if (teleporters!=null && teleporters.Length > 0)
            {           
               
                foreach(ShipTeleporter teleporter in teleporters)
                {
                    if (teleporter.isInverseTeleporter)
                    {
                        continue;
                    }
                    teleporter.PressTeleportButtonOnLocalClient();
                    return "Teleporting...\n\n";
                }
                
            }
            return "Nuh uh, no teleporter\n\n";
        }
        /// <summary>
        /// Entrance teleporter excample. Does not appear to be fully implemented
        /// in game
        /// </summary>
        /// <returns>string</returns>
        private static string EntranceTeleportCommand()
        {

            EntranceTeleport[] inverseteleporters = UnityEngine.Object.FindObjectsOfType<EntranceTeleport>();
            if (inverseteleporters == null || inverseteleporters.Length == 0)
            {
                return "No inverse teleporter detected.\n\n";
            }
            else
            {
                EntranceTeleport shipTeleporter = inverseteleporters[0];
                shipTeleporter.TeleportPlayer();
                return "Teleporting...\n\n";
            }
        }
        private static string InverseTeleportCommand()
        {
            ShipTeleporter[] teleporters = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();
            if (teleporters != null && teleporters.Length > 0)
            {
                foreach (ShipTeleporter teleporter in teleporters)
                {
                    if (!teleporter.isInverseTeleporter)
                    {
                        continue;
                    }

                    if (!StartOfRound.Instance.shipHasLanded)
                    {
                        return "Cannot inverse teleport until ship has fully landed and stabilized.\n\n";
                    }                
                    FieldInfo cooldownTime = teleporter.GetType().GetField("cooldownTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    float cooldown = (float)cooldownTime.GetValue(teleporter);
                  if(cooldown > 0)
                    {
                        return $"Cooldown time for inverse teleporter: {Math.Round(cooldown)} seconds.\n\n";
                    }
                    teleporter.PressTeleportButtonOnLocalClient();
                    return "Teleporting...\n\n";
                }

            }
            return "Nuh uh, no inverse teleporter\n\n";
        }

        private static string SyncCommand()
        {
            commanderSource.NetworkHandler.SyncConfigs();
            return "Synced...\n\n";
            
        }

    }
}
