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
using scp035.API;
using scp228ruj.API;
namespace scp682343
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
			else if (ev.Killer.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				Timing.CallDelayed(0.5f, () => scp682.SetRank("SCP 682", "red"));
			}
			else if (ev.Killer.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				Timing.CallDelayed(0.5f, () => scp343.SetRank("SCP 343", "red"));
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
				ev.Amount = 1000f;
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
			if (Physics.Linecast(ev.Shooter.GetComponent<Scp049PlayerScript>().plyCam.transform.position, ev.TargetPos, out info))
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

		public void RunOnDoorOpen(ref DoorInteractionEvent doorInt)
		{
			bool UnbreakableDoorDetected = false;
			if (doorInt.Player.queryProcessor.PlayerId == scp682?.queryProcessor.PlayerId)
			{
				foreach (string doorName in unbreakableDoorNames)
					if (doorInt.Door.DoorName.Equals(doorName))
						UnbreakableDoorDetected = true;

				if (!UnbreakableDoorDetected)
					doorInt.Door.DestroyDoor(true);
				doorInt.Door.destroyed = true;
				doorInt.Door.Networkdestroyed = true;

				if (UnbreakableDoorDetected)
					doorInt.Allow = true;
				return;
			}
			else if (doorInt.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId)
			{
				if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime >= (double)Configs.initialCooldown)
				{
					doorInt.Allow = true;
				}
				else if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime < (double)Configs.initialCooldown)
				{
					if (!doorInt.Allow)
					{
						int i = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
						doorInt.Player.ClearBroadcasts();
						doorInt.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)i), 1U, false);
						Timing.CallDelayed(1U, () =>
						{
							int ia = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)ia), 1U, false);
						});
						Timing.CallDelayed(2U, () =>
						{
							int iq = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iq), 1U, false);
						});
						Timing.CallDelayed(3U, () =>
						{
							int iz = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iz), 1U, false);
						});
						Timing.CallDelayed(4U, () =>
						{
							int iw = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
							scp343.ClearBroadcasts();
							scp343.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iw), 1U, false);
						});
					}
				}
			}
			else if (doorInt.Allow == false)
			{
				if (Player.GetTeam(doorInt.Player) == Team.CDP)
				{
					if (max_door_tries != 0)
					{
						if (doorInt.Player.queryProcessor.PlayerId == scp343?.queryProcessor.PlayerId) return;

						if (UnityEngine.Random.Range(0, max_door_tries + 1) > door_tries)
						{
							foreach (string doorName in oneno)
								if (doorInt.Door.DoorName.Equals(doorName))
								{
									doorInt.Player.ClearBroadcasts();
									doorInt.Player.Broadcast("<color=red>SCP 181 не любит войну</color>", 10);
									return;
								}
							doorInt.Allow = true;
							doorInt.Player.ClearBroadcasts();
							doorInt.Player.Broadcast(Configs.dooropenbc, 10);
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
					List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty && x != scp343).ToList();
					ReferenceHub player = pList[rand.Next(pList.Count)];
					if (player == null)
					{
						ev.Player.ClearBroadcasts();
						ev.Player.Broadcast(5, $"Игроки не найдены", false);
						ev.Allow = false;
						return;
					}
					ev.Player.SetPosition(player.GetPosition());
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(5, $"Вы телепортированы к {player.GetNickname()}", false);
					ev.Allow = false;
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
			if (command[0] == "ban" || command[0] == "BAN")
			{
				if (command[2] == "0")
				{
					RACom.Allow = false;
					if (!sender.CheckPermission("at.kick"))
					{
						RACom.Sender.RAMessage("Permission denied.");
						return;
					}
					IEnumerable<string> reasons = command.Where(s => s != command[0] && s != command[1]);
					string reason = "";
					foreach (string st in reasons)
						reason += st;
					GameObject obj = Player.GetPlayer(command[1])?.gameObject;
					if (obj == null)
					{
						RACom.Sender.RAMessage("Юзер не найден", false, command[0]);
						return;
					}

					ServerConsole.Disconnect(obj, $"Вы были кикнуты с сервера {reason}");
					RACom.Sender.RAMessage("Игрок кикнут.", true, command[0]);
					return;
				}
			}
		}
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

