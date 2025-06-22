using Exiled.API.Features;
using Hints;
using MEC;
using MongoDB.Bson;
using MongoDB.player;
using MongoDB.scp343.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MongoDB.scp228.API;
using MongoDB.scp035.API;
using Mirror;
using Exiled.API.Extensions;

namespace MongoDB
{
	public static class Extensions
	{
		internal static System.Random Random = new System.Random();
		public static int ItemDur(ItemType weapon)
		{
			if (weapon != ItemType.GunCOM15)
			{
				switch (weapon)
				{
					case ItemType.GunE11SR:
						return 18;
					case ItemType.GunProject90:
						return 50;
					case ItemType.Ammo556:
						return 25;
					case ItemType.GunMP7:
						return 35;
					case ItemType.GunLogicer:
						return 100;
					case ItemType.Ammo762:
						return 25;
					case ItemType.Ammo9mm:
						return 25;
					case ItemType.GunUSP:
						return 18;
				}
				return 50;
			}
			return 12;
		}
		public static void SetRole(this ReferenceHub player, RoleType newRole) => player.characterClassManager.SetPlayersClass(newRole, player.gameObject);
		public static void SetRole(this ReferenceHub player, RoleType newRole, bool keepPosition)
		{
			if (keepPosition)
			{
				player.characterClassManager.NetworkCurClass = newRole;
				player.playerStats.SetHPAmount(player.characterClassManager.Classes.SafeGet(player.GetRole()).maxHP);
			}
			else
				SetRole(player, newRole);
		}
		public static void SetRotation(this ReferenceHub player, Vector2 rotations) => player.SetRotation(rotations.x, rotations.y);
		public static void SetRotation(this ReferenceHub player, float x, float y) => player.playerMovementSync.NetworkRotationSync = new Vector2(x, y);
		internal static void TeleportTo106(ReferenceHub player)
		{
			try
			{
				ReferenceHub scp106 = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
				Vector3 toded = scp106.transform.position;
				player.SetPosition(toded);
			}
			catch
			{
				player.SetPosition(Map.GetRandomSpawnPoint(RoleType.Scp096));
			}
		}
		public static void SetWeaponAmmo(this ReferenceHub rh, int amount)
		{
			rh.inventory.items.ModifyDuration(
			rh.inventory.items.IndexOf(rh.inventory.GetItemInHand()),
			amount);
		}
		public static void SetInventory(this ReferenceHub player, List<Inventory.SyncItemInfo> items)
		{
			player.inventory.items.Clear();

			foreach (Inventory.SyncItemInfo item in items)
				player.inventory.AddNewItem(item.id, item.durability, item.modSight, item.modBarrel, item.modOther);
		}
		public static Room GetCurrentRoom(this ReferenceHub player)
		{
			Vector3 playerPos = player.transform.position;
			Vector3 end = playerPos - new Vector3(0f, 10f, 0f);
			bool flag = Physics.Linecast(playerPos, end, out RaycastHit raycastHit, -84058629);

			if (!flag || raycastHit.transform == null)
				return null;

			Transform transform = raycastHit.transform;

			while (transform.parent != null && transform.parent.parent != null)
				transform = transform.parent;

			foreach (Room room in Map.Rooms)
				if (room.Position == transform.position)
					return room;

			return new Room();
		}
		public static void AddItem(this ReferenceHub player, ItemType itemType) => player.inventory.AddNewItem(itemType);
		public static void AddItem(this ReferenceHub player, Inventory.SyncItemInfo item) => player.inventory.AddNewItem(item.id, item.durability, item.modSight, item.modBarrel, item.modOther);
		public static void DropItem(this ReferenceHub player)
		{
			player.inventory.ServerDropAll();
		}
		internal static int CountRoles(Team team)
		{
			ReferenceHub scp035 = null;
			ReferenceHub scp343 = null;
			ReferenceHub scp228 = null;
			try { scp035 = Scp035Data.GetScp035(); } catch { }
            try { scp343 = scp343Data.GetScp343(); } catch { }
			try { scp228 = Scp228Data.GetScp228(); } catch { }

			int count = 0;
			foreach (Player pl in Player.List)
			{
				try
				{
					if (pl.Team == team)
					{
						if (pl.ReferenceHub.queryProcessor.PlayerId == scp035?.queryProcessor.PlayerId) count--;
						else if (pl.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId) count--;
						else if (pl.ReferenceHub.queryProcessor.PlayerId == scp228?.queryProcessor.PlayerId) count--;
						else if(EventHandlers.Pos.ContainsKey(pl.UserId)) if(!EventHandlers.Pos[pl.UserId].Alive) count--;
						count++;

					}
				}
				catch { }
			}
			return count;
		}
		public static void SetPlayerScale(this Player pl, float size)
		{
			try
			{
				pl.Scale = new Vector3(size, size, size);
			}
			catch { }
		}
		public static void InfectPlayer(ReferenceHub player)
		{
			if (hurte.InfectedPlayers.Contains(player))
			{
				return;
			}

			if (player.characterClassManager.IsAnyScp())
			{
				return;
			}
			hurte.InfectedPlayers.Add(player);
			Plugin.Coroutines.Add(Timing.RunCoroutine(DoInfectionTimer(player), $"{player.characterClassManager.UserId}"));
		}

		private static IEnumerator<float> DoInfectionTimer(ReferenceHub player)
		{
			for (int i = 0; (double)i < (double)hurte.InfectionLength; ++i)
			{
				if (!hurte.InfectedPlayers.Contains(player))
				{
					yield break;
				}
				else
				{
					player.ClearBroadcasts();
					player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, string.Format("�� �������� SCP-008. �� ������� SCP 049-2 ����� {0} ������!", (object)(float)((double)hurte.InfectionLength - (double)i)), 1, 0);
					yield return Timing.WaitForSeconds(1f);
				}
			}

			GameObject gameObject = player.gameObject;
			Vector3 pos = gameObject.transform.position;

			Timing.RunCoroutine(TurnIntoZombie(player, pos));

			yield return Timing.WaitForSeconds(0.6f);

			foreach (Player p in Player.List.ToList())
			{
				ReferenceHub hub = p.ReferenceHub;
				if (Vector3.Distance(hub.gameObject.transform.position, player.gameObject.transform.position) < 10f && hub.characterClassManager.IsHuman() && hub != player)
					InfectPlayer(hub);
			}
			CurePlayer(player);
		}
		internal static void CurePlayer(ReferenceHub player)
		{
			if (hurte.InfectedPlayers.Contains(player))
				hurte.InfectedPlayers.Remove(player);

			Timing.KillCoroutines($"{player.characterClassManager.UserId}");
		}
		internal static IEnumerator<float> TurnIntoZombie(ReferenceHub player, Vector3 position)
		{
			CurePlayer(player);
			if (player.characterClassManager.CurClass == RoleType.Scp0492)
			{
				yield break;
			}
			yield return Timing.WaitForSeconds(0.3f);
			CurePlayer(player);
			player.inventory.ServerDropAll();
			player.characterClassManager.SetClassIDAdv(RoleType.Scp0492, false);
			yield return Timing.WaitForSeconds(0.5f);
			CurePlayer(player);
			player.playerStats.Health = player.playerStats.maxHP;
			player.playerMovementSync.OverridePosition(position, player.gameObject.transform.rotation.y);
			CurePlayer(player);
		}
		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}
		public static void SetPosition(this ReferenceHub player, Vector3 position) => player.SetPosition(position.x, position.y, position.z);
		public static void SetPosition(this ReferenceHub player, float x, float y, float z) => player.playerMovementSync.OverridePosition(new Vector3(x, y, z), player.transform.rotation.eulerAngles.y);
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
		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}
		public static void Hint(this ReferenceHub player, string message, float time)
		{
			player.hints.Show(new TextHint(message.Trim(), new HintParameter[] { new StringHintParameter("") }, HintEffectPresets.FadeInAndOut(0.25f, time, 0f), 10f));
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
		public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
		public static IEnumerable<ReferenceHub> GetHost() => ReferenceHub.GetAllHubs().Values.Where(h => h.IsHost());
		public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
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
		public static float GenerateRandomNumber(float min, float max)
		{
			if (max + 1 <= min) return min;
			return (float)new System.Random().NextDouble() * ((max + 1) - min) + min;
		}
		public static void Broadcast(this ReferenceHub player, string message, ushort time)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}
		public static string GetGroupName(this UserGroup group)
			=> ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key)
				.FirstOrDefault();
		public static bool adminsearch(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group);
		}
		public static Modules.RoleType GetCustomRole(this ReferenceHub player)
		{
			if (player.queryProcessor.PlayerId == MongoDB.scp035.EventHandlers.scpPlayer?.queryProcessor.PlayerId)
			{
				return Modules.RoleType.Scp035;
			}
			else if (player.queryProcessor.PlayerId == MongoDB.scp343.EventHandlers343.scp343?.queryProcessor.PlayerId)
			{
				return Modules.RoleType.Scp343;
			}
			else if (player.queryProcessor.PlayerId == MongoDB.scp228.EventHandlers228.scp228ruj?.queryProcessor.PlayerId)
			{
				return Modules.RoleType.Scp228;
			}
			else
			{
				return (Modules.RoleType)player.characterClassManager.CurClass;
			}
		}
		public static RoleType GetRole(this ReferenceHub player)
		{
			return player.characterClassManager.CurClass;
		}
		public static Team GetTeam(this ReferenceHub player) => GetTeam(GetCustomRole(player));
		public static Team GetTeam(this Modules.RoleType roleType)
		{
			switch (roleType)
			{
				case Modules.RoleType.ChaosInsurgency:
					return Team.CHI;
				case Modules.RoleType.Scientist:
					return Team.RSC;
				case Modules.RoleType.ClassD:
					return Team.CDP;
				case Modules.RoleType.Scp049:
				case Modules.RoleType.Scp93953:
				case Modules.RoleType.Scp93989:
				case Modules.RoleType.Scp0492:
				case Modules.RoleType.Scp079:
				case Modules.RoleType.Scp096:
				case Modules.RoleType.Scp106:
				case Modules.RoleType.Scp173:
				case Modules.RoleType.Scp035:
				case Modules.RoleType.Scp343:
				case Modules.RoleType.Scp228:
					return Team.SCP;
				case Modules.RoleType.Spectator:
					return Team.RIP;
				case Modules.RoleType.FacilityGuard:
				case Modules.RoleType.NtfCadet:
				case Modules.RoleType.NtfLieutenant:
				case Modules.RoleType.NtfCommander:
				case Modules.RoleType.NtfScientist:
					return Team.MTF;
				case Modules.RoleType.Tutorial:
					return Team.TUT;
				default:
					return Team.RIP;
			}
		}
		public static string GetUserId(this ReferenceHub player) => player.characterClassManager.UserId;
		public static Dictionary<int, ReferenceHub> IdHubs = new Dictionary<int, ReferenceHub>();
		public static int GetPlayerId(this ReferenceHub player) => player.queryProcessor.PlayerId;
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

		public static void PlaceCorrosion(this ReferenceHub player)
		{
			player.characterClassManager.RpcPlaceBlood(player.transform.position, 1, 2f);
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
		public static ReferenceHub TryGet228()
		{
			return Scp228Data.GetScp228();
		}
		public static ReferenceHub TryGet343()
		{
			return scp343Data.GetScp343();
		}
		public static ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}
		internal static string GetHRT(float time)
		{
			time /= 60f;
			TimeSpan timeSpan = TimeSpan.FromMinutes(time);
			return string.Format("{0}� {1}� {2}��� {3}���.", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
		public static void BanPlayer(this ReferenceHub player, int duration, string reason, string issuer = "Console") => player.gameObject.BanPlayer(duration, reason, issuer);
		public static void BanPlayer(this GameObject player, int duration, string reason, string issuer = "Console") => PlayerManager.localPlayer.GetComponent<BanPlayer>().BanUser(player, duration, reason, issuer, false);
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
	public class Person
	{
		public ObjectId Id { get; set; }
		public string steam { get; set; }
		public int xp { get; set; } = 0;
		public int to { get; set; } = 750;
		public int lvl { get; set; } = 1;
		public int money { get; set; } = 0;
	}
	public class Modules
	{
		public enum RoleType : sbyte
		{
			None = -1,
			Scp173 = 0,
			ClassD = 1,
			Spectator = 2,
			Scp106 = 3,
			NtfScientist = 4,
			Scp049 = 5,
			Scientist = 6,
			Scp079 = 7,
			ChaosInsurgency = 8,
			Scp096 = 9,
			Scp0492 = 10,
			NtfLieutenant = 11,
			NtfCommander = 12,
			NtfCadet = 13,
			Tutorial = 14,
			FacilityGuard = 15,
			Scp93953 = 16,
			Scp93989 = 17,
			Scp035 = 18,
			Scp343 = 19,
			Scp228 = 20
		}
	}
	[Serializable]
	public class VecPos
	{
		public int sec = 0;
		public Vector3 Pos = new Vector3(0, 0, 0);
		public bool Alive = true;
	}
}