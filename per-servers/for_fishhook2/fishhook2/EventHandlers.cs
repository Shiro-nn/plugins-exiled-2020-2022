using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using Grenades;
using MEC;
using CustomPlayerEffects;
using static DamageTypes;
namespace fishhook2
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };
		public static ReferenceHub scp682;
		public static ReferenceHub scp939ex;
		public static ReferenceHub scp074;
		private static Scp207 scp207;
		private bool isRoundStarted;
		public static bool pspawned;
		private static System.Random rand = new System.Random();
		public const float dur = 327;
		public static Pickup pist1 = new Pickup();
		public static ItemType pist;
		public static int lvl = 0;
		public static int xp = 0;
		public static bool heal1 = false;
		public static bool heal2 = false;
		public static bool heal3 = false;
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
		public void RoundStart()
		{
			isRoundStarted = true;
			pspawned = false;
			lvl = 0;
			xp = 0; heal1 = false; heal2 = false; heal3 = false;
			Timing.CallDelayed(1f, () =>
			{
				selectspawn682();
				x939ex();
				gp();
			});
			coroutines.Add(Timing.RunCoroutine(scp049heal()));
		}
		public void RoundEnd()
		{
			isRoundStarted = false;
		}
		private static IEnumerator<float> scp049heal()
		{
			if (heal1) heal049lvl1();
			if (heal2) heal049lvl2();
			if (heal3) heal049lvl3();
			yield return Timing.WaitForSeconds(1f);
		}
		public static void heal049lvl1()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub player in pList)
			{
				int currHP = (int)player.playerStats.health;
				player.playerStats.health = currHP + 1 > (float)player.playerStats.maxHP ? (float)player.playerStats.maxHP : currHP + 1;
			}
		}
		public static void heal049lvl2()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub player in pList)
			{
				int currHP = (int)player.playerStats.health;
				player.playerStats.health = currHP + 2 > (float)player.playerStats.maxHP ? (float)player.playerStats.maxHP : currHP + 2;
			}
		}
		public static void heal049lvl3()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub player in pList)
			{
				int currHP = (int)player.playerStats.health;
				player.playerStats.health = currHP + 5 > (float)player.playerStats.maxHP ? (float)player.playerStats.maxHP : currHP + 5;
			}
		}
		public void OnTeamRespawn(ref TeamRespawnEvent ev)
		{
			if (!ev.IsChaos)
			{
				Spawnscp074(ev.ToRespawn[rand.Next(ev.ToRespawn.Count)]);
			}
		}
		public static void Spawnscp074(ReferenceHub scp0741)
		{
			if (scp074 == null)
			{
				scp0741.SetRank("SCP 074", "red");
				scp0741.ClearBroadcasts();
				scp0741.Broadcast(Configs.spawn074bcmsg, Configs.spawn074bctime);
				if (Configs.scp074ci)
				{
					scp0741.ClearInventory();
				}
				scp074 = scp0741;
			}
		}
		public static void selectspawn682()
		{
			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				if (scp682 == null)
				{
					Spawn682(pList[rand.Next(pList.Count)]);
				}
			}
		}

		public static void Spawn682(ReferenceHub JG)
		{
			JG.characterClassManager.SetClassID(RoleType.Scp93989);
			JG.SetRank("SCP 682", "red");
			Cassie.CassieMessage(Configs.scpspawnripcassie, false, false);
			Timing.CallDelayed(0.5f, () => JG.playerStats.maxHP = 5000);
			Timing.CallDelayed(0.5f, () => JG.playerStats.health = 5000f);
			JG.ClearBroadcasts();
			JG.Broadcast(Configs.spawndogbcmsg, Configs.spawndogbctime);
			JG.SetRank("SCP 682", "red");
			scp682 = JG;
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			bool UnbreakableDoorDetected = false;
			if (doorInt.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				foreach (string doorName in unbreakableDoorNames)
					if (doorInt.Door.DoorName.Equals(doorName))
						UnbreakableDoorDetected = true;

				if (!UnbreakableDoorDetected)
					BreakDoor(doorInt);
				if (doorInt.Door.DoorName.Equals("914"))
					doorInt.Allow = true;
				return;
			}
		}
		public void BreakDoor(DoorInteractionEvent door)
		{
			door.Door.DestroyDoor(true);
			door.Door.destroyed = true;
			door.Door.Networkdestroyed = true;
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				ev.Amount = 150f;
			}
			if (ev.Attacker.queryProcessor.PlayerId == scp939ex?.queryProcessor.PlayerId)
			{
				ev.Amount = 100f;
			}
			if (IsThisFrustrating(ev.DamageType) && ThisIsMoreFrustrating(ev.DamageType) == pist && ev.Player != ev.Attacker)
			{
				ev.Amount = 100f;
			}
			if (ev.Player.queryProcessor.PlayerId == scp074?.queryProcessor.PlayerId)
			{
				if (Configs.scp074god)
				{
					ev.Amount = 0f;
				}
				if (ev.Attacker.GetTeam() != Team.SCP)
				{
					ev.Attacker.Damage((int)ev.Amount * 5, DamageTypes.Nuke);
				}
				if (ev.Attacker.GetTeam() == Team.SCP)
				{
					ev.Attacker.Damage(50, DamageTypes.Nuke);
					if (!Configs.scp074god)
					{
						ev.Amount = 10f;
					}
				}
			}
		}
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Killer.characterClassManager.CurClass == RoleType.Scp049 && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.SCP && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.TUT)
			{
				Vector3 pos = ev.Killer.transform.position;
				ev.Player.characterClassManager.SetClassID(RoleType.Scp0492);
				ReferenceHub player = ev.Player;
				Timing.CallDelayed(0.5f, () => player.plyMovementSync.OverridePosition(pos, 0f));
				xp += 50;
				if (lvl == 0)
				{
					ev.Killer.SetRank(Configs.prefix049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}/100"), Configs.prefix049c);
					if (xp == 100)
					{
						lvl++;
						xp -= 100;
						heal1 = true;
						ev.Killer.ClearBroadcasts();
						ev.Killer.Broadcast(Configs.lvlup049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}/200"), Configs.lvlup049t);
					}
				}
				if (lvl == 1)
				{
					ev.Killer.SetRank(Configs.prefix049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}/200"), Configs.prefix049c);
					if (xp == 200)
					{
						lvl++;
						xp -= 200;
						heal1 = false;
						heal2 = true;
						ev.Killer.ClearBroadcasts();
						ev.Killer.Broadcast(Configs.lvlup049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}/500"), Configs.lvlup049t);
					}
				}
				if (lvl == 2)
				{
					ev.Killer.SetRank(Configs.prefix049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}/500"), Configs.prefix049c);
					if (xp == 500)
					{
						lvl++;
						xp -= 500;
						heal2 = false;
						heal3 = true;
						ev.Killer.ClearBroadcasts();
						ev.Killer.Broadcast(Configs.lvlup049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}"), Configs.lvlup049t);
					}
				}
				if (lvl == 3)
				{
					ev.Killer.SetRank(Configs.prefix049.Replace("%lvl%", $"{lvl}").Replace("%xp%", $"{xp}"), Configs.prefix049c);
				}
			}
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				kill682();
			}
			if (ev.Player.queryProcessor.PlayerId == scp074?.queryProcessor.PlayerId)
			{
				kill074();
			}
			if (ev.Player.queryProcessor.PlayerId == scp939ex?.queryProcessor.PlayerId)
			{
				kill939ex();
			}
			if (ev.Player.characterClassManager.CurClass == RoleType.Scp049)
			{
				ev.Player.SetRank("", "default");
				ev.Player.RefreshTag();
				ev.Player.HideTag();
			}
		}
		public void OnSetClass(SetClassEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if ((scp682.GetRole() == RoleType.Spectator))
				{
					kill682();
				}
			}
			if (ev.Player.queryProcessor.PlayerId == scp074?.queryProcessor.PlayerId)
			{
				if ((scp682.GetRole() == RoleType.Spectator))
				{
					kill074();
				}
			}
			if (ev.Player.queryProcessor.PlayerId == scp939ex?.queryProcessor.PlayerId)
			{
				if ((scp939ex.GetRole() == RoleType.Spectator))
				{
					kill939ex();
				}
			}
		}
		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				kill682();
			}
			if (ev.Player.queryProcessor.PlayerId == scp074?.queryProcessor.PlayerId)
			{
				kill074();
			}
			if (ev.Player.queryProcessor.PlayerId == scp939ex?.queryProcessor.PlayerId)
			{
				kill939ex();
			}
		}
		public static void kill682()
		{
			scp682.SetRank("", "default");
			scp682.RefreshTag();
			scp682.HideTag();
			scp682 = null;
		}
		public static void kill074()
		{
			scp074.SetRank("", "default");
			scp074.RefreshTag();
			scp074.HideTag();
			scp074 = null;
		}
		public static void kill939ex()
		{
			scp939ex.SetRank("", "default");
			scp939ex.RefreshTag();
			scp939ex.HideTag();
			scp207.ServerDisable();
			scp939ex = null;
			scp207 = null;
		}
		public static void x939ex()
		{

			if (true)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				if (scp939ex == null)
				{
					Spawn939ex(pList[rand.Next(pList.Count)]);
				}
			}
		}
		public static void gp()
		{
			if (!pspawned)
			{
				if (rand.Next(1, 100) < 33)
				{
					Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp173);
					Pickup a = Map.SpawnItem(ItemType.SCP207, 100000, randomSP);
					pist = a.ItemId;
					a.info.durability = dur;
					pspawned = true;
					return;
				}
				if (rand.Next(1, 100) < 33)
				{
					Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scientist);
					Pickup a = Map.SpawnItem(ItemType.SCP207, 100000, randomSP);
					pist = a.ItemId;
					a.info.durability = dur;
					pspawned = true;
					return;
				}
				if (true)
				{
					Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.ClassD);
					Pickup a = Map.SpawnItem(ItemType.SCP207, 100000, randomSP);
					pist = a.ItemId;
					a.info.durability = dur;
					pspawned = true;
					return;
				}
			}
		}
		public bool IsThisFrustrating(DamageType type)
		{
			return ((type == DamageTypes.Usp && pist == ItemType.GunUSP) ||
				(type == DamageTypes.Com15 && pist == ItemType.GunCOM15) ||
				(type == DamageTypes.E11StandardRifle && pist == ItemType.GunE11SR) ||
				(type == DamageTypes.Logicer && pist == ItemType.GunLogicer) ||
				(type == DamageTypes.MicroHid && pist == ItemType.MicroHID) ||
				(type == DamageTypes.Mp7 && pist == ItemType.GunMP7) ||
				(type == DamageTypes.P90 && pist == ItemType.GunProject90));
		}
		public ItemType ThisIsMoreFrustrating(DamageType type)
		{
			if (type == DamageTypes.Usp) return ItemType.GunUSP;
			else if (type == DamageTypes.Com15) return ItemType.GunCOM15;
			else if (type == DamageTypes.E11StandardRifle) return ItemType.GunE11SR;
			else if (type == DamageTypes.Logicer) return ItemType.GunLogicer;
			else if (type == DamageTypes.MicroHid) return ItemType.MicroHID;
			else if (type == DamageTypes.Mp7) return ItemType.GunMP7;
			else if (type == DamageTypes.P90) return ItemType.GunProject90;
			return ItemType.GunUSP;
		}
		public static void OnPickupItem(ref PickupItemEvent ev)
		{
			if(ev.Item.ItemId == pist)
			{
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Configs.png, Configs.pngt);
			}
		}
		public static void Spawn939ex(ReferenceHub JG)
		{
			scp207 = JG.plyMovementSync._scp207;
			JG.characterClassManager.SetClassID(RoleType.Scp93953);
			JG.SetRank("SCP 939ex", "red");
			JG.ClearBroadcasts();
			JG.Broadcast(Configs.spawn939bcmsg, Configs.spawn939bctime);
			scp207.ServerEnable();
			//scp207.ServerDisable();
			scp939ex = JG;
		}
	}
}