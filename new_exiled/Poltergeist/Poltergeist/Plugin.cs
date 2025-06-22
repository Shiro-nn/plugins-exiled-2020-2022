using EXILED;
using Harmony;
using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using EXILED.Extensions;
namespace Poltergeist
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;

		public static System.Random Gen = new System.Random();
		private bool enabled;
		public static MTFRespawn Respawn;
		public static float InitialDelay;
		public static bool TimerOn;
		public static float DurationMin;
		public static bool RandomEvents;
		public static float DurationMax;
		public static int DelayMax;
		public static int DelayMin;

		public override void OnEnable()
		{
			enabled = Config.GetBool("p_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"Poltergeist{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			_ = EventHandlers.Main();
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
		}

		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;

			EventHandlers = null;
		}

		public override void OnReload() { }
		public IEnumerator<float> RunBlackoutTimer()
		{
			if (Respawn == null)
				Respawn = PlayerManager.localPlayer.GetComponent<MTFRespawn>();
			yield return Timing.WaitForSeconds(InitialDelay);

			for (; ; )
			{
				Timing.CallDelayed(10f, () => Respawn.RpcPlayCustomAnnouncement("pitch_0.1  .g1 . .g4 . .g2 . .g5 . . .g6 .", false, false));
				Timing.CallDelayed(10.1f, () => Respawn.RpcPlayCustomAnnouncement("pitch_1.1 .g1", false, false));
				Map.Broadcast(Configs.psb, Configs.psbt);
				TimerOn = true;
				yield return Timing.WaitForSeconds(0.7f);
				float blackoutDur = DurationMax;
				if (RandomEvents)
					blackoutDur = (float)Gen.NextDouble() * (DurationMax - DurationMin) + DurationMin;
				foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
				Extensions.Postfix(item);
				EventHandlers.dr = true;
				EventHandlers.dd = true;
				Extensions.Checkopen();
				EventHandlers.Coroutines.Add(Timing.RunCoroutine(Keter(blackoutDur), "keter"));
				Generator079.generators[0].RpcCustomOverchargeForOurBeautifulModCreators(blackoutDur, false);
				Respawn.RpcPlayCustomAnnouncement("pitch_0.1 .g6", false, false);
				Respawn.RpcPlayCustomAnnouncement("pitch_1.1 .g1", false, false);
				yield return Timing.WaitForSeconds(blackoutDur - 8.7f);
				Respawn.RpcPlayCustomAnnouncement("pitch_0.1 .g1 .g5", false, false);
				Respawn.RpcPlayCustomAnnouncement("pitch_1.1 .g1", false, false);
				EventHandlers.dr = false;
				EventHandlers.dd = false;
				Extensions.canBeFloating.Clear();
				Extensions.haveBeenMoved.Clear();
				yield return Timing.WaitForSeconds(8.7f);
				Timing.KillCoroutines("keter");
				TimerOn = false;
				if (RandomEvents)
					yield return Timing.WaitForSeconds(Gen.Next(DelayMin, DelayMax));
				else
					yield return Timing.WaitForSeconds(InitialDelay);
			}
		}
		public IEnumerator<float> Keter(float dur)
		{
			do
			{
				foreach (ReferenceHub hub in GetHubs())
				{
					bool damaged = false;
					foreach (FlickerableLight light in UnityEngine.Object.FindObjectsOfType<FlickerableLight>())
						if (Vector3.Distance(light.transform.position, hub.gameObject.transform.position) < 10f && !damaged)
							if (hub.characterClassManager.IsHuman() &&
								hub.characterClassManager.CurClass != RoleType.Spectator && !hub.HasLightSource() && EventHandlers.dd)
							{
								damaged = true;
								hub.playerStats.HurtPlayer(new PlayerStats.HitInfo(5f, "Poltergeist", DamageTypes.Wall, 0), hub.gameObject);
								hub.Broadcast(Configs.ab, Configs.abt);
							}

					yield return Timing.WaitForSeconds(5f);
				}
			} while ((dur -= 5f) > 5f);
		}
		public override string getName { get; } = "Poltergeist";
	}
}
