using MEC;
using Qurre.API;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace gok
{
	internal class EventHandlers
	{
		private const string Tag = "gok";
		internal void Waiting() => Cfg.Reload();
		internal void Ra(SendingRAEvent ev)
		{
			if (ev.Name == "gok")
			{
				ev.Allowed = false;
				ev.ReplyMessage = "Успешно";
				SpawnTeam();
			}
			else if (ev.Name == "onegok")
			{
				ev.Allowed = false;
				try
				{
					string name = string.Join(" ", ev.Args);
					Player player = Player.Get(name);
					if (player == null)
					{
						ev.ReplyMessage = "хмм, не нашел игрока";
						return;
					}
					ev.ReplyMessage = "успешно";
					SpawnOne(player);
				}
				catch
				{
					ev.ReplyMessage = "Произошла ошибка";
				}
			}
		}
		internal void TeamRespawn(TeamRespawnEvent ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox && Random.Range(0, 100) < Cfg.Chance)
			{
				ev.Allowed = false;
				SpawnTeam();
			}
		}/*
		public void CheckRound(CheckEvent ev)
		{
			Player scp035 = null;
			Player scp343 = null;
			int mc = 0;
			int cc = 0;
			int sc = 0;
			int dc = 0;
			int scc = 0;
			foreach (var pl in Player.List)
			{
				if (scp035 is null && pl.Tag.Contains("scp035")) scp035 = pl;
				else if (scp343 is null && pl.Tag.Contains("scp343")) scp343 = pl;
				else switch (pl.Team)
					{
						case Team.MTF: mc++; break;
						case Team.CHI: cc++; break;
						case Team.SCP: sc++; break;
						case Team.CDP: dc++; break;
						case Team.RSC: scc++; break;

					}
			}
			bool MTFAlive = mc > 0;
			bool CiAlive = cc > 0;
			bool ScpAlive = sc > 0;
			bool DClassAlive = dc > 0;
			bool ScientistsAlive = scc > 0;
			ev.RoundEnd = false;
			if (ScpAlive && !MTFAlive && !DClassAlive && !ScientistsAlive)
			{
				ev.RoundEnd = true;
			}
			else if (!ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
			{
				ev.RoundEnd = true;
			}
			else if (!ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
			{
				ev.RoundEnd = true;
			}
		}*/
		internal void SpawnTeam()
		{
			int mp = 0;
			List<Player> newsh = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToList();
			foreach (Player sh in newsh)
			{
				if (mp < Cfg.Max_players)
				{
					mp++;
					SpawnOne(sh);
				}
			}
			if (newsh.Count > 0) foreach (Player player in Player.List.Where(x => x.Team == Team.MTF || x.Role == RoleType.Scientist || x.Tag.Contains(Tag))) player.Broadcast(Cfg.All_Spawn_bc_time, Cfg.All_Spawn_bc);
		}
		internal Vector3 SpawnPos => new Vector3(0, 1002, 7);
		internal void SpawnOne(Player sh)
		{
			sh.Tag += Tag;
			sh.Role = RoleType.Tutorial;
			sh.ClearBroadcasts();
			sh.Broadcast(Cfg.Spawn_bc_time, Cfg.Spawn_bc);
			sh.ClearInventory();
			for (int i = 0; i < Cfg.SpawnItems.Count; i++)
				sh.AddItem((ItemType)Cfg.SpawnItems[i]);
			sh.Hp = Cfg.Hp;
			Timing.CallDelayed(0.5f, () => sh.UnitName = "Глобальная оккультная коалиция");
		}
		internal void Dead(DeadEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag))
				ev.Target.Tag = ev.Target.Tag.Replace(Tag, "");
		}
		internal void Spawn(RoleChangeEvent ev)
		{
			if (ev.NewRole != RoleType.Tutorial && ev.Player.Tag.Contains(Tag))
				ev.Player.Tag = ev.Player.Tag.Replace(Tag, "");
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (ev.RoleType == RoleType.Tutorial && ev.Player.Tag.Contains(Tag))
				ev.Position = SpawnPos;
		}
	}
}