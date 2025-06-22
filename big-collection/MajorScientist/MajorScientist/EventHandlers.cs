using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;

namespace MajorScientist
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		internal static ReferenceHub MajorScientist;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		private static int maxHP;
		private int MajorEscape = 0;
		private const float dur = 327;
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private static int AlternativeEnds;
		internal static ReferenceHub MiscMember;

		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			MajorScientist = null;
			MajorEscape = 0;
			AlternativeEnds = 0;
			RoundEnds = 100;

			Timing.CallDelayed(1f, () => selectspawnMS());


		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;

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
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerDie");
				}


			}
			if (ev.Killer.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				Timing.CallDelayed(0.5f, () => MajorScientist.SetRank("Главный Ученый", "yellow"));
			}
		}
		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> EpList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != MajorScientist?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();
			List<ReferenceHub> EpmList = Player.GetHubs().Where(x => x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (((MajorScientist != null)) && (!EpList.Contains(Team.CDP)) && (!EpList.Contains(Team.SCP)) && (!EpList.Contains(Team.CHI)) && (!EpList.Contains(Team.TUT)) && (!EpList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}
			else if (((MajorScientist != null)) && (!EpList.Contains(Team.CDP)) && (!EpList.Contains(Team.SCP)) && (!EpList.Contains(Team.CHI)) && (!EpList.Contains(Team.TUT)) && (EpList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}


		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				if (hasTag) MajorScientist.RefreshTag();
				if (isHidden) MajorScientist.HideTag();
				MajorScientist = null;
				MajorEscape = 1;

				if (Configs.log)
					Log.Info("Major Scientist has escaped.");
			}

			if (ev.Player.queryProcessor.PlayerId != MajorScientist?.queryProcessor.PlayerId)
			{
				if(MajorEscape == -1)
				{
					if (ev.Player.GetRole() == RoleType.Scientist)
					{
						if (ev.Player.IsHandCuffed() == false)
						{
							ev.Allow = false;
							ev.Player.ChangeRole(RoleType.NtfScientist);
							ev.Player.inventory.AddNewItem(ItemType.KeycardNTFLieutenant);
							ev.Player.inventory.AddNewItem(ItemType.GrenadeFrag);
							ev.Player.inventory.AddNewItem(ItemType.Medkit);
							ev.Player.inventory.AddNewItem(ItemType.Radio);
							ev.Player.inventory.AddNewItem(ItemType.WeaponManagerTablet);
						}
					}

					else if (ev.Player.GetRole() == RoleType.ClassD)
					{
						if (ev.Player.IsHandCuffed() == true)
						{
							ev.Allow = false;
							ev.Player.ChangeRole(RoleType.NtfCadet);
							ev.Player.inventory.AddNewItem(ItemType.KeycardSeniorGuard);
							ev.Player.inventory.AddNewItem(ItemType.Disarmer);
							ev.Player.inventory.AddNewItem(ItemType.GunProject90);
							ev.Player.inventory.AddNewItem(ItemType.Medkit);
							ev.Player.inventory.AddNewItem(ItemType.Radio);
							ev.Player.inventory.AddNewItem(ItemType.WeaponManagerTablet);

						}
					}
				}
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				if((MajorScientist.GetRole() == RoleType.Spectator))
				{
					KillMajorScientist();
					MajorEscape = -1;

					
					if (Configs.log)
						if(MajorEscape == -1)
							Log.Info("It seems Major Scientist has died for sure. -Setclass");
				}

				
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerLeave");
				}
			}

		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - Contain106");
				}

			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. -  PocketDimesionDie");
				}
			}
		}

		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				MajorScientist.playerStats.maxHP = Configs.health;
			}
		}
	}
}
