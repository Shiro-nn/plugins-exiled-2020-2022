using System.Collections.Generic;
using MEC;
using System.Linq;
using System.Text.RegularExpressions;
using Qurre.API;
using Qurre.Events;
using Qurre.API.Attributes;
using Qurre.Events.Structs;
using Qurre.API.Objects;
using PlayerRoles;

namespace PlayerXP
{
	static class Events
	{
		static internal Dictionary<string, Stats> Stats = new();

		static Regex RegexSmartSiteReplacer = new("#" + Cfg.Pn);

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting()
		{
			Cfg.Reload();
			Stats.Clear();
		}

		[EventMethod(RoundEvents.End)]
		static void RoundEnd()
		{
			try
			{
				foreach (Stats stats in Stats.Values)
				{
					try { Methods.SaveStats(stats); }
					catch { }
				}
			}
			catch { }
		}

		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
			Timing.CallDelayed(100f, () => ev.Player.Client.Broadcast(15, Cfg.Jm));

			if (string.IsNullOrEmpty(ev.Player.UserInfomation.UserId) ||
				ev.Player.IsHost || ev.Player.UserInfomation.Nickname == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.UserInfomation.UserId))
				Stats.Add(ev.Player.UserInfomation.UserId, Methods.LoadStats(ev.Player.UserInfomation.UserId));

			Timing.CallDelayed(0.5f, () => SetPrefix(ev.Player));
		}

		[EventMethod(PlayerEvents.ChangeRole)]
		static void Spawn(ChangeRoleEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserInfomation.UserId) ||
				ev.Player.IsHost || ev.Player.UserInfomation.Nickname == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.UserInfomation.UserId))
				Stats.Add(ev.Player.UserInfomation.UserId, Methods.LoadStats(ev.Player.UserInfomation.UserId));

			Timing.CallDelayed(0.5f, () => SetPrefix(ev.Player));
		}

		static void AddXP(Player player)
		{
			Stats[player.UserInfomation.UserId].to = Stats[player.UserInfomation.UserId].lvl * 250 + 750;

			if (Stats[player.UserInfomation.UserId].xp >= Stats[player.UserInfomation.UserId].to)
			{
				Stats[player.UserInfomation.UserId].xp -= Stats[player.UserInfomation.UserId].to;
				Stats[player.UserInfomation.UserId].lvl++;
				Stats[player.UserInfomation.UserId].to = Stats[player.UserInfomation.UserId].lvl * 250 + 750;
				Methods.SaveStats(Stats[player.UserInfomation.UserId]);

				player.Client.Broadcast(10,
					Cfg.Lvlup
					.Replace("%lvl%", $"{Stats[player.UserInfomation.UserId].lvl}")
					.Replace("%to.xp%", ((Stats[player.UserInfomation.UserId].to) - Stats[player.UserInfomation.UserId].xp)
					.ToString()
				));

				SetPrefix(player);
			}
		}
		static internal void SetPrefix(Player player)
		{
			if (!Stats.TryGetValue(player.UserInfomation.UserId, out Stats imain))
				return;

			int lvl = imain.lvl;
			string prefix = lvl.Prefix();
			string pref = $"{lvl} {Cfg.Lvl}{prefix}";

			if (player.Administrative.RoleName != null && player.Administrative.RoleName.Contains(pref))
				return;

			string color = lvl switch
			{
				1 => "green",
				2 => "crimson",
				3 => "cyan",
				4 => "deep_pink",
				5 => "yellow",
				6 => "orange",
				7 => "lime",
				8 => "pumpkin",
				9 => "red",

				>= 90 => "red",
				>= 80 => "pumpkin",
				>= 70 => "lime",
				>= 60 => "orange",
				>= 50 => "yellow",
				>= 40 => "deep_pink",
				>= 30 => "cyan",
				>= 20 => "crimson",
				>= 10 => "green",

				_ => "green",
			};

			if (!imain.anonymous && player.Adminsearch())
				player.SetRank($"{player.Administrative.Group.BadgeText} | {pref}", player.Administrative.Group.BadgeColor);
			else player.SetRank(pref, color);
		}

		[EventMethod(PlayerEvents.Escape, int.MinValue)]
		static void XpEscape(EscapeEvent ev)
		{
			if (!ev.Allowed)
				return;

			if (ev.Player is null)
				return;

			int LvlUp = 100;

			if (RegexSmartSiteReplacer.Matches(ev.Player.UserInfomation.Nickname.ToLower()).Count > 0)
				LvlUp *= 2;

			LvlUp *= Cfg.Cf;

			ev.Player.Client.Broadcast(10, Cfg.Eb.Replace("%xp%", $"{LvlUp}"), true);

			Stats[ev.Player.UserInfomation.UserId].xp += LvlUp;
			AddXP(ev.Player);
		}

		[EventMethod(PlayerEvents.Dies)]
		static void Dies(DiesEvent ev)
		{
			if (ev.Target == null || string.IsNullOrEmpty(ev.Target.UserInfomation.UserId)) return;
			if (ev.Attacker == null || string.IsNullOrEmpty(ev.Attacker.UserInfomation.UserId)) return;
			try { SetPrefix(ev.Target); } catch { }

			Player Killer = ev.Attacker;
			if (ev.DamageType == DamageTypes.Pocket)
			{
				var list = Player.List.Where(x => x.RoleInfomation.Role == RoleTypeId.Scp106).ToList();
				if (list.Count != 0) Killer = list.FirstOrDefault();
			}

			if (Killer == ev.Target) return;
			try { GetKillXp(ev.Target, Killer); } catch { }

			static void GetKillXp(Player target, Player killer)
			{
				string targetname = target.UserInfomation.Nickname;
				float LvlUp = 0;
				Team KillerTeam = killer.RoleInfomation.Team;
				Team targetTeam = target.RoleInfomation.Team;

				if (killer.UserInfomation.Id == target.UserInfomation.Id)
					return;

				switch (KillerTeam)
				{
					case Team.ChaosInsurgency:
						{
							switch (targetTeam)
							{
								case Team.FoundationForces or Team.OtherAlive: LvlUp += 50; break;
								case Team.Scientists: LvlUp += 25; break;
								case Team.SCPs: LvlUp += 250; break;
								default: break;
							}
							break;
						}
					case Team.FoundationForces:
						{
							switch (targetTeam)
							{
								case Team.ChaosInsurgency or Team.OtherAlive: LvlUp += 50; break;
								case Team.ClassD: LvlUp += 25; break;
								case Team.SCPs: LvlUp += 250; break;
								default: break;
							}
							break;
						}
					case Team.OtherAlive:
						{
							switch (targetTeam)
							{
								case Team.ClassD or Team.Scientists: LvlUp += 25; break;
								case Team.FoundationForces or Team.ChaosInsurgency: LvlUp += 50; break;
								case Team.SCPs: LvlUp += 10; break;
								default: break;
							}
							break;
						}
					case Team.Scientists:
						{
							switch (targetTeam)
							{
								case Team.ClassD: LvlUp += 25; break;
								case Team.ChaosInsurgency or Team.OtherAlive: LvlUp += 100; break;
								case Team.SCPs: LvlUp += 500; break;
								default: break;
							}
							break;
						}
					case Team.ClassD:
						{
							switch (targetTeam)
							{
								case Team.Scientists: LvlUp += 25; break;
								case Team.FoundationForces or Team.OtherAlive: LvlUp += 100; break;
								case Team.SCPs: LvlUp += 500; break;
								default: break;
							}
							break;
						}
					case Team.SCPs:
						{
							LvlUp += 25;
							break;
						}
				}

				if (RegexSmartSiteReplacer.Matches(killer.UserInfomation.Nickname.ToLower()).Count > 0)
					LvlUp *= 2;

				LvlUp *= Cfg.Cf;

				killer.Client.Broadcast(10, Cfg.Kb.Replace("%xp%", $"{LvlUp}").Replace("%player%", $"{targetname}"), true);
				Stats[killer.UserInfomation.UserId].xp += (int)LvlUp;
				AddXP(killer);
			}
		}

		[EventMethod(ServerEvents.GameConsoleCommand)]
		static void ConsoleXP(GameConsoleCommandEvent ev)
		{
			if (ev.Name == "xp" || ev.Name == "lvl")
			{
				ev.Reply = "\n----------------------------------------------------------- " +
					"\nXP:\n" + Stats[ev.Player.UserInfomation.UserId].xp + "/" + Stats[ev.Player.UserInfomation.UserId].to +
					"\nlvl:\n" + Stats[ev.Player.UserInfomation.UserId].lvl +
					"\n -----------------------------------------------------------";
				ev.Color = "red";
				ev.Allowed = false;
			}
		}
	}
}