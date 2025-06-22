using Qurre.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Loli.Modules;
using Loli.Scps.Api;
using Loli.DataBase;
using InventorySystem.Items.Pickups;
using System.Net;
using System.IO;
using Qurre.API.Controllers;
using Qurre.API.Addons.Models;

namespace Loli
{
	public static class Extensions
	{
		public static string HttpGet(this string uri)
		{
			var request = WebRequest.Create(uri);

			using var response = request.GetResponse();
			using var stream = response.GetResponseStream();
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

		internal static readonly Dictionary<ItemPickupBase, bool> CachedItems = new();
		internal static bool ItsNeededItem(this ItemPickupBase serital)
		{
			if (CachedItems.TryGetValue(serital, out var _data)) return _data;
			var _b = Textures.Models.Rooms.Control.Buttons.ContainsKey(serital) ||
				Textures.Models.Server.Doors.ContainsKey(serital) || Textures.Models.Rooms.Servers.Doors.ContainsKey(serital);
			if (CachedItems.ContainsKey(serital)) CachedItems.Remove(serital);
			CachedItems.Add(serital, _b);
			return _b;
		}
		public static bool ItsHacker(this Player pl) => pl is not null && pl.Tag.Contains(Spawns.Roles.Hacker.HackerTag);
		public static float GetMaxHp(this Player pl)
		{
			float maxhp = pl.MaxHp;
			if (pl.ItsScp035()) maxhp = Scps.Scp035.maxHP;
			else switch (pl.Role)
				{
					case RoleType.ClassD:
						maxhp = 105;
						break;
					case RoleType.Scientist:
						maxhp = 110;
						break;
					case RoleType.ChaosConscript or RoleType.ChaosMarauder or RoleType.ChaosRepressor or RoleType.ChaosRifleman:
						maxhp = 125;
						break;
					case RoleType.NtfPrivate:
						maxhp = 115;
						break;
					case RoleType.NtfSergeant:
						maxhp = 120;
						break;
					case RoleType.NtfCaptain:
						maxhp = 125;
						break;
					case RoleType.NtfSpecialist:
						maxhp = 125;
						break;
					case RoleType.FacilityGuard:
						maxhp = 130;
						break;
					case RoleType.Tutorial:
						maxhp = 125;
						break;
					case RoleType.Scp0492:
						maxhp = 750;
						break;
					case RoleType.Scp106:
						maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp106).maxHP;
						break;
					case RoleType.Scp049:
						maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp049).maxHP;
						break;
					case RoleType.Scp096:
						maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp096).maxHP;
						break;
					case RoleType.Scp93953 or RoleType.Scp93989:
						maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
						break;
					case RoleType.Scp173:
						maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp173).maxHP;
						break;
				}
			float cf = 1;
			if (Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data))
			{
				if (Manager.Static.Data.Roles.TryGetValue(pl.UserId, out var roles) && roles.Prime) cf += 0.1f;
				if (DataBase.Modules.Data.Clans.TryGetValue(data.clan.ToUpper(), out var clan))
					foreach (int boost in clan.Boosts)
						if (boost == 3) cf += 0.05f;
			}
			return maxhp * cf;
		}
		public static void GetAmmo(this Player pl)
		{
			pl.Ammo12Gauge = 999;
			pl.Ammo44Cal = 999;
			pl.Ammo556 = 999;
			pl.Ammo762 = 999;
			pl.Ammo9 = 999;
		}
		public static bool BlockThis(this Player pl)
		{
			if (!Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data)) return false;
			return data.clan.ToLower() == "prts";
		}
		internal static System.Random Random => Qurre.API.Extensions.Random;
		public static float Difference(this float first, float second)
		{
			return Math.Abs(first - second);
		}
		internal static void TeleportTo106(this Player pl)
		{
			if (Player.List.TryFind(out var _scp, x => x.Role is RoleType.Scp106)) pl.Position = _scp.Position;
			else
			{
				List<Vector3> tp = new();
				foreach (GameObject _go in GameObject.FindGameObjectsWithTag("PD_EXIT"))
					tp.Add(_go.transform.position);
				var pos = tp[UnityEngine.Random.Range(0, tp.Count)];
				pos.y += 2f;
				pl.Position = pos;
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
						else if (EventHandlers.Pos.ContainsKey(pl.UserId) && Player.List.Count() > 5 && Round.ElapsedTime.TotalMinutes > 5)
							if (!EventHandlers.IsAlive(pl.UserId)) count--;
						count++;

					}
				}
				catch { }
			}
			return count;
		}
		public static void SetRank(this Player player, string rank, string color = "default")
		{
			player.RoleName = rank;
			player.RoleColor = color;
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
		public static Team GetTeam(this Player pl)
		{
			if (pl.ItsScp035()) return Team.SCP;
			return GetTeam(pl.Role);
		}
		public static Team GetTeam(this Module.RoleType roleType)
		{
			if (roleType is Module.RoleType.Scp035) return Team.SCP;
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
	}
	internal enum HackMode : byte
	{
		Safe,
		Hacking,
		Hacked
	}
	public class FixPrimitiveSmoothing : MonoBehaviour
	{
		internal Model Model;
		private void Update()
		{
			if (Model is null) return;
			for (int i = 0; i < Model.Primitives.Count; i++)
			{
				var prim = Model.Primitives[i];
				prim.Primitive.Base.NetworkMovementSmoothing = prim.Primitive.MovementSmoothing;
				prim.Primitive.Base.NetworkRotation = new LowPrecisionQuaternion(prim.GameObject.transform.rotation);
				prim.Primitive.Base.NetworkPosition = prim.GameObject.transform.position;
			}
		}
	}
	public class FixOnePrimitiveSmoothing : MonoBehaviour
	{
		internal Primitive Primitive;
		private void Update()
		{
			if (Primitive is null) return;
			Primitive.Base.NetworkMovementSmoothing = Primitive.MovementSmoothing;
			Primitive.Base.NetworkRotation = new LowPrecisionQuaternion(Primitive.Rotation);
			Primitive.Base.NetworkPosition = Primitive.Position;
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

		internal static float Interval { get; set; } = 0.5f;


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
			_nextCycle += Interval;

			_roles.Network_myColor = Colors[_position];

			if (++_position >= Colors.Length)
				_position = 0;
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