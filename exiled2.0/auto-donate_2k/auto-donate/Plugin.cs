using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using MongoDB.Driver;
using System.Threading;
namespace auto_donate
{
	public class Plugin : Plugin<Config>
	{
		#region nostatic
		public EventHandlers EventHandlers;
		internal Config config;
		#endregion
		#region cfg
		internal void cfg1() => config = base.Config;
		#endregion
		#region override
		public override PluginPriority Priority { get; } = PluginPriority.Higher;
		public override string Author { get; } = "fydne";
		internal static MongoClient Client;

		public override void OnEnabled()
        {
			Log.Info(Config.MongoURL);
			Client = new MongoClient(Config.MongoURL);
		}
		public override void OnDisabled() => base.OnDisabled();
		public override void OnRegisteringCommands()
		{
			base.OnRegisteringCommands();
			RegisterEvents();
		}
		public override void OnUnregisteringCommands()
		{
			base.OnUnregisteringCommands();

			UnregisterEvents();
		}
		#endregion
		#region RegEvents
		private void RegisterEvents()
		{
			EventHandlers = new EventHandlers(this);
			Exiled.Events.Handlers.Player.Verified += EventHandlers.PlayerJoin;
			Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.Waiting;
			cfg1();
			new Thread(() => EventHandlers.SendPlayers()).Start();
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			Exiled.Events.Handlers.Player.Verified -= EventHandlers.PlayerJoin;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.Waiting;
			EventHandlers = null;
		}
		#endregion
	}
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public string ServerName { get; set; } = "NoRules";
		public string Donates { get; set; } = "1:test,2:admin";
		public string MongoURL { get; set; } = "";
		public string WebIp { get; set; } = "localhost";
		public int ServerID { get; set; } = 0;
	}
}
