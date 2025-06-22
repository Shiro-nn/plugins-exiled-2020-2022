using Loli.AutoEvents;
using Loli.Scps.Api;
using Qurre.API;
using Qurre.API.Events;
using System.Linq;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		private int Cdp = 0;
		private int Rsc = 0;
		internal void Escape(EscapeEvent ev)
		{
			if (!ev.Allowed) return;
			if (ev.Player.GetCustomRole() == Module.RoleType.Scientist)
			{
				if (ev.Player.Cuffed) Cdp++;
				else Rsc++;
			}
			if (ev.Player.GetCustomRole() == Module.RoleType.ClassD)
			{
				if (ev.Player.Cuffed) Rsc++;
				else Cdp++;
			}
		}
		internal void RoundEnding(CheckEvent ev)
		{
			try
			{
				if (Plugin.ClansWars) return;
				if (StormBase.Enabled) return;
				ev.RoundEnd = false;
				if (Addons.OmegaWarhead.Detonated)
				{
					ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
					ev.RoundEnd = true;
					return;
				}
				if (Addons.OmegaWarhead.InProgress) return;
				if (Round.ElapsedTime.TotalMinutes < 1) return;
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
					else switch (pl.Team)
						{
							case Team.CDP:
								nd++;
								break;
							case Team.RSC:
								ns++;
								break;
							case Team.CHI:
								ci++;
								break;
							case Team.MTF:
								mtf++;
								break;
							case Team.SCP:
								{
									scp++;
									if (pl.Role is RoleType.Scp0492) zombies++;
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
					warhead_kills = ev.ClassList.warhead_kills,
					time = ev.ClassList.time
				};
				ev.ClassList = cList;
				if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive && !(Plugin.RolePlay && CiAlive))
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
					ev.RoundEnd = true;
					if (d > s) cw++;
					else if (d < s) mw++;
					else if (scp > d + s) sw++;
					if (ci > mtf) cw++;
					else if (ci < mtf) mw++;
					else if (scp > ci + mtf) sw++;
					if (cw > mw)
					{
						if (cw > sw) ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
						else if (mw < sw) ev.LeadingTeam = LeadingTeam.Anomalies;
						else ev.LeadingTeam = LeadingTeam.Draw;
					}
					else if (mw > cw)
					{
						if (mw > sw) ev.LeadingTeam = LeadingTeam.FacilityForces;
						else if (cw < sw) ev.LeadingTeam = LeadingTeam.Anomalies;
						else ev.LeadingTeam = LeadingTeam.Draw;
					}
					else ev.LeadingTeam = LeadingTeam.Draw;
				}
			}
			catch { }
		}
	}
}