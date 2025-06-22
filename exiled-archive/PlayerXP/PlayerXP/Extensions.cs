using Exiled.API.Features;
using Hints;
using MEC;
using MongoDB.Bson;
using PlayerXP.player;
using PlayerXP.scp343.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerXP.scp228.API;

namespace PlayerXP
{
    public static class Extensions
	{
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
				foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				{
					if (door.DoorName == "106_PRIMARY")
					{
						player.SetPosition(new Vector3(door.transform.position.x, door.transform.position.y + 1, door.transform.position.z));
					}
				}
			}
		}
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

			return new Room(transform.name, transform, transform.position);
		}
		public static void AddItem(this ReferenceHub player, ItemType itemType) => player.inventory.AddNewItem(itemType);
		public static void AddItem(this ReferenceHub player, Inventory.SyncItemInfo item) => player.inventory.AddNewItem(item.id, item.durability, item.modSight, item.modBarrel, item.modOther);
		public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
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
			Coroutines.Add(Timing.RunCoroutine(DoInfectionTimer(player), $"{player.characterClassManager.UserId}"));
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
					//broadcast.RpcAddElement(string.Format("�� �������� SCP-008. �� ������� SCP 049-2 ����� {0} ������!", (object)(float)((double)this.plugin.InfectionLength - (double)i)), 1U, false);
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
		public static RoleType GetRole(this ReferenceHub player) => player.characterClassManager.CurClass;
		public static Team GetTeam(this ReferenceHub player) => GetTeam(GetRole(player));
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
		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(DelayAction(0.5f, () => player.playerMovementSync.OverridePosition(pos, 0)));
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
	public class MapObjectLoaded
	{
		public int id { get; set; } = 0;
		public string room { get; set; } = "none";
		public string name { get; set; } = "";
		public Vector3 position { get; set; } = new Vector3();
		public Vector3 scale { get; set; } = new Vector3(1, 1, 1);
		public Vector3 rotation { get; set; } = new Vector3();
		public GameObject workStation { get; set; } = null;
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
}