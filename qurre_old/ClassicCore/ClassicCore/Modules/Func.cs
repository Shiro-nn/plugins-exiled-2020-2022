using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClassicCore.Modules
{
	static internal class Func
	{
		static internal void UpdateDeaths(DiesEvent ev)
		{
			if (!ev.Allowed) return;
			if (ev.Target.Team == Team.SCP || ev.Target.Team == Team.TUT) return;
			if (Cache.Roles.ContainsKey(ev.Target.Id)) Cache.Roles[ev.Target.Id] = ev.Target.Role;
			else Cache.Roles.Add(ev.Target.Id, ev.Target.Role);
		}
		static internal bool IsAlive(string userid)
		{
			if (!Cache.Pos.TryGetValue(userid, out var _data)) return true;
			return _data.Alive;
		}
		static internal void PosCheck()
		{
			try
			{
				foreach (Player pl in Player.List)
				{
					if (!Cache.Pos.ContainsKey(pl.UserId)) Cache.Pos.Add(pl.UserId, new VecPos());
					if (pl.Role is not RoleType.Spectator && Vector3.Distance(Cache.Pos[pl.UserId].Pos, pl.Position) < 0.1)
					{
						if (Cache.Pos[pl.UserId].sec > 30)
						{
							Cache.Pos[pl.UserId].Alive = false;
						}
						else
						{
							Cache.Pos[pl.UserId].sec += 5;
							Cache.Pos[pl.UserId].Pos = pl.Position;
						}
					}
					else
					{
						Cache.Pos[pl.UserId].Alive = true;
						Cache.Pos[pl.UserId].sec = 0;
						Cache.Pos[pl.UserId].Pos = pl.Position;
					}
				}
			}
			catch { }
		}
		static internal void CheckEscape(Player pl)
		{
			if (!Round.Started) return;
			if (pl is not null && pl.Escape is not null && Vector3.Distance(pl.Position, pl.Escape.worldPosition) < Escape.radius)
			{
				bool _cuffed = pl.Cuffed;
				if (!_cuffed && pl.Role is RoleType.FacilityGuard)
				{
					pl.Position = Map.GetRandomSpawnPoint(RoleType.NtfSpecialist);
					pl.BlockSpawnTeleport = true;
					pl.DropItems();
					pl.SetRole(RoleType.NtfSpecialist, false, CharacterClassManager.SpawnReason.Respawn);
				}
				else if (_cuffed)
				{
					var team = pl.Team;
					if (team is Team.CHI) pl.SetRole(RoleType.NtfSergeant, false, CharacterClassManager.SpawnReason.Respawn);
					else if (team is Team.MTF) pl.SetRole(RoleType.ChaosRepressor, false, CharacterClassManager.SpawnReason.Respawn);
				}
			}
		}
	}
}