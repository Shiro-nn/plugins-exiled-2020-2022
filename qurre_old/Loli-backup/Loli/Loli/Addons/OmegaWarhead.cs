using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using Qurre.API.Objects;
using System.Linq;
using Loli.DataBase;
using System.IO;
using System.Net;
using UnityEngine;
using HarmonyLib;
using System.Threading;
namespace Loli.Addons
{
	[HarmonyPatch]
	internal static class OmegaWarhead_AudioFix1
	{
		internal static System.Reflection.MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("Qurre.API.Addons.Audio.Extensions.Microphone");
			return AccessTools.Method(type, "StopCapture");
		}
		internal static bool Prefix() => OmegaWarhead.AllowStopAudio;
	}
	public class OmegaWarhead
	{
		public OmegaWarhead()
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
		private static string AudioPath => Path.Combine(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio"), "OmegaWarhead.raw");
		public static void Start()
		{
			if (Plugin.ClansWars) return;
			if (InProgress) return;
			Modules.EventHandlers.AutoAlphaInProgress = false;
			try { Alpha.Stop(); } catch { }
			InProgress = true;
			GlobalLights.ChangeColor(Color.blue, true, true, true);
			DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.WarheadStart);
			AllowStopAudio = false;
			RoundThis = Round.CurrentRound;
			BlockAlpha = true;
			Audio.PlayFromStream(File.Open(AudioPath, FileMode.Open, FileAccess.Read, FileShare.Read), 100, true, playerName: "Омега-Боеголовка");
			Timing.RunCoroutine(CallDelayed(Round.CurrentRound), "OmegaWarheadDelayed");
		}
		private static IEnumerator<float> CallDelayed(int round)
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
		internal void Waiting()
		{
			Refresh();
		}
		internal void Refresh()
		{
			BlockAlpha = false;
			InProgress = false;
			Detonated = false;
			ElevatorLocked = false;
			AllowStopAudio = true;
		}
		internal void AntiAlpha(AlphaStartEvent ev)
		{
			if (BlockAlpha) ev.Allowed = false;
		}
		internal void NotDisable(AlphaStopEvent ev)
		{
			if (InProgress) ev.Allowed = false;
		}
		internal void AllKill()
		{
			if (InProgress)
			{
				foreach (Player pl in Player.List.Where(x => !x.Overwatch && x.Role != RoleType.None && x.Role != RoleType.Spectator))
					pl.Kill("Взрыв Омега-Боеголовки");
				Timing.CallDelayed(1f, () => Round.End());
			}
		}
		internal void LiftUse(UseLiftEvent ev)
		{
			if (InProgress && ElevatorLocked && RoundThis == Round.CurrentRound && (ev.Lift.Type == LiftType.GateB || ev.Lift.Type == LiftType.GateA))
				ev.Allowed = false;
			else if (InProgress)
			{
				if (!ev.Lift.Operative || ev.Lift.Locked) ev.Allowed = false;
				else ev.Allowed = true;
			}
		}
		private void RaActivate(SendingRAEvent ev)
		{
			if (ev.CommandSender.SenderId == "SERVER CONSOLE") Activate();
			if (Manager.Static.Data.Users.TryGetValue(ev.CommandSender.SenderId, out var data) && data.id == 1) Activate();
			void Activate()
			{
				ev.Allowed = false;
				ev.ReplyMessage = "Успешно";
				Map.Broadcast("<size=25%><color=#6f6f6f>Совет О5 согласился на <color=red>взрыв</color> <color=#0089c7>ОМЕГА Боеголовки</color></color></size>", 10, true);
				Start();
			}
		}
		private void RaMusic(SendingRAEvent ev)
		{
			if (!Manager.Static.Data.Users.TryGetValue(ev.CommandSender.SenderId, out var data)) return;
			if (data.id != 1) return;
			ev.Allowed = false;
			Audio.PlayFromStream(File.Open(AudioPath, FileMode.Open, FileAccess.Read, FileShare.Read), 100);
			ev.ReplyMessage = "Успешно";
		}
		private static void DownloadAudio(string url)
		{
			var fl = AudioPath;
			var dir = Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio");
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			if (File.Exists(fl)) return;
			new Thread(() =>
			{
				WebRequest request = WebRequest.Create(url);
				WebResponse response = request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				Stream fileStream = File.OpenWrite(Path.Combine(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio"), "OmegaWarhead.raw"));
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