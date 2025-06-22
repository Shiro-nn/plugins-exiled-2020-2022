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
		public TcpClient tcp;
		public NetworkStream s;
		public static TcpClient stcp;
		public static NetworkStream ss;
		public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public static void OnRoundEnd(RoundEndEvent ev)
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
		}
		public void OnWaitingForPlayers()
		{
			Coroutines.Add(Timing.RunCoroutine(timeplayers()));
		}
		internal IEnumerator<float> timeplayers()
		{
			for (; ; )
			{
				sendplayer();
				yield return Timing.WaitForSeconds(1f);
			}
		}
		internal void sendplayer()
        {
			try
			{
				int portsend = 665;
				if (Server.Port == 25565)
				{
					portsend = 665;
				}
				else if (Server.Port == 25566)
				{
					portsend = 664;
				}
				else
				{
					return;
				}
				stcp = new TcpClient();
				stcp.Connect($"localhost", portsend);
				ss = stcp.GetStream();
				string str = $"players=;={Player.List.Count()}";
				byte[] ba = Encoding.UTF8.GetBytes(str);

				ss.Write(ba, 0, ba.Length);
				stcp.Close();
			}
			catch { }
		}
		public void OnPlayerJoin(JoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server") return;
			spawnpref(ev.Player.ReferenceHub);
		}
		public void spawnpref(ReferenceHub player)
		{
			var client = new MongoClient("mongodb://mongo-root:passw0rd@135.181.233.201/auto_donate?authSource=admin");
			var database = client.GetDatabase("auto_donate");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			foreach (var document in list)
			{
				if ((bool)document["re"])
				{
					var component = player.GetComponent<RainbowTagController>();
					if (component == null) player.gameObject.AddComponent<RainbowTagController>();
				}

				if ((bool)document["ve"])
				{
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("fors"), false, true, true);
				}

				if ((bool)document["ee"])
				{
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("injener"), false, true, true);
				}

				if ((bool)document["ke"])
				{
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("god"), false, true, true);
				}

				if ((bool)document["me"])
				{
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("multi"), false, true, true);
				}
			}
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
	internal static class LevenshteinDistance
	{
		internal static int Compute(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			for (int i = 1; i <= n; i++)
			{
				for (int j = 1; j <= m; j++)
				{
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			return d[n, m];
		}
	}
}