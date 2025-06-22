using Loli.Addons;
using Loli.DataBase.Modules;
using PlayerRoles;
using Qurre.API;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Loli
{
	static class Extensions
	{
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

		static internal float GetMaxHp(this Player pl)
		{
			float maxhp = pl.HealthInfomation.MaxHp;
			switch ((RoleType)pl.RoleInfomation.Role)
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
			}

			return maxhp * cf;
		}

		static internal void GetAmmo(this Player pl)
		{
			pl.Inventory.Base.UserInventory.ReserveAmmo[ItemType.Ammo12gauge] = 999;
			pl.Inventory.Base.UserInventory.ReserveAmmo[ItemType.Ammo44cal] = 999;
			pl.Inventory.Base.UserInventory.ReserveAmmo[ItemType.Ammo556x45] = 999;
			pl.Inventory.Base.UserInventory.ReserveAmmo[ItemType.Ammo762x39] = 999;
			pl.Inventory.Base.UserInventory.ReserveAmmo[ItemType.Ammo9x19] = 999;
			pl.Inventory.Base.SendAmmoNextFrame = true;
		}


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
						if (Caches.Positions.ContainsKey(pl.UserInfomation.UserId) && Player.List.Count() > 5 && Round.ElapsedTime.TotalMinutes > 5)
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
			return (RoleType)player.RoleInfomation.Role;
		}
		static internal Team GetTeam(this Player pl)
		{
			return GetTeam(pl.RoleInfomation.Role);
		}
		static internal Team GetTeam(this RoleType roleType)
		{
			if (roleType is RoleType.Scp035) return Team.SCPs;
			return GetTeam((RoleTypeId)roleType);
		}
		static internal Team GetTeam(this RoleTypeId roleType)
			=> PlayerRolesUtils.GetTeam(roleType);
	}
}