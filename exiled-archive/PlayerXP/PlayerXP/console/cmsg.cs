using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerXP.console
{
	public class cmsg
	{
		private readonly Plugin plugin;
		public cmsg(Plugin plugin) => this.plugin = plugin;
		internal bool roundstartb = false;
		internal bool voitinround = false;
		internal bool voitstart = false;
		internal int hasagree = 0;
		internal int hasdisagree = 0;
		private static Dictionary<string, bool> voitps = new Dictionary<string, bool>();
		internal void roundstart()
		{
			roundstartb = true;
			voitinround = false;
			voitstart = false;
			hasagree = 0;
			hasdisagree = 0;
			voitps.Clear();
		}
		internal void roundend(RoundEndedEventArgs ev)
		{
			roundstartb = false;
			voitinround = false;
			voitstart = false;
			hasagree = 0;
			hasdisagree = 0;
			voitps.Clear();
		}
		public void console(SendingConsoleCommandEventArgs ev)
		{
			try
			{
				string cmd = ev.Name;
				if (ev.Player.UserId == "-@steam")
				{
					if (cmd == "clear")
					{
						foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
							item.Delete();
						foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
						{
							NetworkServer.Destroy(doll.gameObject);
						}
					}
				}
				if (cmd == "kill")
				{
					ev.IsAllowed = false;
					ev.Player.ReferenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(666666, "WORLD", DamageTypes.Bleeding, ev.Player.ReferenceHub.queryProcessor.PlayerId), ev.Player.ReferenceHub.gameObject);
				}
				if (cmd == "help" || cmd == "хелп" || cmd == "хэлп")
				{
					ev.IsAllowed = false;
					ev.ReturnMessage = $"\n" +
						$".help-команда помощи" +
						$"\n\n" +
						$"Валюта:" +
						$"\n\n" +
						$".money-посмотреть ваш баланс\nНапример: .money" +
						$"\n\n" +
						$".pay-передать монеты игроку\nНапример: .pay hmm" +
						$"\n\n" +
						$".хелп-команда помощи" +
						$"\n\n" +
						$".мани-посмотреть баланс\nНапример: .мани" +
						$"\n\n" +
						$".пей-передать монеты игроку\nНапример: .пей hmm" +
						$"\n\n" +
						$".хэлп-команда помощи" +
						$"\n\n" +
						$".баланс-посмотреть баланс\nНапример: .баланс" +
						$"\n\n" +
						$".пэй-передать монеты игроку\nНапример: .пэй hmm" +
						$"\n\n" +
						$"SCP 228 RU J" +
						$"\n\n" +
						$".vodka-посмотреть местонахождение водки(только для SCP 228 RU J)" +
						$"\n\n" +
						$".vodka tp-тп к водке(только для SCP 228 RU J)" +
						$"\n\n" +
						$".водка-посмотреть местонахождение водки(только для SCP 228 RU J)" +
						$"\n\n" +
						$".водка тп-тп к водке(только для SCP 228 RU J)" +
						$"\n\n" +
						$"Другое:" +
						$"\n\n" +
						$".kill-помереть" +
						$"\n\n" +
						$".s-стать ученым, будучи охраной" +
						$"\n\n" +
						$".force-стать другим scp, будучи scp\nНапример: .force 106" +
						$"\n\n" +
						$".has-запустить голосование на прятки" +
						$"\n" +
						$".has yes-проголосовать за прятки" +
						$"\n" +
						$".has no-проголосовать против пряток" +
						$"\n\n" +
						$".mvc-запустить голосование на ивент *mtf vc ci*" +
						$"\n" +
						$".mvc yes-проголосовать за ивент *mtf vc ci*" +
						$"\n" +
						$".mvc no-проголосовать против ивента *mtf vc ci*" +
						$"\n" +
						$".mvc info-узнать про ивент *mtf vc ci*" +
						$"\n\n";
					ev.Color = "cyan";
				}
				if (ev.Name == "has")
				{
					ev.IsAllowed = false;
					if (ev.Arguments.Count() >= 1)
					{
						if (voitstart)
						{
							Log.Info(ev.Arguments[0]);
							string yes = "yes";
							string no = "no";
							if (ev.Arguments[0] == yes)
							{
								if (voitps.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
								{
									ev.ReturnMessage = "\nВы уже голосовали";
									ev.Color = "red";
								}
								else
								{
									hasagree++;
									voitps.Add(ev.Player.ReferenceHub.characterClassManager.UserId, true);
									ev.ReturnMessage = "\nВы успешно проголосовали за.";
									ev.Color = "green";
								}
							}
							else if (ev.Arguments[0] == no)
							{
								if (voitps.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
								{
									ev.ReturnMessage = "\nВы уже голосовали";
									ev.Color = "red";
								}
								else
								{
									hasdisagree++;
									voitps.Add(ev.Player.ReferenceHub.characterClassManager.UserId, true);
									ev.ReturnMessage = "\nВы успешно против.";
									ev.Color = "green";
								}
							}
							else
							{
								ev.ReturnMessage = $"\nИспользуйте:\n.has {yes}\n.has {no}";
								ev.Color = "red";
							}
						}
						else
						{
							ev.ReturnMessage = "\nГолосование не начато";
							ev.Color = "red";
						}
                    }
                    else
					{
						if (!voitstart && !voitinround && roundstartb)
						{
							voitstartv(ev.Player.ReferenceHub);
						}
						else if (voitstart)
						{
							ev.ReturnMessage = "\nГолосование уже начато";
							ev.Color = "red";
                        }
                        else if(voitinround)
						{
							ev.ReturnMessage = "\nГолосование уже было запущено в этом раунде";
							ev.Color = "red";
						}
						else if (!roundstartb)
						{
							ev.ReturnMessage = "\nРаунд не запущен";
							ev.Color = "red";
						}
					}
				}
				if (ev.Name == "mvc")
				{
					ev.IsAllowed = false;
					if (ev.Arguments.Count() >= 1)
					{
						if (ev.Arguments[0] == "info")
						{
							ev.ReturnMessage = "\nmvc-MTF vs CI\nРП ивента:\nПовстанцы Хаоса узнали, что в комплексе находятся ценные объекты, которые надо сохранить, но совет О5 знает о планах ПХ, поэтому высылает все свои силы МОГ, соответственно МОГ должны взорвать АЛЬФА-Боеголовку, а ПХ-помешать, т.к через 15 минут приезжает *сбор ценных предметов*";
							ev.Color = "cyan";
						}
						else if (voitstart)
						{
							string yes = "yes";
							string no = "no";
							if (ev.Arguments[0] == yes)
							{
								if (voitps.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
								{
									ev.ReturnMessage = "\nВы уже голосовали";
									ev.Color = "red";
								}
								else
								{
									hasagree++;
									voitps.Add(ev.Player.ReferenceHub.characterClassManager.UserId, true);
									ev.ReturnMessage = "\nВы успешно проголосовали за.";
									ev.Color = "green";
								}
							}
							else if (ev.Arguments[0] == no)
							{
								if (voitps.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
								{
									ev.ReturnMessage = "\nВы уже голосовали";
									ev.Color = "red";
								}
								else
								{
									hasdisagree++;
									voitps.Add(ev.Player.ReferenceHub.characterClassManager.UserId, true);
									ev.ReturnMessage = "\nВы успешно против.";
									ev.Color = "green";
								}
							}
							else
							{
								ev.ReturnMessage = $"\nИспользуйте:\n.mvc {yes}\n.mvc {no}\n.mvc info";
								ev.Color = "red";
							}
						}
						else
						{
							ev.ReturnMessage = "\nГолосование не начато";
							ev.Color = "red";
						}
						
					}
					else
					{
						if (!voitstart && !voitinround && roundstartb)
						{
							voitstartmvc(ev.Player.ReferenceHub);
						}
						else if (voitstart)
						{
							ev.ReturnMessage = "\nГолосование уже начато";
							ev.Color = "red";
						}
						else if (voitinround)
						{
							ev.ReturnMessage = "\nГолосование уже было запущено в этом раунде";
							ev.Color = "red";
						}
						else if (!roundstartb)
						{
							ev.ReturnMessage = "\nРаунд не запущен";
							ev.Color = "red";
						}
					}
				}
			}
			catch
			{
				ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
				ev.Color = "red";
			}
		}
		internal void voitstartv(ReferenceHub voitowner)
		{
			Map.Broadcast(15, $"<size=20%><color=red>{voitowner.nicknameSync.DisplayName}</color> <color=#00ffff>запросил голосование на проведение пряток</color>\n<color=lime>Если вы согласны, то напишите в консоли на</color> <color=#ff0>[<color=#00ffff>ё</color>]</color>\n<color=#15ff00>.has yes</color>\n<color=#0089c7>Если нет, то</color>\n<color=#ff0000>.has no</color>\n<color=#fdffbb>У вас есть <color=red>60</color> секунд на выбор</color></size>");
			voitinround = true; 
			voitstart = true;
			hasagree = 0;
			hasdisagree = 0;
			voitps.Clear();
			bool end = false;
			Timing.CallDelayed(60f, () =>
			{
				if (!end)
				{
					end = true;
					if (hasagree >= hasdisagree)
					{
						Map.Broadcast(10, $"<i><color=lime>Ура!</color>\n<color=#0089c7>Прятки будут включены</color>\n<color=#15ff00><b>{hasagree}</b> за</color>,<color=#ff0000><b>{hasdisagree}</b> против</color></i>");
						GameCore.Console.singleton.TypeCommand($"/has");
					}
					else
					{
						Map.Broadcast(10, $"<i><color=red>Прятки не будут включены</color>\n<color=#15ff00><b>{hasagree}</b> за</color>,<color=#ff0000><b>{hasdisagree}</b> против</color></i>");
					}
				}
			});
		}
		internal void voitstartmvc(ReferenceHub voitowner)
		{
			Map.Broadcast(15, $"<size=20%><color=red>{voitowner.nicknameSync.DisplayName}</color> <color=#00ffff>запросил голосование на ивент <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color></color>\n<color=lime>Если вы согласны, то напишите в консоли на</color> <color=#ff0>[<color=#00ffff>ё</color>]</color>\n<color=#15ff00>.mvc yes</color>\n<color=#0089c7>Если нет, то</color>\n<color=#ff0000>.mvc no</color>\n<color=#fdffbb>У вас есть <color=red>60</color> секунд на выбор</color>\n<color=red>.<color=#006dff>mvc info</color>-</color><color=#03d2b0>узнать про ивент</color></size>");
			voitinround = true;
			voitstart = true;
			hasagree = 0;
			hasdisagree = 0;
			voitps.Clear();
			bool end = false;
			Timing.CallDelayed(60f, () =>
			{
				if (!end)
				{
					end = true;
					if (hasagree >= hasdisagree)
					{
						Map.Broadcast(10, $"<i><color=lime>Ура!</color>\n<color=#0089c7>Ивент <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color> будет включен</color>\n<color=#15ff00><b>{hasagree}</b> за</color>,<color=#ff0000><b>{hasdisagree}</b> против</color></i>");
						GameCore.Console.singleton.TypeCommand($"/mvc");
					}
					else
					{
						Map.Broadcast(10, $"<i><color=red>Ивент <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color> не будет включен</color>\n<color=#15ff00><b>{hasagree}</b> за</color>,<color=#ff0000><b>{hasdisagree}</b> против</color></i>");
					}
				}
			});
		}
	}
}
