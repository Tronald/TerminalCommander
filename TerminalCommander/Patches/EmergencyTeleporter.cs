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
        public void StartTeleporter(ShipTeleporter teleporter)
        {
            StartCoroutine(Teleport(teleporter));
        }
        IEnumerator Teleport(ShipTeleporter teleporter)
        {         
            teleporter.PressTeleportButtonOnLocalClient();
            teleporter.cooldownAmount = 0f;
            StartOfRound.Instance.mapScreen.SwitchRadarTargetForward(true);
            yield return new WaitForSeconds(2);
        }
    }
}
