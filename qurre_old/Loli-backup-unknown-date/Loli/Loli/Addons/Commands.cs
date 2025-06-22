using UnityEngine;
using Qurre.API.Events;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Objects;
using System.Linq;
using Loli.DataBase.Modules.Controllers;
using System.Collections.Generic;
using Utils.Networking;
using System.IO;
using Loli.Addons.RolePlay.Roles;
using Mirror;
using DateTime = System.DateTime;
namespace Loli.Addons
{
	public class Commands
	{
		private readonly List<string> PromoUsing = new();
		private readonly Dictionary<string, DateTime> GodRateLimits = new();
		public void Console(SendingConsoleEvent ev)
		{
			try
			{
				if (ev.Name == "help" || ev.Name == "хелп" || ev.Name == "хэлп")
				{
					ev.Allowed = false;
					ev.ReturnMessage = $"\n" +
						$".help / .хелп / .хэлп — команда помощи" +
						$"\n\n" +
						$"Валюта:" +
						$"\n" +
						$".money / .мани / .баланс — посмотреть ваш баланс. Например: .money" +
						$"\n" +
						$".pay / .пей / .пэй — передать монеты игроку. Например: .pay 10 hmm" +
						$"\n\n" +
						$"Крюк-кошка:" +
						$"\n" +
						$".cat_hook run — использовать крюк-кошку" +
						$"\n" +
						$".cat_hook drop — выкинуть крюк-кошку" +
						$"\n" +
						$".cat_hook info — информация о крюк-кошке" +
						$"\n\n" +
						$"Другое:" +
						$"\n" +
						$".kill — помереть" +
						$"\n" +
						$".pocket — войти в карманное измерение(только для SCP 106)" +
						$"\n" +
						$".s — стать ученым, будучи охраной" +
						$"\n" +
						$".force — стать другим scp, будучи scp\nНапример: .force 106" +
						$"\n" +
						$".079 — список команд SCP 079" +
						$"\n" +
						$".key <Ключ> — активировать ключ" +
						$"\n\n" +
						$"Общение:" +
						$"\n" +
						$".chat / .чат — текстовой чат" +
						$"\n\n" +
						$"Обратная связь:" +
						$"\n" +
						$".bug / .баг <сообщение> — отправить баг";
					ev.Color = "green";
				}
				else if (ev.Name == "kill")
				{
					ev.Allowed = false;
					if (ev.Player.Role == RoleType.ClassD)
					{
						string tag = " NotForce";
						ev.Player.Tag += tag;
						MEC.Timing.CallDelayed(1f, () => ev.Player.Tag.Replace(tag, ""));
					}
					ev.Player.Kill("Вскрыты вены");
				}
				else if (ev.Name == "tps")
				{
					ev.Allowed = false;
					ev.ReturnMessage = $"TPS: {Plugin.TicksMinutes}";
				}
				else if (ev.Name == "key")
				{
					ev.Allowed = false;
					if (Plugin.RolePlay)
					{
						ev.ReturnMessage = "Ключи отключены на РП-серверах";
						return;
					}
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
						if (ev.Player.AllItems.Count == 8)
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
						if (ev.Player.AllItems.Count == 8)
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
				else if (ev.Name == "believe" || ev.Name == "god")
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
				else if (ev.Player.UserId == "-@steam")
				{
					if (ev.Name == "clear")
					{
						foreach (Pickup item in Map.Pickups) item.Destroy();
						foreach (var doll in Map.Ragdolls) doll.Destroy();
					}
					else if (ev.Name == "door")
					{
						foreach (Door door in Map.Doors) Object.Destroy(door.DoorVariant);
					}
					else if (ev.Name == "tesla")
					{
						foreach (Tesla tesla in Map.Teslas) tesla.Destroy();
					}
					else if (ev.Name == "end")
					{
						Round.End();
					}
					else if (ev.Name == "play")
					{
						if (2 > ev.Args.Count()) return;
						ev.Allowed = false;
						string path = Path.Combine(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio"), string.Join(" ", ev.Args.Skip(1)).Trim());
						new Audio(new FileStream(path, FileMode.Open), byte.Parse(ev.Args[0]));
					}
					else if (ev.Name == "fm" || ev.Name == "facilitymanager")
					{
						if (ev.Args.Count() == 0) return;
						ev.Allowed = false;
						FacilityManager.Spawn(Player.Get(string.Join(" ", ev.Args).Trim()));
						ev.ReturnMessage = "Успешно";
					}
					else if (ev.Name == "test")
					{
						var cl = float.Parse(ev.Args[0]);
						Map.Rooms.Find(x => x.Type == RoomType.Surface).LightColor = new Color(cl, cl, cl);
					}
					else if (ev.Name == "test2")
					{
						ClansWars.Manager.Static.Capturing.Map2.Enable();
					}
					else if (ev.Name == "test3")
					{
						var car = GameObject.Find("CICarConcept");
						Object.DestroyImmediate(car, true);
					}
					else if (ev.Name == "test4")
					{
						Priest controller = Priest.Get(ev.Player);
						if (controller is not null) controller.StartCalling();
					}
					else if (ev.Name == "test5")
					{
						ev.Player.SendConsoleMessage($"{PlayableScps.VisionInformation.VisionLayerMask}", "green");
						ev.Player.SendConsoleMessage($"{LayerMask.NameToLayer("Door")}", "green");
					}
					else if (ev.Name == "test6")
					{
						List<Subtitles.SubtitlePart> list = new(1);
						list.Add(new Subtitles.SubtitlePart(Subtitles.SubtitleType.SCP, new string[] { "106" }));
						list.Add(new Subtitles.SubtitlePart(Subtitles.SubtitleType.Custom, new string[] { "Something text" }));
						new Subtitles.SubtitleMessage(list.ToArray()).SendToAuthenticated(0);
					}
					else if (ev.Name == "test7")
					{
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
					}
					else if (ev.Name == "test8")
					{
						ev.Allowed = false;
						ev.ReturnMessage = $"{Map.Rooms.Where(x => x.Type == RoomType.EzVent).Count()}";
					}
					else if (ev.Name == "test9")
					{
						ev.Allowed = false;
						Door door = Door.Spawn(ev.Player.Position, DoorPrefabs.DoorHCZ);
					}
					else if (ev.Name == "test10")
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
					}
					else if (ev.Name == "test11")
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
					}
					else if (ev.Name == "test12")
					{
						new Textures.Models.Mercury(ev.Player);
					}
					else if (ev.Name == "hehe")
					{
						ev.Player.Scp079Controller.Lvl = 5;
						ev.Player.Scp079Controller.MaxEnergy = 200;
						ev.Player.Scp079Controller.Energy = 200;
					}
					else if (ev.Name == "i")
					{
						ev.Player.Invisible = true;
					}
					else if (ev.Name == "n")
					{
						ev.Player.Invisible = false;
					}
				}
			}
			catch
			{
				if (ev.Name == "help" || ev.Name == "хелп" || ev.Name == "хэлп" || ev.Name == "kill" || ev.Name == "tesla" || ev.Name == "door" || ev.Name == "clear")
				{
					ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
					ev.Color = "red";
				}
			}
		}
	}
}