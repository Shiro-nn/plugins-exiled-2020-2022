using Loli.DataBase;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.Addons
{
	public class BackupPower
	{
		static public bool InProgress = false;
		static public bool SystemsBreak = false;
		static public void StartBackup(float dur)
		{
			if (OmegaWarhead.InProgress) return;
			if (dur < 20) dur = 20;
			Timing.RunCoroutine(DoCor());
			IEnumerator<float> DoCor()
			{
				InProgress = true;
				SystemsBreak = true;
				GlobalLights.TurnOff(16f);
				yield return Timing.WaitForSeconds(1);
				GlobalLights.ChangeColor(Color.black);
				yield return Timing.WaitForSeconds(13);
				GlobalLights.ChangeColor(Color.red);
				yield return Timing.WaitForSeconds(1);
				SystemsBreak = false;
				var str = $"<color=rainbow><b>Внимание всему персоналу</b></color>\n" +
					$"<size=30%><color=#6f6f6f>Замечена хакерская атака на защитные системы комплекса</color></size>";
				var str2 = "<size=30%><color=#6f6f6f>,\nкомплекс переходит на резервное питание.</color></size>";
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
		static internal void Ra(SendingRAEvent ev)
		{
			if (!Manager.Static.Data.Users.TryGetValue(ev.Player.UserId, out var _d)) return;
			if (_d.id != 1) return;
			ev.Allowed = false;
			ev.ReplyMessage = "Успешно.";
			StartBackup(float.Parse(ev.Args[0]));
		}
		static internal void HackActivated(InteractDoorEvent ev)
		{
			if (!SystemsBreak) return;
			if (ev.Player.Tag.Contains(Spawns.Roles.Hacker.HackerTag)) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(InteractLiftEvent ev)
		{
			if (!SystemsBreak) return;
			if (ev.Player.Tag.Contains(Spawns.Roles.Hacker.HackerTag)) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(ChangeCameraEvent ev)
		{
			if (!SystemsBreak) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(Scp079InteractDoorEvent ev)
		{
			if (!SystemsBreak) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(Scp079InteractLiftEvent ev)
		{
			if (!SystemsBreak) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(Scp079InteractTeslaEvent ev)
		{
			if (!SystemsBreak) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(Scp079SpeakerEvent ev)
		{
			if (!SystemsBreak) return;
			ev.Allowed = false;
		}
		static internal void HackActivated(Scp079ElevatorTeleportEvent ev)
		{
			if (!SystemsBreak) return;
			ev.Allowed = false;
		}
		static private IEnumerator<float> BcChange(MapBroadcast bc, string str, string str2)
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