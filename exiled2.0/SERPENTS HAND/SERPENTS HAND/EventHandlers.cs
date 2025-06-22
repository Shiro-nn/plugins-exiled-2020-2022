using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using scp035.API;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace serpentshand
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public void WaitingForPlayers() => plugin.cfg1();
		public static List<int> shPlayers = new List<int>();
		internal void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			if (ev.Name == "sh")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "Успешно";
				spawnteam();
			}
			if (ev.Name == "onesh")
			{
				try
				{
					string name = string.Join(" ", ev.Arguments.Skip(0));
					Player player = Player.Get(name);
					ev.IsAllowed = false;
					if (player == null)
					{
						ev.ReplyMessage = "хмм, не нашел игрока";
						return;
					}
					ev.ReplyMessage = "успешно";
					spawnonesh(player);
				}
				catch
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = "Произошла ошибка";
					return;
				}
			}
		}
		public void OnRoundStart() => shPlayers.Clear();

		public void OnRoundEnd(RoundEndedEventArgs ev) => shPlayers.Clear();
		internal void TeamRespawn(RespawningTeamEventArgs ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency && Random.Range(0, 100) < plugin.config.Chance)
			{
				ev.IsAllowed = false;
				spawnteam();
			}
			else if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency) Map.Broadcast(plugin.config.ciBcTime, plugin.config.ciBc);
		}
		internal void spawnteam()
		{
			int mp = 0;
			List<Player> newsh = Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList();
			if (newsh.Count > 0) Map.Broadcast(plugin.config.Map_Spawn_bc_time, plugin.config.Map_Spawn_bc);
			foreach (Player sh in newsh)
			{
				if (mp < plugin.config.Max_players)
				{
					mp++;
					spawnonesh(sh);
				}
			}
		}
		public void spawnonesh(Player sh)
		{
			shPlayers.Add(sh.Id);
			sh.Role = RoleType.Tutorial;
			sh.ClearBroadcasts();
			sh.Broadcast(plugin.config.Spawn_bc_time, plugin.config.Spawn_bc);
			Timing.CallDelayed(0.5f, () =>
			{
				sh.Inventory.items.ToList().Clear();
				for (int i = 0; i < plugin.config.SpawnItems.Count; i++)
					sh.Inventory.AddNewItem((ItemType)plugin.config.SpawnItems[i]);
				if (sh.Role == RoleType.Tutorial)
				{
					sh.ReferenceHub.playerStats.Health = plugin.config.Hp;
					sh.ReferenceHub.playerStats.maxHP = plugin.config.Hp;
				}
			});
			Timing.CallDelayed(0.3f, () => sh.Position = new Vector3(85.293f, 988.7609f, -68.15958f));
		}









		public void OnPlayerJoin(VerifiedEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
				shPlayers.Remove(ev.Player.Id);
		}
		public void Leave(LeftEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
			{
				shPlayers.Remove(ev.Player.Id);
			}
		}
		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPocketDimensionEscaping(EscapingPocketDimensionEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
			}
		}
		private void TeleportTo106(Player player)
		{
			Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
				player.ReferenceHub.playerMovementSync.OverridePosition(scp106.ReferenceHub.transform.position, 0f);
			else player.Position = Map.GetRandomSpawnPoint(RoleType.Scp096);
		}
		internal void hurt(HurtingEventArgs ev)
		{
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == 0) return;
			ReferenceHub target = ev.Target.ReferenceHub;
			ReferenceHub attacker = ev.Attacker.ReferenceHub;
			if (((shPlayers.Contains(target.queryProcessor.PlayerId) && Player.Get(target).Role == RoleType.Tutorial && (ev.Attacker.Team == Team.SCP || ev.DamageType == DamageTypes.Pocket)) ||
				(shPlayers.Contains(attacker.queryProcessor.PlayerId) && Player.Get(attacker).Role == RoleType.Tutorial && (ev.Target.Team == Team.SCP)) ||
				(shPlayers.Contains(target.queryProcessor.PlayerId) && Player.Get(target).Role == RoleType.Tutorial && Player.Get(attacker).Role == RoleType.Tutorial && shPlayers.Contains(attacker.queryProcessor.PlayerId) &&
				target.queryProcessor.PlayerId != attacker.queryProcessor.PlayerId)))
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
			else if (shPlayers.Contains(target.queryProcessor.PlayerId) && Player.Get(target).Role == RoleType.Tutorial && ev.Attacker.Team == Team.SCP)
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
			else if (shPlayers.Contains(attacker.queryProcessor.PlayerId) && Player.Get(attacker).Role == RoleType.Tutorial && ev.Target.Team == Team.SCP)
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
		}
		internal void died(DiedEventArgs ev)
		{
			if (shPlayers.Contains(ev.Target.ReferenceHub.queryProcessor.PlayerId))
				shPlayers.Remove(ev.Target.ReferenceHub.queryProcessor.PlayerId);
		}
		public void setrole(ChangingRoleEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
				if (ev.NewRole != RoleType.Tutorial)
					if (shPlayers.Contains(ev.Player.Id))
						shPlayers.Remove(ev.Player.Id);
		}
		public void scpzeroninesixe(EnragingEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
				ev.IsAllowed = false;
		}
		public void scpzeroninesixeadd(AddingTargetEventArgs ev)
		{
			if (shPlayers.Contains(ev.Target.ReferenceHub.queryProcessor.PlayerId) && ev.Target.Role == RoleType.Tutorial)
				ev.IsAllowed = false;
		}
		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			ReferenceHub scp035 = null;
			try { scp035 = Player.Get(Scp035Data.GetScp035()).ReferenceHub; } catch { }
			bool MTFAlive = Player.List.Where(x => x.Team == Team.MTF).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.MTF ? 1 : 0) > 0;
			bool CiAlive = Player.List.Where(x => x.Team == Team.CHI).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.CHI ? 1 : 0) > 0;
			bool ScpAlive = Player.List.Where(x => x.Team == Team.SCP).ToList().Count + (scp035 != null && Player.Get(scp035)?.Role != RoleType.Spectator ? 1 : 0) > 0;
			bool DClassAlive = Player.List.Where(x => x.Team == Team.CDP).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.CDP ? 1 : 0) > 0;
			bool ScientistsAlive = Player.List.Where(x => x.Team == Team.RSC).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.RSC ? 1 : 0) > 0;
			bool SHAlive = shPlayers.Where(x => Player.Get(x)?.Role == RoleType.Tutorial).ToList().Count > 0;
			ev.IsAllowed = false;
			if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive)
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			else if (!SHAlive && !ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			else if (!SHAlive && !ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
		}
	}
}