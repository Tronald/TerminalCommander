using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using BepInEx.Logging;
using UnityEngine.UI;
using System.Collections;
using TerminalCommander;
using System;

namespace TerminalCommander.Patches
{
    /// <summary>
    /// Patches in game terminal. Client side execution
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]

    internal class TerminalPatch
    {
        private static string turretName = "turretscript";
        private static string landmineName = "landmine";

        private static ManualLogSource logSource; // Log source field

        // Method to set the log source
        public static void SetLogSource(ManualLogSource log)
        {
            logSource = log;
        }


        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void HotKeyPatch(ref bool ___terminalInUse, ref TMP_InputField ___screenText)
        {
            try
            {
                if (___terminalInUse && BepInEx.UnityInput.Current.GetKey(KeyCode.LeftControl) || BepInEx.UnityInput.Current.GetKey(KeyCode.RightControl))
                {
                    Terminal t = FindActiveTerminal();
                    if (t == null)
                    {
                        logSource.LogInfo($"{Commander.modName} ERROR: Terminal could not be found.");
                    }
                    //Switch Hot Key
                    //Executes a monitor switch
                    if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.S))
                    {

                        TerminalNode tn = t.terminalNodes.specialNodes[20];
                        StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(callRPC: true);
                        t.LoadNewNode(tn);

                    }

                    //Transmit Hot Key
                    //Executes transmit text
                    else if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.T))
                    {
                        ___screenText.text += "transmit ";
                    }

                    //Jamming Hot Key
                    //Disable all turrets and mines
                    else if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.J))
                    {
                        TerminalAccessibleObject[] taos = (from x in UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>()
                                                           select x).ToArray();

                        List<string> items = new List<string>();

                        foreach (var tao in taos.Where(x => x.name.ToLower() == turretName || x.name.ToLower() == landmineName))
                        {
                            items.Add(tao.objectCode);
                            tao.CallFunctionFromTerminal();
                        }

                        SetTerminalText(t, "Jamming turrets and land mines");

                        logSource.LogInfo($"{Commander.modName} TerminalAccessibleObjects Called: Count{taos.Count()} - ({string.Join(", ", items)})");
                    }
                }
            }
            catch(Exception ex)
            {
                logSource.LogInfo($"{Commander.modName} ERROR: {ex.Message}");
            }
        }
        static void SetTerminalText(Terminal t, string s)
        {
            TerminalNode tn = new TerminalNode();
            tn.clearPreviousText = true;
            tn.acceptAnything = false;
            tn.displayText = "Jamming turrets and land mines.\n\n";
            t.LoadNewNode(tn);

            t.screenText.ActivateInputField();
            ((Selectable)t.screenText).Select();
        }    
        static Terminal FindActiveTerminal()
        {
            // Implement logic to find the active instance of the TerminalNode
            // This can be done through GameObject.Find, iterating through GameObjects, etc.

            // Example: finding by tag
            Terminal[] terminals = UnityEngine.Object.FindObjectsOfType<Terminal>();

            if (terminals.Length > 0)
            {
                // For simplicity, just return the first TerminalNode found.
                // You might want to implement more sophisticated logic based on your requirements.
                return terminals[0];
            }

            return null;
        }

    }
}
