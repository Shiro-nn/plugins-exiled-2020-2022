using Loli.Discord;
using MongoDB.Bson;
using MongoDB.Driver;
using Qurre.API;
using Qurre.API.DataBase;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		internal void AntiBan(SendingRAEvent ev)
		{
			if (ev.Name != "ban") return;
			string[] str = ev.Args[0].Split('.');
			if (str.Count() > 2)
			{
				ev.Allowed = false;
				var _data = DataBase.Manager.Static.Data.Users[ev.Player.UserId];
				if (_data == null || (_data.don && !(_data.sr || _data.hr || _data.ghr || _data.ar || _data.gar))) return;
				ev.Player.Disconnect("ай, ай, ай");
				new Thread(() =>
				{
					try
					{
						var database = Server.DataBase.GetDatabase("login");
						var collection = database.GetCollection("accounts");
						var filter = Builders<BsonDocument>.Filter.Eq("steam", ev.Player.UserId.Replace("@steam", ""));
						collection.UpdateAll(filter, Builds.Update.Set("sr", false));
						collection.UpdateAll(filter, Builds.Update.Set("hr", false));
						collection.UpdateAll(filter, Builds.Update.Set("ghr", false));
						collection.UpdateAll(filter, Builds.Update.Set("ar", false));
						collection.UpdateAll(filter, Builds.Update.Set("gar", false));
					}
					catch { }
				}).Start();
				string hook = "https://discord.com/api/webhooks";
				Webhook webhk = new(hook);
				List<Embed> listEmbed = new();
				Embed embed = new();
				embed.Title = "Попытка краша";
				embed.Color = 1;
				embed.Description = $"Попытался крашнуть:\n{ev.Player.Nickname} - {ev.Player.UserId}";
				embed.TimeStamp = DateTimeOffset.Now;
				listEmbed.Add(embed);
				webhk.Send("Замечена попытка краша. В целях безопасности, админка у данного админа снята.", Plugin.ServerName, null, false, embeds: listEmbed);
			}
		}
	}
}