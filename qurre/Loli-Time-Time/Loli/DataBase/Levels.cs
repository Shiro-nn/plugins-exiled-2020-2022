using Loli.Addons;
using Loli.BetterHints;
using Loli.DataBase.Modules;
using MEC;
using Newtonsoft.Json;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Loli.DataBase
{
	static class Levels
	{
		static Levels()
		{
			CommandsSystem.RegisterConsole("money", ConsoleXP);
			CommandsSystem.RegisterConsole("мани", ConsoleXP);
			CommandsSystem.RegisterConsole("баланс", ConsoleXP);
			CommandsSystem.RegisterConsole("xp", ConsoleXP);
			CommandsSystem.RegisterConsole("lvl", ConsoleXP);
			CommandsSystem.RegisterConsole("level", ConsoleXP);

			CommandsSystem.RegisterConsole("pay", ConsolePay);
			CommandsSystem.RegisterConsole("пей", ConsolePay);
			CommandsSystem.RegisterConsole("пэй", ConsolePay);
		}

		static readonly Regex regexSmartSiteReplacer = new(@"#fydne");
		static internal List<Help> KillHelp = new();
		static internal Dictionary<string, int> MoneyUps = new();

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh()
		{
			KillHelp.Clear();
			MoneyUps.Clear();
		}

		[EventMethod(PlayerEvents.Attack)]
		static void Damage(AttackEvent ev)
		{
			if (ev.Target == null || ev.Attacker == null) return;
			if (ev.FriendlyFire && !Server.FriendlyFire) return;
			if (!ev.Allowed || ev.Attacker == null || ev.Target == null || ev.Attacker == ev.Target || ev.Damage < 1) return;
			if (KillHelp.TryFind(out var dt, x => x.Attacker == ev.Attacker && x.Target == ev.Target)) dt.Damage += (int)ev.Damage;
			else KillHelp.Add(new Help { Attacker = ev.Attacker, Target = ev.Target });
		}

		[EventMethod(PlayerEvents.Spawn)]
		static void Spawn(SpawnEvent ev)
		{
			if (ev.Player == null) return;
			while (KillHelp.Any(x => x.Target == ev.Player)) KillHelp.Remove(KillHelp.First(x => x.Target == ev.Player));
		}

		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (ev.Player == null) return;
			while (KillHelp.Any(x => x.Target == ev.Player)) KillHelp.Remove(KillHelp.First(x => x.Target == ev.Player));
		}

		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
			ev.Player.SetRank("1 уровень", "green");
			try
			{
				Timing.CallDelayed(100f, () => ev.Player.Client.Broadcast(15, "<color=red>Если вы напишите в нике</color> <color=#9bff00>#fydne</color>,\n" +
			  "<color=#fdffbb>вы будете получать в 2 раза больше опыта & монет</color>"));
			}
			catch { }
		}

		[EventMethod(PlayerEvents.Spawn)]
		static void PlayerSpawn(SpawnEvent ev)
		{
			Timing.CallDelayed(1.5f, () => SetPrefix(ev.Player));
		}


		[EventMethod(RoundEvents.End)]
		static void XpEnd()
		{
			foreach (Player pl in Player.List)
			{
				try
				{
					string nick = pl.UserInfomation.Nickname.ToLower();
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						Stats.AddXP(pl, 200);
						pl.Client.Broadcast(10, "<color=#fdffbb>Вы получили <color=red>200xp</color>, т.к в вашем нике есть <color=#0089c7>#fydne</color>!</color>");
					}
				}
				catch { }
			}
		}

		static void ConsoleXP(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (!Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var data))
			{
				ev.Reply = "Вы не найдены в списке";
				ev.Color = "red";
				return;
			}
			ev.Reply = "\n----------------------------------------------------------- \n" +
						$"XP:\n{data.xp}/{data.to}\nУровень:\n{data.lvl}\nБаланс:\n{data.money}" +
						"\n-----------------------------------------------------------";
			ev.Color = "red";
		}

		static DateTime LastPay = DateTime.Now;
		static void ConsolePay(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (ev.Args.Count() < 2)
			{
				ev.Reply = $"Команда введена неверно\nПример: {ev.Name} 10 hmm";
				ev.Color = "red";
				return;
			}
			if (!int.TryParse(ev.Args[0], out int result))
			{
				ev.Reply = "Введите корректное кол-во монет";
				ev.Color = "red";
				return;
			}
			string search = string.Join(" ", ev.Args.Skip(1));
			Player pl = search.GetPlayer();
			if (pl == null)
			{
				ev.Reply = $"Игрок \"{search}\" не найден";
				ev.Color = "red";
				return;
			}
			if (pl.UserInfomation.UserId == ev.Player.UserInfomation.UserId)
			{
				ev.Reply = "Нельзя передать монетки самому себе";
				ev.Color = "red";
				return;
			}
			if ((DateTime.Now - LastPay).TotalSeconds < 0.5)
			{
				ev.Reply = "Рейт-лимит, попробуйте снова";
				ev.Color = "red";
				return;
			}
			LastPay = DateTime.Now;
			{
				Core.Socket.Emit("database.get.stats", new object[] { ev.Player.UserInfomation.UserId.Replace("@steam", "").Replace("@discord", ""),
							ev.Player.UserInfomation.UserId.Contains("discord"), ev.Player.UserInfomation.UserId+"+updating"});
				string uid = "";
				uid = Core.Socket.On("database.get.stats", obj => Timing.CallDelayed(0.5f, () => DoUpdate(obj)));
				void DoUpdate(object[] obj)
				{
					string userid = obj[1].ToString();
					if (userid != ev.Player.UserInfomation.UserId + "+updating") return;
					var sender = userid.Replace("+updating", "").GetPlayer();
					if (sender is null) return;
					Core.Socket.Off(uid);
					SocketStatsData json = JsonConvert.DeserializeObject<SocketStatsData>(obj[0].ToString());
					if (json.money >= result)
					{
						Stats.AddMoney(ev.Player, 0 - result);
						Stats.AddMoney(pl, result);
						ev.Player.Hint(new(16, 6, $"<align=right><color=#fdffbb>-{result} 💰</color></align>", 5, false));
						pl.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{result} 💰</color></align>", 5, false));
						ev.Player.Client.SendConsole($"Вы успешно передали {result} монет игроку {pl.UserInfomation.Nickname}", "white");
						pl.Client.SendConsole($"{ev.Player.UserInfomation.Nickname} передал вам {result} монет", "white");
						return;
					}
					ev.Player.Client.SendConsole($"Не хватает монет({json.money}/{result})", "red");
				}
			}
			ev.Reply = "Запрос на перевод создан";
		}

		[EventMethod(PlayerEvents.Escape)]
		static void XpEscape(EscapeEvent ev)
		{
			if (!ev.Allowed) return;

			if (ev.Role == RoleTypeId.NtfPrivate)
				ev.Role = RoleTypeId.NtfSergeant;

			try
			{
				{
					float moneyUp = 10;
					float LvlUp = 100;
					GetTotalMoney(ev.Player, ref moneyUp);
					GetTotalXp(ev.Player, ref LvlUp);
					ev.Player.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰</color></align>", 5, false));
					ev.Player.Client.SendConsole($"Вы получили {LvlUp}xp & {moneyUp} монет за побег", "white");
					Stats.Add(ev.Player, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
				}
				if (ev.Player.GamePlay.Cuffed)
				{
					Player cuffer = ev.Player.GamePlay.Cuffer;
					if (cuffer == null || cuffer == ev.Player) return;
					float moneyUp = 5;
					float LvlUp = 50;
					GetTotalMoney(cuffer, ref moneyUp);
					GetTotalXp(cuffer, ref LvlUp);
					cuffer.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰</color></align>", 5, false));
					cuffer.Client.SendConsole($"Вы получили {LvlUp}xp & {moneyUp} монет за помощь в побеге", "white");
					Stats.Add(cuffer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
				}
			}
			catch { }
		}

		[EventMethod(PlayerEvents.Dead)]
		static void XpDeath(DeadEvent ev)
		{
			try
			{
				if (ev.Target == null || string.IsNullOrEmpty(ev.Target.UserInfomation.UserId)) return;
				if (ev.Attacker == null || string.IsNullOrEmpty(ev.Attacker.UserInfomation.UserId)) return;
				try { SetPrefix(ev.Target); } catch { }
				Player Killer = ev.Attacker;
				var killHelp = KillHelp;
				if (ev.DamageType == DamageTypes.Pocket)
				{
					var list = Player.List.Where(x => x.RoleInfomation.Role == RoleTypeId.Scp106).ToList();
					if (list.Count != 0) Killer = list.FirstOrDefault();
				}
				if (Killer == ev.Target) return;
				try { GetKillXp(ev.Target, Killer); } catch { }
				foreach (Help help in killHelp.Where(x => x.Target == ev.Target && x.Attacker != Killer)) try { GetHelpXp(ev.Target, help.Attacker); } catch { }
			}
			catch { }
		}

		static void GetKillXp(Player target, Player killer)
		{
			string targetname = target.UserInfomation.Nickname;
			float moneyUp = 0;
			float LvlUp = 0;
			Team KillerTeam = killer.GetTeam();
			Team targetTeam = target.GetTeam();
			if (KillerTeam == Team.Dead) KillerTeam = Caches.Role[killer.UserInfomation.Id].GetTeam();
			if (targetTeam == Team.Dead) targetTeam = Caches.Role[target.UserInfomation.Id].GetTeam();
			if (killer.UserInfomation.Id == target.UserInfomation.Id) return;
			switch (KillerTeam)
			{
				case Team.ChaosInsurgency:
					{
						moneyUp += 5;
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
						moneyUp += 5;
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
						moneyUp += 3;
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
						moneyUp += 7;
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
						moneyUp += 7;
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
						moneyUp += 3;
						LvlUp += 25;
						break;
					}
			}
			GetTotalMoney(killer, ref moneyUp);
			GetTotalXp(killer, ref LvlUp);
			killer.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰", 5, false));
			killer.Client.SendConsole($"Вы получили {LvlUp}xp & {moneyUp} монет за убийство {targetname}, который был {targetTeam}", "white");
			Stats.Add(killer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
		}
		static void GetHelpXp(Player target, Player killer)
		{
			string targetname = target.UserInfomation.Nickname;
			float moneyUp = 0;
			float LvlUp = 0;
			Team KillerTeam = killer.GetTeam();
			Team targetTeam = target.GetTeam();
			if (KillerTeam == Team.Dead) KillerTeam = Caches.Role[killer.UserInfomation.Id].GetTeam();
			if (targetTeam == Team.Dead) targetTeam = Caches.Role[target.UserInfomation.Id].GetTeam();
			if (killer.UserInfomation.Id == target.UserInfomation.Id) return;
			switch (KillerTeam)
			{
				case Team.ChaosInsurgency:
					{
						moneyUp += 1;
						switch (targetTeam)
						{
							case Team.FoundationForces or Team.OtherAlive: LvlUp += 5; break;
							case Team.Scientists: LvlUp += 2; break;
							case Team.SCPs: LvlUp += 25; break;
							default: break;
						}
						break;
					}
				case Team.FoundationForces:
					{
						moneyUp += 1;
						switch (targetTeam)
						{
							case Team.ChaosInsurgency or Team.OtherAlive: LvlUp += 5; break;
							case Team.ClassD: LvlUp += 2; break;
							case Team.SCPs: LvlUp += 25; break;
							default: break;
						}
						break;
					}
				case Team.OtherAlive:
					{
						switch (targetTeam)
						{
							case Team.ClassD or Team.Scientists: LvlUp += 2; break;
							case Team.FoundationForces or Team.ChaosInsurgency: LvlUp += 5; break;
							case Team.SCPs: LvlUp += 1; break;
							default: break;
						}
						break;
					}
				case Team.Scientists:
					{
						moneyUp += 2;
						switch (targetTeam)
						{
							case Team.ClassD: LvlUp += 2; break;
							case Team.ChaosInsurgency or Team.OtherAlive: LvlUp += 10; break;
							case Team.SCPs: LvlUp += 50; break;
							default: break;
						}
						break;
					}
				case Team.ClassD:
					{
						moneyUp += 2;
						switch (targetTeam)
						{
							case Team.Scientists: LvlUp += 2; break;
							case Team.FoundationForces or Team.OtherAlive: LvlUp += 10; break;
							case Team.SCPs: LvlUp += 50; break;
							default: break;
						}
						break;
					}
				case Team.SCPs:
					{
						LvlUp += 3;
						break;
					}
			}
			GetTotalMoney(killer, ref moneyUp);
			GetTotalXp(killer, ref LvlUp);
			killer.Hint(new(16, 6, $"<align=right><color=#fdffbb>+{LvlUp}xp & {moneyUp} 💰", 5, false));
			killer.Client.SendConsole($"Вы получили {LvlUp}xp & {moneyUp} монет за помощь в убийстве {targetname}, который был {targetTeam}", "white");
			Stats.Add(killer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
		}
		internal static void SetPrefix(Player player)
		{
			string prefix = "";
			string clan = "";
			string prime = "";
			string role = "";
			string srole = "";
			string color = "red";
			Data.Users.TryGetValue(player.UserInfomation.UserId, out var imain);
			Data.Roles.TryGetValue(player.UserInfomation.UserId, out var roles);
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
						role += " | Набор Администрации";
					}
					if (Data.Donates.ContainsKey(player.UserInfomation.UserId))
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
					if (imain.id == 1385 || imain.id == 3947)
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
				if (player.Tag.Contains(Scps.God.Tag))
				{
					color = "red";
					srole = $"Бог | {srole}";
				}
				if (player.ItsScp035())
				{
					color = "red";
					srole = $"SCP 035 | {srole}";
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
		static internal void GetTotalXp(Player pl, ref float xp)
		{
			float UpDegree = 1;
			if (regexSmartSiteReplacer.Matches(pl.UserInfomation.Nickname.ToLower()).Count > 0) UpDegree++;
			if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var user))
			{
				if (Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var roles) && roles.Prime) UpDegree++;
				if (Data.Clans.TryGetValue(user.clan.ToUpper(), out var clan))
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
		static internal void GetTotalMoney(Player pl, ref float money)
		{
			float UpDegree = 1;
			if (regexSmartSiteReplacer.Matches(pl.UserInfomation.Nickname.ToLower()).Count > 0) UpDegree++;
			money *= UpDegree;
			if (!MoneyUps.ContainsKey(pl.UserInfomation.UserId)) MoneyUps.Add(pl.UserInfomation.UserId, (int)money);
			else MoneyUps[pl.UserInfomation.UserId] += (int)money;
			if (MoneyUps[pl.UserInfomation.UserId] >= 100)
			{
				if (MoneyUps[pl.UserInfomation.UserId] - (int)money >= 100) money = 0;
				else money = 100 - MoneyUps[pl.UserInfomation.UserId];
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