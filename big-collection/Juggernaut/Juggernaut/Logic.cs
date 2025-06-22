using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;
using EXILED.Extensions;
using EXILED;

namespace Juggernaut
{ 
	partial class EventHandlers
	{



		private static void KillJuggernaut(bool setRank = true)
		{
			if (setRank)
			{
				Juggernaut.SetRank("", "default");
				if (hasTag) Juggernaut.RefreshTag();
				if (isHidden) Juggernaut.HideTag();
				Juggernaut.inventory.Clear();
				Map.ClearBroadcasts();
				Map.Broadcast($"\n <color=lime>Джаггернаут помер.</color>", 10);
				Timing.CallDelayed(10, () => Map.ClearBroadcasts());
				GameCore.Console.singleton.TypeCommand($"/god {Juggernaut.GetPlayerId()}. 0");
				if (Configs.log && true)
					Timing.CallDelayed(0.3f, () => Log.Info("Джаггернаут помер"));

				Juggernaut = null;
			}
		}

		public static void SpawnJG(ReferenceHub JG)
		{
			if (JG != null)
			{
				JG.ChangeRole(RoleType.Tutorial);
				JG.inventory.Clear();
				Timing.CallDelayed(120f, () =>
				{
					for (int i = 2; i < Configs.spawnitems.Count; i++)
					{
						JG.AddItem(ItemType.KeycardChaosInsurgency);
						JG.AddItem(ItemType.GunE11SR);
						JG.AddItem(ItemType.GrenadeFrag);
						JG.AddItem(ItemType.SCP500);
						JG.AddItem(ItemType.Medkit);
						JG.AddItem(ItemType.Medkit);
						JG.AddItem(ItemType.Medkit);
						JG.AddItem(ItemType.Radio);
					}
				});
				JG.ClearBroadcasts();
				JG.Broadcast($"<color=red>Вы заспавнены за Джаггернаута.</color>\n <color=red>Вы против всех</color>\n Вы будете освобождены через 2 минуты", 10);
				hasTag = !string.IsNullOrEmpty(JG.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(JG.serverRoles.HiddenBadge);
				if (isHidden) JG.RefreshTag();
				Timing.CallDelayed(0.5f, () => JG.SetRank("Джаггернаут", "magenta"));
				Timing.CallDelayed(0.5f, () => JG.ammoBox.Networkamount = "9999:9999:9999");

				Juggernaut = JG;

				Timing.CallDelayed(120, () => {
					if (Juggernaut != null)
					{
						Juggernaut.effectsController.EnableEffect("SCP-207");
						GameCore.Console.singleton.TypeCommand($"/doortp {Juggernaut.GetPlayerId()}. 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/open 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/lock 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/cassie error . SCP 3 6 3 9 escape");
						GameCore.Console.singleton.TypeCommand($"/god {Juggernaut.GetPlayerId()}. 0");
						Map.ClearBroadcasts();
						Map.Broadcast($"<color=magenta>Появился Джаггернаут</color>\n <color=red>Он против всех, кроме SCP и Длани</color>", 10);
					}
				});
				if (Configs.log)
				{
					if (Juggernaut == null)
					{
						Log.Info("uh-oh, it seeme there's a problem with spawning JG..");
					}
						
				}
			}
		}

		public static void selectspawnJG()
		{

			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();

				if (Configs.dsreplace)
				{
					pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.FacilityGuard && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				}

				if (pList.Count > 4 && Juggernaut == null)
				{
					SpawnJG(pList[rand.Next(pList.Count)]);
				}


				if (Configs.log)
					Log.Info("Джаггернаут заспавнен");
			}
		}



		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}
		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}

	}
}
