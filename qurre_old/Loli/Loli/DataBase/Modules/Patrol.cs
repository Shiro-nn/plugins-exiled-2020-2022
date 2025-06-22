using Loli.Addons;
using Loli.Discord;
using Newtonsoft.Json.Linq;
using Qurre.API;
using Qurre.API.Events;
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
			Plugin.Socket.On("database.get.patrol", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get(userid);
				if (pl is null) return;
				if (!(bool)obj[2])
				{
					if (Verified.Contains(pl.UserId)) Verified.Remove(pl.UserId);
				}
				else
				{
					if (!Verified.Contains(pl.UserId)) Verified.Add(pl.UserId);
				}
				if (!(bool)obj[0])
				{
					if (List.Contains(pl.UserId)) List.Remove(pl.UserId);
					return;
				}
				pl.RaLogin();
				if (!List.Contains(pl.UserId)) List.Add(pl.UserId);
			});
			CommandsSystem.RegisterConsole("patrol", Patrol);
			CommandsSystem.RegisterRemoteAdmin("overwatch", OverWatch);
			CommandsSystem.RegisterRemoteAdmin("ban", Ban);
			static void Ban(SendingRAEvent ev)
			{
				if (!Verified.Contains(ev.CommandSender.SenderId)) return;
				ev.Allowed = false;
				if (!int.TryParse(ev.Args[1], out int dur))
				{
					ev.ReplyMessage = "Неверно указано время";
					return;
				}
				var ids = ev.Args[0].Split('.');
				List<Player> pls = new();
				foreach (var id in ids)
				{
					try
					{
						var pl = Player.Get(int.Parse(id));
						if (pl is not null) pls.Add(pl);
					}
					catch { }
				}
				if (pls.Count == 0)
				{
					ev.ReplyMessage = "Игроки не выбраны";
					return;
				}
				string reason = string.Join(" ", ev.Args.Skip(2));
				if (reason.Length == 0)
				{
					ev.ReplyMessage = "Отсутствует причина бана";
					return;
				}
				reason += " - Бан от патруля";
				if (pls.Count > 3)
				{
					ev.ReplyMessage = "ай, ай, ай";
					return;
				}
				foreach (var pl in pls)
				{
					pl.Ban(dur * 60, reason, $"Патруль ({ev.CommandSender.SenderId})");
				}
				ev.Success = true;
			}
			static void OverWatch(SendingRAEvent ev)
			{
				if (!List.Contains(ev.CommandSender.SenderId)) return;
				ev.Allowed = false;
				ev.ReplyMessage = "Успешно";
				ev.Success = true;
				if (ev.Args.Length < 2)
				{
					ev.Player.Overwatch = !ev.Player.Overwatch;
					return;
				}
				ev.Player.Overwatch = ev.Args[1] == "1";
			}
			static void Patrol(SendingConsoleEvent ev)
			{
				ev.Allowed = false;
				if (!List.Contains(ev.Player.UserId))
				{
					ev.ReturnMessage = "Вы не являетесь патрулем";
					ev.Color = "red";
					return;
				}
				if (ev.Args.Length < 2)
				{
					ev.ReturnMessage = "Первый аргумент должен быть индентификатором подозреваемого, а второй - описанием подозрительного момента";
					ev.Color = "red";
					return;
				}
				var pl = Player.Get(ev.Args[0]);
				if (pl is null)
				{
					ev.ReturnMessage = "Подозреваемый не найден";
					ev.Color = "red";
					return;
				}
				ev.ReturnMessage = "Отправка репорта...";
				ev.Color = "white";
				new Thread(() =>
				{
					SteamMainInfoApi json = new() { personaname = pl.Nickname };
					int lvl = 0;
					if (!pl.UserId.Contains("@discord"))
					{
						var url = "https://" + "api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=C6080922CC0D5C8AE73B1C2499805ED5&format=json&steamids=" +
							pl.UserId.Replace("@steam", "");
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
						var url = "https://" + "api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key=C6080922CC0D5C8AE73B1C2499805ED5&steamid=" +
							pl.UserId.Replace("@steam", "");
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
					Webhook webhk = new("https://discord.com/api/webhooks");
					List<Embed> listEmbed = new();
					Embed embed = new()
					{
						Color = 16729088,
						Author = new() { Name = $"{json.personaname} | {lvl} уровень в стиме | {pl.UserId}", IconUrl = json.avatar },
						Footer = new() { Text = Server.Ip + ":" + Server.Port },
						TimeStamp = System.DateTimeOffset.Now,
						Description = string.Join(" ", ev.Args.Skip(1)).Trim()
					};
					listEmbed.Add(embed);
					webhk.Send("<@&722023017680732200>, вас вызывают на сервер " + Plugin.ServerName, Plugin.ServerName, null, false, embeds: listEmbed);
					ev.Player.SendConsoleMessage("Администрация вызвана", "white");

					new Webhook("https://discord.com/api/webhooks")
						.Send($"{ev.Player.Nickname} - {ev.Player.UserId} из патруля", Plugin.ServerName, null, false, embeds: listEmbed);
				}).Start();
			}
		}
#pragma warning disable CS0649
		[System.Serializable]
		class SteamMainInfoApi
		{
			public string personaname;
			public string avatar;
		}
#pragma warning restore CS0649
	}
}