namespace scp181
{
    public class Plugin : Qurre.Plugin
    {
        public static bool Enabled { get; internal set; }
        public EventHandlers EventHandlers;
        #region override
        public override int Priority { get; } = 100000;
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "scp181";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Enabled = Config.GetBool("scp181_enable", true);
            if (!Enabled) return;
            Cfg.Reload();
            EventHandlers = new EventHandlers(this);
            Qurre.Events.Round.WaitingForPlayers += EventHandlers.WaitingForPlayers;
            Qurre.Events.Round.Start += EventHandlers.RoundStart;
            Qurre.Events.Player.InteractDoor += EventHandlers.door;
            Qurre.Events.Player.Damage += EventHandlers.hurt;
        }
        private void UnregisterEvents()
        {
            if (!Enabled) return;
            Qurre.Events.Round.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Qurre.Events.Round.Start -= EventHandlers.RoundStart;
            Qurre.Events.Player.InteractDoor -= EventHandlers.door;
            Qurre.Events.Player.Damage -= EventHandlers.hurt;
            EventHandlers = null;
        }
        #endregion
    }
    public class Cfg
    {
        public static int MaxDoorTries { get; set; } = 5;
        public static int MaxTries { get; set; } = 5;
        public static string dontopen { get; set; } = "\n<color=red>SCP 181 не любит войну</color>";
        public static ushort dontopent { get; set; } = 10;
        public static string open { get; set; } = "\n<color=#54ff00>SCP 181 открыл вам дверь</color>";
        public static ushort opent { get; set; } = 10;
        public static string safe { get; set; } = "\n<color=#54ff00>Вам помог SCP 181</color>";
        public static ushort safet { get; set; } = 10;
        public static string anti_safe { get; set; } = "\n<color=red>Этой жертве помог SCP 181</color>";
        public static ushort anti_safet { get; set; } = 10;
        public static void Reload()
        {
            Cfg.MaxDoorTries = Plugin.Config.GetInt("scp181_max_door_tries", 5);
            Cfg.MaxTries = Plugin.Config.GetInt("scp181_max_tries", 5);
            Cfg.dontopen = Plugin.Config.GetString("scp181_dont_open_bc", "\n<color=red>SCP 181 не любит войну</color>");
            Cfg.dontopent = Plugin.Config.GetUShort("scp181_dont_open_bc_time", 10);
            Cfg.open = Plugin.Config.GetString("scp181_open_bc", "\n<color=#54ff00>SCP 181 открыл вам дверь</color>");
            Cfg.opent = Plugin.Config.GetUShort("scp181_open_bc_time", 10);
            Cfg.safe = Plugin.Config.GetString("scp181_safe_bc", "\n<color=#54ff00>Вам помог SCP 181</color>");
            Cfg.safet = Plugin.Config.GetUShort("scp181_safe_bc_time", 10);
            Cfg.anti_safe = Plugin.Config.GetString("scp181_anti_safe_bc", "\n<color=red>Этой жертве помог SCP 181</color>");
            Cfg.anti_safet = Plugin.Config.GetUShort("scp181_anti_safe_bc_time", 10);
        }
    }
}