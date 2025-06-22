using Qurre.API;
using System;
using System.Linq;
using System.Threading;
namespace ClassicCore.Logs
{
	public class Api
	{
		public static void SendOnline()
		{
			for (; ; )
			{
				try
				{
					int players = Player.List.Count();
					int maxpls = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
					Init.Socket.Emit("server.online", new object[] { ServerConsole.Ip, Server.Port, Init.ServerName, players, maxpls });
				}
				catch { }
				Thread.Sleep(1000);
			}
		}
		public static void SendRa(string cdata, Status status = Status.Standart)
		{
			try
			{
				Init.Socket.Emit("server.remote-admin", new object[] { (byte)status, Init.ServerName, ServerConsole.Port,
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