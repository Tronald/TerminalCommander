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

        public void SyncConfigs(TerminalCommanderConfiguration config)
        {
            if (IsServer)
            {
                SyncConfigsClientRpc(config);
            }
            else
            {
                SynConfigsServerRpc(config);
            }
        }

        [ClientRpc]
        private void SyncConfigsClientRpc(TerminalCommanderConfiguration config)
        {
            //Sets gameplay configs to match host rules.
            commander.Configs.Set_Configs(config); 
        }

        [ServerRpc]
        private void SynConfigsServerRpc(TerminalCommanderConfiguration config)
        {
            SyncConfigsClientRpc(config);
        }
    }
}
