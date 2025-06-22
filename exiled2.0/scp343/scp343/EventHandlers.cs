using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace scp343
{
	public partial class EventHandlers
	{
		public Plugin plugin;
		public static Plugin plugins;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
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
		public int tranqtime = 0;
		public int moneytime = 0;
		public int lighttime = 0;
		public int doortime = 0;
		public int tptime = 0;
		public int healalltime = 0;
		public int healtime = 0;
		public int esctime = 0;
		public void OnRoundStart()
		{
			tranqtime = 0;
			moneytime = 0;
			lighttime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			esctime = 3;
			roundstart = true;
			escape343 = false;
			scp343 = null;
			players.Clear();
			ffPlayers.Clear();
			scpPlayer = null;
			Timing.CallDelayed(0.4f, () =>
			{
				selectspawnSSS();
			});
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			tranqtime = 0;
			moneytime = 0;
			lighttime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			esctime = 3;
			roundstart = false;
			scp343 = null;
			players.Clear();
			ffPlayers.Clear();
		}
		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (ev.Target == null || scp343 == null) return;
			if (ev.Target.Id == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
			}
		}
		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				ev.Amount = 0;
				if (ev.Target != ev.Attacker)
				{
					if (tranqtime == 0)
					{
						if (20f <= Vector3.Distance(ev.Target.ReferenceHub.transform.position, new Vector3(0, -1995, 0)))
						{
							if (Warhead.IsDetonated) return;
							if (ev.Target.ReferenceHub.characterClassManager.CurClass == RoleType.Scp173) return;
							SleepGood(ev.Target);
							ev.Attacker.ReferenceHub.Hint(plugin.config.tranq.Replace("%player%", $"{ev.Target.ReferenceHub.GetNickname()}"), 5);
							tranqtime = 1;
						}
					}
					else if (tranqtime != 0)
					{
						ReferenceHub victim = ev.Attacker.ReferenceHub;
						ev.Attacker.ReferenceHub.Hint(plugin.config.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						Timing.CallDelayed(1U, () => victim.Hint(plugin.config.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
						Timing.CallDelayed(2U, () => victim.Hint(plugin.config.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
						Timing.CallDelayed(3U, () => victim.Hint(plugin.config.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
						Timing.CallDelayed(4U, () => victim.Hint(plugin.config.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
					}
				}
			}
			if (ev.Target.Id == scp343?.queryProcessor.PlayerId)
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
					Timing.CallDelayed(0.5f, () => Map.Broadcast(plugin.config.scpgodescapebctime, plugin.config.scpgodescapebc));
					Cassie.Message(plugin.config.scpgodescapecassie, false, false);
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
				if (scp343.GetRole() == RoleType.Spectator)
				{
					Killscp343();
					Cassie.Message(plugin.config.scpgodripcassie, false, false);
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
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId && !ev.IsAllowed)
			{
				if (roundtimeint >= plugin.config.initialCooldown)
				{
					if (doortime == 0)
					{
						ev.IsAllowed = true;
					}
					else
					{
						scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - doortime}"), 1);
						Timing.CallDelayed(1U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - doortime}"), 1));
						Timing.CallDelayed(2U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - doortime}"), 1));
						Timing.CallDelayed(3U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - doortime}"), 1));
						Timing.CallDelayed(4U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - doortime}"), 1));
					}
				}
				else if (roundtimeint < plugin.config.initialCooldown)
				{
					if (!ev.IsAllowed)
					{
						int i = plugin.config.initialCooldown - roundtimeint;
						ev.Player.ReferenceHub.Hint(plugin.config.dontaccess.Replace("{0}", $"{plugin.config.initialCooldown - roundtimeint}"), 1);
						Timing.CallDelayed(1U, () => scp343.Hint(plugin.config.dontaccess.Replace("{0}", $"{plugin.config.initialCooldown - roundtimeint}"), 1));
						Timing.CallDelayed(2U, () => scp343.Hint(plugin.config.dontaccess.Replace("{0}", $"{plugin.config.initialCooldown - roundtimeint}"), 1));
						Timing.CallDelayed(3U, () => scp343.Hint(plugin.config.dontaccess.Replace("{0}", $"{plugin.config.initialCooldown - roundtimeint}"), 1));
						Timing.CallDelayed(4U, () => scp343.Hint(plugin.config.dontaccess.Replace("{0}", $"{plugin.config.initialCooldown - roundtimeint}"), 1));
					}
				}
			}
		}
		public void tesla(TriggeringTeslaEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsTriggerable = false;
			}
		}
		public void OnDropItem(DroppingItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				if (ev.Item.id == ItemType.Ammo9mm)
				{
					ev.IsAllowed = false;
					try
					{
						List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && x != scp343).ToList();
						ReferenceHub player = pList[rand.Next(pList.Count)];
						if (player == null)
						{
							ev.Player.ReferenceHub.Hint($"<b><color=#ff0000>Игроки не найдены</color></b>", 5);
							return;
						}
						if (tptime == 0)
						{
							ev.Player.ReferenceHub.SetPosition(player.transform.position);
							ev.Player.ReferenceHub.Hint($"<b><color=#15ff00>Вы телепортированы к {player.GetNickname()}</color></b>", 5);
						}
						else
						{
							scp343.Hint(plugin.config.wait.Replace("{0}", $"{tptime}"), 1);
							Timing.CallDelayed(1U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{tptime}"), 1));
							Timing.CallDelayed(2U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{tptime}"), 1));
							Timing.CallDelayed(3U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{tptime}"), 1));
							Timing.CallDelayed(4U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{tptime}"), 1));
						}
					}
					catch
					{
						ev.Player.ReferenceHub.Hint($"<b><color=#ff0000>Произошла ошибка, повторите позже</color></b>", 5);
					}
				}
				if (ev.Item.id == ItemType.SCP500)
				{
					ev.IsAllowed = false;
					List<ReferenceHub> pList = ReferenceHub.GetAllHubs().Values.Where(x => x.queryProcessor.PlayerId != scp343.queryProcessor.PlayerId && x.GetTeam() != Team.SCP && x.GetTeam() != Team.RIP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
					if (healalltime == 0)
					{
						foreach (ReferenceHub player in pList)
						{
							if (player != null && Vector3.Distance(scp343.transform.position, player.transform.position) <= plugin.config.healDistance)
							{
								player.playerStats.Health = player.playerStats.maxHP;
							}
						}
						ev.Player.ReferenceHub.Hint($"<b><color=#15ff00>Игроки вылечены</color></b>", 5);
						healalltime = 1;
					}
					else
					{
						scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						Timing.CallDelayed(1U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - healalltime}"), 1));
						Timing.CallDelayed(2U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - healalltime}"), 1));
						Timing.CallDelayed(3U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - healalltime}"), 1));
						Timing.CallDelayed(4U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - healalltime}"), 1));
					}
				}
				if (ev.Item.id == ItemType.Coin)
				{
					if (moneytime == 0)
					{
						GameCore.Console.singleton.TypeCommand("/intercom-reset");
						ev.Player.Broadcast(10, plugin.config.icom);
						moneytime = 1;
					}
					else
					{
						scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - moneytime}"), 1);
						Timing.CallDelayed(1U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - moneytime}"), 1));
						Timing.CallDelayed(2U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - moneytime}"), 1));
						Timing.CallDelayed(3U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - moneytime}"), 1));
						Timing.CallDelayed(4U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - moneytime}"), 1));
					}
				}
				if (ev.Item.id == ItemType.Adrenaline)
				{
					if (lighttime == 0)
					{
						ev.Player.CurrentRoom.TurnOffLights(plugin.config.light_off_duration);
						ev.Player.Broadcast(10, plugin.config.light);
						lighttime = 1;
					}
					else
					{
						scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - lighttime}"), 1);
						Timing.CallDelayed(1U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - lighttime}"), 1));
						Timing.CallDelayed(2U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - lighttime}"), 1));
						Timing.CallDelayed(3U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - lighttime}"), 1));
						Timing.CallDelayed(4U, () => scp343.Hint(plugin.config.wait.Replace("{0}", $"{60 - lighttime}"), 1));
					}
				}
				if (ev.Item.id == ItemType.SCP268)
				{
					ev.Player.IsInvisible = !ev.Player.IsInvisible;
					if (ev.Player.IsInvisible) ev.Player.Broadcast(10, plugin.config.kep_on);
					else ev.Player.Broadcast(10, plugin.config.kep_off);
				}
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
				ev.IsAllowed = false;
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
						ev.ReplyMessage = plugin.config.errorinra;
						return;
					}
					ev.ReplyMessage = plugin.config.sucinra343;
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
		public IEnumerator<float> time()
		{
			for (; ; )
			{
				if (tranqtime != 0) tranqtime++;
				if (tranqtime == 60) tranqtime = 0;
				if (moneytime != 0) moneytime++;
				if (moneytime == 120) moneytime = 0;
				if (lighttime != 0) lighttime++;
				if (lighttime == 120) lighttime = 0;
				if (doortime != 0) doortime++;
				if (doortime == 60) doortime = 0;
				if (tptime != 0) tptime++;
				if (tptime == 60) tptime = 0;
				if (healalltime != 0) healalltime++;
				if (healalltime == 60) healalltime = 0;
				if (healtime != 0) healtime++;
				if (healtime == 60) healtime = 0;
				if (roundstart) roundtimeint++;
				if (!roundstart) roundtimeint = 0;
				try { Player.Get(scp343).Health = 777; } catch { }
				yield return Timing.WaitForSeconds(1f);
			}
		}
		internal void Killscp343()
		{
			scp343.inventory.Clear();
			Timing.CallDelayed(0.5f, () => GameCore.Console.singleton.TypeCommand($"/god {scp343.queryProcessor.PlayerId}. 0"));
			scp343.SetRank("");
			scp343 = null;
			esctime = 3;
		}
		public static void Spawn343(ReferenceHub sss)
		{
			scp343 = sss;
			Map.Broadcast(Plugin.u1, Plugin.s1);
			sss.characterClassManager.SetClassIDAdv(RoleType.Tutorial, false);
			sss.playerStats.Health = 777;
			sss.inventory.Clear();
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(0.5f, () => sss.inventory.Clear());
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Ammo9mm));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Coin));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.GunCOM15));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Adrenaline));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => sss.SetRank("SCP 343", "red"));
			Timing.CallDelayed(0.5f, () => sss.SetPosition(Map.GetRandomSpawnPoint(RoleType.ClassD)));
			Timing.CallDelayed(0.5f, () => GameCore.Console.singleton.TypeCommand($"/god {sss.queryProcessor.PlayerId}. 1"));
			sss.ClearBroadcasts();
			sss.Broadcast(Plugin.s2, Plugin.u2);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n " + Plugin.c + "\n----------------------------------------------------------- ", "red");
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
			sss.characterClassManager.SetClassID(RoleType.Tutorial);
			sss.playerStats.Health = 777;
			sss.inventory.Clear();
			Timing.CallDelayed(0.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(0.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.maxHP = 777);
			Timing.CallDelayed(2.5f, () => sss.playerStats.Health = 777);
			Timing.CallDelayed(0.5f, () => sss.inventory.Clear());
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Ammo9mm));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP500));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Coin));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.GunCOM15));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Adrenaline));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.Flashlight));
			Timing.CallDelayed(0.5f, () => sss.SetRank("SCP 343", "red"));
			Timing.CallDelayed(0.5f, () => sss.SetPosition(Map.GetRandomSpawnPoint(RoleType.ClassD)));
			Timing.CallDelayed(0.5f, () => GameCore.Console.singleton.TypeCommand($"/god {sss.queryProcessor.PlayerId}. 1"));
			sss.ClearBroadcasts();
			sss.Broadcast(plugin.config.repbcmsg, plugin.config.repbctime);
			sss.characterClassManager.TargetConsolePrint(sss.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n" + plugin.config.spawnconsolemsg + "\n ----------------------------------------------------------- ", "red");
			hasTag = !string.IsNullOrEmpty(sss.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(sss.serverRoles.HiddenBadge);
			PlayerEffectsController componentInParent2 = sss.GetComponentInParent<PlayerEffectsController>();
			componentInParent2.GetEffect<CustomPlayerEffects.Scp207>();
			componentInParent2.EnableEffect<CustomPlayerEffects.Scp207>();
		}
		public void selectspawnSSS()
		{
			List<Player> pList = Player.List.Where(x => x.Role == RoleType.ClassD && x.UserId != null && x.UserId != string.Empty).ToList();
			if (Player.List.ToList().Count >= plugin.config.minpeople && scp343 == null)
			{
				Spawn343(pList[rand.Next(pList.Count)].ReferenceHub);
			}
		}


		public void SleepGood(Player player)
		{
			int IdkHowToCode = (int)player.Role;
			Vector3 UglyCopy = player.Position;
			try { if (player.Team != Team.SCP && player.Role != RoleType.Tutorial) player.Inventory.DropItem(player.CurrentItemIndex); } catch { }
			List<Inventory.SyncItemInfo> items = player.Inventory.items.ToList();
			if (player.Role == RoleType.Tutorial) IdkHowToCode = 15;
			player.ReferenceHub.Hint(plugin.config.vtranq, 5);
			player.GameObject.GetComponent<RagdollManager>().SpawnRagdoll(player.Position, Quaternion.identity, Vector3.zero, IdkHowToCode,
				new PlayerStats.HitInfo(1000f, player.UserId, DamageTypes.None, player.Id), false,
				player.Nickname, player.Nickname, 0);
			player.Inventory.items.Clear();
			player.ReferenceHub.SetPosition(-229, 1993.7f, -67);
			Timing.RunCoroutine(Sleep2God(player.ReferenceHub, items, UglyCopy, 3f));
		}
		public IEnumerator<float> Sleep2God(ReferenceHub player, List<Inventory.SyncItemInfo> items, Vector3 pos, float time)
		{
			yield return Timing.WaitForSeconds(time);
			player.playerMovementSync.OverridePosition(pos, 0f, false);
			player.SetInventory(items);
			foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
			{
				if (doll.owner.ownerHLAPI_id == player.GetNickname())
				{
					NetworkServer.Destroy(doll.gameObject);
				}
			}
			if (Warhead.IsDetonated)
			{
				if (player.GetCurrentRoom().Zone != ZoneType.Surface) player.Damage(999999, DamageTypes.Nuke);
				if (Vector3.Distance(player.transform.position, new Vector3(0, 1001, 8)) <= 3.6f) player.Damage(999999, DamageTypes.Nuke);
			}
		}
	}
}