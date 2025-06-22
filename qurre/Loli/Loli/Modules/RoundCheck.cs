using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Modules
{
	static class RoundCheck
	{
		static int Cdp = 0;
		static int Rsc = 0;

		[EventMethod(RoundEvents.Waiting)]
		static void WaitingPlayers()
		{
			Rsc = 0;
			Cdp = 0;
		}

		[EventMethod(PlayerEvents.Escape)]
		static void Escape(EscapeEvent ev)
		{
			if (!ev.Allowed) return;

			var role = ev.Player.GetCustomRole();
			if (role == RoleType.Scientist)
			{
				if (ev.Player.GamePlay.Cuffed) Cdp++;
				else Rsc++;
			}
			else if (role == RoleType.ClassD)
			{
				if (ev.Player.GamePlay.Cuffed) Rsc++;
				else Cdp++;
			}
		}

		[EventMethod(RoundEvents.Check)]
		static void RoundEnding(RoundCheckEvent ev)
		{
			try
			{
				ev.End = false;

				if (Addons.OmegaWarhead.Detonated)
				{
					ev.Winner = LeadingTeam.ChaosInsurgency;
					ev.End = true;
					return;
				}

				if (Addons.OmegaWarhead.InProgress)
					return;
				if (Round.ElapsedTime.TotalMinutes < 1)
					return;

				bool end = false;
				int cw = 0;
				int mw = 0;
				int sw = 0;
				int nd = 0;
				int ns = 0;
				int ci = 0;
				int mtf = 0;
				int scp = 0;
				int sh = 0;
				int zombies = 0;
				foreach (var pl in Player.List)
				{
					if (pl.ItsScp035()) scp++;
					else if (Spawns.SerpentsHand.ItsAliveHand(pl)) sh++;
					else switch (pl.RoleInfomation.Team)
						{
							case Team.ClassD:
								nd++;
								break;
							case Team.Scientists:
								ns++;
								break;
							case Team.ChaosInsurgency:
								ci++;
								break;
							case Team.FoundationForces:
								mtf++;
								break;
							case Team.SCPs:
								{
									scp++;
									if (pl.RoleInfomation.Role is RoleTypeId.Scp0492) zombies++;
									break;
								}
						}
				}
				int d = Cdp + nd;
				int s = Rsc + ns;
				bool MTFAlive = mtf > 0;
				bool CiAlive = ci > 0;
				bool ScpAlive = scp > 0;
				bool DClassAlive = nd > 0;
				bool ScientistsAlive = ns > 0;
				bool SHAlive = sh > 0;
				var cList = new RoundSummary.SumInfo_ClassList
				{
					class_ds = d,
					scientists = s,
					chaos_insurgents = ci,
					mtf_and_guards = mtf,
					scps_except_zombies = scp - zombies,
					zombies = zombies,
					warhead_kills = ev.Info.warhead_kills,
				};
				ev.Info = cList;
				if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive)
				{
					end = true;
					sw++;
				}
				else if (!SHAlive && !ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
				{
					end = true;
					mw++;
				}
				else if (!SHAlive && !ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
				{
					end = true;
					cw++;
				}
				else if (!ScpAlive && !MTFAlive && !ScientistsAlive && !DClassAlive && !CiAlive)
				{
					end = true;
				}
				if (end)
				{
					ev.End = true;
					if (d > s) cw++;
					else if (d < s) mw++;
					else if (scp > d + s) sw++;
					if (ci > mtf) cw++;
					else if (ci < mtf) mw++;
					else if (scp > ci + mtf) sw++;
					if (cw > mw)
					{
						if (cw > sw) ev.Winner = LeadingTeam.ChaosInsurgency;
						else if (mw < sw) ev.Winner = LeadingTeam.Anomalies;
						else ev.Winner = LeadingTeam.Draw;
					}
					else if (mw > cw)
					{
						if (mw > sw) ev.Winner = LeadingTeam.FacilityForces;
						else if (cw < sw) ev.Winner = LeadingTeam.Anomalies;
						else ev.Winner = LeadingTeam.Draw;
					}
					else ev.Winner = LeadingTeam.Draw;
				}
			}
			catch { }
		}
	}
}