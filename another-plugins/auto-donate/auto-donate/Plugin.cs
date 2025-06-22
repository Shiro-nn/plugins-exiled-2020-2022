namespace auto_donate
{
    public class Plugin : Qurre.Plugin
	{
		public EventHandlers EventHandlers;
		public override string Developer => "old fydne";
		public override string Name => "Auto Donate";
		public override int Priority => -9999;
		public override void Enable()
		{
			new MongoDB.Driver.MongoClient("mongodb://mongo-root:passw0rd@135.181.233.201/auto_donate?authSource=admin").GetDatabase("auto_donate");
			EventHandlers = new EventHandlers();
			Qurre.Events.Round.End += EventHandlers.OnRoundEnd;
			Qurre.Events.Round.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
			Qurre.Events.Player.Join += EventHandlers.OnPlayerJoin;
		}
		public override void Disable()
		{
			Qurre.Events.Round.End -= EventHandlers.OnRoundEnd;
			Qurre.Events.Round.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
			Qurre.Events.Player.Join -= EventHandlers.OnPlayerJoin;
			EventHandlers = null;
		}

	}
}
