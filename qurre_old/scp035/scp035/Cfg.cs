namespace scp035
{
    internal class Cfg
    {
        internal static ushort bct;
        internal static string bc1;
        internal static string bc2;
        internal static string bc3;
        internal static string cassie;
        internal static string ra1;
        internal static string ra2;
        internal static string ra3;
        internal static string dr;
        internal static void Reload()
        {
            Cfg.bct = Plugin.Config.GetUShort("scp035_bc_time", 10);
            Cfg.bc1 = Plugin.Config.GetString("scp035_spawn_bc", "<size=30%><color=#707480>You have been infected with <color=red>SCP 035</color>,</color>\n<color=#707480>now your task is to help other <color=red>SCPs</color></color></size>");
            Cfg.bc2 = Plugin.Config.GetString("scp035_damage_bc", "<size=25%><color=#6f6f6f><color=red>SCP 035</color> attacks you</color></size>");
            Cfg.bc3 = Plugin.Config.GetString("scp035_distance_bc", "<size=25%><color=#f47fff>*<color=#0089c7>sniffs</color>*</color>\n<color=#6f6f6f>You smell rot, it looks like it's <color=red>SCP 035</color></color></size>");
            Cfg.cassie = Plugin.Config.GetString("scp035_cassie", "ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE . ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B");
            Cfg.ra1 = Plugin.Config.GetString("scp035_command", "scp035");
            Cfg.ra2 = Plugin.Config.GetString("scp035_not_found", "Player not found!");
            Cfg.ra3 = Plugin.Config.GetString("scp035_suc", "Successfully!");
            Cfg.dr = Plugin.Config.GetString("scp035_dead_reason", "Died of decomposition");
        }
    }
}