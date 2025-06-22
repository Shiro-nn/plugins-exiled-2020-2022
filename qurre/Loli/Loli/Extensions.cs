using InventorySystem.Items.Pickups;
using Loli.Addons;
using Loli.DataBase.Modules;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Loli
{
	static class Extensions
	{
		[EventMethod(RoundEvents.Waiting)]
		static void ClearCache()
		{
			CachedItems.Clear();
		}

		static internal string HttpGet(this string uri)
		{
			var request = WebRequest.Create(uri);

			using var response = request.GetResponse();
			using var stream = response.GetResponseStream();
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
		static internal float Difference(this float first, float second)
		{
			return Math.Abs(first - second);
		}

		static internal readonly Dictionary<ItemPickupBase, bool> CachedItems = new();
		static internal bool ItsNeededItem(this ItemPickupBase serital)
		{
			if (CachedItems.TryGetValue(serital, out var _data))
				return _data;

			var _b = Textures.Models.Rooms.Control.Buttons.ContainsKey(serital) ||
				Textures.Models.Server.Doors.ContainsKey(serital) || Textures.Models.Rooms.Servers.Doors.ContainsKey(serital);

			if (CachedItems.ContainsKey(serital))
				CachedItems.Remove(serital);

			CachedItems.Add(serital, _b);

			return _b;
		}

		static internal float GetMaxHp(this Player pl)
		{
			float maxhp = pl.HealthInfomation.MaxHp;
			if (pl.ItsScp035())
				maxhp = Scps.Scp035.maxHP;
			else switch ((RoleType)pl.RoleInfomation.Role)
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
						maxhp = -1;
						break;
					case RoleType.Scp049:
						maxhp = -1;
						break;
					case RoleType.Scp096:
						maxhp = -1;
						break;
					case RoleType.Scp939:
						maxhp = -1;
						break;
					case RoleType.Scp173:
						maxhp = -1;
						break;
				}

			float cf = 1;
			if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var data))
			{
				if (Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var roles) && roles.Prime)
					cf += 0.1f;
				if (Data.Clans.TryGetValue(data.clan.ToUpper(), out var clan))
					foreach (int boost in clan.Boosts)
						if (boost == 3)
							cf += 0.05f;
			}

			return maxhp * cf;
		}

		static internal void GetAmmo(this Player pl)
		{
			pl.Inventory.Ammo.Ammo12Gauge = 999;
			pl.Inventory.Ammo.Ammo44Cal = 999;
			pl.Inventory.Ammo.Ammo556 = 999;
			pl.Inventory.Ammo.Ammo762 = 999;
			pl.Inventory.Ammo.Ammo9 = 999;
		}

		static internal bool ItsHacker(this Player pl)
			=> pl is not null && pl.Tag.Contains(Spawns.Roles.Hacker.HackerTag);

		static internal void SetRank(this Player player, string rank, string color = "default")
		{
			player.Administrative.RoleName = rank;
			player.Administrative.RoleColor = color;
		}

		static internal int CountRoles(this Team team)
		{
			int count = 0;
			foreach (var pl in Player.List)
			{
				try
				{
					if (pl.GetTeam() == team)
					{
						if (pl.ItsScp035()) count--;
						else if (Caches.Positions.ContainsKey(pl.UserInfomation.UserId) && Player.List.Count() > 5 && Round.ElapsedTime.TotalMinutes > 5)
							if (!Caches.IsAlive(pl.UserInfomation.UserId)) count--;
						count++;
					}
				}
				catch { }
			}
			return count;
		}

		static internal RoleType GetCustomRole(this Player player)
		{
			if (player.ItsScp035())
				return RoleType.Scp035;
			return (RoleType)player.RoleInfomation.Role;
		}
		static internal Team GetTeam(this Player pl)
		{
			if (pl.ItsScp035()) return Team.SCPs;
			return GetTeam(pl.RoleInfomation.Role);
		}
		static internal Team GetTeam(this RoleType roleType)
		{
			if (roleType is RoleType.Scp035) return Team.SCPs;
			return GetTeam((RoleTypeId)roleType);
		}
		static internal Team GetTeam(this RoleTypeId roleType)
			=> PlayerRolesUtils.GetTeam(roleType);

		static internal List<Player> Get035()
			=> Player.List.Where(x => x.Tag.Contains(Scps.Scp035.Tag) && x.RoleInfomation.Role != RoleTypeId.Spectator).ToList();
		static internal bool ItsScp035(this Player pl)
			=> pl.Tag.Contains(Scps.Scp035.Tag);
	}
}