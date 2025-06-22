using UnityEngine;
using Qurre.API.Events;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Loli.BetterHints;
namespace Loli.DataBase
{
	[Serializable]
	public class ShopItem
	{
		public ItemType Type = ItemType.None;
		public int Sum = 0;
		public Pickup Pickup;
		public Vector3 Position = Vector3.zero;
		internal ShopItem(ItemType type, int sum, Pickup pickup, Vector3 pos)
		{
			Type = type;
			Sum = sum;
			Pickup = pickup;
			Position = pos;
		}
	}
	public class Shop
	{
		public static readonly List<ShopItem> Items = new();
		public static bool ItsShop(Pickup p)
		{
			if (Items.Where(x => x.Pickup == p).Count() > 0) return true;
			return false;
		}
		public void Refresh()
		{
			MEC.Timing.CallDelayed(1f, () => Spawn());
		}
		public void Refresh(RoundEndEvent _) => Clear();
		internal void Pickup(PickupItemEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Allowed) return;
			if (!Manager.Static.Data.Users.TryGetValue(ev.Player.UserId, out var _main)) return;
			if (ev.Pickup.Type == ItemType.Coin)
			{
				ev.Allowed = false;

				Manager.Static.Stats.AddMoney(ev.Player, 1);
				ev.Pickup.Destroy();
				ev.Player.Hint(new(-20, 6, $"<align=left><size=70%><color=#0089c7>+монетка</color></size></align>", 5, false));
				return;
			}
			if (!ItsShop(ev.Pickup)) return;
			ShopItem item = Items.Find(x => x.Pickup == ev.Pickup);
			if (item == null) return;
			ev.Allowed = false;
			Postfix(ev.Player, item.Sum, item.Type);
			void Postfix(Player pl, int money, ItemType item)
			{
				if (!Manager.Static.Data.Users.TryGetValue(ev.Player.UserId, out var _main)) return;
				if (_main.money >= money)
				{
					Manager.Static.Stats.AddMoney(ev.Player, 0 - money);
					SendBC(pl, money, 0, true);
					pl.AddItem(item);
				}
				else SendBC(pl, money, _main.money, false);
			}
			void SendBC(Player pl, int money, int itsmoney, bool suc)
			{
				string text = $"<align=left><size=70%><color=#6f6f6f>Не хватает монет(<color=red>{itsmoney}/{money}</color>)</color></size></align>";
				if (suc) text = $"<align=left><size=70%><color=#6f6f6f>Вы успешно купили этот товар за <color=red>{money}</color> монет</color></size></align>";
				pl.Hint(new(-20, 6, text, 5, false));
			}
		}
		public static void Spawn()
		{
			Vector3 SpawnCard = Map.GetRandomSpawnPoint(RoleType.Scp049);
			Vector3 SpawnGun = Map.GetRandomSpawnPoint(RoleType.Scp93989);
			Vector3 SpawnMed = Map.GetRandomSpawnPoint(RoleType.Scp173);
			Vector3 SpawnScp = Map.GetRandomSpawnPoint(RoleType.Scp096);
			Vector3 SpawnEtc = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
			if (Extensions.Random.Next(1, 100) < 20)
			{
				SpawnCard = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
				SpawnGun = Map.GetRandomSpawnPoint(RoleType.Scp049);
				SpawnMed = Map.GetRandomSpawnPoint(RoleType.Scp93989);
				SpawnScp = Map.GetRandomSpawnPoint(RoleType.Scp173);
				SpawnEtc = Map.GetRandomSpawnPoint(RoleType.Scp096);
				Map.Broadcast($"<size=30%><color=#31d400>Магазин появился на <color=red>случайном</color> спавне</color></size>", 10);
			}
			else if (Extensions.Random.Next(1, 100) < 40)
			{
				SpawnCard = Map.GetRandomSpawnPoint(RoleType.Scp096);
				SpawnGun = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
				SpawnMed = Map.GetRandomSpawnPoint(RoleType.Scp049);
				SpawnScp = Map.GetRandomSpawnPoint(RoleType.Scp93989);
				SpawnEtc = Map.GetRandomSpawnPoint(RoleType.Scp173);
				Map.Broadcast($"<size=30%><color=#ffb500>Магазин появился на <color=red>случайном</color> спавне</color></size>", 10);
			}
			else if (Extensions.Random.Next(1, 100) < 60)
			{
				SpawnCard = Map.GetRandomSpawnPoint(RoleType.Scp93989);
				SpawnGun = Map.GetRandomSpawnPoint(RoleType.Scp096);
				SpawnMed = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
				SpawnScp = Map.GetRandomSpawnPoint(RoleType.Scp049);
				SpawnEtc = Map.GetRandomSpawnPoint(RoleType.Scp173);
				Map.Broadcast($"<size=30%><color=#fdffbb>Магазин появился на <color=red>случайном</color> спавне</color></size>", 10);
			}
			else if (Extensions.Random.Next(1, 100) < 80)
			{
				SpawnCard = Map.GetRandomSpawnPoint(RoleType.Scp93989);
				SpawnGun = Map.GetRandomSpawnPoint(RoleType.Scp096);
				SpawnMed = Map.GetRandomSpawnPoint(RoleType.Scp173);
				SpawnScp = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
				SpawnEtc = Map.GetRandomSpawnPoint(RoleType.Scp049);
				Map.Broadcast($"<size=30%><color=#0089c7>Магазин появился на <color=red>случайном</color> спавне</color></size>", 10);
			}
			else Map.Broadcast($"<size=30%><color=#989dff>Магазин появился на <color=red>случайном</color> спавне</color></size>", 10);

			PostSpawn(ItemType.KeycardContainmentEngineer, 45, SpawnCard + Vector3.forward * 0.5f + Vector3.right * 0.5f);
			PostSpawn(ItemType.KeycardNTFLieutenant, 30, SpawnCard + Vector3.forward * 0.5f + Vector3.left * 0.5f);
			PostSpawn(ItemType.KeycardNTFCommander, 45, SpawnCard + Vector3.right * 0.5f);
			PostSpawn(ItemType.KeycardFacilityManager, 60, SpawnCard + Vector3.left * 0.5f);

			PostSpawn(ItemType.GunFSP9, 55, SpawnGun + Vector3.forward + Vector3.right);
			PostSpawn(ItemType.GunCOM15, 30, SpawnGun + Vector3.forward + Vector3.left);
			PostSpawn(ItemType.GunCrossvec, 50, SpawnGun + Vector3.right);
			PostSpawn(ItemType.GunE11SR, 60, SpawnGun);
			PostSpawn(ItemType.GunCOM18, 40, SpawnGun + Vector3.left);
			PostSpawn(ItemType.GunLogicer, 70, SpawnGun + Vector3.back + Vector3.right);
			PostSpawn(ItemType.GunAK, 60, SpawnGun + Vector3.back + Vector3.left);
			PostSpawn(ItemType.GunShotgun, 60, SpawnGun + Vector3.back + Vector3.back + Vector3.left);
			PostSpawn(ItemType.GunRevolver, 50, SpawnGun + Vector3.back + Vector3.back + Vector3.right);

			PostSpawn(ItemType.Medkit, 15, SpawnMed + Vector3.forward * 0.5f + Vector3.right * 0.5f);
			PostSpawn(ItemType.Adrenaline, 15, SpawnMed + Vector3.forward * 0.5f + Vector3.left * 0.5f);
			PostSpawn(ItemType.Painkillers, 10, SpawnMed + Vector3.right * 0.5f);
			PostSpawn(ItemType.Flashlight, 5, SpawnMed + Vector3.left * 0.5f);

			PostSpawn(ItemType.SCP500, 30, SpawnScp + Vector3.forward * 0.5f + Vector3.left * 0.5f);
			PostSpawn(ItemType.SCP207, 30, SpawnScp + Vector3.right * 0.5f);
			PostSpawn(ItemType.SCP330, 50, SpawnScp);

			PostSpawn(ItemType.Radio, 10, SpawnEtc + Vector3.forward * 0.5f + Vector3.right * 0.5f);
			PostSpawn(ItemType.GrenadeFlash, 15, SpawnEtc + Vector3.forward * 0.5f + Vector3.left * 0.5f);
			PostSpawn(ItemType.GrenadeHE, 30, SpawnEtc + Vector3.right * 0.5f);
			PostSpawn(ItemType.KeycardJanitor, 5, SpawnEtc + Vector3.left * 0.5f);

			static void PostSpawn(ItemType type, int sum, Vector3 pos) => Items.Add(new ShopItem(type, sum, new Item(type).Spawn(pos), pos));
		}
		internal void Clear()
		{
			foreach (ShopItem item in Items) try { item.Pickup.Destroy(); } catch { }
			Items.Clear();
		}
	}
}