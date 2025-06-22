using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using HarmonyLib;
using Hints;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace scp343
{
	public partial class EventHandlers343
	{
		public Plugin plugin;
		public static Plugin plugins;
		public EventHandlers343(Plugin plugin) => this.plugin = plugin;
		public static List<ReferenceHub> sList = new List<ReferenceHub>();
		internal static ReferenceHub scp343;
		internal static bool isHidden;
		internal static bool hasTag;
		internal static System.Random rand = new System.Random();
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		public static bool dra = false;
		public DateTime ExpireDate;
		public bool escape343 = false;
		public int roundtimeint = 0;
		public bool roundstart = false;
		public bool scp268for343 = false;
		private static Vector3 alphaon = new Vector3(0, 1001, 8);
		public int tranqtime = 0;
		public int doortime = 0;
		public int tptime = 0;
		public int healalltime = 0;
		public int healtime = 0;
		public int esctime = 0;
		public bool autowarheadstart = false;
		public void OnRoundStart()
		{
			autowarheadstart = false;
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			esctime = 3;
			scp268for343 = false;
			roundstart = true;
			escape343 = false;
			scp343 = null;
			players.Clear();
			ffPlayers.Clear();
			scpPlayer = null;
			Timing.CallDelayed(0.3f, () =>
			{
				selectspawnSSS();
			});
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			esctime = 3;
			scp268for343 = false;
			roundstart = false;
			scp343 = null;
			players.Clear();
			ffPlayers.Clear();
		}

		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				scp268for343 = false;
				Killscp343();
			}
		}
		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				ev.Amount = 0;
			}
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					if (ev.Amount >= 100) return;
				}
				ev.IsAllowed = false;
				ev.Amount = 0f;
				return;
			}
		}
		public void scpzeroninesixe(EnragingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnShoot(ShootingEventArgs ev)
		{
			ReferenceHub hub = ev.Shooter.ReferenceHub;
			if (hub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				hub.weaponManager.GetShootPermission(Team.CDP, true);
				hub.weaponManager.GetShootPermission(Team.CHI, true);
				hub.SetWeaponAmmo(999);
			}
		}
		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				esctime--;
				if (esctime == 0)
				{
					Killscp343();
					Timing.CallDelayed(0.5f, () => Map.ClearBroadcasts());
					Timing.CallDelayed(0.5f, () => Map.Broadcast(Configs.scpgodescapebctime, Configs.scpgodescapebc));
					Cassie.Message(Configs.scpgodescapecassie, false, false);
				}
				else
				{
					ev.IsAllowed = false;
					scp343.Hint($"<b><color=red>Подождите <color=#0089c7>{esctime}</color> секунд</color></b>", 1);
				}
			}
		}
		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if ((scp343.GetRole() == RoleType.Spectator))
				{
					scp268for343 = false;
					Killscp343();
					Cassie.Message(Configs.scpgodripcassie, false, false);
				}
			}
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				try
				{
					Killscp343();
					selectspawnSSS2();
					scp343.SetPosition(ev.Player.ReferenceHub.transform.position);
				}
				catch
				{ }
			}
		}
		public void OnContain106(ContainingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnPocketDimensionEnter(EscapingPocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				Extensions.TeleportTo106(scp343);
			}
		}
		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				Extensions.TeleportTo106(scp343);
			}
		}

		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (ev.Door.DoorName == "012_BOTTOM")
			{
				ev.IsAllowed = false;
			}
			else
			{
				if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
				{
					if (roundtimeint >= Configs.initialCooldown)
					{
						if (!ev.IsAllowed)
						{
							if (doortime == 0)
							{
								ev.IsAllowed = true;
							}
							else
							{
								scp343.Hint(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1);
								Timing.CallDelayed(1U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1));
								Timing.CallDelayed(2U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1));
								Timing.CallDelayed(3U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1));
								Timing.CallDelayed(4U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1));
							}
						}
					}
					else if (roundtimeint < Configs.initialCooldown)
					{
						if (!ev.IsAllowed)
						{
							int i = Configs.initialCooldown - roundtimeint;
							ev.Player.ReferenceHub.Hint(Configs.dontaccess.Replace("{0}", $"{Configs.initialCooldown - roundtimeint}"), 1);
							Timing.CallDelayed(1U, () => scp343.Hint(Configs.dontaccess.Replace("{0}", $"{Configs.initialCooldown - roundtimeint}"), 1));
							Timing.CallDelayed(2U, () => scp343.Hint(Configs.dontaccess.Replace("{0}", $"{Configs.initialCooldown - roundtimeint}"), 1));
							Timing.CallDelayed(3U, () => scp343.Hint(Configs.dontaccess.Replace("{0}", $"{Configs.initialCooldown - roundtimeint}"), 1));
							Timing.CallDelayed(4U, () => scp343.Hint(Configs.dontaccess.Replace("{0}", $"{Configs.initialCooldown - roundtimeint}"), 1));
						}
					}
				}
			}
		}
		public void OnDropItem(DroppingItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (ev.Item.id == ItemType.Ammo9mm)
				{
					try
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && x != scp343).ToList();
						ReferenceHub player = pList[rand.Next(pList.Count)];
						if (player == null)
						{
							ev.Player.ReferenceHub.Hint($"<b><color=#ff0000>Игроки не найдены</color></b>", 5);
							ev.IsAllowed = false;
							return;
						}
						if (tptime == 0)
						{
							ev.Player.ReferenceHub.SetPosition(player.transform.position);
							ev.Player.ReferenceHub.Hint($"<b><color=#15ff00>Вы телепортированы к {player.GetNickname()}</color></b>", 5);
							ev.IsAllowed = false;
						}
						else
						{
							scp343.Hint(Configs.wait.Replace("{0}", $"{tptime}"), 1);
							Timing.CallDelayed(1U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{tptime}"), 1));
							Timing.CallDelayed(2U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{tptime}"), 1));
							Timing.CallDelayed(3U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{tptime}"), 1));
							Timing.CallDelayed(4U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{tptime}"), 1));
						}
					}
					catch
					{
						ev.Player.ReferenceHub.Hint($"<b><color=#ff0000>Произошла ошибка, повторите позже</color></b>", 5);
					}
				}
				if (ev.Item.id == ItemType.Ammo762)
				{
					if (!scp268for343)
					{
						PlayerEffectsController componentInParent2 = scp343.GetComponentInParent<PlayerEffectsController>();
						componentInParent2.GetEffect<CustomPlayerEffects.Scp268>();
						componentInParent2.EnableEffect<CustomPlayerEffects.Scp268>();
						scp268for343 = true;
						ev.IsAllowed = false;
						return;
					}
					else if (scp268for343)
					{
						PlayerEffectsController componentInParent2 = scp343.GetComponentInParent<PlayerEffectsController>();
						componentInParent2.DisableEffect<CustomPlayerEffects.Scp268>();
						scp268for343 = false;
						return;
					}
				}
				if (ev.Item.id == ItemType.SCP500)
				{
					List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.queryProcessor.PlayerId != scp343.queryProcessor.PlayerId && x.GetTeam() != Team.SCP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
					if (healalltime == 0)
					{
						foreach (ReferenceHub player in pList)
						{
							if (player != null && Vector3.Distance(scp343.transform.position, player.transform.position) <= Configs.healDistance)
							{
								player.playerStats.Health = player.playerStats.maxHP;
							}
						}
						ev.Player.ReferenceHub.Hint($"<b><color=#15ff00>Игроки вылечены</color></b>", 5);
						healalltime = 1;
					}
					else
					{
						scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						Timing.CallDelayed(1U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1));
						Timing.CallDelayed(2U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1));
						Timing.CallDelayed(3U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1));
						Timing.CallDelayed(4U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1));
					}
				}
				if (ev.Item.id == ItemType.Medkit)
				{
					List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.queryProcessor.PlayerId != scp343.queryProcessor.PlayerId && x.GetTeam() != Team.SCP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && Vector3.Distance(scp343.transform.position, x.transform.position) <= Configs.healDistance).ToList();
					if (pList.Count == 0)
					{
						ev.Player.ReferenceHub.Hint($"<b><color=#ff0000>Игроки не найдены</color></b>", 5);
					}
					else
					{
						if (healtime == 0)
						{
							ReferenceHub player = pList[rand.Next(pList.Count)];
							player.playerStats.Health = player.playerStats.maxHP;
							ev.Player.ReferenceHub.Hint($"{player.GetNickname()} успешно вылечен", 5);
							healtime = 1;
						}
						else
						{
							scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1);
							Timing.CallDelayed(1U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1));
							Timing.CallDelayed(2U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1));
							Timing.CallDelayed(3U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1));
							Timing.CallDelayed(4U, () => scp343.Hint(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1));
						}
					}
				}
				ev.IsAllowed = false;
			}
		}
		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnFemurEnter(EnteringFemurBreakerEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (ev.Pickup.ItemId == ItemType.GunCOM15 || ev.Pickup.ItemId == ItemType.GunE11SR || ev.Pickup.ItemId == ItemType.GunProject90 || ev.Pickup.ItemId == ItemType.GunMP7 || ev.Pickup.ItemId == ItemType.GunLogicer || ev.Pickup.ItemId == ItemType.GunUSP)
				{
					ev.Pickup.Delete();
					Extensions.SpawnItem(ItemType.Medkit, 10000, ev.Player.ReferenceHub.transform.position);
				}
				else if (ev.Pickup.ItemId == ItemType.MicroHID)
				{
					ev.Pickup.Delete();
					Extensions.SpawnItem(ItemType.MicroHID, 10000, ev.Player.ReferenceHub.transform.position);
				}
				else if (ev.Pickup.ItemId == ItemType.GrenadeFrag || ev.Pickup.ItemId == ItemType.GrenadeFlash)
				{
					ev.Pickup.Delete();
					Extensions.SpawnItem(ItemType.Adrenaline, 10000, ev.Player.ReferenceHub.transform.position);
				}
				else
				{
					ev.Pickup.Delete();
					Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position, ev.Pickup.rotation,
							ev.Pickup.weaponMods.Sight,
							ev.Pickup.weaponMods.Barrel,
							ev.Pickup.weaponMods.Other);
				}
				ev.IsAllowed = false;
			}
		}
		public void OnTeamRespawn(SpawningEventArgs ev)
		{
			ev.Player.ReferenceHub.GetComponent<CharacterClassManager>().GodMode = true;
			Timing.CallDelayed(5f, () => ev.Player.ReferenceHub.GetComponent<CharacterClassManager>().GodMode = false);
		}
		public void OnWarheadCancel(StoppingEventArgs ev)
		{
			try
			{
				if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
				{
					ev.IsAllowed = false;
				}
				if (autowarheadstart)
				{
					ev.IsAllowed = false;
				}
			}
			catch
			{ ev.IsAllowed = false; }
		}
		public void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			try
			{
				string name = string.Join(" ", ev.Arguments.Skip(0));
				ReferenceHub player = Extensions.GetPlayer(name);
				if (ev.Name == "scp343")
				{
					ev.IsAllowed = false;
					if (player == null)
					{
						ev.ReplyMessage = Configs.errorinra;
						return;
					}
					ev.ReplyMessage = Configs.sucinra343;
					Spawn343(player);
				}
			}
			catch
			{
				if (ev.Name == "scp343")
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = "Произошла ошибка";
					return;
				}
			}
		}
		public void medical(UsingMedicalItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId && (ev.Item == ItemType.Adrenaline || ev.Item == ItemType.Painkillers || ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500))
			{
				ev.IsAllowed = false;
			}
		}
		internal void scp914(UpgradingItemsEventArgs ev)
		{
			if (ev.Players.Contains(Player.Get(scp343)))
				ev.Players.Remove(Player.Get(scp343));
		}
		public void OnLockerInteraction(InteractingLockerEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnGenOpen(UnlockingGeneratorEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public IEnumerator<float> tranqtimee()
		{
			for (; ; )
			{
				if (tranqtime != 0) tranqtime++;
				if (tranqtime == 60) tranqtime = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public IEnumerator<float> doortimee()
		{
			for (; ; )
			{
				if (doortime != 0) doortime++;
				if (doortime == 60) doortime = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public IEnumerator<float> tptimee()
		{
			for (; ; )
			{
				if (tptime != 0) tptime++;
				if (tptime == 60) tptime = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public IEnumerator<float> healalltimee()
		{
			for (; ; )
			{
				if (healalltime != 0) healalltime++;
				if (healalltime == 60) healalltime = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public IEnumerator<float> healtimee()
		{
			for (; ; )
			{
				if (healtime != 0) healtime++;
				if (healtime == 60) healtime = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public IEnumerator<float> roundtime()
		{
			for (; ; )
			{
				if (roundstart) roundtimeint++;
				if (!roundstart) roundtimeint = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		internal void Killscp343()
		{
			scp343.inventory.Clear();
			scp343.GetComponent<CharacterClassManager>().GodMode = false;
			scp343 = null;
			esctime = 3;
			scp343.SetRank("");
		}
		public static void Spawn343(ReferenceHub sss)
		{
			scp343 = sss;
			Map.Broadcast(Configs.mapspawnt, Configs.mapspawn);
			sss.characterClassManager.SetClassID(RoleType.ClassD);
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(0.5f, () => sss.inventory.Clear());
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Ammo9mm));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Medkit));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => sss.SetRank("SCP 343", "red"));
			sss.ClearBroadcasts();
			sss.Broadcast(Configs.spawnbcmsg, Configs.spawnbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n " + Configs.spawnconsolemsg + "\n----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
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
			Spawn3432(pList[rand.Next(pList.Count)]);
		}
		public void Spawn3432(ReferenceHub sss)
		{
			scp343 = sss;
			sss.characterClassManager.SetClassID(RoleType.ClassD);
			sss.inventory.Clear();
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(0.5f, () => sss.inventory.Clear());
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Ammo9mm));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Medkit));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => sss.SetRank("SCP 343", "red"));
			sss.ClearBroadcasts();
			sss.Broadcast(Configs.repbcmsg, Configs.repbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + Configs.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
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
				Spawn343(pList[rand.Next(pList.Count)]);
			}
		}
	}

	public static class Extensions
	{
		internal static int CountRoles(Team team)
		{
			ReferenceHub scp343 = EventHandlers343.scp343;

			int count = 0;
			foreach (Player pl in Player.List)
			{
				try
				{
					if (pl.Team == team)
					{
						if (pl.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
						{
							count--;
						}
						count++;

					}
				}
				catch { }
			}
			return count;
		}
		internal static void TeleportTo106(ReferenceHub player)
		{
			try
			{
				ReferenceHub scp106 = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
				Vector3 toded = scp106.transform.position;
				player.SetPosition(toded);
			}
			catch
			{
				foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				{
					if (door.DoorName == "106_PRIMARY")
					{
						player.SetPosition(new Vector3(door.transform.position.x, door.transform.position.y + 1, door.transform.position.z));
					}
				}
			}
		}
		public static RoleType GetRole(this ReferenceHub player)
		{return player.characterClassManager.CurClass;
		}
		public static Team GetTeam(this ReferenceHub player) => GetTeam(GetRole(player));
		public static Team GetTeam(this RoleType roleType)
		{
			switch (roleType)
			{
				case RoleType.ChaosInsurgency:
					return Team.CHI;
				case RoleType.Scientist:
					return Team.RSC;
				case RoleType.ClassD:
					return Team.CDP;
				case RoleType.Scp049:
				case RoleType.Scp93953:
				case RoleType.Scp93989:
				case RoleType.Scp0492:
				case RoleType.Scp079:
				case RoleType.Scp096:
				case RoleType.Scp106:
				case RoleType.Scp173:
					return Team.SCP;
				case RoleType.Spectator:
					return Team.RIP;
				case RoleType.FacilityGuard:
				case RoleType.NtfCadet:
				case RoleType.NtfLieutenant:
				case RoleType.NtfCommander:
				case RoleType.NtfScientist:
					return Team.MTF;
				case RoleType.Tutorial:
					return Team.TUT;
				default:
					return Team.RIP;
			}
		}
		public static Pickup SpawnItem(ItemType itemType, float durability, Vector3 position, Quaternion rotation = default, int sight = 0, int barrel = 0, int other = 0)
			=> HostInventory.SetPickup(itemType, durability, position, rotation, sight, barrel, other);
		public static void SetPosition(this ReferenceHub player, Vector3 position) => player.SetPosition(position.x, position.y, position.z);
		public static void SetPosition(this ReferenceHub player, float x, float y, float z) => player.playerMovementSync.OverridePosition(new Vector3(x, y, z), player.transform.rotation.eulerAngles.y);
		private static Inventory _hostInventory;
		public static Inventory HostInventory
		{
			get
			{
				if (_hostInventory == null)
					_hostInventory = GetPlayer(PlayerManager.localPlayer).inventory;

				return _hostInventory;
			}
		}
		public static ReferenceHub GetPlayer(this GameObject player) => ReferenceHub.GetHub(player);
		public static Dictionary<int, ReferenceHub> IdHubs = new Dictionary<int, ReferenceHub>();
		public static int GetPlayerId(this ReferenceHub player) => player.queryProcessor.PlayerId;
		public static ReferenceHub GetPlayer(int playerId)
		{
			if (IdHubs.ContainsKey(playerId))
				return IdHubs[playerId];

			foreach (ReferenceHub hub in GetHubs())
			{
				if (hub.GetPlayerId() == playerId)
				{
					IdHubs.Add(playerId, hub);

					return hub;
				}
			}

			return null;
		}
		public static string GetUserId(this ReferenceHub player) => player.characterClassManager.UserId;
		public static ReferenceHub GetPlayer(string args)
		{
			try
			{
				if (StrHubs.ContainsKey(args))
					return StrHubs[args];

				ReferenceHub playerFound = null;

				if (short.TryParse(args, out short playerId))
					return GetPlayer(playerId);

				if (args.EndsWith("@steam") || args.EndsWith("@discord") || args.EndsWith("@northwood") || args.EndsWith("@patreon"))
				{

					foreach (ReferenceHub player in GetHubs())
					{
						if (player.GetUserId() == args)
						{
							playerFound = player;

						}
					}
				}
				else
				{

					if (args == "WORLD" || args == "SCP-018" || args == "SCP-575" || args == "SCP-207")
						return null;

					int maxNameLength = 31, lastnameDifference = 31;
					string str1 = args.ToLower();

					foreach (ReferenceHub player in GetHubs())
					{
						if (!player.GetNickname().ToLower().Contains(args.ToLower()))
							continue;

						if (str1.Length < maxNameLength)
						{
							int x = maxNameLength - str1.Length;
							int y = maxNameLength - player.GetNickname().Length;
							string str2 = player.GetNickname();

							for (int i = 0; i < x; i++) str1 += "z";

							for (int i = 0; i < y; i++) str2 += "z";

							int nameDifference = Compute(str1, str2);
							if (nameDifference < lastnameDifference)
							{
								lastnameDifference = nameDifference;
								playerFound = player;

							}
						}
					}
				}

				if (playerFound != null)
					StrHubs.Add(args, playerFound);

				return playerFound;
			}
			catch
			{
				return null;
			}
		}
		internal static int Compute(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			for (int i = 1; i <= n; i++)
			{
				for (int j = 1; j <= m; j++)
				{
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			return d[n, m];
		}
		public static Dictionary<string, ReferenceHub> StrHubs = new Dictionary<string, ReferenceHub>();
		public static void SetInventory(this ReferenceHub player, List<Inventory.SyncItemInfo> items)
		{
			player.inventory.items.Clear();

			foreach (Inventory.SyncItemInfo item in items)
				player.inventory.AddNewItem(item.id, item.durability, item.modSight, item.modBarrel, item.modOther);
		}
		public static void SetWeaponAmmo(this ReferenceHub rh, int amount)
		{
			rh.inventory.items.ModifyDuration(
			rh.inventory.items.IndexOf(rh.inventory.GetItemInHand()),
			amount);
		}
		public static string GetNickname(this ReferenceHub player) => player.nicknameSync.Network_myNickSync;
		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
			{
				player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
			}
			public static Room GetCurrentRoom(this ReferenceHub player)
			{
				Vector3 playerPos = player.transform.position;
				Vector3 end = playerPos - new Vector3(0f, 10f, 0f);
				bool flag = Physics.Linecast(playerPos, end, out RaycastHit raycastHit, -84058629);

				if (!flag || raycastHit.transform == null)
					return null;

				Transform transform = raycastHit.transform;

				while (transform.parent != null && transform.parent.parent != null)
					transform = transform.parent;

				foreach (Room room in Map.Rooms)
					if (room.Position == transform.position)
						return room;

				return new Room();
			}
			public static void AddItem(this ReferenceHub player, ItemType itemType) => player.inventory.AddNewItem(itemType);
		public static void AddItem(this ReferenceHub player, Inventory.SyncItemInfo item) => player.inventory.AddNewItem(item.id, item.durability, item.modSight, item.modBarrel, item.modOther);
		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}
		public static void Hint(this ReferenceHub player, string message, float time)
		{
			player.hints.Show(new TextHint(message.Trim(), new HintParameter[] { new StringHintParameter("") }, HintEffectPresets.FadeInAndOut(0.25f, time, 0f), 10f));
		}
		public static void ClearBroadcasts(this ReferenceHub player) => BroadcastComponent.TargetClearElements(player.scp079PlayerScript.connectionToClient);
		private static Broadcast _broadcast;
		internal static Broadcast BroadcastComponent
		{
			get
			{
				if (_broadcast == null)
					_broadcast = PlayerManager.localPlayer.GetComponent<Broadcast>();

				return _broadcast;
			}
		}
		public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
		public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
	}
	internal static class Configs
	{
		internal static string sucinra343;
		internal static string errorinra;
		internal static bool nuke;
		internal static int initialCooldown;
		internal static string dontaccess;
		internal static string scpgodripcassie;
		internal static string scpgodescapecassie;
		internal static ushort scpgodescapebctime;
		internal static string scpgodescapebc;
		internal static int minpeople;
		internal static string repbcmsg;
		internal static ushort repbctime;
		internal static int health;
		internal static ushort spawnbctime;
		internal static string spawnconsolemsg;
		internal static string spawnbcmsg;
		internal static string mapspawn;
		internal static ushort mapspawnt;
		internal static float healDistance;
		internal static float tranqdur;
		internal static string wait;
		internal static string tranq;
		internal static string vtranq;

		internal static void ReloadConfig(Config Config)
		{
			Configs.sucinra343 = Config.sucinra343;
			Configs.errorinra = Config.errorinra;
			Configs.nuke = Config.nuke;
			Configs.initialCooldown = Config.initialCooldown;
			Configs.dontaccess = Config.dontaccess;
			Configs.scpgodripcassie = Config.scpgodripcassie;
			Configs.scpgodescapecassie = Config.scpgodescapecassie;
			Configs.scpgodescapebctime = Config.scpgodescapebctime;
			Configs.scpgodescapebc = Config.scpgodescapebc;
			Configs.minpeople = Config.minpeople;
			Configs.repbcmsg = Config.repbcmsg;
			Configs.repbctime = Config.repbctime;
			Configs.health = Config.health;
			Configs.spawnbctime = Config.spawnbctime;
			Configs.spawnconsolemsg = Config.spawnconsolemsg;
			Configs.spawnbcmsg = Config.spawnbcmsg;
			Configs.mapspawn = Config.mapspawn;
			Configs.mapspawnt = Config.mapspawnt;
			Configs.healDistance = Config.healDistance;
			Configs.tranqdur = Config.tranqdur;
			Configs.wait = Config.wait;
			Configs.tranq = Config.tranq;
			Configs.vtranq = Config.vtranq;
		}
	}
}

namespace scp343
{
	using System.Text.RegularExpressions;
	using Exiled.API.Enums;
	using Exiled.API.Features;
	using Exiled.API.Interfaces;
	public class Plugin : Plugin<Config>
	{
		#region nostatic
		public EventHandlers343 EventHandlers343;
		#endregion
		#region override
		public override PluginPriority Priority { get; } = PluginPriority.Higher;
		public override string Author { get; } = "fydne";

		public override void OnEnabled()
		{
			base.OnEnabled();
		}
		public override void OnDisabled()
		{
			base.OnDisabled();
		}
		public override void OnRegisteringCommands()
		{
			base.OnRegisteringCommands();
			RegisterEvents();
		}
		public override void OnUnregisteringCommands()
		{
			base.OnUnregisteringCommands();

			UnregisterEvents();
		}
		#endregion
		#region RegEvents
		private void RegisterEvents()
		{
			EventHandlers343 = new EventHandlers343(this);
			Configs.ReloadConfig(base.Config);
			ServerConsole.ReloadServerName();
			Exiled.Events.Handlers.Server.RoundStarted += EventHandlers343.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded += EventHandlers343.OnRoundEnd;
			Exiled.Events.Handlers.Player.Died += EventHandlers343.OnPlayerDie;
			Exiled.Events.Handlers.Player.Hurting += EventHandlers343.OnPlayerHurt;
			Exiled.Events.Handlers.Scp096.Enraging += EventHandlers343.scpzeroninesixe;
			Exiled.Events.Handlers.Player.Shooting += EventHandlers343.OnShoot;
			Exiled.Events.Handlers.Player.Escaping += EventHandlers343.OnCheckEscape;
			Exiled.Events.Handlers.Player.ChangingRole += EventHandlers343.OnSetClass;
			Exiled.Events.Handlers.Player.Left += EventHandlers343.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing += EventHandlers343.OnContain106;
			Exiled.Events.Handlers.Player.EscapingPocketDimension += EventHandlers343.OnPocketDimensionEnter;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers343.OnPocketDimensionDie;
			Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers343.RunOnDoorOpen;
			Exiled.Events.Handlers.Player.DroppingItem += EventHandlers343.OnDropItem;
			Exiled.Events.Handlers.Player.Handcuffing += EventHandlers343.OnPlayerHandcuffed;
			Exiled.Events.Handlers.Player.EnteringFemurBreaker += EventHandlers343.OnFemurEnter;
			Exiled.Events.Handlers.Player.PickingUpItem += EventHandlers343.OnPickupItem;
			Exiled.Events.Handlers.Player.Spawning += EventHandlers343.OnTeamRespawn;
			Exiled.Events.Handlers.Warhead.Stopping += EventHandlers343.OnWarheadCancel;
			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers343.ra;
			Exiled.Events.Handlers.Player.UsingMedicalItem += EventHandlers343.medical;
			Exiled.Events.Handlers.Scp914.UpgradingItems += EventHandlers343.scp914;
			Exiled.Events.Handlers.Player.InteractingLocker += EventHandlers343.OnLockerInteraction;
			Exiled.Events.Handlers.Player.UnlockingGenerator += EventHandlers343.OnGenOpen;
			Timing.RunCoroutine(EventHandlers343.roundtime());
			Timing.RunCoroutine(EventHandlers343.tranqtimee());
			Timing.RunCoroutine(EventHandlers343.doortimee());
			Timing.RunCoroutine(EventHandlers343.tptimee());
			Timing.RunCoroutine(EventHandlers343.healalltimee());
			Timing.RunCoroutine(EventHandlers343.healtimee());
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers343.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers343.OnRoundEnd;
			Exiled.Events.Handlers.Player.Died -= EventHandlers343.OnPlayerDie;
			Exiled.Events.Handlers.Player.Hurting -= EventHandlers343.OnPlayerHurt;
			Exiled.Events.Handlers.Scp096.Enraging -= EventHandlers343.scpzeroninesixe;
			Exiled.Events.Handlers.Player.Shooting -= EventHandlers343.OnShoot;
			Exiled.Events.Handlers.Player.Escaping -= EventHandlers343.OnCheckEscape;
			Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers343.OnSetClass;
			Exiled.Events.Handlers.Player.Left -= EventHandlers343.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing -= EventHandlers343.OnContain106;
			Exiled.Events.Handlers.Player.EscapingPocketDimension -= EventHandlers343.OnPocketDimensionEnter;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers343.OnPocketDimensionDie;
			Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers343.RunOnDoorOpen;
			Exiled.Events.Handlers.Player.DroppingItem -= EventHandlers343.OnDropItem;
			Exiled.Events.Handlers.Player.Handcuffing -= EventHandlers343.OnPlayerHandcuffed;
			Exiled.Events.Handlers.Player.EnteringFemurBreaker -= EventHandlers343.OnFemurEnter;
			Exiled.Events.Handlers.Player.PickingUpItem -= EventHandlers343.OnPickupItem;
			Exiled.Events.Handlers.Player.Spawning -= EventHandlers343.OnTeamRespawn;
			Exiled.Events.Handlers.Warhead.Stopping -= EventHandlers343.OnWarheadCancel;
			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers343.ra;
			Exiled.Events.Handlers.Player.UsingMedicalItem -= EventHandlers343.medical;
			Exiled.Events.Handlers.Scp914.UpgradingItems -= EventHandlers343.scp914;
			Exiled.Events.Handlers.Player.InteractingLocker -= EventHandlers343.OnLockerInteraction;
			Exiled.Events.Handlers.Player.UnlockingGenerator -= EventHandlers343.OnGenOpen;
			EventHandlers343 = null;
		}
		#endregion
	}
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public string sucinra343 { get; set; } = "океей, ты scp343";
		public string errorinra { get; set; } = "Игрок не найден";
		public bool nuke { get; set; } = false;
		public int initialCooldown { get; set; } = 120;
		public string dontaccess { get; set; } = "<b><color=red>Вы сможете открывать двери через {0} секунд!</color></b>";
		public string scpgodripcassie { get; set; } = "scp 3 4 3 CONTAINMENT MINUTE";
		public string scpgodescapecassie { get; set; } = "scp 3 4 3 escape";
		public ushort scpgodescapebctime { get; set; } = 10;
		public string scpgodescapebc { get; set; } = "<color=red>SCP 343 сбежал!</color>";
		public int minpeople { get; set; } = 4;
		public string repbcmsg { get; set; } = "<color=red>Вы заменили вышедшего SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>";
		public ushort repbctime { get; set; } = 10;
		public int health { get; set; } = 777;
		public ushort spawnbctime { get; set; } = 10;
		public string spawnconsolemsg { get; set; } = "Вы заспавнились за SCP 343\n Вы сможете открывать двери через 2 минуты\n Выбросив 7.62 вы станете невидимым\n Выбросив 9mm вы телепортируетесь к рандомному игроку\n Выбросив аптечку вы вылечите ближайшего игрока\n Выбросив SCP 500 вы вылечите группу людей в 5 метрах от себя\n Удачи";
		public string spawnbcmsg { get; set; } = "<color=red>Вы заспавнились за SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>";
		public string mapspawn { get; set; } = "В раунде появился <color=red>SCP 343</color>";
		public ushort mapspawnt { get; set; } = 10;
		public int healDistance { get; set; } = 5;
		public int tranqdur { get; set; } = 5;
		public string wait { get; set; } = "<b><color=#ff0000>Подождите {0} секунд</color></b>";
		public string tranq { get; set; } = "<b><color=#15ff00>Вы успешно усыпили %player%</color></b>";
		public string vtranq { get; set; } = "<b>Вас оглушил <color=red>SCP-343</color></b>";
	}
}