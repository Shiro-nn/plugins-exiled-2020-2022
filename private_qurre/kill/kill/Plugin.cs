namespace kill
{
    public class Plugin : Qurre.Plugin
    {
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "kill";
        public override void Enable() => Qurre.Events.Server.SendingRA += Console;
        public override void Disable() => Qurre.Events.Server.SendingRA -= Console;
        private void Console(Qurre.API.Events.SendingRAEvent ev)
        {
            if (ev.Name == "kill")
            {
                ev.Allowed = false;
                ev.Player.Kill("Вскрыты вены :(");
            }
        }
    }
}