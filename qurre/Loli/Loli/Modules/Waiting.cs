using Loli.Textures.Models.Rooms;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Modules
{
	static class Waiting
	{
		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
			string str = "\n<color=#00fffb>Добро пожаловать на сервер</color> ";
			string project = "<color=#ff0000>f</color><color=#ff004c>y</color><color=#ff007b>d</color><color=#ff00a2>n</color><color=#e600ff>e</color>";
			string str2 = "<b><color=#09ff00>Приятной игры!</color></b>";
			ev.Player.Client.ShowHint($"<b>{str}{project}</b>\n{str2}".Trim(), 10);
			Timing.CallDelayed(1f, () =>
			{
				if (!Round.Started && Round.ElapsedTime.Minutes == 0)
				{
					ev.Player.RoleInfomation.SetNew(RoleTypeId.Tutorial, RoleChangeReason.Respawn);
					Timing.CallDelayed(0.5f, () =>
					{
						ev.Player.HealthInfomation.Hp = 100;
						Timing.CallDelayed(0.3f, () =>
						{
							try
							{
								if (ev.Player.RoleInfomation.Role != RoleTypeId.Tutorial)
									return;

								ev.Player.MovementState.Position = Range.SpawnPoint.position;
								ev.Player.GetAmmo();
								ev.Player.Inventory.AddItem(ItemType.GunCOM18);
								ev.Player.Inventory.AddItem(ItemType.GunRevolver);
								ev.Player.Inventory.AddItem(ItemType.ParticleDisruptor);
								ev.Player.Inventory.AddItem(ItemType.ParticleDisruptor);
								ev.Player.Inventory.AddItem(ItemType.Jailbird);
							}
							catch { }
						});
					});
				}
			});

			try
			{
				if (ev.Player.UserInfomation.Id != Server.Host.UserInfomation.Id)
					Timing.CallDelayed(5f, () => Core.Socket.Emit("server.join", new object[] { ev.Player.UserInfomation.UserId, ev.Player.UserInfomation.Ip }));
			}
			catch { }
			try { if (!Round.Ended) Core.Socket.Emit("server.addip", new object[] { Core.ServerID, ev.Player.UserInfomation.Ip }); } catch { }
		}

		[EventMethod(PlayerEvents.PickupItem)]
		static void AntiWaiting(PickupItemEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.InteractDoor)]
		static void AntiWaiting(InteractDoorEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}

		[EventMethod(MapEvents.RagdollSpawn)]
		static void AntiWaiting(RagdollSpawnEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}

		[EventMethod(MapEvents.PlaceBlood)]
		static void AntiWaiting(PlaceBloodEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}

		[EventMethod()]
		internal void AntiWaiting(CreatePickupEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.DropItem)]
		static void AntiWaiting(DropItemEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.ThrowProjectile)]
		static void AntiWaiting(ThrowProjectileEvent ev)
		{
			if (Round.Waiting)
				ev.Allowed = false;
		}
	}
}