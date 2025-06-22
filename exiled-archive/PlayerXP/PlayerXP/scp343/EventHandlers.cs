using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Grenades;
using PlayerXP.events.hideandseek.API;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PlayerXP.scp343
{
	public partial class EventHandlers343
	{
		public Plugin plugin;
		public EventHandlers343(Plugin plugin) => this.plugin = plugin;
		public static List<ReferenceHub> sList = new List<ReferenceHub>();
		internal static ReferenceHub scp343;
		internal static bool isHidden;
		internal static bool hasTag;
		internal static System.Random rand = new System.Random();
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };
		string[] oneno = { "HID", "LCZ_ARMORY", "012_BOTTOM" };
		public static bool dra = false;
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
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
		public bool autowarheadstart = false;
		private bool Tryhason()
		{
			return hasData.Gethason();
		}
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
			this.Coroutines.Add(Timing.RunCoroutine(roundtime()));
			this.Coroutines.Add(Timing.RunCoroutine(tranqtimee()));
			this.Coroutines.Add(Timing.RunCoroutine(doortimee()));
			this.Coroutines.Add(Timing.RunCoroutine(tptimee()));
			this.Coroutines.Add(Timing.RunCoroutine(healalltimee()));
			this.Coroutines.Add(Timing.RunCoroutine(healtimee()));
			this.Coroutines.Add(Timing.RunCoroutine(alphastart()));
		}
		public void OnRoundStart()
		{
			autowarheadstart = false;
			tranqtime = 0;
			doortime = 0;
			tptime = 0;
			healalltime = 0;
			healtime = 0;
			scp268for343 = false;
			roundstart = true;
			escape343 = false;
			scp343 = null;
			players.Clear();
			ffPlayers.Clear();
			scpPlayer = null;
			Timing.CallDelayed(0.3f, () =>
			{
				if (!plugin.mtfvsci.GamemodeEnabled)
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
			scp268for343 = false;
			roundstart = false;
			scp343 = null;
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
			players.Clear();
			ffPlayers.Clear();
		}

		public void OnRoundRestart()
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
		}

		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				scp268for343 = false;
				Killscp343();
			}
		}
		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			List<Team> p2List = Player.List.Where(x => x.ReferenceHub.queryProcessor.PlayerId != scp343?.queryProcessor.PlayerId).Select(x => Extensions.GetTeam(x.ReferenceHub)).ToList();

			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			if (!p2List.Contains(Team.MTF) && !p2List.Contains(Team.CDP) && !p2List.Contains(Team.RSC) && scp343 != null)
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.CDP) && !p2List.Contains(Team.CHI) && p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && p2List.Contains(Team.MTF) && p2List.Contains(Team.RSC) && scp343 != null))
			{
				ev.IsAllowed = true;
				ev.IsRoundEnded = true;
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
						if (ev.Target.ReferenceHub.characterClassManager.CurClass == RoleType.Scp173) return;
						SleepGood(ev.Target.ReferenceHub);
						ev.Attacker.ReferenceHub.Hint(Configs.tranq.Replace("%player%", $"{ev.Target.ReferenceHub.GetNickname()}"), 5);
						tranqtime = 1;
					}
					else if (tranqtime != 0)
					{
						ReferenceHub victim = ev.Attacker.ReferenceHub;
						ev.Attacker.ReferenceHub.Hint(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						Timing.CallDelayed(1U, () => victim.Hint(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
						Timing.CallDelayed(2U, () => victim.Hint(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
						Timing.CallDelayed(3U, () => victim.Hint(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
						Timing.CallDelayed(4U, () => victim.Hint(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1));
					}
				}
			}
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					if (ev.Amount >= 100) return;
				}
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
				int savedAmmo = (int)hub.inventory.GetItemInHand().durability;
				hub.SetWeaponAmmo(0);
				Timing.CallDelayed(0.2f, () => { hub.SetWeaponAmmo(savedAmmo); });
			}
		}
		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
				Timing.CallDelayed(0.5f, () => Map.ClearBroadcasts());
				Timing.CallDelayed(0.5f, () => Map.Broadcast(Configs.scpgodescapebctime, Configs.scpgodescapebc));
				Cassie.Message(Configs.scpgodescapecassie, false, false);
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
				if (ev.Pickup.itemId == ItemType.Coin || ev.Pickup == shop.shop25 || ev.Pickup == shop.shop24 || ev.Pickup == shop.shop23 || ev.Pickup == shop.shop22 || ev.Pickup == shop.shop21 || ev.Pickup == shop.shop20 ||
					ev.Pickup == shop.shop19 || ev.Pickup == shop.shop18 || ev.Pickup == shop.shop17 || ev.Pickup == shop.shop16 || ev.Pickup == shop.shop15 || ev.Pickup == shop.shop14 ||
					ev.Pickup == shop.shop13 || ev.Pickup == shop.shop12 || ev.Pickup == shop.shop11 || ev.Pickup == shop.shop10 || ev.Pickup == shop.shop9 || ev.Pickup == shop.shop8 ||
					ev.Pickup == shop.shop7 || ev.Pickup == shop.shop6 || ev.Pickup == shop.shop5 || ev.Pickup == shop.shop4 || ev.Pickup == shop.shop3 || ev.Pickup == shop.shop2 || ev.Pickup == shop.shop1)
				{
					return;
				}
				else if (ev.Pickup.ItemId == ItemType.GunCOM15 || ev.Pickup.ItemId == ItemType.GunE11SR || ev.Pickup.ItemId == ItemType.GunProject90 || ev.Pickup.ItemId == ItemType.GunMP7 || ev.Pickup.ItemId == ItemType.GunLogicer || ev.Pickup.ItemId == ItemType.GunUSP)
				{
					ev.Pickup.Delete();
                    PlayerXP.Extensions.SpawnItem(ItemType.Medkit, 100000, ev.Player.ReferenceHub.transform.position);
				}
				else if (ev.Pickup.ItemId == ItemType.MicroHID)
				{
					ev.Pickup.Delete();
                    PlayerXP.Extensions.SpawnItem(ItemType.MicroHID, 100000, ev.Player.ReferenceHub.transform.position);
				}
				else if (ev.Pickup.ItemId == ItemType.GrenadeFrag || ev.Pickup.ItemId == ItemType.GrenadeFlash)
				{
					ev.Pickup.Delete();
                    PlayerXP.Extensions.SpawnItem(ItemType.Adrenaline, 100000, ev.Player.ReferenceHub.transform.position);
                }
                else
				{
					ev.Pickup.Delete();
					PlayerXP.Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position, ev.Pickup.rotation);
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
                ReferenceHub sender = ev.CommandSender.SenderId == "SERVER CONSOLE" || ev.CommandSender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : PlayerXP.Extensions.GetPlayer(ev.CommandSender.SenderId);
                ReferenceHub player = PlayerXP.Extensions.GetPlayer(ev.Arguments[0]);
				if (ev.Name == "scp343")
				{
					ev.IsAllowed = false;
					if (ev.CommandSender.SenderId == "-@steam")
					{
						if (player == null)
						{
							ev.ReplyMessage = Configs.errorinra;
							return;
						}
						ev.ReplyMessage = Configs.sucinra343;
						Spawn343(player);
					}
					else
					{
						ev.ReplyMessage = "Отказано в доступе";
					}
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
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
            {
				ev.IsAllowed = false;
            }
		}
		internal void scp914(UpgradingItemsEventArgs ev)
		{
			foreach (Player player in ev.Players)
			{
				if (player.ReferenceHub.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
				{
					player.ReferenceHub.inventory.Clear();
					Timing.CallDelayed(0.5f, () => player.ReferenceHub.AddItem(ItemType.SCP268));
					Timing.CallDelayed(0.5f, () => player.ReferenceHub.AddItem(ItemType.Ammo9mm));
					Timing.CallDelayed(0.5f, () => player.ReferenceHub.AddItem(ItemType.Medkit));
					Timing.CallDelayed(0.5f, () => player.ReferenceHub.AddItem(ItemType.SCP500));
					Timing.CallDelayed(0.5f, () => player.ReferenceHub.AddItem(ItemType.GunUSP));
					Timing.CallDelayed(0.5f, () => player.ReferenceHub.AddItem(ItemType.Flashlight));
				}
			}
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
		public IEnumerator<float> alphastart()
		{
			for (; ; )
			{
				if (Round.ElapsedTime.Minutes == 16)
				{
					if (!autowarheadstart)
					{
						if (!Tryhason())
						{
							Map.ClearBroadcasts();
							Map.Broadcast(Configs.autoabct, Configs.autoabc);
							Extensions.StartNuke();
							autowarheadstart = true;
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
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
		public void SleepGood(ReferenceHub player)
		{
			int IdkHowToCode = (int)player.characterClassManager.CurClass;
			Vector3 UglyCopy = player.transform.position;
			List<Inventory.SyncItemInfo> items = player.inventory.items.ToList();

			if (player.characterClassManager.CurClass == RoleType.Tutorial) IdkHowToCode = 15;
			player.Hint(Configs.vtranq, 5);
			player.gameObject.GetComponent<RagdollManager>().SpawnRagdoll(player.gameObject.transform.position, Quaternion.identity, Vector3.zero, IdkHowToCode,
				new PlayerStats.HitInfo(1000f, player.characterClassManager.UserId, DamageTypes.None, player.queryProcessor.PlayerId), false,
				player.GetNickname(), player.GetNickname(), 0);
			player.inventory.items.Clear();
			player.SetPosition(-229, 993.7f, -67);
            gate3.editor.Editor.LoadMap(null, "sleep");
			Timing.RunCoroutine(Sleep2God(player, items, UglyCopy, Configs.tranqdur));
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
				if (Vector3.Distance(player.transform.position, alphaon) <= 3.6f) player.Damage(999999, DamageTypes.Nuke);
			}
		}
	}
}