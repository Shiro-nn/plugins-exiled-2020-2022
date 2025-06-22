using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SerpentsHand
{
	public class EventHandlers
	{
		internal void SpawnSquad()
		{
			List<Player> list = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToList();
			if (list.Count > 0) Map.Broadcast(Cfg.Map_Spawn_bc, Cfg.Map_Spawn_bc_time);
			list.ShuffleList();
			for (int i = 0; i < list.Count && i < Cfg.Max_players; i++)
				SpawnPlayer(list[i]);
		}
		private const string Tag = " Serpent's Hand";
		public static void SpawnPlayer(Player sh)
		{
			sh.Tag += Tag;
			sh.SetRole(RoleType.Tutorial, false, CharacterClassManager.SpawnReason.Respawn);
			sh.ClearBroadcasts();
			sh.Broadcast(Cfg.Spawn_bc_time, Cfg.Spawn_bc);
			Timing.CallDelayed(0.5f, () =>
			{
				sh.ClearInventory();
				sh.AddItem(ItemType.Ammo12gauge, 3);
				sh.AddItem(ItemType.Ammo44cal, 3);
				sh.AddItem(ItemType.Ammo556x45, 3);
				sh.AddItem(ItemType.Ammo762x39, 3);
				sh.AddItem(ItemType.Ammo9x19, 3);
				for (int i = 0; i < Cfg.SpawnItems.Count; i++)
					sh.AddItem((ItemType)Cfg.SpawnItems[i]);
				if (sh.Role == RoleType.Tutorial)
				{
					sh.Hp = Cfg.Hp;
					sh.MaxHp = Cfg.Hp;
				}
				sh.UnitName = Cfg.UnitName;
			});
		}


		internal void Waiting() => Cfg.Reload();
		internal void Ra(SendingRAEvent ev)
		{
			if (ev.Name == "sh")
			{
				ev.Allowed = false;
				ev.ReplyMessage = "Successfully";
				SpawnSquad();
			}
			if (ev.Name == "onesh")
			{
				ev.Allowed = false;
				try
				{
					string name = string.Join(" ", ev.Args.Skip(0));
					Player player = Player.Get(name);
					if (player == null)
					{
						ev.ReplyMessage = "Player not found";
						return;
					}
					ev.ReplyMessage = "Successfully";
					SpawnPlayer(player);
				}
				catch
				{
					ev.ReplyMessage = "An error has occurred";
				}
			}
		}
		internal void TeamRespawn(TeamRespawnEvent ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency && Extensions.Random.Next(0, 100) < Cfg.Chance)
			{
				ev.Allowed = false;
				SpawnSquad();
			}
		}
		internal void PocketFail(PocketFailEscapeEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			List<Vector3> tp = new List<Vector3>();
			foreach (GameObject _go in GameObject.FindGameObjectsWithTag("PD_EXIT"))
				tp.Add(_go.transform.position);
			var pos = tp[Random.Range(0, tp.Count)];
			pos.y += 2f;
			ev.Player.ReferenceHub.playerMovementSync.AddSafeTime(2f, false);
			ev.Player.Position = pos;
		}
		internal void PocketEnter(PocketEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (ev.Attacker.Id == 0) return;
			if ((ev.Target.Tag.Contains(Tag) && (ev.Attacker.Team == Team.SCP || ev.DamageType == DamageTypes.Pocket)) ||
				(ev.Attacker.Tag.Contains(Tag) && ev.Target.Team == Team.SCP) ||
				(ev.Target.Tag.Contains(Tag) && ev.Attacker.Tag.Contains(Tag) && ev.Target != ev.Attacker))
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}
		}
		internal void Dead(DeadEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag)) ev.Target.Tag = ev.Target.Tag.Replace(Tag, "");
		}
		internal void RoleChange(RoleChangeEvent ev)
		{
			if (ev.NewRole != RoleType.Tutorial && ev.Player.Tag.Contains(Tag)) ev.Player.Tag = ev.Player.Tag.Replace(Tag, "");
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
            {
				if(ev.RoleType != RoleType.Tutorial) ev.Player.Tag = ev.Player.Tag.Replace(Tag, "");
				else ev.Position = new Vector3(85.293f, 988.7609f, -68.15958f);
			}
		}
		internal void AddTarget(AddTargetEvent ev)
		{
			if (!ev.Target.Tag.Contains(Tag)) return;
			ev.Allowed = false;
		}
		internal void Check(CheckEvent ev)
		{
			Player scp035 = null;
			try { scp035 = Player.List.Where(x => x.Tag.Contains(" scp035")).First(); } catch { }
			Player scp343ByKnuckles = null;
			try { scp343ByKnuckles = Player.List.Where(x => x.Tag.Contains("scp343-knuckles")).First(); } catch { }
			bool MTFAlive = Player.List.Where(x => x.Team == Team.MTF).ToList().Count - (scp035 != null && scp035?.Team == Team.MTF ? 1 : 0) - (scp343ByKnuckles != null && scp343ByKnuckles?.Team == Team.MTF ? 1 : 0) > 0;
			bool CiAlive = Player.List.Where(x => x.Team == Team.CHI).ToList().Count - (scp035 != null && scp035?.Team == Team.CHI ? 1 : 0) - (scp343ByKnuckles != null && scp343ByKnuckles?.Team == Team.CHI ? 1 : 0) > 0;
			bool ScpAlive = Player.List.Where(x => x.Team == Team.SCP).ToList().Count + (scp035 != null && scp035?.Role != RoleType.Spectator ? 1 : 0) > 0;
			bool DClassAlive = Player.List.Where(x => x.Team == Team.CDP).ToList().Count - (scp035 != null && scp035?.Team == Team.CDP ? 1 : 0) - (scp343ByKnuckles != null && scp343ByKnuckles?.Team == Team.CDP ? 1 : 0) > 0;
			bool ScientistsAlive = Player.List.Where(x => x.Team == Team.RSC).ToList().Count - (scp035 != null && scp035?.Team == Team.RSC ? 1 : 0) - (scp343ByKnuckles != null && scp343ByKnuckles?.Team == Team.RSC ? 1 : 0) > 0;
			bool SHAlive = Player.List.Where(x => x.Tag.Contains(Tag)).ToList().Count > 0;
			ev.RoundEnd = false;
			if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive)
			{
				ev.RoundEnd = true;
			}
			else if (!SHAlive && !ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
			{
				ev.RoundEnd = true;
			}
			else if (!SHAlive && !ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
			{
				ev.RoundEnd = true;
			}
		}
	}
}