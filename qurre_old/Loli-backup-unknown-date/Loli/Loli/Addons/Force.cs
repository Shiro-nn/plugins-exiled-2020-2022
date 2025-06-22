using MEC;
using System.Collections.Generic;
using System.Linq;
using Qurre.API;
using Qurre.API.Events;
namespace Loli.Addons
{
	public class Force
	{
		private readonly Plugin plugin;
		public Force(Plugin plugin) => this.plugin = plugin;
		public GameConsoleTransmission GameConsoleTransmission;
		public void Join(JoinEvent ev)
		{
			if (Plugin.ClansWars) return;
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled || Plugin.RolePlay) return;
			if (Round.ElapsedTime.Minutes == 0 && Round.Started) Timing.CallDelayed(0.5f, () => ev.Player.Role = RoleType.ClassD);
		}
		public void Spawn(SpawnEvent ev)
		{
			if (Plugin.ClansWars) return;
			if (Plugin.RolePlay || AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (Round.ElapsedTime.Minutes == 0)
			{
				if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server")
					return;
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
		public void Console(SendingConsoleEvent ev)
		{
			try
			{
				if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
				if (ev.Name == "s")
				{
					ev.Allowed = false;
					if (Plugin.RolePlay)
					{
						ev.ReturnMessage = "\n" +
						"Данная команда недоступна на РП-серверах.";
						ev.Color = "red";
					}
					else if (Plugin.ClansWars)
					{
						ev.ReturnMessage = "\n" +
						"Данная команда недоступна на сервере клановых войн.";
						ev.Color = "red";
					}
					else if (ev.Player.Role == RoleType.FacilityGuard)
					{
						if (Round.ElapsedTime.Minutes == 0)
						{
							ev.Player.Role = RoleType.Scientist;
							ev.ReturnMessage = "\nУспешно";
							ev.Color = "green";
						}
						else
						{
							ev.ReturnMessage = "\n" +
							"Прошло более одной минуты.";
							ev.Color = "red";
						}
					}
					else
					{
						ev.ReturnMessage = "\n" +
						"Увы, но вы не являетесь охраной.";
						ev.Color = "red";
					}
				}
				else if (ev.Name == "force")
				{
					ev.Allowed = false;
                    if (Plugin.RolePlay)
					{
						ev.ReturnMessage = "\n" +
						"Данная команда недоступна на РП-серверах.";
						ev.Color = "red";
						return;
					}
					else if (Plugin.ClansWars)
					{
						ev.ReturnMessage = "\n" +
						"Данная команда недоступна на сервере клановых войн.";
						ev.Color = "red";
					}
					if (ev.Player.Team == Team.SCP)
					{
						if(ev.Player.Role == RoleType.Scp0492)
						{
							ev.ReturnMessage = "\n" +
							"Увы, но данная команда недоступна для SCP 049-2.";
							ev.Color = "red";
							return;
						}
						if (1 >= Round.ElapsedTime.Minutes)
						{
							try
							{
								if (ev.Player.Scp106Controller.Is106 && ev.Player.Scp106Controller.UsingPortal)
								{
									ev.ReturnMessage = "\nАй, ай, ай, багоюзер.";
									ev.Color = "red";
									return;
								}
							}
							catch { }
							if (ev.Args[0] == "173")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp173 && x.UserId != null && x.UserId != string.Empty).ToList();
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
							else if (ev.Args[0] == "106")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp106 && x.UserId != null && x.UserId != string.Empty).ToList();
								if (pList.Count != 0)
								{
									ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
									ev.Color = "red";
								}
								else if (DataBase.Manager.Static.Data.Contain)
								{
									ev.ReturnMessage = "\nУсловия содержания уже восстановлены";
									ev.Color = "red";
								}
								else
								{
									ev.Player.SetRole(RoleType.Scp106);
									ev.ReturnMessage = "\nУспешно!";
									ev.Color = "green";
								}
							}
							else if (ev.Args[0] == "049")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp049 && x.UserId != null && x.UserId != string.Empty).ToList();
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
							else if (ev.Args[0] == "079")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp079 && x.UserId != null && x.UserId != string.Empty).ToList();
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
							else if (ev.Args[0] == "096")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp096 && x.UserId != null && x.UserId != string.Empty).ToList();
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
							else if (ev.Args[0] == "939")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp93953 || x.Role == RoleType.Scp93989 && x.UserId != null && x.UserId != string.Empty).ToList();
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
							else if (ev.Args[0] == "93953" || ev.Args[0] == "939-53")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp93953 && x.UserId != null && x.UserId != string.Empty).ToList();
								if (pList.Count == 0)
								{
									ev.Player.SetRole(RoleType.Scp93953);
									ev.ReturnMessage = "\nУспешно!";
									ev.Color = "green";
								}
								else
								{
									ev.ReturnMessage = "\nЭтот scp уже есть в раунде";
									ev.Color = "red";
								}
							}
							else if (ev.Args[0] == "93989" || ev.Args[0] == "939-89")
							{
								List<Player> pList = Player.List.Where(x => x.Role == RoleType.Scp93989 && x.UserId != null && x.UserId != string.Empty).ToList();
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
									" SCP049 -\n.force 049\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP079 -\n.force 079\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP096 -\n.force 096\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP106 -\n.force 106\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP173 -\n.force 173\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP939 -\n.force 939\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP939-53 -\n.force 93953\n.force 939-53\n~-~-~-~-~-~-~-~-~-~-~-~-~-~\n" +
									" SCP939-89 -\n.force 93989\n.force 939-89\n~-~-~-~-~-~-~-~-~-~-~-~-~-~";
								ev.Color = "red";
							}
						}
						else
						{
							ev.ReturnMessage = "\n" +
								"Прошло более 2-х минут";
							ev.Color = "red";
						}
					}
					else
					{
						ev.ReturnMessage = "\n" +
						"Увы, но вы не являетесь SCP.";
						ev.Color = "red";
					}
				}
			}
			catch { }
		}
	}
}