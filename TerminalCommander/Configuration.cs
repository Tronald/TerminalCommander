using DunGen;
using System;

namespace TerminalCommander
{
    [Serializable]
    public class TerminalCommanderConfiguration 
    {
        public bool AllowJamming { get; set; } = true;
        public bool AllowBigDoors { get; set; } = true;
        public int JammingCoolDown { get; set; } = 0;
        public int BigDoorsCoolDown { get; set; } = 0;

        public UnityEngine.KeyCode JammingKey { get; set; } = UnityEngine.KeyCode.J;
        public UnityEngine.KeyCode DoorKey{ get; set; } = UnityEngine.KeyCode.D;
        public UnityEngine.KeyCode SwitchKey { get; set; } = UnityEngine.KeyCode.S;
        public UnityEngine.KeyCode MonitorKey { get; set; } = UnityEngine.KeyCode.M;
        public UnityEngine.KeyCode TransmitKey { get; set; } = UnityEngine.KeyCode.T;
        public UnityEngine.KeyCode TeleportKey { get; set; } = UnityEngine.KeyCode.W;
        public UnityEngine.KeyCode InverseTeleportKey { get; set; } = UnityEngine.KeyCode.I;

        //IN WORKS
        public void Set_Configs(Commander c)
        {
            //Gameplay Settings
            AllowJamming = c.Config.Bind("Gameplay", "Allow jamming", true, "If false, removes turret and mine jamming ability.").Value;
            AllowBigDoors = c.Config.Bind("Gameplay", "Allow door control", true, "If false, removes ability to operate facility powered doors.").Value;
            
            //Cooldown is for player only, not entire server. This prevents spam. Consider coverting to a gameplay setting in the future.
            JammingCoolDown = c.Config.Bind("Gameplay", "Jamming cool down", 0, "Cool down time in seconds before the same player can send another jamming signal.").Value;
            BigDoorsCoolDown = c.Config.Bind("Gameplay", "Door control cool down", 0, "Cool down time in seconds before the same player can send another command to open / close all doors.").Value;
           
            //Key Binds
            JammingKey = c.Config.Bind("Key Binds", "Jamming (Ctrl+)", UnityEngine.KeyCode.J).Value;
            DoorKey = c.Config.Bind("Key Binds", "Open/Close facility doors (Ctrl+)", UnityEngine.KeyCode.D).Value;
            SwitchKey = c.Config.Bind("Key Binds", "Play switch on monitor (Ctrl+)", UnityEngine.KeyCode.S).Value;
            MonitorKey = c.Config.Bind("Key Binds", "Turn on/off monitor (Ctrl+)", UnityEngine.KeyCode.M).Value;
            TransmitKey = c.Config.Bind("Key Binds", "Start transmission (Ctrl+)", UnityEngine.KeyCode.T).Value;
            TeleportKey = c.Config.Bind("Key Binds", "Teleport (Ctrl+)", UnityEngine.KeyCode.W).Value;
            InverseTeleportKey = c.Config.Bind("Key Binds", "Inverse Teleport (Ctrl+)", UnityEngine.KeyCode.I).Value;
        }

        public void Set_Configs(TerminalCommanderConfiguration hostConfig)
        {
            AllowJamming = hostConfig.AllowJamming;
            AllowBigDoors = hostConfig.AllowBigDoors;
            JammingCoolDown = hostConfig.JammingCoolDown;
            BigDoorsCoolDown = hostConfig.BigDoorsCoolDown;
        }
    }
}
