using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;
using EXILED.Extensions;
using EXILED;

namespace MajorScientist
{
	partial class EventHandlers
	{



		private static void KillMajorScientist(bool setRank = true)
		{
			if (setRank)
			{
				if (hasTag) MajorScientist.RefreshTag();
				if (isHidden) MajorScientist.HideTag();
				Map.Broadcast($"<color=red>Главный ученый помер.</color>\n <color=aqua>Press F</color>", 10);

				if (Configs.log && true)
					Timing.CallDelayed(0.3f, () => Log.Info("Главный ученый помер. Press F"));

				MajorScientist = null;
			}
		}

		public static void SpawnMS(ReferenceHub MS)
		{
			if (MS != null)
			{


				if (Configs.dsreplace)
				{
					Timing.CallDelayed(0.2f, () => MS.ChangeRole(RoleType.Scientist));
				}

				MS.inventory.Clear();
				MS.AddItem(ItemType.KeycardScientistMajor);
				MS.AddItem(ItemType.SCP500);
				MS.AddItem(ItemType.GunCOM15);
				MS.AddItem(ItemType.Radio);
				MS.AddItem(ItemType.Flashlight);
				MS.AddItem(ItemType.Adrenaline);



				maxHP = MS.playerStats.maxHP;
				MS.playerStats.maxHP = Configs.health;
				MS.playerStats.health = Configs.health;



				if (Configs.dsreplace)
					MS.Broadcast($"Вы <color=yellow><b>ГЛАВНЫЙ УЧЕНЫЙ</b></color>!\nЕсли ты умрешь, то MTF не сможет победить. Ты должен cбежать!", 10);
				else
					MS.Broadcast($"Вы <color=yellow><b>ГЛАВНЫЙ УЧЕНЫЙ</b></color>!\nЕсли ты умрешь, то MTF не сможет победить. Ты должен cбежать!", 10);

				hasTag = !string.IsNullOrEmpty(MS.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(MS.serverRoles.HiddenBadge);
				if (isHidden) MS.RefreshTag();
				MS.SetRank("Главный Ученый", "yellow");
				Timing.CallDelayed(0.5f, () => MS.ammoBox.Networkamount = "9999:9999:9999");

				MajorScientist = MS;

				if (Configs.log)
				{
					if (MajorScientist == null)
					{
						Log.Info("uh-oh, it seeme there's a problem with spawning MS..");
					}
						
				}
			}
		}

		public static void selectspawnMS()
		{

			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scientist && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();

				if (Configs.dsreplace)
				{
					pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				}

				if (pList.Count > 0 && MajorScientist == null)
				{
					SpawnMS(pList[rand.Next(pList.Count)]);
				}

				Timing.CallDelayed(0.5f, () => MajorScientist.effectsController.EnableEffect("SCP-207"));

				if (Configs.log)
					Log.Info("Major Scientist has spawned.");
			}
		}


		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}
		
	}
}
