using HarmonyLib;
using MongoDB.Bson;
using MongoDB.Driver;
using Qurre;
using System.Threading;
namespace auto_donate
{
	public class Plugin : Qurre.Plugin
	{
		#region nostatic
		public EventHandlers EventHandlers;
		#endregion
		#region override
		public override int Priority { get; } = 111111;
		public override string Developer { get; } = "fydne";
		public override string Name { get; } = "Auto Donate";
		internal static MongoClient Client;

		public override void Enable()
        {
			Configs.Reload();
			Log.Info($"Подключаюсь к бд. Ссылка - [удалено]");
			Client = new MongoClient(Configs.MongoURL);
			Log.Info($"Подключен к бд");
			Log.Info($"Получаю монго-дб");
			var database = Client.GetDatabase("auto_donate");
			Log.Info($"Монго-дб успешно получен");
			Log.Info($"Получаю коллекцию");
			database.GetCollection<BsonDocument>("donates");
			Log.Info($"Коллекция успешно получена");
			RegisterEvents();
		}
		public override void Disable() => UnregisterEvents();
		#endregion
		#region RegEvents
		private Harmony hInstance;
		private void RegisterEvents()
		{
			new Thread(() => EventHandlers.SendPlayers()).Start();
			EventHandlers = new EventHandlers();
			Qurre.Events.Player.Join += EventHandlers.PlayerJoin;
			Qurre.Events.Round.Waiting += EventHandlers.Waiting;
			hInstance = new Harmony("fydne.autodonate");
			hInstance.PatchAll();
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			Qurre.Events.Player.Join -= EventHandlers.PlayerJoin;
			Qurre.Events.Round.Waiting -= EventHandlers.Waiting;
			EventHandlers = null;
			hInstance.UnpatchAll(null);
		}
		#endregion
	}
}
