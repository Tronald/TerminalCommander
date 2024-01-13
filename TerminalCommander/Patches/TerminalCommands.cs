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

           
        }
        private static string OnHotKeyHelpCommand()
        {
            return "HOTKEYS\n\n" +
                ">Ctrl+D\nOpen / close all doors.\n\n" +
                ">Ctrl+J\nJam all turrets and land mines.\n\n" +
                ">Ctrl+M\nTurn monitor on and off.\n\n" +
                ">Ctrl+S\nQuickly switch players on the monitor.\n\n" +
                ">Ctrl+T\nBegin a signal transmission command.\n\n";
        }

      
    }
}
