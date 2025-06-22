using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;
using EXILED.Extensions;
using EXILED;

namespace scp343
{
	partial class EventHandlers
	{

		private static void Killscp343(bool setRank = true)
		{
			if (setRank)
			{
				scp343.SetRank("", "default");
				if (hasTag) scp343.RefreshTag();
				if (isHidden) scp343.HideTag();
				scp343.inventory.Clear();
				scp343.GetComponent<CharacterClassManager>().GodMode = false;
				scp343 = null;
			}
		}
		public static void Spawn343(ReferenceHub sss)
		{
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.mapspawn, Configs.mapspawnt);
			sss.characterClassManager.SetClassID(RoleType.ClassD);
			sss.inventory.Clear();
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.health = 777);
			sss.ClearBroadcasts();
			sss.Broadcast(Configs.spawnbcmsg, Configs.spawnbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + Configs.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
			if (isHidden) sss.RefreshTag();
			sss.SetRank("SCP 343", "red");
			Timing.CallDelayed(1f, () => sss.SetRank("БОГ (SCP 343)", "red"));
			sss.AddItem(ItemType.SCP268);
			sss.AddItem(ItemType.Ammo9mm);
			sss.AddItem(ItemType.Medkit);
			sss.AddItem(ItemType.SCP500);
			sss.AddItem(ItemType.GunUSP);
			sss.AddItem(ItemType.Flashlight);
			scp343 = sss;
		}
		public static void selectspawnSSS2()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (pList.Count == 0) return;
			Spawn3432(pList[rand.Next(pList.Count)]);
		}
		public static void Spawn3432(ReferenceHub sss)
		{
			sss.characterClassManager.SetClassID(RoleType.ClassD);
			sss.inventory.Clear();
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = Configs.health);
			Timing.CallDelayed(0.5f, () => sss.playerStats.health = Configs.health);
			sss.ClearBroadcasts();
			sss.Broadcast(Configs.repbcmsg, Configs.repbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + Configs.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
			if (isHidden) sss.RefreshTag();
			sss.SetRank("БОГ (SCP 343)", "red");
			sss.AddItem(ItemType.SCP268);
			sss.AddItem(ItemType.Ammo9mm);
			sss.AddItem(ItemType.Medkit);
			sss.AddItem(ItemType.SCP500);
			sss.AddItem(ItemType.GunUSP);
			sss.AddItem(ItemType.Flashlight);
			scp343 = sss;
		}
		public static void selectspawnSSS()
		{
			List<ReferenceHub> pListall = Player.GetHubs().Where(x => x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (pListall.Count > Configs.minpeople && scp343 == null)
			{
				Spawn343(pList[rand.Next(pList.Count)]);
			}
		}
	}
}
