using DunGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;

namespace TerminalCommander
{
    public class NetworkHandler : NetworkBehaviour
    {
        public static NetworkHandler Instance;
        private Commander commander;
     
        public NetworkHandler(Commander c)
        {
            commander = c;
        }      
        private void Awake()
        {
            commander.log.LogInfo("AWAKENING NETWORKHANDLER");
            Instance = this;
        }
        public void SyncConfigs()
        {
            SyncConfigsServerRpc(commander.Configs.AllowJamming, commander.Configs.AllowBigDoors);           
        }

        [ClientRpc]
        private void SyncConfigsClientRpc(bool jam, bool door)
        {
            commander.log.LogInfo($"Client Configs Received: {jam} {door}");
            //Sets gameplay configs to match host rules.        
            commander.Configs.AllowBigDoors = door;
            commander.Configs.AllowJamming = jam;
        }

        [ServerRpc]
        private void SyncConfigsServerRpc(bool jam, bool door)
        {
            SyncConfigsClientRpc(jam,door);
        }

        public void ForceWake()
        {
            WakeServerRpc(true);
        }

        [ServerRpc]
        private void WakeServerRpc(bool t)
        {
            commander.log.LogInfo($"Waking NetworkHandler");
        }
        [ClientRpc]
        private void WakeClientRpc(bool t)
        {
            WakeClientRpc(t);
        }
    }
}
