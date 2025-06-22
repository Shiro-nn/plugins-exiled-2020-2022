using Loli.Addons;
using Loli.Modules;
using Loli.Scps.Api;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
namespace Loli.DataBase
{
	internal class Levels
	{
		internal static Levels Static { get; private set; }
		private readonly Plugin plugin;
		internal Levels(Plugin plugin)
		{
			this.plugin = plugin;
			Static = this;
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
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (!ev.Allowed || ev.Attacker == null || ev.Target == null || ev.Attacker == ev.Target || ev.Amount < 1) return;
			if (KillHelp.Where(x => x.Attacker == ev.Attacker && x.Target == ev.Target).Count() == 0) KillHelp.Add(new Help { Attacker = ev.Attacker, Target = ev.Target });
			KillHelp.Find(x => x.Attacker == ev.Attacker && x.Target == ev.Target).Damage += (int)ev.Amount;
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (ev.Player == null) return;
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			while (KillHelp.Where(x => x.Target == ev.Player).Count() != 0) KillHelp.Remove(KillHelp.Where(x => x.Target == ev.Player).ToArray()[0]);
		}
		internal void Leave(LeaveEvent ev)
		{
			if (ev.Player == null) return;
			while (KillHelp.Where(x => x.Target == ev.Player || x.Attacker == ev.Player).Count() != 0)
				KillHelp.Remove(KillHelp.Where(x => x.Target == ev.Player || x.Attacker == ev.Player).ToArray()[0]);
		}
		public void Join(JoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server") return;
			ev.Player.SetRank($"1 уровень", "green");
			try
			{
				Timing.CallDelayed(100f, () => ev.Player.Broadcast(15, "<color=red>Если вы напишите в нике</color> <color=#9bff00>#fydne</color>,\n" +
			  "<color=#fdffbb>вы будете получать в 2 раза больше опыта & монет</color>"));
			}
			catch { }
		}
		public void PlayerSpawn(SpawnEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server") return;
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
		public void Console(SendingConsoleEvent ev)
		{
			string cmd = ev.Name;
			try
			{
				if (cmd == "money" || cmd == "мани" || cmd == "баланс" || cmd == "xp" || cmd == "lvl" || cmd == "level")
				{
					var main = Manager.Static.Data.Users[ev.Player.UserId];
					ev.Allowed = false;
					ev.ReturnMessage = $"\n----------------------------------------------------------- \n" +
						$"XP:\n{main.xp}/{main.to}\nlvl:\n{main.lvl}\nБаланс:\n{main.money}" +
						$"\n-----------------------------------------------------------";
					ev.Color = "red";
				}
				else if (cmd == "pay" || cmd == "пей" || cmd == "пэй")
				{
					ev.Allowed = false;
					if (ev.Args.Count() < 2)
					{
						ev.ReturnMessage = $"Ошибка! Пример: {ev.Name[0]} 10 hmm";
						ev.Color = "red";
						return;
					}
					if (!int.TryParse(ev.Args[0], out int result))
					{
						ev.ReturnMessage = "Введите корректное кол-во монет";
						ev.Color = "red";
						return;
					}
					Player pl = Extensions.GetPlayer(string.Join(" ", ev.Args.Skip(1)));
					if (pl == null)
					{
						ev.ReturnMessage = "Игрок не найден.";
						ev.Color = "red";
						return;
					}
					new Thread(() =>
					{
						if (!Manager.Static.Stats.Update(ev.Player, out var _main)) return;
						if (_main.money >= result)
						{
							Manager.Static.Stats.AddMoney(ev.Player, 0 - result);
							Manager.Static.Stats.AddMoney(pl, result);
							ev.Player.SendConsoleMessage($"Вы успешно передали {result} монет игроку {pl.Nickname}.", "green");
							pl.Broadcast(5, $"<size=20><color=lime>{ev.Player.Nickname} передал вам {result} монет</color></size>", true);
							return;
						}
						ev.Player.SendConsoleMessage($"Не хватает монет({_main.money}/{result}).", "red");
						return;
					}).Start();
				}
			}
			catch
			{
				if (cmd == "money" || cmd == "мани" || cmd == "баланс" || cmd == "xp" || cmd == "lvl" || cmd == "level" || cmd == "pay" || cmd == "пей" || cmd == "пэй")
				{
					ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже.";
					ev.Color = "red";
				}
			}
		}
		public void XpEscape(EscapeEvent ev)
		{
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (!ev.Allowed) return;
			if (ev.NewRole == RoleType.NtfPrivate) ev.NewRole = RoleType.NtfSergeant;
			try
			{
				{
					float moneyUp = 10;
					float LvlUp = 100;
					GetTotalMoney(ev.Player, ref moneyUp);
					GetTotalXp(ev.Player, ref LvlUp);
					ev.Player.Broadcast(10, $"<color=#fdffbb>Вы получили {LvlUp}xp & {moneyUp} монет за побег</color>", true);
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
					cuffer.Broadcast(10, $"<color=#fdffbb>Вы получили {LvlUp}xp & {moneyUp} монет за помощь в побеге</color>", true);
					Manager.Static.Stats.Add(cuffer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
				}
			}
			catch { }
		}
		public void XpDeath(DeadEvent ev)
		{
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			try
			{
				if (ev.Target == null || string.IsNullOrEmpty(ev.Target.UserId)) return;
				if (ev.Killer == null || string.IsNullOrEmpty(ev.Killer.UserId)) return;
				try { SetPrefix(ev.Target); } catch { }
				if (EventHandlers.Upgrade914.ContainsKey(ev.Target.UserId) && EventHandlers.Upgrade914[ev.Target.UserId] && ev.Target.Role == RoleType.ClassD) return;
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
			string targetname = target.Nickname;
			float moneyUp = 0;
			float LvlUp = 0;
			Team KillerTeam = killer.GetTeam();
			Team targetTeam = target.GetTeam();
			if (KillerTeam == Team.RIP) KillerTeam = EventHandlers.DRole[killer.Id].GetTeam();
			if (targetTeam == Team.RIP) targetTeam = EventHandlers.DRole[target.Id].GetTeam();
			if (killer.Id == target.Id) return;
			if (KillerTeam == Team.CHI)
			{
				moneyUp += 5;
				if (targetTeam == Team.MTF) LvlUp += 50;
				else if (targetTeam == Team.RSC) LvlUp += 25;
				else if (targetTeam == Team.SCP) LvlUp += 250;
				else if (targetTeam == Team.TUT) LvlUp += 50;
			}
			else if (KillerTeam == Team.MTF)
			{
				moneyUp += 5;
				if (targetTeam == Team.CHI) LvlUp += 50;
				else if (targetTeam == Team.CDP) LvlUp += 25;
				else if (targetTeam == Team.SCP) LvlUp += 250;
				else if (targetTeam == Team.TUT) LvlUp += 50;
			}
			else if (KillerTeam == Team.TUT)
			{
				moneyUp += 3;
				if (targetTeam == Team.CDP) LvlUp += 25;
				else if (targetTeam == Team.RSC) LvlUp += 25;
				else if (targetTeam == Team.MTF) LvlUp += 50;
				else if (targetTeam == Team.CHI) LvlUp += 50;
				else if (targetTeam == Team.SCP) LvlUp += 10;
			}
			else if (KillerTeam == Team.RSC)
			{
				moneyUp += 7;
				if (targetTeam == Team.CDP) LvlUp += 25;
				else if (targetTeam == Team.CHI) LvlUp += 100;
				else if (targetTeam == Team.TUT) LvlUp += 100;
				else if (targetTeam == Team.SCP) LvlUp += 500;
			}
			else if (KillerTeam == Team.CDP)
			{
				moneyUp += 7;
				if (targetTeam == Team.RSC) LvlUp += 25;
				else if (targetTeam == Team.MTF) LvlUp += 100;
				else if (targetTeam == Team.TUT) LvlUp += 100;
				else if (targetTeam == Team.SCP) LvlUp += 500;
			}
			else if (KillerTeam == Team.SCP)
			{
				moneyUp += 3;
				LvlUp += 25;
			}
			GetTotalMoney(killer, ref moneyUp);
			GetTotalXp(killer, ref LvlUp);
			killer.Broadcast(10, $"<color=#fdffbb>Вы получили {LvlUp}xp & {moneyUp} монет за убийство</color> <color=red>{targetname}</color>", true);
			Manager.Static.Stats.Add(killer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
		}
		private void GetHelpXp(Player target, Player killer)
		{
			string targetname = target.Nickname;
			float moneyUp = 0;
			float LvlUp = 0;
			Team KillerTeam = killer.GetTeam();
			Team targetTeam = target.GetTeam();
			if (KillerTeam == Team.RIP) KillerTeam = EventHandlers.DRole[killer.Id].GetTeam();
			if (targetTeam == Team.RIP) targetTeam = EventHandlers.DRole[target.Id].GetTeam();
			if (killer.Id == target.Id) return;
			if (KillerTeam == Team.CHI)
			{
				moneyUp += 1;
				if (targetTeam == Team.MTF) LvlUp += 5;
				else if (targetTeam == Team.RSC) LvlUp += 2;
				else if (targetTeam == Team.SCP) LvlUp += 25;
				else if (targetTeam == Team.TUT) LvlUp += 5;
			}
			else if (KillerTeam == Team.MTF)
			{
				moneyUp += 1;
				if (targetTeam == Team.CHI) LvlUp += 5;
				else if (targetTeam == Team.CDP) LvlUp += 2;
				else if (targetTeam == Team.SCP) LvlUp += 25;
				else if (targetTeam == Team.TUT) LvlUp += 5;
			}
			else if (KillerTeam == Team.TUT)
			{
				if (targetTeam == Team.CDP) LvlUp += 2;
				else if (targetTeam == Team.RSC) LvlUp += 2;
				else if (targetTeam == Team.MTF) LvlUp += 5;
				else if (targetTeam == Team.CHI) LvlUp += 5;
				else if (targetTeam == Team.SCP) LvlUp += 1;
			}
			else if (KillerTeam == Team.RSC)
			{
				moneyUp += 2;
				if (targetTeam == Team.CDP) LvlUp += 2;
				else if (targetTeam == Team.CHI) LvlUp += 10;
				else if (targetTeam == Team.TUT) LvlUp += 10;
				else if (targetTeam == Team.SCP) LvlUp += 50;
			}
			else if (KillerTeam == Team.CDP)
			{
				moneyUp += 2;
				if (targetTeam == Team.RSC) LvlUp += 2;
				else if (targetTeam == Team.MTF) LvlUp += 10;
				else if (targetTeam == Team.TUT) LvlUp += 10;
				else if (targetTeam == Team.SCP) LvlUp += 50;
			}
			else if (KillerTeam == Team.SCP) LvlUp += 3;
			GetTotalMoney(killer, ref moneyUp);
			GetTotalXp(killer, ref LvlUp);
			killer.Broadcast(10, $"<color=#fdffbb>Вы получили {LvlUp}xp & {moneyUp} монет за помощь в убийстве</color> <color=red>{targetname}</color>", true);
			Manager.Static.Stats.Add(killer, (int)Math.Round(LvlUp), (int)Math.Round(moneyUp));
		}
		internal static void SetPrefix(Player player)
		{
			string hook = "";
			try
			{
				if (player.Id == CatHook.hook_owner.Id)
				{
					hook = " | Нашел Крюк-Кошку";
				}
			}
			catch
			{ }
			string prefix = "";
			string clan = "";
			string prime = "";
			string role = "";
			string mrole = "";
			string srole = "";
			string color = "red";
			Manager.Static.Data.Users.TryGetValue(player.UserId, out var imain);
			Manager.Static.Data.Roles.TryGetValue(player.UserId, out var roles);
			int lvl = imain.lvl;
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
				else if (lvl >= 10 && 20 > lvl) color = "tomato";
				else if (lvl >= 20 && 30 > lvl) color = "crimson";
				else if (lvl >= 30 && 40 > lvl) color = "cyan";
				else if (lvl >= 40 && 50 > lvl) color = "deep_pink";
				else if (lvl >= 50 && 60 > lvl) color = "yellow";
				else if (lvl >= 60 && 70 > lvl) color = "orange";
				else if (lvl >= 70 && 80 > lvl) color = "lime";
				else if (lvl >= 80 && 90 > lvl) color = "pumpkin";
				else if (lvl >= 90 && 100 > lvl) color = "red";
				else if (lvl >= 100 && 200 > lvl) color = "tomato";
				else if (lvl >= 200 && 300 > lvl) color = "crimson";
				else if (lvl >= 300 && 400 > lvl) color = "cyan";
				else if (lvl >= 400 && 500 > lvl) color = "deep_pink";
				else if (lvl >= 500 && 600 > lvl) color = "yellow";
				else if (lvl >= 600 && 700 > lvl) color = "orange";
				else if (lvl >= 700 && 800 > lvl) color = "lime";
				else if (lvl >= 800 && 900 > lvl) color = "pumpkin";
				else if (lvl >= 900 && 1000 > lvl) color = "red";
				else if (lvl >= 1000) color = "red";
			}
			catch { }
			try
			{
				if (imain.clan != "" && !imain.anonym && !imain.or)
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
					if (imain.sr)
					{
						color = "lime";
						role = $" | Стажер {imain.warns}";
					}
					if (imain.hr)
					{
						color = "aqua";
						role = $" | Хелпер {imain.warns}";
					}
					if (imain.ghr)
					{
						color = "cyan";
						role = $" | Главный хелпер {imain.warns}";
					}
					if (imain.ar)
					{
						color = "yellow";
						role = $" | Админ {imain.warns}";
					}
					if (imain.gar)
					{
						color = "red";
						role = $" | Главный Админ {imain.warns}";
					}
					if (imain.asr)
					{
						color = "pumpkin";
						mrole += "Набор Администрации | ";
					}
					if (imain.dcr)
					{
						color = "pumpkin";
						mrole += "Контроль Донатеров | ";
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
				}
				if (imain.ytr)
				{
					color = "red";
					role += $" | YouTube";
				}
				if (imain.id == 1073)
				{
					color = "lime";
					role += $" | 3D Designer";
				}
				if (imain.id == 3053)
				{
					color = "deep_pink";
					role += $" | mercury :3";
				}
				if(Modules.CustomDonates.TryGetPlayerListPrefix(imain.id, out var customPref))
				{
					color = customPref.Color;
					role += $" | {customPref.Name}";
				}
				if (player.ItsScp035())
				{
					color = "red";
					srole = $"SCP 035 | {srole}";
				}
			}
			catch
			{ }
			try
			{
				if (imain.find && !imain.anonym) player.SetRank($"{clan}{srole}{mrole}{lvl} уровень{prime}{hook}{role}{prefix}".Trim(), color);
				else player.SetRank($"{clan}{srole}{lvl} уровень{prime}{hook}{prefix}".Trim(), color);
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
			if (Plugin.Anarchy) xp /= 10;
		}
		public void GetTotalMoney(Player pl, ref float money)
		{
			if (Plugin.Anarchy)
			{
				money = 0;
				return;
			}
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