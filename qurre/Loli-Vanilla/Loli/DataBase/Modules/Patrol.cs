using Loli.Addons;
using Loli.Webhooks;
using Newtonsoft.Json.Linq;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Loli.DataBase.Modules
{
	internal static class Patrol
	{
		internal static List<string> List = new();
		internal static List<string> Verified = new();
		internal static void Init()
		{
			Core.Socket.On("database.get.patrol", obj =>
			{
				string userid = obj[1].ToString();
				var pl = userid.GetPlayer();

				if (pl is null)
					return;

				if (!(bool)obj[2])
				{
					if (Verified.Contains(pl.UserInformation.UserId))
						Verified.Remove(pl.UserInformation.UserId);
				}
				else
				{
					if (!Verified.Contains(pl.UserInformation.UserId))
						Verified.Add(pl.UserInformation.UserId);
				}

				if (!(bool)obj[0])
				{
					if (List.Contains(pl.UserInformation.UserId))
						List.Remove(pl.UserInformation.UserId);
					return;
				}

				pl.Administrative.RaLogin();

				if (!List.Contains(pl.UserInformation.UserId))
					List.Add(pl.UserInformation.UserId);
			});

			CommandsSystem.RegisterRemoteAdmin("bring", Bring);
			CommandsSystem.RegisterRemoteAdmin("ban", Ban);

			static void Ban(RemoteAdminCommandEvent ev)
			{
				if (!Verified.Contains(ev.Sender.SenderId))
					return;

				ev.Allowed = false;

				if (!int.TryParse(ev.Args[1], out int dur))
				{
					ev.Reply = "Неверно указано время";
					return;
				}

				var ids = ev.Args[0].Split('.');
				List<Player> pls = new();
				foreach (var id in ids)
				{
					try
					{
						var pl = int.Parse(id).GetPlayer();
						if (pl is not null) pls.Add(pl);
					}
					catch { }
				}

				if (pls.Count == 0)
				{
					ev.Reply = "Игроки не выбраны";
					return;
				}

				string reason = string.Join(" ", ev.Args.Skip(2));
				if (reason.Length == 0)
				{
					ev.Reply = "Отсутствует причина бана";
					return;
				}

				reason += " - Бан от патруля";

				if (pls.Count > 3)
				{
					ev.Reply = "ай, ай, ай";
					return;
				}

				foreach (var pl in pls)
				{
					pl.Administrative.Ban(dur * 60, reason, $"Патруль ({ev.Sender.SenderId})");
				}

				ev.Success = true;
			}

			static void Bring(RemoteAdminCommandEvent ev)
			{
				if (!ev.Allowed)
					return;

				if (!Verified.Contains(ev.Sender.SenderId))
					return;

				string Arg0 = ev.Args.Length > 0 ? ev.Args[0].ToLower() : string.Empty;

				var pls = Arg0.Split('.');
				foreach (var id in pls)
				{
					try
					{
						Player pl = id.GetPlayer();
						pl.MovementState.Position = ev.Player.MovementState.Position;
					}
					catch (Exception err)
					{
						ev.Player.Client.SendConsole($"Произошла ошибка при bring {id}: {err}", "red");
					}
				}

				ev.Allowed = false;
				ev.Reply = "Успешно";
				ev.Success = true;
			}
		}


		[EventMethod(ServerEvents.CheaterReport)]
		static void Report(CheaterReportEvent ev)
		{
			if (!List.Contains(ev.Issuer.UserInformation.UserId))
				return;

			ev.Issuer.Client.ShowHint("<color=#ff7073>Репорт в отдел патруля отправлен</color>");
			ev.Issuer.Client.SendConsole("<color=#ff7073>Репорт в отдел патруля отправлен</color>", "white");

			new Thread(() =>
			{
				SteamMainInfoApi json = new() { personaname = ev.Target.UserInformation.Nickname };
				int lvl = 0;

				if (!ev.Target.UserInformation.UserId.Contains("@discord"))
				{
					var url = "https://" + $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={Core.SteamToken}&format=json&steamids=" +
						ev.Target.UserInformation.UserId.Replace("@steam", "");
					var request = WebRequest.Create(url);
					request.Method = "GET";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					var privacy = JObject.Parse(data);
					var pls = privacy["response"]["players"];
					json = pls.ToObject<SteamMainInfoApi[]>()[0];
				}

				{
					var url = "https://" + $"api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key={Core.SteamToken}&steamid=" +
						ev.Target.UserInformation.UserId.Replace("@steam", "");
					var request = WebRequest.Create(url);
					request.Method = "GET";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					if (!data.Contains("player_level")) lvl = 0;
					else
					{
						var privacy = JObject.Parse(data);
						lvl = privacy["response"]["player_level"].ToObject<int>();
					}
				}

				Dishook webhk = new("https://discord.com/api/webhooks/");
				List<Embed> listEmbed = new();
				Embed embed = new()
				{
					Color = 16729088,
					Author = new() { Name = $"{json.personaname} | {lvl} уровень в стиме | {ev.Target.UserInformation.UserId}", IconUrl = json.avatar },
					Footer = new() { Text = Server.Ip + ":" + Core.Port },
					TimeStamp = DateTimeOffset.Now,
					Description = ev.Reason.Trim()
				};
				listEmbed.Add(embed);
				webhk.Send("<@&1193669353204371507>, вас вызывают на сервер " + Core.ServerName, Core.ServerName, null, false, embeds: listEmbed);

				ev.Issuer.Client.SendConsole("Администрация вызвана", "white");

				new Dishook("https://discord.com/api/webhooks/")
					.Send($"{ev.Issuer.UserInformation.Nickname} - {ev.Issuer.UserInformation.UserId} из патруля", Core.ServerName, null, false, embeds: listEmbed);
			}).Start();
		}


		[EventMethod(ServerEvents.RemoteAdminCommand, 3)]
		static void Force(RemoteAdminCommandEvent ev)
		{
			if (!ev.Allowed)
				return;

			if (ev.Name != "forceclass")
				return;

			bool list1 = List.Contains(ev.Sender.SenderId);
			bool list2 = Verified.Contains(ev.Sender.SenderId);

			if (!list1 && !list2)
				return;

			string Arg0 = ev.Args.Length > 0 ? ev.Args[0].ToLower() : string.Empty;
			string Arg1 = ev.Args.Length > 1 ? ev.Args[1].ToLower() : string.Empty;

			if (Arg0 == "spectator")
			{
				ev.Player.RoleInformation.SetNew(RoleTypeId.Spectator, RoleChangeReason.RemoteAdmin);
				goto IL_1;
			}

			if (Arg0 == "overwatch" || Arg1 == "overwatch")
			{
				ev.Player.RoleInformation.SetNew(RoleTypeId.Overwatch, RoleChangeReason.RemoteAdmin);
				goto IL_1;
			}

			if (!list2)
				return;

			if (Arg0 == "tutorial")
			{
				ev.Player.RoleInformation.SetNew(RoleTypeId.Tutorial, RoleChangeReason.RemoteAdmin);
				goto IL_1;
			}

			if (Arg1 == "tutorial")
			{
				var pls = Arg0.Split('.');
				foreach (var id in pls)
				{
					try
					{
						if (!int.TryParse(id.Replace(".", ""), out int parsed_id))
						{
							ev.Player.Client.SendConsole($"Произошла ошибка при спавне {{парсинг int}} {id}", "red");
							continue;
						}

						Player pl = parsed_id.GetPlayer();
						pl.RoleInformation.SetNew(RoleTypeId.Tutorial, RoleChangeReason.RemoteAdmin);
					}
					catch (Exception err)
					{
						ev.Player.Client.SendConsole($"Произошла ошибка при спавне {id}: {err}", "red");
					}
				}
				goto IL_1;
			}

			if (Arg0 == "scp0492" || Arg1 == "scp0492")
			{
				ev.Player.RoleInformation.SetNew(RoleTypeId.Scp0492, RoleChangeReason.RemoteAdmin);
				goto IL_1;
			}


			if (Arg1 == "spectator")
			{
				var pls = Arg0.Split('.');
				foreach (var id in pls)
				{
					try
					{
						if (!int.TryParse(id.Replace(".", ""), out int parsed_id))
						{
							ev.Player.Client.SendConsole($"Произошла ошибка при спавне {{парсинг int}} {id}", "red");
							continue;
						}

						Player pl = parsed_id.GetPlayer();
						if (pl.RoleInformation.Role is RoleTypeId.Tutorial or RoleTypeId.Scp0492)
						{
							pl.RoleInformation.SetNew(RoleTypeId.Spectator, RoleChangeReason.RemoteAdmin);
						}
					}
					catch (Exception err)
					{
						ev.Player.Client.SendConsole($"Произошла ошибка при спавне {id}: {err}", "red");
					}
				}
				goto IL_1;
			}

			return;

		IL_1:
			{
				ev.Allowed = false;
				ev.Reply = "Успешно";
				ev.Success = true;
			}
		}

#pragma warning disable CS0649
		[Serializable]
		class SteamMainInfoApi
		{
			public string personaname;
			public string avatar;
		}
#pragma warning restore CS0649
	}
}