using UnityEngine;
using Qurre.API.Events;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using System.Linq;
using System.Collections.Generic;
using Mirror;
using DateTime = System.DateTime;
using RoundRestarting;
using ClassicCore.DataBase.Modules.Controllers;
namespace ClassicCore.Addons
{
	public class Commands
	{
		internal Commands()
		{
			CommandsSystem.RegisterConsole("help", Help);
			CommandsSystem.RegisterConsole("хелп", Help);
			CommandsSystem.RegisterConsole("хэлп", Help);

			CommandsSystem.RegisterConsole("kill", Suicide);
			CommandsSystem.RegisterConsole("tps", TPS);
			CommandsSystem.RegisterConsole("god", God);
			CommandsSystem.RegisterConsole("exh", Exh);

			Qurre.Events.Server.SendingConsole += Console;
		}
		private readonly Dictionary<string, DateTime> GodRateLimits = new();
		private void Help(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			ev.ReturnMessage = $"\n" +
				$".help / .хелп / .хэлп — Команда помощи" +
				$"\n\n" +
				$"Валюта:" +
				$"\n" +
				$".xp — Посмотреть ваш опыт. Например: .xp" +
				$"\n" +
				$".pay / .пей / .пэй — Передать монеты игроку. Например: .pay 10 hmm" +
				$"\n\n" +
				$"Другое:" +
				$"\n" +
				$".kill — Суицид не выход" +
				$"\n" +
				$".s — Стать ученым, будучи охраной" +
				$"\n" +
				$".force — Стать другим SCP, будучи другим SCP\nНапример: .force 106" +
				$"\n" +
				$".079 — Список команд SCP 079" +
				$"\n\n" +
				$"Общение:" +
				$"\n" +
				$".chat / .чат — Текстовой чат" +
				$"\n\n" +
				$"Обратная связь:" +
				$"\n" +
				$".bug / .баг <сообщение> — Отправить баг\nНапример: .баг Я дед инсайд";
			ev.Color = "white";
		}
		private void Suicide(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.Role is RoleType.ClassD)
			{
				string tag = " NotForce";
				ev.Player.Tag += tag;
				MEC.Timing.CallDelayed(1f, () => ev.Player.Tag.Replace(tag, ""));
			}
			ev.Player.Kill("Вскрыты вены");
		}
		private void TPS(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			ev.ReturnMessage = $"TPS: {Init.TicksMinutes}";
		}
		private void God(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (GodRateLimits.TryGetValue(ev.Player.UserId, out var rate))
			{
				if ((DateTime.Now - rate).TotalSeconds < 1)
				{
					ev.ReturnMessage = "Использовать данную команду можно раз в секунду";
					return;
				}
				GodRateLimits[ev.Player.UserId] = DateTime.Now;
			}
			else GodRateLimits.Add(ev.Player.UserId, DateTime.Now);
			if (ev.Player.Tag.Contains(Priest.Tag))
			{
				ev.ReturnMessage = "Вы - священник\nВаша цель: Собрать как можно больше последователей";
				return;
			}
			if (ev.Player.Tag.Contains(Beginner.Tag))
			{
				ev.ReturnMessage = "Вы - верующий в истинного бога\nВаша цель: Призвать истинного бога";
				return;
			}
			var __list = Player.List.Where(x => x.UserId != ev.Player.UserId && x.Team != Team.SCP && x.Team != Team.RIP &&
			x.Tag.Contains(Priest.Tag)).OrderBy(x => x.DistanceTo(ev.Player));
			if (ev.Player.Team != Team.SCP && !ev.Player.Tag.Contains(Beginner.Tag) &&
				!ev.Player.Tag.Contains(Priest.Tag) && __list.Where(x => 5 >= x.DistanceTo(ev.Player)).Count() > 0)
			{
				var t = __list.First();
				Priest controller = Priest.Get(t);
				if (controller is not null)
				{
					controller.StartRecruitment(ev.Player);
					ev.ReturnMessage = "Вы успешно уверовали в истинного бога";
				}
				else ev.ReturnMessage = "Попробуйте снова, священнику плохо";
			}
			else
			{
				string str = "";
				if (__list.Count() == 0) str = "Их нет в живых";
				else str = $"До ближайшего - {System.Math.Round(__list.First().DistanceTo(ev.Player))} метров";
				ev.ReturnMessage = $"Вы находитесь слишком далеко от священников\n{str}";
			}
		}
		private void Exh(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			Priest controller = Priest.Get(ev.Player);
			if (controller is null)
			{
				ev.ReturnMessage = "Вы не священник :/";
				ev.Color = "red";
				return;
			}
			controller.StartCalling();
			ev.ReturnMessage = "Начинаем призыв...";
			ev.Color = "white";
		}
		public void Console(SendingConsoleEvent ev)
		{
			if (ev.Player.UserId != "-@steam") return;
			switch (ev.Name)
			{
				case "clear":
					{
						foreach (Pickup item in Map.Pickups) item.Destroy();
						foreach (var doll in Map.Ragdolls) doll.Destroy();
						break;
					}
				case "door":
					{
						foreach (Door door in Map.Doors) Object.Destroy(door.DoorVariant);
						break;
					}
				case "tesla":
					{
						foreach (Tesla tesla in Map.Teslas) tesla.Destroy();
						break;
					}
				case "end":
					{
						Round.End();
						break;
					}
				case "ping":
					{
						ev.Allowed = false;
						Qurre.Log.Info(ev.Player.Ping);
						ev.ReturnMessage = $"{ev.Player.Ping}";
						break;
					}
				case "test1":
					{
						ev.Allowed = false;
						ev.Player.Reconnect();
						break;
					}
				case "test2":
					{
						ev.Allowed = false;
						ev.Player.Connection.Send(new RoundRestartMessage(RoundRestartType.RedirectRestart, 1f, 7666, true, false));
						break;
					}
				case "test3":
					{
						ev.Allowed = false;
						var target = ev.Player;
						try
						{
							string name = string.Join(" ", ev.Args);
							if (name.Trim() != "")
							{
								var pl = Player.Get(name);
								if (pl != null) target = pl;
							}
						}
						catch { }
						Qurre.Log.Info(target.Nickname);
						target.Connection.Send(new SceneMessage
						{
							sceneName = "MainMenuRemastered",
							sceneOperation = SceneOperation.Normal,
							customHandling = false
						});
						break;
					}
				case "hehe":
					{
						ev.Allowed = false;
						ev.Player.Scp079Controller.Lvl = 5;
						ev.Player.Scp079Controller.MaxEnergy = 200;
						ev.Player.Scp079Controller.Energy = 200;
						break;
					}
				case "i":
					{
						ev.Allowed = false;
						ev.Player.Invisible = true;
						break;
					}
				case "n":
					{
						ev.Allowed = false;
						ev.Player.Invisible = false;
						break;
					}
			}
		}
	}
}