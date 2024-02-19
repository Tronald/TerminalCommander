using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace TerminalCommander.Patches
{
    internal class EmergencyTeleporter : MonoBehaviour
    {
        public void StartTeleporter(Commander commanderSource, Terminal terminal, ShipTeleporter teleporter)
        {
            StartCoroutine(Teleport(commanderSource, terminal, teleporter));
        }
        IEnumerator Teleport(Commander commanderSource, Terminal terminal, ShipTeleporter teleporter)
        {
            commanderSource.log.LogInfo($"Gathering radar targets {StartOfRound.Instance.mapScreen.radarTargets.Count}");
            List<PlayerControllerB> tped = new List<PlayerControllerB>();
            //terminal.terminalAudio.PlayOneShot(commanderSource.Audio.emergencyAudio);

            for (int pcount = 0; pcount < StartOfRound.Instance.mapScreen.radarTargets.Count; pcount++)
            {        
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(pcount);
                yield return new WaitForSeconds(.035f); //Allow target switch

                var player = StartOfRound.Instance.mapScreen.targetedPlayer;
                if (tped.Contains(player)) { continue; }
                
                commanderSource.log.LogInfo($"TP {player.playerUsername} - {!player.isInHangarShipRoom} {!player.isInElevator}");
                if (!player.isInHangarShipRoom && !player.isInElevator)
                {
                    teleporter.PressTeleportButtonOnLocalClient();
                }             
                tped.Add(player);
                yield return new WaitForSeconds(5); //Teleporter cannot be ran concurrently in Vanilla.
            }

            HUDManager.Instance.AddTextToChatOnServer(ChatManagerPatch.EmergencyTpEndMessage);
        }
    }
   
}
