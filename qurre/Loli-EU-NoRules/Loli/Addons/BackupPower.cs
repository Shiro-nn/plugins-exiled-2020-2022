using Loli.DataBase.Modules;
using Loli.Spawns.Roles;
using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Addons
{
	static class BackupPower
	{
		static internal bool InProgress = false;
		static internal bool SystemsBreak = false;
		static internal void StartBackup(float dur)
		{
			if (OmegaWarhead.InProgress) return;
			if (dur < 20) dur = 20;
			Timing.RunCoroutine(DoCor());
			IEnumerator<float> DoCor()
			{
				InProgress = true;
				SystemsBreak = true;
				LostSignal(16);
				GlobalLights.TurnOff(16f);
				yield return Timing.WaitForSeconds(1);
				GlobalLights.ChangeColor(Color.black);
				yield return Timing.WaitForSeconds(13);
				GlobalLights.ChangeColor(Color.red);
				yield return Timing.WaitForSeconds(1);
				SystemsBreak = false;
				var str = $"<color=rainbow><b>Внимание всему персоналу</b></color>\n" +
					$"<size=70%><color=#6f6f6f>Замечена хакерская атака на защитные системы комплекса</color></size>";
				var str2 = "<size=70%><color=#6f6f6f>,\nкомплекс переходит на резервное питание.</color></size>";
				ushort time = 30;
				if (dur < 45) time = (ushort)(dur / 2);
				var bc = Map.Broadcast(str.Replace("rainbow", "#ff0000"), time, true);
				Cassie.Send("ATTENTION TO ALL PERSONNEL . . A hacker attack on the security systems of the facility has been noticed . . the facility proceed to backup power");
				Timing.RunCoroutine(BcChange(bc, str, str2));
				yield return Timing.WaitForSeconds(dur - 17);
				GlobalLights.TurnOff(3f);
				yield return Timing.WaitForSeconds(2);
				InProgress = false;
				GlobalLights.SetToDefault();
				yield break;
			}
		}

		static internal void Ra(RemoteAdminCommandEvent ev)
		{
			if (!Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var _d)) return;
			if (!_d.administration.owner) return;

			ev.Allowed = false;
			ev.Reply = "Успешно";

			StartBackup(float.Parse(ev.Args[0]));
		}

		[EventMethod(PlayerEvents.InteractDoor)]
		static internal void HackActivated(InteractDoorEvent ev)
		{
			if (!SystemsBreak)
				return;

			if (ev.Player.Tag.Contains(Hacker.HackerTag))
				return;

			ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.InteractLift)]
		static internal void HackActivated(InteractLiftEvent ev)
		{
			if (!SystemsBreak)
				return;

			if (ev.Player.Tag.Contains(Hacker.HackerTag))
				return;

			ev.Allowed = false;
		}

		static void LostSignal(float dur)
		{
			foreach (var pl in Player.List)
			{
				try
				{
					if (pl.RoleInfomation.Role is PlayerRoles.RoleTypeId.Scp079)
						pl.RoleInfomation.Scp079.LostSignal(dur);
				}
				catch { }
			}
		}

		static IEnumerator<float> BcChange(MapBroadcast bc, string str, string str2)
		{
			bool red_color = false;
			for (int i = 0; i < 10; i++)
			{
				yield return Timing.WaitForSeconds(1f);
				var color = "#fdffbb";
				if (red_color)
				{
					color = "#ff0000";
					red_color = false;
				}
				else red_color = true;
				var msg = str.Replace("rainbow", color);
				if (i > 4) msg += str2;
				bc.Message = msg;
			}
		}
	}
}