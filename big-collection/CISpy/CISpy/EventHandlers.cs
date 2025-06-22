using EXILED;
using EXILED.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CISpy
{
	partial class EventHandlers
	{
		internal static Dictionary<ReferenceHub, bool> spies = new Dictionary<ReferenceHub, bool>();
		private List<ReferenceHub> ffPlayers = new List<ReferenceHub>();
		internal static bool cispyview = false;
		private bool isDisplayFriendly = false;
		//private bool isDisplaySpy = false;

		private Random rand = new Random();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfigs();
		}

		public void OnRoundStart()
		{
			spies.Clear();
			ffPlayers.Clear();
			if (rand.Next(1, 101) <= Configs.guardSpawnChance)
			{
				ReferenceHub player = Player.GetHubs().FirstOrDefault(x => x.GetRole() == RoleType.FacilityGuard);
				if (player != null)
				{
					MakeSpy(player);
				}
			}
		}

		public void OnTeamRespawn(ref TeamRespawnEvent ev)
		{
			if (!ev.IsChaos && rand.Next(1, 101) <= Configs.spawnChance && ev.ToRespawn.Count >= Configs.minimumSquadSize)
			{
				List<ReferenceHub> respawn = new List<ReferenceHub>(ev.ToRespawn);
				Timing.CallDelayed(0.1f, () =>
				{
					Log.Warn(Configs.spyRoles.Count.ToString());
					List<ReferenceHub> roleList = respawn.Where(x => Configs.spyRoles.Contains((int)x.GetRole())).ToList();
					if (roleList.Count > 0)
					{
						ReferenceHub player = roleList[rand.Next(roleList.Count)];
						if (player != null)
						{
							MakeSpy(player);
						}
					}
				});
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (spies.ContainsKey(ev.Player))
			{
				Timing.CallDelayed(0.1f, () => spies.Remove(ev.Player));
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (spies.ContainsKey(ev.Player))
			{
				spies.Remove(ev.Player);
				ev.Player.SetRank("");
			}

			ReferenceHub player = ev.Player;

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			int playerid = ev.Player.queryProcessor.PlayerId;
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != playerid && !spies.ContainsKey(x) && x.queryProcessor.PlayerId != scp035?.queryProcessor.PlayerId).Select(x => x.GetTeam()).ToList();

			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP)) ||
			((pList.Contains(Team.CDP) || pList.Contains(Team.CHI)) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC)))
			{
				RevealSpies();
			}
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ffPlayers.Contains(ev.Attacker))
			{
				RemoveFF(ev.Attacker);
			}

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			if (spies.ContainsKey(ev.Player) && !spies.ContainsKey(ev.Attacker) && ev.Player.queryProcessor.PlayerId != ev.Attacker.queryProcessor.PlayerId && (ev.Attacker.GetTeam() == Team.CHI || ev.Attacker.GetTeam() == Team.CDP))
			{
				if (!isDisplayFriendly)
				{
					ev.Attacker.Broadcast(Configs.teamshootbc, Configs.teamshootbct);
					isDisplayFriendly = true;
				}
				Timing.CallDelayed(3f, () =>
				{
					isDisplayFriendly = false;
				});
				ev.Amount = 0;
			}
			else if (!spies.ContainsKey(ev.Player) && spies.ContainsKey(ev.Attacker) && (ev.Player.GetTeam() == Team.CHI || ev.Player.GetTeam() == Team.CDP) && ev.Player.queryProcessor.PlayerId != scp035?.queryProcessor.PlayerId)
			{
				ev.Amount = 0;
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Target == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			if (spies.ContainsKey(ev.Shooter) && !spies.ContainsKey(target) && (Player.GetTeam(target) == Team.RSC || Player.GetTeam(target) == Team.MTF) && target.queryProcessor.PlayerId != scp035?.queryProcessor.PlayerId)
			{
				if (!spies[ev.Shooter])
				{
					spies[ev.Shooter] = true;
					ev.Shooter.Broadcast(Configs.shootbc.Replace("%team%", $"{(target.GetTeam() == Team.MTF ? "<color=#00b0fc>Nine Tailed Fox" : $"<color=#fcff8d>{Configs.scientist}</color>")}"), Configs.shootbct);
					ev.Shooter.SetRank(Configs.role, Configs.rolec);
					cispyview = true;
				}
				GrantFF(ev.Shooter);
			}
			else if (spies.ContainsKey(target) && !spies.ContainsKey(ev.Shooter) && (ev.Shooter.GetTeam() == Team.MTF || ev.Shooter.GetTeam() == Team.RSC))
			{
				if (spies[target])
				{
					GrantFF(ev.Shooter);
				}
			}
		}
	}
}