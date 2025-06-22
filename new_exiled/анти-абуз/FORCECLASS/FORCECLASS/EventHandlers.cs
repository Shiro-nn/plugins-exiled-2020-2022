using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using Mirror;
using Log = EXILED.Log;
using Grenades;
using System.Text;
using System.Text.RegularExpressions;
namespace FORCECLASS
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		internal static Dictionary<ReferenceHub, int> force = new Dictionary<ReferenceHub, int>();
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			force[ev.Player] = 0;
		}
		internal void RemoteAdminCommand(ref RACommandEvent ev)
		{
			string[] command = ev.Command.Split(' ');
			ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);
			if (command[0] == "FORCECLASS")
			{
				if (sender.IspremiumTagUser() || sender.IsvipplusTagUser())
				{
					ev.Allow = false;
					if (force[sender] != 4)
					{
						ev.Allow = false;
						ev.Sender.RAMessage("Не забывайте, что у вас есть только 3 спавна");
						GameCore.Console.singleton.TypeCommand($"/FORCECLASS {sender.GetPlayerId()}. {command[2]}");
						force[sender]++;
						return;
					}
					ev.Allow = false;
					ev.Sender.RAMessage("Спавниться более трех раз ЗАПРЕЩЕНО");
				}
			}
		}
	}
}
