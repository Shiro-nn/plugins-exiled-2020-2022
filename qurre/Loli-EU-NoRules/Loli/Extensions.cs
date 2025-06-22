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

namespace Loli
{
	static class Extensions
	{
		[EventMethod(RoundEvents.Waiting)]
		static void ClearCache()
		{
			CachedItems.Clear();
		}

		[EventMethod(RoundEvents.Start)]
		static void AddCache()
		{
			MEC.Timing.CallDelayed(6f, () =>
			{
				foreach (var pick in Map.Pickups)
					CachedItems.Add(pick.Base, true);
			});
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

			var _b = Builds.Models.Server.Doors.ContainsKey(serital);

			if (CachedItems.ContainsKey(serital))
				CachedItems.Remove(serital);

			CachedItems.Add(serital, _b);

			return _b;
		}

		static internal float GetMaxHp(this Player pl)
		{
			float maxhp = pl.HealthInfomation.MaxHp;
			switch (pl.RoleInfomation.Role)
				{
					case RoleTypeId.ClassD:
						maxhp = 105;
						break;
					case RoleTypeId.Scientist:
						maxhp = 110;
						break;
					case RoleTypeId.ChaosConscript or RoleTypeId.ChaosMarauder or RoleTypeId.ChaosRepressor or RoleTypeId.ChaosRifleman:
						maxhp = 125;
						break;
					case RoleTypeId.NtfPrivate:
						maxhp = 115;
						break;
					case RoleTypeId.NtfSergeant:
						maxhp = 120;
						break;
					case RoleTypeId.NtfCaptain:
						maxhp = 125;
						break;
					case RoleTypeId.NtfSpecialist:
						maxhp = 125;
						break;
					case RoleTypeId.FacilityGuard:
						maxhp = 130;
						break;
					case RoleTypeId.Tutorial:
						maxhp = 125;
						break;
					case RoleTypeId.Scp0492:
						maxhp = 750;
						break;
					case RoleTypeId.Scp106:
						maxhp = -1;
						break;
					case RoleTypeId.Scp049:
						maxhp = -1;
						break;
					case RoleTypeId.Scp096:
						maxhp = -1;
						break;
					case RoleTypeId.Scp939:
						maxhp = -1;
						break;
					case RoleTypeId.Scp173:
						maxhp = -1;
						break;
				}

			float cf = 1;
			if (Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var roles) && roles.Prime)
				cf += 0.1f;

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

		static internal bool ItsHacker(this Player pl)
			=> pl is not null && pl.Tag.Contains(Spawns.Roles.Hacker.HackerTag);
		static internal Team GetTeam(this Player pl)
			=> PlayerRolesUtils.GetTeam(pl.RoleInfomation.Role);
		static internal Team GetTeam(this RoleTypeId roleType)
			=> PlayerRolesUtils.GetTeam(roleType);
	}
}