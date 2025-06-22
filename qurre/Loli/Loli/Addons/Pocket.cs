using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events.Structs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.Addons
{
    static class Pocket
	{
		static Pocket()
		{
			CommandsSystem.RegisterConsole("pocket", Console);
		}

		static void Console(GameConsoleCommandEvent ev)
		{
			if (ev.Name != "pocket")
				return;

			ev.Allowed = false;

			if (ev.Player.RoleInfomation.Role is not RoleTypeId.Scp106)
			{
				ev.Reply = "Вы не являетесь SCP 106";
				ev.Color = "red";
				return;
			}

			ev.Reply = "Успешно";
			ev.Color = "green";
			ev.Player.RoleInfomation.Scp106.PortalPosition = new(0, -2001, 0);

			Timing.CallDelayed(0.2f, () =>
			{
				if (ev.Player.RoleInfomation.Role != RoleTypeid.Scp106)
					return;

				ev.Player.RoleInfomation.Scp106.PortalPosition = new(0, -2001, 0);
				ev.Player.RoleInfomation.Scp106.UsePortal();
			});
		}

		[EventMethod()]
		static void AntiCicleDamage(PocketEnterEvent ev)
		{
			try
			{
				if (ev.Player.Role != RoleType.Scp106 && ev.Player.Room.Type == RoomType.Pocket)
					ev.Allowed = false;
			}
			catch { }
		}

		[EventMethod()]
		static void Scp106Lure(PocketEscapeEvent ev)
		{
			if (ev.Player.Role != RoleType.Scp106) return;
			ev.Allowed = false;
			ev.Player.Scp106Controller.PortalPosition = Respawn.GetPosition(RoleTypeId.Scp106) - (Vector3.up * 3);
			Timing.CallDelayed(0.2f, () =>
			{
				if (ev.Player.Role != RoleType.Scp106) return;
				ev.Player.Scp106Controller.PortalPosition = Respawn.GetPosition(RoleTypeId.Scp106) - (Vector3.up * 3);
				ev.Player.Scp106Controller.UsePortal();
			});
		}

		[EventMethod()]
		static void TeleportLure(PocketFailEscapeEvent ev)
		{
			if (ev.Player.Role == RoleType.Scp106)
			{
				ev.Allowed = false;
				ev.Player.Scp106Controller.PortalPosition = Respawn.GetPosition(RoleTypeId.Scp106) - (Vector3.up * 3);
				Timing.CallDelayed(0.2f, () =>
				{
					if (ev.Player.Role != RoleType.Scp106) return;
					ev.Player.Scp106Controller.PortalPosition = Respawn.GetPosition(RoleTypeId.Scp106) - (Vector3.up * 3);
					ev.Player.Scp106Controller.UsePortal();
				});
			}
			else
			{
				if (!ev.Allowed) return;
				var list = Map.Rooms.Where(x => x.Zone == ZoneType.Light && (x.Shape == RoomShape.Straight || x.Shape == RoomShape.XShape ||
				x.Shape == RoomShape.TShape || x.Type == RoomType.LczPlants)).ToArray();
				var pos = list[Random.Range(0, list.Count() - 1)].Position + (Vector3.up * 5);
				List<Item> itms = new();
				foreach (var itm in ev.Player.AllItems) if (itm.Category != ItemCategory.Ammo) itms.Add(itm);
				foreach (var item in itms)
				{
					try
					{
						if (item.Type == ItemType.KeycardContainmentEngineer || item.Type == ItemType.KeycardFacilityManager)
						{
							ev.Player.RemoveItem(item);
							NewItems.SpawnInLure();
						}
						else if (item.Category == ItemCategory.Firearm || item.Category == ItemCategory.Armor || item.Category == ItemCategory.MicroHID || item.Category == ItemCategory.Grenade)
						{
							ev.Player.RemoveItem(item);
							item.Spawn(new Vector3(0, -1995, 0));
						}
						else
						{
							ev.Player.RemoveItem(item);
							item.Spawn(pos);
						}
					}
					catch { }
				}
				ev.Allowed = false;
				Timing.CallDelayed(0.1f, () => ev.Player.Position = pos);
				Timing.CallDelayed(0.2f, () => ev.Player.Kill(DeathTranslations.PocketDecay));
			}
		}
	}
}