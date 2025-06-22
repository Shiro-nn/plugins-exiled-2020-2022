using Loli.DataBase;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.Addons
{
	public class BackupPower
	{
		public static bool InProgress = false;
		public static void StartBackup(float dur)
		{
			if (Plugin.ClansWars) return;
			if (dur < 5) return;
			if (OmegaWarhead.InProgress) return;
			InProgress = true;
			var str = $"<color=rainbow><b>Внимание всему персоналу</b></color>\n" +
				$"<size=30%><color=#6f6f6f>Замечена хакерская атака на защитные системы комплекса</color></size>";
			var str2 = "<size=30%><color=#6f6f6f>,\nкомплекс переходит на резервное питание.</color></size>";
			ushort time = 30;
			if (dur < 45) time = (ushort)(dur / 2);
			var bc = Map.Broadcast(str.Replace("rainbow", "#ff0000"), time, true);
			Cassie.Send("ATTENTION TO ALL PERSONNEL . . A hacker attack on the security systems of the complex has been noticed . . the complex proceed to backup power");
			Lights.TurnOff(5f);
			Timing.RunCoroutine(BcChange(bc, str, str2));
			Timing.CallDelayed(4, () =>
			{
				foreach (Room room in Map.Rooms) room.LightColor = Color.red;
				Timing.CallDelayed(dur - 4, () =>
				  {
					  InProgress = false;
					  foreach (Room room in Map.Rooms) room.LightOverride = false;
                      if (Plugin.RolePlay)
					  {
						  Qurre.API.Extensions.GetRoom(RoomType.LczClassDSpawn).LightColor = Color.red;
						  Qurre.API.Extensions.GetRoom(RoomType.Lcz173).LightColor = Color.red;
						  Qurre.API.Extensions.GetRoom(RoomType.Hcz106).LightColor = Color.red;
					  }
				  });
			});
			Timing.CallDelayed(dur - 2f, () => Lights.TurnOff(3f));
		}
		internal static void Ra(SendingRAEvent ev)
		{
			try
			{
				if (ev.Player == Server.Host || ev.Player.Id == Server.Host.Id) return;
				if ((ev.Name == "bp" || ev.Name == "backup_power") && Manager.Static.Data.Users[ev.Player.UserId].or)
				{
					ev.Allowed = false;
					ev.ReplyMessage = "Успешно.";
					StartBackup(float.Parse(ev.Args[0]));
				}
			}
			catch { }
		}
		private static IEnumerator<float> BcChange(MapBroadcast bc, string str, string str2)
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