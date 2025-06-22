using Qurre.API;
using System;
using System.Linq;
using System.Threading;
namespace Loli.Logs
{
	public class Api
	{
		public static void SendOnline()
		{
			for (; ; )
			{
				try
				{
					if (Plugin.ClansWars) return;
					int players = Player.List.Count();
					int maxpls = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
					Plugin.Socket.Emit("server.online", new object[] { ServerConsole.Ip, Server.Port, Plugin.ServerName, players, maxpls });
				}
				catch { }
				Thread.Sleep(1000);
			}
		}
		public static void SendRa(string cdata, Status status = Status.Standart)
		{
			try
			{
				Plugin.Socket.Emit("server.remote-admin", new object[] { Plugin.HardRP ? 3 : (byte)status, Plugin.ServerName, ServerConsole.Port,
					$"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata.Replace(":keyboard:", "⌨️")}",
					$"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}"
				});
			}
			catch { }
		}
		public enum Status : byte
		{
			Standart,
			Donate
		}
	}
}