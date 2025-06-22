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
				var scp035 = Scp035.Get035();
				if (Round.ElapsedTime.TotalMinutes < 1) return;
				bool end = false;
				int cw = 0;
				int mw = 0;
				int sw = 0;
				int dc = Extensions.CountRoles(Team.CDP);
				int sc = Extensions.CountRoles(Team.RSC);
				int d = Cdp + dc;
				int s = Rsc + sc;
				int ci = Extensions.CountRoles(Team.CHI);
				int mtf = Extensions.CountRoles(Team.MTF);
				int scp = Extensions.CountRoles(Team.SCP) + scp035.Count;
				bool MTFAlive = mtf > 0;
				bool CiAlive = ci > 0;
				bool ScpAlive = scp > 0;
				bool DClassAlive = dc > 0;
				bool ScientistsAlive = sc > 0;
				bool SHAlive = Player.List.Where(x => Spawns.SerpentsHand.ItsAliveHand(x)).Count() > 0;
				if (Round.ElapsedTime.TotalMinutes < 1) return;
				var scps = Player.List.Where(x => x.Team == Team.SCP);
				var cList = new RoundSummary.SumInfo_ClassList
				{
					class_ds = d,
					scientists = s,
					chaos_insurgents = ci,
					mtf_and_guards = mtf,
					scps_except_zombies = scps.Where(x => x.Role != RoleType.Scp0492).Count() + scp035.Count,
					zombies = scps.Where(x => x.Role == RoleType.Scp0492).Count(),
					warhead_kills = ev.ClassList.warhead_kills,
					time = ev.ClassList.time
				};
				ev.ClassList = cList;
				if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive/* && !CiAlive*/)
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