using Qurre.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Loli.Modules;
using Loli.Scps.Api;
using Loli.DataBase;
using InventorySystem.Items.Pickups;
namespace Loli
{
	public static class Extensions
	{
		internal static Dictionary<ItemPickupBase, bool> CachedItems = new();
		internal static bool ItsNeededItem(this ItemPickupBase serital)
		{
			if (CachedItems.TryGetValue(serital, out var _data)) return _data;
			var _b = Textures.Models.Rooms.ServersManager.Lift.Doors.Contains(serital) || Textures.Models.Rooms.Control.Buttons.ContainsKey(serital) ||
				Textures.Models.Server.Doors.ContainsKey(serital) || Textures.Models.Rooms.Servers.Doors.ContainsKey(serital);
			if (CachedItems.ContainsKey(serital)) CachedItems.Remove(serital);
			CachedItems.Add(serital, _b);
			return _b;
		}
		public static bool ItsHacker(this Player pl) => pl is not null && pl.Tag.Contains(Spawns.Roles.Hacker.HackerTag);
		public static bool ItsSpyFacilityManager(this Player pl) => pl is not null && pl.Tag.Contains(Addons.RolePlay.Roles.FacilityManager.TagSpy);
		public static float GetMaxHp(this Player pl)
		{
			float maxhp = pl.MaxHp;
			if (pl.Role == RoleType.Scp106) maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
			else if (pl.Role == RoleType.Scp049) maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
			else if (pl.Role == RoleType.Scp0492) maxhp = 750;
			else if (pl.Role == RoleType.Scp096) maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
			else if (pl.Role == RoleType.Scp93953 || pl.Role == RoleType.Scp93989) maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
			else if (pl.Role == RoleType.Scp173) maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
			if (Plugin.RolePlay)
			{
				if (pl.Role == RoleType.ClassD || pl.Team == Team.CHI || pl.Role == RoleType.NtfPrivate || pl.Role == RoleType.NtfCaptain ||
					pl.Role == RoleType.NtfSergeant || pl.Role == RoleType.NtfSpecialist || pl.Role == RoleType.Scientist ||
					pl.Role == RoleType.FacilityGuard || pl.Role == RoleType.Tutorial) maxhp = UnityEngine.Random.Range(100, 135);
			}
			else
			{
				if (pl.Role == RoleType.ClassD) maxhp = 105;
				else if (pl.Team == Team.CHI) maxhp = 125;
				else if (pl.Role == RoleType.NtfPrivate) maxhp = 115;
				else if (pl.Role == RoleType.NtfCaptain) maxhp = 125;
				else if (pl.Role == RoleType.NtfSergeant) maxhp = 120;
				else if (pl.Role == RoleType.NtfSpecialist) maxhp = 125;
				else if (pl.Role == RoleType.Scientist) maxhp = 110;
				else if (pl.Role == RoleType.FacilityGuard) maxhp = 130;
				else if (pl.Role == RoleType.Tutorial) maxhp = 125;
			}
			if (pl.GetCustomRole() == Module.RoleType.Scp035) maxhp = Scps.Scp035.maxHP;
			float cf = 1;
			if (Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data))
			{
				if (Manager.Static.Data.Roles.TryGetValue(pl.UserId, out var roles) && roles.Prime) cf += 0.1f;
				if (DataBase.Modules.Data.Clans.TryGetValue(data.clan.ToUpper(), out var clan))
					foreach (int boost in clan.Boosts)
					{
						if (boost == 3) cf += 0.05f;
					}
			}
			return maxhp * cf;
		}
		public static void GetAmmo(this Player pl)
		{
			if (Plugin.RolePlay)
			{
				if (pl.Team == Team.CHI || pl.Team == Team.MTF || pl.Team == Team.TUT)
				{
					pl.AddItem(ItemType.Ammo9x19, 5);
					pl.AddItem(ItemType.Ammo556x45, 5);
					pl.AddItem(ItemType.Ammo762x39, 5);
					pl.AddItem(ItemType.Ammo44cal, 5);
					pl.AddItem(ItemType.Ammo12gauge, 5);
				}
				else if (pl.Team == Team.RSC) pl.AddItem(ItemType.Ammo9x19);
			}
			else
			{
				pl.Ammo12Gauge = 999;
				pl.Ammo44Cal = 999;
				pl.Ammo556 = 999;
				pl.Ammo762 = 999;
				pl.Ammo9 = 999;
			}
		}
		public static bool BlockThis(this Player pl)
		{
			if (!Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data)) return false;
			return data.clan.ToLower() == "prts";
		}
		internal static System.Random Random => Qurre.API.Extensions.Random;
		public static float Difference(this float first, float second)
		{
			if (first > second) return first - second;
			else return second - first;
		}
		internal static void TeleportTo106(this Player player)
		{
			try
			{
				Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
				player.Position = scp106.Position;
			}
			catch
			{
				List<Vector3> tp = new List<Vector3>();
				foreach (GameObject _go in GameObject.FindGameObjectsWithTag("PD_EXIT"))
					tp.Add(_go.transform.position);
				var pos = tp[UnityEngine.Random.Range(0, tp.Count)];
				pos.y += 2f;
				player.Position = pos;
			}
		}
		internal static int CountRoles(Team team)
		{
			int count = 0;
			foreach (var pl in Player.List)
			{
				try
				{
					if (pl.GetTeam() == team)
					{
						if (pl.ItsScp035()) count--;
						else if (!Plugin.RolePlay && EventHandlers.Pos.ContainsKey(pl.UserId) && Player.List.Count() > 5 && Round.ElapsedTime.TotalMinutes > 5)
							if (!EventHandlers.Pos[pl.UserId].Alive) count--;
						count++;

					}
				}
				catch { }
			}
			return count;
		}
		public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
		public static IEnumerable<ReferenceHub> GetHost() => ReferenceHub.GetAllHubs().Values.Where(h => h.IsHost());
		public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
		public static void SetRank(this Player player, string rank, string color = "default")
		{
			player.RoleName = rank;
			player.RoleColor = color;
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
		public static string GetGroupName(this UserGroup group)
			=> ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key)
				.FirstOrDefault();
		public static bool AdminSearch(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId).GetGroupName();
			return !string.IsNullOrEmpty(group);
		}
		public static Module.RoleType GetCustomRole(this Player player)
		{
			if (player.ItsScp035())
			{
				return Module.RoleType.Scp035;
			}
			else
			{
				return (Module.RoleType)player.Role;
			}
		}
		public static RoleType GetRole(this ReferenceHub player)
		{
			return player.characterClassManager.CurClass;
		}
		public static Team GetTeam(this Player player) => GetTeam(GetCustomRole(player));
		public static Team GetTeam(this Module.RoleType roleType)
		{
			if (roleType == Module.RoleType.Scp035) return Team.SCP;
			return GetTeam((RoleType)roleType);
		}
		public static Team GetTeam(this RoleType roleType)
		{
            return roleType switch
            {
                RoleType.ChaosConscript or RoleType.ChaosMarauder or RoleType.ChaosRepressor or RoleType.ChaosRifleman => Team.CHI,
                RoleType.Scientist => Team.RSC,
                RoleType.ClassD => Team.CDP,
                RoleType.Scp049 or RoleType.Scp93953 or RoleType.Scp93989 or RoleType.Scp0492 or RoleType.Scp079 or RoleType.Scp096 or RoleType.Scp106 or RoleType.Scp173 => Team.SCP,
                RoleType.Spectator => Team.RIP,
                RoleType.FacilityGuard or RoleType.NtfCaptain or RoleType.NtfPrivate or RoleType.NtfSergeant or RoleType.NtfSpecialist => Team.MTF,
                RoleType.Tutorial => Team.TUT,
                _ => Team.RIP,
            };
        }
		public static Player GetPlayer(string args)
		{
			try
			{
				Player found = null;

				if (short.TryParse(args, out short playerId))
					return Player.Get(playerId);

				if (args.EndsWith("@steam") || args.EndsWith("@discord") || args.EndsWith("@northwood") || args.EndsWith("@patreon"))
				{
					foreach (Player player in Player.List)
					{
						if (player.UserId == args) found = player;
					}
				}
				else
				{
					if (args == "WORLD" || args == "SCP-018" || args == "SCP-575" || args == "SCP-207")
						return null;

					int maxNameLength = 31, lastnameDifference = 31;
					string str1 = args.ToLower();

					foreach (Player player in Player.List)
					{
						if (!player.Nickname.ToLower().Contains(args.ToLower()))
							continue;

						if (str1.Length < maxNameLength)
						{
							int x = maxNameLength - str1.Length;
							int y = maxNameLength - player.Nickname.Length;
							string str2 = player.Nickname;

							for (int i = 0; i < x; i++) str1 += "z";

							for (int i = 0; i < y; i++) str2 += "z";

							int nameDifference = LevenshteinDistance.Compute(str1, str2);
							if (nameDifference < lastnameDifference)
							{
								lastnameDifference = nameDifference;
								found = player;

							}
						}
					}
				}
				return found;
			}
			catch
			{
				return null;
			}
		}
	}
	internal enum HackMode : byte
	{
		Safe,
		Hacking,
		Hacked
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
			_originalColor = _roles.Network_myColor;
		}


		private void OnDisable()
		{
			_roles.Network_myColor = _originalColor;
		}


		private void Update()
		{
			if (Time.time < _nextCycle) return;
			_nextCycle += interval;

			_roles.Network_myColor = Colors[_position];

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
	public class Module
	{
		public enum RoleType : sbyte
		{
			None = -1,
			Scp173 = 0,
			ClassD = 1,
			Spectator = 2,
			Scp106 = 3,
			NtfSpecialist = 4,
			Scp049 = 5,
			Scientist = 6,
			Scp079 = 7,
			ChaosConscript = 8,
			Scp096 = 9,
			Scp0492 = 10,
			NtfSergeant = 11,
			NtfCaptain = 12,
			NtfPrivate = 13,
			Tutorial = 14,
			FacilityGuard = 15,
			Scp93953 = 16,
			Scp93989 = 17,
			ChaosRifleman = 18,
			ChaosRepressor = 19,
			ChaosMarauder = 20,
			Scp035 = 21
		}
	}
	[Serializable]
	public class VecPos
	{
		public int sec = 0;
		public Vector3 Pos = new(0, 0, 0);
		public bool Alive { get; set; } = true;
	}
	public class GameCoreSender : CommandSender
	{
		public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
		{
			ServerConsole.AddLog($"{text}", ConsoleColor.White);
		}

		public override void Print(string text)
		{
			ServerConsole.AddLog($"{text}", ConsoleColor.White);
		}

		public string Name;
		public GameCoreSender(string name)
		{
			Name = name;
		}
		public override string SenderId => "SERVER CONSOLE";
		public override string Nickname => Name;
		public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
		public override byte KickPower => byte.MaxValue;
		public override bool FullPermissions => true;
	}
}