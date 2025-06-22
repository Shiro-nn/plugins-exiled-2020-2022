using CustomPlayerEffects;
using InventorySystem.Configs;
using InventorySystem.Items.Firearms;
using Loli.Addons;
using Loli.DataBase;
using MEC;
using Mirror.LiteNetLib4Mirror;
using Newtonsoft.Json.Linq;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Addons.Items;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.Modules
{
	static class Fixes
	{
		static readonly DateTime _init = DateTime.Now;

		static TimeSpan Elapsed => DateTime.Now - _init;

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting()
		{
			_frz = 0;
			_lastRound = -1;
		}

		[EventMethod(RoundEvents.End)]
		static void FixFastEnd()
		{
			if (Elapsed.TotalSeconds < 60)
				return;

			if (Round.ElapsedTime.TotalSeconds < 60)
			{
				Timing.RunCoroutine(Run());

				static IEnumerator<float> Run()
				{
					yield return Timing.WaitForSeconds(3f);
					Round.DimScreen();
					yield return Timing.WaitForSeconds(1f);
					Extensions.RestartCrush("Раунд закончился в первую минуту игры");
					yield break;
				}
			}
		}

		static int _frz = 0;
		static int _lastRound = -1;
		static internal void FixRoundFreeze()
		{
			if (Elapsed.TotalSeconds < 10)
				return;

			if (Round.Started)
				return;

			if (_lastRound == -1)
				return;

			if (GameCore.RoundStart.singleton.NetworkTimer != _lastRound)
			{
				_frz = 0;
				_lastRound = GameCore.RoundStart.singleton.NetworkTimer;
				return;
			}

			_frz++;
			if (_frz > 20)
				Extensions.RestartCrush("Зафризился таймер начала раунда");
		}

		static int _bcdead = 0;
		static internal void CheckBcAlive()
		{
			if (Elapsed.TotalSeconds < 30)
				return;

			try
			{
				if (Broadcast.Singleton is null)
					throw new NullReferenceException("");

				_bcdead = 0;
			}
			catch
			{
				_bcdead++;
			}

			if (_bcdead > 10)
				Extensions.RestartCrush("Умерли игровые скрипты, проверка на броадкастах");
		}

		static int ffe_process = 0;
		static internal void FixFreezeEnd()
		{
			if (!Round.Started || Round.Waiting)
			{
				ffe_process = 0;
				return;
			}

			if (Player.List.Any())
			{
				ffe_process = 0;
				return;
			}

			if (ffe_process < 30)
			{
				ffe_process++;
				return;
			}

			Extensions.RestartCrush("Фриз сервера при отсутствии игроков");
		}

		[EventMethod(PlayerEvents.Spawn, int.MinValue)]
		static void FixZombieSpawn(SpawnEvent ev)
		{
			if (ev.Role != RoleTypeId.Scp0492)
				return;

			if (Vector3.Distance(ev.Position, Vector3.zero) > 3 &&
				Vector3.Distance(ev.Position, Vector3.down * 2000) > 3)
				return;

			ev.Position = GetZombiePoint();

			static Vector3 GetZombiePoint()
			{
				if (Player.List.TryFind(out var doctor, x => x.RoleInformation.Role == RoleTypeId.Scp049))
					return doctor.MovementState.Position;
				if (Map.Rooms.TryFind(out var room, x => x.Type == RoomType.Hcz049))
					return room.Position + (Vector3.up * 2);

				return new(86, 989, -69);
			}
		}

		[EventMethod(PlayerEvents.Damage)]
		static void AntiLiftKills(DamageEvent ev)
		{
			if (ev.LiteType is not LiteDamageTypes.Scp018 and not LiteDamageTypes.Explosion)
			{
				return;
			}

			if (!Map.Doors.Any(x => Vector3.Distance(x.Position, ev.Target.MovementState.Position) < 2f))
			{
				return;
			}

			ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Escape)]
		static void AntiEscapeBag(EscapeEvent ev)
		{
			if (Round.ElapsedTime.TotalSeconds < 10)
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Dies)]
		static void Dies(DiesEvent ev)
		{
			if (!Round.Started && Round.ElapsedTime.TotalSeconds < 1)
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.ChangeItem)]
		static void FixInvisible(ChangeItemEvent ev)
		{
			if (ev.NewItem == null)
				return;

			if (!ev.Player.Effects.CheckActive<Invisible>())
				return;

			if (ev.NewItem.Category != ItemCategory.Firearm && ev.NewItem.Category != ItemCategory.Grenade &&
				ev.NewItem.Category != ItemCategory.SCPItem && ev.NewItem.Category != ItemCategory.MicroHID)
				return;

			ev.Player.Effects.Disable<Invisible>();
		}

		[EventMethod(PlayerEvents.PickupItem)]
		static void FixFreeze(PickupItemEvent ev)
		{
			if (ev.Player.Inventory.Base.UserInventory.Items.Any(x => x.Value.ItemSerial == ev.Pickup.Serial))
			{
				ev.Allowed = false;
                ev.Player.Inventory.AddItem(ev.Pickup.Type);
				ev.Pickup.Destroy();
			}
		}

		[EventMethod(ServerEvents.RemoteAdminCommand, int.MaxValue)]
		static void FixCrashes(RemoteAdminCommandEvent ev)
		{
			if (ev.Sender.SenderId != "SERVER CONSOLE")
				return;

			switch (ev.Name)
			{
				case "forceclass": ev.Allowed = false; return;
				case "give": ev.Allowed = false; return;
				default: return;
			}
		}

		[EventMethod(RoundEvents.Start)]
		static void FixNotSpawn()
		{
			Timing.CallDelayed(1f, () =>
			{
				int pls = Player.List.Count();
				int pls2 = Player.List.Count(x => x.RoleInformation.IsAlive || x.RoleInformation.Role == RoleTypeId.Overwatch);
				if (pls == 0 || pls / 1.5 > pls2)
				{
					Timing.CallDelayed(5f, () =>
					{
						foreach (Player pl in Player.List)
							pl.Client.DimScreen();
						Timing.CallDelayed(1f, () => Extensions.RestartCrush("Не заспавнило больше половины игроков"));
					});
					try { RoundSummary.singleton.RpcShowRoundSummary(RoundSummary.singleton.classlistStart, default, LeadingTeam.Draw, 0, 0, 0, 5, 1); } catch { }
				}
			});
		}


		static int AntiAllFreeze_Count = 0;
		static int AntiAllFreeze_NeedPlayers
			=> Math.Max(Math.Max(Player.List.Count(), 1) / 6 * 4, 5);

		static internal void AntiAllFreeze()
		{
			int laggedCount = 0;

			var list = Player.List.Where(x => (DateTime.Now - x.JoinedTime).TotalMinutes > 1 && x.RoleInformation.IsAlive && !x.Disconnected);

			if (list.Count() < 5)
			{
				AntiAllFreeze_Count = 0;
				return;
			}

			if (Round.Waiting)
			{
				AntiAllFreeze_Count = 0;
				return;
			}

			if (Round.Ended)
			{
				AntiAllFreeze_Count = 0;
				return;
			}

			foreach (var pl in list)
			{
				Vector3 currentPos = pl.MovementState.Position;
				Vector3 currentRot = pl.MovementState.Rotation;

				if (pl.Variables["AntiFreeze_CachePos"] is Vector3 cachedPos &&
					pl.Variables["AntiFreeze_CacheRot"] is Vector3 cachedRot)
				{
					if (Vector3.Distance(cachedPos, currentPos) < 0.1f &&
						Vector3.Distance(cachedRot, currentRot) < 0.1f)
						laggedCount++;
				}

				pl.Variables["AntiFreeze_CachePos"] = currentPos;
				pl.Variables["AntiFreeze_CacheRot"] = currentRot;
			}

			if (laggedCount >= AntiAllFreeze_NeedPlayers)
			{
				AntiAllFreeze_Count++;
			}
			else
			{
				AntiAllFreeze_Count = 0;
			}

			if (AntiAllFreeze_Count == 20)
			{
				Map.Broadcast("<b><color=red>Замечен фриз всех игроков</color></b>\nПодвигайтесь, если это не так", 10, true);
			}

			if (AntiAllFreeze_Count > 30)
			{
				Extensions.RestartCrush("Замечен фриз игроков при проверке позиции и ротации");
			}
		}
	}
}