using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace force
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public void OnConsoleCommand(Exiled.Events.EventArgs.SendingConsoleCommandEventArgs ev)
		{
			try
			{
				string effort = ev.Name;
				foreach (string s in ev.Arguments)
					effort += $" {s}";
				string[] args = effort.Split(' ');
				string cmd = ev.Name.ToLower();
				if (cmd.StartsWith("bc"))
				{
					string text = "";
					foreach (string s in ev.Arguments)
						text += $" {s}";
					Map.Broadcast(10, $"<color=red><b>{ev.Player.Nickname}</b></color> <color=#00ffff><b>RP чат</b></color> {text}");
				}
				if (cmd.StartsWith("forceclass"))
				{
					ev.Allow = false;
					if (args[1] == "173")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp173 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.Scp173);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
							ev.Color = "red";
						}
					}
					else if (args[1] == "d-class" || args[1] == "d")
					{
						ev.Player.SetRole(RoleType.ClassD);
						ev.ReturnMessage = "\nУспешно!";
						ev.Color = "green";
					}
					else if (args[1] == "106")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.Scp106);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
							ev.Color = "red";
						}
					}
					else if (args[1] == "049")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.Scp049);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
							ev.Color = "red";
						}
					}
					else if (args[1] == "scientist" || args[1] == "s")
					{
						ev.Player.SetRole(RoleType.Scientist);
						ev.ReturnMessage = "\nУспешно!";
						ev.Color = "green";
					}
					else if (args[1] == "079")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.Scp079);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
							ev.Color = "red";
						}
					}
					else if (args[1] == "chaosinsurgency" || args[1] == "ci")
					{
						ev.Player.SetRole(RoleType.ChaosInsurgency);
						ev.ReturnMessage = "\nУспешно!";
						ev.Color = "green";
					}
					else if (args[1] == "096")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp096 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.Scp096);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
							ev.Color = "red";
						}
					}
					else if (args[1] == "spectator")
					{
						ev.Player.SetRole(RoleType.Spectator);
						ev.ReturnMessage = "\nУспешно!";
						ev.Color = "green";
					}
					else if (args[1] == "ntfcommander" || args[1] == "nc" || args[1] == "commander")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfCommander && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.NtfCommander);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nОн уже есть в раунде";
							ev.Color = "red";
						}
					}
					else if (args[1] == "cadet" || args[1] == "ntfcadet")
					{
						ev.Player.SetRole(RoleType.NtfCadet);
						ev.ReturnMessage = "\nУспешно!";
						ev.Color = "green";
					}
					else if (args[1] == "facilityguard" || args[1] == "guard" || args[1] == "g")
					{
						ev.Player.SetRole(RoleType.FacilityGuard);
						ev.ReturnMessage = "\nУспешно!";
						ev.Color = "green";
					}
					else if (args[1] == "939")
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => (x.characterClassManager.CurClass == RoleType.Scp93953 || x.characterClassManager.CurClass == RoleType.Scp93989) && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
						if (pList.Count == 0)
						{
							ev.Player.SetRole(RoleType.Scp93989);
							ev.ReturnMessage = "\nУспешно!";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
							ev.Color = "red";
						}
					}
					else
					{
                        ev.ReturnMessage = "\n" +
							" hmm, не удалось найти нужную роль\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP049 -\n .forceclass 049\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP079 -\n .forceclass 079\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP096 -\n .forceclass 096\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP106 -\n .forceclass 106\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP173 -\n .forceclass 173\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP939 -\n .forceclass 939\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Наблюдатель -\n .forceclass spectator\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Сотрудник класса D -\n .forceclass d-class\n .forceclass d\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Хаос -\n .forceclass chaosinsurgency\n .forceclass ci\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Научный сотрудник -\n .forceclass scientist\n .forceclass s\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Охрана -\n .forceclass guard\n .forceclass facilityguard\n .forceclass g\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Кадет -\n .forceclass ntfcadet\n .forceclass cadet\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" Начальник Службы Безопасности -\n .forceclass ntfcommander\n .forceclass commander\n .forceclass nc\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n";

						ev.Color = "red";
					}
				}
			}
			catch
			{
				string cmd = ev.Name.ToLower();
				if (cmd.StartsWith("forceclass"))
				{
					ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
					ev.Color = "red";
				}
			}
		}
	}
}
