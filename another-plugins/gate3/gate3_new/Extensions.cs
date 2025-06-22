using Exiled.API.Features;
using MEC;
using MongoDB.scp035.API;
using MongoDB.scp228.API;
using MongoDB.scp343.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MongoDB.gate3
{
	public static class Extensions
	{
        public static Plugin plugin;

        public static void InvokeStaticMethod(this Type type, string methodName, object[] param)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic |
								 BindingFlags.Static | BindingFlags.Public;
			MethodInfo info = type.GetMethod(methodName, flags);
			info?.Invoke(null, param);
		}
		public static void SetPosition(this ReferenceHub player, Vector3 position) => player.SetPosition(position.x, position.y, position.z);
		public static void SetPosition(this ReferenceHub player, float x, float y, float z) => player.playerMovementSync.OverridePosition(new Vector3(x, y, z), player.transform.rotation.eulerAngles.y);
		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}
		public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
		public static RoleType GetRole(this ReferenceHub player) => player.characterClassManager.CurClass;
		public static Pickup SpawnItem(ItemType itemType, float durability, Vector3 position, Quaternion rotation = default, int sight = 0, int barrel = 0, int other = 0)
			=> HostInventory.SetPickup(itemType, durability, position, rotation, sight, barrel, other);
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
		public static ReferenceHub GetPlayer(this GameObject player) => ReferenceHub.GetHub(player);
		public static void DropItem(this ReferenceHub player)
		{
			player.inventory.ServerDropAll();
		}
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				player.characterClassManager.SetClassID(role);
				Timing.CallDelayed(0.5f, () => player.playerMovementSync.OverridePosition(pos, 0));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}
		public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
		public static float GenerateRandomNumber(float min, float max)
		{
			if (max + 1 <= min) return min;
			return (float)new System.Random().NextDouble() * ((max + 1) - min) + min;
		}
		internal static Stats LoadStats(string userId)
		{
			return new Stats()
			{
				UserId = userId,
				shelter = 3,
				elevator = 3,
			};
		}

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
		internal static int CountRoles(Team team)
		{
			ReferenceHub scp035 = Scp035Data.GetScp035();
			ReferenceHub scp343 = scp343Data.GetScp343();
			ReferenceHub scp228 = Scp228Data.GetScp228();

			int count = 0;
			foreach (Player pl in Player.List)
			{
				try
				{
					if (pl.Team == team)
					{
						if (pl.ReferenceHub.queryProcessor.PlayerId == scp035?.queryProcessor.PlayerId)
						{
							count--;
						}
						if (pl.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
						{
							count--;
						}
						if (pl.ReferenceHub.queryProcessor.PlayerId == scp228?.queryProcessor.PlayerId)
						{
							count--;
						}
						count++;

					}
				}
				catch { }
			}
			return count;
		}
		public static Dictionary<string, ReferenceHub> StrHubs = new Dictionary<string, ReferenceHub>();
		public static string GetNickname(this ReferenceHub player) => player.nicknameSync.Network_myNickSync;
		public static string GetUserId(this ReferenceHub player) => player.characterClassManager.UserId;
		public static Dictionary<int, ReferenceHub> IdHubs = new Dictionary<int, ReferenceHub>();
		public static int GetPlayerId(this ReferenceHub player) => player.queryProcessor.PlayerId;
	}
	[Serializable]
	public class Stats
	{
		public string UserId;
		public int shelter;
		public int elevator;
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