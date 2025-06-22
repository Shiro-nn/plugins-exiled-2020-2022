using Loli.Addons;
using Loli.Discord;
using Newtonsoft.Json.Linq;
using Qurre.API;
using Qurre.Events.Structs;
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
				if (pl is null) return;
				if (!(bool)obj[2])
				{
					if (Verified.Contains(pl.UserInfomation.UserId))
						Verified.Remove(pl.UserInfomation.UserId);
				}
				else
				{
					if (!Verified.Contains(pl.UserInfomation.UserId))
						Verified.Add(pl.UserInfomation.UserId);
				}
				if (!(bool)obj[0])
				{
					if (List.Contains(pl.UserInfomation.UserId))
						List.Remove(pl.UserInfomation.UserId);
					return;
				}
				pl.Administrative.RaLogin();
				if (!List.Contains(pl.UserInfomation.UserId))
					List.Add(pl.UserInfomation.UserId);
			});
			CommandsSystem.RegisterConsole("patrol", Patrol);
			CommandsSystem.RegisterRemoteAdmin("overwatch", OverWatch);
			CommandsSystem.RegisterRemoteAdmin("ban", Ban);
			static void Ban(RemoteAdminCommandEvent ev)
			{
				if (!Verified.Contains(ev.Sender.SenderId)) return;
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
			static void OverWatch(RemoteAdminCommandEvent ev)
			{
				if (!List.Contains(ev.Sender.SenderId)) return;
				ev.Allowed = false;
				ev.Reply = "Успешно";
				ev.Success = true;
				if (ev.Args.Length < 2)
				{
					ev.Player.GamePlay.Overwatch = !ev.Player.GamePlay.Overwatch;
					return;
				}
				ev.Player.GamePlay.Overwatch = ev.Args[1] == "1";
			}
			static void Patrol(GameConsoleCommandEvent ev)
			{
				ev.Allowed = false;
				if (!List.Contains(ev.Player.UserInfomation.UserId))
				{
					ev.Reply = "Вы не являетесь патрулем";
					ev.Color = "red";
					return;
				}
				if (ev.Args.Length < 2)
				{
					ev.Reply = "Первый аргумент должен быть индентификатором подозреваемого, а второй - описанием подозрительного момента";
					ev.Color = "red";
					return;
				}
				var pl = ev.Args[0].GetPlayer();
				if (pl is null)
				{
					ev.Reply = "Подозреваемый не найден";
					ev.Color = "red";
					return;
				}
				ev.Reply = "Отправка репорта...";
				ev.Color = "white";
				new Thread(() =>
				{
					SteamMainInfoApi json = new() { personaname = pl.UserInfomation.Nickname };
					int lvl = 0;
					if (!pl.UserInfomation.UserId.Contains("@discord"))
					{
						var url = "https://" + $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={Core.SteamToken}&format=json&steamids=" +
							pl.UserInfomation.UserId.Replace("@steam", "");
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
							pl.UserInfomation.UserId.Replace("@steam", "");
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
						Author = new() { Name = $"{json.personaname} | {lvl} уровень в стиме | {pl.UserInfomation.UserId}", IconUrl = json.avatar },
						Footer = new() { Text = Server.Ip + ":" + Server.Port },
						TimeStamp = System.DateTimeOffset.Now,
						Description = string.Join(" ", ev.Args.Skip(1)).Trim()
					};
					listEmbed.Add(embed);
					webhk.Send("<@&722023017680732200>, вас вызывают на сервер " + Core.ServerName, Core.ServerName, null, false, embeds: listEmbed);
					ev.Player.Client.SendConsole("Администрация вызвана", "white");

					new Webhook("https://discord.com/api/webhooks")
						.Send($"{ev.Player.UserInfomation.Nickname} - {ev.Player.UserInfomation.UserId} из патруля", Core.ServerName, null, false, embeds: listEmbed);
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