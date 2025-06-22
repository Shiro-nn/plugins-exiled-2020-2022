using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;
using EXILED.Extensions;
using EXILED;

namespace scp682343
{ 
	partial class EventHandlers
	{

		private static void Killscp343(bool setRank = true)//kill scp 343
		{
			if (setRank)
			{
				scp343.SetRank("", "default");
				if (hasTag) scp343.RefreshTag();
				if (isHidden) scp343.HideTag();
				scp343.inventory.Clear();
				SetPlayerScale(scp343.gameObject, 1f, 1f, 1f);
				scp343.GetComponent<CharacterClassManager>().GodMode = false;
				scp343 = null;
			}
		}
		private static void Killscp682(bool setRank = true)//kill 682
		{
			if (setRank)
			{
				dra = false;
				scp682.SetRank("", "default");
				if (hasTag) scp682.RefreshTag();
				if (isHidden) scp682.HideTag();
				Cassie.CassieMessage(Configs.scprepripcassie, false, false);
				scp682 = null;
			}
		}
		private static void repscp682(bool setRank = true)//replace 682
		{
			if (setRank)
			{
				dra = false;
				scp682.SetRank("", "default");
				if (hasTag) scp682.RefreshTag();
				if (isHidden) scp682.HideTag();
				scp682 = null;
			}
		}

		public static void SpawnJG(ReferenceHub JG)//2 events for correct spawn
		{
			if (scp682 == null)
			{
				JG.characterClassManager.SetClassID(RoleType.Scp93989);
				Timing.CallDelayed(1.5f, () => maxHP = JG.playerStats.maxHP);
				Cassie.CassieMessage(Configs.scpspawnripcassie, false, false);
				Timing.CallDelayed(0.5f, () => JG.playerStats.maxHP = 444);
				Timing.CallDelayed(0.5f, () => JG.playerStats.Health = 444);
				Timing.CallDelayed(1.5f, () => JG.playerStats.maxHP = 400);
				Timing.CallDelayed(1.5f, () => JG.playerStats.Health = 400);
				JG.ClearBroadcasts();
				JG.Broadcast(Configs.spawndogbcmsg, Configs.spawndogbctime);
				hasTag = !string.IsNullOrEmpty(JG.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(JG.serverRoles.HiddenBadge);
				if (isHidden) JG.RefreshTag();
				JG.SetRank("SCP 682", "red");
				scp682 = JG;
				dra = true;
				killscpforrep();
				SetPlayerScale(scp682.gameObject, 1.1f, 1.1f, 1.1f);
			}
		}
		public static void Spawn343(ReferenceHub sss)//spawn scp 343
		{
			if (scp343 == null)
			{
				sss.characterClassManager.SetClassID(RoleType.ClassD);
				sss.inventory.Clear();
				maxHP = sss.playerStats.maxHP;
				Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
				Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
				sss.ClearBroadcasts();
				sss.Broadcast(Configs.spawnbcmsg, Configs.spawnbctime);
				sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + Configs.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
				hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
				if (isHidden) sss.RefreshTag();
				sss.SetRank("SCP 343", "red");
				Timing.CallDelayed(1f, () => sss.SetRank("SCP 343", "red"));
				PlayerEffectsController componentInParent2 = sss.GetComponentInParent<PlayerEffectsController>();
				componentInParent2.GetEffect<CustomPlayerEffects.Scp207>();
				componentInParent2.EnableEffect<CustomPlayerEffects.Scp207>();
				sss.AddItem(ItemType.Ammo556);
				sss.AddItem(ItemType.Ammo762);
				sss.AddItem(ItemType.Ammo9mm);
				sss.AddItem(ItemType.Flashlight);
				scp343 = sss;
			}
		}

		public static void selectspawnJG()//2 events for correct spawn
		{

			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				if (scp682 == null)
				{
					SpawnJG(pList[rand.Next(pList.Count)]);
				}
			}
		}
		public static void selectspawnJ()//2 events for correct spawn
		{

			if (true)
			{
				if (Configs.dsreplace)
				{
					List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93953 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
					if (scp682 == null)
					{
						SpawnJG(pList[rand.Next(pList.Count)]);
					}
				}
			}
		}

		public static void selectspawnJG2()//select for replace player disconnect(682)
		{

			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				if (pList.Count == 0) return; 
				SpawnJG2(pList[rand.Next(pList.Count)]);
			}
		}
		public static void SpawnJG2(ReferenceHub JG)//spawn for replace player disconnect(682)
		{
			if (scp682 != null)
			{
				JG.characterClassManager.SetClassID(RoleType.Scp93989);
				Timing.CallDelayed(0.5f, () => JG.playerStats.maxHP = 444);
				Timing.CallDelayed(0.5f, () => JG.playerStats.Health = 444);
				JG.ClearBroadcasts();
				JG.Broadcast(Configs.repdogbcmsg, Configs.repdogbctime);
				hasTag = !string.IsNullOrEmpty(JG.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(JG.serverRoles.HiddenBadge);
				if (isHidden) JG.RefreshTag();
				JG.SetRank("SCP 682", "red");
				scp682 = JG;
				dra = true;
				SetPlayerScale(scp682.gameObject, 1.1f, 1.1f, 1.1f);
			}
		}
		public static void selectspawnSSS2()//select for replace player disconnect(343)
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (pList.Count == 0) return;
			Spawn3432(pList[rand.Next(pList.Count)]);
		}
		public static void Spawn3432(ReferenceHub sss)//spawn for replace player disconnect(343)
		{
			if (scp343 != null)
			{
				sss.characterClassManager.SetClassID(RoleType.ClassD);
				sss.inventory.Clear();
				maxHP = sss.playerStats.maxHP;
				Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = Configs.health);
				Timing.CallDelayed(0.5f, () => sss.playerStats.Health = Configs.health);
				sss.ClearBroadcasts();
				sss.Broadcast(Configs.repbcmsg, Configs.repbctime);
				sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + Configs.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
				hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
				if (isHidden) sss.RefreshTag();
				sss.SetRank("SCP 343", "red");
				sss.AddItem(ItemType.Ammo556);
				sss.AddItem(ItemType.Ammo762);
				sss.AddItem(ItemType.Ammo9mm);
				sss.AddItem(ItemType.Flashlight);
				PlayerEffectsController componentInParent2 = sss.GetComponentInParent<PlayerEffectsController>();
				componentInParent2.GetEffect<CustomPlayerEffects.Scp207>();
				componentInParent2.EnableEffect<CustomPlayerEffects.Scp207>();
				scp343 = sss;
			}
		}
		public static void selectspawnSSS()//search player for spawn 343
		{

			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				if (pList.Count > Configs.minpeople && scp343 == null)
				{
					Spawn343(pList[rand.Next(pList.Count)]);
				}
			}
		}



		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}
		private void GrantFF(ReferenceHub player)
		{
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}

	}
}
