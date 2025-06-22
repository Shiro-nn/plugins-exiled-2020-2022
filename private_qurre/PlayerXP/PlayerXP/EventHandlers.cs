using System.Collections.Generic;
using MEC;
using System.Linq;
using System.Text.RegularExpressions;
using Qurre.API.Events;
using Qurre.API;
namespace PlayerXP
{
	public class EventHandlers
	{
		public static Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		private Regex RegexSmartSiteReplacer => new Regex(@"#" + Configs.Pn);
		public void OnRoundStart() => Configs.Reload();
		public void OnWaitingForPlayers()
		{
			Stats.Clear();
		}
		public void OnRoundEnd(RoundEndEvent _) { try { foreach (Stats stats in Stats.Values) Methods.SaveStats(stats); } catch { } }

		public void OnPlayerJoin(JoinEvent ev)
		{
			Timing.CallDelayed(100f, () => ev.Player.Broadcast(15, Configs.Jm));
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server") return;
			if (!Stats.ContainsKey(ev.Player.UserId)) Stats.Add(ev.Player.UserId, Methods.LoadStats(ev.Player.UserId));
			Timing.CallDelayed(0.5f, () => SetPrefix(ev.Player));
		}
		public void OnPlayerSpawn(RoleChangeEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server") return;
			if (!Stats.ContainsKey(ev.Player.UserId)) Stats.Add(ev.Player.UserId, Methods.LoadStats(ev.Player.UserId));
			Timing.CallDelayed(0.5f, () => SetPrefix(ev.Player));
		}
		public void AddXP(Player player)
		{
			Stats[player.UserId].to = Stats[player.UserId].lvl * 250 + 750;
			if (Stats[player.UserId].xp >= Stats[player.UserId].to)
			{
				Stats[player.UserId].xp -= Stats[player.UserId].to;
				Stats[player.UserId].lvl++;
				Stats[player.UserId].to = Stats[player.UserId].lvl * 250 + 750;
				player.Broadcast(10, Configs.Lvlup.Replace("%lvl%", $"{Stats[player.UserId].lvl}").Replace("%to.xp%", ((Stats[player.UserId].to) - Stats[player.UserId].xp).ToString()));
				Methods.SaveStats(Stats[player.UserId]);

				SetPrefix(player);
			}
		}
		public void SetPrefix(Player player)
		{
			string color = "green";
			var imain = Stats[player.UserId];
			int lvl = imain.lvl;
			string prefix = lvl.Prefix();
			string pref = $"{lvl} {Configs.Lvl}{prefix}";
			try
			{
				if (lvl == 1) color = "green";
				else if (lvl == 2) color = "crimson";
				else if (lvl == 3) color = "cyan";
				else if (lvl == 4) color = "deep_pink";
				else if (lvl == 5) color = "yellow";
				else if (lvl == 6) color = "orange";
				else if (lvl == 7) color = "lime";
				else if (lvl == 8) color = "pumpkin";
				else if (lvl == 9) color = "red";
				else if (lvl >= 10 && 20 >= lvl) color = "green";
				else if (lvl >= 20 && 30 >= lvl) color = "crimson";
				else if (lvl >= 30 && 40 >= lvl) color = "cyan";
				else if (lvl >= 40 && 50 >= lvl) color = "deep_pink";
				else if (lvl >= 50 && 60 >= lvl) color = "yellow";
				else if (lvl >= 60 && 70 >= lvl) color = "orange";
				else if (lvl >= 70 && 80 >= lvl) color = "lime";
				else if (lvl >= 80 && 90 >= lvl) color = "pumpkin";
				else if (lvl >= 90 && 100 >= lvl) color = "red";
				else if (lvl >= 100) color = "red";
			}
			catch { }
			var group = player.GetGroup();
			if (group != null) player.SetRank($"{group.BadgeText} | {pref}", group.BadgeColor);
			else player.SetRank(pref, color);
		}
		public void OnCheckEscape(EscapeEvent ev)
		{
			if (ev.Allowed)
			{
				try
				{
					int LvlUp = 100;
					if (RegexSmartSiteReplacer.Matches(ev.Player?.Nickname.ToLower()).Count > 0) LvlUp *= 2;
					LvlUp *= Configs.Cf;
					ev.Player.Broadcast(10, Configs.Eb.Replace("%xp%", $"{LvlUp}"));
					Stats[ev.Player.UserId].xp += LvlUp;
					AddXP(ev.Player);
				}
				catch { }
			}
		}

		public void OnPlayerDeath(DiesEvent ev)
		{
			try
			{
				Player target = ev.Target;
				Player killer = ev.Killer;
				string targetname = ev.Target?.Nickname;
				try { SetPrefix(ev.Target); } catch { }
				if (target == null || string.IsNullOrEmpty(target.UserId)) return;
				if (killer == null || string.IsNullOrEmpty(killer.UserId)) return;
				int LvlUp = 0;
				if (killer.Id == target.Id) return;
				if (killer.Team == Team.CHI)
				{
					if (target.Team == Team.MTF)
						LvlUp += 50;
					else if (target.Team == Team.RSC)
						LvlUp += 25;
					else if (target.Team == Team.SCP)
						LvlUp += 250;
					else if (target.Team == Team.TUT)
						LvlUp += 50;
				}
				else if (killer.Team == Team.MTF)
				{
					if (target.Team == Team.CHI)
						LvlUp += 50;
					else if (target.Team == Team.CDP)
						LvlUp += 25;
					else if (target.Team == Team.SCP)
						LvlUp += 250;
					else if (target.Team == Team.TUT)
						LvlUp += 50;
				}
				else if (killer.Team == Team.TUT)
				{
					if (target.Team == Team.CDP)
						LvlUp += 25;
					else if (target.Team == Team.RSC)
						LvlUp += 25;
					else if (target.Team == Team.MTF)
						LvlUp += 50;
					else if (target.Team == Team.CHI)
						LvlUp += 50;
					else if (target.Team == Team.SCP)
						LvlUp += 10;
				}
				else if (killer.Team == Team.RSC)
				{
					if (target.Team == Team.CDP)
						LvlUp += 25;
					else if (target.Team == Team.CHI)
						LvlUp += 100;
					else if (target.Team == Team.TUT)
						LvlUp += 100;
					else if (target.Team == Team.SCP)
						LvlUp += 500;
				}
				else if (killer.Team == Team.CDP)
				{
					if (target.Team == Team.RSC)
						LvlUp += 25;
					else if (target.Team == Team.MTF)
						LvlUp += 100;
					else if (target.Team == Team.TUT)
						LvlUp += 100;
					else if (target.Team == Team.SCP)
						LvlUp += 500;
				}
				else if (killer.Team == Team.SCP)
				{
					LvlUp += 25;
				}
				string nick = ev.Killer?.Nickname.ToLower();
				MatchCollection matches = RegexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0) LvlUp *= 2;
				LvlUp *= Configs.Cf;
				killer.Broadcast(10, Configs.Kb.Replace("%xp%", $"{LvlUp}").Replace("%player%", $"{targetname}"));
				Stats[killer.UserId].xp += LvlUp;
				AddXP(killer);
			}
			catch { }
		}
		public void OnPocketDimensionDie(PocketFailEscapeEvent ev)
		{
			try
			{
				if (ev.Allowed)
				{
					var list = Player.List.Where(x => x.Role == RoleType.Scp106).ToList();
					if (list.Count == 0) return;
					Player scp106 = list.FirstOrDefault();
					int LvlUp = 25;
					if (RegexSmartSiteReplacer.Matches(scp106?.Nickname.ToLower()).Count > 0) LvlUp *= 2;
					LvlUp *= Configs.Cf;
					scp106.Broadcast(10, Configs.Kb.Replace("%xp%", $"{LvlUp}").Replace("%player%", $"{ev.Player?.Nickname}"));
					Stats[scp106.UserId].xp += LvlUp;
					AddXP(scp106);
				}
			}
			catch { }
		}
		public void Console(SendingConsoleEvent ev)
		{
			if (ev.Name == "xp" || ev.Name == "lvl")
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n" + Stats[ev.Player.UserId].xp + "/" + Stats[ev.Player.UserId].to + "\nlvl:\n" + Stats[ev.Player.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
		}
	}
}