using MEC;
using MongoDB.Bson;
using MongoDB.Driver;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
namespace auto_donate
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static Dictionary<string, string> prefix = new Dictionary<string, string>();
		public static Dictionary<string, bool> donator = new Dictionary<string, bool>();
		public void PlayerJoin(JoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			bool gof = false;
			Timing.CallDelayed(1.5f, () =>
			{
				if (!gof)
				{
					gof = true;
					spawnpref(ev.Player.ReferenceHub);
				}
			});
		}
		public void AddReserve()
		{
			var client = new MongoClient(Plugin.MongoUrl);
			var database = client.GetDatabase("simetria");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var list = collection.Find(_ => true).ToList();
			foreach (var document in list)
			{
				if ((bool)document["d8"]) ReservedSlot.Users.Add((string)document["steam"] + "@steam");
			}
		}
		public void spawnpref(ReferenceHub player)
		{
			var client = new MongoClient(Plugin.MongoUrl);
			var database = client.GetDatabase("simetria");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			foreach (var document in list)
			{
				if ((bool)document["d2"])
				{
					try
					{
						if (prefix.ContainsKey(player.characterClassManager.UserId)) prefix.Remove(player.characterClassManager.UserId);
						prefix.Add(player.characterClassManager.UserId, (string)document["prefix"]);
					}
					catch { }
					var component = player.GetComponent<RainbowTagController>();
					if (component == null) player.gameObject.AddComponent<RainbowTagController>();
					try
					{
						if (donator.ContainsKey(player.characterClassManager.UserId)) donator.Remove(player.characterClassManager.UserId);
						donator.Add(player.characterClassManager.UserId, true);
					}
					catch { }
				}
				if ((bool)document["d6"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("ntf"), false, true, false);
					try
					{
						if (donator.ContainsKey(player.characterClassManager.UserId)) donator.Remove(player.characterClassManager.UserId);
						donator.Add(player.characterClassManager.UserId, true);
					}
					catch { }
				}
				else if ((bool)document["d7"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("mog1"), false, true, false);
					try
					{
						if (donator.ContainsKey(player.characterClassManager.UserId)) donator.Remove(player.characterClassManager.UserId);
						donator.Add(player.characterClassManager.UserId, true);
					}
					catch { }
				}
				else if ((bool)document["d5"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("mlevent"), false, true, false);
					try
					{
						if (donator.ContainsKey(player.characterClassManager.UserId)) donator.Remove(player.characterClassManager.UserId);
						donator.Add(player.characterClassManager.UserId, true);
					}
					catch { }
				}
				else if ((bool)document["d3"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("himik"), false, true, false);
					try
					{
						if (donator.ContainsKey(player.characterClassManager.UserId)) donator.Remove(player.characterClassManager.UserId);
						donator.Add(player.characterClassManager.UserId, true);
					}
					catch { }
				}
				else if ((bool)document["d1"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("mag"), false, true, false);
					try
					{
						if (donator.ContainsKey(player.characterClassManager.UserId)) donator.Remove(player.characterClassManager.UserId);
						donator.Add(player.characterClassManager.UserId, true);
					}
					catch { }
				}
			}
		}
		internal void Waiting() => AddReserve();
		public void OnCommand(SendingRAEvent ev)
		{
			#region logs
			if (ev.Player.Id == 1) return;
			if (ev.Player.UserId == "") return;
			if (ev.Player.Nickname == "Dedicated Server") return;
			if (ev.Player.UserId == "-@steam") return;
			if (!donator.ContainsKey(ev.Player.UserId)) return;
			string Args = string.Join(" ", ev.Args);
			string msg = "";
			try
			{
				if (ev.Name == "forceclass")
				{
					string targets = "";
					string[] spearator = { "." };
					string[] strlist = ev.Args[0].Split(spearator, 2,
						   System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in strlist)
					{
						targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
					}
					RoleType role = (RoleType)Int32.Parse(ev.Args[1]);
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {targets} {ev.Args[1]}^{role}^ {Args.Replace(ev.Args[0], "").Replace(ev.Args[1], "")}";
				}
				else if (ev.Name == "request_data")
				{
					string targets = "";
					string[] spearator = { "." };
					string[] strlist = ev.Args[1].Split(spearator, 2,
						   System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in strlist)
					{
						targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
					}
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {ev.Args[0]} {targets} {Args.Replace(ev.Args[0], "").Replace(ev.Args[1], "")}";
				}
				else if (ev.Name == "give")
				{
					string targets = "";
					string[] spearator = { "." };
					string[] strlist = ev.Args[0].Split(spearator, 2,
						   System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in strlist)
					{
						targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
					}
					ItemType item = (ItemType)Int32.Parse(ev.Args[1]);
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {targets} {ev.Args[1]}^{item}^ {Args.Replace(ev.Args[0], "").Replace(ev.Args[1], "")}";
				}
				else if (ev.Name == "overwatch" || ev.Name == "bypass" || ev.Name == "heal" || ev.Name == "god" || ev.Name == "noclip")
				{
					string targets = "";
					string[] spearator = { "." };
					string[] strlist = ev.Args[0].Split(spearator, 2,
						   System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in strlist)
					{
						targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
					}
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {targets} {Args.Replace(ev.Args[0], "")}";
				}
				else if (ev.Name == "bring" || ev.Name == "goto")
				{
					string target = $"{ev.Args[0]}^{Player.Get(int.Parse(ev.Args[0]))?.Nickname}^ ";
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {target} {Args.Replace(ev.Args[0], "")}";
				}
				else
				{
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {Args}";
				}
			}
			catch
			{
				msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {Args}";
			}
			if (donator[ev.Player.UserId])
			{
				sendralog($"{msg}");
			}
			#endregion
		}
		internal void okp()
		{
			Timing.CallDelayed(5f, () => sendplayersinfo());
		}
		public static NetworkStream ss;
		internal void sendralog(string cdata)
		{
			try
			{
				string server = Plugin.ServerName;
				string mode = "ral";
				TcpClient stcp = new TcpClient();
				stcp.Connect($"localhost", 421);
				ss = stcp.GetStream();
				string str = $"{mode}=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata.Replace(":keyboard:", "⌨️")}=;={server}=;={ServerConsole.Port}=;={DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
				byte[] ba = Encoding.UTF8.GetBytes(str);
				ss.Write(ba, 0, ba.Length);
				stcp.Close();
			}
			catch { }
		}
		internal void sendplayersinfo()
		{
			okp();
			try
			{
				TcpClient stcp = new TcpClient();
				stcp.Connect($"localhost", 421);
				ss = stcp.GetStream();
				string name = Plugin.ServerName;
				int players = Player.List.ToList().Count;
				int maxplay = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
				string str = $"online=;={name}=;={players}=;={maxplay}=;={Server.Ip}=;={Server.Port}";
				byte[] ba = Encoding.UTF8.GetBytes(str);
				ss.Write(ba, 0, ba.Length);
				stcp.Close();
			}
			catch { }
		}
	}
	public class RainbowTagController : MonoBehaviour
	{
		private ServerRoles _roles;
		private string _originalColor;

		private int _position = 0;
		private float _nextCycle = 0f;

		public static string[] Colors =
		{
			"pink",
			"red",
			"brown",
			"silver",
			"light_green",
			"crimson",
			"cyan",
			"aqua",
			"deep_pink",
			"tomato",
			"yellow",
			"magenta",
			"blue_green",
			"orange",
			"lime",
			"green",
			"emerald",
			"carmine",
			"nickel",
			"mint",
			"army_green",
			"pumpkin"
		};

		public static float interval { get; set; } = 0.5f;


		private void Start()
		{
			_roles = GetComponent<ServerRoles>();
			_nextCycle = Time.time;
			_originalColor = _roles.NetworkMyColor;
		}


		private void OnDisable()
		{
			_roles.NetworkMyColor = _originalColor;
		}


		private void Update()
		{
			if (Time.time < _nextCycle) return;
			_nextCycle += interval;

			_roles.NetworkMyColor = Colors[_position];

			if (++_position >= Colors.Length)
				_position = 0;
		}
	}
}