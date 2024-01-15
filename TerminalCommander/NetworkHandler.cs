﻿using DunGen;
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
            
             SyncConfigsServerRpc();
            
           
        }     

        [ClientRpc]
        private void SyncConfigsClientRpc(TerminalCommanderConfiguration configs)
        {
            commander.log.LogInfo($"Client Configs Received");

            //Sets gameplay configs to match host rules.        
            commander.Configs.AllowBigDoors = configs.AllowBigDoors;
            commander.Configs.AllowJamming = configs.AllowJamming;
        }
        [ServerRpc]
        private void SyncConfigsServerRpc()
        {
            SyncConfigsClientRpc(commander.Configs);
            commander.log.LogInfo($"Client Configs Sent");

          
        }
    }
}
