namespace scp714
{
    public class Plugin : Qurre.Plugin
    {
        public static bool Enabled { get; internal set; }
        public EventHandlers EventHandlers;
        #region override
        public override int Priority { get; } = -100;
        public override string Developer { get; } = "fydne";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Enabled = Plugin.Config.GetBool("scp714_enable", true);
            if (!Enabled) return;
            Cfg.Reload();
            EventHandlers = new EventHandlers();
            Qurre.Events.Round.Start += EventHandlers.RoundStart;
            Qurre.Events.Player.PickupItem += EventHandlers.Pickup;
            Qurre.Events.Player.Dies += EventHandlers.Dying;
            MEC.Timing.RunCoroutine(EventHandlers.aok());
        }
        private void UnregisterEvents()
        {
            if (!Enabled) return;
            Qurre.Events.Round.Start -= EventHandlers.RoundStart;
            Qurre.Events.Player.PickupItem -= EventHandlers.Pickup;
            Qurre.Events.Player.Dies -= EventHandlers.Dying;
            EventHandlers = null;
        }
        #endregion
    }
    public class Cfg
    {
        public static string PickupBc { get; set; } = "<b><color=lime>Вы подобрали SCP714</color></b>";
        public static ushort PickupBcTime { get; set; } = 5;
        public static void Reload()
        {
            Cfg.PickupBc = Plugin.Config.GetString("scp714_bc", "<b><color=lime>Вы подобрали SCP714</color></b>");
            Cfg.PickupBcTime = Plugin.Config.GetUShort("scp714_bc_time", 5);
        }
    }
}