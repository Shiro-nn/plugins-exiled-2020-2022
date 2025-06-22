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
			for(; ; )
			{
				try
				{
					if (Plugin.ClansWars) return;
					int players = Player.List.ToList().Count;
					int maxplay = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
					Addons.NetSocket.Send($"online=;={Plugin.ServerName}=;={players}=;={maxplay}=;={ServerConsole.Ip}=;={Server.Port}=;={Plugin.ServerID}=;=");
				}
				catch { }
				Thread.Sleep(1000);
			}
		}
		public static void SendRa(string cdata, Status status = Status.Standart)
		{
			try
			{
				string mode = "ral";
				if(status == Status.Donate) mode = "radl";
				else if (status == Status.YouTube) mode = "rayl";
				Addons.NetSocket.Send($"{mode}=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata.Replace(":keyboard:", "⌨️")}=;=" +
                    $"{Plugin.ServerName}=;={ServerConsole.Port}=;=" +
                    $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}=;=");
			}
			catch { }
		}
		public enum Status : byte
		{
			Standart,
			Donate,
			YouTube,
		}
	}
}