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
			if (Plugin.RolePlay)
			{
				str2 = $"<b><color=#ff0000>Не забудьте прочитать правила!</color></b>\n{str2}";
				project = "<color=#ff0000>f</color><color=#ff004c>y</color><color=#ff007b>d</color><color=#ff00a2>n</color><color=#e600ff>e</color>";
				if (Plugin.ServerID == 8) project += " <color=#c300ff>M</color><color=#b300ff>e</color><color=#9900ff>d</color>" +
						"<color=#8400ff>i</color><color=#6200ff>u</color><color=#4000ff>m</color> <color=#1900ff>R</color><color=#0033ff>P</color>";
			}
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
								ev.Player.AddItem(ItemType.GunCOM18);
								ev.Player.Ammo9 = 999;
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
			try
			{
				if (ev.Player.UserId == "-@steam")
				{
					new Nimb(ev.Player);
					new Glow(ev.Player, new UnityEngine.Color32(250, 0, 242, 255));
					new Textures.Models.Mercury(ev.Player);
				}
			}
			catch { }

			try
			{
				if (ev.Player.Id != Server.Host.Id)
				{
					string text = $"join=;={ev.Player.UserId}=;={ev.Player.Ip}=;=";
					Timing.CallDelayed(5f, () => Addons.NetSocket.Send(text));
				}
			}
			catch { }
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