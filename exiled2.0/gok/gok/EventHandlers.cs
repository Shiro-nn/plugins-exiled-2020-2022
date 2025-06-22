using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using scp035.API;
using scp343.API;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace gok
{
    public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public void WaitingForPlayers() => plugin.cfg1();
		public static List<int> gokPlayers = new List<int>();
		internal void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			if (ev.Name == "gok")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "Успешно";
				spawnteam();
			}
			try
			{
				if (ev.Name == "onegok")
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
			}
			catch
			{
				if (ev.Name == "onesh")
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = "Произошла ошибка";
					return;
				}
			}
		}
		public void OnRoundStart()
		{
			gokPlayers.Clear();
		}
		public void OnRoundEnd(RoundEndedEventArgs ev) => gokPlayers.Clear();
		internal void TeamRespawn(RespawningTeamEventArgs ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox && Random.Range(0, 100) < plugin.config.Chance)
			{
				ev.IsAllowed = false;
				spawnteam();
			}
		}
		public void Ending(EndingRoundEventArgs ev)
		{
			ReferenceHub scp035 = null;
			ReferenceHub scp343 = null;
			try { scp035 = Player.Get(Scp035Data.GetScp035()).ReferenceHub; } catch { }
			try { scp343 = Player.Get(Data.scp343().ReferenceHub).ReferenceHub; } catch { }
			bool MTFAlive = Player.List.Where(x => x.Team == Team.MTF).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.MTF ? 1 : 0) + gokPlayers.Where(x => Player.Get(x)?.Role == RoleType.Tutorial).Count() > 0;
			bool CiAlive = Player.List.Where(x => x.Team == Team.CHI).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.CHI ? 1 : 0) > 0;
			bool ScpAlive = Player.List.Where(x => x.Team == Team.SCP).ToList().Count + (scp035 != null && Player.Get(scp035)?.Role != RoleType.Spectator ? 1 : 0) > 0;
			bool DClassAlive = Player.List.Where(x => x.Team == Team.CDP).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.CDP ? 1 : 0) - (scp343 != null && Player.Get(scp343)?.Team == Team.CDP ? 1 : 0) > 0;
			bool ScientistsAlive = Player.List.Where(x => x.Team == Team.RSC).ToList().Count - (scp035 != null && Player.Get(scp035)?.Team == Team.RSC ? 1 : 0) > 0;
			ev.IsAllowed = false;
			if (ScpAlive && !MTFAlive && !DClassAlive && !ScientistsAlive)
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			else if (!ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			else if (!ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
		}
		internal void spawnteam()
		{
			int mp = 0;
			List<Player> newsh = Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList();
			foreach (Player sh in newsh)
			{
				if (mp < plugin.config.Max_players)
				{
					mp++;
					spawnonesh(sh);
				}
			}
			if (newsh.Count > 0) foreach (Player player in Player.List.Where(x => x.Team == Team.MTF || x.Role == RoleType.Scientist || gokPlayers.Contains(x.Id))) player.Broadcast(plugin.config.All_Spawn_bc_time, plugin.config.All_Spawn_bc);
		}
		public Vector3 getrandomspawnsh => Spawnpoint();
		private Vector3 Spawnpoint()
		{
			return new Vector3(0, 1002, 7);
		}
		public void spawnonesh(Player sh)
		{
			gokPlayers.Add(sh.Id);
			sh.ReferenceHub.characterClassManager.SetClassID(RoleType.Tutorial);
			sh.ClearBroadcasts();
			sh.Broadcast(plugin.config.Spawn_bc_time, plugin.config.Spawn_bc);
			sh.ReferenceHub.inventory.items.ToList().Clear();
			for (int i = 0; i < plugin.config.SpawnItems.Count; i++)
				sh.Inventory.AddNewItem((ItemType)plugin.config.SpawnItems[i]);
			sh.ReferenceHub.playerStats.Health = plugin.config.Hp;
			Timing.CallDelayed(0.3f, () => sh.Position = getrandomspawnsh);
		}
		public void OnPlayerJoin(VerifiedEventArgs ev)
		{
			if (gokPlayers.Contains(ev.Player.Id))
				gokPlayers.Remove(ev.Player.Id);
		}
		internal void died(DiedEventArgs ev)
		{
			if (gokPlayers.Contains(ev.Target.Id))
				gokPlayers.Remove(ev.Target.Id);
		}
		public void setrole(ChangingRoleEventArgs ev)
		{
			if (gokPlayers.Contains(ev.Player.Id))
				if (ev.NewRole != RoleType.Tutorial)
					if (gokPlayers.Contains(ev.Player.Id))
						gokPlayers.Remove(ev.Player.Id);
		}
	}
}