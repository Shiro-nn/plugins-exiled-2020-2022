using HarmonyLib;
using Loli.Addons;
using Loli.DataBase;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.RoundStart)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.RoundEnd)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.AntiGrenade)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.AntiScpAttack)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Dead)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Cuff)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Attack)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.AddTarget)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Escape)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.ChangeRole)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Leave)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Pocket), new Type[] { typeof(object) }));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Pocket), new Type[] { typeof(object) }));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Pocket), new Type[] { typeof(object) }));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Door)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Drop)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Femur)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Pickup)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.AlphaEv)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Medical)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Upgrade)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Locker)));
			Qurre.API.Core.InjectEventMethod(AccessTools.Method(typeof(God), nameof(God.Generator)));

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
		static public bool Scp268 = false;
		static public int tranqtime = 0;
		static public int doortime = 0;
		static public int tptime = 0;
		static public int healalltime = 0;
		static public int healtime = 0;
		static public int resurrectiontime = 0;

        [EventsIgnore]
		[EventMethod(RoundEvents.Start)]
		static void RoundStart()
		{
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			resurrectiontime = 0;
			Scp268 = false;
		}

		[EventsIgnore]
		[EventMethod(RoundEvents.End)]
		static void RoundEnd()
		{
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			resurrectiontime = 0;
			Scp268 = false;

			foreach (var pl in Player.List.Where(x => x.Tag.Contains(Tag)))
				Kill(pl);
		}

		[EventsIgnore]
		[EventMethod(EffectEvents.Flashed)]
		static void AntiGrenade(PlayerFlashedEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(ScpEvents.Attack)]
		static void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.Dead)]
		static void Dead(DeadEvent ev)
		{
			if (ev.Target == null) 
				return;
			if (!ev.Target.Tag.Contains(Tag))
				return;

			Scp268 = false;
			Kill(ev.Target);
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.Cuff)]
		static void Cuff(CuffEvent ev)
		{
			if (ev.Target != null && ev.Target.Tag.Contains(Tag))
				ev.Allowed = false;
			if (ev.Cuffer != null && ev.Cuffer.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.Attack)]
		static void Attack(DamageEvent ev)
		{
			if (ev.Target.Tag.Contains(Tag) || ev.Attacker.Tag.Contains(Tag))
			{
				ev.Allowed = false;
				ev.Damage = 0f;
				return;
			}
		}

		static void AddTarget(AddTargetEvent ev)
		{
			if (ev.Target == null)
				return;

			if (ev.Target.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.Escape)]
		static void Escape(EscapeEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.ChangeRole)]
		static void ChangeRole(ChangeRoleEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag))
				return;

			Scp268 = false;
			Kill(ev.Player);
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag))
				return;

			Kill(ev.Player);

			Player pl = SelectReplace();

			if (pl == null)
				return;

			pl.MovementState.Position = ev.Player.MovementState.Position;
			pl.MovementState.Rotation = ev.Player.MovementState.Rotation;
		}

		static void Pocket(PocketEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		static void Pocket(PocketEscapeEvent ev)
		{
			if (ev.Player == null)
				return;
			if (!ev.Player.Tag.Contains(Tag))
				return;

			ev.Allowed = false;
			Extensions.TeleportTo106(ev.Player);
		}

		static void Pocket(PocketFailEscapeEvent ev)
		{
			if (ev.Player == null)
				return;
			if (!ev.Player.Tag.Contains(Tag))
				return;

			ev.Allowed = false;
			Extensions.TeleportTo106(ev.Player);
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.InteractDoor)]
		static void Door(InteractDoorEvent ev)
		{
			if (ev.Allowed)
				return;
			if (ev.Player == null)
				return;
			if (!ev.Player.Tag.Contains(Tag))
				return;

			if (Round.ElapsedTime.TotalSeconds >= 120)
			{
				if (doortime == 0)
					ev.Allowed = true;
				else
				{
					ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1);
					Timing.CallDelayed(1, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
					Timing.CallDelayed(2, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
					Timing.CallDelayed(3, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
					Timing.CallDelayed(4, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - doortime} секунд</color></b>", 1));
				}
			}
			else if (Round.ElapsedTime.TotalSeconds < 120)
			{
				ev.Player.Client.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1);
				Timing.CallDelayed(1U, () => ev.Player.Client.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
				Timing.CallDelayed(2U, () => ev.Player.Client.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
				Timing.CallDelayed(3U, () => ev.Player.Client.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
				Timing.CallDelayed(4U, () => ev.Player.Client.ShowHint($"<b><color=red>Вы сможете открывать двери через {Math.Round(120 - Round.ElapsedTime.TotalSeconds)} секунд!</color></b>", 1));
			}
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.DropItem)]
		static void Drop(DropItemEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag))
				return;
			if (ev.Item.Type == ItemType.Flashlight)
				ev.Allowed = false;
			else if (ev.Item.Type == ItemType.Coin)
			{
				ev.Allowed = false;
				try
				{
					List<Player> list = Player.List.Where(x => x.RoleInfomation.Role != RoleTypeId.Spectator &&
					x.RoleInfomation.Role != RoleTypeId.Scp079 && x.UserInfomation.UserId != null &&
					x.UserInfomation.UserId != string.Empty && !x.Tag.Contains(Tag) && x.MovementState.Position != Vector3.zero).ToList();
					Player player = list[Random.Range(0, list.Count)];
					if (player == null)
					{
						ev.Player.Client.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
						return;
					}
					if (tptime == 0)
					{
						ev.Player.MovementState.Position = player.MovementState.Position;
						ev.Player.Client.ShowHint($"<b><color=#15ff00>Вы телепортированы к {player.UserInfomation.Nickname}</color></b>", 5);
						tptime = 55;
					}
					else
					{
						ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1);
						Timing.CallDelayed(1, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
						Timing.CallDelayed(2, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
						Timing.CallDelayed(3, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
						Timing.CallDelayed(4, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - tptime} секунд</color></b>", 1));
					}
				}
				catch
				{
					ev.Player.Client.ShowHint("<b><color=#ff0000>Произошла ошибка, повторите позже</color></b>", 5);
				}
			}
			if (ev.Item.Type == ItemType.SCP268)
			{
				ev.Allowed = false;
				if (!Scp268)
				{
					ev.Player.Effects.Enable(EffectType.Invisible);
					Scp268 = true;
					return;
				}
				else if (Scp268)
				{
					ev.Player.Effects.Disable(EffectType.Invisible);
					Scp268 = false;
					return;
				}
			}
			if (ev.Item.Type == ItemType.Medkit)
			{
				ev.Allowed = false;
				var list = Player.List.Where(x => !x.Tag.Contains(Tag) && x.GetTeam() != Team.SCPs &&
				Vector3.Distance(ev.Player.MovementState.Position, x.MovementState.Position) <= 5);
				if (list.Count() == 0)
				{
					ev.Player.Client.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
					return;
				}
				if (healalltime == 0)
				{
					foreach (Player player in list) player.HealthInfomation.Hp = player.HealthInfomation.MaxHp;
					ev.Player.Client.ShowHint("<b><color=#15ff00>Игроки вылечены</color></b>", 5);
					healalltime = 1;
				}
				else
				{
					ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1);
					Timing.CallDelayed(1U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
					Timing.CallDelayed(2U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
					Timing.CallDelayed(3U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
					Timing.CallDelayed(4U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healalltime} секунд</color></b>", 1));
				}
			}
			if (ev.Item.Type == ItemType.Painkillers)
			{
				ev.Allowed = false;
				var list = Player.List.Where(x => !x.Tag.Contains(Tag) && x.GetTeam() != Team.SCPs &&
				Vector3.Distance(ev.Player.MovementState.Position, x.MovementState.Position) <= 5).ToList();
				if (list.Count == 0)
				{
					ev.Player.Client.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
					return;
				}
				if (healtime == 0)
				{
					Player player = list[Random.Range(0, list.Count)];
					player.HealthInfomation.Hp = player.HealthInfomation.MaxHp;
					ev.Player.Client.ShowHint($"{player.UserInfomation.Nickname} успешно вылечен", 5);
					healtime = 1;
				}
				else
				{
					ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1);
					Timing.CallDelayed(1U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
					Timing.CallDelayed(2U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
					Timing.CallDelayed(3U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
					Timing.CallDelayed(4U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {60 - healtime} секунд</color></b>", 1));
				}
			}
			if (ev.Item.Type == ItemType.SCP500)
			{
				ev.Allowed = false;
				if (resurrectiontime == 0)
				{
					var list = Map.Ragdolls.Where(x => x != null && Vector3.Distance(ev.Player.MovementState.Position + Vector3.down, x.Position) <= 2f &&
					x.Owner != null && x.Owner.RoleInfomation.Role == RoleTypeId.Spectator && Caches.Role.ContainsKey(x.Owner.UserInfomation.Id));
					if (list.Count() == 0)
					{
						ev.Player.Client.ShowHint("<b><color=#ff0000>Игроки не найдены</color></b>", 5);
						return;
					}
					var doll = list.First();
					doll.Destroy();
					resurrectiontime = 1;
					doll.Owner.RoleInfomation.SetNew(Caches.Role[doll.Owner.UserInfomation.Id], RoleChangeReason.Respawn);
					Timing.CallDelayed(0.3f, () => doll.Owner.HealthInfomation.Hp = 20);
					Timing.CallDelayed(0.8f, () => doll.Owner.HealthInfomation.Hp = 20);
					Timing.CallDelayed(0.5f, () => doll.Owner.MovementState.Position = ev.Player.MovementState.Position);
					Timing.CallDelayed(0.5f, () => doll.Owner.Inventory.Clear());
					ev.Player.Client.ShowHint($"<color=red>Вы вылечили игрока {doll.Owner.UserInfomation.Nickname}!</color>", 10);
					doll.Owner.Client.ShowHint($"<color=red>Вас вылечил {ev.Player.UserInfomation.Nickname}!</color>", 10);
					doll.Owner.MovementState.Scale = new Vector3(1, 1, 1);
				}
				else
				{
					ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1);
					Timing.CallDelayed(1U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
					Timing.CallDelayed(2U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
					Timing.CallDelayed(3U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
					Timing.CallDelayed(4U, () => ev.Player.Client.ShowHint($"<b><color=#ff0000>Подождите {120 - resurrectiontime} секунд</color></b>", 1));
				}
			}
		}

		static void Femur(FemurBreakerEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.PickupItem)]
		static void Pickup(PickupItemEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			if (Shop.ItsShop(ev.Pickup)) return;
			if (ev.Pickup.Type == ItemType.Coin || Shop.ItsShop(ev.Pickup)) return;
			if (ev.Pickup.Category == ItemCategory.Firearm)
			{
				ev.Pickup.Destroy();
				new Item(ItemType.Medkit).Spawn(ev.Player.MovementState.Position);
			}
			else if (ev.Pickup.Category == ItemCategory.MicroHID)
			{
				ev.Pickup.Destroy();
				new Item(ItemType.MicroHID).Spawn(ev.Player.MovementState.Position);
			}
			else if (ev.Pickup.Category == ItemCategory.Grenade)
			{
				ev.Pickup.Destroy();
				new Item(ItemType.Adrenaline).Spawn(ev.Player.MovementState.Position);
			}
		}

		[EventsIgnore]
		[EventMethod(AlphaEvents.Stop)]
		static void AlphaEv(AlphaStopEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.UseItem)]
		static void Medical(UseItemEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag) &&
				(ev.Item.Category == ItemCategory.Medical ||
				 ev.Item.Category == ItemCategory.SCPItem))
					ev.Allowed = false;
		}

		static void Upgrade(UpgradePlayerEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			ev.Player.Position = ev.Player.Position + ev.Move;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.InteractLocker)]
		static void Locker(InteractLockerEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventsIgnore]
		[EventMethod(PlayerEvents.InteractGenerator)]
		static void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		static internal void Ra(RemoteAdminCommandEvent ev)
		{
			if (ev.Name == "scp343")
			{
				ev.Allowed = false;
				ev.Prefix = "SCP343";
				try
				{
					string name = string.Join(" ", ev.Args);
					Player player = name.GetPlayer();
					if (ev.Sender.SenderId == "-@steam"
						|| ev.Sender.SenderId == "SERVER CONSOLE")
					{
						if (player == null)
						{
							ev.Success = false;
							ev.Reply = "Игрок не найден";
							return;
						}
						ev.Reply = "Успешно";
						Spawn(player);
					}
					else
					{
						ev.Success = false;
						ev.Reply = "Отказано в доступе";
					}
				}
				catch
				{
					ev.Success = false;
					ev.Reply = "Произошла ошибка";
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
			pl.Inventory.Clear();
			pl.GamePlay.GodMode = false;
			pl.Tag = pl.Tag.Replace(Tag, "");
			pl.HealthInfomation.Hp = 100;
			try { Levels.SetPrefix(pl); } catch { pl.SetRank($"[data deleted] уровень", "green"); }

			if (Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(pl))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Remove(pl);
		}
		public static void SpawnPriest(Player pr)
		{
			try
			{
				pr.HealthInfomation.Kill("Перерождение");
				Timing.CallDelayed(0.5f, () => Spawn(pr));
			}
			catch { SelectReplace(); }
		}
		public static void Spawn(Player pl)
		{
			if (!Init) new God();
			GodPlayer = pl;
			pl.RoleInfomation.Role = RoleTypeId.Tutorial;
			pl.Tag = pl.Tag.Replace(Tag, "") + Tag;
			Timing.CallDelayed(1f, () => pl.Tag = pl.Tag.Replace(Tag, "") + Tag);
			Timing.CallDelayed(0.5f, () => pl.HealthInfomation.MaxHp = 777);
			Timing.CallDelayed(0.5f, () => pl.HealthInfomation.Hp = 777);
			Timing.CallDelayed(0.5f, () => pl.Inventory.Clear());
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.Coin));
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.Painkillers));
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.Medkit));
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.GunRevolver));
			Timing.CallDelayed(0.5f, () => pl.Inventory.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => pl.Inventory.Ammo.Ammo44Cal = 999);
			Timing.CallDelayed(0.5f, () => { try { Levels.SetPrefix(pl); } catch { pl.SetRank("Бог", "red"); }; });
			pl.Client.Broadcast(10, "<color=red>Вы заспавнились за Бога</color>\n <color=red>Больше информации в вашей консоли на [ё]</color>", true);
			pl.Client.SendConsole("\n----------------------------------------------------------- \n " + "Вы заспавнились за БОГА" +
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
			pl.Effects.Enable(EffectType.Scp207);
			pl.UserInfomation.CustomInfo = "Бог";
			pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;

			if (!Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(pl))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Add(pl);
		}
		public static Player SelectReplace()
		{
			List<Player> pList = Player.List.Where(x => x.RoleInfomation.Role == RoleTypeId.Spectator &&
				x.UserInfomation.UserId != null && x.UserInfomation.UserId != string.Empty && !x.GamePlay.Overwatch).ToList();
			if (pList.Count == 0) return null;
			if (Player.List.Any(x => x.Tag.Contains(Tag))) return null;
			Player pl = pList[Random.Range(0, pList.Count)];
			Spawn(pl);
			return pl;
		}
	}
}