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
				Map.Broadcast(Configs.jd, Configs.jdt);
				Timing.CallDelayed(10, () => Map.ClearBroadcasts());
				if (Configs.log && true)
					Timing.CallDelayed(0.3f, () => Log.Info("Juggernaut is dead :("));

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
					JG.AddItem(ItemType.KeycardChaosInsurgency);
					JG.AddItem(ItemType.GunE11SR);
					JG.AddItem(ItemType.GrenadeFrag);
					JG.AddItem(ItemType.SCP500);
					JG.AddItem(ItemType.Medkit);
					JG.AddItem(ItemType.Medkit);
					JG.AddItem(ItemType.Medkit);
					JG.AddItem(ItemType.Radio);
				});
				JG.ClearBroadcasts();
				JG.Broadcast(Configs.js, Configs.jst);
				hasTag = !string.IsNullOrEmpty(JG.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(JG.serverRoles.HiddenBadge);
				if (isHidden) JG.RefreshTag();
				Timing.CallDelayed(0.5f, () => JG.SetRank(Configs.jug, Configs.jugc));

				Juggernaut = JG;

				Timing.CallDelayed(120, () =>
				{
					if (Juggernaut != null)
					{
						Juggernaut.playerEffectsController.EnableEffect<CustomPlayerEffects.Scp207>();
						GameCore.Console.singleton.TypeCommand($"/doortp {Juggernaut.GetPlayerId()}. 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/open 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/lock 079_FIRST");
						Cassie.CassieMessage("error . SCP 3 6 3 9 escape", false, false);
						Map.ClearBroadcasts();
						Map.Broadcast(Configs.je, Configs.jet);
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

				if (pList.Count > Configs.tp && Juggernaut == null)
				{
					SpawnJG(pList[rand.Next(pList.Count)]);
				}


				if (Configs.log)
					Log.Info("Juggernaut spawned");
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