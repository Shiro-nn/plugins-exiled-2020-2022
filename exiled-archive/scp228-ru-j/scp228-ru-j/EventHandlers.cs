using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using hideandseek.API;
using Exiled.Events;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;

namespace scp228ruj
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		internal static ReferenceHub scp228ruj;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		public static bool pickupspawn;
		public const float dur = 327;
		public static Pickup vodka = new Pickup();
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		public static bool dedopen = false;
		public static bool aopen = false;
		public static bool checkopen = false;
		public static bool gateopen = false;
		public static bool ds = false;
		public static string vodka1 = Configs.error1;
		public static string vodka2 = Configs.error2;
		public static string vodkacolor = Configs.error3;

		public void OnWaitingForPlayers()
		{
			try
			{
				Configs.ReloadConfig();
			}
			catch { }
		}

		private bool Tryhason()
		{
			return hasData.Gethason();
		}
		public void OnRoundStart()
		{
			isRoundStarted = true;
			scp228ruj = null;
			RoundEnds = 100;
			ffPlayers.Clear();
			scpPlayer = null;
			pickupspawn = false;
			try
			{
				if (Tryhason()) return;
			}
			catch { }
			Timing.CallDelayed(1f, () => selectspawnJG());

			players.Clear();
			coroutines.Add(Timing.RunCoroutine(CorrodeUpdate()));

		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			scp228ruj = null;
			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{

			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDeath(Exiled.Events.EventArgs.DiedEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
			}
		}
		public void OnCheckRoundEnd(Exiled.Events.EventArgs.EndingRoundEventArgs ev)
		{
			List<Team> p2List = Player.List.Where(x => x.ReferenceHub.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId).Select(x => Extensions.GetTeam2(x.ReferenceHub)).ToList();

			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
			if ((!p2List.Contains(Team.MTF) && p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
		}
		public void OnPlayerHurt(Exiled.Events.EventArgs.HurtingEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					return;
				}
			}
			if (ffPlayers.Contains(ev.Attacker.ReferenceHub.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker.ReferenceHub);
			}
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Amount = 0f;
			}
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Amount = 0f;
			}
		}

		public void OnCheckEscape(Exiled.Events.EventArgs.EscapingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ds)
				{
					ev.IsAllowed = false;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.eebt, Configs.eeb);
				}
				if (!ds)
				{
					ev.NewRole = RoleType.Spectator;
					ev.IsAllowed = true;
					escapescp228ruj();
				}
			}
		}

		public void OnSetClass(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if ((scp228ruj.GetRole() == RoleType.Spectator))
				{
					Killscp228ruj();
				}
			}
		}

		public void OnPlayerLeave(Exiled.Events.EventArgs.LeftEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
			}
		}

		public void OnContain106(EnteringFemurBreakerEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				shPocketPlayers.Add(ev.Player.ReferenceHub.queryProcessor.PlayerId);
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player.ReferenceHub);
			}
		}

		public void OnPocketDimensionExit(EscapingPocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player.ReferenceHub);
			}
		}
		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.Pickup == vodka)
				{
					ev.IsAllowed = true;
					ds = false;
					checkopen = true;
					gateopen = true;
					ev.Player.ClearBroadcasts();
					scp228ruj.Broadcast(Configs.svb, Configs.svbt);
					return;
				}
				ev.IsAllowed = false;
			}
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.Pickup == vodka)
				{
					ev.IsAllowed = false;
					scp228ruj.ClearBroadcasts();
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.vppbt, Configs.vppb.Replace("%player%", $"{scp228ruj.nicknameSync.Network_myNickSync}"));
					scp228ruj.Broadcast(Configs.vpb.Replace("%player%", $"{ev.Player?.Nickname}"), Configs.vpbt);
					return;
				}
			}
		}
		public void scpzeroninesixe(TriggeringTeslaEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsTriggerable = false;
			}
		}
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (gateopen)
				{
					if (ev.Door.DoorName.Contains("GATE"))
					{
						ev.IsAllowed = true;
					}
				}
				if (checkopen)
				{
					if (ev.Door.DoorName.Contains("CHECKPOINT"))
					{
						ev.IsAllowed = true;
					}
				}
				if (dedopen)
				{
					if (ev.Door.DoorName.Contains("106"))
					{
						ev.IsAllowed = true;
					}
				}
				if (aopen)
				{
					if (ev.Door.DoorName.Contains("096"))
					{
						ev.IsAllowed = true;
					}
				}
			}
		}
		private void TeleportTo106(ReferenceHub player)
		{
			Player pscp106 = Player.List.Where(x => x.ReferenceHub.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			ReferenceHub scp106 = pscp106.ReferenceHub;
			if (scp106 != null)
			{
				player.playerMovementSync.OverridePosition(scp106.transform.position, 0f);
			}
		}
		private IEnumerator<float> CorrodeUpdate()
		{
			while (isRoundStarted)
			{
				if (scp228ruj != null)
				{
					IEnumerable<Player> pList = Player.List.Where(x => x.ReferenceHub.queryProcessor.PlayerId != scp228ruj.queryProcessor.PlayerId);
					if (!Configs.scpFriendlyFire) pList = pList.Where(x => Extensions.GetTeam2(x.ReferenceHub) != Team.SCP);
					if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Extensions.GetTeam2(x.ReferenceHub) != Team.TUT);
					foreach (Player pplayer in pList)
					{
						ReferenceHub player = pplayer.ReferenceHub;
						if (player != null && Vector3.Distance(scp228ruj.transform.position, player.transform.position) <= 2f)
						{
							CorrodePlayer(player);
							pplayer.ClearBroadcasts();
							pplayer.Broadcast(1, Configs.a, 0);
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		private static IEnumerator<float> Gopd()
		{
			for (; ; )
			{
				if (ds)
				{
					opd();
				}
				yield return Timing.WaitForSeconds(3f);
			}
		}
		private static void opd()
		{
			scp228ruj.Damage(1, DamageTypes.Nuke);
		}
		private void CorrodePlayer(ReferenceHub player)
		{
			player.Damage(5, DamageTypes.Nuke);
		}
		public void RunOnRACommandSent(SendingRemoteAdminCommandEventArgs ev)
		{
			try
			{
				string str2 = ev.Name + " ";
				foreach (string str3 in ev.Arguments)
					str2 = str2 + str3 + " ";
				string[] command = str2.Split(' ');
				Player player = Player.Get(command[1]);
				if (ev.Name.ToLower().StartsWith(Configs.com))
				{
					ev.IsAllowed = false;
					if (player == null)
					{
						ev.ReplyMessage = Configs.nf;
						return;
					}
					ev.ReplyMessage = Configs.suc;
					SpawnJG(player.ReferenceHub);
				}
			}
			catch (Exception e)
			{
				if (ev.Name.ToLower().StartsWith(Configs.com))
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = $"\nПроизошла ошибка:\n{e}";
				}
			}
		}
	}
}