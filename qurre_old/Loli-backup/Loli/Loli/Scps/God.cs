using Loli.DataBase;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Scps
{
	public class God
	{
		internal const string Tag = " Scp343";
		internal static bool Init = false;
		internal static Player GodPlayer;
		public God()
		{
			if (Init) return;
			RoundStart();
			Qurre.Events.Round.Start += RoundStart;
			Qurre.Events.Round.End += RoundEnd;
			Qurre.Events.Player.Flashed += AntiGrenade;
			Qurre.Events.Player.ScpAttack += AntiScpAttack;
			Qurre.Events.Player.Dead += Dead;
			Qurre.Events.Player.Cuff += Cuff;
			Qurre.Events.Player.Damage += Damage;
			Qurre.Events.Scp096.AddTarget += AddTarget;
			Qurre.Events.Player.Escape += Escape;
			Qurre.Events.Player.RoleChange += ChangeRole;
			Qurre.Events.Player.Leave += Leave;
			Qurre.Events.Scp106.Contain += Contain;
			Qurre.Events.Scp106.PocketEnter += Pocket;
			Qurre.Events.Scp106.PocketEscape += Pocket;
			Qurre.Events.Scp106.PocketFailEscape += Pocket;
			Qurre.Events.Player.InteractDoor += Door;
			Qurre.Events.Player.DroppingItem += Drop;
			Qurre.Events.Scp106.FemurBreakerEnter += Femur;
			Qurre.Events.Player.PickupItem += Pickup;
			Qurre.Events.Alpha.Stopping += AlphaEv;
			Qurre.Events.Player.ItemUsing += Medical;
			Qurre.Events.Scp914.UpgradePlayer += Upgrade;
			Qurre.Events.Player.InteractLocker += Locker;
			Qurre.Events.Player.InteractGenerator += Generator;
			Timing.RunCoroutine(Cor());
			Init = true;
			IEnumerator<float> Cor()
			{
				while (true)
				{
					UpdateTimes();
					yield return Timing.WaitForSeconds(1);
				}
			}
		}
		public bool Scp268 = false;
		public int tranqtime = 0;
		public int doortime = 0;
		public int tptime = 0;
		public int healalltime = 0;
		public int healtime = 0;
		public int resurrectiontime = 0;
		public void RoundStart()
		{
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			resurrectiontime = 0;
			Scp268 = false;
		}

		public void RoundEnd(RoundEndEvent ev)
		{
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			resurrectiontime = 0;
			Scp268 = false;
			foreach (var pl in Player.List.Where(x => x.Tag.Contains(Tag))) Kill(pl);
		}
		internal void AntiGrenade(FlashedEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Dead(DeadEvent ev)
		{
			if (ev.Target == null) return;
			if (!ev.Target.Tag.Contains(Tag)) return;
			Scp268 = false;
			Kill(ev.Target);
		}
		public void Cuff(CuffEvent ev)
		{
			if (ev.Target != null && ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
			if (ev.Cuffer != null && ev.Cuffer.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Damage(DamageEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag) || ev.Attacker.Tag.Contains(Tag))
			{
				ev.Allowed = false;
				ev.Amount = 0f;
				return;
			}
		}
		public void AddTarget(AddTargetEvent ev)
		{
			if (ev.Target == null) return;
			if (ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Escape(EscapeEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void ChangeRole(RoleChangeEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			Scp268 = false;
			Kill(ev.Player);
		}

		public void Leave(LeaveEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			Kill(ev.Player);
			Player pl = SelectReplace();
			if (pl == null) return;
			pl.Position = ev.Player.Position;
			pl.Rotation = ev.Player.Rotation;
		}
		public void Contain(ContainEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Pocket(PocketEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Pocket(PocketEscapeEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			Extensions.TeleportTo106(ev.Player);
		}
		public void Pocket(PocketFailEscapeEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			Extensions.TeleportTo106(ev.Player);
		}
		public void Door(InteractDoorEvent ev)
		{
			if (ev.Allowed) return;
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			if (Round.ElapsedTime.TotalSeconds >= 120)
			{
				if (doortime == 0) ev.Allowed = true;
				else
				{
					ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1);
					Timing.CallDelayed(1, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
					Timing.CallDelayed(2, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
					Timing.CallDelayed(3, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
					Timing.CallDelayed(4, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
				}
			}
			else if (Round.ElapsedTime.TotalSeconds < 120)
			{
				ev.Player.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1);
				Timing.CallDelayed(1U, () => ev.Player.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
				Timing.CallDelayed(2U, () => ev.Player.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
				Timing.CallDelayed(3U, () => ev.Player.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
				Timing.CallDelayed(4U, () => ev.Player.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
			}
		}
		public void Drop(DroppingItemEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			if (ev.Item.Type == ItemType.Flashlight) ev.Allowed = false;
			else if (ev.Item.Type == ItemType.Coin)
			{
				ev.Allowed = false;
				try
				{
					List<Player> list = Player.List.Where(x => x.Role != RoleType.Spectator && x.Role != RoleType.Scp079 && x.UserId != null && x.UserId != string.Empty &&
					!x.Tag.Contains(Tag) && x.Position != Vector3.zero).ToList();
					Player player = list[Extensions.Random.Next(list.Count)];
					if (player == null)
					{
						ev.Player.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
						return;
					}
					if (tptime == 0)
					{
						ev.Player.Position = player.Position;
						ev.Player.ShowHint($"<b><color=#15ff00>Вы телепортированы к {player.Nickname}</color></b>", 5);
						tptime = 55;
					}
					else
					{
						ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1);
						Timing.CallDelayed(1, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
						Timing.CallDelayed(2, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
						Timing.CallDelayed(3, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
						Timing.CallDelayed(4, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
					}
				}
				catch
				{
					ev.Player.ShowHint("<b><color=#ff0000>Произошла ошибка, повторите позже</color></b>", 5);
				}
			}
			if (ev.Item.Type == ItemType.SCP268)
			{
				ev.Allowed = false;
				if (!Scp268)
				{
					ev.Player.EnableEffect(EffectType.Invisible);
					Scp268 = true;
					return;
				}
				else if (Scp268)
				{
					ev.Player.DisableEffect(EffectType.Invisible);
					Scp268 = false;
					return;
				}
			}
			if (ev.Item.Type == ItemType.Medkit)
			{
				ev.Allowed = false;
				var list = Player.List.Where(x => !x.Tag.Contains(Tag) && x.GetTeam() != Team.SCP && Vector3.Distance(ev.Player.Position, x.Position) <= 5);
				if (list.Count() == 0)
				{
					ev.Player.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
					return;
				}
				if (healalltime == 0)
				{
					foreach (Player player in list) player.Hp = player.MaxHp;
					ev.Player.ShowHint("<b><color=#15ff00>Игроки вылечены</color></b>", 5);
					healalltime = 1;
				}
				else
				{
					ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1);
					Timing.CallDelayed(1U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
					Timing.CallDelayed(2U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
					Timing.CallDelayed(3U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
					Timing.CallDelayed(4U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
				}
			}
			if (ev.Item.Type == ItemType.Painkillers)
			{
				ev.Allowed = false;
				var list = Player.List.Where(x => !x.Tag.Contains(Tag) && x.GetTeam() != Team.SCP && Vector3.Distance(ev.Player.Position, x.Position) <= 5).ToList();
				if (list.Count == 0)
				{
					ev.Player.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
					return;
				}
				if (healtime == 0)
				{
					Player player = list[Extensions.Random.Next(list.Count)];
					player.Hp = player.MaxHp;
					ev.Player.ShowHint($"{player.Nickname} успешно вылечен", 5);
					healtime = 1;
				}
				else
				{
					ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1);
					Timing.CallDelayed(1U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
					Timing.CallDelayed(2U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
					Timing.CallDelayed(3U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
					Timing.CallDelayed(4U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
				}
			}
			if (ev.Item.Type == ItemType.SCP500)
			{
				ev.Allowed = false;
				if (resurrectiontime == 0)
				{
					var list = Map.Ragdolls.Where(x => x != null && Vector3.Distance(ev.Player.Position + Vector3.down, x.Position) <= 2f &&
					x.Owner != null && x.Owner.Role == RoleType.Spectator && Modules.EventHandlers.DRole.ContainsKey(x.Owner.Id));
					if (list.Count() == 0)
					{
						ev.Player.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
						return;
					}
					var doll = list.First();
					doll.Destroy();
					resurrectiontime = 1;
					doll.Owner.BlockSpawnTeleport = true;
					doll.Owner.ClassManager.SetClassIDAdv(Modules.EventHandlers.DRole[doll.Owner.Id], false, CharacterClassManager.SpawnReason.Respawn);
					Timing.CallDelayed(0.3f, () => doll.Owner.Hp = 20);
					Timing.CallDelayed(0.8f, () => doll.Owner.Hp = 20);
					Timing.CallDelayed(0.5f, () => doll.Owner.Position = ev.Player.Position);
					Timing.CallDelayed(0.5f, () => doll.Owner.ClearInventory());
					ev.Player.ShowHint($"<color=red>Вы вылечили игрока {doll.Owner.Nickname}!</color>", 10);
					doll.Owner.ShowHint($"<color=red>Вас вылечил {ev.Player.Nickname}!</color>", 10);
					doll.Owner.Scale = new Vector3(1, 1, 1);
				}
				else
				{
					ev.Player.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1);
					Timing.CallDelayed(1U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
					Timing.CallDelayed(2U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
					Timing.CallDelayed(3U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
					Timing.CallDelayed(4U, () => ev.Player.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
				}
			}
		}
		public void Femur(FemurBreakerEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Pickup(PickupItemEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			if (Shop.ItsShop(ev.Pickup)) return;
			if (ev.Pickup.Type == ItemType.Coin || Shop.ItsShop(ev.Pickup)) return;
			if (ev.Pickup.Category == ItemCategory.Firearm)
			{
				ev.Pickup.Destroy();
				new Item(ItemType.Medkit).Spawn(ev.Player.Position);
			}
			else if (ev.Pickup.Category == ItemCategory.MicroHID)
			{
				ev.Pickup.Destroy();
				new Item(ItemType.MicroHID).Spawn(ev.Player.Position);
			}
			else if (ev.Pickup.Category == ItemCategory.Grenade)
			{
				ev.Pickup.Destroy();
				new Item(ItemType.Adrenaline).Spawn(ev.Player.Position);
			}
		}
		public void AlphaEv(AlphaStopEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
		}
		public void Medical(ItemUsingEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag) && (ev.Item.Category == ItemCategory.Medical || ev.Item.Category == ItemCategory.SCPItem)) ev.Allowed = false;
		}
		internal void Upgrade(UpgradePlayerEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			ev.Player.Position = ev.Player.Position + ev.Move;
		}
		public void Locker(InteractLockerEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		public static void Ra(SendingRAEvent ev)
		{
			if (ev.Name == "scp343")
			{
				ev.Allowed = false;
				ev.Prefix = "SCP343";
				try
				{
					string name = string.Join(" ", ev.Args);
					Player player = Player.Get(name);
					if (ev.CommandSender.SenderId == "-@steam"
						|| ev.CommandSender.SenderId == "SERVER CONSOLE")
					{
						if (player == null)
						{
							ev.Success = false;
							ev.ReplyMessage = "Игрок не найден";
							return;
						}
						ev.ReplyMessage = "Успешно";
						Spawn(player);
					}
					else
					{
						ev.Success = false;
						ev.ReplyMessage = "Отказано в доступе";
					}
				}
				catch
				{
					ev.Success = false;
					ev.ReplyMessage = "Произошла ошибка";
					return;
				}
			}
		}
		public void UpdateTimes()
		{
			if (tranqtime >= 60) tranqtime = 0;
			if (tranqtime != 0) tranqtime++;
			if (doortime == 60) doortime = 0;
			if (doortime != 0) doortime++;
			if (tptime == 60) tptime = 0;
			if (tptime != 0) tptime++;
			if (healalltime == 60) healalltime = 0;
			if (healalltime != 0) healalltime++;
			if (healtime == 60) healtime = 0;
			if (healtime != 0) healtime++;
			if (resurrectiontime == 120) resurrectiontime = 0;
			if (resurrectiontime != 0) resurrectiontime++;
		}

		internal static void Kill(Player pl)
		{
			GodPlayer = null;
			pl.ClearInventory();
			pl.GodMode = false;
			pl.Tag = pl.Tag.Replace(Tag, "");
			pl.Hp = 100;
			try { Levels.SetPrefix(pl); } catch { pl.SetRank($"[data deleted] уровень", "green"); }
			foreach (Player scp in Player.List) try { scp.Scp173Controller.IgnoredPlayers.Remove(pl); } catch { }
		}
		public static void SpawnPriest(Player pr)
		{
			try
			{
				pr.Kill("Перерождение");
				Timing.CallDelayed(0.5f, () => Spawn(pr));
			}
			catch { SelectReplace(); }
		}
		public static void Spawn(Player pl)
		{
			if (Plugin.ClansWars) return;
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (!Init) new God();
			GodPlayer = pl;
			pl.Role = RoleType.Tutorial;
			pl.Tag = pl.Tag.Replace(Tag, "") + Tag;
			Timing.CallDelayed(1f, () => pl.Tag = pl.Tag.Replace(Tag, "") + Tag);
			Timing.CallDelayed(0.5f, () => pl.MaxHp = 777);
			Timing.CallDelayed(0.5f, () => pl.Hp = 777);
			Timing.CallDelayed(0.5f, () => pl.ClearInventory());
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.Coin));
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.Painkillers));
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.Medkit));
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.GunRevolver));
			Timing.CallDelayed(0.5f, () => pl.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => pl.Ammo44Cal = 999);
			Timing.CallDelayed(0.5f, () => { try { Levels.SetPrefix(pl); } catch { pl.SetRank("Бог", "red"); }; });
			pl.Broadcast(10, "<color=red>Вы заспавнились за Бога</color>\n <color=red>Больше информации в вашей консоли на [ё]</color>", true);
			pl.SendConsoleMessage("\n----------------------------------------------------------- \n " + "Вы заспавнились за БОГА" +
				"\n" +
				"Вы сможете открывать двери через 2 минуты" +
				"\n" +
				"Выбросив монетку, вы телепортируетесь к рандомному игроку" +
				"\n" +
				"Выбросив обезболивающее, вы вылечите ближайшего игрока" +
				"\n" +
				"Выбросив аптечку, вы вылечите группу людей в 5 метрах от себя" +
				"\n" +
				"Выбросив SCP 500, вы оживите труп" +
				"\n----------------------------------------------------------- ", "red");
			pl.EnableEffect(EffectType.Scp207);
			foreach (Player scp in Player.List) try { scp.Scp173Controller.IgnoredPlayers.Add(pl); } catch { }
			pl.NicknameSync.Network_customPlayerInfoString = "Бог";
			pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
		}
		public static Player SelectReplace()
		{
			List<Player> pList = Player.List.Where(x => x.Role == RoleType.Spectator && x.UserId != null && x.UserId != string.Empty && !x.Overwatch).ToList();
			if (pList.Count == 0) return null;
			if (Player.List.Any(x => x.Tag.Contains(Tag))) return null;
			Player pl = pList[Extensions.Random.Next(pList.Count)];
			Spawn(pl);
			return pl;
		}
	}
}