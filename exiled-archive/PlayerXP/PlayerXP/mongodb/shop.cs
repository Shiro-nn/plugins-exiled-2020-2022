using Exiled.API.Features;
using Exiled.Events.EventArgs;
using PlayerXP.scp228.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerXP
{
    public class shop
	{
		private readonly Plugin plugin;
		public shop(Plugin plugin) => this.plugin = plugin;

		public static Pickup shop1 = new Pickup();
		public static Pickup shop2 = new Pickup();
		public static Pickup shop3 = new Pickup();
		public static Pickup shop4 = new Pickup();
		public static Pickup shop5 = new Pickup();
		public static Pickup shop6 = new Pickup();
		public static Pickup shop7 = new Pickup();
		public static Pickup shop8 = new Pickup();
		public static Pickup shop9 = new Pickup();
		public static Pickup shop10 = new Pickup();
		public static Pickup shop11 = new Pickup();
		public static Pickup shop12 = new Pickup();
		public static Pickup shop13 = new Pickup();
		public static Pickup shop14 = new Pickup();
		public static Pickup shop15 = new Pickup();
		public static Pickup shop16 = new Pickup();
		public static Pickup shop17 = new Pickup();
		public static Pickup shop18 = new Pickup();
		public static Pickup shop19 = new Pickup();
		public static Pickup shop20 = new Pickup();
		public static Pickup shop21 = new Pickup();
		public static Pickup shop22 = new Pickup();
		public static Pickup shop23 = new Pickup();
		public static Pickup shop24 = new Pickup();
		public static Pickup shop25 = new Pickup();
		public void OnRoundStart()
		{
			shopspawn();
		}
		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			clearshop();
		}
		internal void pickup(PickingUpItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId || ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
            {
				if (ev.Pickup == shop25)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop25 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop24)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop24 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop23)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop23 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop22)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop22 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop21)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop21 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop20)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop20 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop19)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop19 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop18)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop18 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop17)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop17 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop16)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop16 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop15)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop15 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop14)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop14 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop13)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop13 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop12)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop12 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop11)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop11 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop10)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop10 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop9)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop9 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop8)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop8 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop7)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop7 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop6)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop6 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop5)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop5 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop4)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop4 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop3)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop3 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop2)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop2 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
				else if (ev.Pickup == shop1)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					shop1 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
			}
			if (ev.Pickup.ItemId == ItemType.Coin)
			{
				ev.IsAllowed = false;
				plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] += 1;
				ev.Pickup.Delete();
				ev.Player.ClearBroadcasts();
				ev.Player.ReferenceHub.Broadcast("<color=aqua>+монетка</color>", 3);
			}
			else if (ev.Pickup == shop25)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 3)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 3 монетки", 5);
					ev.Player.AddItem(ItemType.WeaponManagerTablet);
					ev.Pickup.Delete();
					shop25 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/3)", 5);
				}
			}
			else if (ev.Pickup == shop24)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 6)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 6;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 6 монет", 5);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					ev.Pickup.Delete();
					shop24 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/6)", 5);
				}
			}
			else if (ev.Pickup == shop23)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 4)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 4;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 4 монетки", 5);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					ev.Pickup.Delete();
					shop23 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/5)", 5);
				}
			}
			else if (ev.Pickup == shop22)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 3)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 3 монетки", 5);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Pickup.Delete();
					shop22 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/3)", 5);
				}
			}
			else if (ev.Pickup == shop21)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 13)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 13;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 13 монет", 5);
					ev.Player.AddItem(ItemType.SCP268);
					ev.Pickup.Delete();
					shop21 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/13)", 5);
				}
			}
			else if (ev.Pickup == shop20)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 8)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 8;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 8 монет", 5);
					ev.Player.AddItem(ItemType.SCP018);
					ev.Pickup.Delete();
					shop20 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/8)", 5);
				}
			}
			else if (ev.Pickup == shop19)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 5)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 5;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 5 монет", 5);
					ev.Player.AddItem(ItemType.SCP207);
					ev.Pickup.Delete();
					shop19 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/5)", 5);
				}
			}
			else if (ev.Pickup == shop18)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 6)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 6;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 6 монет", 5);
					ev.Player.AddItem(ItemType.SCP500);
					ev.Pickup.Delete();
					shop18 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/6)", 5);
				}
			}
			else if (ev.Pickup == shop17)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 20)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 20;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 20 монет", 5);
					ev.Player.AddItem(ItemType.MicroHID);
					ev.Pickup.Delete();
					shop17 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/20)", 5);
				}
			}
			else if (ev.Pickup == shop16)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 2)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 2;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 2 монетки", 5);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Pickup.Delete();
					shop16 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/2)", 5);
				}
			}
			else if (ev.Pickup == shop15)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 2)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 2;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 2 монетки", 5);
					ev.Player.AddItem(ItemType.Painkillers);
					ev.Pickup.Delete();
					shop15 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/2)", 5);
				}
			}
			else if (ev.Pickup == shop14)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 3)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 3 монетки", 5);
					ev.Player.AddItem(ItemType.Adrenaline);
					ev.Pickup.Delete();
					shop14 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/3)", 5);
				}
			}
			else if (ev.Pickup == shop13)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 3)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 3 монетки", 5);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Pickup.Delete();
					shop13 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/3)", 5);
				}
			}
			else if (ev.Pickup == shop12)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 9)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 9;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 9 монет", 5);
					ev.Player.AddItem(ItemType.GunUSP);
					ev.Pickup.Delete();
					shop12 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/9)", 5);
				}
			}
			else if (ev.Pickup == shop11)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 17)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 17;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 17 монет", 5);
					ev.Player.AddItem(ItemType.GunLogicer);
					ev.Pickup.Delete();
					shop11 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/17)", 5);
				}
			}
			else if (ev.Pickup == shop10)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 11)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 11;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 11 монет", 5);
					ev.Player.AddItem(ItemType.GunMP7);
					ev.Pickup.Delete();
					shop10 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/11)", 5);
				}
			}
			else if (ev.Pickup == shop9)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 15)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 15;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 15 монет", 5);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Pickup.Delete();
					shop9 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/15)", 5);
				}
			}
			else if (ev.Pickup == shop8)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 13)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 13;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 13 монет", 5);
					ev.Player.AddItem(ItemType.GunProject90);
					ev.Pickup.Delete();
					shop8 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/13)", 5);
				}
			}
			else if (ev.Pickup == shop7)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 7)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 7;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 7 монет", 5);
					ev.Player.AddItem(ItemType.GunCOM15);
					ev.Pickup.Delete();
					shop7 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/7)", 5);
				}
			}
			else if (ev.Pickup == shop6)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 3)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 3 монетки", 5);
					ev.Player.AddItem(ItemType.Radio);
					ev.Pickup.Delete();
					shop6 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/3)", 5);
				}
			}
			else if (ev.Pickup == shop5)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 20)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 20;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 20 монет", 5);
					ev.Player.AddItem(ItemType.KeycardO5);
					ev.Pickup.Delete();
					shop5 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/20)", 5);
				}
			}
			else if (ev.Pickup == shop4)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 15)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 15;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 15 монет", 5);
					ev.Player.AddItem(ItemType.KeycardFacilityManager);
					ev.Pickup.Delete();
					shop4 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/15)", 5);
				}
			}
			else if (ev.Pickup == shop3)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 10)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 10;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 10 монет", 5);
					ev.Player.AddItem(ItemType.KeycardNTFCommander);
					ev.Pickup.Delete();
					shop3 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/10)", 5);
				}
			}
			else if (ev.Pickup == shop2)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 7)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 7;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 7 монет", 5);
					ev.Player.AddItem(ItemType.KeycardNTFLieutenant);
					ev.Pickup.Delete();
					shop2 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/7)", 5);
				}
			}
			else if (ev.Pickup == shop1)
			{
				ev.IsAllowed = false;
				if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 10)
				{
					plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 10;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("Вы успешно купили этот товар за 10 монет", 5);
					ev.Player.AddItem(ItemType.KeycardContainmentEngineer);
					ev.Pickup.Delete();
					shop1 = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"Не хватает монет({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/10)", 5);
				}
			}
		}
		public static void shopspawn()
		{
			if (Random.Range(1, 100) < 25)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp049);
				Pickup q = Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Extensions.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Extensions.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Extensions.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Extensions.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Extensions.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Extensions.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Extensions.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Extensions.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Extensions.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Extensions.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Extensions.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Extensions.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Extensions.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Extensions.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Extensions.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Extensions.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Extensions.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Extensions.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Extensions.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Extensions.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Extensions.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Extensions.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Extensions.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast(10, "<color=aqua>Магазин появился на спавне</color> <color=red>SCP 049</color>");
				return;
			}
			if (Random.Range(1, 100) < 25)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp173);
				Pickup q = Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Extensions.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Extensions.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Extensions.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Extensions.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Extensions.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Extensions.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Extensions.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Extensions.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Extensions.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Extensions.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Extensions.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Extensions.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Extensions.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Extensions.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Extensions.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Extensions.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Extensions.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Extensions.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Extensions.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Extensions.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Extensions.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Extensions.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Extensions.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast(10, "<color=aqua>Магазин появился на спавне</color> <color=red>SCP 173</color>");
				return;
			}
			if (Random.Range(1, 100) < 25)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp106);
				Pickup q = Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Extensions.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Extensions.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Extensions.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Extensions.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Extensions.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Extensions.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Extensions.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Extensions.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Extensions.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Extensions.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Extensions.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Extensions.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Extensions.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Extensions.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Extensions.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Extensions.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Extensions.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Extensions.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Extensions.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Extensions.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Extensions.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Extensions.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Extensions.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast(10, "<color=aqua>Магазин появился на спавне</color> <color=red>SCP 106</color>");
				foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
					if (door.DoorName.Contains("106_BOTTOM"))
					{
						door.Networklocked = true;
						door.NetworkisOpen = true;
					}
				return;
			}
			if (true)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
				Pickup q = Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Extensions.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Extensions.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Extensions.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Extensions.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Extensions.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Extensions.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Extensions.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Extensions.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Extensions.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Extensions.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Extensions.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Extensions.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Extensions.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Extensions.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Extensions.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Extensions.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Extensions.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Extensions.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Extensions.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Extensions.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Extensions.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Extensions.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Extensions.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast(10, "<color=aqua>Магазин появился на спавне</color> <color=red>SCP 939</color>");
				return;
			}
		}
		internal void clearshop()
		{
			shop1 = null;
			shop2 = null;
			shop3 = null;
			shop4 = null;
			shop5 = null;
			shop6 = null;
			shop7 = null;
			shop8 = null;
			shop9 = null;
			shop10 = null;
			shop11 = null;
			shop12 = null;
			shop13 = null;
			shop14 = null;
			shop15 = null;
			shop16 = null;
			shop17 = null;
			shop18 = null;
			shop19 = null;
			shop20 = null;
			shop21 = null;
			shop22 = null;
			shop23 = null;
			shop24 = null;
			shop25 = null;
		}
	}
}
