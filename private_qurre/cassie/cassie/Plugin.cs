namespace cassie
{
    public class Plugin : Qurre.Plugin
    {
        public EventHandlers EventHandlers;
        #region override
        public override int Priority { get; } = 999999;
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "cassie";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Cfg.Reload();
            EventHandlers = new EventHandlers();
            Qurre.Events.Round.WaitingForPlayers += EventHandlers.WaitingForPlayers;
            Qurre.Events.Map.MTFAnnouncement += EventHandlers.AnnouncingMTF;
            Qurre.Events.Round.TeamRespawn += EventHandlers.TeamRespawn;
        }
        private void UnregisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers -= EventHandlers.WaitingForPlayers;
            Qurre.Events.Map.MTFAnnouncement -= EventHandlers.AnnouncingMTF;
            Qurre.Events.Round.TeamRespawn -= EventHandlers.TeamRespawn;
            EventHandlers = null;
        }
        #endregion
    }
    public class Cfg
    {
        public static string MTFCassie { get; set; } = "XMAS_EPSILON11 %UnitName% %UnitNumber% XMAS_HASENTERED %ScpsLeft% XMAS_SCPSUBJECTS";
        public static string CICassie { get; set; } = ".g1";
        public static void Reload()
        {
            Cfg.MTFCassie = Plugin.Config.GetString("cassie_mtf", "XMAS_EPSILON11 %UnitName% %UnitNumber% XMAS_HASENTERED %ScpsLeft% XMAS_SCPSUBJECTS");
            Cfg.CICassie = Plugin.Config.GetString("cassie_chaos", ".g1");
        }
    }
}