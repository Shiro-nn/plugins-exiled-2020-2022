using UnityEngine;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Addons.Items;
using Qurre.API.Objects;
using System.Linq;
using Loli.DataBase.Modules.Controllers;
using System.Collections.Generic;
using System.IO;
using Mirror;
using DateTime = System.DateTime;
using RoundRestarting;
using Qurre.Events.Structs;
using Qurre.API.Attributes;
using Qurre.Events;
using Loli.DataBase.Modules;
using PlayerRoles;
using System;

namespace Loli.Addons
{
	static class Commands
	{
		static Commands()
		{
			CommandsSystem.RegisterConsole("help", Help);
			CommandsSystem.RegisterConsole("хелп", Help);
			CommandsSystem.RegisterConsole("хэлп", Help);

			CommandsSystem.RegisterConsole("kill", Suicide);
			CommandsSystem.RegisterConsole("tps", TPS);
			CommandsSystem.RegisterConsole("size", Size);
			CommandsSystem.RegisterConsole("god", God);
			CommandsSystem.RegisterConsole("exh", Exh);
		}

		static readonly List<string> PromoUsing = new();
		static readonly Dictionary<string, DateTime> GodRateLimits = new();

		static void Help(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Reply = $"\n" +
				$".help / .хелп / .хэлп — Команда помощи" +
				$"\n\n" +
				$"Валюта:" +
				$"\n" +
				$".money / .мани / .баланс — Посмотреть ваш баланс. Например: .money" +
				$"\n" +
				$".pay / .пей / .пэй — Передать монеты игроку. Например: .pay 10 hmm" +
				$"\n\n" +
				$"Другое:" +
				$"\n" +
				$".kill — Суицид не выход" +
				$"\n" +
				$".pocket — Войти в карманное измерение. Только для SCP 106" +
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

		static void Suicide(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.RoleInfomation.Role is RoleTypeId.ClassD)
			{
				string tag = " NotForce";
				ev.Player.Tag += tag;
				MEC.Timing.CallDelayed(1f, () => ev.Player.Tag.Replace(tag, ""));
			}
			ev.Player.HealthInfomation.Kill("Вскрыты вены");
		}

		static void TPS(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Reply = $"TPS: {Core.TicksMinutes}";
			ev.Player.Client.SendConsole($"Альтернативный TPS: {Math.Round(1f / Time.smoothDeltaTime)}", "white");
		}

		static bool ThisAdmin(string userid)
		{
			try
			{
				if (Data.Users.TryGetValue(userid, out var data) &&
					(CustomDonates.ThisYt(data) ||
					data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.owner)) return true;
				else return false;
			}
			catch { return false; }
		}

		static void Size(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (!CustomDonates.ThisYt(ev.Player.UserInfomation.UserId)
				&& !ThisAdmin(ev.Player.UserInfomation.UserId))
			{
				ev.Reply = "Отказано в доступе";
				return;
			}
			if (ev.Args.Length < 3)
			{
				ev.Reply = "Вводите 3 аргумента <x> <y> <z>";
				return;
			}
			if (!float.TryParse(ev.Args[0], out float x) || x < 0.1)
			{
				ev.Reply = "Неправильно указана координата <x>";
				return;
			}
			if (!float.TryParse(ev.Args[1], out float y) || y < 0.1)
			{
				ev.Reply = "Неправильно указана координата <y>";
				return;
			}
			if (!float.TryParse(ev.Args[2], out float z) || z < 0.1)
			{
				ev.Reply = "Неправильно указана координата <z>";
				return;
			}
			var target = ev.Player;
			try
			{
				string name = string.Join(" ", ev.Args.Skip(3));
				if (name.Trim() != "")
				{
					var pl = name.GetPlayer();
					if (pl != null) target = pl;
				}
			}
			catch { }
			target.MovementState.Scale = new(x, y, z);
			ev.Reply = $"Успешно изменен размер у {target.UserInfomation.Nickname}";
		}

		static void God(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (GodRateLimits.TryGetValue(ev.Player.UserInfomation.UserId, out var rate))
			{
				if ((DateTime.Now - rate).TotalSeconds < 1)
				{
					ev.Reply = "Использовать данную команду можно раз в секунду";
					return;
				}
				GodRateLimits[ev.Player.UserInfomation.UserId] = DateTime.Now;
			}
			else GodRateLimits.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
			if (ev.Player.Tag.Contains(Priest.Tag))
			{
				ev.Reply = "Вы - священник\nВаша цель: Собрать как можно больше последователей";
				return;
			}
			if (ev.Player.Tag.Contains(Beginner.Tag))
			{
				ev.Reply = "Вы - верующий в истинного бога\nВаша цель: Призвать истинного бога";
				return;
			}
			var __list = Player.List.Where(x => x.UserInfomation.UserId != ev.Player.UserInfomation.UserId &&
			x.RoleInfomation.Team != Team.SCPs && x.RoleInfomation.Team != Team.Dead &&
			x.Tag.Contains(Priest.Tag)).OrderBy(x => x.DistanceTo(ev.Player));
			if (ev.Player.RoleInfomation.Team != Team.SCPs && !ev.Player.Tag.Contains(Beginner.Tag) &&
				!ev.Player.Tag.Contains(Priest.Tag) && __list.Where(x => 5 >= x.DistanceTo(ev.Player)).Count() > 0)
			{
				var t = __list.First();
				Priest controller = Priest.Get(t);
				if (controller is not null)
				{
					controller.StartRecruitment(ev.Player);
					ev.Reply = "Вы успешно уверовали в истинного бога";
				}
				else ev.Reply = "Попробуйте снова, священнику плохо";
			}
			else
			{
				string str = "";
				if (__list.Count() == 0) str = "Их нет в живых";
				else str = $"До ближайшего - {System.Math.Round(__list.First().DistanceTo(ev.Player))} метров";
				ev.Reply = $"Вы находитесь слишком далеко от священников\n{str}";
			}
		}

		static void Exh(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			Priest controller = Priest.Get(ev.Player);
			if (controller is null)
			{
				ev.Reply = "Вы не священник :/";
				ev.Color = "red";
				return;
			}
			controller.StartCalling();
			ev.Reply = "Начинаем призыв...";
			ev.Color = "white";
		}

		[EventMethod(ServerEvents.GameConsoleCommand)]
		static void Console(GameConsoleCommandEvent ev)
		{
			if (ev.Player.UserInfomation.UserId != "-@steam") return;
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
				case "play":
					{
						if (2 > ev.Args.Count()) return;
						ev.Allowed = false;
						string path = Path.Combine(Path.Combine(Pathes.Plugins, "Audio"), string.Join(" ", ev.Args.Skip(1)).Trim());
						Audio.PlayFromFile(path, byte.Parse(ev.Args[0]), true);
						break;
					}
				case "stop":
					{
						ev.Allowed = false;
						Audio.Microphone.StopCapture();
						break;
					}
				case "test1":
					{
						ev.Allowed = false;
						Vector3 pos = ev.Player.MovementState.Position;
						for (float i = 0; i < 30; i++)
						{
							new Primitive(PrimitiveType.Cube, pos + Vector3.down * (i * 0.1f), Color.red, size: new(0.1f, 0.01f, 0.1f));
						}
						for (int i = 0; i < 30; i++)
						{
							new Primitive(PrimitiveType.Cube, pos + Vector3.up * (i * 0.1f), Color.blue, size: new(0.1f, 0.01f, 0.1f));
						}
						for (int i = 0; i < 30; i++)
						{
							new Primitive(PrimitiveType.Cube, pos + Vector3.left * (i * 0.1f), Color.green, size: new(0.01f, 0.1f, 0.01f));
						}
						for (int i = 0; i < 30; i++)
						{
							new Primitive(PrimitiveType.Cube, pos + Vector3.right * (i * 0.1f), Color.yellow, size: new(0.01f, 0.1f, 0.01f));
						}
						break;
					}
				case "test2":
					{
						ev.Allowed = false;
						Door door = new(ev.Player.MovementState.Position, DoorPrefabs.DoorHCZ);
						MEC.Timing.RunCoroutine(Teleport());
						IEnumerator<float> Teleport()
						{
							for (; ; )
							{
								try { door.Position = ev.Player.MovementState.Position; } catch { yield break; }
								yield return MEC.Timing.WaitForSeconds(1);
							}
						}
						break;
					}
				case "test3":
					{
						ev.Allowed = false;
						Door door = new(ev.Player.MovementState.Position - Vector3.right * 2, DoorPrefabs.DoorHCZ);
						for (int i = 0; i < door.GameObject.transform.childCount; i++)
						{
							Transform child = door.GameObject.transform.GetChild(i);
							if (child.name.Contains("Button")) MEC.Timing.RunCoroutine(Teleport(child));
						}
						MEC.Timing.RunCoroutine(Update());
						IEnumerator<float> Teleport(Transform tr)
						{
							for (; ; )
							{
								try { tr.localPosition = ev.Player.MovementState.Position - door.Position; } catch { yield break; }
								yield return MEC.Timing.WaitForSeconds(1);
							}
						}
						IEnumerator<float> Update()
						{
							yield return MEC.Timing.WaitForSeconds(1);
							for (; ; )
							{
								try
								{
									NetworkServer.UnSpawn(door.GameObject);
									NetworkServer.Spawn(door.GameObject);
								}
								catch { yield break; }
								yield return MEC.Timing.WaitForSeconds(5);
							}
						}
						break;
					}
				case "test4":
					{
						ev.Allowed = false;
						Log.Info(ev.Player.Ping);
						ev.Reply = $"{ev.Player.Ping}";
						break;
					}
				case "test5":
					{
						ev.Allowed = false;
						ev.Player.Reconnect();
						break;
					}
				case "test6":
					{
						ev.Allowed = false;
						ev.Player.Connection.Send(new RoundRestartMessage(RoundRestartType.RedirectRestart, 1f, 7666, true, false));
						break;
					}
				case "test7":
					{
						ev.Allowed = false;
						var target = ev.Player;
						try
						{
							string name = string.Join(" ", ev.Args);
							if (name.Trim() != "")
							{
								var pl = name.GetPlayer();
								if (pl != null) target = pl;
							}
						}
						catch { }
						Log.Info(target.UserInfomation.Nickname);
						target.Connection.Send(new SceneMessage
						{
							sceneName = "MainMenuRemastered",
							sceneOperation = SceneOperation.Normal,
							customHandling = false
						});
						break;
					}
				case "test8":
					{
						ev.Allowed = false;
						CustomNetworkManager.TypedSingleton.ServerChangeScene("MainMenuRemastered");
						break;
					}
				case "test9":
					{
						ev.Allowed = false;
						AirDrop.Call();
						break;
					}
				case "test10":
					{
						ev.Allowed = false;
						Stats.AddMoney(ev.Player, int.Parse(ev.Args[0]));
						break;
					}
				case "ztank":
					{
						ev.Allowed = false;
						Log.Info("Za Hashux");
						try { Textures.Load._tank.Unload(); } catch { }
						string loc = Path.Combine(Pathes.Plugins, "Schemes", "ZTank.json");
						Textures.Load._tank = SchematicUnity.API.SchematicManager.LoadSchematic(loc, new(189.84f, 992.4335f, -75.58f), Quaternion.Euler(new(0, -97)));
						break;
					}
				case "hehe":
					{
						ev.Allowed = false;
						ev.Player.RoleInfomation.Scp079.Lvl = 5;
						ev.Player.RoleInfomation.Scp079.MaxEnergy = 200;
						ev.Player.RoleInfomation.Scp079.Energy = 200;
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