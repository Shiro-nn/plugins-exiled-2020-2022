using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace auto_donate
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public void PlayerJoin(VerifiedEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			spawnpref(ev.Player);
		}
		public void spawnpref(Player pl)
		{
			string web_name = pl.UserId.Replace("@steam", "").Replace("@discord", "");
			var database = Plugin.Client.GetDatabase("auto_donate");
			var collection = database.GetCollection<BsonDocument>("donates");
			var list = collection.Find(new BsonDocument("owner", web_name)).ToList();
			foreach (var document in list)
			{
				if ((int)document["server"] == plugin.config.ServerID)
					pl.ReferenceHub.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup(Role($"{document["id"]}")), false, true, false);
			}
		}
		private string Role(string id)
		{
			string prfx = plugin.config.Donates;
			if (prfx.Contains(','))
			{
				var array = prfx.Split(',');
				foreach (string txt in array.Where(x => x.Contains(':')))
				{
					try
					{
						var array2 = txt.Split(':');
						if (array2[0] == id) return array2[1];
					}
					catch { }
				}
			}
			else if (prfx.Contains(':'))
			{
				try
				{
					var array2 = prfx.Split(':');
					if (array2[0] == id) return array2[1];
				}
				catch { }
			}
			return "";
		}
		internal void Waiting()
		{
			plugin.cfg1();
		}
		public static NetworkStream ss;
		internal void SendPlayers()
		{
			for(; ; )
			{
				Thread.Sleep(5000);
				try
				{
					TcpClient stcp = new TcpClient();
					stcp.Connect(plugin.config.WebIp, 421);
					ss = stcp.GetStream();
					string name = plugin.config.ServerName;
					int players = Player.List.ToList().Count;
					int maxplay = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
					string str = $"online=;={name}=;={players}=;={maxplay}=;={Server.IpAddress}=;={Server.Port}";
					byte[] ba = Encoding.UTF8.GetBytes(str);
					ss.Write(ba, 0, ba.Length);
					stcp.Close();
				}
				catch { }
			}
		}
	}
}