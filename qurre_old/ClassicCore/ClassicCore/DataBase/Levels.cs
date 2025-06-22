using ClassicCore.Addons;
using ClassicCore.BetterHints;
using MEC;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace ClassicCore.DataBase
{
	internal class Levels
	{
		internal static Levels Static { get; private set; }
		internal Levels()
		{
			Static = this;

			CommandsSystem.RegisterConsole("xp", ConsoleXP);
			CommandsSystem.RegisterConsole("lvl", ConsoleXP);
			CommandsSystem.RegisterConsole("level", ConsoleXP);

			CommandsSystem.RegisterConsole("pay", ConsolePay);
			CommandsSystem.RegisterConsole("пей", ConsolePay);
			CommandsSystem.RegisterConsole("пэй", ConsolePay);
		}
		private readonly Regex regexSmartSiteReplacer = new(@"#fydne");
		internal List<Help> KillHelp = new();
		internal Dictionary<string, int> MoneyUps = new();
		internal void Refresh()
		{
			KillHelp.Clear();
			MoneyUps.Clear();
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (ev.Target == null || ev.Attacker == null) return;
			if (ev.FriendlyFire && !Server.FriendlyFire) return;
			if (!ev.Allowed || ev.Attacker == null || ev.Target == null || ev.Attacker == ev.Target || ev.Amount < 1) return;
			if (KillHelp.TryFind(out var dt, x => x.Attacker == ev.Attacker && x.Target == ev.Target)) dt.Damage += (int)ev.Amount;
			else KillHelp.Add(new Help { Attacker = ev.Attacker, Target = ev.Target });
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (ev.Player == null) return;
			while (KillHelp.Any(x => x.Target == ev.Player)) KillHelp.Remove(KillHelp.First(x => x.Target == ev.Player));
		}
		internal void Leave(LeaveEvent ev)
		{
			if (ev.Player == null) return;
			while (KillHelp.Any(x => x.Target == ev.Player)) KillHelp.Remove(KillHelp.First(x => x.Target == ev.Player));
		}
		public void Join(JoinEvent ev)
		{
			ev.Player.SetRank("1 уровень", "green");
			try
			{
				Timing.CallDelayed(100f, () => ev.Player.Broadcast(15, "<color=red>Если вы напишите в нике</color> <color=#9bff00>#fydne</color>,\n" +
			  "<color=#fdffbb>вы будете получать в 2 раза больше опыта</color>"));
			}
			catch { }
		}
		public void PlayerSpawn(SpawnEvent ev)
		{
			Timing.CallDelayed(1.5f, () => SetPrefix(ev.Player));
		}
		public void XpEnd(RoundEndEvent _)
		{
			foreach (Player pl in Player.List)
			{
				try
				{
					string nick = pl.Nickname.ToLower();
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						Manager.Static.Stats.AddXP(pl, 200);
						pl.Broadcast(10, "<color=#fdffbb>Вы получили <color=red>200xp</color>, т.к в вашем нике есть <color=#0089c7>#fydne</color>!</color>");
					}
				}
				catch { }
			}
		}
		private void ConsoleXP(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (!Manager.Static.Data.Users.TryGetValue(ev.Player.UserId, out var data))
			{
				ev.ReturnMessage = "Вы не найдены в списке";
				ev.Color = "red";
				return;
			}
			ev.ReturnMessage = "\n----------------------------------------------------------- \n" +
				$"XP:\n{data.xp}/{data.to}\nУровень:\n{data.lvl}\nБаланс:\n{data.money}" +
				"\n-----------------------------------------------------------";
			ev.Color = "red";
		}
		private void ConsolePay(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			ev.ReturnMessage = "Переводы доступны на Non Classic серверах";
		}
		public void XpEscape(EscapeEvent ev)
		{
			if (!ev.Allowed) return;
			if (ev.NewRole == RoleType.NtfPrivate) ev.NewRole = RoleType.NtfSergeant;
			try
			{
				{
					float moneyUp = 10;
					float LvlUp = 100;
					GetTotalMoney(ev.Player, ref moneyUp);
					GetTotalXp(ev.Player, ref LvlUp);
					ev.Player.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰</color></align>", 5, false));
					ev.Player.SendConsoleMessage($"Вы получили {LvlUp}xp & {moneyUp} монет за побег", "white");
					Manager.Static.Stats.Add(ev.Player, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
				}
				if (ev.Player.Cuffed)
				{
					Player cuffer = ev.Player.Cuffer;
					if (cuffer == null || cuffer == ev.Player) return;
					float moneyUp = 5;
					float LvlUp = 50;
					GetTotalMoney(cuffer, ref moneyUp);
					GetTotalXp(cuffer, ref LvlUp);
					cuffer.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰</color></align>", 5, false));
					cuffer.SendConsoleMessage($"Вы получили {LvlUp}xp & {moneyUp} монет за помощь в побеге", "white");
					Manager.Static.Stats.Add(cuffer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
				}
			}
			catch { }
		}
		public void XpDeath(DeadEvent ev)
		{
			try
			{
				if (ev.Target == null || string.IsNullOrEmpty(ev.Target.UserId)) return;
				if (ev.Killer == null || string.IsNullOrEmpty(ev.Killer.UserId)) return;
				try { SetPrefix(ev.Target); } catch { }
				Player Killer = ev.Killer;
				var killHelp = KillHelp;
				if (ev.DamageType == DamageTypes.Pocket)
				{
					var list = Player.List.Where(x => x.Role == RoleType.Scp106).ToList();
					if (list.Count != 0) Killer = list.FirstOrDefault();
				}
				if (Killer == ev.Target) return;
				try { GetKillXp(ev.Target, Killer); } catch { }
				foreach (Help help in killHelp.Where(x => x.Target == ev.Target && x.Attacker != Killer)) try { GetHelpXp(ev.Target, help.Attacker); } catch { }
			}
			catch { }
		}
		private void GetKillXp(Player target, Player killer)
		{
			if (killer.Id == target.Id) return;
			string targetname = target.Nickname;
			float moneyUp = 0;
			float LvlUp = 0;
			Team KillerTeam = killer.Team;
			Team targetTeam = target.Team;
			if (KillerTeam == Team.RIP) KillerTeam = ClassicCore.Modules.Cache.Roles[killer.Id].GetTeam();
			if (targetTeam == Team.RIP) targetTeam = ClassicCore.Modules.Cache.Roles[target.Id].GetTeam();
			switch (KillerTeam)
			{
				case Team.CHI:
					{
						moneyUp += 5;
						switch (targetTeam)
						{
							case Team.MTF or Team.TUT: LvlUp += 50; break;
							case Team.RSC: LvlUp += 25; break;
							case Team.SCP: LvlUp += 250; break;
							default: break;
						}
						break;
					}
				case Team.MTF:
					{
						moneyUp += 5;
						switch (targetTeam)
						{
							case Team.CHI or Team.TUT: LvlUp += 50; break;
							case Team.CDP: LvlUp += 25; break;
							case Team.SCP: LvlUp += 250; break;
							default: break;
						}
						break;
					}
				case Team.TUT:
					{
						moneyUp += 3;
						switch (targetTeam)
						{
							case Team.CDP or Team.RSC: LvlUp += 25; break;
							case Team.MTF or Team.CHI: LvlUp += 50; break;
							case Team.SCP: LvlUp += 10; break;
							default: break;
						}
						break;
					}
				case Team.RSC:
					{
						moneyUp += 7;
						switch (targetTeam)
						{
							case Team.CDP: LvlUp += 25; break;
							case Team.CHI or Team.TUT: LvlUp += 100; break;
							case Team.SCP: LvlUp += 500; break;
							default: break;
						}
						break;
					}
				case Team.CDP:
					{
						moneyUp += 7;
						switch (targetTeam)
						{
							case Team.RSC: LvlUp += 25; break;
							case Team.MTF or Team.TUT: LvlUp += 100; break;
							case Team.SCP: LvlUp += 500; break;
							default: break;
						}
						break;
					}
				case Team.SCP:
					{
						LvlUp += 25;
						break;
					}
			}
			GetTotalMoney(killer, ref moneyUp);
			GetTotalXp(killer, ref LvlUp);
			killer.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰", 5, false));
			killer.SendConsoleMessage($"Вы получили {LvlUp}xp & {moneyUp} монет за убийство {targetname}", "white");
			Manager.Static.Stats.Add(killer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
		}
		private void GetHelpXp(Player target, Player killer)
		{
			string targetname = target.Nickname;
			float moneyUp = 0;
			float LvlUp = 0;
			Team KillerTeam = killer.Team;
			Team targetTeam = target.Team;
			if (KillerTeam == Team.RIP) KillerTeam = ClassicCore.Modules.Cache.Roles[killer.Id].GetTeam();
			if (targetTeam == Team.RIP) targetTeam = ClassicCore.Modules.Cache.Roles[target.Id].GetTeam();
			if (killer.Id == target.Id) return;
			switch (KillerTeam)
			{
				case Team.CHI:
					{
						moneyUp += 1;
						switch (targetTeam)
						{
							case Team.MTF or Team.TUT: LvlUp += 5; break;
							case Team.RSC: LvlUp += 2; break;
							case Team.SCP: LvlUp += 25; break;
							default: break;
						}
						break;
					}
				case Team.MTF:
					{
						moneyUp += 1;
						switch (targetTeam)
						{
							case Team.CHI or Team.TUT: LvlUp += 5; break;
							case Team.CDP: LvlUp += 2; break;
							case Team.SCP: LvlUp += 25; break;
							default: break;
						}
						break;
					}
				case Team.TUT:
					{
						switch (targetTeam)
						{
							case Team.CDP or Team.RSC: LvlUp += 2; break;
							case Team.MTF or Team.CHI: LvlUp += 5; break;
							case Team.SCP: LvlUp += 1; break;
							default: break;
						}
						break;
					}
				case Team.RSC:
					{
						moneyUp += 2;
						switch (targetTeam)
						{
							case Team.CDP: LvlUp += 2; break;
							case Team.CHI or Team.TUT: LvlUp += 10; break;
							case Team.SCP: LvlUp += 50; break;
							default: break;
						}
						break;
					}
				case Team.CDP:
					{
						moneyUp += 2;
						switch (targetTeam)
						{
							case Team.RSC: LvlUp += 2; break;
							case Team.MTF or Team.TUT: LvlUp += 10; break;
							case Team.SCP: LvlUp += 50; break;
							default: break;
						}
						break;
					}
				case Team.SCP:
					{
						LvlUp += 3;
						break;
					}
			}
			GetTotalMoney(killer, ref moneyUp);
			GetTotalXp(killer, ref LvlUp);
			killer.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰", 5, false));
			killer.SendConsoleMessage($"Вы получили {LvlUp}xp & {moneyUp} монет за помощь в убийстве {targetname}", "white");
			Manager.Static.Stats.Add(killer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
		}
		internal static void SetPrefix(Player player)
		{
			string prefix = "";
			string clan = "";
			string prime = "";
			string role = "";
			string srole = "";
			string color = "red";
			Manager.Static.Data.Users.TryGetValue(player.UserId, out var imain);
			Manager.Static.Data.Roles.TryGetValue(player.UserId, out var roles);
			int lvl = imain.lvl;
			try
			{
				color = lvl switch
				{
					10 or (>= 10 and < 10) or (>= 100 and < 100) => "tomato",
					0 or 1 => "green",
					2 or (>= 20 and < 30) or (>= 200 and < 300) => "crimson",
					3 or (>= 30 and < 40) or (>= 300 and < 400) => "cyan",
					4 or (>= 40 and < 50) or (>= 400 and < 500) => "deep_pink",
					5 or (>= 50 and < 60) or (>= 500 and < 600) => "yellow",
					6 or (>= 60 and < 70) or (>= 600 and < 700) => "orange",
					7 or (>= 70 and < 80) or (>= 700 and < 800) => "lime",
					8 or (>= 80 and < 90) or (>= 800 and < 900) => "pumpkin",
					9 or (>= 90 and < 100) or (>= 900 and < 1000) => "red",
					_ => "red",
				};
			}
			catch { }
			try
			{
				if (imain.clan != "" && !imain.anonym && imain.id != 1)
				{
					clan = $"{imain.clan} | ";
				}
				if (roles.Prime)
				{
					prime = " | Прайм";
				}
				if (roles.Rainbow && imain.prefix != "")
				{
					prefix = $" | {imain.prefix}";
				}
				if (!imain.anonym)
				{
					string warns = "";
					switch (imain.warnings)
					{
						case 0: break;
						case 1: warns = " | 1 пред"; break;
						case 2: warns = " | 2 преда"; break;
						case 3: warns = " | 3 преда"; break;
						default: warns = " | уволен"; break;
					}
					if (imain.trainee)
					{
						color = "lime";
						role = $" | Стажер{warns}";
					}
					if (imain.helper)
					{
						color = "aqua";
						role = $" | Хелпер{warns}";
					}
					if (imain.mainhelper)
					{
						color = "cyan";
						role = $" | Главный хелпер{warns}";
					}
					if (imain.admin)
					{
						color = "yellow";
						role = $" | Админ{warns}";
					}
					if (imain.mainadmin)
					{
						color = "red";
						role = $" | Главный Админ{warns}";
					}
					if (imain.owner && imain.id != 1)
					{
						color = "pumpkin";
						role = " | Контроль Администрации";
					}
					if (imain.selection)
					{
						color = "pumpkin";
						role += " | Набор Администрации";
					}
					if (Manager.Static.Data.Donates.ContainsKey(player.UserId))
					{
						color = "lime";
						role += " | Донатер";
					}
					if (roles.Mage)
					{
						role += " | Маг";
						color = "mint";
					}
					if (roles.Sage)
					{
						role += " | Мудрец";
						color = "crimson";
					}
					if (roles.Star)
					{
						role += " | Звездочка";
						color = "magenta";
					}
					if (roles.Priest)
					{
						role += " | Священник";
						color = "pink";
					}
					if (Modules.CustomDonates.ThisYt(imain))
					{
						color = "red";
						role += $" | YouTube";
					}
				}
				if (imain.id == 1073)
				{
					color = "lime";
					role += " | Архитектор Комплекса";
				}
				if (Modules.CustomDonates.TryGetPlayerListPrefix(imain.id, out var customPref))
				{
					color = customPref.Color;
					role += $" | {customPref.Name}";
				}
			}
			catch { }
			try
			{
				if (imain.found && !imain.anonym) player.SetRank($"{clan}{srole}{lvl} уровень{prime}{role}{prefix}".Trim(), color);
				else player.SetRank($"{clan}{srole}{lvl} уровень{prime}{prefix}".Trim(), color);
			}
			catch { }
		}
		public void GetTotalXp(Player pl, ref float xp)
		{
			float UpDegree = 1;
			if (regexSmartSiteReplacer.Matches(pl?.Nickname.ToLower()).Count > 0) UpDegree++;
			if (Manager.Static.Data.Users.TryGetValue(pl.UserId, out var user))
			{
				if (Manager.Static.Data.Roles.TryGetValue(pl.UserId, out var roles) && roles.Prime) UpDegree++;
				if (Modules.Data.Clans.TryGetValue(user.clan.ToUpper(), out var clan))
				{
					foreach (int boost in clan.Boosts)
					{
						if (boost == 1) UpDegree++;
						else if (boost == 2) UpDegree += 2;
					}
				}
			}
			xp *= UpDegree;
		}
		public void GetTotalMoney(Player pl, ref float money)
		{
			float UpDegree = 1;
			if (regexSmartSiteReplacer.Matches(pl?.Nickname.ToLower()).Count > 0) UpDegree++;
			money *= UpDegree;
			if (!MoneyUps.ContainsKey(pl.UserId)) MoneyUps.Add(pl.UserId, (int)money);
			else MoneyUps[pl.UserId] += (int)money;
			if (MoneyUps[pl.UserId] >= 100)
			{
				if (MoneyUps[pl.UserId] - (int)money >= 100) money = 0;
				else money = 100 - MoneyUps[pl.UserId];
			}
			if (money < 0) money = 0;
		}
		[Serializable]
		public class Help
		{
			public Player Attacker;
			public Player Target;
			public int Damage = 0;
		}
	}
}