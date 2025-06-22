namespace scp966
{
    public class Plugin : Qurre.Plugin
    {
        public static bool Enabled { get; internal set; }
        public EventHandlers EventHandlers;
        #region override
        public override string Name { get; } = "scp966";
        public override string Developer { get; } = "fydne";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Enabled = Config.GetBool("scp966_enable", true);
            if (!Enabled) return;
            Cfg.Reload();
            EventHandlers = new EventHandlers();
            Qurre.Events.Round.Waiting += EventHandlers.WaitingForPlayers;
            Qurre.Events.Round.Start += EventHandlers.RoundStart;
            Qurre.Events.Player.InteractDoor += EventHandlers.Door;
            Qurre.Events.Player.Damage += EventHandlers.Hurt;
            Qurre.Events.Scp914.UpgradePlayer += EventHandlers.Scp914;
            Qurre.Events.Player.Leave += EventHandlers.Leave;
            Qurre.Events.Player.RoleChange += EventHandlers.SetClass;
            Qurre.Events.Player.Dead += EventHandlers.Died;
            Qurre.Events.Server.SendingRA += EventHandlers.Ra;
        }
        private void UnregisterEvents()
        {
            if (!Enabled) return;
            Qurre.Events.Round.Waiting -= EventHandlers.WaitingForPlayers;
            Qurre.Events.Round.Start -= EventHandlers.RoundStart;
            Qurre.Events.Player.InteractDoor -= EventHandlers.Door;
            Qurre.Events.Player.Damage -= EventHandlers.Hurt;
            Qurre.Events.Scp914.UpgradePlayer -= EventHandlers.Scp914;
            Qurre.Events.Player.Leave -= EventHandlers.Leave;
            Qurre.Events.Player.RoleChange -= EventHandlers.SetClass;
            Qurre.Events.Player.Dead -= EventHandlers.Died;
            Qurre.Events.Server.SendingRA -= EventHandlers.Ra;
            EventHandlers = null;
        }
        #endregion
    }
    public class Cfg
    {
        public static int Hp { get; set; } = 500;
        public static string Bc { get; set; } = "Вы-SCP 966";
        public static ushort Bc_time { get; set; } = 10;
        public static int min_players_for_spawn { get; set; } = 5;
        public static string Cassie_dead { get; set; } = "scp 9 6 6 containment minute";
        public static void Reload()
        {
            Cfg.Hp = Plugin.Config.GetInt("scp966_hp", 500);
            Cfg.Bc = Plugin.Config.GetString("scp966_bc", "Вы-SCP 966");
            Cfg.Bc_time = Plugin.Config.GetUShort("scp966_bc_time", 10);
            Cfg.min_players_for_spawn = Plugin.Config.GetUShort("scp966_min_players_for_spawn", 5);
            Cfg.Cassie_dead = Plugin.Config.GetString("scp966_cassie_dead", "scp 9 6 6 containment minute");
        }
    }
}