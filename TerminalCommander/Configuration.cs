using System;

namespace TerminalCommander
{
    [Serializable]
    public class TerminalCommanderConfiguration 
    {
        public bool AllowJamming { get; set; }
        public bool AllowBigDoors { get; set; }

        //IN WORKS
        public void Set_Configs(Commander c)
        {
           
            var configEntry = c.Config.Bind("General", "Allow Jamming", true, "If false, removes turret and mine jamming ability.");
            AllowJamming = configEntry.Value;

            configEntry = c.Config.Bind("General", "Allow Facility Big Doors Control", true, "If false, removes ability to operate facility powered doors.");
            AllowJamming = configEntry.Value;

            //Key Bind Example for Later
            //c.Config.Bind("General", "Jamming Hot Key (Ctrl+)", UnityEngine.KeyCode.J, "If false, removes turret and mine jamming ability.");

        }

        public void Set_Configs(TerminalCommanderConfiguration hostConfig)
        {
            AllowJamming = hostConfig.AllowJamming;
            AllowBigDoors = hostConfig.AllowBigDoors;
        }
    }
}
