using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            float amt = teleporter.cooldownAmount;
            foreach (var player in StartOfRound.Instance.ClientPlayerList)
            {
                //Skip person who called emergency tp              
                // if (player.playerClientId != (ulong)StartOfRound.Instance.thisClientPlayerId)
                // {
                teleporter.cooldownAmount = amt;
                terminal.terminalAudio.PlayOneShot(commanderSource.Audio.emergencyAudio);
                teleporter.PressTeleportButtonOnLocalClient();
                teleporter.cooldownAmount = 0f;
                StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(true);
                yield return new WaitForSeconds(2);
                // }
            }
            teleporter.cooldownAmount = amt;
        }
    }
}
