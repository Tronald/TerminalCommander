using BepInEx.Logging;
using DunGen;
using System;
using System.Text.RegularExpressions;

namespace TerminalCommander
{
    [Serializable]
    public class TerminalCommanderConfiguration 
    {
        public bool SyncHost { get; set; } = true;
        public bool AllowJamming { get; set; } = true;
        public bool AllowBigDoors { get; set; } = true;
        public bool AllowEmergencyTeleporter { get; set; } = true;
        public int JammingCoolDown { get; set; } = 0;
        public int BigDoorsCoolDown { get; set; } = 0;

        public UnityEngine.KeyCode JammingKey { get; set; } = UnityEngine.KeyCode.J;
        public UnityEngine.KeyCode DoorKey{ get; set; } = UnityEngine.KeyCode.D;
        public UnityEngine.KeyCode SwitchKey { get; set; } = UnityEngine.KeyCode.S;
        public UnityEngine.KeyCode MonitorKey { get; set; } = UnityEngine.KeyCode.M;
        public UnityEngine.KeyCode TransmitKey { get; set; } = UnityEngine.KeyCode.T;
        public UnityEngine.KeyCode TeleportKey { get; set; } = UnityEngine.KeyCode.W;
        public UnityEngine.KeyCode InverseTeleportKey { get; set; } = UnityEngine.KeyCode.I;
        public UnityEngine.KeyCode EmergencyTeleportKey { get; set; } = UnityEngine.KeyCode.E;

        //IN WORKS
        public void Set_Configs(Commander c)
        {
            //Gameplay Settings
            SyncHost = c.Config.Bind("Gameplay", "Sync Host", true, "If true, host gameplay settings will sync with clients using Terminal Commander.").Value;
            AllowJamming = c.Config.Bind("Gameplay", "Allow jamming", true, "If false, removes turret and mine jamming ability.").Value;
            AllowBigDoors = c.Config.Bind("Gameplay", "Allow door control", true, "If false, removes ability to operate facility powered doors.").Value;
            AllowEmergencyTeleporter = c.Config.Bind("Gameplay", "Allow emergency teleport (experimental)", true, "If false, removes ability to emergency teleport everyone back to the ship.").Value;

            //Cooldown is for player only, not entire server. This prevents spam. Consider coverting to a gameplay setting in the future.
            JammingCoolDown = c.Config.Bind("Gameplay", "Jamming cool down", 0, "Cool down time in seconds before the same player can send another jamming signal.").Value;
            BigDoorsCoolDown = c.Config.Bind("Gameplay", "Door control cool down", 0, "Cool down time in seconds before the same player can send another command to open / close all doors.").Value;
         

            //Max cooldown 200
            if (JammingCoolDown > 255) { JammingCoolDown = 255; }
            if (BigDoorsCoolDown > 255) { JammingCoolDown = 255; }

            //Key Binds
            JammingKey = c.Config.Bind("Key Binds", "Jamming (Ctrl+)", UnityEngine.KeyCode.J).Value;
            DoorKey = c.Config.Bind("Key Binds", "Open/Close facility doors (Ctrl+)", UnityEngine.KeyCode.D).Value;
            SwitchKey = c.Config.Bind("Key Binds", "Play switch on monitor (Ctrl+)", UnityEngine.KeyCode.S).Value;
            MonitorKey = c.Config.Bind("Key Binds", "Turn on/off monitor (Ctrl+)", UnityEngine.KeyCode.M).Value;
            TransmitKey = c.Config.Bind("Key Binds", "Start transmission (Ctrl+)", UnityEngine.KeyCode.T).Value;
            TeleportKey = c.Config.Bind("Key Binds", "Teleport (Ctrl+)", UnityEngine.KeyCode.W).Value;
            InverseTeleportKey = c.Config.Bind("Key Binds", "Inverse Teleport (Ctrl+)", UnityEngine.KeyCode.I).Value;
            EmergencyTeleportKey = c.Config.Bind("Key Binds", "Emergency Teleport (Ctrl+)", UnityEngine.KeyCode.E).Value;
        }

        public void Set_Configs(string hostConfigString)
        {
            Regex regex = new Regex(@"tsync[0,1][0,1][0,1]:\d+:\d+");
            if (!regex.IsMatch(hostConfigString)) { return; }
            string[] values = hostConfigString.Replace("tsync", "").Split(':');

            AllowJamming = Convert.ToBoolean(Convert.ToInt16(values[0][0].ToString()));
            AllowBigDoors = Convert.ToBoolean(Convert.ToInt16(values[0][1].ToString()));
            AllowEmergencyTeleporter = Convert.ToBoolean(Convert.ToInt16(values[0][2].ToString()));
            JammingCoolDown = Convert.ToInt32(values[1]);
            BigDoorsCoolDown = Convert.ToInt32(values[2]);
        }

        public  string SyncMessage()
        {
            int aJam = AllowJamming ? 1 : 0;
            int aBD = AllowBigDoors ? 1 : 0;
            int aET = AllowEmergencyTeleporter ? 1 : 0;
            return $"tsync{aJam}{aBD}{aET}:{JammingCoolDown}:{BigDoorsCoolDown}";
        }
    }
}
