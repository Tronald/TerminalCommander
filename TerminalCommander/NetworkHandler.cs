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
            Instance = this;
        }
        public void SyncConfigs()
        {
            SyncConfigsServerRpc(commander.Configs);           
        }

        [ClientRpc]
        private void SyncConfigsClientRpc(TerminalCommanderConfiguration config)
        {
            commander.log.LogInfo($"Client Configs Received: {config.AllowJamming} {config.AllowBigDoors}");
            //Sets gameplay configs to match host rules.        
            commander.Configs.Set_Configs(config);     
        }

        [ServerRpc]
        private void SyncConfigsServerRpc(TerminalCommanderConfiguration config)
        {
            commander.log.LogInfo($"Server Configs Sent: {config.AllowJamming} {config.AllowBigDoors}");
            SyncConfigsClientRpc(config);
        }
    }
}
