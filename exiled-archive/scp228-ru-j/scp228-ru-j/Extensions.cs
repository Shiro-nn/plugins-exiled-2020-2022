using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace scp228ruj
{
	public static class Extensions
	{

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(EventHandlers.DelayAction(0.5f, () => player.playerMovementSync.OverridePosition(pos, 0)));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}
		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}

		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.RefreshHiddenTag();
		}

		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
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

		public static Pickup SpawnItem(ItemType itemType, float durability, Vector3 position, Quaternion rotation = default, int sight = 0, int barrel = 0, int other = 0)
			=> HostInventory.SetPickup(itemType, durability, position, rotation, sight, barrel, other);
		public static void AddItem(this ReferenceHub player, ItemType itemType) => player.inventory.AddNewItem(itemType);
		public static RoleType GetRole(this ReferenceHub player) => player.characterClassManager.CurClass;
		public static Team GetTeam2(this ReferenceHub player) => GetTeam(GetRole(player));
		public static Team GetTeam(this RoleType roleType)
		{
			switch (roleType)
			{
				case RoleType.ChaosInsurgency:
					return Team.CHI;
				case RoleType.Scientist:
					return Team.RSC;
				case RoleType.ClassD:
					return Team.CDP;
				case RoleType.Scp049:
				case RoleType.Scp93953:
				case RoleType.Scp93989:
				case RoleType.Scp0492:
				case RoleType.Scp079:
				case RoleType.Scp096:
				case RoleType.Scp106:
				case RoleType.Scp173:
					return Team.SCP;
				case RoleType.Spectator:
					return Team.RIP;
				case RoleType.FacilityGuard:
				case RoleType.NtfCadet:
				case RoleType.NtfLieutenant:
				case RoleType.NtfCommander:
				case RoleType.NtfScientist:
					return Team.MTF;
				case RoleType.Tutorial:
					return Team.TUT;
				default:
					return Team.RIP;
			}
		}
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
