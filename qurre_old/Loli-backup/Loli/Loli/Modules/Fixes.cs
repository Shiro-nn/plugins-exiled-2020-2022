using InventorySystem.Configs;
using InventorySystem.Items.Firearms;
using Loli.Scps.Api;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		public void AntiEscapeBag(EscapeEvent ev)
		{
			if (Round.ElapsedTime.TotalSeconds < 10) ev.Allowed = false;
		}
		internal void AntiBloodFlood(NewBloodEvent ev)
		{
			if (!ev.Allowed) return;
			bool spawn = Bloods.Where(x => Vector3.Distance(x, ev.Position) < 1f).Count() != 0;
			if (spawn) Bloods.Add(ev.Position);
			else ev.Allowed = false;
		}
		internal void AntiScp106Bag(ContainEvent ev)
		{
			if (!ev.Allowed) return;
			DataBase.Manager.Static.Data.Contain = true;
			Timing.CallDelayed(45f, () =>
			{
				List<Player> list = Player.List.Where(x => x.Role == RoleType.Scp106).ToList();
				foreach (Player pl in list) pl.Kill("Вы были убиты, т.к у вас было 0 хп.");
			});
		}
		internal void AntiDisBalanceItems()
		{
			var picks = Map.Pickups;
			foreach (var pick in picks)
			{
				if (pick.Type == ItemType.SCP1853)
				{
					new Item(ItemType.Adrenaline).Spawn(pick.Position, pick.Rotation);
					pick.Destroy();
				}
				else if (pick.Type == ItemType.SCP244a || pick.Type == ItemType.SCP244b)
				{
					new Item(ItemType.Medkit).Spawn(pick.Position, pick.Rotation);
					pick.Destroy();
				}
			}
		}
		internal void FixLogicer(ZoomingEvent ev)
		{
			if (Plugin.Anarchy) return;
			if (ev.Item.Type == ItemType.GunLogicer && ev.Value)
			{
				ev.Player.Inventory.ServerSelectItem(0);
				Timing.CallDelayed(0.1f, () => ev.Player.Inventory.ServerSelectItem(ev.Item.Serial));
			}
		}
		internal void FixLogicer()
		{
			if (Plugin.Anarchy) return;
			foreach (var pl in Player.List)
			{
				if (pl.ItemTypeInHand is not ItemType.GunLogicer) continue;
				if (pl.ItemInHand.Base is not Firearm gun) continue;
				if (gun.Status.Attachments != 276) gun.Status = new(gun.Status.Ammo, gun.Status.Flags, 276);
			}
		}
		internal void FixLogicer(ItemChangeEvent ev)
		{
			if (Plugin.Anarchy) return;
			if (ev.NewItem == null) return;
			if (ev.NewItem.Type == ItemType.GunLogicer && ev.Allowed)
			{
				try
				{
					if (ev.NewItem.Base is Firearm gun)
						if (gun.Status.Attachments != 276) gun.Status = new(gun.Status.Ammo, gun.Status.Flags, 276);
				}
				catch { }
			}
		}
		public void FixZeroTargets(WindupEvent ev)
		{
			if (ev.Player == null || ev.Player == Server.Host || ev.Player.UserId == null) return;
			if (ev.Player.Scp096Controller.Is096 && ev.Player.Scp096Controller.Targets.Count == 0)
			{
				ev.Allowed = false;
				try { if (ev.Player.Scp096Controller.Is096) ev.Player.Scp096Controller.RageState = PlayableScps.Scp096PlayerState.Calming; } catch { }
			}
		}
		public void FixZeroTargets(PreWindupEvent ev)
		{
			if (ev.Player == null || ev.Player == Server.Host || ev.Player.UserId == null) return;
			if (ev.Player.Scp096Controller.Is096 && ev.Player.Scp096Controller.Targets.Count == 0)
			{
				ev.Allowed = false;
				try { if (ev.Player.Scp096Controller.Is096) ev.Player.Scp096Controller.RageState = PlayableScps.Scp096PlayerState.Calming; } catch { }
			}
		}
		public void FixRageScp096(AddTargetEvent ev)
		{
			if (ev.Target == null) return;
			if (ev.Target.Tag.Contains(Spawns.SerpentsHand.HandTag) || ev.Target.ItsScp035()) ev.Allowed = false;
		}
		internal void Dying(DiesEvent ev)
		{
			if (!Round.Started && Round.ElapsedTime.TotalSeconds < 1)
				ev.Allowed = false;
		}
		internal void Hurt(DamageEvent ev)
		{
			if (!Round.Started && Round.ElapsedTime.TotalSeconds < 1)
				ev.Allowed = false;
		}
		internal void Scp914(UpgradeEvent ev)
		{
			foreach (Player pl in ev.Players.Where(x => x.Role == RoleType.Scp0492))
			{
				if (!Upgrade914.ContainsKey(pl.UserId)) Upgrade914.Add(pl.UserId, true);
				else Upgrade914[pl.UserId] = true;
				pl.GodMode = true;
				pl.SetRole(RoleType.ClassD, true);
				pl.Hp = 25;
				pl.MaxHp = (int)pl.GetMaxHp();
				pl.ClearInventory();
				Timing.CallDelayed(0.5f, () =>
				{
					pl.Hp = 25;
					pl.GodMode = false;

				});
				if (Plugin.RolePlay)
				{
					pl.DisplayNickname = "D-class ????";
					pl.UnitName = $"Прозвище: <color=red>{pl.Nickname.Replace("<", "").Replace(">", "")}</color>";
				}
			}
		}
		internal void AntiTeamCuff(CuffEvent ev)
		{
			if (Plugin.RolePlay) return;
			if (ev.Cuffer.GetTeam() == Team.MTF && ev.Target.GetTeam() == Team.RSC) ev.Allowed = false;
			else if (ev.Cuffer.GetTeam() == Team.CHI && ev.Target.GetTeam() == Team.CDP) ev.Allowed = false;
		}
		internal void AntiFlood(DropAmmoEvent ev) => ev.Allowed = false;
		internal void AntiAmmo(CreatePickupEvent ev)
        {
			if (ev.Info.ItemId == ItemType.Ammo12gauge || ev.Info.ItemId == ItemType.Ammo44cal ||
				ev.Info.ItemId == ItemType.Ammo556x45 || ev.Info.ItemId == ItemType.Ammo762x39 ||
				ev.Info.ItemId == ItemType.Ammo9x19) ev.Allowed = false;
        }
		internal void FixAmmoFlood(DiesEvent ev)
		{
			if (!ev.Allowed) return;
			ev.Target.Ammo12Gauge = 0;
			ev.Target.Ammo44Cal = 0;
			ev.Target.Ammo556 = 0;
			ev.Target.Ammo762 = 0;
			ev.Target.Ammo9 = 0;
			var _ammo = ev.Target.AllItems.Where(x => x.Category == ItemCategory.Ammo);
			foreach (var ammo in _ammo) try { ev.Target.RemoveItem(ammo); } catch { }
		}
		internal void ScpDeadFix(ConvertUnitNameEvent ev)
        {
			if (ev.UnitName.ToLower().Contains("разведгруппа")) ev.UnitName = "the first mobile task force group";
			else if (ev.UnitName.ToLower().Contains("аварийный отряд")) ev.UnitName = "emergency mobile task force squad";
			else if (ev.UnitName.ToLower().Contains("охрана")) ev.UnitName = "Guard";
			else if (ev.UnitName.ToLower().Contains("fydne")) ev.UnitName = "mobile task force";
		}
		internal void FixFF(DamageProcessEvent ev)
		{
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (Server.FriendlyFire) return;
			if (Round.Ended) return;
			if (ev.Target == null || ev.Attacker == null) return;
			if (!Ally(ev.Target.GetTeam(), ev.Attacker.GetTeam())) return;
			ev.Allowed = false;
			ev.Amount = 0;
			ev.FriendlyFire = true;
			static bool Ally(Team killer, Team target)
			{
				if (killer == target) return true;
				if ((killer == Team.MTF || killer == Team.RSC) && (target == Team.MTF || target == Team.RSC)) return true;
				if ((killer == Team.CHI || killer == Team.CDP) && (target == Team.CHI || target == Team.CDP)) return true;
				return false;
			}
		}
		internal void FixInvisible(ItemChangeEvent ev)
		{
			if (ev.NewItem == null) return;
			if (!ev.Player.GetEffectActive<CustomPlayerEffects.Invisible>()) return;
			if(ev.NewItem.Category != ItemCategory.Firearm && ev.NewItem.Category != ItemCategory.Grenade &&
				ev.NewItem.Category != ItemCategory.SCPItem && ev.NewItem.Category != ItemCategory.MicroHID) return;
			ev.Player.DisableEffect<CustomPlayerEffects.Invisible>();
		}
		internal void FixInvisible(ShootingEvent ev)
		{
			if (ev.Shooter.GetEffectActive<CustomPlayerEffects.Invisible>()) ev.Shooter.DisableEffect<CustomPlayerEffects.Invisible>();
		}
		internal void FixRagdollScale(RagdollSpawnEvent ev)
        {
			if (ev.Owner == null || ev.Owner == Server.Host) return;
			var s1 = ev.Ragdoll.Scale;
			var s2 = ev.Owner.Scale;
			ev.Ragdoll.Scale = new Vector3(s1.x * s2.x, s1.y * s2.y, s1.z * s2.z);
        }
		internal void FixItemsLimits()
		{
			if (Plugin.RolePlay)
			{
				InventoryLimits.StandardAmmoLimits[ItemType.Ammo9x19] = 75;
				InventoryLimits.StandardAmmoLimits[ItemType.Ammo556x45] = 200;
				InventoryLimits.StandardAmmoLimits[ItemType.Ammo762x39] = 150;
				InventoryLimits.StandardAmmoLimits[ItemType.Ammo44cal] = 30;
				InventoryLimits.StandardAmmoLimits[ItemType.Ammo12gauge] = 70;

				InventoryLimits.StandardCategoryLimits[ItemCategory.Armor] = -1;
				InventoryLimits.StandardCategoryLimits[ItemCategory.Grenade] = 2;
				InventoryLimits.StandardCategoryLimits[ItemCategory.Keycard] = 3;
				InventoryLimits.StandardCategoryLimits[ItemCategory.Medical] = 3;
				InventoryLimits.StandardCategoryLimits[ItemCategory.MicroHID] = -1;
				InventoryLimits.StandardCategoryLimits[ItemCategory.Radio] = -1;
				InventoryLimits.StandardCategoryLimits[ItemCategory.SCPItem] = 3;
				InventoryLimits.StandardCategoryLimits[ItemCategory.Firearm] = 1;
			}
			else
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
		}
	}
}