using CustomPlayerEffects;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Linq;
using UnityEngine;

namespace Loli.Modules
{
	static class Fixes
	{
		static internal void AutoEndZeroPlayers()
		{
			if (Round.Started && !Round.Ended && Player.List.Count() == 0)
				Server.Restart();
		}

		[EventMethod(PlayerEvents.Spawn, int.MinValue)]
		static void FixZombieSpawn(SpawnEvent ev)
		{
			if (ev.Role != RoleTypeId.Scp0492)
				return;

			if (Vector3.Distance(ev.Position, Vector3.zero) > 3 &&
				Vector3.Distance(ev.Position, Vector3.down * 2000) > 3)
				return;

			ev.Position = GetZombiePoint();

			static Vector3 GetZombiePoint()
			{
				if (Player.List.TryFind(out var doctor, x => x.RoleInfomation.Role == RoleTypeId.Scp049))
					return doctor.MovementState.Position;
				if (Map.Rooms.TryFind(out var room, x => x.Type == Qurre.API.Objects.RoomType.Hcz049))
					return room.Position + (Vector3.up * 2);

				return new(86, 989, -69);
			}
		}

		[EventMethod(PlayerEvents.Escape)]
		static void AntiEscapeBag(EscapeEvent ev)
		{
			if (Round.ElapsedTime.TotalSeconds < 10)
				ev.Allowed = false;
		}

		[EventMethod(RoundEvents.Start)]
		static void AntiDisBalanceItems()
		{
			var picks = Map.Pickups;
			foreach (var pick in picks)
			{
				if (pick.Type == ItemType.SCP1853)
				{
					new Item(ItemType.SCP330).Spawn(pick.Position, pick.Rotation);
					pick.Destroy();
				}
				else if (pick.Type == ItemType.SCP244a || pick.Type == ItemType.SCP244b)
				{
					new Item(ItemType.SCP018).Spawn(pick.Position, pick.Rotation);
					pick.Destroy();
				}
			}
		}

		[EventMethod(PlayerEvents.Dies)]
		static void Dies(DiesEvent ev)
		{
			if (!Round.Started && Round.ElapsedTime.TotalSeconds < 1)
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Cuff)]
		static void AntiTeamCuff(CuffEvent ev)
		{
			if (ev.Cuffer.GetTeam().GetFaction() == ev.Target.GetTeam().GetFaction())
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.DropAmmo)]
		static void AntiFlood(DropAmmoEvent ev) => ev.Allowed = false;

		[EventMethod(MapEvents.CreatePickup, int.MinValue)]
		static void AntiAmmo(CreatePickupEvent ev)
		{
			if (ev.Info.ItemId == ItemType.Ammo12gauge || ev.Info.ItemId == ItemType.Ammo44cal ||
				ev.Info.ItemId == ItemType.Ammo556x45 || ev.Info.ItemId == ItemType.Ammo762x39 ||
				ev.Info.ItemId == ItemType.Ammo9x19) ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.ChangeItem)]
		static void FixInvisible(ChangeItemEvent ev)
		{
			if (ev.NewItem == null)
				return;

			if (!ev.Player.Effects.CheckActive<Invisible>())
				return;

			if (ev.NewItem.Category != ItemCategory.Firearm && ev.NewItem.Category != ItemCategory.Grenade &&
				ev.NewItem.Category != ItemCategory.SCPItem && ev.NewItem.Category != ItemCategory.MicroHID)
				return;

			ev.Player.Effects.Disable<Invisible>();
		}

		[EventMethod(PlayerEvents.PickupItem)]
		static void FixFreeze(PickupItemEvent ev)
		{
			if (ev.Player.Inventory.Base.UserInventory.Items.Any(x => x.Value.ItemSerial == ev.Pickup.Serial))
			{
				ev.Allowed = false;
				var item = ev.Player.Inventory.AddItem(ev.Pickup.Type);
				var ser = ev.Pickup.Serial;
				ev.Pickup.Destroy();
				if (Clear.Pickups.Contains(ser))
					Clear.Pickups.Add(item.Serial);
			}
		}

		[EventMethod(ServerEvents.RemoteAdminCommand, int.MaxValue)]
		static void FixCrashes(RemoteAdminCommandEvent ev)
		{
			if (ev.Sender.SenderId != "SERVER CONSOLE")
				return;

			switch (ev.Name)
			{
				case "forceclass": ev.Allowed = false; return;
				case "give": ev.Allowed = false; return;
				default: return;
			}
		}

		[EventMethod(RoundEvents.Start)]
		static void FixNotSpawn()
		{
			Timing.CallDelayed(1f, () =>
			{
				int pls = Player.List.Count();
				int pls2 = Player.List.Count(x => x.RoleInfomation.IsAlive || x.RoleInfomation.Role == RoleTypeId.Overwatch);
				if (pls == 0 || pls / 1.5 > pls2)
				{
					Timing.CallDelayed(5f, () =>
					{
						foreach (Player pl in Player.List)
							pl.Client.DimScreen();
						Timing.CallDelayed(1f, () => Server.Restart());
					});
					try { RoundSummary.singleton.RpcShowRoundSummary(RoundSummary.singleton.classlistStart, default, LeadingTeam.Draw, 0, 0, 0, 5, 1); } catch { }
				}
			});
		}
	}
}