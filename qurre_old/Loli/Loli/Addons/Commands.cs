using UnityEngine;
using Qurre.API.Events;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Objects;
using System.Linq;
using Loli.DataBase.Modules.Controllers;
using System.Collections.Generic;
using System.IO;
using Mirror;
using DateTime = System.DateTime;
using RoundRestarting;
namespace Loli.Addons
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
			CommandsSystem.RegisterConsole("size", Size);
			CommandsSystem.RegisterConsole("key", Key);
			CommandsSystem.RegisterConsole("god", God);
			CommandsSystem.RegisterConsole("exh", Exh);
		}
		private readonly List<string> PromoUsing = new();
		private readonly Dictionary<string, DateTime> GodRateLimits = new();
		private void Help(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			ev.ReturnMessage = $"\n" +
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
				$"\n" +
				$".key <Ключ> — Активировать ключ" +
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
			ev.ReturnMessage = $"TPS: {Plugin.TicksMinutes}";
		}
		private bool ThisAdmin(string userid)
		{
			try
			{
				if (DataBase.Manager.Static.Data.Users.TryGetValue(userid, out var data) &&
					(DataBase.Modules.CustomDonates.ThisYt(data) ||
					data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.owner)) return true;
				else return false;
			}
			catch { return false; }
		}
		private void Size(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (!DataBase.Modules.CustomDonates.ThisYt(ev.Player.UserId)
				&& !ThisAdmin(ev.Player.UserId))
			{
				ev.ReturnMessage = "Отказано в доступе";
				return;
			}
			if (ev.Args.Length < 3)
			{
				ev.ReturnMessage = "Вводите 3 аргумента <x> <y> <z>";
				return;
			}
			if (!float.TryParse(ev.Args[0], out float x) || x < 0.1)
			{
				ev.ReturnMessage = "Неправильно указана координата <x>";
				return;
			}
			if (!float.TryParse(ev.Args[1], out float y) || y < 0.1)
			{
				ev.ReturnMessage = "Неправильно указана координата <y>";
				return;
			}
			if (!float.TryParse(ev.Args[2], out float z) || z < 0.1)
			{
				ev.ReturnMessage = "Неправильно указана координата <z>";
				return;
			}
			var target = ev.Player;
			try
			{
				string name = string.Join(" ", ev.Args.Skip(3));
				if (name.Trim() != "")
				{
					var pl = Player.Get(name);
					if (pl != null) target = pl;
				}
			}
			catch { }
			target.Scale = new(x, y, z);
			ev.ReturnMessage = $"Успешно изменен размер у {target.Nickname}";
		}
		private void Key(SendingConsoleEvent ev)
		{
			ev.Allowed = false;
			if (ev.Args.Count() == 0)
			{
				ev.ReturnMessage = "Введите ключ";
				return;
			}
			if (Round.Waiting)
			{
				ev.ReturnMessage = "Сейчас ожидание игроков";
				return;
			}
			if (ev.Player.GetTeam() == Team.SCP)
			{
				ev.ReturnMessage = "За SCP нельзя использовать ключи";
				return;
			}
			string key = ev.Args[0];
			if (key == "schw3SgVwsdw") ev.ReturnMessage = "Данный ключ уже использовали 5 раз";
			else if (key == "sjHdjkS482JHd")
			{
				if (PromoUsing.Count == 5)
				{
					ev.ReturnMessage = "Данный ключ уже использовали 5 раз";
					return;
				}
				if (PromoUsing.Contains(ev.Player.UserId))
				{
					ev.ReturnMessage = "Вы уже использовали данный ключ";
					return;
				}
				if (ev.Player.AllItems.Count() == 8)
				{
					ev.ReturnMessage = "Ваш инвентарь заполнен";
					return;
				}
				PromoUsing.Add(ev.Player.UserId);
				ev.Player.AddItem(ItemType.Medkit);
				ev.ReturnMessage = "Вы успешно использовали ключ";
			}
			else if (key == "jhSA6sw8w^*wfyT^9sgw")
			{
				if (PromoUsing.Count == 5)
				{
					ev.ReturnMessage = "Данный ключ уже использовали 5 раз";
					return;
				}
				if (PromoUsing.Contains(ev.Player.UserId))
				{
					ev.ReturnMessage = "Вы уже использовали данный ключ";
					return;
				}
				if (ev.Player.AllItems.Count() == 8)
				{
					ev.ReturnMessage = "Ваш инвентарь заполнен";
					return;
				}
				PromoUsing.Add(ev.Player.UserId);
				ev.Player.AddItem(ItemType.KeycardJanitor);
				ev.ReturnMessage = "Вы успешно использовали ключ";
			}
			else ev.ReturnMessage = "Вы ввели недействительный ключ";
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
				case "play":
					{
						if (2 > ev.Args.Count()) return;
						ev.Allowed = false;
						string path = Path.Combine(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio"), string.Join(" ", ev.Args.Skip(1)).Trim());
						Audio.PlayFromFile(path, byte.Parse(ev.Args[0]), true);
						break;
					}
				case "stop":
					{
						ev.Allowed = false;
						Audio.Microphone.StopCapture();
						break;
					}
				case "test2":
					{
						ev.Allowed = false;
						ev.Player.SendConsoleMessage($"{PlayableScps.VisionInformation.VisionLayerMask}", "green");
						ev.Player.SendConsoleMessage($"{LayerMask.NameToLayer("Door")}", "green");
						break;
					}
				case "test3":
					{
						ev.Allowed = false;
						Vector3 pos = ev.Player.Position;
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
				case "test4":
					{
						ev.Allowed = false;
						Door door = Door.Spawn(ev.Player.Position, DoorPrefabs.DoorHCZ);
						MEC.Timing.RunCoroutine(Teleport());
						IEnumerator<float> Teleport()
						{
							for (; ; )
							{
								try { door.Position = ev.Player.Position; } catch { yield break; }
								yield return MEC.Timing.WaitForSeconds(1);
							}
						}
						break;
					}
				case "test5":
					{
						ev.Allowed = false;
						Door door = Door.Spawn(ev.Player.Position - Vector3.right * 2, DoorPrefabs.DoorHCZ);
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
								try { tr.localPosition = ev.Player.Position - door.Position; } catch { yield break; }
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
				case "test6":
					{
						ev.Allowed = false;
						Qurre.Log.Info(ev.Player.Ping);
						ev.ReturnMessage = $"{ev.Player.Ping}";
						break;
					}
				case "test7":
					{
						ev.Allowed = false;
						ev.Player.Reconnect();
						break;
					}
				case "test8":
					{
						ev.Allowed = false;
						ev.Player.Connection.Send(new RoundRestartMessage(RoundRestartType.RedirectRestart, 1f, 7666, true, false));
						break;
					}
				case "test9":
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
				case "test10":
					{
						ev.Allowed = false;
						CustomNetworkManager.TypedSingleton.ServerChangeScene("MainMenuRemastered");
						break;
					}
				case "test11":
					{
						ev.Allowed = false;
						AirDrop.Call();
						break;
					}
				case "test12":
					{
						ev.Allowed = false;
						DataBase.Manager.Static.Stats.AddMoney(ev.Player, int.Parse(ev.Args[0]));
						break;
					}
				case "test13":
					{
						ev.Allowed = false;
						if (ev.Player.TryGetEffect(EffectType.Hypothermia, out CustomPlayerEffects.PlayerEffect playerEffect))
						{
							Qurre.Log.Info(playerEffect.Intensity);
						}
						break;
					}
				case "ztank":
					{
						ev.Allowed = false;
						Qurre.Log.Info("Za Hashux");
						try { Textures.Load._tank.Unload(); } catch { }
						string loc = Path.Combine(Qurre.PluginManager.PluginsDirectory, "Schemes", "ZTank.json");
						Textures.Load._tank = SchematicUnity.API.SchematicManager.LoadSchematic(loc, new(189.84f, 992.4335f, -75.58f), Quaternion.Euler(new(0, -97)));
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