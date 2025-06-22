using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.scp343
{
	public partial class EventHandlers343
	{

		internal void Killscp343()
		{
			ReferenceHub player = scp343;
			scp343.inventory.Clear();
			scp343.GetComponent<CharacterClassManager>().GodMode = false;
			scp343 = null;
			esctime = 3;
			try { plugin.donate.setprefix(player); } catch { player.SetRank("[data deleted] уровень", "green"); }
		}
		public static void Spawn343(ReferenceHub sss)
		{
			scp343 = sss;
			Map.Broadcast(Configs.mapspawnt, Configs.mapspawn);
			sss.characterClassManager.SetClassID(RoleType.ClassD);
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(0.5f, () => sss.inventory.Clear());
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Ammo9mm));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Painkillers));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Medkit));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.GunUSP));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => { try { plugins.donate.setprefix(sss); } catch { sss.SetRank("SCP 343", "red"); }; });
			sss.ClearBroadcasts();
			sss.Broadcast(Configs.spawnbcmsg, Configs.spawnbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n " + Configs.spawnconsolemsg + "\n----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
			if (isHidden) sss.RefreshTag();
			PlayerEffectsController componentInParent2 = sss.GetComponentInParent<PlayerEffectsController>();
			componentInParent2.GetEffect<CustomPlayerEffects.Scp207>();
			componentInParent2.EnableEffect<CustomPlayerEffects.Scp207>();
		}
		public void selectspawnSSS2()
		{
			List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && 
			x.characterClassManager.UserId != null && 
			x.characterClassManager.UserId != string.Empty &&
			!Player.Get(x).IsOverwatchEnabled).ToList();
			if (pList.Count == 0) return;
			Spawn3432(pList[Extensions.Random.Next(pList.Count)]);
		}
		public void Spawn3432(ReferenceHub sss)
		{
			scp343 = sss;
			sss.characterClassManager.SetClassID(RoleType.ClassD);
			sss.inventory.Clear();
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(0.5f, () => sss.inventory.Clear());
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Ammo9mm));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Painkillers));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Medkit));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.GunUSP));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => { try { plugin.donate.setprefix(sss); } catch { sss.SetRank("SCP 343", "red"); }; });
			sss.ClearBroadcasts();
			sss.Broadcast(Configs.repbcmsg, Configs.repbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + Configs.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
			if (isHidden) sss.RefreshTag();
			PlayerEffectsController componentInParent2 = sss.GetComponentInParent<PlayerEffectsController>();
			componentInParent2.GetEffect<CustomPlayerEffects.Scp207>();
			componentInParent2.EnableEffect<CustomPlayerEffects.Scp207>();
		}
		public void selectspawnSSS()
		{
			List<ReferenceHub> pListall = Extensions.GetHubs().Where(x => x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (pListall.Count > Configs.minpeople && scp343 == null)
			{
                Spawn343(pList[Extensions.Random.Next(pList.Count)]);
			}
		}
	}
}
