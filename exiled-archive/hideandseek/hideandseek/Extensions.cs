using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;
namespace hideandseek
{
	public static class Extensions
	{

		private static AlphaWarheadController _alphaWarheadController;
		public static AlphaWarheadController AlphaWarheadController
		{
			get
			{
				if (_alphaWarheadController == null)
					_alphaWarheadController = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();

				return _alphaWarheadController;
			}
		}

		public static void StartNuke()
		{
			AlphaWarheadController.InstantPrepare();
			AlphaWarheadController.StartDetonation();
		}

		public static void Broadcast(this ReferenceHub player, string message, ushort time)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}
		public static void ClearBroadcasts(this ReferenceHub player) => BroadcastComponent.TargetClearElements(player.scp079PlayerScript.connectionToClient);
		private static Broadcast _broadcast;
		internal static Broadcast BroadcastComponent
		{
			get
			{
				if (_broadcast == null)
					_broadcast = PlayerManager.localPlayer.GetComponent<Broadcast>();

				return _broadcast;
			}
		}
		private static Inventory _hostInventory;
		public static Inventory HostInventory
		{
			get
			{
				if (_hostInventory == null)
					_hostInventory = GetPlayer(PlayerManager.localPlayer).inventory;

				return _hostInventory;
			}
		}
		public static void AddItem(this ReferenceHub player, ItemType itemType) => player.inventory.AddNewItem(itemType);
		public static RoleType GetRole(this ReferenceHub player) => player.characterClassManager.CurClass;
		public static string GetUserId(this ReferenceHub player) => player.characterClassManager.UserId;
		public static Dictionary<int, ReferenceHub> IdHubs = new Dictionary<int, ReferenceHub>();
		public static int GetPlayerId(this ReferenceHub player) => player.queryProcessor.PlayerId;
		public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
		public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
		public static Dictionary<string, ReferenceHub> StrHubs = new Dictionary<string, ReferenceHub>();
		public static string GetNickname(this ReferenceHub player) => player.nicknameSync.Network_myNickSync;
		public static ReferenceHub GetPlayer(this GameObject player) => ReferenceHub.GetHub(player);

		public static ReferenceHub GetPlayer(int playerId)
		{
			if (IdHubs.ContainsKey(playerId))
				return IdHubs[playerId];

			foreach (ReferenceHub hub in GetHubs())
			{
				if (hub.GetPlayerId() == playerId)
				{
					IdHubs.Add(playerId, hub);

					return hub;
				}
			}

			return null;
		}
		public static ReferenceHub GetPlayer(string args)
		{
			try
			{
				if (StrHubs.ContainsKey(args))
					return StrHubs[args];

				ReferenceHub playerFound = null;

				if (short.TryParse(args, out short playerId))
					return GetPlayer(playerId);

				if (args.EndsWith("@steam") || args.EndsWith("@discord") || args.EndsWith("@northwood") || args.EndsWith("@patreon"))
				{

					foreach (ReferenceHub player in GetHubs())
					{
						if (player.GetUserId() == args)
						{
							playerFound = player;

						}
					}
				}
				else
				{

					if (args == "WORLD" || args == "SCP-018" || args == "SCP-575" || args == "SCP-207")
						return null;

					int maxNameLength = 31, lastnameDifference = 31;
					string str1 = args.ToLower();

					foreach (ReferenceHub player in GetHubs())
					{
						if (!player.GetNickname().ToLower().Contains(args.ToLower()))
							continue;

						if (str1.Length < maxNameLength)
						{
							int x = maxNameLength - str1.Length;
							int y = maxNameLength - player.GetNickname().Length;
							string str2 = player.GetNickname();

							for (int i = 0; i < x; i++) str1 += "z";

							for (int i = 0; i < y; i++) str2 += "z";

							int nameDifference = LevenshteinDistance.Compute(str1, str2);
							if (nameDifference < lastnameDifference)
							{
								lastnameDifference = nameDifference;
								playerFound = player;

							}
						}
					}
				}

				if (playerFound != null)
					StrHubs.Add(args, playerFound);

				return playerFound;
			}
			catch
			{
				return null;
			}
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
