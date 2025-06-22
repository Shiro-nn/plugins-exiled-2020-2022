using MongoDB.Bson;
using MongoDB.Driver;
using Qurre;
using Qurre.API;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace auto_donate
{
	public class EventHandlers
	{
		internal void PlayerJoin(JoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId)) return;
			GetDonate(ev.Player);
		}
		public static Dictionary<string, string> Donates = new Dictionary<string, string>();
		private void GetDonate(Player pl)
		{
			string web_name = pl.UserId.Replace("@steam", "").Replace("@discord", "");
			var database = Plugin.Client.GetDatabase("auto_donate");
			var collection = database.GetCollection<BsonDocument>("donates");
			var list = collection.Find(new BsonDocument("owner", web_name)).ToList();
			foreach (var document in list)
			{
				Log.Info($"Донатер - {pl.Nickname}; Сервер привилегии - {document["server"]}; Данный сервер - {Configs.ServerID}; " +
					$"Подходит ли к данному серверу - {(int)document["server"] == Configs.ServerID}; Роль доната - {Role($"{document["id"]}")}");
				if ((int)document["server"] == Configs.ServerID)
				{
					if (Donates.ContainsKey(pl.UserId)) Donates.Remove(pl.UserId);
					var group = Role($"{document["id"]}");
					Donates.Add(pl.UserId, group);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(pl.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(pl.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(pl.UserId, group);
					pl.Group = ServerStatic.GetPermissionsHandler().GetGroup(group);
				}
			}
		}
		private string Role(string id)
		{
			string prfx = Configs.Donates;
			foreach (string txt in prfx.Split(',').Where(x => x.Contains(':')))
			{
				try
				{
					var array2 = txt.Split(':');
					if (array2[0] == id) return array2[1];
				}
				catch { }
			}
			return "";
		}
		internal void Waiting()
        {
			Donates.Clear();
			Configs.Reload();
		}
		internal void SendPlayers()
		{
			for(; ; )
			{
				Thread.Sleep(5000);
				try
				{
					TcpClient stcp = new TcpClient();
					stcp.Connect(Configs.WebIp, 421);
					var ss = stcp.GetStream();
					string name = Configs.ServerName;
					int players = Player.List.ToList().Count;
					int maxplay = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
					string str = $"online=;={name}=;={players}=;={maxplay}=;={Server.Ip}=;={Server.Port}";
					byte[] ba = Encoding.UTF8.GetBytes(str);
					ss.Write(ba, 0, ba.Length);
					stcp.Close();
				}
				catch { }
			}
		}
	}
}