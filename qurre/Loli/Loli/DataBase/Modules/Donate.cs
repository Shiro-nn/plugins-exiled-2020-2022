using Loli.Addons;
using Loli.Spawns;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Linq;

namespace Loli.DataBase.Modules
{
	static class Donate
	{
		internal static int DonateLimint => 5;
		static Donate()
		{
			CommandsSystem.RegisterRemoteAdmin("hidetag", AntiHideTag);
		}
		static internal DateTime LastCall = DateTime.Now;
		static internal Faction LastFaction = Faction.Unclassified;
		static void AntiHideTag(RemoteAdminCommandEvent ev)
		{
			ev.Prefix = "HIDETAG";
			ev.Allowed = false;
			ev.Success = false;
			ev.Reply = "Недоступно";
		}

		[EventMethod(ServerEvents.RemoteAdminCommand)]
		static void Ra(RemoteAdminCommandEvent ev)
		{
			switch (ev.Sender.SenderId)
			{
				case "SERVER CONSOLE": return;
				case "GAME CONSOLE": return;
				case "Effects Controller": return;
			}
			switch (ev.Name)
			{
				case "give":
					{
						ev.Prefix = "GIVE";
						Data.Roles.TryGetValue(ev.Sender.SenderId, out var roles);
						if (!(roles.Mage || roles.Sage || roles.Star || (Data.giver.TryGetValue(ev.Sender.SenderId, out var ___) && ___))
							 && !CustomDonates.ThisDonater(ev.Sender.SenderId)) return;
						ev.Allowed = false;
						if (Round.Waiting)
						{
							ev.Reply = "Раунд еще не начался";
							ev.Success = false;
							return;
						}
						if (ev.Player.RoleInfomation.Team == Team.SCPs)
						{
							ev.Reply = "Вы играете за SCP. За SCP нельзя выдавать предметы.";
							ev.Success = false;
							return;
						}
						if (ev.Player.Inventory.ItemsCount == 8)
						{
							ev.Reply = "У вас заполнен инвентарь";
							ev.Success = false;
							return;
						}
						if (!Data.giveway.ContainsKey(ev.Player.UserInfomation.UserId)) Data.giveway.Add(ev.Player.UserInfomation.UserId, 0);
						Data.giveway.TryGetValue(ev.Player.UserInfomation.UserId, out var giver);
						if (giver >= DonateLimint)
						{
							ev.Reply = $"Вы уже выдали {DonateLimint} предметов";
							ev.Success = false;
							return;
						}
						var itemN = -1;
						if (ev.Args.Length > 1)
						{
							try { itemN = Convert.ToInt32(ev.Args[1].Split('.')[0]); } catch { }
						}
						else
						{
							try { itemN = Convert.ToInt32(ev.Args[0].Split('.')[0]); } catch { }
						}
						if (itemN < 0 || itemN > 43)
						{
							ev.Reply = "ID предмета не найден";
							ev.Success = false;
							return;
						}
						ItemType item = (ItemType)itemN;
						if (item == ItemType.Ammo12gauge || item == ItemType.Ammo44cal || item == ItemType.Ammo556x45 ||
							item == ItemType.Ammo762x39 || item == ItemType.Ammo9x19)
						{
							ev.Player.GetAmmo();
							Data.giveway[ev.Player.UserInfomation.UserId]++;
							ev.Reply = "Успешно";
							return;
						}
						if (item == ItemType.MicroHID)
						{
							ev.Reply = "MicroHid только 1";
							ev.Success = false;
							return;
						}
						else if ((ev.Player.RoleInfomation.Role == RoleTypeId.ClassD || ev.Player.RoleInfomation.Role == RoleTypeId.Scientist) &&
								(item == ItemType.KeycardGuard || item == ItemType.KeycardNTFOfficer || item == ItemType.GunCOM15 || item == ItemType.GunCOM18
								|| item == ItemType.KeycardChaosInsurgency || item == ItemType.KeycardContainmentEngineer || item == ItemType.KeycardFacilityManager
								|| item == ItemType.KeycardNTFCommander || item == ItemType.KeycardNTFLieutenant || item == ItemType.KeycardO5 ||
								item == ItemType.SCP018 || item == ItemType.GrenadeHE || item == ItemType.GrenadeFlash) &&
								(3 >= Round.ElapsedTime.TotalMinutes))
						{
							ev.Reply = "3 минуты не прошло";
							ev.Success = false;
							return;
						}
						else if ((ev.Player.RoleInfomation.Role == RoleTypeId.ClassD || ev.Player.RoleInfomation.Role == RoleTypeId.Scientist) &&
								(item == ItemType.GunCrossvec || item == ItemType.GunFSP9 || item == ItemType.GunRevolver) &&
								(4 >= Round.ElapsedTime.TotalMinutes))
						{
							ev.Reply = "4 минуты не прошло";
							ev.Success = false;
							return;
						}
						else if ((ev.Player.RoleInfomation.Role == RoleTypeId.ClassD || ev.Player.RoleInfomation.Role == RoleTypeId.Scientist) && (item == ItemType.GunAK ||
							item == ItemType.GunLogicer || item == ItemType.GunShotgun || item == ItemType.GunE11SR) &&
								(5 >= Round.ElapsedTime.TotalMinutes))
						{
							ev.Reply = "5 минут не прошло";
							ev.Success = false;
							return;
						}
						double CoolDown = 2;
						if (!Data.gives.ContainsKey(ev.Player.UserInfomation.UserId)) Data.gives.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
						else if ((DateTime.Now - Data.gives[ev.Sender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((Data.gives[ev.Sender.SenderId] - DateTime.Now).TotalSeconds);
							ev.Reply = $"Предметы можно выдавать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						ev.Player.Inventory.AddItem(item);
						ev.Reply = "Успешно";
						Data.gives[ev.Sender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						Data.giveway[ev.Player.UserInfomation.UserId]++;
						return;
					}
				case "forceclass":
					{
						ev.Prefix = "FORCECLASS";
						Data.Roles.TryGetValue(ev.Sender.SenderId, out var roles);
						if (!(roles.Priest || roles.Mage || roles.Sage || roles.Star || (Data.forcer.TryGetValue(ev.Sender.SenderId, out var ___) && ___))
							&& !CustomDonates.ThisDonater(ev.Sender.SenderId)) return;
						ev.Allowed = false;
						if (Round.Waiting)
						{
							ev.Reply = "Раунд еще не начался";
							ev.Success = false;
							return;
						}
						if (!Data.force.ContainsKey(ev.Player.UserInfomation.UserId)) Data.force.Add(ev.Player.UserInfomation.UserId, 0);
						Data.force.TryGetValue(ev.Player.UserInfomation.UserId, out var forcer);
						if (forcer >= DonateLimint)
						{
							ev.Reply = $"Вы уже меняли роль {DonateLimint} раз";
							ev.Success = false;
							return;
						}
						RoleTypeId role = RoleTypeId.None;
						try { role = (RoleTypeId)Enum.Parse(typeof(RoleTypeId), ev.Args[1]); } catch { }
						if (role == RoleTypeId.None)
						{
							ev.Reply = $"Произошла ошибка при получении роли. Проверьте правильность команды";
							ev.Success = false;
							return;
						}
						if (Round.ElapsedTime.Minutes == 0 &&
							(role == RoleTypeId.ChaosConscript || role == RoleTypeId.ChaosMarauder || role == RoleTypeId.ChaosRepressor ||
										   role == RoleTypeId.ChaosRifleman || role == RoleTypeId.NtfSpecialist ||
										   role == RoleTypeId.NtfSergeant || role == RoleTypeId.NtfPrivate || role == RoleTypeId.NtfCaptain))
						{
							ev.Reply = $"Подождите {Math.Round(60 - Round.ElapsedTime.TotalSeconds)} секунд.";
							ev.Success = false;
							return;
						}
						if (Alpha.Detonated)
						{
							ev.Reply = "Нельзя заспавниться после взрыва боеголовки";
							ev.Success = false;
							return;
						}
						try { if (ev.Player.Tag.Contains(Scps.God.Tag)) Scps.God.Kill(ev.Player); } catch { }
						var team = role.GetTeam();
						if (roles.Priest)
						{
							if (role == RoleTypeId.Tutorial || team == Team.SCPs)
							{
								ev.Success = false;
								ev.Reply = "Священник не может нести зло в сие мир";
								return;
							}
							ev.Reply = $"Успешно\nЛимит: {forcer + 1}/{DonateLimint}";
							if ((team == Team.FoundationForces && role != RoleTypeId.FacilityGuard) || team == Team.ChaosInsurgency)
								ev.Player.GamePlay.BlockSpawnTeleport = true;
							ev.Player.RoleInfomation.SetNew(role, RoleChangeReason.Respawn);
							if (team == Team.FoundationForces && role != RoleTypeId.FacilityGuard) ev.Player.MovementState.Position = Textures.Models.Rooms.Bashni.MTFSpawnPoint;
							else if (team == Team.ChaosInsurgency) ev.Player.MovementState.Position = Textures.Models.Rooms.Bashni.ChaosSpawnPoint;
							Data.force[ev.Player.UserInfomation.UserId]++;
							return;
						}
						if (role == RoleTypeId.Tutorial)
						{
							ev.Success = false;
							ev.Reply = "Отключен, ввиду баланса";
							return;
						}
						int scps = Player.List.Where(x => x.RoleInfomation.Team == Team.SCPs).Count();
						if (team == Team.SCPs && scps > 2 && Player.List.Count() / scps < 6)
						{
							ev.Success = false;
							ev.Reply = "SCP слишком много";
							return;
						}
						if (team == Team.SCPs)
						{
							if (!Data.scp_play.ContainsKey(ev.Player.UserInfomation.UserId)) Data.scp_play.Add(ev.Player.UserInfomation.UserId, false);
							if (Data.scp_play[ev.Player.UserInfomation.UserId])
							{
								ev.Success = false;
								ev.Reply = "Вы уже играли за SCP";
								return;
							}

							if (Force.SpawnedSCPs.Contains(role))
							{
								ev.Success = false;
								ev.Reply = "Данный SCP уже был в раунде";
								return;
							}
						}
						int Scp173 = 0;
						int Scp106 = 0;
						int Scp049 = 0;
						int Scp079 = 0;
						int Scp096 = 0;
						int Scp939 = 0;
						foreach (var pl in Player.List)
						{
							switch (pl.RoleInfomation.Role)
							{
								case RoleTypeId.Scp173: Scp173++; break;
								case RoleTypeId.Scp106: Scp106++; break;
								case RoleTypeId.Scp049: Scp049++; break;
								case RoleTypeId.Scp079: Scp079++; break;
								case RoleTypeId.Scp096: Scp096++; break;
								case RoleTypeId.Scp939: Scp939++; break;
							}
						}
						if ((role == RoleTypeId.Scp939 && Scp939 > 0) || (role == RoleTypeId.Scp096 && Scp096 > 0) ||
							(role == RoleTypeId.Scp079 && Scp079 > 0) || (role == RoleTypeId.Scp049 && Scp049 > 0) ||
							(role == RoleTypeId.Scp106 && Scp106 > 0) || (role == RoleTypeId.Scp173 && Scp173 > 0))
						{
							ev.Success = false;
							ev.Reply = "Этот SCP уже есть";
							return;
						}
						double CoolDown = 2;
						if (!Data.forces.ContainsKey(ev.Player.UserInfomation.UserId)) Data.forces.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
						else if ((DateTime.Now - Data.forces[ev.Sender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((Data.forces[ev.Sender.SenderId] - DateTime.Now).TotalSeconds);
							ev.Reply = $"Спавниться можно раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						ev.Reply = $"Успешно\nЛимит: {forcer + 1}/{DonateLimint}";
						if ((team == Team.FoundationForces && role != RoleTypeId.FacilityGuard) || team == Team.ChaosInsurgency)
							ev.Player.GamePlay.BlockSpawnTeleport = true;
						ev.Player.RoleInfomation.SetNew(role, RoleChangeReason.Respawn);
						if (team == Team.FoundationForces && role != RoleTypeId.FacilityGuard) ev.Player.MovementState.Position = Textures.Models.Rooms.Bashni.MTFSpawnPoint;
						else if (team == Team.ChaosInsurgency) ev.Player.MovementState.Position = Textures.Models.Rooms.Bashni.ChaosSpawnPoint;
						Data.forces[ev.Sender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						Data.force[ev.Player.UserInfomation.UserId]++;
						return;
					}
				case "effect":
					{
						ev.Prefix = "EFFECT";
						Data.Roles.TryGetValue(ev.Sender.SenderId, out var roles);
						if (!(roles.Sage || roles.Star || (Data.effecter.TryGetValue(ev.Sender.SenderId, out var ___) && ___))
							 && !CustomDonates.ThisDonater(ev.Sender.SenderId)) return;
						ev.Allowed = false;
						double CoolDown = 3;
						if (!Data.effect.ContainsKey(ev.Player.UserInfomation.UserId)) Data.effect.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
						else if ((DateTime.Now - Data.effect[ev.Sender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((Data.effect[ev.Sender.SenderId] - DateTime.Now).TotalSeconds);
							ev.Reply = $"Эффекты можно использовать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}

						if (ev.Args[0].ToLower() == "movementboost" || ev.Args[0].ToLower() == "bodyshotreduction" || ev.Args[0].ToLower() == "damagereduction"
							|| ev.Args[0].ToLower() == "spawnprotected" || ev.Args[0].ToLower() == "scp1853")
						{
							ev.Reply = "Данный эффект слишком сильно влияет на баланс";
							ev.Success = false;
							return;
						}
						if (ev.Args[0].ToLower() == "invisible" && ev.Player.RoleInfomation.Team == Team.SCPs)
						{
							ev.Reply = "За SCP нельзя выдавать эффект невидимости";
							ev.Success = false;
							return;
						}
						if (ev.Args[0].ToLower() == "scp207" && ev.Args[1] != "0" && ev.Player.RoleInfomation.Team == Team.SCPs) ev.Args[1] = "1";
						else if (ev.Args[0].ToLower() == "invisible" && ev.Args[1] != "0") ev.Args[2] = "30";

						int time = 0;
						try { time = int.Parse(ev.Args[2]); }
						catch
						{
							ev.Success = false;
							ev.Reply = "Произошла ошибка при парсинге времени";
							return;
						}
						byte intensivity = 1;
						try { intensivity = byte.Parse(ev.Args[1]); } catch { }

						if (ev.Player.Effects.Controller.TryGetEffect(ev.Args[0], out var _status))
						{
							ev.Player.Effects.Enable(_status, time, false);
							_status.Intensity = intensivity;
						}

						Data.effect[ev.Sender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						ev.Reply = "Успешно";
						return;
					}
				case "server_event":
					{
						ev.Prefix = "SERVER_EVENT";
						Data.Roles.TryGetValue(ev.Sender.SenderId, out var roles);
						if (!roles.Star && !CustomDonates.ThisDonater(ev.Sender.SenderId)) return;
						ev.Allowed = false;
						if ((DateTime.Now - LastCall).TotalSeconds < 90)
						{
							ev.Success = false;
							var w = Math.Round(90 - (DateTime.Now - LastCall).TotalSeconds);
							ev.Reply = $"Последний отряд был вызван менее 90 секунд назад\nПодождите {w} сек";
							return;
						}
						if (ev.Args[0].ToLower() == "force_mtf_respawn")
						{
							if (LastFaction == Faction.FoundationStaff)
							{
								ev.Success = false;
								ev.Reply = "Последний отряд, вызванный донатером, был МТФ";
								return;
							}
							if ((DateTime.Now - MobileTaskForces.LastCall).TotalSeconds < 45)
							{
								ev.Success = false;
								var w = Math.Round(45 - (DateTime.Now - MobileTaskForces.LastCall).TotalSeconds);
								ev.Reply = $"Последний отряд МОГ приехал менее 45 секунд назад\nПодождите {w} сек";
								return;
							}
							LastFaction = Faction.FoundationStaff;
							ev.Success = true;
							ev.Reply = "Успешно";
							LastCall = DateTime.Now;
							MobileTaskForces.SpawnMtf();
							return;
						}
						if (ev.Args[0].ToLower() == "force_ci_respawn")
						{
							if (LastFaction == Faction.FoundationEnemy)
							{
								ev.Success = false;
								ev.Reply = "Последний отряд, вызванный донатером, был Хаос";
								return;
							}
							if ((DateTime.Now - ChaosInsurgency.LastCall).TotalSeconds < 30)
							{
								ev.Success = false;
								var w = Math.Round(30 - (DateTime.Now - ChaosInsurgency.LastCall).TotalSeconds);
								ev.Reply = $"Последний отряд ПХ приехал менее 30 секунд назад\nПодождите {w} сек";
								return;
							}
							LastFaction = Faction.FoundationEnemy;
							ev.Success = true;
							ev.Reply = "Успешно";
							LastCall = DateTime.Now;
							ChaosInsurgency.SpawnCI();
							return;
						}
						ev.Success = false;
						ev.Reply = "Отряд не найден";
						return;
					}
			}
		}
	}
}