using Loli.Addons;
using Loli.Spawns;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Linq;
namespace Loli.DataBase.Modules
{
	internal class Donate
	{
		internal static int DonateLimint => Plugin.Anarchy ? 20 : 5;
		internal readonly Manager Manager;
		internal Donate(Manager manager)
		{
			Manager = manager;
			CommandsSystem.RegisterRemoteAdmin("hidetag", AntiHideTag);
		}
		internal DateTime LastCall = DateTime.Now;
		internal Faction LastFaction = Faction.Others;
		private void AntiHideTag(SendingRAEvent ev)
		{
			ev.Prefix = "HIDETAG";
			ev.Allowed = false;
			ev.Success = false;
			ev.ReplyMessage = "Недоступно";
		}
		internal void Ra(SendingRAEvent ev)
		{
			switch (ev.CommandSender.SenderId)
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
						Manager.Data.Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!(roles.Mage || roles.Sage || roles.Star || (Manager.Data.giver.TryGetValue(ev.CommandSender.SenderId, out var ___) && ___))
							 && !CustomDonates.ThisDonater(ev.CommandSender.SenderId)) return;
						ev.Allowed = false;
						if (Round.Waiting)
						{
							ev.ReplyMessage = "Раунд еще не начался";
							ev.Success = false;
							return;
						}
						if (ev.Player.Team == Team.SCP)
						{
							ev.ReplyMessage = "Вы играете за SCP. За SCP нельзя выдавать предметы.";
							ev.Success = false;
							return;
						}
						if (ev.Player.AllItems.Count() == 8)
						{
							ev.ReplyMessage = "У вас заполнен инвентарь";
							ev.Success = false;
							return;
						}
						if (!Manager.Data.giveway.ContainsKey(ev.Player.UserId)) Manager.Data.giveway.Add(ev.Player.UserId, 0);
						Manager.Data.giveway.TryGetValue(ev.Player.UserId, out var giver);
						if (giver >= DonateLimint)
						{
							ev.ReplyMessage = $"Вы уже выдали {DonateLimint} предметов";
							ev.Success = false;
							return;
						}
						var itemN = -1;
						if (ev.Args.Length > 1)
						{
							try { itemN = Convert.ToInt32(ev.Args[1]); } catch { }
						}
						else
						{
							try { itemN = Convert.ToInt32(ev.Args[0]); } catch { }
						}
						if (itemN < 0 || itemN > 43)
						{
							ev.ReplyMessage = "ID предмета не найден";
							ev.Success = false;
							return;
						}
						ItemType item = (ItemType)itemN;
						if (item == ItemType.Ammo12gauge || item == ItemType.Ammo44cal || item == ItemType.Ammo556x45 ||
							item == ItemType.Ammo762x39 || item == ItemType.Ammo9x19)
						{
							ev.Player.GetAmmo();
							Manager.Data.giveway[ev.Player.UserId]++;
							ev.ReplyMessage = "Успешно";
							return;
						}
						if (Plugin.Anarchy)
						{
							ev.Player.AddItem(item);
							ev.ReplyMessage = "Успешно";
							Manager.Data.giveway[ev.Player.UserId]++;
							return;
						}
						if (item == ItemType.MicroHID)
						{
							ev.ReplyMessage = "MicroHid только 1";
							ev.Success = false;
							return;
						}
						else if ((ev.Player.Role == RoleType.ClassD || ev.Player.Role == RoleType.Scientist) &&
								(item == ItemType.KeycardGuard || item == ItemType.KeycardNTFOfficer || item == ItemType.GunCOM15 || item == ItemType.GunCOM18
								|| item == ItemType.KeycardChaosInsurgency || item == ItemType.KeycardContainmentEngineer || item == ItemType.KeycardFacilityManager
								|| item == ItemType.KeycardNTFCommander || item == ItemType.KeycardNTFLieutenant || item == ItemType.KeycardO5 ||
								item == ItemType.SCP018 || item == ItemType.GrenadeHE || item == ItemType.GrenadeFlash) &&
								(3 >= Round.ElapsedTime.TotalMinutes))
						{
							ev.ReplyMessage = "3 минуты не прошло";
							ev.Success = false;
							return;
						}
						else if ((ev.Player.Role == RoleType.ClassD || ev.Player.Role == RoleType.Scientist) &&
								(item == ItemType.GunCrossvec || item == ItemType.GunFSP9 || item == ItemType.GunRevolver) &&
								(4 >= Round.ElapsedTime.TotalMinutes))
						{
							ev.ReplyMessage = "4 минуты не прошло";
							ev.Success = false;
							return;
						}
						else if ((ev.Player.Role == RoleType.ClassD || ev.Player.Role == RoleType.Scientist) && (item == ItemType.GunAK ||
							item == ItemType.GunLogicer || item == ItemType.GunShotgun || item == ItemType.GunE11SR) &&
								(5 >= Round.ElapsedTime.TotalMinutes))
						{
							ev.ReplyMessage = "5 минут не прошло";
							ev.Success = false;
							return;
						}
						double CoolDown = 2;
						if (!Manager.Data.gives.ContainsKey(ev.Player.UserId)) Manager.Data.gives.Add(ev.Player.UserId, DateTime.Now);
						else if ((DateTime.Now - Manager.Data.gives[ev.CommandSender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((Manager.Data.gives[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
							ev.ReplyMessage = $"Предметы можно выдавать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						ev.Player.AddItem(item);
						ev.ReplyMessage = "Успешно";
						Manager.Data.gives[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						Manager.Data.giveway[ev.Player.UserId]++;
						return;
					}
				case "forceclass":
					{
						ev.Prefix = "FORCECLASS";
						Manager.Data.Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!(roles.Priest || roles.Mage || roles.Sage || roles.Star || (Manager.Data.forcer.TryGetValue(ev.CommandSender.SenderId, out var ___) && ___))
							&& !CustomDonates.ThisDonater(ev.CommandSender.SenderId)) return;
						ev.Allowed = false;
						if (Round.Waiting)
						{
							ev.ReplyMessage = "Раунд еще не начался";
							ev.Success = false;
							return;
						}
						if (!Manager.Data.force.ContainsKey(ev.Player.UserId)) Manager.Data.force.Add(ev.Player.UserId, 0);
						Manager.Data.force.TryGetValue(ev.Player.UserId, out var forcer);
						if (forcer >= DonateLimint)
						{
							ev.ReplyMessage = $"Вы уже меняли роль {DonateLimint} раз";
							ev.Success = false;
							return;
						}
						var dorole = -1;
						if (ev.Args.Length > 1) try { dorole = Convert.ToInt32(ev.Args[1]); } catch { }
						else try { dorole = Convert.ToInt32(ev.Args[0]); } catch { }
						if (dorole > 20 || dorole < 0)
						{
							ev.Success = false;
							ev.ReplyMessage = "ID роли не найден";
							return;
						}
						RoleType role = (RoleType)dorole;
						if (role == RoleType.NtfSpecialist && !Plugin.Anarchy)
						{
							ev.ReplyMessage = "За данную роль нельзя заспавниться";
							ev.Success = false;
							return;
						}
						if ((role == RoleType.ChaosConscript || role == RoleType.ChaosMarauder || role == RoleType.ChaosRepressor ||
										   role == RoleType.ChaosRifleman || role == RoleType.NtfSpecialist ||
										   role == RoleType.NtfSergeant || role == RoleType.NtfPrivate || role == RoleType.NtfCaptain)
										   && Round.ElapsedTime.Minutes == 0)
						{
							ev.ReplyMessage = $"Подождите {Math.Round(60 - Round.ElapsedTime.TotalSeconds)} секунд.";
							ev.Success = false;
							return;
						}
						if (!(role == RoleType.ChaosConscript || role == RoleType.ChaosMarauder || role == RoleType.ChaosRepressor ||
										   role == RoleType.ChaosRifleman || role == RoleType.NtfSpecialist ||
										   role == RoleType.NtfSergeant || role == RoleType.NtfPrivate || role == RoleType.NtfCaptain)
										   && Alpha.Detonated)
						{
							ev.ReplyMessage = "После взрыва боеголовки, запрещено спавниться за подземные классы.";
							ev.Success = false;
							return;
						}
						try { if (ev.Player.Tag.Contains(Scps.God.Tag)) Scps.God.Kill(ev.Player); } catch { }
						var team = role.GetTeam();
						if (roles.Priest)
						{
							if (role == RoleType.Tutorial || team == Team.SCP)
							{
								ev.Success = false;
								ev.ReplyMessage = "Священник не может нести зло в сие мир";
								return;
							}
							ev.ReplyMessage = $"Успешно\nЛимит: {forcer + 1}/{DonateLimint}";
							if ((team == Team.MTF && role != RoleType.FacilityGuard) || team == Team.CHI)
								ev.Player.BlockSpawnTeleport = true;
							ev.Player.SetRole(role, false, CharacterClassManager.SpawnReason.Respawn);
							if (team == Team.MTF && role != RoleType.FacilityGuard) ev.Player.Position = Textures.Models.Rooms.Bashni.MTFSpawnPoint;
							else if (team == Team.CHI) ev.Player.Position = Textures.Models.Rooms.Bashni.ChaosSpawnPoint;
							Manager.Data.force[ev.Player.UserId]++;
							try { Qurre.Events.Invoke.Player.Spawn(new(ev.Player, role, Map.GetRandomSpawnPoint(role), default)); } catch { }
							return;
						}
						if (role == RoleType.Tutorial)
						{
							if (Plugin.Anarchy)
							{
								ev.ReplyMessage = $"Успешно\nЛимит: {forcer + 1}/{DonateLimint}";
								Manager.plugin.SpawnManager.SerpentsHand.SpawnOne(ev.Player);
								Manager.Data.force[ev.Player.UserId]++;
								return;
							}
							ev.Success = false;
							ev.ReplyMessage = "Отключен, ввиду баланса";
							return;
						}
						int scps = Player.List.Where(x => x.Team == Team.SCP).Count();
						if (!Plugin.Anarchy && team == Team.SCP && scps > 2 && Player.List.Count() / scps < 6)
						{
							ev.Success = false;
							ev.ReplyMessage = "SCP слишком много";
							return;
						}
						if (team == Team.SCP)
						{
							if (!Manager.Data.scp_play.ContainsKey(ev.Player.UserId)) Manager.Data.scp_play.Add(ev.Player.UserId, false);
							if (Manager.Data.scp_play[ev.Player.UserId])
							{
								ev.Success = false;
								ev.ReplyMessage = "Вы уже играли за SCP";
								return;
							}
						}
						int Scp173 = 0;
						int Scp106 = 0;
						int Scp049 = 0;
						int Scp079 = 0;
						int Scp096 = 0;
						int Scp93989 = 0;
						int Scp93953 = 0;
						foreach (var pl in Player.List)
						{
							switch (pl.Role)
							{
								case RoleType.Scp173: Scp173++; break;
								case RoleType.Scp106: Scp106++; break;
								case RoleType.Scp049: Scp049++; break;
								case RoleType.Scp079: Scp079++; break;
								case RoleType.Scp096: Scp096++; break;
								case RoleType.Scp93989: Scp93989++; break;
								case RoleType.Scp93953: Scp93953++; break;
							}
						}
						if ((role == RoleType.Scp93953 && Scp93953 > 0) || (role == RoleType.Scp93989 && Scp93989 > 0) || (role == RoleType.Scp096 && Scp096 > 0) ||
							(role == RoleType.Scp079 && Scp079 > 0) || (role == RoleType.Scp049 && Scp049 > 0) || (role == RoleType.Scp106 && Scp106 > 0) ||
							(role == RoleType.Scp173 && Scp173 > 0))
						{
							ev.Success = false;
							ev.ReplyMessage = "Этот SCP уже есть";
							return;
						}
						if (role == RoleType.Scp106 && Manager.Data.Contain)
						{
							ev.Success = false;
							ev.ReplyMessage = "Условия содержания SCP 106 уже восстановлены";
							return;
						}
						double CoolDown = 2;
						if (!Manager.Data.forces.ContainsKey(ev.Player.UserId)) Manager.Data.forces.Add(ev.Player.UserId, DateTime.Now);
						else if ((DateTime.Now - Manager.Data.forces[ev.CommandSender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((Manager.Data.forces[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
							ev.ReplyMessage = $"Спавниться можно раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						ev.ReplyMessage = $"Успешно\nЛимит: {forcer + 1}/{DonateLimint}";
						if ((team == Team.MTF && role != RoleType.FacilityGuard) || team == Team.CHI)
							ev.Player.BlockSpawnTeleport = true;
						ev.Player.SetRole(role, false, CharacterClassManager.SpawnReason.Respawn);
						if (team == Team.MTF && role != RoleType.FacilityGuard) ev.Player.Position = Textures.Models.Rooms.Bashni.MTFSpawnPoint;
						else if (team == Team.CHI) ev.Player.Position = Textures.Models.Rooms.Bashni.ChaosSpawnPoint;
						Manager.Data.forces[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						Manager.Data.force[ev.Player.UserId]++;
						try { Qurre.Events.Invoke.Player.Spawn(new(ev.Player, role, Map.GetRandomSpawnPoint(role), default)); } catch { }
						return;
					}
				case "effect":
					{
						ev.Prefix = "EFFECT";
						Manager.Data.Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!(roles.Sage || roles.Star || (Manager.Data.effecter.TryGetValue(ev.CommandSender.SenderId, out var ___) && ___))
							 && !CustomDonates.ThisDonater(ev.CommandSender.SenderId)) return;
						ev.Allowed = false;
						double CoolDown = 3;
						if (!Manager.Data.effect.ContainsKey(ev.Player.UserId)) Manager.Data.effect.Add(ev.Player.UserId, DateTime.Now);
						else if ((DateTime.Now - Manager.Data.effect[ev.CommandSender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((Manager.Data.effect[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
							ev.ReplyMessage = $"Эффекты можно использовать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						var effects = ev.Args[1].Split('=');
						if (effects[0].ToLower() == "invisible" && ev.Player.Team == Team.SCP)
						{
							ev.Allowed = false;
							ev.ReplyMessage = "За SCP нельзя выдавать эффект невидимости";
							ev.Success = false;
							return;
						}
						Manager.Data.effect[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						if (effects[0].ToLower() == "scp207" && effects[1] != "0" && ev.Player.Team == Team.SCP) ev.Args[1] = "scp207=1";
						else if (effects[0].ToLower() == "invisible" && effects[1] != "0") ev.Player.EnableEffect(EffectType.Invisible, 30);
						else if (effects[0].ToLower() == "visuals939" && effects[1] != "0") ev.Player.EnableEffect(EffectType.Visuals939, 30);
						else
						{
							var com = $"{ev.Name} {ev.Player.Id}. {ev.Args[1]}";
							GameCore.Console.singleton.TypeCommand($"/{com}", new EffectsSender(ev.CommandSender.Nickname, com));
						}
						ev.ReplyMessage = "Успешно";
						return;
					}
				case "server_event":
					{
						ev.Prefix = "SERVER_EVENT";
						Manager.Data.Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!roles.Star && !CustomDonates.ThisDonater(ev.CommandSender.SenderId)) return;
						ev.Allowed = false;
						if ((DateTime.Now - LastCall).TotalSeconds < 90)
						{
							ev.Success = false;
							var w = Math.Round(90 - (DateTime.Now - LastCall).TotalSeconds);
							ev.ReplyMessage = $"Последний отряд был вызван менее 90 секунд назад\nПодождите {w} сек";
							return;
						}
						if (ev.Args[0].ToLower() == "force_mtf_respawn")
						{
							if (LastFaction == Faction.FoundationStaff)
							{
								ev.Success = false;
								ev.ReplyMessage = "Последний отряд, вызванный донатером, был МТФ";
								return;
							}
							if ((DateTime.Now - SpawnManager.Instance.MobileTaskForces.LastCall).TotalSeconds < 45)
							{
								ev.Success = false;
								var w = Math.Round(45 - (DateTime.Now - SpawnManager.Instance.MobileTaskForces.LastCall).TotalSeconds);
								ev.ReplyMessage = $"Последний отряд МОГ приехал менее 45 секунд назад\nПодождите {w} сек";
								return;
							}
							LastFaction = Faction.FoundationStaff;
							ev.Success = true;
							ev.ReplyMessage = "Успешно";
							LastCall = DateTime.Now;
							SpawnManager.Instance.MobileTaskForces.SpawnMtf();
							return;
						}
						if (ev.Args[0].ToLower() == "force_ci_respawn")
						{
							if (LastFaction == Faction.FoundationEnemy)
							{
								ev.Success = false;
								ev.ReplyMessage = "Последний отряд, вызванный донатером, был Хаос";
								return;
							}
							if ((DateTime.Now - SpawnManager.Instance.ChaosInsurgency.LastCall).TotalSeconds < 30)
							{
								ev.Success = false;
								var w = Math.Round(30 - (DateTime.Now - SpawnManager.Instance.ChaosInsurgency.LastCall).TotalSeconds);
								ev.ReplyMessage = $"Последний отряд ПХ приехал менее 30 секунд назад\nПодождите {w} сек";
								return;
							}
							LastFaction = Faction.FoundationEnemy;
							ev.Success = true;
							ev.ReplyMessage = "Успешно";
							LastCall = DateTime.Now;
							SpawnManager.Instance.ChaosInsurgency.SpawnCI();
							return;
						}
						ev.Success = false;
						ev.ReplyMessage = "Отряд не найден";
						return;
					}
			}
		}
	}
}