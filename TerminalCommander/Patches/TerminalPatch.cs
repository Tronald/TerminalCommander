using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using BepInEx.Logging;
using UnityEngine.UI;
using TerminalApi;
using TerminalApi.Classes;
using static TerminalApi.TerminalApi;
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
        private static string doorName = "bigdoor";
        private static bool openDoors = true; //Used to close or open all doors

        private static ManualLogSource logSource; // Log source field
        private static Commander commanderSource;
        // Method to set the log source
        public static void SetSource(Commander source)
        {
            commanderSource = source;
            logSource = commanderSource.log;
            SetCommands();
        }

        private static TerminalKeyword CheckForExactSentences(Terminal t, string playerWord)
        {
            for (int i = 0; i < t.terminalNodes.allKeywords.Length; i++)
            {
                if (t.terminalNodes.allKeywords[i].word == playerWord)
                {
                    return t.terminalNodes.allKeywords[i];
                }
            }

            return null;
        }

      

      

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void HotKeyPatch(ref bool ___terminalInUse, ref TMP_InputField ___screenText)
        {
            try
            {
                if (___terminalInUse && BepInEx.UnityInput.Current.GetKey(KeyCode.LeftControl) || BepInEx.UnityInput.Current.GetKey(KeyCode.RightControl))
                {
                    Terminal t = FindActiveObject<Terminal>();
                    RoundManager r = FindRoundManager();

                    if (t == null)
                    {
                        logSource.LogInfo($"{Commander.modName} ERROR: Terminal could not be found.");
                    }

                    //if (r != null)
                    //{
                    //    logSource.LogInfo($"{Commander.modName} RoundManagerPower: {r.powerOffPermanently}");
                    //}

                    //Switch Hot Key
                    //Executes a monitor switch
                    if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.S))
                    {

                        //TerminalNode tn = t.terminalNodes.specialNodes[20];
                        //StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(callRPC: true);
                        //t.LoadNewNode(tn);

                        string cmd = "switch";

                        t.screenText.text += cmd;
                        t.textAdded = cmd.Length;

                        t.OnSubmit();

                    }

                    //Transmit Hot Key
                    //Executes transmit text
                    else if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.T))
                    {
                        ___screenText.text += "transmit ";
                    }



                    //Door Hot Key
                    //Open / Close all doors
                    else if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.D))
                    {
                        TerminalAccessibleObject[] taos = (from x in UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>()
                                                           select x).ToArray();

                        List<string> items = new List<string>();
                       
                        foreach (var tao in taos.Where(x => x.name.ToLower().Contains(doorName)))
                        {
                            if (!tao.isBigDoor) { return; } //Not a big door
                            items.Add(tao.objectCode);
                            tao.SetDoorLocalClient(openDoors);                              
                        }

                        if (openDoors)
                        {
                            SetTerminalText(t, "Opening all doors\n\n");
                            openDoors = false;
                        }
                        else
                        {
                            SetTerminalText(t, "Closing all doors\n\n");
                            openDoors = true;
                        }
                        t.terminalAudio.PlayOneShot(t.codeBroadcastSFX, 1f);
                        t.codeBroadcastAnimator.SetTrigger("display");
                        logSource.LogInfo($"{Commander.modName} TerminalAccessibleObjects Called: Count{taos.Count()} - ({string.Join(", ", items)})");
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

                        SetTerminalText(t, "Jamming turrets and land mines\n\n");
                       
                        t.terminalAudio.PlayOneShot(t.codeBroadcastSFX, 1f);
                        t.codeBroadcastAnimator.SetTrigger("display");

                        logSource.LogInfo($"{Commander.modName} TerminalAccessibleObjects Called: Count{taos.Count()} - ({string.Join(", ", items)})");
                    }

                    //View Monitor Hot Key
                    //Quickly turn on and off monitor
                    else if (BepInEx.UnityInput.Current.GetKeyDown(UnityEngine.KeyCode.M))
                    {
                        string cmd = "view monitor";

                        t.screenText.text += cmd;
                        t.textAdded = cmd.Length;

                        t.OnSubmit();                 
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
            tn.displayText = s;
            t.LoadNewNode(tn);

            t.screenText.ActivateInputField();
            ((Selectable)t.screenText).Select();
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

        static RoundManager FindRoundManager()
        {
            // Implement logic to find the active instance of the TerminalNode
            // This can be done through GameObject.Find, iterating through GameObjects, etc.

            // Example: finding by tag
            RoundManager[] roundManager = UnityEngine.Object.FindObjectsOfType<RoundManager>();

            if (roundManager.Length > 0)
            {
                // For simplicity, just return the first TerminalNode found.
                // You might want to implement more sophisticated logic based on your requirements.
                return roundManager[0];
            }

            return null;
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
            return ">Ctrl+D\nOpen / close all doors.\n\n" +
                ">Ctrl+J\nJam all turrets and land mines.\n\n" +
                ">Ctrl+M\nTurn monitor on and off.\n\n" +
                ">Ctrl+S\nQuickly switch players on the monitor.\n\n" +
                ">Ctrl+T\nBegin a signal transmission command\n\n";


        }
    }
}
