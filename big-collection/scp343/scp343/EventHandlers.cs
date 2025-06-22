using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using Grenades;
using Mirror;
using hideandseek.API;

namespace scp343
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static List<ReferenceHub> sList = new List<ReferenceHub>();
		internal static ReferenceHub scp343;
		private static bool isHidden;
		private static bool hasTag;
		private static System.Random rand = new System.Random();
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };
		string[] oneno = { "HID", "LCZ_ARMORY", "012_BOTTOM" };
		private readonly int grenade_pickup_mask = 1049088;
		private string banorkicktwo;
		private string banorkickone;
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
				selectspawnSSS();
			});
		}

		public void OnRoundEnd()
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

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				scp268for343 = false;
				Killscp343();
			}
		}
		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> p2List = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp343?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if (!p2List.Contains(Team.MTF) && !p2List.Contains(Team.CDP) && !p2List.Contains(Team.RSC) && scp343 != null)
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.CDP) && !p2List.Contains(Team.CHI) && p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && p2List.Contains(Team.MTF) && p2List.Contains(Team.RSC) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Attacker.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Amount = 0;
				if (ev.Player != ev.Attacker)
				{
					if (tranqtime == 0)
					{
						if (ev.Player.characterClassManager.CurClass == RoleType.Scp173) return;
						if (ev.Player.GetTeam() == Team.SCP) return;
						SleepGood(ev.Player);
						ev.Attacker.ClearBroadcasts();
						ev.Attacker.Broadcast(Configs.tranq.Replace("%player%", $"{ev.Player.GetNickname()}"), 5);
						tranqtime = 1;
					}
					else if (tranqtime != 0)
					{
						ReferenceHub victim = ev.Attacker;
						ev.Attacker.ClearBroadcasts();
						ev.Attacker.Broadcast(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						Timing.CallDelayed(1U, () =>
						{
							victim.ClearBroadcasts();
							victim.Broadcast(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						});
						Timing.CallDelayed(2U, () =>
						{
							victim.ClearBroadcasts();
							victim.Broadcast(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						});
						Timing.CallDelayed(3U, () =>
						{
							victim.ClearBroadcasts();
							victim.Broadcast(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						});
						Timing.CallDelayed(4U, () =>
						{
							victim.ClearBroadcasts();
							victim.Broadcast(Configs.wait.Replace("{0}", $"{60 - tranqtime}"), 1);
						});
					}
				}
			}
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					if (ev.Amount >= 100) return;
				}
				ev.Amount = 0f;
				return;
			}
		}
		public IEnumerator<float> alphastart()
		{
			for (; ; )
			{
				if (roundtimeint > Configs.startalpha)
				{
					if (!autowarheadstart)
					{
						Map.ClearBroadcasts();
						Map.Broadcast(Configs.autoabc, Configs.autoabct);
						Map.StartNuke();
						autowarheadstart = true;
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
		public void scpzeroninesixe(ref Scp096EnrageEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void ban2(PlayerBannedEvent ev)
		{
			ExpireDate = new DateTime(ev.Details.Expires).AddHours(2);
		}
		public void ban(PlayerBanEvent ev)
		{
			if (ev.Duration == 0)
			{
				ServerConsole.Disconnect(ev.BannedPlayer.gameObject, Configs.kickmsg.Replace("%admin%", $"{ev.Issuer.GetNickname()}").Replace("%reason%", $"{ev.Reason}"));
			}
			else
			{
				ServerConsole.Disconnect(ev.BannedPlayer.gameObject, Configs.banmsg.Replace("%admin%", $"{ev.Issuer.GetNickname()}").Replace("%reason%", $"{ev.Reason}"));
			}
			Timing.CallDelayed(0.3f, () =>
			{
				if (ev.Duration == 0)
				{
					banorkicktwo = Configs.kick;
					banorkickone = $"";
				}
				if (ev.Duration != 0)
				{
					banorkicktwo = Configs.ban;
					banorkickone = $"{Configs.before}</color> <color=#00ffff> {ExpireDate.ToString("dd/MM/yyyy HH:mm")}";
				}
				Map.Broadcast($"<color=#00ffff>{ev.BannedPlayer.GetNickname()}</color> <color=red>{banorkicktwo}</color> <color=#00ffff>{ev.Issuer.GetNickname()}</color> <color=red>{banorkickone}</color>", Configs.kickbant);
			});
		}
		public void intercom(ref IntercomSpeakEvent ev)
		{
			if (ev.Player.GetRole() == RoleType.Scp93953 || ev.Player.GetRole() == RoleType.Scp93989)
			{
				ev.Allow = true;
			}
		}
		public void tesla(ref TriggerTeslaEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Triggerable = false;
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			ReferenceHub hub = ev.Shooter;
			if (ev.Shooter.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				int savedAmmo = (int)ev.Shooter.inventory.GetItemInHand().durability;
				ev.Shooter.SetWeaponAmmo(0);
				Timing.CallDelayed(0.2f, () => { hub.SetWeaponAmmo(savedAmmo); });
			}
			if (ev.Target == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;
			if (ev.TargetPos != Vector3.zero
				&& Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
			{
				var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
				if (pickup != null && pickup.Rb != null)
				{
					pickup.Rb.AddExplosionForce(Vector3.Distance(ev.TargetPos, ev.Shooter.GetPosition()), ev.Shooter.GetPosition(), 500f, 3f, ForceMode.Impulse);
				}
				var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
				if (grenade != null)
				{
					grenade.NetworkfuseTime = 0.1f;
				}
			}
		}
		public void SleepGood(ReferenceHub player)
		{
			int IdkHowToCode = (int)player.characterClassManager.CurClass;
			Vector3 UglyCopy = player.GetPosition();
			List<Inventory.SyncItemInfo> items = player.inventory.items.ToList();

			if (player.characterClassManager.CurClass == RoleType.Tutorial) IdkHowToCode = 15;
			player.ClearBroadcasts();
			player.Broadcast(Configs.vtranq, 5);
			player.gameObject.GetComponent<RagdollManager>().SpawnRagdoll(player.gameObject.transform.position, Quaternion.identity, IdkHowToCode,
				new PlayerStats.HitInfo(1000f, player.characterClassManager.UserId, DamageTypes.None, player.queryProcessor.PlayerId), false,
				player.GetNickname(), player.GetNickname(), 0);
			player.ClearInventory();
			EventPlugin.GhostedIds.Add(player.queryProcessor.PlayerId);
			player.SetPosition(1, 1, 1);
			Timing.RunCoroutine(Sleep2God(player, items, UglyCopy, Configs.tranqdur));
		}
		public IEnumerator<float> Sleep2God(ReferenceHub player, List<Inventory.SyncItemInfo> items, Vector3 pos, float time)
		{
			yield return Timing.WaitForSeconds(time);
			player.plyMovementSync.OverridePosition(pos, 0f, false);
			player.SetInventory(items);
			EventPlugin.GhostedIds.Remove(player.queryProcessor.PlayerId);
			foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
			{
				if (doll.owner.ownerHLAPI_id == player.GetNickname())
				{
					NetworkServer.Destroy(doll.gameObject);
				}
			}
			if (Map.IsNukeDetonated)
			{
				if (player.GetCurrentRoom().Zone != EXILED.ApiObjects.ZoneType.Surface) player.Kill();
				if (Vector3.Distance(player.GetPosition(), alphaon) <= 3.6f) player.Kill();
			}
		}
		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
				Timing.CallDelayed(0.5f, () => Map.ClearBroadcasts());
				Timing.CallDelayed(0.5f, () => Map.Broadcast(Configs.scpgodescapebc, Configs.scpgodescapebctime));
				Cassie.CassieMessage(Configs.scpgodescapecassie, false, false);
			}
		}
		public void OnSetClass(SetClassEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if ((scp343.GetRole() == RoleType.Spectator))
				{
					scp268for343 = false;
					Killscp343();
					Cassie.CassieMessage(Configs.scpgodripcassie, false, false);
				}
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				try
				{
					Killscp343();
					selectspawnSSS2();
					scp343.SetPosition(ev.Player.GetPosition());
				}
				catch
				{ }
			}
		}
		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				TeleportTo106(scp343);
			}
		}

		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			if (doorInt.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (roundtimeint >= Configs.initialCooldown)
				{
					if (!doorInt.Allow)
					{
						if (doortime == 0)
						{
							doorInt.Allow = true;
						}
						else
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1);
							Timing.CallDelayed(1U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1);
							});
							Timing.CallDelayed(2U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1);
							});
							Timing.CallDelayed(3U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1);
							});
							Timing.CallDelayed(4U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - doortime}"), 1);
							});
						}
					}
				}
				else if (roundtimeint < Configs.initialCooldown)
				{
					if (!doorInt.Allow)
					{
						int i = Configs.initialCooldown - roundtimeint;
						doorInt.Player.ClearBroadcasts();
						doorInt.Player.Broadcast(Configs.dontaccess.Replace("{0}", $"{i}"), 1);
						Timing.CallDelayed(1U, () =>
						{
							int ia = Configs.initialCooldown - roundtimeint;
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.dontaccess.Replace("{0}", $"{ia}"), 1);
						});
						Timing.CallDelayed(2U, () =>
						{
							int iq = Configs.initialCooldown - roundtimeint;
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.dontaccess.Replace("{0}", $"{iq}"), 1);
						});
						Timing.CallDelayed(3U, () =>
						{
							int iz = Configs.initialCooldown - roundtimeint;
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.dontaccess.Replace("{0}", $"{iz}"), 1);
						});
						Timing.CallDelayed(4U, () =>
						{
							int iw = Configs.initialCooldown - roundtimeint;
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.dontaccess.Replace("{0}", $"{iw}"), 1);
						});
					}
				}
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
		public void OnStopCountdown(WarheadCancelEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = Configs.nuke;
			}
		}
		public void OnDropItem(ref DropItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (ev.Item.id == ItemType.Ammo9mm)
				{
					try
					{
						List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && x != scp343).ToList();
						ReferenceHub player = pList[rand.Next(pList.Count)];
						if (player == null)
						{
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(5, $"Игроки не найдены", false);
							ev.Allow = false;
							return;
						}
						if (tptime == 0)
						{
							ev.Player.SetPosition(player.GetPosition());
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(5, $"Вы телепортированы к {player.GetNickname()}", false);
							ev.Allow = false;
						}
						else
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{tptime}"), 1);
							Timing.CallDelayed(1U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{tptime}"), 1);
							});
							Timing.CallDelayed(2U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{tptime}"), 1);
							});
							Timing.CallDelayed(3U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{tptime}"), 1);
							});
							Timing.CallDelayed(4U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{tptime}"), 1);
							});
						}
					}
					catch
					{
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(5, $"Произошла ошибка, повторите позже", false);
					}
				}
				if (ev.Item.id == ItemType.Ammo762)
				{
					if (!scp268for343)
					{
						PlayerEffectsController componentInParent2 = scp343.GetComponentInParent<PlayerEffectsController>();
						scp268for343 = true;
						ev.Allow = false;
						return;
					}
					else if (scp268for343)
					{
						PlayerEffectsController componentInParent2 = scp343.GetComponentInParent<PlayerEffectsController>();
						scp268for343 = false;
						return;
					}
				}
				if (ev.Item.id == ItemType.SCP500)
				{
					List<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp343.queryProcessor.PlayerId && x.GetTeam() != Team.SCP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
					if (healalltime == 0)
					{
						foreach (ReferenceHub player in pList)
						{
							if (player != null && Vector3.Distance(scp343.transform.position, player.transform.position) <= Configs.healDistance)
							{
								player.playerStats.health = player.playerStats.maxHP;
							}
						}
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(5, $"Игроки вылечены", false);
						healalltime = 1;
					}
					else
					{
						scp343.ClearBroadcasts();
						scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						Timing.CallDelayed(1U, () =>
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						});
						Timing.CallDelayed(2U, () =>
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						});
						Timing.CallDelayed(3U, () =>
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						});
						Timing.CallDelayed(4U, () =>
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healalltime}"), 1);
						});
					}
				}
				if (ev.Item.id == ItemType.Medkit)
				{
					List<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp343.queryProcessor.PlayerId && x.GetTeam() != Team.SCP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && Vector3.Distance(scp343.transform.position, x.transform.position) <= Configs.healDistance).ToList();
					if (pList.Count == 0)
					{
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(5, $"Игроки не найдены", false);
					}
					else
					{
						if (healtime == 0)
						{
							ReferenceHub player = pList[rand.Next(pList.Count)];
							player.playerStats.health = player.playerStats.maxHP;
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(5, $"{player.GetNickname()} успешно вылечен", false);
							healtime = 1;
						}
						else
						{
							scp343.ClearBroadcasts();
							scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1);
							Timing.CallDelayed(1U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1);
							});
							Timing.CallDelayed(2U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1);
							});
							Timing.CallDelayed(3U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1);
							});
							Timing.CallDelayed(4U, () =>
							{
								scp343.ClearBroadcasts();
								scp343.Broadcast(Configs.wait.Replace("{0}", $"{60 - healtime}"), 1);
							});
						}
					}
				}
				ev.Allow = false;
			}
		}
		public void OnPlayerHandcuffed(ref HandcuffEvent ev)
		{
			if (ev.Target.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public void OnFemurEnter(FemurEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		private void TeleportTo106(ReferenceHub player)
		{
			ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			Vector3 toded = scp106.GetPosition();
			if (scp106 != null)
			{

				Timing.CallDelayed(1f, () => player.SetPosition(toded));
			}
		}
		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if(ev.Item.ItemId == ItemType.GunCOM15 || ev.Item.ItemId == ItemType.GunE11SR || ev.Item.ItemId == ItemType.GunProject90 || ev.Item.ItemId == ItemType.GunMP7 || ev.Item.ItemId == ItemType.GunLogicer || ev.Item.ItemId == ItemType.GunUSP)
                {
					ev.Item.Delete();
					Map.SpawnItem(ItemType.Medkit, 100000, ev.Player.transform.position);
				}
				if (ev.Item.ItemId == ItemType.MicroHID)
				{
					ev.Item.Delete();
					Map.SpawnItem(ItemType.MicroHID, 100000, ev.Player.transform.position);
				}
				if (ev.Item.ItemId == ItemType.GrenadeFrag || ev.Item.ItemId == ItemType.GrenadeFlash)
				{
					ev.Item.Delete();
					Map.SpawnItem(ItemType.Adrenaline, 100000, ev.Player.transform.position);
				}
				ev.Allow = false;
			}
		}
		public void OnTeamRespawn(PlayerSpawnEvent ev)
		{
			ev.Player.GetComponent<CharacterClassManager>().GodMode = true;
			Timing.CallDelayed(5f, () => ev.Player.GetComponent<CharacterClassManager>().GodMode = false);
		}
		public void OnWarheadCancel(WarheadCancelEvent ev)
		{
			try
			{
				if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
				{
					ev.Allow = false;
				}
				if (autowarheadstart)
				{
					ev.Allow = false;
				}
			}
			catch
			{ ev.Allow = false; }
		}
		public void RunOnRACommandSent(ref RACommandEvent RACom)
		{
			try
			{
				ReferenceHub sender = RACom.Sender.SenderId == "SERVER CONSOLE" || RACom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RACom.Sender.SenderId);
				string[] command = RACom.Command.Split(' ');
				string cmd = RACom.Command.ToLower();
				ReferenceHub player = Plugin.GetPlayer(command[1]);
				if (cmd.StartsWith("scp343"))
				{
					if (sender.CheckPermission("ra.scp343"))
					{
						RACom.Allow = false;
						if (player == null)
						{
							RACom.Sender.RAMessage(Configs.errorinra);
							return;
						}
						RACom.Sender.RAMessage(Configs.sucinra343);
						Spawn343(player);
					}
					else if (!sender.CheckPermission("ra.scp343"))
					{
						RACom.Allow = false;
						RACom.Sender.RAMessage("Отказано в доступе");
					}
				}
			}
			catch
			{ }
		}
	}
}