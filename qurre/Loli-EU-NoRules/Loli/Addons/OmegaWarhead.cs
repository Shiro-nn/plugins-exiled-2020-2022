using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using System.Linq;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;
using Qurre.Events.Structs;
using Loli.DataBase.Modules;
using PlayerRoles;
using Qurre.API.Attributes;
using Qurre.Events;

namespace Loli.Addons
{
	static class OmegaWarhead
	{
		static OmegaWarhead()
		{
			DownloadAudio("https://cdn.scpsl.store/qurre/audio/OmegaWarhead.raw");
			CommandsSystem.RegisterRemoteAdmin("ow", RaActivate);
			CommandsSystem.RegisterRemoteAdmin("omega_warhead", RaActivate);
			CommandsSystem.RegisterRemoteAdmin("audio", RaMusic);
		}

		public static bool InProgress = false;
		public static bool Detonated = false;
		public static bool ElevatorLocked = false;
		public static bool AllowStopAudio = true;
		public static bool BlockAlpha = false;
		public static int RoundThis = 0;
		static string AudioPath => Path.Combine(Path.Combine(Pathes.Plugins, "Audio"), "OmegaWarhead.raw");
		public static void Start()
		{
			if (InProgress) return;
			AutoAlpha.InProgress = false;
			try { Alpha.Stop(); } catch { }
			InProgress = true;
			GlobalLights.ChangeColor(Color.blue, true, true, true);
			DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.WarheadStart);
			AllowStopAudio = false;
			RoundThis = Round.CurrentRound;
			BlockAlpha = true;
			Timing.RunCoroutine(CallDelayed(Round.CurrentRound), "OmegaWarheadDelayed");
		}
		static IEnumerator<float> CallDelayed(int round)
		{
			yield return Timing.WaitForSeconds(170f);
			if (Round.CurrentRound == round) ElevatorLocked = true;
			yield return Timing.WaitForSeconds(33f);
			if (Round.CurrentRound == round)
			{
				Alpha.Detonate();
				Detonated = true;
			}
			yield return Timing.WaitForSeconds(22f);
			if (Round.CurrentRound == round) AllowStopAudio = true;
		}

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh()
		{
			BlockAlpha = false;
			InProgress = false;
			Detonated = false;
			ElevatorLocked = false;
			AllowStopAudio = true;
		}

		[EventMethod(AlphaEvents.Start)]
		static void AntiAlpha(AlphaStartEvent ev)
		{
			if (BlockAlpha) ev.Allowed = false;
		}

		[EventMethod(AlphaEvents.Stop)]
		static void NotDisable(AlphaStopEvent ev)
		{
			if (InProgress) ev.Allowed = false;
		}

		[EventMethod(AlphaEvents.Detonate)]
		static void AllKill()
		{
			if (InProgress)
			{
				foreach (Player pl in Player.List.Where(x => !x.GamePlay.Overwatch &&
				x.RoleInfomation.Role != RoleTypeId.None &&
				x.RoleInfomation.Role != RoleTypeId.Spectator))
					pl.HealthInfomation.Kill("Взрыв Омега-Боеголовки");
				Timing.CallDelayed(1f, () => Round.End());
			}
		}

		static void RaActivate(RemoteAdminCommandEvent ev)
		{
			if (ev.Sender.SenderId == "SERVER CONSOLE") Activate();
			if (Data.Users.TryGetValue(ev.Sender.SenderId, out var data) && data.administration.owner) Activate();
			void Activate()
			{
				ev.Allowed = false;
				ev.Reply = "Успешно";
				Map.Broadcast("<size=65%><color=#6f6f6f>Совет О5 согласился на <color=red>взрыв</color> <color=#0089c7>ОМЕГА Боеголовки</color></color></size>", 10, true);
				Start();
			}
		}
		static void RaMusic(RemoteAdminCommandEvent ev)
		{
			if (!Data.Users.TryGetValue(ev.Sender.SenderId, out var data)) return;
			if (!data.administration.owner) return;
			ev.Allowed = false;
			ev.Reply = "Успешно";
		}

		static void DownloadAudio(string url)
		{
			var fl = AudioPath;
			var dir = Path.Combine(Pathes.Plugins, "Audio");
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			if (File.Exists(fl)) return;
			new Thread(() =>
			{
				WebRequest request = WebRequest.Create(url);
				WebResponse response = request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				Stream fileStream = File.OpenWrite(Path.Combine(Path.Combine(Pathes.Plugins, "Audio"), "OmegaWarhead.raw"));
				byte[] buffer = new byte[4096];
				int bytesRead = responseStream.Read(buffer, 0, 4096);
				while (bytesRead > 0)
				{
					fileStream.Write(buffer, 0, bytesRead);
					bytesRead = responseStream.Read(buffer, 0, 4096);
				}
			}).Start();
		}
	}
}