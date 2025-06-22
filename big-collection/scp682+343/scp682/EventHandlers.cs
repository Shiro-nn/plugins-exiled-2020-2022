using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using Mirror;
using Log = EXILED.Log;
namespace scp682343
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static List<ReferenceHub> sList = new List<ReferenceHub>();
		internal static ReferenceHub scp682;
		internal static ReferenceHub scp343;
		private static bool isHidden;
		private static bool hasTag;
		private static int maxHP;
		private const float dur = 327;
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private static int AlternativeEnds;
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		bool IsBreakDoorsForUserActive;
		bool IsBreakAllForUserActive;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };
		private int max_tries = 5;
		private int max_door_tries = 5;
		private int tries = 0;
		private int shootgod = 0;
		private int door_tries = 0;
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private bool isRoundStarted;
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
		public void OnRoundStart()
		{
			scp682 = null;
			scp343 = null;
			Timing.CallDelayed(3f, () => {
				selectspawnSSS();
				selectspawnJG();
				selectspawnJ();
			});
			players.Clear();
			isRoundStarted = true;
			AlternativeEnds = 0;
			RoundEnds = 100;
			ffPlayers.Clear();
			scpPlayer = null;
			tries = 0;
			shootgod = 0;
			door_tries = 0;
			IsBreakDoorsForUserActive = false;
			IsBreakAllForUserActive = false;
		}

		public void OnRoundEnd()
		{
			scp682 = null;
			scp343 = null;
			isRoundStarted = false;
			tries = 0;
			door_tries = 0;
			Timing.KillCoroutines(coroutines);
			coroutines.Clear(); 
			players.Clear();
			AlternativeEnds = 0;
			ffPlayers.Clear();
			tries = 0;
			door_tries = 0;
			IsBreakDoorsForUserActive = false;
			IsBreakAllForUserActive = false;
		}

		public void OnRoundRestart()
		{
			
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp682?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				Killscp682();
				if(ev.Killer == pList.Contains(Team.CHI))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE CHAOSINSURGENCY", false, false);
					Killscp682();
				}
				else if (ev.Killer == pList.Contains(Team.MTF))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE MTFUNIT", false, false);
					Killscp682();
				}
				else if (ev.Killer == pList.Contains(Team.CDP))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE CLASSD PERSONNEL", false, false);
					Killscp682();
				}
				else if (ev.Killer == pList.Contains(Team.RSC))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE SCIENTIST PERSONNEL", false, false);
					Killscp682();
				}
				else if (ev.Killer == !pList.Contains(Team.RSC) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.CHI))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE UNKNOWN", false, false);
					Killscp682();
				}
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
			}
		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> p2List = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp343?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp682?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && ((pList.Contains(Team.SCP) && scp682 != null) || !pList.Contains(Team.SCP) && scp682 != null)) ||
				(Configs.winWithTutorial && !pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && pList.Contains(Team.TUT) && scp682 != null))
			{
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			else if (scp682 != null && !pList.Contains(Team.SCP) && (pList.Contains(Team.CDP) || pList.Contains(Team.CHI) || pList.Contains(Team.MTF) || pList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				ev.Amount = 100f;
			}
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
				{
					GrantFF(ev.Attacker);
				}

				if (scp682 != null)
				{
					if (ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
					{
						ev.Amount = Configs.dmg;
					}
					if (!Configs.scpFriendlyFire &&
						((ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Player) == Team.SCP) ||
						(ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Attacker) == Team.SCP)))
					{
						ev.Amount = 0f;
					}

					if (!Configs.tutorialFriendlyFire &&
						ev.Attacker.queryProcessor.PlayerId != ev.Player.queryProcessor.PlayerId &&
						((ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Player) == Team.TUT) ||
						(ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Attacker) == Team.TUT)))
					{
						ev.Amount = 0f;
					}
				}
			}
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Amount = 0f;
			}
			if (Player.GetRole(ev.Attacker) == RoleType.Scp106)
			{
				if (Player.GetTeam(ev.Player) == Team.CDP)
				{
					if (UnityEngine.Random.Range(0, max_tries) > tries)
					{
						ev.Amount = 0f;
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(Configs.safeuserbc, 10);
						ev.Attacker.ClearBroadcasts();
						ev.Attacker.Broadcast(Configs.safeattackerbc, 10);
						TeleportTo106(ev.Player);
					}
					tries++;
				}
			}
			if (Player.GetTeam(ev.Attacker) == Team.SCP)
			{
				if (Player.GetRole(ev.Attacker) != RoleType.Scp106)
					if (Player.GetTeam(ev.Player) == Team.CDP)
					{
						if (UnityEngine.Random.Range(0, max_tries) > tries)
						{
							ev.Amount = 0f;
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(Configs.safeuserbc, 10);
							ev.Attacker.ClearBroadcasts();
							ev.Attacker.Broadcast(Configs.safeattackerbc, 10);
						}
						tries++;
					}
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			ReferenceHub hub = ev.Shooter;
			if (ev.Target == null || scp682 == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;
			if ((ev.Shooter.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
				Player.GetTeam(target) == Player.GetTeam(scp682))
				|| (target.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
				Player.GetTeam(ev.Shooter) == Player.GetTeam(scp682)))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}

			// If friendly fire is off, to allow for chaos and dclass to hurt eachother
			if ((ev.Shooter.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId || target.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId) &&
				(((Player.GetTeam(ev.Shooter) == Team.CDP && Player.GetTeam(target) == Team.CHI)
				|| (Player.GetTeam(ev.Shooter) == Team.CHI && Player.GetTeam(target) == Team.CDP)) ||
				((Player.GetTeam(ev.Shooter) == Team.RSC && Player.GetTeam(target) == Team.MTF)
				|| (Player.GetTeam(ev.Shooter) == Team.MTF && Player.GetTeam(target) == Team.RSC))))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}
		}
		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
				Timing.CallDelayed(0.5f, () => Map.ClearBroadcasts());
				Timing.CallDelayed(0.5f, () => Map.Broadcast(Configs.scpgodescapebc, Configs.scpgodescapebctime));
				Cassie.CassieMessage(Configs.scpgodescapecassie, false, false);
			}
		}
		public void OnSetClass(SetClassEvent ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if((scp682.GetRole() == RoleType.Spectator))
				{
					Killscp682();
				}
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if ((scp343.GetRole() == RoleType.Spectator))
				{
					Killscp343();
					Cassie.CassieMessage(Configs.scpgodripcassie, false, false);
				}
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				repscp682();
				selectspawnJG2();
				scp682.SetPosition(ev.Player.GetPosition());
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
				selectspawnSSS2();
				scp343.SetPosition(ev.Player.GetPosition());
			}
		}
		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				TeleportTo106(scp343);
			}
		}

		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			bool UnbreakableDoorDetected = false;
			if (doorInt.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				foreach (string doorName in unbreakableDoorNames)
					if (doorInt.Door.DoorName.Equals(doorName))
						UnbreakableDoorDetected = true;

				if (!UnbreakableDoorDetected)
					BreakDoor(doorInt);

				return;
			}
			else if (doorInt.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime >= (double)Configs.initialCooldown)
				{
					doorInt.Allow = true;
				}
				else if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime < (double)Configs.initialCooldown) 
				{
					if (!doorInt.Allow)
					{
						int i = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
						doorInt.Player.ClearBroadcasts();
						doorInt.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)i), 1U, false);
						Timing.CallDelayed(1U, () => {
							int ia = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)ia), 1U, false);
						});
						Timing.CallDelayed(2U, () => {
							int iq = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iq), 1U, false);
						});
						Timing.CallDelayed(3U, () => {
							int iz = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iz), 1U, false);
						});
						Timing.CallDelayed(4U, () => {
							int iw = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iw), 1U, false);
						});
					}
				}
			}
			else if (doorInt.Allow == false)
			{
				if (Player.GetTeam(doorInt.Player) == Team.CDP)
				{
					if (max_door_tries != 0)
					{
						if (UnityEngine.Random.Range(0, max_door_tries + 1) > door_tries)
						{
							doorInt.Allow = true;
							doorInt.Player.ClearBroadcasts();
							doorInt.Player.Broadcast(Configs.dooropenbc, 10);
						}
						door_tries++;
					}
				}
			}
		}
		public void OnStopCountdown(WarheadCancelEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = Configs.nuke;
			}
		}
		public void BreakDoor(DoorInteractionEvent door)
		{
			door.Door.DestroyDoor(true);
			door.Door.destroyed = true;
			door.Door.Networkdestroyed = true;
		}
		private void TeleportTo106(ReferenceHub player)
		{
			ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
			{

				Timing.CallDelayed(1f, () => player.SetPosition(scp106.GetPosition()));
			}
		}
		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void OnTeamRespawn(PlayerSpawnEvent ev)
		{
			GameCore.Console.singleton.TypeCommand($"/god {ev.Player.GetPlayerId()}. 1");
			Timing.CallDelayed(5f, () => GameCore.Console.singleton.TypeCommand($"/god {ev.Player.GetPlayerId()}. 0"));
		}
		public void RunOnRACommandSent(ref RACommandEvent RACom)
		{
			string[] command = RACom.Command.Split(' ');
			ReferenceHub sender = RACom.Sender.SenderId == "SERVER CONSOLE" || RACom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RACom.Sender.SenderId);

			switch (command[0].ToLower())
			{
				case "scp682":
					try
					{
						RACom.Allow = false;
						if (!CheckIfValueIsValid(Int32.Parse(command[1])))
						{
							RACom.Sender.RAMessage(Configs.errorinra);
							return;
						}

						ReferenceHub ChosenPlayer = new ReferenceHub();
						foreach (ReferenceHub hub in Player.GetHubs())
						{
							if (hub.GetPlayerId() == Int32.Parse(command[1]))
							{
								RACom.Sender.RAMessage(Configs.sucinra682);
								SpawnJG(hub);
							}
						}
					}
					catch (Exception)
					{
						RACom.Sender.RAMessage(Configs.errorinra);
						return;
					}
					break;
				case "scp343":
					RACom.Allow = false;
					try
					{
						if (!CheckIfValueIsValid(Int32.Parse(command[1])))
						{
							RACom.Sender.RAMessage(Configs.errorinra);
							return;
						}

						ReferenceHub ChosenPlayer = new ReferenceHub();
						foreach (ReferenceHub hub in Player.GetHubs())
						{
							if (hub.GetPlayerId() == Int32.Parse(command[1]))
							{
								RACom.Sender.RAMessage(Configs.sucinra343);
								Spawn343(hub);
							}
						}
					}
					catch (Exception)
					{
						RACom.Sender.RAMessage(Configs.errorinra);
						return;
					}
					break;
			}
		}

		private bool CheckIfValueIsValid(int id)
		{
			foreach (ReferenceHub hubs in Player.GetHubs())
			{
				if (hubs.GetPlayerId() == id)
					return true;
			}
			return false;
		}

	}
}
