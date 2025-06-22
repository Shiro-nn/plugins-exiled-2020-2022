using Qurre.API;
using System;
using System.Linq;
using System.Threading;

namespace Loli.Logs
{
	static class Api
	{
		static internal void SendOnline()
		{
			for (; ; )
			{
				try
				{
					int players = Player.List.Count();
					int maxpls = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
					Core.Socket.Emit("server.online", new object[] { ServerConsole.Ip, Core.Port, Core.ServerName, players, maxpls });
				}
				catch { }
				Thread.Sleep(1000);
			}
		}

		static internal void SendRa(string cdata, Status status = Status.Standart)
		{
			try
			{
				Core.Socket.Emit("server.remote-admin", new object[] { (byte)status, Core.ServerName, Server.Port,
					$"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata.Replace(":keyboard:", "⌨️")}",
					$"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}"
				});
			}
			catch { }
		}

		internal enum Status : byte
		{
			Standart,
			Donate
		}
	}
}