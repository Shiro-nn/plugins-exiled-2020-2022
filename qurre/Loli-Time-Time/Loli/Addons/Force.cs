using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;

namespace Loli.Addons
{
	static class Force
	{
		static Force()
		{
			CommandsSystem.RegisterConsole("s", ConsoleS);
			CommandsSystem.RegisterConsole("force", ConsoleForce);
		}

		static internal List<RoleTypeId> SpawnedSCPs = new();

		[EventMethod(RoundEvents.Waiting)]
		static void Clear()
			=> SpawnedSCPs.Clear();

		[EventMethod(PlayerEvents.Spawn)]
		static void AddToList(SpawnEvent ev)
		{
			if (ev.Role.GetTeam() != Team.SCPs)
				return;

			if (SpawnedSCPs.Contains(ev.Role))
				return;

			SpawnedSCPs.Add(ev.Role);
		}

		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
			if (Round.ElapsedTime.Minutes == 0 && Round.Started)
				Timing.CallDelayed(0.5f, () => ev.Player.RoleInfomation.Role = RoleTypeId.ClassD);
		}

		[EventMethod(PlayerEvents.Spawn)]
		static void Spawn(SpawnEvent ev)
		{
			if (Round.ElapsedTime.Minutes == 0)
			{
				if (ev.Role == RoleTypeId.FacilityGuard)
				{
					ev.Player.Client.Broadcast(10, "<color=lime>Вы можете стать <color=#fdffbb>Ученым</color><color=#00ffff>,</color> " +
						"написав <color=#0089c7>.</color><color=#ff0>s</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>");
				}
				if (ev.Role.GetTeam() == Team.SCPs)
				{
					ev.Player.Client.Broadcast(10, "<color=lime>Вы можете изменить своего <color=red>SCP</color><color=#00ffff>," +
						"</color> написав <color=#0089c7>.</color><color=#ff0>force</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>");
				}
			}
		}

		static void ConsoleS(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.RoleInfomation.Role is not RoleTypeId.FacilityGuard)
			{
				ev.Reply = "Данная команда доступна только для Охраны Комплекса";
				ev.Color = "red";
				return;
			}
			if (Round.ElapsedTime.TotalMinutes > 1)
			{
				ev.Reply = "Прошло более одной минуты";
				ev.Color = "red";
				return;
			}
			ev.Player.RoleInfomation.Role = RoleTypeId.Scientist;
			ev.Reply = "Успешно";
			ev.Color = "green";
		}

		static void ConsoleForce(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.RoleInfomation.Team is not Team.SCPs)
			{
				ev.Reply = "Данная команда доступна только для SCP";
				ev.Color = "red";
				return;
			}
			if (ev.Player.RoleInfomation.Role == RoleTypeId.Scp0492)
			{
				ev.Reply = "\n" +
				"Увы, но данная команда недоступна для SCP 049-2.";
				ev.Color = "red";
				return;
			}
			if (Round.ElapsedTime.TotalMinutes > 2)
			{
				ev.Reply = "Прошло более 2-х минут";
				ev.Color = "red";
				return;
			}
			switch (ev.Args[0])
			{
				case "173":
					{
						RoleTypeId type = RoleTypeId.Scp173;

						if (SpawnedSCPs.Contains(type))
						{
							ev.Reply = "Данный SCP уже был в раунде";
							ev.Color = "red";
							return;
						}

						ev.Player.RoleInfomation.Role = type;
						ev.Reply = "Успешно";
						ev.Color = "green";
						return;
					}
				case "106":
					{
						RoleTypeId type = RoleTypeId.Scp106;

						if (SpawnedSCPs.Contains(type))
						{
							ev.Reply = "Данный SCP уже был в раунде";
							ev.Color = "red";
							return;
						}

						ev.Player.RoleInfomation.Role = type;
						ev.Reply = "Успешно";
						ev.Color = "green";
						return;
					}
				case "049":
					{
						RoleTypeId type = RoleTypeId.Scp049;

						if (SpawnedSCPs.Contains(type))
						{
							ev.Reply = "Данный SCP уже был в раунде";
							ev.Color = "red";
							return;
						}

						ev.Player.RoleInfomation.Role = type;
						ev.Reply = "Успешно";
						ev.Color = "green";
						return;
					}
				case "079":
					{
						RoleTypeId type = RoleTypeId.Scp079;

						if (SpawnedSCPs.Contains(type))
						{
							ev.Reply = "Данный SCP уже был в раунде";
							ev.Color = "red";
							return;
						}

						ev.Player.RoleInfomation.Role = type;
						ev.Reply = "Успешно";
						ev.Color = "green";
						return;
					}
				case "096":
					{
						RoleTypeId type = RoleTypeId.Scp096;

						if (SpawnedSCPs.Contains(type))
						{
							ev.Reply = "Данный SCP уже был в раунде";
							ev.Color = "red";
							return;
						}

						ev.Player.RoleInfomation.Role = type;
						ev.Reply = "Успешно";
						ev.Color = "green";
						return;
					}
				case "939":
					{
						RoleTypeId type = RoleTypeId.Scp939;

						if (SpawnedSCPs.Contains(type))
						{
							ev.Reply = "Данный SCP уже был в раунде";
							ev.Color = "red";
							return;
						}

						ev.Player.RoleInfomation.Role = type;
						ev.Reply = "Успешно";
						ev.Color = "green";
						return;
					}
				default:
					{
						ev.Reply = "\n" +
							" Не удалось найти доступную роль\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP049 -\n.force 049\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP079 -\n.force 079\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP096 -\n.force 096\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP106 -\n.force 106\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP173 -\n.force 173\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
							" SCP939 -\n.force 939\n~-~-~-~-~-~-~-~-~-~-~-~-~-~";
						ev.Color = "red";
						return;
					}

			}
		}
	}
}