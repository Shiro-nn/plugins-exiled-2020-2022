using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using PlayerXP.events.hideandseek.API;
using System.Collections.Generic;
using System.Linq;

namespace PlayerXP.force
{
    public class forceclass
	{
		private readonly Plugin plugin;
		public forceclass(Plugin plugin) => this.plugin = plugin;
		internal static bool sdo = false;
		public void OnRoundStart()
		{
			sdo = true;
			bool fdgf = false;
			Timing.CallDelayed(90f, () =>
			{
				if (!fdgf)
				{
					fdgf = true;
					sdo = false;
				}
			});
		}
		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (sdo)
			{
				bool sdgsf = false;
				Timing.CallDelayed(3f, () =>
				{
					if (!sdgsf)
					{
						sdgsf = true;
						ev.Player.ReferenceHub.characterClassManager.SetClassID(RoleType.ClassD);
					}
				});
			}
		}
		public void OnPlayerSpawn(SpawningEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			if (hasData.Gethason()) return;
			if (ev.RoleType == RoleType.FacilityGuard)
			{
				ev.Player.ReferenceHub.Broadcast("<color=lime>Вы можете стать <color=#fdffbb>Ученым</color><color=#00ffff>,</color> написав <color=#0089c7>.</color><color=#ff0>s</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>", 10);
			}
			if (ev.RoleType == RoleType.Scp049 || ev.RoleType == RoleType.Scp079 || ev.RoleType == RoleType.Scp096 || ev.RoleType == RoleType.Scp106 || ev.RoleType == RoleType.Scp173 || ev.RoleType == RoleType.Scp93953 || ev.RoleType == RoleType.Scp93989)
			{
				ev.Player.ReferenceHub.Broadcast("<color=lime>Вы можете изменить своего <color=red>SCP</color><color=#00ffff>,</color> написав <color=#0089c7>.</color><color=#ff0>force</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>", 10);
			}
		}
		public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
		{
			try
			{
				if (ev.Player.Role == RoleType.FacilityGuard)
				{
					if (Round.ElapsedTime.Minutes == 0)
					{
						if (ev.Name == "s")
						{
							ev.Player.ClearBroadcasts();
							ev.Player.SetRole(RoleType.Scientist);
						}
					}
					else
					{
						if (ev.Name == "s")
						{
							ev.ReturnMessage = "\n" +
							"Прошло более одной минуты";
							ev.Color = "red";
						}
					}
				}
				if (ev.Player.Team == Team.SCP)
				{
					if (Round.ElapsedTime.Minutes == 0)
					{
						string effort = ev.Name;
						foreach (string s in ev.Arguments)
							effort += $" {s}";
						string[] args = effort.Split(' ');
						string cmd = ev.Name.ToLower();
						if (cmd.StartsWith("force"))
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
									" SCP049 -\n .force 049\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP079 -\n .force 079\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP096 -\n .force 096\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP106 -\n .force 106\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP173 -\n .force 173\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP939 -\n .force 939\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n";

								ev.Color = "red";
							}
						}
					}
					else
					{
						if (ev.Name == "force")
						{
							ev.ReturnMessage = "\n" +
								"Прошло более одной минуты";
							ev.Color = "red";
						}
					}
				}
				else
				{
					if (ev.Name == "force")
					{
						ev.ReturnMessage = "\n" +
							"Вы не SCP";
						ev.Color = "red";
					}
				}
			}
			catch
			{ }
			try
			{
				string cmd = ev.Name;
				if (cmd.StartsWith("sr"))
				{
					if (ev.Player.UserId == "-@steam")
					{
						bool goorno = false;
						ev.IsAllowed = false;
						PlayerManager.localPlayer.GetComponent<PlayerStats>()?.Roundrestart();
						Timing.CallDelayed(1.5f, () =>
						{
							if (!goorno)
							{
								goorno = true;
								GameCore.Console.singleton.TypeCommand($"restart");
								RemoteAdmin.QueryProcessor QueryProcessor = new RemoteAdmin.QueryProcessor();
								QueryProcessor.GCT.SendToServer("restart");
							}
						});
					}
				}
			}
			catch
			{
				ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
				ev.Color = "red";
			}
		}
	}
}
