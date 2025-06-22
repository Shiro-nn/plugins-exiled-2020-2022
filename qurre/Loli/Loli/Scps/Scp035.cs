using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Qurre.API;
using Qurre.API.Controllers;
using Loli.DataBase;
using Loli.Addons;
using System;
using Qurre.Events.Structs;
using Qurre.API.Attributes;
using Qurre.Events;
using PlayerRoles;

namespace Loli.Scps
{
	static class Scp035
	{
		internal const string Tag = " Scp035";
		static Scp035()
		{
			CommandsSystem.RegisterRemoteAdmin("scp035", Ra);
		}

		internal const int maxHP = 250;
		private static Pickup Item;

		[EventMethod(RoundEvents.Start)]
		static void RoundStart()
		{
			Item = null;
			Timing.RunCoroutine(RefreshItms(Round.CurrentRound));
		}

		[EventMethod(EffectEvents.Flashed)]
		static void AntiGrenade(PlayerFlashedEvent ev)
		{
			if (ev.Player == null || ev.Thrower == null)
				return;

			if (ev.Player.Tag.Contains(Tag) && ev.Thrower.RoleInfomation.Team == Team.OtherAlive)
			{
				ev.Allowed = false;
			}
			else if (ev.Thrower.Tag.Contains(Tag) && (ev.Player.RoleInfomation.Team == Team.OtherAlive || ev.Player.RoleInfomation.Team == Team.SCPs))
			{
				ev.Allowed = false;
			}
			else if (ev.Thrower.RoleInfomation.Team == Team.OtherAlive && ev.Player.RoleInfomation.Team == Team.SCPs)
			{
				ev.Allowed = false;
			}
		}

		[EventMethod(ScpEvents.Attack)]
		static void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target == null) return;
			if (ev.Target.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.PickupItem)]
		static void Pickup(PickupItemEvent ev)
		{
			if (!ev.Allowed || ev.Pickup == null || Item != ev.Pickup)
				return;
			ev.Allowed = false;
			if (ev.Player.GetTeam() != Team.SCPs && ev.Player.GetTeam() != Team.OtherAlive)
				InfectPlayer(ev.Player, ev.Pickup);
			else RefreshItems();
		}

		[EventMethod(PlayerEvents.Attack)]
		static void Attack(AttackEvent ev)
		{
			if (ev.Attacker == null || ev.Target == null)
				return;
			if (ev.Target.Tag.Contains(Tag) || ev.Attacker.Tag.Contains(Tag))
				ev.FriendlyFire = false;
			if ((ev.Attacker.Tag.Contains(Tag) &&
				ev.Target.RoleInfomation.Team == Team.SCPs) ||
				(ev.Target.Tag.Contains(Tag) &&
				ev.Attacker.RoleInfomation.Team == Team.SCPs))
			{
				ev.Allowed = false;
				ev.Damage = 0f;
			}

			if (ev.Attacker.UserInfomation.Id != ev.Target.UserInfomation.Id &&
				((ev.Attacker.Tag.Contains(Tag) &&
				ev.Target.RoleInfomation.Team == Team.OtherAlive) ||
				(ev.Target.Tag.Contains(Tag) &&
				ev.Attacker.RoleInfomation.Team == Team.OtherAlive)))
			{
				ev.Allowed = false;
				ev.Damage = 0f;
			}
		}

		static internal DateTime LastChange = DateTime.Now;

		[EventMethod(PlayerEvents.Dies)]
		static void Dies(DiesEvent ev)
		{
			if (ev.Attacker == null || ev.Target == null)
				return;
			if (!ev.Attacker.Tag.Contains(Tag) ||
				ev.Attacker.UserInfomation.Id == ev.Target.UserInfomation.Id)
				return;
			if (ev.Target.RoleInfomation.Team == Team.SCPs)
				return;
			if (ev.Target.RoleInfomation.Role == RoleTypeId.Spectator)
				return;
			if ((DateTime.Now - LastChange).TotalMilliseconds < 500)
				return;

			ev.Attacker.ChangeBody(ev.Target.RoleInfomation.Role, true, ev.Target.MovementState.Position,
				ev.Target.MovementState.Rotation, "Труп сильно разложен, напоминает проказу");
			LastChange = DateTime.Now;
			Timing.CallDelayed(0.3f, () => ev.Attacker.GetAmmo());
		}

		[EventMethod(PlayerEvents.Dead)]
		static void Dead(DeadEvent ev)
		{
			try
			{
				if (ev.Attacker == null || ev.Target == null) return;
				if (ev.Target.Tag.Contains(Tag)) Kill(ev.Target);
				if (!ev.Attacker.Tag.Contains(Tag)) return;
				foreach (var doll in Map.Ragdolls.Where(x => x.Owner?.UserInfomation.Id == ev.Target.UserInfomation.Id))
					doll.Destroy();
			}
			catch { }
		}

		[EventMethod(PlayerEvents.Heal)]
		static void Med(HealEvent ev)
		{
			try
			{
				if (ev.Player is null)
					return;

				if (ev.Player.Tag.Contains(Tag))
					ev.Player.HealthInfomation.MaxHp = maxHP;
			}
			catch { }
		}

		static void Pocket(PocketEnterEvent ev)
		{
			if (ev.Player == null)
				return;
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
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

		static void Femur(FemurBreakerEnterEvent ev)
		{
			if (ev.Player == null)
				return;
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Escape)]
		static void Escape(EscapeEvent ev)
		{
			if (ev.Player == null)
				return;
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (ev.Player == null)
				return;
			if (ev.Player.Tag.Contains(Tag))
				Kill(ev.Player);
		}

		[EventMethod(PlayerEvents.Cuff)]
		static void Cuff(CuffEvent ev)
		{
			if (ev.Target == null)
				return;
			if (ev.Target.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.InteractGenerator)]
		static void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Player == null)
				return;
			if (ev.Player.Tag.Contains(Tag))
				ev.Allowed = false;
		}

		static void Ra(RemoteAdminCommandEvent ev)
		{
			ev.Prefix = "scp035";
			ev.Allowed = false;
			if (ev.Sender.SenderId != "-@steam" && ev.Sender.SenderId != "SERVER CONSOLE")
			{
				ev.Reply = "Отказано в доступе";
				return;
			}
			Player player = string.Join(" ", ev.Args).GetPlayer();
			if (player is null)
			{
				ev.Reply = "Игрок не найден!";
				return;
			}
			ev.Reply = "Успешно";
			Spawn(player);
		}

		static void RemovePossessedItems()
		{
			try { Item?.Destroy(); } catch { }
			Item = null;
		}
		static internal void RefreshItems()
		{
			RemovePossessedItems();
			try
			{
				var items = Map.Pickups.Where(x => x.Category != ItemCategory.Ammo && !Shop.ItsShop(x)).ToList();
				foreach (var item in items)
				{
					if (Item != null) return;
					var itms = items.Where(x => Vector3.Distance(x.Position, item.Position) < 5).ToList();
					if (itms.Count > 5)
					{
						int it = UnityEngine.Random.Range(6, 33);
						Pickup p = new Item((ItemType)it).Spawn(item.Position);
						Item = p;
						return;
					}
				}
				if (Item == null) Timing.RunCoroutine(RefreshItms(Round.CurrentRound));
			}
			catch { }
		}
		static internal void Kill(Player pl)
		{
			pl.Tag = pl.Tag.Replace(Tag, "");
			Cassie.Send("scp 0 3 5 containment minute");
			Timing.RunCoroutine(RefreshItms(Round.CurrentRound));
			try
			{
				try { Levels.SetPrefix(pl); } catch { pl.SetRank($"0 уровень", "green"); }
				if (Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(pl))
					Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Remove(pl);
			}
			catch { }
		}
		static internal void Spawn(Player pl)
		{
			if (Extensions.Get035().Count > 0) return;
			try { Scp008.Cure(pl); } catch { }
			pl.Tag = Tag;
			pl.Client.Broadcast(10, "<size=30%><color=#707480>Вас заразил <color=red>SCP 035</color>,</color>\n" +
				"<color=#707480>теперь ваша задача <color=#0089c7>-</color> помочь другим <color=red>SCP</color></color></size>");
			Cassie.Send("ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE");
			var _hp = pl.GetMaxHp();
			if(_hp > 0)
			{
				pl.HealthInfomation.MaxHp = (int)_hp;
				pl.HealthInfomation.Hp = _hp;
			}
			try { Levels.SetPrefix(pl); } catch { pl.SetRank("SCP 035", "red"); }
			pl.UserInfomation.CustomInfo = "SCP 035";
			pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;

			if (!Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(pl))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Add(pl);
		}
		static internal void InfectPlayer(Player player, Pickup pItem)
		{
			pItem.Destroy();
			Spawn(player);
			RemovePossessedItems();
		}
		static internal void CorrodeUpdate()
		{
			try
			{
				var scpList = Player.List.Where(x => x.Tag.Contains(Tag));
				if (Round.Started && scpList.Count() > 0)
				{
					IEnumerable<Player> pList = Player.List.Where(x => !x.Tag.Contains(Tag) &&
					x.RoleInfomation.Team is not Team.SCPs and not Team.OtherAlive and not Team.Dead);
					foreach (var scp in scpList)
					{
						foreach (Player pl in pList)
						{
							if (Vector3.Distance(scp.MovementState.Position, pl.MovementState.Position) <= 1.5f)
							{
								pl.Client.Broadcast(1, "<size=25%><color=#6f6f6f>Вас атакует <color=red>SCP 035</color></color></size>", true);
								CorrodePlayer(pl, scp);
							}
							else if (Vector3.Distance(scp.MovementState.Position, pl.MovementState.Position) <= 15f)
							{
								pl.Client.Broadcast(1, "<size=25%><color=#f47fff>*<color=#0089c7>принюхивается</color>*</color>\n" +
									"<color=#6f6f6f>Вы чувствуете запах гнили, похоже, это <color=red>SCP 035</color></color></size>", true);
							}
						}
					}
				}
			}
			catch { }
		}
		static internal void CorrodeHost()
		{
			foreach (var scp in Player.List)
			{
				if (!scp.Tag.Contains(Tag)) continue;
				scp.HealthInfomation.Hp -= 1;
				if (scp.HealthInfomation.Hp <= 0)
				{
					scp.HealthInfomation.Kill("Труп сильно разложен. Похоже, проказа.");
					Kill(scp);
				}
			}
		}
		static internal IEnumerator<float> RefreshItms(int round)
		{
			RemovePossessedItems();
			yield return Timing.WaitForSeconds(120);
			if (Round.CurrentRound == round) RefreshItems();
			yield break;
		}
		static void CorrodePlayer(Player target, Player scp)
		{
			if (scp is not null)
			{
				int currHP = (int)(scp.HealthInfomation.Hp);
				scp.HealthInfomation.Hp = currHP + 5 > maxHP ? maxHP : currHP + 5;
			}
			if (target.HealthInfomation.Hp - 5 > 0)
				target.HealthInfomation.Damage(5, "Труп сильно разложен, напоминает проказу");
			else
			{
				scp.ChangeBody(target.RoleInfomation.Role, true, target.MovementState.Position,
					target.MovementState.Rotation, "Труп сильно разложен, напоминает проказу");
				Timing.CallDelayed(0.3f, () => scp.GetAmmo());
				target.HealthInfomation.Kill("Труп сильно разложен, напоминает проказу");
				foreach (var doll in Map.Ragdolls)
					if (doll.Owner.UserInfomation.Id == target.UserInfomation.Id)
						doll.Destroy();
			}
		}
	}
}