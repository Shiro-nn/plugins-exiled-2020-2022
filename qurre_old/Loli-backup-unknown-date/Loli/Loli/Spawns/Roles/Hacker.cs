using Loli.Addons;
using Loli.DataBase;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using Respawning;
using DateTime = System.DateTime;
using System.Collections.Generic;
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
			pl.Tag += HackerTag;
			pl.SetRole(RoleType.ChaosRepressor);
			pl.NicknameSync.Network_customPlayerInfoString = "Хакер";
			pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.PowerStatus;
			if (guard == null)
			{
				var list = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToArray();
				if (list.Length > 0) SpawnHackerGuard(list[Extensions.Random.Next(0, list.Length)]);
			}
			else SpawnHackerGuard(guard);
			Timing.CallDelayed(0.3f, () =>
			{
				pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=red>Хакер</color> <color=green>Повстанцев Хаоса</color>\n" +
					"Ваша задача - взломать комплекс, и выкачать информацию.</color></size>", 10, true);
				pl.Position = new Vector3(176, 985, 39);
				pl.ClearInventory();
				pl.GetAmmo();
				pl.AddItem(ItemType.KeycardChaosInsurgency);
				pl.AddItem(ItemType.GunE11SR);
				pl.AddItem(ItemType.SCP500);
				pl.AddItem(ItemType.ArmorHeavy);
				pl.AddItem(ItemType.Radio);
				pl.AddItem(ItemType.Flashlight);
				pl.AddItem(ItemType.GrenadeFlash);
				pl.AddItem(ItemType.SCP268);
				if (Plugin.RolePlay)
				{
					pl.AhpActiveProcesses.ForEach(x => x.DecayRate = 0);
					pl.MaxAhp = 150;
					pl.Ahp = 150;
				}
			});
		}
		internal static void SpawnHackerGuard(Player pl)
		{
			SpawnManager.SpawnProtect(pl);
			pl.Tag += HackerGuardTag;
			pl.SetRole(RoleType.ChaosRepressor);
			pl.NicknameSync.Network_customPlayerInfoString = "Охранник Хакера";
			pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.PowerStatus;
			Timing.CallDelayed(0.3f, () =>
			{
				pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=red>Охранник Хакера</color> <color=green>Повстанцев Хаоса</color>\n" +
					"Ваша задача - защитить <color=red>хакера</color>.</color></size>", 10, true);
				pl.Position = new Vector3(175, 985, 39);
				pl.ClearInventory();
				pl.GetAmmo();
				pl.AddItem(ItemType.KeycardChaosInsurgency);
				pl.AddItem(ItemType.GunE11SR);
				pl.AddItem(ItemType.GunLogicer);
				pl.AddItem(ItemType.SCP500);
				pl.AddItem(ItemType.GrenadeHE);
				pl.AddItem(ItemType.GrenadeFlash);
				pl.AddItem(ItemType.Flashlight);
				pl.AddItem(ItemType.ArmorHeavy);
			});
			Timing.CallDelayed(0.5f, () => pl.Position = new Vector3(175, 985, 39));
		}
		internal static void FixPos(SpawnEvent ev)
		{
			if (ev.RoleType.GetTeam() != Team.CHI) return;
			if (ev.Player.Tag.Contains(HackerTag)) ev.Position = new Vector3(176, 985, 39);
			else if (ev.Player.Tag.Contains(HackerGuardTag)) ev.Position = new Vector3(175, 985, 39);
		}
		internal static void HackerZero(RoleChangeEvent ev)
		{
			if (ev.NewRole == RoleType.ChaosRepressor) return;
			if (ev.Player.Tag.Contains(HackerTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerTag, "");
				ev.Player.AhpActiveProcesses.ForEach(x => x.DecayRate = 1);
			}
			if (ev.Player.Tag.Contains(HackerGuardTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerGuardTag, "");
			}
		}
		internal static void HackerZero(SpawnEvent ev)
		{
			if (ev.RoleType == RoleType.ChaosRepressor) return;
			if (ev.Player.Tag.Contains(HackerTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerTag, "");
				ev.Player.AhpActiveProcesses.ForEach(x => x.DecayRate = 1);
			}
			if (ev.Player.Tag.Contains(HackerGuardTag))
			{
				ev.Player.Tag = ev.Player.Tag.Replace(HackerGuardTag, "");
			}
		}
		internal static void HackerZero(DeadEvent ev)
		{
			if (ev.Target.Tag.Contains(HackerTag))
			{
				ev.Target.Tag = ev.Target.Tag.Replace(HackerTag, "");
				ev.Target.AhpActiveProcesses.ForEach(x => x.DecayRate = 1);
			}
			if (ev.Target.Tag.Contains(HackerGuardTag))
			{
				ev.Target.Tag = ev.Target.Tag.Replace(HackerGuardTag, "");
			}
		}
	}
}