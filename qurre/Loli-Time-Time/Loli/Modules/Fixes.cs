using CustomPlayerEffects;
using InventorySystem.Configs;
using InventorySystem.Items.Firearms;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Linq;

namespace Loli.Modules
{
	static class Fixes
	{
		static internal void AutoEndZeroPlayers()
		{
			if (Round.Started && !Round.Ended && Player.List.Count() == 0)
				Round.Restart();
		}
		static internal void FixLogicer()
		{
			foreach (var pl in Player.List)
			{
				if (pl.Inventory.Base.CurInstance.ItemTypeId is not ItemType.GunLogicer)
					continue;
				if (pl.Inventory.Base.CurInstance is not Firearm gun)
					continue;

				if (gun.Status.Attachments != 276)
					gun.Status = new(gun.Status.Ammo, gun.Status.Flags, 276);
			}
		}

		[EventMethod(PlayerEvents.ChangeItem)]
		static void FixLogicer(ChangeItemEvent ev)
		{
			if (ev.NewItem == null)
				return;

			if (ev.NewItem.Type == ItemType.GunLogicer && ev.Allowed)
			{
				try
				{
					if (ev.NewItem.Base is Firearm gun)
						if (gun.Status.Attachments != 276)
							gun.Status = new(gun.Status.Ammo, gun.Status.Flags, 276);
				}
				catch { }
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

		[EventMethod(MapEvents.CreatePickup)]
		static void AntiAmmo(CreatePickupEvent ev)
		{
			if (ev.Info.ItemId == ItemType.Ammo12gauge || ev.Info.ItemId == ItemType.Ammo44cal ||
				ev.Info.ItemId == ItemType.Ammo556x45 || ev.Info.ItemId == ItemType.Ammo762x39 ||
				ev.Info.ItemId == ItemType.Ammo9x19) ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Attack)]
		static void FixFF(AttackEvent ev)
		{
			if (Server.FriendlyFire)
				return;
			if (Round.Ended)
				return;
			if (ev.Target == null || ev.Attacker == null)
				return;
			if (ev.Target.GetTeam().GetFaction() != ev.Attacker.GetTeam().GetFaction())
				return;

			ev.Allowed = false;
			ev.Damage = 0;
			ev.FriendlyFire = true;

			Log.Info($"FF DETECTOR: Target: {ev.Target.UserInfomation.Nickname}; Attacker: {ev.Attacker.UserInfomation.Nickname}");
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

		[EventMethod(MapEvents.RagdollSpawned)]
		static void FixRagdollScale(RagdollSpawnedEvent ev)
		{
			if (ev.Ragdoll.Owner == null || ev.Ragdoll.Owner == Server.Host)
				return;

			var s1 = ev.Ragdoll.Scale;
			var s2 = ev.Ragdoll.Owner.MovementState.Scale;
			ev.Ragdoll.Scale = new(s1.x * s2.x, s1.y * s2.y, s1.z * s2.z);
		}

		[EventMethod(RoundEvents.Waiting)]
		static void FixItemsLimits()
		{
			InventoryLimits.StandardAmmoLimits[ItemType.Ammo9x19] = 9999;
			InventoryLimits.StandardAmmoLimits[ItemType.Ammo556x45] = 9999;
			InventoryLimits.StandardAmmoLimits[ItemType.Ammo762x39] = 9999;
			InventoryLimits.StandardAmmoLimits[ItemType.Ammo44cal] = 9999;
			InventoryLimits.StandardAmmoLimits[ItemType.Ammo12gauge] = 9999;

			InventoryLimits.StandardCategoryLimits[ItemCategory.Armor] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.Grenade] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.Keycard] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.Medical] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.MicroHID] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.Radio] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.SCPItem] = 8;
			InventoryLimits.StandardCategoryLimits[ItemCategory.Firearm] = 8;
		}

		[EventMethod(PlayerEvents.PickupItem)]
		static void FixFreeze(PickupItemEvent ev)
		{
			if (ev.Player.Inventory.Base.UserInventory.Items.Any(x => x.Value.ItemSerial == ev.Pickup.Serial))
			{
				//Qurre.Log.Warn("Player_PickupItem SERVER CRASH ROUND => " + ev.Pickup.Serial.ToString() + ", TYPE  => " + ev.Pickup.Type.ToString());
				ev.Allowed = false;
				var item = ev.Player.Inventory.AddItem(ev.Pickup.Type);
				var ser = ev.Pickup.Serial;
				ev.Pickup.Destroy();
				if (Clear.Pickups.Contains(ser))
					Clear.Pickups.Add(item.Serial);
			}
		}
	}
}