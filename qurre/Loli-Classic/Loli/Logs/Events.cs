using HarmonyLib;
using Loli.DataBase.Modules;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
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
			return AccessTools.Method(type, "RemoteAdminCommand");
		}

		[HarmonyPrefix]
		internal static bool Prefix(RemoteAdminCommandEvent ev)
		{
			try
			{
				#region list
				if (ev.Name.ToLower() == "list")
				{
					ev.Allowed = false;
					string message = $"{Player.List.Count()}/{Core.MaxPlayers}\n";

					foreach (Player player in Player.List)
						message += $"{player.UserInfomation.Nickname} - {player.UserInfomation.UserId} " +
							$"({player.UserInfomation.Id}) [{player.GetCustomRole()}]\n";

					if (string.IsNullOrEmpty(message))
						message = $"Нет игроков онлайн";

					ev.Sender.RaReply($"{message}", true, true, string.Empty);
				}
				else if (ev.Name.ToLower() == "stafflist")
				{
					ev.Allowed = false;
					bool isStaff = false;
					string names = "";
					foreach (Player player in Player.List)
					{
						if (Data.Users.TryGetValue(player.UserInfomation.UserId, out var _main) &&
							(_main.trainee || _main.helper || _main.mainhelper || _main.admin || _main.mainadmin || _main.control || _main.maincontrol))
						{
							string role = "";
							if (_main.trainee) role = "- Стажер";
							else if (_main.helper) role = "- Хелпер";
							else if (_main.mainhelper) role = "- Главный Хелпер";
							else if (_main.admin) role = "- Админ";
							else if (_main.mainadmin) role = "- Главный Админ";
							else if (_main.control) role = "- Контроль MRP";
							else if (_main.maincontrol) role = "- Контроль Администрации";
							isStaff = true;
							names += $"{player.UserInfomation.Nickname} {role}\n";
						}
					}

					string response = isStaff ? names : $"Нет администрации онлайн.";
					ev.Sender.RaReply($"{Player.List.Count()}/{Core.MaxPlayers}\n{response}", true, true, string.Empty);
				}
				#endregion
				try
				{
					#region logs
					if (ev.Name.StartsWith("$"))
						return false;
					if (ev.Player == null)
						return false;
					if (ev.Player == Server.Host)
						return false;
					if (ev.Player.UserInfomation.UserId == "")
						return false;
					if (ev.Player.UserInfomation.Nickname == "Dedicated Server")
						return false;
					if (SCPDiscordLogs.Api.BlockInRaLogs(ev.Player.UserInfomation.UserId))
						return false;

					string Args = string.Join(" ", ev.Args);
					string msg = "";
					string d = "'";

					try
					{
						if (ev.Name == "forceclass")
						{
							string targets = "";

							string role = ev.Args.Length > 1 ? ev.Args[1] : ev.Args[0];
							if (ev.Args.Count() > 1)
							{
								string[] spearator = { "." };
								string[] strlist = ev.Args[0].Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
								foreach (string s in strlist)
								{
									try { targets += $"{s}{d}{int.Parse(s).GetPlayer()?.UserInfomation.Nickname}{d} "; } catch { }
								}
							}
							msg = $"{ev.Name} {targets} {role} {Args.Replace(ev.Args[0], "").Replace($"{role}", "")}";
						}
						else if (ev.Name == "request_data")
						{
							string targets = "";
							string[] spearator = { "." };
							string[] strlist = ev.Args[1].Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
							foreach (string s in strlist)
							{
								targets += $"{s}{d}{int.Parse(s).GetPlayer()?.UserInfomation.Nickname}{d} ";
							}
							msg = $"{ev.Name} {ev.Args[0]} {targets} {Args.Replace(ev.Args[0].ToLower(), "").Replace(ev.Args[1].ToLower(), "")}";
						}
						else if (ev.Name == "give")
						{
							string targets = "";
							string itemsstr = "";
							string itemsoriginal = "";

							if (ev.Args.Count() > 1)
							{
								string[] spearator = { "." };
								string[] strlist = ev.Args[0].Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
								foreach (string s in strlist)
								{
									try { targets += $"{s}{d}{int.Parse(s).GetPlayer()?.UserInfomation.Nickname}{d} "; } catch { }
								}

								string[] items = ev.Args[1].Split(spearator, int.MaxValue, StringSplitOptions.RemoveEmptyEntries);
								foreach (string s in items)
								{
									try { itemsstr += $"{s}{d}{(ItemType)int.Parse(s)}{d}"; } catch { }
								}

								itemsoriginal = ev.Args[1];
							}
							else
							{
								string[] spearator = { "." };
								string[] items = ev.Args[0].Split(spearator, int.MaxValue, StringSplitOptions.RemoveEmptyEntries);
								foreach (string s in items)
								{
									try { itemsstr += $"{s}{d}{(ItemType)int.Parse(s)}{d}"; } catch { }
								}

								itemsoriginal = ev.Args[0];
							}
							msg = $"{ev.Name} {targets} {itemsstr} {Args.Replace(ev.Args[0], "").Replace($"{itemsoriginal}", "")}";
						}
						else if (ev.Name == "overwatch" || ev.Name == "bypass" || ev.Name == "heal" || ev.Name == "god" || ev.Name == "noclip" || ev.Name == "doortp" || ev.Name == "bring"
							|| ev.Name == "mute" || ev.Name == "unmute" || ev.Name == "imute" || ev.Name == "iunmute")
						{
							string targets = "";
							string[] spearator = { "." };
							string[] strlist = ev.Args[0].Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
							foreach (string s in strlist)
							{
								var pl = int.Parse(ev.Args[0]).GetPlayer();
								try { targets += $"{s}{d}{pl?.UserInfomation.Nickname}({pl?.UserInfomation.UserId}){d} "; } catch { }
							}
							msg = $"{ev.Name} {targets} {Args.Replace(ev.Args[0], "")}";
						}
						else if (ev.Name == "goto")
						{
							var pl = int.Parse(ev.Args[0]).GetPlayer();
							string target = $"{ev.Args[0]}{d}{pl?.UserInfomation.Nickname}({pl?.UserInfomation.UserId}){d} ";
							msg = $"{ev.Name} {target} {Args.Replace(ev.Args[0], "")}";
						}
						else
						{
							msg = $"{ev.Command}";
						}
					}
					catch
					{
						msg = $"{ev.Command}";
					}

					msg = ":keyboard: {0} использовал команду: " + SCPDiscordLogs.Api.AntiMD(msg);
					SCPDiscordLogs.Api.SendMessage(msg.Replace("{0}", SCPDiscordLogs.Api.PlayerInfo(ev.Player, false)));

					if (!Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var main))
						return false;
					if (main.trainee || main.helper || main.mainhelper || main.admin || main.mainadmin || main.control || main.maincontrol)
					{
						Api.SendRa(msg.Replace("{0}", $"<@{main.id}>"));
						SCPDiscordLogs.Api.SendMessage(msg.Replace("{0}", $"<@{main.discord}>"), SCPDiscordLogs.Api.Status.RemoteAdmin);
					}
					else if (main.donater || (Data.Roles.TryGetValue(ev.Player.UserInfomation.UserId, out var _r) && (_r.Priest || _r.Mage || _r.Sage || _r.Star)))
					{
						Events._wait_ra = true;
						Events._round_ra = true;
						Api.SendRa(msg.Replace("{0}", $"<@{main.id}>"), Api.Status.Donate);
					}
					#endregion
				}
				catch { }
				return false;
			}
			catch { return true; }
		}
	}
	static class Events
	{
		internal static bool _wait_ra = false;
		internal static bool _round_ra = false;

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting()
		{
			if (_round_ra) Api.SendRa($"⌛ Ожидание игроков...", Api.Status.Donate);
			_wait_ra = false;
			_round_ra = false;
		}

		[EventMethod(RoundEvents.Start)]
		static void RoundStart()
		{
			if (_wait_ra) Api.SendRa($"▶️ Раунд запущен: {Player.List.Count()} игроков на сервере.", Api.Status.Donate);
			_wait_ra = false;
			_round_ra = false;
		}

		[EventMethod(RoundEvents.End)]
		static void RoundEnd()
		{
			if (_round_ra) Api.SendRa($"⏹️ Раунд закончен: {Player.List.Count()} игроков онлайн.", Api.Status.Donate);
		}
	}
}