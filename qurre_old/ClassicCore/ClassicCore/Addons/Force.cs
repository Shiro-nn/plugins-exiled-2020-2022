using MEC;
using Qurre.API;
using Qurre.API.Events;
namespace ClassicCore.Addons
{
	internal class Force
	{
		internal Force()
		{
			CommandsSystem.RegisterConsole("s", ConsoleS);
			CommandsSystem.RegisterConsole("force", ConsoleForce);
		}
		static internal void Join(JoinEvent ev)
		{
			if (Round.ElapsedTime.Minutes == 0 && Round.Started) Timing.CallDelayed(0.5f, () => ev.Player.Role = RoleType.ClassD);
		}
		static internal void Spawn(SpawnEvent ev)
		{
			if (Round.ElapsedTime.Minutes == 0)
			{
				if (ev.RoleType == RoleType.FacilityGuard)
				{
					ev.Player.Broadcast(10, "<color=lime>Вы можете стать <color=#fdffbb>Ученым</color><color=#00ffff>,</color> " +
						"написав <color=#0089c7>.</color><color=#ff0>s</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>");
				}
				if (ev.RoleType.GetTeam() == Team.SCP)
				{
					ev.Player.Broadcast(10, "<color=lime>Вы можете изменить своего <color=red>SCP</color><color=#00ffff>," +
						"</color> написав <color=#0089c7>.</color><color=#ff0>force</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>");
				}
			}
		}
		private void ConsoleS(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.Role is not RoleType.FacilityGuard)
			{
				ev.ReturnMessage = "Данная команда доступна только для Охраны Комплекса";
				ev.Color = "red";
				return;
			}
			if (Round.ElapsedTime.TotalMinutes > 1)
			{
				ev.ReturnMessage = "Прошло более одной минуты";
				ev.Color = "red";
				return;
			}
			ev.Player.Role = RoleType.Scientist;
			ev.ReturnMessage = "Успешно";
			ev.Color = "green";
		}
		private void ConsoleForce(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.Team is not Team.SCP)
			{
				ev.ReturnMessage = "Данная команда доступна только для SCP";
				ev.Color = "red";
				return;
			}
			if (ev.Player.Role == RoleType.Scp0492)
			{
				ev.ReturnMessage = "\n" +
				"Увы, но данная команда недоступна для SCP 049-2.";
				ev.Color = "red";
				return;
			}
			if (Round.ElapsedTime.TotalMinutes > 2)
			{
				ev.ReturnMessage = "Прошло более 2-х минут";
				ev.Color = "red";
				return;
			}
			try
			{
				if (ev.Player.Scp106Controller.Is106 && ev.Player.Scp106Controller.UsingPortal)
				{
					ev.ReturnMessage = "Ай, ай, ай, багоюзер.";
					ev.Color = "red";
					return;
				}
			}
			catch { }
			switch (ev.Args[0])
			{
				case "173":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp173))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp173);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "106":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp106))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						if (DataBase.Manager.Static.Data.Contain)
						{
							ev.ReturnMessage = "Условия содержания SCP-106 уже восстановлены";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp106);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "049":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp049))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp049);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "079":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp079))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp079);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "096":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp096))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp096);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "939":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp93953 or RoleType.Scp93989))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp93989);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "93953" or "939-53":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp93953))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp93953);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				case "93989" or "939-89":
					{
						if (Player.List.TryFind(out _, x => x.Role is RoleType.Scp93989))
						{
							ev.ReturnMessage = "Данный SCP уже имеется в раунде";
							ev.Color = "red";
							return;
						}
						ev.Player.SetRole(RoleType.Scp93989);
						ev.ReturnMessage = "Успешно";
						ev.Color = "green";
						break;
					}
				default:
					{
						ev.ReturnMessage = "\n" +
							" Не удалось найти доступную роль\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP049 -\n.force 049\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP079 -\n.force 079\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP096 -\n.force 096\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP106 -\n.force 106\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP173 -\n.force 173\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP939 -\n.force 939\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP939-53 -\n.force 93953\n.force 939-53\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP939-89 -\n.force 93989\n.force 939-89\n~-~-~-~-~-~-~-~-~-~-~-~-~-~";
						ev.Color = "red";
						break;
					}

			}
		}
	}
}