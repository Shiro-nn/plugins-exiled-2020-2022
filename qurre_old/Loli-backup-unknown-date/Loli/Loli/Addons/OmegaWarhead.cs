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
using Qurre.API.Addons;
namespace Loli.Addons
{
	/*[HarmonyPatch(typeof(MicrophoneModule), nameof(MicrophoneModule.StopCapture))]
	internal static class OmegaWarhead_AudioFix1
	{
		private static bool Prefix() => OmegaWarhead.AllowStopAudio;
	}
	[HarmonyPatch(typeof(MicrophoneModule), nameof(MicrophoneModule.UpdateSubscribers))]
	internal static class OmegaWarhead_AudioFix2
	{
		private static bool Prefix(ref bool __result)
		{
			if (OmegaWarhead.InProgress) return true;
			__result = false;
			return false;
		}
	}
	[HarmonyPatch(typeof(Audio), nameof(Audio.StopCapture))]
	internal static class OmegaWarhead_AudioFix3
	{
		private static bool Prefix() => OmegaWarhead.AllowStopAudio;
	}
	[HarmonyPatch(typeof(Audio), nameof(Audio.UpdateSubscribers))]
	internal static class OmegaWarhead_AudioFix4
	{
		private static bool Prefix(ref bool __result)
        {
			if (OmegaWarhead.InProgress) return true;
			__result = false;
			return false;
		}
	}*/
	public class OmegaWarhead
	{
		public OmegaWarhead() => DownloadAudio("https://cdn.scpsl.store/qurre/audio/OmegaWarhead.raw");
		public static bool InProgress = false;
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
			foreach (Room room in Map.Rooms) room.LightColor = Color.blue;
			DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.WarheadStart);
			AllowStopAudio = false;
			RoundThis = Round.CurrentRound;
			BlockAlpha = true;
			Timing.RunCoroutine(CallDelayed(Round.CurrentRound), "OmegaWarheadDelayed");
		}
		private static IEnumerator<float> CallDelayed(int round)
		{
			yield return Timing.WaitForSeconds(170f);
			if (Round.CurrentRound == round) ElevatorLocked = true;
			yield return Timing.WaitForSeconds(33f);
			if (Round.CurrentRound == round) Alpha.Detonate();
			yield return Timing.WaitForSeconds(22f);
			if (Round.CurrentRound == round) AllowStopAudio = true;
		}
		internal void Waiting()
		{
			//new Audio(new FileStream(AudioPath, FileMode.Open), 100);
			//var _audio = AudioOutdate.DissonanceComms.gameObject.AddComponent<Audio>().Init(new FileStream(AudioPath, FileMode.Open), 50);
			//_audio.RestartCapture(_audio.Name, true);
			Refresh();
		}
		internal void Refresh()
		{
			BlockAlpha = false;
			InProgress = false;
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
		internal void Ra(SendingRAEvent ev)
		{
			try
			{
				if (ev.Player == Server.Host || ev.Player.Id == Server.Host.Id) return;
				if (!Manager.Static.Data.Users.TryGetValue(ev.CommandSender.SenderId, out var data)) return;
				if ((ev.Name == "ow" || ev.Name == "omega_warhead") && data.or)
				{
					ev.Allowed = false;
					ev.ReplyMessage = "Успешно.";
					Map.Broadcast("<size=25%><color=#6f6f6f>Совет О5 согласился на <color=red>взрыв</color> <color=#0089c7>ОМЕГА Боеголовки</color></color></size>", 10, true);
					Start();
				}
				if (ev.Name == "audio" && data.or)
				{
					ev.Allowed = false;
					new Audio(new FileStream(AudioPath, FileMode.Open), 100);
					ev.ReplyMessage = "Успешно.";
					//new Audio(new FileStream(AudioPath, FileMode.Open), 0.5f);
					//Bot bot = Bot.Create(Map.GetRandomSpawnPoint(RoleType.Tutorial), Vector2.zero, RoleType.Tutorial);
					//var audio = bot.GameObject.AddComponent<Audio>().Init(new FileStream(AudioPath, FileMode.Open), 50);
					//audio.RestartCapture(audio.Name, true);
				}
			}
			catch { }
		}
		private static void DownloadAudio(string url)
		{
			var fl = AudioPath;
			var dir = Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio");
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			if (File.Exists(fl)) return;
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
		}
	}
}