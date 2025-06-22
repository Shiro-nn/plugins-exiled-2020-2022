using HarmonyLib;
using Loli.Discord;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Loli.Logs
{
	[HarmonyPatch]
	internal static class RALogsPatch
	{
		internal static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("SCPDiscordLogs.EventHandlers");
			return AccessTools.Method(type, "SendingRA");
		}
		internal static bool Prefix(SendingRAEvent ev)
		{
			try
			{
				#region list
				if (ev.Name.ToLower() == "list")
				{
					ev.Allowed = false;
					string message = $"{PlayerManager.players.Count}/{Plugin.MaxPlayers}\n";
					foreach (Player player in Player.List) message += $"{player.Nickname} - {player.UserId} ({player.Id}) [{player.GetCustomRole()}]\n";
					if (string.IsNullOrEmpty(message)) message = $"Нет игроков онлайн.";
					ev.CommandSender.RaReply($"{message}", true, true, string.Empty);
				}
				else if (ev.Name.ToLower() == "stafflist")
				{
					ev.Allowed = false;
					bool isStaff = false;
					string names = "";
					foreach (Player player in Player.List)
					{
						var _main = DataBase.Manager.Static.Data.Users[player.UserId];
						if (_main.sr || _main.hr || _main.ghr || _main.ar || _main.gar)
						{
							string role = "";
							if (_main.sr) role = "- Стажер";
							if (_main.hr) role = "- Хелпер";
							if (_main.ghr) role = "- Главный Хелпер";
							if (_main.ar) role = "- Админ";
							if (_main.gar) role = "- Главный Админ";
							isStaff = true;
							names += $"{player.Nickname} {role}\n";
						}
					}

					string response = isStaff ? names : $"Нет администрации онлайн.";
					ev.CommandSender.RaReply($"{PlayerManager.players.Count}/{Plugin.MaxPlayers}\n{response}", true, true, string.Empty);
				}
				#endregion
				#region logs
				if (ev.Player == null) return false;
				if (ev.Player == Server.Host) return false;
				if (ev.Player.UserId == "") return false;
				if (ev.Player.UserId == "-@steam") return false;
				string Args = ev.Command.ToLower().Replace($"{ev.Name} ", "");
				string msg = "";
				try
				{
					if (ev.Name == "forceclass")
					{
						string targets = "";
						RoleType role = RoleType.None;
						var role_id = 0;
						if (ev.Args.Count() > 1)
						{
							string[] spearator = { "." };
							string[] strlist = ev.Args[0].Split(spearator, 2, System.StringSplitOptions.RemoveEmptyEntries);
							foreach (string s in strlist)
							{
								try { targets += $"{s}^{Player.Get(int.Parse(s)).Nickname}^ "; } catch { }
							}
							try { role_id = Convert.ToInt32(ev.Args[1]); } catch { }
							try { role = (RoleType)Convert.ToInt32(ev.Args[1]); } catch { }
						}
						else
						{
							try { role_id = Convert.ToInt32(ev.Args[0]); } catch { }
							try { role = (RoleType)Convert.ToInt32(ev.Args[0]); } catch { }
						}
						msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {targets} {role_id}^{role}^ {Args.Replace(ev.Args[0], "").Replace($"{role_id}", "")}";
					}
					else if (ev.Name == "request_data")
					{
						string targets = "";
						string[] spearator = { "." };
						string[] strlist = ev.Args[1].Split(spearator, 2, System.StringSplitOptions.RemoveEmptyEntries);
						foreach (string s in strlist)
						{
							targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
						}
						msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {ev.Args[0]} {targets} {Args.Replace(ev.Args[0].ToLower(), "").Replace(ev.Args[1].ToLower(), "")}";
					}
					else if (ev.Name == "give")
					{
						string targets = "";
						ItemType item = ItemType.Coin;
						var item_id = 0;
						if (ev.Args.Count() > 1)
						{
							string[] spearator = { "." };
							string[] strlist = ev.Args[0].Split(spearator, 2, System.StringSplitOptions.RemoveEmptyEntries);
							foreach (string s in strlist)
							{
								try { targets += $"{s}^{Player.Get(int.Parse(s)).Nickname}^ "; } catch { }
							}
							try { item = (ItemType)Convert.ToInt32(ev.Args[1]); } catch { }
							try { item_id = Convert.ToInt32(ev.Args[1]); } catch { }
						}
						else
						{
							try { item = (ItemType)Convert.ToInt32(ev.Args[0]); } catch { }
							try { item_id = Convert.ToInt32(ev.Args[0]); } catch { }
						}
						msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {targets} {item_id}^{item}^ {Args.Replace(ev.Args[0], "").Replace($"{item_id}", "")}";
					}
					else if (ev.Name == "overwatch" || ev.Name == "bypass" || ev.Name == "heal" || ev.Name == "god" || ev.Name == "noclip" || ev.Name == "doortp" || ev.Name == "bring"
						|| ev.Name == "mute" || ev.Name == "unmute" || ev.Name == "imute" || ev.Name == "iunmute")
					{
						string targets = "";
						string[] spearator = { "." };
						string[] strlist = ev.Args[0].Split(spearator, 2, System.StringSplitOptions.RemoveEmptyEntries);
						foreach (string s in strlist)
						{
							try { targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ "; } catch { }
						}
						msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {targets} {Args.Replace(ev.Args[0], "")}";
					}
					else if (ev.Name == "goto")
					{
						string target = $"{ev.Args[0]}^{Player.Get(int.Parse(ev.Args[0]))?.Nickname}^ ";
						msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Name} {target} {Args.Replace(ev.Args[0], "")}";
					}
					else
					{
						msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Command}";
					}
				}
				catch
				{
					msg = $":keyboard: {ev.Player.Nickname}({ev.Player.UserId}) использовал команду: {ev.Command}";
				}
				var main = DataBase.Manager.Static.Data.Users[ev.Player.UserId];
				SCPDiscordLogs.Api.SendMessage($"{msg}");
				if (main.sr || main.hr || main.ghr || main.ar || main.gar)
				{
					if (Plugin.Anarchy)
					{
						try { SendHook($"{msg.Replace($"{ev.Player.Nickname}({ev.Player.UserId})", $"<@{main.discord}>")}"); } catch { }
						return false;
					}
					Api.SendRa($"{msg.Replace($"{ev.Player.Nickname}({ev.Player.UserId})", $"<@{main.id}>")}");
					SCPDiscordLogs.Api.SendMessage($"{msg.Replace($"{ev.Player.Nickname}({ev.Player.UserId})", $"<@{main.discord}>")}", SCPDiscordLogs.Api.Status.RemoteAdmin);
				}
				else if (main.don || (DataBase.Manager.Static.Data.Roles.TryGetValue(ev.Player.UserId, out var _r) && (_r.Priest || _r.Mage || _r.Sage || _r.Star)))
				{
					Events._wait_ra = true;
					Events._round_ra = true;
					Api.SendRa($"{msg.Replace($"{ev.Player.Nickname}({ev.Player.UserId})", $"<@{main.id}>")}", Api.Status.Donate);
				}
				else if (main.ytr)
				{
					Api.SendRa($"{msg.Replace($"{ev.Player.Nickname}({ev.Player.UserId})", $"<@{main.id}>")}", Api.Status.YouTube);
				}
				#endregion
				return false;
			}
			catch { return true; }
		}
		internal const string Hook = "https://discord.com/api/webhooks";
		private static void SendHook(string message)
		{
			Webhook webhk = new(Hook);
			List<Embed> listEmbed = new();
			Embed embed = new();
			embed.Color = 3693497;
			embed.Description = message;
			listEmbed.Add(embed);
			webhk.Send("", Plugin.ServerName, embeds: listEmbed);
		}
	}
	public class Events
	{
		public Plugin plugin;
		public static Plugin splugin;
		public Events(Plugin plugin)
		{
			this.plugin = plugin;
			splugin = plugin;
		}
		internal static bool _wait_ra = false;
		internal static bool _round_ra = false;
		public void Waiting()
		{
			if (_round_ra) Api.SendRa($"⌛ Ожидание игроков...", Api.Status.Donate);
			_wait_ra = false;
			_round_ra = false;
		}
		public void RoundStart()
		{
			if (_wait_ra) Api.SendRa($"▶️ Раунд запущен: {Player.List.Count()} игроков на сервере.", Api.Status.Donate);
			_wait_ra = false;
			_round_ra = false;
		}
		public void RoundEnd(RoundEndEvent _)
		{
			if (_round_ra) Api.SendRa($"⏹️ Раунд закончен: {Player.List.Count()} игроков онлайн.", Api.Status.Donate);
		}
	}
}