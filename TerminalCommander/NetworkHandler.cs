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
        private bool isHost = false;
        public NetworkHandler(Commander c)
        {
            commander = c;
        }      
        private void Awake()
        {
            if(IsServer)
            {
                isHost = true;
            }
            commander.log.LogInfo($"IS HOST {isHost}.");
            Instance = this;
        }
        public void SetHost()
        {

            commander.log.LogInfo($"Setting as host.");
            isHost = true;

        }

        public void SyncConfigs()
        {
            commander.log.LogInfo($"Client SyncConfigs Called SERVER: {isHost}");
            if (isHost)
            {
                SyncConfigsServerRpc(commander.Configs);
            }
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
