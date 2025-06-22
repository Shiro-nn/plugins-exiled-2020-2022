using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using Grenades;
using Mirror;
using EXILED.ApiObjects;
using System.Threading;
using RemoteAdmin;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace gate3
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public bool roundstart = false;
		public int roundtimeint = 0;
		public Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public bool tp = false;
		public float x;
		public float y;
		public float z;
		public Door d;
		public static float forceShoot = 100.0f;
		public static float rangeShoot = 7.0f;
		private readonly int grenade_pickup_mask = 1049088;
		public SyncListString list;
		protected readonly List<SyncObject> syncObjects = new List<SyncObject>();
		public void OnWaitingForPlayers()
		{
			this.list = new SyncListString();
			this.list.Add("fydne");
			this.InitSyncObject((SyncObject)this.list);
			this.Coroutines.Add(Timing.RunCoroutine(roundtime()));
			this.Coroutines.Add(Timing.RunCoroutine(checkdoor1()));
			this.Coroutines.Add(Timing.RunCoroutine(checkdoor2()));
			this.Coroutines.Add(Timing.RunCoroutine(checkshelter()));
			this.Coroutines.Add(Timing.RunCoroutine(checkelevator()));
			this.Coroutines.Add(Timing.RunCoroutine(checkitem()));
			this.Coroutines.Add(Timing.RunCoroutine(checkshelterdoor()));
		}

		protected void InitSyncObject(SyncObject syncObject)
		{
			this.syncObjects.Add(syncObject);
		}
		public void OnRoundStart()
		{
			tp = false;
			roundstart = true;
		}

		public void OnRoundEnd()
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
			roundstart = false;
		}

		public void OnRoundRestart()
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
			roundstart = false;
		}
		public IEnumerator<float> roundtime()
		{
			for (; ; )
			{
				this.list.Add("fydne");
				this.InitSyncObject((SyncObject)this.list);
				if (roundstart) roundtimeint++;
				if (!roundstart) roundtimeint = 0;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.characterClassManager.UserId) || ev.Player.characterClassManager.IsHost || ev.Player.nicknameSync.MyNick == "Dedicated Server")
				return;
			if (!Stats.ContainsKey(ev.Player.characterClassManager.UserId))
				Stats.Add(ev.Player.characterClassManager.UserId, Extensions.LoadStats(ev.Player.characterClassManager.UserId));

		}
		public void OnPlayerSpawn(PlayerSpawnEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.characterClassManager.UserId) || ev.Player.characterClassManager.IsHost || ev.Player.nicknameSync.MyNick == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.characterClassManager.UserId))
				Stats.Add(ev.Player.characterClassManager.UserId, Extensions.LoadStats(ev.Player.characterClassManager.UserId));
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
			if (Physics.Linecast(ev.Shooter.plyMovementSync.transform.position, ev.TargetPos, out info))
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
		public void RunOnDoorOpen(ref DoorInteractionEvent ev)
		{
			Door door = ev.Door;
			ReferenceHub player = ev.Player;
			foreach (Collider c in Physics.OverlapSphere(player.GetPosition(), 3000000f))
			{
				if (c.gameObject.name == "Nodoor")
				{
					Vector3 shel = c.transform.position;
					float num = shel.x + 10f;
					float num2 = shel.y + 10f;
					float num3 = shel.z + 10f;
					float num4 = shel.x - 10f;
					float num5 = shel.y - 10f;
					float num6 = shel.z - 10f;
					if (door.transform.position.x <= num && door.transform.position.x >= num4)
					{
						if (door.transform.position.y <= num2 && door.transform.position.y >= num5)
						{
							if (door.transform.position.z <= num3 && door.transform.position.z >= num6)
							{
								ev.Allow = false;
								var playerIntentory = ev.Player.inventory.items;
								foreach (var item in playerIntentory)
								{
									var gameItem = GameObject.FindObjectOfType<Inventory>().availableItems.FirstOrDefault(i => i.id == item.id);

									if (gameItem == null)
										continue;

									if (gameItem.permissions == null || gameItem.permissions.Length == 0)
										continue;

									foreach (var itemPerm in gameItem.permissions)
									{
										if (itemPerm == door.permissionLevel)
										{
											ev.Allow = true;
											continue;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == ev.Attacker.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Wall )
				{
					ev.Amount = 0f;
				}
			}
		}
		public IEnumerator<float> checkshelterdoor()
		{
			for (; ; )
			{
				List<ReferenceHub> playerssl = Plugin.GetHubs();
				foreach (ReferenceHub player in playerssl)
				{
					foreach (Collider c in Physics.OverlapSphere(player.GetPosition(), 3000000f))
					{
						if (c.gameObject.name == "Nodoor")
						{
							Vector3 shel = c.transform.position;
							float num = shel.x + 10f;
							float num2 = shel.y + 10f;
							float num3 = shel.z + 10f;
							float num4 = shel.x - 10f;
							float num5 = shel.y - 10f;
							float num6 = shel.z - 10f;
							foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
							{
								if (door.transform.position.x <= num && door.transform.position.x >= num4)
								{
									if (door.transform.position.y <= num2 && door.transform.position.y >= num5)
									{
										if (door.transform.position.z <= num3 && door.transform.position.z >= num6)
										{
											d = door;
											door.GrenadesResistant = true;
											door.permissionLevel = "EXIT_ACC";
										}
									}
								}
							}
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public IEnumerator<float> checkitem()
		{
			for (; ; )
			{
				foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
				{
					Vector3 door1 = new Vector3(-141, 995, -66.5f);
					Vector3 door2 = new Vector3(-76, 987, -72);
					if (item.transform.position.x <= door2.x && item.transform.position.x >= door1.x)
					{
						if (item.transform.position.z <= door1.z && item.transform.position.z >= door2.z)
						{
							item.transform.position = new Vector3(item.transform.position.x, 988.3f, item.transform.position.z);
						}
					}
				}
				yield return Timing.WaitForSeconds(10f);
			}
		}
		public IEnumerator<float> checkshelter()
		{
			for (; ; )
			{
				List<ReferenceHub> playerssl = Plugin.GetHubs();
				foreach (ReferenceHub player in playerssl)
				{
					foreach (Collider c in Physics.OverlapSphere(player.GetPosition(), 3000000f))
					{
						if (c.gameObject.name == "Nodoor")
						{
							Vector3 elevator = new Vector3(-81, 990, -68);
							Vector3 randomSP = c.transform.position;
							float num = randomSP.x + 4f;
							float num2 = randomSP.y + 4f;
							float num3 = randomSP.z + 4f;
							float num4 = randomSP.x - 4f;
							float num5 = randomSP.y - 4f;
							float num6 = randomSP.z - 4f;
							Vector3 pp = player.GetPosition();
							if (player.GetPosition().x <= num && player.GetPosition().x >= num4)
							{
								if (player.GetPosition().y <= num2 && player.GetPosition().y >= num5)
								{
									if (player.GetPosition().z <= num3 && player.GetPosition().z >= num6)
									{
										player.ClearBroadcasts();
										player.Broadcast("\nПодождите 3 секунды", 2);
										yield return Timing.WaitForSeconds(1f);
										if (player.GetPosition().x <= num && player.GetPosition().x >= num4)
										{
											if (player.GetPosition().y <= num2 && player.GetPosition().y >= num5)
											{
												if (player.GetPosition().z <= num3 && player.GetPosition().z >= num6)
												{
													player.ClearBroadcasts();
													player.Broadcast("\nПодождите 2 секунды", 2);
													yield return Timing.WaitForSeconds(1f);
													if (player.GetPosition().x <= num && player.GetPosition().x >= num4)
													{
														if (player.GetPosition().y <= num2 && player.GetPosition().y >= num5)
														{
															if (player.GetPosition().z <= num3 && player.GetPosition().z >= num6)
															{
																player.ClearBroadcasts();
																player.Broadcast("\nПодождите 1 секунду", 2);
																yield return Timing.WaitForSeconds(1f);
																if (player.GetPosition().x <= num && player.GetPosition().x >= num4)
																{
																	if (player.GetPosition().y <= num2 && player.GetPosition().y >= num5)
																	{
																		if (player.GetPosition().z <= num3 && player.GetPosition().z >= num6)
																		{
																			player.plyMovementSync.OverridePosition(elevator, 0f);
																			MapEditor.Editor.LoadMap(null, "gate3");
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				yield return Timing.WaitForSeconds(0.1f);
			}
		}

		public IEnumerator<float> checkelevator()
		{
			for (; ; )
			{
				List<ReferenceHub> playerssl = Plugin.GetHubs();
				foreach (ReferenceHub player in playerssl)
				{
					foreach (Collider c in Physics.OverlapSphere(player.GetPosition(), 3000000f))
					{
						if (c.gameObject.name == "Nodoor")
						{
							Vector3 shel = c.transform.position;
							float num = shel.x + 10f;
							float num2 = shel.y + 10f;
							float num3 = shel.z + 10f;
							float num4 = shel.x - 10f;
							float num5 = shel.y - 10f;
							float num6 = shel.z - 10f;
							Vector3 door1 = new Vector3(-84, 1000, -73);
							Vector3 door2 = new Vector3(-78, 980, -80);
							foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
							{
								if (door.transform.position.x <= num && door.transform.position.x >= num4)
								{
									if (door.transform.position.y <= num2 && door.transform.position.y >= num5)
									{
										if (door.transform.position.z <= num3 && door.transform.position.z >= num6)
										{
											if (player.GetPosition().x <= door2.x && player.GetPosition().x >= door1.x)
											{
												if (player.GetPosition().y <= door1.y && player.GetPosition().y >= door2.y)
												{
													if (player.GetPosition().z <= door1.z && player.GetPosition().z >= door2.z)
													{
														player.ClearBroadcasts();
														player.Broadcast("\nПодождите 3 секунды", 2);
														yield return Timing.WaitForSeconds(1f);
														if (player.GetPosition().x <= door2.x && player.GetPosition().x >= door1.x)
														{
															if (player.GetPosition().y <= door1.y && player.GetPosition().y >= door2.y)
															{
																if (player.GetPosition().z <= door1.z && player.GetPosition().z >= door2.z)
																{
																	player.ClearBroadcasts();
																	player.Broadcast("\nПодождите 2 секунды", 2);
																	yield return Timing.WaitForSeconds(1f);
																	if (player.GetPosition().x <= door2.x && player.GetPosition().x >= door1.x)
																	{
																		if (player.GetPosition().y <= door1.y && player.GetPosition().y >= door2.y)
																		{
																			if (player.GetPosition().z <= door1.z && player.GetPosition().z >= door2.z)
																			{
																				player.ClearBroadcasts();
																				player.Broadcast("\nПодождите 1 секунду", 2);
																				yield return Timing.WaitForSeconds(1f);
																				player.plyMovementSync.OverridePosition(new Vector3(door.transform.position.x, door.transform.position.y + 1f, door.transform.position.z), 0f);
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				yield return Timing.WaitForSeconds(0.1f);
			}
		}
		public IEnumerator<float> checkdoor1()
		{
			for (; ; )
			{
				Vector3 door1 = new Vector3(-58, 991, -51);
				Vector3 door2 = new Vector3(-55, 987, -48);
				Vector3 randomSP = new Vector3(-80, 984, -52);
				List<ReferenceHub> playerssl = Plugin.GetHubs();
				foreach (ReferenceHub player in playerssl)
				{
					if (player.GetPosition().x <= door2.x && player.GetPosition().x >= door1.x)
					{
						if (player.GetPosition().y <= door1.y && player.GetPosition().y >= door2.y)
						{
							if (player.GetPosition().z <= door2.z && player.GetPosition().z >= door1.z)
							{
								player.plyMovementSync.OverridePosition(randomSP, 0f);
								Stats[player.characterClassManager.UserId].shelter = 3;
							}
						}
					}
				}
				yield return Timing.WaitForSeconds(0.1f);
			}
		}
		public IEnumerator<float> checkdoor2()
		{
			for (; ; )
			{
				Vector3 door1 = new Vector3(-84, 982, -50);
				Vector3 door2 = new Vector3(-77, 990, -44);
				Vector3 randomSP = new Vector3(-51, 989, -50);
				List<ReferenceHub> playerssl = Plugin.GetHubs();
				foreach (ReferenceHub player in playerssl)
				{
					if (player.GetPosition().x <= door2.x && player.GetPosition().x >= door1.x)
					{
						if (player.GetPosition().y <= door2.y && player.GetPosition().y >= door1.y)
						{
							if (player.GetPosition().z <= door2.z && player.GetPosition().z >= door1.z)
							{
								player.plyMovementSync.OverridePosition(randomSP, 0f);
								Stats[player.characterClassManager.UserId].shelter = 3;
							}
						}
					}
				}
				yield return Timing.WaitForSeconds(0.1f);
			}
		}
	}
}

