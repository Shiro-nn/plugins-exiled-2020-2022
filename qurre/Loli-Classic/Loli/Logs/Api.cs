using Qurre.API;
using System;

namespace Loli.Logs
{
	static class Api
	{
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