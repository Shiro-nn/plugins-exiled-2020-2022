using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Linq;
using UnityEngine;

namespace Loli.Spawns.Roles
{
	internal static class Hacker
	{
		internal const string HackerTag = " Hacker";
		internal const string HackerGuardTag = " HackGuard";
		internal static void SpawnHacker(Player pl, Player guard = null)
		{
			SpawnManager.SpawnProtect(pl);
			pl.Tag += HackerTag;
			pl.RoleInfomation.SetNew(RoleTypeId.ChaosRepressor, RoleChangeReason.Respawn);
			pl.UserInfomation.CustomInfo = "Хакер";
			pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.PowerStatus;
			if (guard is null)
			{
				var list = Player.List.Where(x => x.RoleInfomation.Role is RoleTypeId.Spectator && !x.GamePlay.Overwatch).ToArray();
				if (list.Length > 0) SpawnHackerGuard(list[Random.Range(0, list.Length)]);
			}
			else SpawnHackerGuard(guard);
			Timing.CallDelayed(0.3f, () =>
			{
				pl.Client.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=red>Хакер</color> <color=green>Повстанцев Хаоса</color>\n" +
					"Ваша задача - взломать комплекс, и выкачать информацию.</color></size>", 10, true);
				pl.MovementState.Position = new Vector3(176, 985, 39);
				pl.Inventory.Clear();
				pl.GetAmmo();
				pl.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
				pl.Inventory.AddItem(ItemType.GunE11SR);
				pl.Inventory.AddItem(ItemType.ParticleDisruptor);
				pl.Inventory.AddItem(ItemType.SCP500);
				pl.Inventory.AddItem(ItemType.ArmorHeavy);
				pl.Inventory.AddItem(ItemType.Radio);
				pl.Inventory.AddItem(ItemType.Flashlight);
				pl.Inventory.AddItem(ItemType.GrenadeFlash);
			});
		}
		internal static void SpawnHackerGuard(Player pl)
		{
			SpawnManager.SpawnProtect(pl);
			pl.Tag += HackerGuardTag;
			pl.RoleInfomation.SetNew(RoleTypeId.ChaosRepressor, RoleChangeReason.Respawn);
			pl.UserInfomation.CustomInfo = "Охранник Хакера";
			pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.PowerStatus;
			Timing.CallDelayed(0.3f, () =>
			{
				pl.Client.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=red>Охранник Хакера</color> <color=green>Повстанцев Хаоса</color>\n" +
					"Ваша задача - защитить <color=red>хакера</color>.</color></size>", 10, true);
				pl.MovementState.Position = new Vector3(175, 985, 39);
				pl.Inventory.Clear();
				pl.GetAmmo();
				pl.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
				pl.Inventory.AddItem(ItemType.GunE11SR);
				pl.Inventory.AddItem(ItemType.GunLogicer);
				pl.Inventory.AddItem(ItemType.SCP500);
				pl.Inventory.AddItem(ItemType.GrenadeHE);
				pl.Inventory.AddItem(ItemType.GrenadeFlash);
				pl.Inventory.AddItem(ItemType.Flashlight);
				pl.Inventory.AddItem(ItemType.ArmorHeavy);
			});
			Timing.CallDelayed(0.5f, () => pl.MovementState.Position = new Vector3(175, 985, 39));
		}

		[EventMethod(PlayerEvents.Spawn)]
		internal static void FixPos(SpawnEvent ev)
		{
			if (ev.Role.GetTeam() is not Team.ChaosInsurgency)
				return;

			if (ev.Player.Tag.Contains(HackerTag))
				ev.Position = new Vector3(176, 985, 39);
			else if (ev.Player.Tag.Contains(HackerGuardTag))
				ev.Position = new Vector3(175, 985, 39);
		}

		[EventMethod(PlayerEvents.ChangeRole)]
		internal static void HackerZero(ChangeRoleEvent ev)
		{
			if (ev.Role is RoleTypeId.ChaosRepressor)
				return;

			if (ev.Player.Tag.Contains(HackerTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerTag, "");
				ev.Player.HealthInfomation.AhpActiveProcesses.ForEach(x => x.DecayRate = 1);
			}
			if (ev.Player.Tag.Contains(HackerGuardTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerGuardTag, "");
			}
		}

		[EventMethod(PlayerEvents.Spawn)]
		internal static void HackerZero(SpawnEvent ev)
		{
			if (ev.Role is RoleTypeId.ChaosRepressor)
				return;

			if (ev.Player.Tag.Contains(HackerTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerTag, "");
				ev.Player.HealthInfomation.AhpActiveProcesses.ForEach(x => x.DecayRate = 1);
			}
			if (ev.Player.Tag.Contains(HackerGuardTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerGuardTag, "");
			}
		}

		[EventMethod(PlayerEvents.Dead)]
		internal static void HackerZero(DeadEvent ev)
		{
			if (ev.Target.Tag.Contains(HackerTag))
			{
				ev.Target.Tag = ev.Target.Tag.Replace(HackerTag, "");
				ev.Target.HealthInfomation.AhpActiveProcesses.ForEach(x => x.DecayRate = 1);
			}
			if (ev.Target.Tag.Contains(HackerGuardTag))
			{
				ev.Target.Tag = ev.Target.Tag.Replace(HackerGuardTag, "");
			}
		}
	}
}