using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Qurre.API.Events;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Loli.DataBase;
using Loli.Addons;
using System;

namespace Loli.Scps
{
	public class Scp035
	{
		internal const string Tag = " Scp035";
		public static Plugin Plugin;
		public Scp035(Plugin plugin)
        {
			Plugin = plugin;
			CommandsSystem.RegisterRemoteAdmin("scp035", Ra);
		}
		internal const int maxHP = 250;
		private static Pickup Item;
		internal void RoundStart()
		{
			Item = null;
			Timing.RunCoroutine(RefreshItms(Round.CurrentRound));
		}
		internal void AntiGrenade(FlashedEvent ev)
		{
			if (ev.Target == null || ev.Thrower == null) return;
			if (ev.Target.Tag.Contains(Tag) && ev.Thrower.Team == Team.TUT)
			{
				ev.Allowed = false;
			}
			else if (ev.Thrower.Tag.Contains(Tag) && (ev.Target.Team == Team.TUT || ev.Target.Team == Team.SCP))
			{
				ev.Allowed = false;
			}
			else if (ev.Thrower.Team == Team.TUT && ev.Target.Team == Team.SCP)
			{
				ev.Allowed = false;
			}
		}
		internal void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target == null) return;
			if (ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Pickup(PickupItemEvent ev)
		{
			if (!ev.Allowed || ev.Pickup == null || Item != ev.Pickup) return;
			ev.Allowed = false;
			if (ev.Player.GetTeam() != Team.SCP && ev.Player.GetTeam() != Team.TUT)
				InfectPlayer(ev.Player, ev.Pickup);
			else RefreshItems();
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (ev.Attacker == null || ev.Target == null) return;
			if (ev.Target.Tag.Contains(Tag) || ev.Attacker.Tag.Contains(Tag)) ev.FriendlyFire = false;
			if ((ev.Attacker.Tag.Contains(Tag) &&
				ev.Target.Team == Team.SCP) ||
				(ev.Target.Tag.Contains(Tag) &&
				ev.Attacker.Team == Team.SCP))
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}

			if (ev.Attacker.Id != ev.Target.Id &&
				((ev.Attacker.Tag.Contains(Tag) &&
				ev.Target.Team == Team.TUT) ||
				(ev.Target.Tag.Contains(Tag) &&
				ev.Attacker.Team == Team.TUT)))
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}
		}
		internal DateTime LastChange = DateTime.Now;
		internal void Dies(DiesEvent ev)
		{
			if (ev.Killer == null || ev.Target == null) return;
			if (!ev.Killer.Tag.Contains(Tag) || ev.Killer.Id == ev.Target.Id) return;
			if (ev.Target.Team == Team.SCP) return;
			if (ev.Target.Role == RoleType.Spectator) return;
			if ((DateTime.Now - LastChange).TotalMilliseconds < 500) return;
			ev.Killer.ChangeBody(ev.Target.Role, true, ev.Target.Position, ev.Target.Rotation, "Труп сильно разложен, напоминает проказу");
			LastChange = DateTime.Now;
			if (!Plugin.RolePlay) Timing.CallDelayed(0.3f, () => ev.Killer.GetAmmo());
		}
		internal void Dead(DeadEvent ev)
		{
			try
			{
				if (ev.Killer == null || ev.Target == null) return;
				if (ev.Target.Tag.Contains(Tag)) Kill(ev.Target);
				if (!ev.Killer.Tag.Contains(Tag)) return;
				foreach (var doll in Map.Ragdolls.Where(x => x.Owner?.Id == ev.Target.Id)) doll.Destroy();
			}
			catch { }
		}
		internal void Med(HealEvent ev)
		{
			try
			{
				if (ev.Player == null) return;
				if (ev.Player.Tag.Contains(Tag)) ev.Player.MaxHp = maxHP;
			}
			catch { }
		}
		internal void Pocket(PocketEnterEvent ev)
		{
			if (ev.Player == null) return;
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Pocket(PocketFailEscapeEvent ev)
		{
			if (ev.Player == null) return;
			if (!ev.Player.Tag.Contains(Tag)) return;
			ev.Allowed = false;
			Extensions.TeleportTo106(ev.Player);
		}
		internal void Femur(FemurBreakerEnterEvent ev)
		{
			if (ev.Player == null) return;
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Escape(EscapeEvent ev)
		{
			if (ev.Player == null) return;
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Leave(LeaveEvent ev)
		{
			if (ev.Player == null) return;
			if (ev.Player.Tag.Contains(Tag)) Kill(ev.Player);
		}
		internal void Contain(ContainEvent ev)
		{
			if (ev.Player == null) return;
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Cuff(CuffEvent ev)
		{
			if (ev.Target == null) return;
			if (ev.Target.Tag.Contains(Tag)) ev.Allowed = false;
		}
		internal void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Player == null) return;
			if (ev.Player.Tag.Contains(Tag)) ev.Allowed = false;
		}
		private void Ra(SendingRAEvent ev)
		{
			ev.Prefix = "scp035";
			ev.Allowed = false;
			if (ev.CommandSender.SenderId != "-@steam" && ev.CommandSender.SenderId != "SERVER CONSOLE")
			{
				ev.ReplyMessage = "Отказано в доступе";
				return;
			}
			Player player = Player.Get(string.Join(" ", ev.Args));
			if (player is null)
			{
				ev.ReplyMessage = "Игрок не найден!";
				return;
			}
			ev.ReplyMessage = "Успешно";
			Spawn(player);
		}
		private static void RemovePossessedItems()
		{
			try { Item?.Destroy(); } catch { }
			Item = null;
		}
		internal void RefreshItems()
		{
			if (Plugin.ClansWars) return;
			RemovePossessedItems();
			try
			{
				var items = Map.Pickups.Where(x => x.Category != ItemCategory.Ammo && !DataBase.Shop.ItsShop(x)).ToList();
				foreach (var item in items)
				{
					if (Item != null) return;
					var itms = items.Where(x => Vector3.Distance(x.Position, item.Position) < 5).ToList();
					if (itms.Count > 5)
					{
						int it = Extensions.Random.Next(6, 33);
						Pickup p = new Item((ItemType)it).Spawn(item.Position);
						Item = p;
						return;
					}
				}
				if (Item == null) Timing.RunCoroutine(RefreshItms(Round.CurrentRound));
			}
			catch { }
		}
		internal void Kill(Player pl)
		{
			pl.Tag = pl.Tag.Replace(Tag, "");
			Cassie.Send("scp 0 3 5 containment minute");
			Timing.RunCoroutine(RefreshItms(Round.CurrentRound));
			try
			{
				try { Levels.SetPrefix(pl); } catch { pl.SetRank($"0 уровень", "green"); }
				foreach (Player scp in Player.List) try { scp.Scp173Controller.IgnoredPlayers.Remove(pl); } catch { }
			}
			catch { }
		}
		public static void Spawn(Player pl)
		{
			if (Plugin.ClansWars) return;
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (Api.Scp035.Get035().Count > 0) return;
			try { Scp008.Cure(pl); } catch { }
			pl.Tag = Tag;
			pl.Broadcast(10, "<size=30%><color=#707480>Вас заразил <color=red>SCP 035</color>,</color>\n" +
				"<color=#707480>теперь ваша задача <color=#0089c7>-</color> помочь другим <color=red>SCP</color></color></size>");
			Cassie.Send("ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE");
			var _hp = pl.GetMaxHp();
			pl.MaxHp = (int)_hp;
			pl.Hp = _hp;
			try { Levels.SetPrefix(pl); } catch { pl.SetRank("SCP 035", "red"); }
			foreach (Player p in Player.List) try { p.Scp173Controller.IgnoredPlayers.Add(pl); } catch { }
			pl.NicknameSync.Network_customPlayerInfoString = "SCP 035";
			pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
		}
		public void InfectPlayer(Player player, Pickup pItem)
		{
			if (Plugin.ClansWars) return;
			pItem.Destroy();
			Spawn(player);
			RemovePossessedItems();
		}
		internal void CorrodeUpdate()
		{
			try
			{
				var scpList = Player.List.Where(x => x.Tag.Contains(Tag));
				if (Round.Started && scpList.Count() > 0)
				{
					IEnumerable<Player> pList = Player.List.Where(x => !x.Tag.Contains(Tag) &&
					x.Team is not Team.SCP and not Team.TUT and not Team.RIP);
					foreach (var scp in scpList)
					{
						foreach (Player pl in pList)
						{
							if (Vector3.Distance(scp.Position, pl.Position) <= 1.5f)
							{
								pl.Broadcast(1, "<size=25%><color=#6f6f6f>Вас атакует <color=red>SCP 035</color></color></size>", true);
								CorrodePlayer(pl, scp);
							}
							else if (Vector3.Distance(scp.Position, pl.Position) <= 15f)
							{
								pl.Broadcast(1, "<size=25%><color=#f47fff>*<color=#0089c7>принюхивается</color>*</color>\n" +
									"<color=#6f6f6f>Вы чувствуете запах гнили, похоже, это <color=red>SCP 035</color></color></size>", true);
							}
						}
					}
				}
			}
			catch { }
		}
		internal void CorrodeHost()
		{
			foreach (var scp in Player.List)
			{
				if (!scp.Tag.Contains(Tag)) continue;
				scp.Hp -= 1;
				if (scp.Hp <= 0)
				{
					scp.Kill("Погиб от разложения");
					Kill(scp);
				}
			}
		}
		internal IEnumerator<float> RefreshItms(int round)
		{
			RemovePossessedItems();
			yield return Timing.WaitForSeconds(120);
			if (Round.CurrentRound == round) RefreshItems();
			yield break;
		}
		private void CorrodePlayer(Player target, Player scp)
		{
			if (scp is not null)
			{
				int currHP = (int)scp.Hp;
				scp.Hp = currHP + 5 > maxHP ? maxHP : currHP + 5;
			}
			if (target.Hp - 5 > 0) target.Damage(5, "Труп сильно разложен, напоминает проказу");
			else
			{
				scp.ChangeBody(target.Role, true, target.Position, target.Rotation, "Труп сильно разложен, напоминает проказу");
				if (!Plugin.RolePlay) Timing.CallDelayed(0.3f, () => scp.GetAmmo());
				target.Kill("Труп сильно разложен, напоминает проказу");
				foreach (var doll in Map.Ragdolls)
					if (doll.Owner.Id == target.Id) doll.Destroy();
			}
		}
	}
}