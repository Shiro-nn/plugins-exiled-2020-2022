using MEC;
namespace gate3
{
    public class Plugin : Qurre.Plugin
    {
        public EventHandlers EventHandlers;
        #region override
        public override System.Version Version => new System.Version(1, 1, 1);
        public override System.Version NeededQurreVersion => new System.Version(1, 1, 1);
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "gate3";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        public override void Reload() { }
        #endregion
        #region Events
        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Qurre.Events.Round.WaitingForPlayers += EventHandlers.Waiting;
            Qurre.Events.Round.Start += EventHandlers.RoundStart;
            Qurre.Events.Player.InteractDoor += EventHandlers.DoorOpen;
            Qurre.Events.Player.Join += EventHandlers.PlayerJoin;
            Qurre.Events.Alpha.Detonated += EventHandlers.Detonated;
            Timing.RunCoroutine(EventHandlers.etc());
        }
        private void UnregisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers -= EventHandlers.Waiting;
            Qurre.Events.Round.Start -= EventHandlers.RoundStart;
            Qurre.Events.Player.InteractDoor -= EventHandlers.DoorOpen;
            Qurre.Events.Player.Join -= EventHandlers.PlayerJoin;
            Qurre.Events.Alpha.Detonated -= EventHandlers.Detonated;
            EventHandlers = null;
        }
        #endregion
    }
}
