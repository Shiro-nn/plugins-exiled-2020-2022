using MongoDB.Driver;
namespace auto_donate
{
	public class Plugin : Qurre.Plugin
	{
		#region nostatic
		public EventHandlers EventHandlers;
		internal static string MongoUrl => "mongodb://simetria:dsflj897ufheuf8hui87y@mongo.scpsl.store/simetria";
		internal static string ServerName => Plugin.Config.GetString("auto_donate_server_name", "NoRules");
		#endregion
		#region override
		public override int Priority { get; } = -9999999;
		public override string Developer { get; } = "fydne";
		public override string Name { get; } = "Auto Donate";

		public override void Enable() => RegisterEvents();
		public override void Disable() => UnregisterEvents();
		#endregion
		#region RegEvents
		private void RegisterEvents()
		{
			new MongoClient(MongoUrl);
			EventHandlers = new EventHandlers(this);
			Qurre.Events.Player.Join += EventHandlers.PlayerJoin;
			Qurre.Events.Round.WaitingForPlayers += EventHandlers.Waiting;
			Qurre.Events.Server.SendingRA += EventHandlers.OnCommand;
			EventHandlers.okp();
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			Qurre.Events.Player.Join -= EventHandlers.PlayerJoin;
			Qurre.Events.Round.WaitingForPlayers -= EventHandlers.Waiting;
			Qurre.Events.Server.SendingRA -= EventHandlers.OnCommand;
			EventHandlers = null;
		}
		#endregion
	}
}