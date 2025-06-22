using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using MEC;
using static DamageTypes;
using System;
using Object = UnityEngine.Object;

namespace fishhook4
{
	partial class EventHandlers
	{
		public Plugin plugin;
		private static System.Random rand = new System.Random();
		public static bool p173 = false;
		public static bool cw = false;
		public static int ss = 0;
        public ItemType sgun;
		public Pickup sgunp;
		public static ReferenceHub dbs;
		public static ReferenceHub scpc;
		public static ReferenceHub scpq;
		public static ReferenceHub scpq1;
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };
		public EventHandlers(Plugin plugin)
		{
			this.plugin = plugin;
			Enum.TryParse("GunE11SR", out sgun);
		}
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
		public void OnRoundStart()
		{
			p173 = false;
			Timing.CallDelayed(rand.Next(Configs.mh, Configs.mmh), () =>
			{
				HomeElderly();
			});
			Timing.CallDelayed(rand.Next(Configs.g3mi, Configs.g3ma), () =>
			{
				gift343();
			});
			Timing.CallDelayed(rand.Next(Configs.mfmi, Configs.mfma), () =>
			{
				mf();
			});
			Timing.CallDelayed(rand.Next(Configs.ctbmi, Configs.ctbma), () =>
			{
				ctb();
			});
			Timing.CallDelayed(rand.Next(Configs.cdmi, Configs.cdma), () =>
			{
				cd();
			});
			Timing.CallDelayed(rand.Next(Configs.gwmi, Configs.gwma), () =>
			{
				gw();
			});
			Timing.CallDelayed(rand.Next(Configs.hdmi, Configs.hdma), () =>
			{
				hd();
			});
			Timing.CallDelayed(rand.Next(Configs.dbmi, Configs.dbma), () =>
			{
				db();
			});
			Timing.CallDelayed(rand.Next(Configs.rdmi, Configs.rdma), () =>
			{
				rd();
			});
			Timing.CallDelayed(rand.Next(Configs.sdmi, Configs.sdma), () =>
			{
				sd();
			});
			Timing.CallDelayed(rand.Next(Configs.gami, Configs.gama), () =>
			{
				ga();
			});
			Timing.CallDelayed(rand.Next(Configs.lrmi, Configs.lrma), () =>
			{
				lr();
			});
			Timing.CallDelayed(rand.Next(Configs.psmi, Configs.psma), () =>
			{
				ps();
			});
			Timing.CallDelayed(rand.Next(Configs.yyomi, Configs.yyoma), () =>
			{
				yyo();
			});
			Timing.CallDelayed(rand.Next(Configs.lfmi, Configs.lfma), () =>
			{
				lf();
			});
			Timing.CallDelayed(rand.Next(Configs.aami, Configs.aama), () =>
			{
				aa();
			});
			Timing.CallDelayed(rand.Next(Configs.pomi, Configs.poma), () =>
			{
				po();
			});
			Timing.CallDelayed(rand.Next(Configs.fhmi, Configs.fhma), () =>
			{
			fh();
			});
			sc();
			sq();
		}
		public void sq()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scientist && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (scpq == null)
			{
				scpq = pList[rand.Next(pList.Count)];
			}

		}
		public void sc()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();

			if (scpc == null)
			{
				Spawnc(pList[rand.Next(pList.Count)]);
			}
		}
		public void Spawnc(ReferenceHub player)
		{
			player.characterClassManager.SetClassID(RoleType.ClassD);
			player.ClearBroadcasts();
			player.Broadcast(Configs.scb, Configs.scbt);
			player.SetRank("SCP-cxk", "aqua");
			scpc = player;
		}
		public void fh()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub d in pList)
			{
				d.AddItem(ItemType.SCP207);
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.fhb, Configs.fhbt);
			}
		}
		public void po()
		{
			ReferenceHub one = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator).FirstOrDefault();
			one.SetRole(RoleType.Scp173);
			ReferenceHub two = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator).FirstOrDefault();
			two.SetRole(RoleType.Scp173);
			ReferenceHub three = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator).FirstOrDefault();
			three.SetRole(RoleType.Scp173);
			Timing.CallDelayed(0.3f, () =>
			{
				one.SetHealth(30f);
				two.SetHealth(30f);
				three.SetHealth(30f);
			});
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.pob, Configs.pobt);
		}
		public void aa()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub g in pList)
			{
				g.ammoBox.Networkamount = "200:200:200";
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.aab, Configs.aabt);
			}
		}
		public void lf()
		{
			Generator079.generators[0].RpcCustomOverchargeForOurBeautifulModCreators(Configs.lft, false);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.lfb, Configs.lfbt);
		}
		public void yyo()
		{
			if(dbs != null)
			{
			dbs.SetRole(RoleType.ClassD);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.yyob, Configs.yyobt);
			}
		}
		public void ps()
		{
			ReferenceHub d = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079).FirstOrDefault();
			d.SetRole(RoleType.Scp079);
			dbs = d;
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.psb, Configs.psbt);
		}
		public void lr()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub g in pList)
			{
				Vector3 gp = g.transform.position;
				g.SetRole(RoleType.Scp0492);
				Timing.CallDelayed(0.3f, () =>
				{
					g.plyMovementSync.OverridePosition(gp, 0f);
				});
			}
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.lrb, Configs.lrbt);
		}
		public void ga()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp0492 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub g in pList)
			{
				Vector3 gp = g.transform.position;
				g.SetRole(RoleType.Scp049);
				Timing.CallDelayed(0.3f, () =>
				{
					g.plyMovementSync.OverridePosition(gp, 0f);
				});
			}
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.gab, Configs.gabt);
		}
		public void sd()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scientist && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub s in pList)
			{
				Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
				foreach (var item in s.inventory.items) items.Add(item);
				s.ClearInventory();
				s.AddItem(ItemType.MicroHID);
				s.AddItem(ItemType.MicroHID);
				s.SetHealth(500f);
				Timing.CallDelayed(0.3f, () =>
				{
					foreach (var item in items) s.inventory.AddNewItem(item.id);
				});
			}
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.sdb, Configs.sdbt);
		}
		public void rd()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub d in pList)
			{
				d.SetRole(RoleType.ClassD);
			}
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.rdb, Configs.rdbt);
		}
		public void db()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			foreach (ReferenceHub d in pList)
			{
				d.AddItem(ItemType.GunProject90);
				d.AddItem(ItemType.GunE11SR);
			}
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.dbb, Configs.dbbt);
		}
		public void hd()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub player = pList[rand.Next(pList.Count)];
			ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			Vector3 pos1 = player.transform.position;
			Quaternion rot = scp106.transform.rotation;
			scp106.plyMovementSync.OverridePosition(pos1, 0f);
			scp106.SetRotation(rot.x, rot.y);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.hdb, Configs.hdbt);
		}
		public void gw()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub player = pList[rand.Next(pList.Count)];
			player.inventory.DropItem(8,8,8,8);
			player.AddItem(ItemType.KeycardO5);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.gwb, Configs.gwbt);
		}
		public void cd()
		{
			p173 = true;
			Timing.CallDelayed(Configs.ctbf, () => p173 = false);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.cdb, Configs.cdbt);
		}
		public void ctb()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub player = pList[rand.Next(pList.Count)];

			Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
			foreach (var item in player.inventory.items) items.Add(item);
			Vector3 pos1 = player.transform.position;
			Quaternion rot = player.transform.rotation;
			int health = (int)player.playerStats.health;
			string ammo = player.ammoBox.Networkamount;

			player.SetRole(RoleType.Scp096);

			Timing.CallDelayed(0.3f, () =>
			{
				player.plyMovementSync.OverridePosition(pos1, 0f);
				player.SetRotation(rot.x, rot.y);
				player.inventory.items.Clear();
				foreach (var item in items) player.inventory.AddNewItem(item.id);
				player.playerStats.health = health;
				player.ammoBox.Networkamount = ammo;
			});
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.ctbb, Configs.ctbbt);
		}
		public void mf()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp096 && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub player = pList[rand.Next(pList.Count)];

			Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
			foreach (var item in player.inventory.items) items.Add(item);
			Vector3 pos1 = player.transform.position;
			Quaternion rot = player.transform.rotation;
			int health = (int)player.playerStats.health;
			string ammo = player.ammoBox.Networkamount;

			player.SetRole(RoleType.Scp106);

			Timing.CallDelayed(0.3f, () =>
			{
				player.plyMovementSync.OverridePosition(pos1, 0f);
				player.SetRotation(rot.x, rot.y);
				player.inventory.items.Clear();
				foreach (var item in items) player.inventory.AddNewItem(item.id);
				player.playerStats.health = health;
				player.ammoBox.Networkamount = ammo;
			});
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.mfb, Configs.mfbt);
		}
		public void HomeElderly()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			pList[rand.Next(pList.Count)].SetRole(RoleType.Scp106);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.hb, Configs.hbt);
		}
		public void gift343()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.IsHuman() && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub player = pList[rand.Next(pList.Count)];
			player.SetRole(RoleType.Scp106);
			player.inventory.ServerDropAll();
			player.AddItem(ItemType.KeycardO5);
			player.AddItem(ItemType.MicroHID);
			player.AddItem(ItemType.Medkit);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.g3b, Configs.g3bt);
		}
		public void hurt(ref PlayerHurtEvent ev)
		{
			if (p173)
			{
				if (ev.Attacker.GetRole() == RoleType.Scp173)
				{
					ev.Amount = 0f;
				}
			}
		}
		public void CheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime >= (double)Configs.alphastart)
			{
				if (Configs.nuke)
				{
					if (!cw)
					{
						Map.StartNuke();
						Map.Broadcast(Configs.aabc, Configs.aabctime);
						cw = true;
					}
				}
			}
		}
		public void RoundStart()
		{
			cw = false;
			ss = 0;
			dbs = null;
			scpq1 = null;
			scpq = null;
			scpc = null;
			Timing.RunCoroutine(spawn2818());
		}

		public void RoundEnd()
		{
			cw = false;
			scpq1 = null;
			scpq = null;
			scpc = null;
			dbs = null;
		}
		public void WarheadCancel(WarheadCancelEvent ev)
		{
			if (cw)
			{
				ev.Allow = false;
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			ReferenceHub hub = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079).FirstOrDefault();
			if (ev.TargetPos == hub.transform.position)
			{
				ss++;
				if(ss == 100)
				{
					hub.SetRole(RoleType.Spectator);
					Cassie.CassieMessage("SCP 0 7 9 contained successfully, containment unit unknown", false, false);
				}
			}
		}
		public IEnumerator<float> spawn2818()
		{
			yield return Timing.WaitForSeconds(2);
			foreach (Pickup item in Object.FindObjectsOfType<Pickup>())
			{
				if (item.ItemId == ItemType.GunCOM15)
				{
					item.ItemId = sgun;
					item.RefreshDurability(true, true);
					sgunp = item;
					yield return -1f;
				}
			}
		}
		public void Shoot(ref ShootEvent ev)
		{
			ReferenceHub hub = ev.Shooter;
			if (ev.Shooter.inventory.NetworkcurItem == sgun)
			{
				if (ev.Shooter.inventory.GetItemInHand().durability < 0)
				{
					int savedAmmo = (int)ev.Shooter.inventory.GetItemInHand().durability;
					ev.Shooter.SetWeaponAmmo(0);
					Timing.CallDelayed(0.2f, () => { 
						hub.SetWeaponAmmo(savedAmmo);
						Vector3 randomSP = hub.transform.position;
						Pickup q = Map.SpawnItem(ItemType.GunE11SR, 100000, randomSP);
						q.ItemId = sgun;
						q = sgunp;
						hub.ClearInventory();
						hub.SetRole(RoleType.Spectator);
					});
					return;
				}
			}
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (IsThisFrustrating(ev.DamageType) && ThisIsMoreFrustrating(ev.DamageType) == sgun && ev.Player != ev.Attacker)
			{
				if (ev.Attacker.GetRole() == RoleType.Scp106)
				{
					ev.Amount = 300f;
					return;
				}
				ev.Amount = 2000f;
			}
			if (ev.Attacker.queryProcessor.PlayerId == scpq1?.queryProcessor.PlayerId)
			{
				ev.Amount = 0f;
			}
		}

		public void OnPickupEvent(ref PickupItemEvent ev)
		{
			if (ev.Item == sgunp)
			{
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Configs.p2b, Configs.p2bt);
			}
		}
		public bool IsThisFrustrating(DamageType type)
		{
			return ((type == DamageTypes.Usp && sgun == ItemType.GunUSP) ||
				(type == DamageTypes.Com15 && sgun == ItemType.GunCOM15) ||
				(type == DamageTypes.E11StandardRifle && sgun == ItemType.GunE11SR) ||
				(type == DamageTypes.Logicer && sgun == ItemType.GunLogicer) ||
				(type == DamageTypes.MicroHid && sgun == ItemType.MicroHID) ||
				(type == DamageTypes.Mp7 && sgun == ItemType.GunMP7) ||
				(type == DamageTypes.P90 && sgun == ItemType.GunProject90));
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
			return ItemType.GunE11SR;
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent ev)
		{
			bool UnbreakableDoorDetected = false;
			if (ev.Player.queryProcessor.PlayerId == scpc?.queryProcessor.PlayerId)
			{
				if (rand.Next(1, 101) > 50)
				{
					foreach (string doorName in unbreakableDoorNames)
						if (ev.Door.DoorName.Equals(doorName))
							UnbreakableDoorDetected = true;

					if (!UnbreakableDoorDetected)
						BreakDoor(ev);

					return;
				}
				ev.Allow = false;
			}
		}
		public void BreakDoor(DoorInteractionEvent door)
		{
			door.Door.DestroyDoor(true);
			door.Door.destroyed = true;
			door.Door.Networkdestroyed = true;
		}
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpq?.queryProcessor.PlayerId)
			{
				ev.Player.SetRole(RoleType.Scp93989);
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Configs.sqb, Configs.sqbt);
				scpq = null;
			}
			if (ev.Player.queryProcessor.PlayerId == scpq1?.queryProcessor.PlayerId)
			{
				scpq1 = null;
			}
		}
	}
}