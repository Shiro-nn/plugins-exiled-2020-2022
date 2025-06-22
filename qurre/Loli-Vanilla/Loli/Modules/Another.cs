using Loli.DataBase;
using Loli.DataBase.Modules;
using Loli.HintsCore;
using Loli.Webhooks;
using MEC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Loli.Modules
{
	static class Another
	{
		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (!Round.Ended)
				Core.Socket.Emit("server.leave", new object[] { Core.ServerID, ev.Player.UserInformation.UserId });
		}

		[EventMethod(RoundEvents.End)]
		static void ClearIps() => Core.Socket.Emit("server.clearips", new object[] { Core.ServerID });



		static readonly string[] WhiteList = [ "-@steam", "-@steam", "-@steam", "-@steam", "-@steam" ];
		static readonly List<string> CachedSended = [];

		[EventMethod(PlayerEvents.Join)]
		static void BlockKids(JoinEvent ev)
		{
			string userId = ev.Player.UserInformation.UserId;

			if (!userId.EndsWith("@steam"))
				return;

			if (WhiteList.Contains(userId))
				return;

			new Thread(() =>
			{
				DoubfulData data = getData();

				if (data is null)
				{
					return;
				}

				if (data.Banned && data.DaysSinceLastBan < 30 && (data.GameHours < 3600 || data.Level < 3))
				{
					DoRun(data, false, text: "Недавно была получена игровая блокировка");
					return;
				}

				var created_array = data.CreatedFormatted.Split('.');
				int created_year = int.Parse(created_array[2]);
				DateTime register = new(created_year, int.Parse(created_array[1]), int.Parse(created_array[0]));
				TimeSpan elapsed = DateTime.Now - register;

				if (elapsed.TotalDays < 3)
				{
					DoRun(data, false, text: "Аккаунт создан совсем недавно");
					return;
				}

				if (data.GameHours < 1200 && data.Level < 3 && created_year != 1970 && elapsed.TotalDays < 30)
				{
					DoRun(data, false, text: "Мало часов в SCPSL, низкий уровень Steam, аккаунт создан недавно");
					return;
				}

				if (created_year == 1970 || elapsed.TotalDays < 16 || !data.IsSetup)
				{
					GeoIP geoip = GetGeoIP();

					if (geoip is null)
					{
						return;
					}

					if (geoip.Org.Contains("Cloudflare, Inc"))
					{
						DoRun(data, text: "Подключение с провайдера в ЧС\nFrom: CloudFlare");
						return;
					}

					if (Check(geoip, "AS16345 PJSC \"Vimpelcom\"", city: "Saratov"))
						return;

					if (Check(geoip, "AS16345 PJSC \"Vimpelcom\"", city: "Lyubertsy"))
						return;

					if (Check(geoip, "AS12714 PJSC MegaFon", city: "Belgorod"))
						return;

					if (Check(geoip, "AS28917 Fiord Networks, UAB", region: "Moscow Oblast"))
						return;

					if (Check(geoip, "AS205368 Fnet LLC", region: "Yerevan"))
						return;

					bool Check(GeoIP json, string org, string city = "", string region = "")
					{
						if ((json.City == city || json.Region == region) && json.Org == org)
						{
							DoRun(data, false, text: $"Подключение с провайдера в ЧС\nПровайдер: ||{json.Org}||\nГород: ||{json.City} ({json.Country})||");
							return true;
						}
						return false;
					}
				}

			}).Start();

			DoubfulData getData()
			{
				try
				{
					var url = $"{Core.APIUrl}/doubtful?steam={userId.Replace("@steam", "")}";
					var request = WebRequest.Create(url);
					request.Method = "POST";

					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);

					var data = reader.ReadToEnd();
					DoubfulData json = JsonConvert.DeserializeObject<DoubfulData>(data);
					return json;
				}
				catch
				{
					return null;
				}
			};

			GeoIP GetGeoIP()
			{
				try
				{
					var url = $"{Core.APIUrl}/geoip?ip={ev.Player.UserInformation.Ip}";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					GeoIP json = JsonConvert.DeserializeObject<GeoIP>(data);
					return json;
				}
				catch
				{
					return null;
				}
			};

			void DoRun(DoubfulData data, bool disconnect = true, string text = "")
			{
				if (disconnect)
				{
					ev.Player.Client.Disconnect("<b><color=red>Вам был закрыт доступ на сервер</color>\n" +
						"<color=#00ff19>Определение автоматизировано,\n" +
						"Если вас случайно занесло в черный список,\n" +
						"Откройте тикет на сервере в Discord</color></b>"
					);
				}

				if (CachedSended.Contains(userId))
					return;

				CachedSended.Add(userId);

				SteamMainInfoApi json = new() { Name = ev.Player.UserInformation.Nickname };
				int serverLvl = -1;

				try
				{
					var url = "https://" + $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={Core.SteamToken}&format=json&steamids=" + data.Id;
					var request = WebRequest.Create(url);
					request.Method = "GET";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var reply = reader.ReadToEnd();
					var privacy = JObject.Parse(reply);
					var pls = privacy["response"]["players"];
					json = pls.ToObject<SteamMainInfoApi[]>()[0];
				}
				catch { }

				if (Data.Users.TryGetValue(userId, out var imain))
					serverLvl = imain.lvl;

				string hook = "https://discord.com/api/webhooks";
				Dishook webhk = new(hook);
				Embed embed = new()
				{
					Title = "Попытка подключения",
					Url = "https://steamcommunity.com/profiles/" + data.Id,
					Color = disconnect ? 16202518 : 16753920,
					Thumbnail = new()
					{
						Url = json.AvatarFull
					},
					Description = $"**Ник:** {json.Name ?? ev.Player.UserInformation.Nickname} \n" +
					$"**UserID:** {userId} \n" +
					$"**IP:** ||{ev.Player.UserInformation.Ip}|| \n" +
					$"**Аккаунт создан:** {data.CreatedFormatted} \n" +
					$"**Уровень в Steam:** {data.Level} \n" +
					$"**Уровень на сервере:** {serverLvl} \n" +
					$"**Часов в SCPSL:** {(data.GameHours == 0 ? "Неизвестно" : Math.Floor((decimal)(data.GameHours / 60)))} \n" +
					$"**Настроен ли профиль в Steam:** {(data.IsSetup ? "Да" : "Нет")} \n" +
					$"**Наличие банов в Steam:** {(data.Banned ? "Имеются" : "Нет")} \n" +
					(data.Banned ? $"**Прошло с последнего бана в Steam:** {data.DaysSinceLastBan} дней \n" : string.Empty) +
					$" \n" +
					$"Причина репорта: \n" +
					$"{text}",
					Footer = new()
					{
						Text = disconnect ? "🛑 доступ на сервер закрыт" : "⚠️ предупреждение для администрации",
					},
					TimeStamp = DateTimeOffset.Now
				};
				List<Embed> listEmbed = new() { embed };
				webhk.Send("Замечена попытка подключения на сервер мутной личности.", Core.ServerName, null, false, embeds: listEmbed);
			}
		}
	}
}