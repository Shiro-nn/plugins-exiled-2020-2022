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
namespace GIVE
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		internal static Dictionary<ReferenceHub, int> giveway = new Dictionary<ReferenceHub, int>();
		internal static bool ula;
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			giveway[ev.Player] = 0;
		}
		internal void RemoteAdminCommand(ref RACommandEvent ev)
		{
			string[] command = ev.Command.Split(' ');
			CommandSender send = ev.Sender;
			ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);
			if (command[0] == "GIVE")
			{
				if (sender.IsvipTagUser() || sender.IsvipplusTagUser())
				{
					ev.Allow = false;
					if (ula) giveway[sender] = 0;
					if (command[2] == "16")
					{
						ev.Sender.RAMessage("Пылесос только 1");
						ev.Allow = false;
						return;
					}
					else if (command[2] == "11")
					{
						ev.Sender.RAMessage("Черная карта-слишком");
						ev.Allow = false;
						return;
					}
					else if (sender.GetRole() == RoleType.ClassD)
					{
						if (command[2] == "13" || command[2] == "17" || command[2] == "20" || command[2] == "21" || command[2] == "23" || command[2] == "24" || command[2] == "25" || command[2] == "30" || command[2] == "31" || command[2] == "32" || command[2] == "6" || command[2] == "7" || command[2] == "8" || command[2] == "9" || command[2] == "10")
						{
							if (!RoundSummary.RoundInProgress() || (double)180 >= (double)(float)RoundSummary.roundTime)
							{
								ev.Sender.RAMessage("3 минуты не прошло");
								ev.Allow = false;
								return;
							}
						}
					}
					else if (sender.GetRole() == RoleType.Scientist)
					{
						if (command[2] == "13" || command[2] == "17" || command[2] == "20" || command[2] == "21" || command[2] == "23" || command[2] == "24" || command[2] == "25" || command[2] == "30" || command[2] == "31" || command[2] == "32" || command[2] == "6" || command[2] == "7" || command[2] == "8" || command[2] == "9" || command[2] == "10")
						{
							if (!RoundSummary.RoundInProgress() || (double)300 >= (double)(float)RoundSummary.roundTime)
							{
								ev.Sender.RAMessage("5 минут не прошло");
								ev.Allow = false;
								return;
							}
						}
					}
					else if (giveway[sender] == 4)
					{
						ev.Sender.RAMessage("Вы уже выдали 3 предмета");
						ev.Allow = false;
						return;
					}
					Timing.CallDelayed(0.4f, () => GameCore.Console.singleton.TypeCommand($"/GIVE {command[1]}. {command[2]}"));
					ev.Sender.RAMessage("Успешно");
					giveway[sender]++;
					ula = false;
				}
				else if (sender.IsstaTagUser())
				{
					ev.Allow = false;
					if (command[2] == "16")
					{
						ev.Sender.RAMessage("Пылесос только 1");
						ev.Allow = false;
						return;
					}
					else if (command[2] == "11")
					{
						ev.Sender.RAMessage("Черная карта-слишком");
						ev.Allow = false;
						return;
					}
					GameCore.Console.singleton.TypeCommand($"/GIVE {command[1]}. {command[2]}");
					ev.Sender.RAMessage("Успешно");
				}
			}
		}
	}
}
