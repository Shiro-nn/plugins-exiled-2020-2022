using Loli.DataBase.Modules.Controllers;
using Loli.Textures.Models.Rooms;
using MEC;
using Qurre.API;
using Qurre.API.Events;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		internal void Join(JoinEvent ev)
		{
			string str = "\n<color=#00fffb>Добро пожаловать на сервер</color> ";
			string project = "<color=#ff0000>f</color><color=#ff004c>y</color><color=#ff007b>d</color><color=#ff00a2>n</color><color=#e600ff>e</color>";
			string str2 = "<b><color=#09ff00>Приятной игры!</color></b>";
			ev.Player.ShowHint($"<b>{str}{project}</b>\n{str2}".Trim(), 10);
			Timing.CallDelayed(1f, () =>
			{
				if (!Round.Started && Round.ElapsedTime.Minutes == 0)
				{
					ev.Player.Role = RoleType.Tutorial;
					Timing.CallDelayed(0.5f, () =>
					{
						ev.Player.Hp = ev.Player.ClassManager.Classes.SafeGet(RoleType.Tutorial).maxHP;
						Timing.CallDelayed(0.3f, () =>
						{
							try
							{
								if (ev.Player.Role != RoleType.Tutorial) return;
								ev.Player.Position = Range.SpawnPoint.position;
								ev.Player.GetAmmo();
								ev.Player.AddItem(ItemType.GunCOM18);
								ev.Player.AddItem(ItemType.GunRevolver);
								ev.Player.AddItem(ItemType.ParticleDisruptor);
								ev.Player.AddItem(ItemType.ParticleDisruptor);
							}
							catch { }
						});
					});
				}
			});

			try { if (ev.Player.UserId == "-@steam") new Nimb(ev.Player); } catch { }
			try { if (ev.Player.UserId == "-@steam") new Nimb(ev.Player); } catch { }
			try { if (ev.Player.UserId == "-@steam") new Nimb(ev.Player); } catch { }
			try { if (ev.Player.UserId == "-@steam") new Nimb(ev.Player); } catch { }

			try { if (ev.Player.UserId == "-@steam") new Glow(ev.Player, new UnityEngine.Color32(255, 0, 0, 255)); } catch { }
			try { if (ev.Player.UserId == "-@steam") new Glow(ev.Player, new UnityEngine.Color32(0, 0, 255, 255)); } catch { }

			try
			{
				if (ev.Player.Id != Server.Host.Id)
					Timing.CallDelayed(5f, () => Plugin.Socket.Emit("server.join", new object[] { ev.Player.UserId, ev.Player.Ip }));
			}
			catch { }
			try { if (!Round.Ended) Plugin.Socket.Emit("server.addip", new object[] { Plugin.ServerID, ev.Player.Ip }); } catch { }
		}
		internal void AntiWaiting(PickupItemEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
		internal void AntiWaiting(InteractDoorEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
		internal void AntiWaiting(RagdollSpawnEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
		internal void AntiWaiting(NewBloodEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
		internal void AntiWaiting(CreatePickupEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
		internal void AntiWaiting(DroppingItemEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
		internal void AntiWaiting(ThrowItemEvent ev)
		{
			if (Round.Waiting) ev.Allowed = false;
		}
	}
}