using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Qurre.API.Objects;
using Qurre.API.Attributes;
using Qurre.Events;
using PlayerRoles;
using Qurre.Events.Structs;

namespace Loli.Spawns
{
	static class SerpentsHand
	{
		internal static string HandTag => " SerpentsHand";
		static MapBroadcast bc;
		static bool already = false;
		public static List<Player> SerpentsHands => Player.List.Where(x => x.Tag.Contains(HandTag) && x.RoleInfomation.Role is RoleTypeId.Tutorial).ToList();

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh() => already = false;

		static public Vector3 RandomSpawnPoint
		{
			get
			{
				try
				{
					var spawn = Respawn.GetPosition(RoleTypeId.NtfCaptain);
					return new Vector3(spawn.x - 360, spawn.y, spawn.z);
				}
				catch { return new Vector3(-176, 989, -58); }
			}
		}
		public static bool ItsAliveHand(Player pl) => pl.Tag.Contains(HandTag) && pl.RoleInfomation.Role is RoleTypeId.Tutorial &&
			Modules.EventHandlers.IsAlive(pl.UserInfomation.UserId);

		static public void Spawn()
		{
			if (Addons.OmegaWarhead.InProgress) return;
			if (Alpha.Detonated) return;
			if (already)
			{
				SpawnManager.Spawn();
				return;
			}
			already = true;
			if (Player.List.Where(x => x.RoleInfomation.Role is RoleTypeId.Spectator && !x.GamePlay.Overwatch).Count() == 0) return;
			bc = Map.Broadcast("", 25, true);
			int toSpawn = 15;
			bool spawned = false;
			Timing.RunCoroutine(SpawnPost());
			IEnumerator<float> SpawnPost()
			{
				while (!spawned)
				{
					if (0 >= toSpawn)
					{
						var list = Player.List.Where(x => x.RoleInfomation.Role is RoleTypeId.Spectator && !x.GamePlay.Overwatch).ToList();
						if (list.Count() != 0)
						{
							Cassie.Send("SERPENTS HAND HASENTERED");
							bc.Message = "<size=30%><color=red>Внимание всему персоналу!</color>\n" +
								"<color=#00ffff>Отряд <color=#15ff00>Длань Змеи</color></color> <color=#0089c7>замечен на территории комплекса</color></size>";
						}
						else bc.End();
						list.Shuffle();
						list = list.Take(10).ToList();
						try { SCPDiscordLogs.Api.SendMessage($"<:serpents_hand:840582086544064543> Приехал отряд длань змеи в кол-ве {list.Count} человек."); } catch { }
						foreach (Player sh in list) SpawnOne(sh);
						spawned = true;
						yield break;
					}
					else if (Player.List.Where(x => x.RoleInfomation.Role is RoleTypeId.Spectator && !x.GamePlay.Overwatch).Count() != 0)
					{
						bc.Message = $"<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Замечен отряд <color=#15ff00>Длань Змеи</color></color>\n" +
							$"<color=#0089c7>Они будут на территории комплекса через {toSpawn} секунд</color></size>";
					}
					else bc.Message = "";
					toSpawn--;
					yield return Timing.WaitForSeconds(1f);
				}
			}
		}
		static internal void SpawnOne(Player sh)
		{
			SpawnManager.SpawnProtect(sh);
			sh.Tag += HandTag;
			sh.RoleInfomation.Role = RoleTypeId.Tutorial;
			sh.Inventory.Clear();
			sh.GetAmmo();
			sh.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
			sh.Inventory.AddItem(ItemType.GunE11SR);
			sh.Inventory.AddItem(ItemType.Radio);
			sh.Inventory.AddItem(ItemType.Medkit);
			sh.Inventory.AddItem(ItemType.GrenadeFlash);
			sh.Inventory.AddItem(ItemType.GrenadeHE);
			sh.Inventory.AddItem(ItemType.Flashlight);
			sh.Inventory.AddItem(ItemType.ArmorHeavy);
			sh.HealthInfomation.Hp = 125;
			sh.HealthInfomation.MaxHp = 125;
			string mission = "<color=#00ffdc>Ваша задача - убить всех, кроме <color=red>SCP</color></color>";
			sh.Client.Broadcast(10, $"<size=30%><color=red>Вы</color> — <color=#15ff00>Длань змея</color>\n{mission}</size>", true);
			sh.UserInfomation.CustomInfo = "Длань змея";
			sh.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.PowerStatus;

			if (!Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(sh))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Add(sh);
		}









		public void Pocket(PocketFailEscapeEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
			ev.Player.TeleportTo106();
		}
		public void Pocket(PocketEscapeEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
			ev.Player.TeleportTo106();
		}
		public void Pocket(PocketEnterEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
		}
		public void Femur(FemurBreakerEnterEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
		}

		[EventMethod(ScpEvents.Attack)]
		static void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target.Tag.Contains(HandTag))
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Attack)]
		static void Attack(AttackEvent ev)
		{
			if (ev.Attacker.UserInfomation.Id == 0) return;
			if ((ev.Target.Tag.Contains(HandTag) && (ev.Attacker.GetTeam() is Team.SCPs || ev.DamageType is DamageTypes.Pocket)) ||
				(ev.Attacker.Tag.Contains(HandTag) && ev.Target.GetTeam() is Team.SCPs) ||
				(ev.Target.Tag.Contains(HandTag) && ev.Attacker.Tag.Contains(HandTag) &&
				ev.Target.UserInfomation.Id != ev.Attacker.UserInfomation.Id))
			{
				ev.Allowed = false;
				ev.Damage = 0f;
			}
			else if (ev.Target.Tag.Contains(HandTag) && ev.Attacker.GetTeam() is Team.SCPs)
			{
				ev.Allowed = false;
				ev.Damage = 0f;
			}
			else if (ev.Attacker.Tag.Contains(HandTag) && ev.Target.GetTeam() is Team.SCPs)
			{
				ev.Allowed = false;
				ev.Damage = 0f;
			}
		}

		[EventMethod(PlayerEvents.Dead)]
		static void Dead(DeadEvent ev)
		{
			if (ev.Target is null) return;
			if (!ev.Target.Tag.Contains(HandTag)) return;
			ev.Target.Tag = ev.Target.Tag.Replace(HandTag, "");

			if (Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(ev.Target))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Remove(ev.Target);
		}

		[EventMethod(PlayerEvents.ChangeRole)]
		static void Spawn(ChangeRoleEvent ev)
		{
			if (ev.Player is null) return;
			if (ev.Role is RoleTypeId.Tutorial) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Player.Tag = ev.Player.Tag.Replace(HandTag, "");

			if (Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(ev.Player))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Remove(ev.Player);
		}

		[EventMethod(PlayerEvents.Spawn)]
		static void Spawn(SpawnEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			if (ev.Role is RoleTypeId.Tutorial)
			{
				//ev.Position = RandomSpawnPoint;
				int rand = Random.Range(0, 100);
				if (rand > 50) ev.Position = new Vector3(0, 1002, 8);
				else ev.Position = new Vector3(86, 989, -69);
				return;
			}
			ev.Player.Tag = ev.Player.Tag.Replace(HandTag, "");

			if (Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Contains(ev.Player))
				Qurre.API.Classification.Roles.Scp173.IgnoredPlayers.Remove(ev.Player);
		}

		[EventMethod(PlayerEvents.InteractGenerator)]
		static void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
		}
	}
}