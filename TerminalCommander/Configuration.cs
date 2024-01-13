using BepInEx;

namespace TerminalCommander
{
    /// <summary>
    /// Will be used to store user configs for hot key binds and game settings.
    /// </summary>
    public class TerminalCommanderConfiguration
    {
        public bool AllowJamming { get; set; }

        public  void Set_Configs(Commander c)
        {
           
            //var configEntry = c.Config.Bind("General", "Allow Jamming", true, "If false, removes turret and mine jamming ability.");
            //AllowJamming = configEntry.Value;
            //c.Config.Bind("General", "Jamming Hot Key (Ctrl+)", UnityEngine.KeyCode.J, "If false, removes turret and mine jamming ability.");

        }
    }
}
