namespace roundtime
{
    public class Plugin : Qurre.Plugin
    {
        public Events Events;
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "Rainbow Tags";

        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        private void RegisterEvents()
        {
            Events = new Events(this);
            Qurre.Events.Round.WaitingForPlayers += Events.wait;
            Qurre.Events.Player.Damage += Events.hurt;
        }
        private void UnregisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers -= Events.wait;
            Qurre.Events.Player.Damage -= Events.hurt;
            Events = null;
        }
    }
}
