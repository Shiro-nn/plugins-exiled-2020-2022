using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;
using Grenades;
using Mirror;
using Exiled.Events.EventArgs;
using Hints;
using static Door;
using Exiled.API.Features;
using Respawning;
using System;
using Random = UnityEngine.Random;
using MongoDB.scp228.API;
using MongoDB.scp343.API;
using Exiled.API.Enums;
using static Lift;
using MongoDB.events.hideandseek.API;
using MongoDB.logs;
using MongoDB.events;
using MongoDB.scp035.API;
using System.Net;
using MongoDB.events.hideandseek;
using CommsHack;
using System.IO;

namespace MongoDB.gate3
{
	public partial class EventHandlers
	{
		public gate3p plugin;
		public EventHandlers(gate3p plugin) => this.plugin = plugin;
		public List<CoroutineHandle> shc = new List<CoroutineHandle>();
		public bool roundstart = false;
		public int roundtimeint = 0;
		public Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public Door d;
		public Door sdoor;
		public float pnum;
		public float pnum2;
		public float pnum3;
		public float pnum4;
		public float pnum5;
		public float pnum6;
		public float snum;
		public float snum2;
		public float snum3;
		public float snum4;
		public float snum5;
		public float snum6;
		internal bool newspawn = false;
		internal int newspawnint = 0;
		public static List<int> shPlayers = new List<int>();
		internal int spawnshtime = 15;
		internal bool alphaon = false;
		internal bool alphadis = false;
		private string mainpath = Path.Combine(Paths.Plugins, "CustomEA");
		internal void elevator(InteractingElevatorEventArgs ev)
		{
			if (ev.Type == ElevatorType.GateA || ev.Type == ElevatorType.GateB)
			{
				if (ev.Status == Status.Up)
				{
					Timing.CallDelayed(5, () => loadgate3());
				}
			}
		}
		internal void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string effort = ev.Name;
			foreach (string s in ev.Arguments)
				effort += $" {s}";
			string[] args = effort.Split(' ');
			if (ev.Name == "server_event")
			{
				if (args[1] == "force_mtf_respawn" || args[1] == "FORCE_MTF_RESPAWN")
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = "SERVER_EVENT#Успешно";
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
					bool yes = false;
					Timing.CallDelayed(15f, () =>
					{
						if (!yes)
						{
							yes = true;
							RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
						}
					});
				}
				else if (args[1] == "force_ci_respawn" || args[1] == "FORCE_CI_RESPAWN")
				{
					newspawn = true;
					ev.IsAllowed = false;
					ev.ReplyMessage = "SERVER_EVENT#Успешно";
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
					bool yes = false;
					Timing.CallDelayed(15f, () =>
					{
						if (!yes)
						{
							yes = true;
							RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency);
						}
					});
					bool yes2 = false;
					Timing.CallDelayed(35f, () =>
					{
						if (!yes2)
						{
							yes2 = true;
							newspawn = false;
							RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
						}
					});
				}
			}
			if (ev.Name == "ci")
			{
				newspawn = true;
				ev.IsAllowed = false;
				ev.ReplyMessage = "SERVER_EVENT#Успешно";
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
				bool yes2 = false;
				Timing.CallDelayed(35f, () =>
				{
					if (!yes2)
					{
						yes2 = true;
						newspawn = false;
						RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
					}
				});
			}
			if (ev.Name == "mtf")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "SERVER_EVENT#Успешно";
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
			}
			if (ev.Name == "sh")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "SERVER_EVENT#Успешно";
				spawnsh();
			}
		}
		public void OnWaitingForPlayers()
		{
			alphadis = false;
			alphaon = false;
			spawnshtime = 15;
			newspawn = false;
			GameCore.Console.singleton.TypeCommand($"/mapeditor load gate3static");
			//editor.Editor.LoadMap(null, "gate3static");
		}
		public void OnRoundStart()
		{
			alphadis = false;
			alphaon = false;
			shPlayers.Clear();

			newspawnint = 0;
			newspawn = false;
			checkshelterdoor2();
			roundstart = true;
			List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
			foreach (ReferenceHub player in playerssl)
			{
				foreach (Collider c in Physics.OverlapSphere(player.transform.position, 3000000f))
				{
					if (c.gameObject.name == "Nodoor")
					{
						Vector3 randomSP = c.transform.position;
						pnum = randomSP.x + 4f;
						pnum2 = randomSP.y + 4f;
						pnum3 = randomSP.z + 4f;
						pnum4 = randomSP.x - 4f;
						pnum5 = randomSP.y - 4f;
						pnum6 = randomSP.z - 4f;
						Vector3 shel = c.transform.position;
						snum = shel.x + 10f;
						snum2 = shel.y + 10f;
						snum3 = shel.z + 10f;
						snum4 = shel.x - 10f;
						snum5 = shel.y - 10f;
						snum6 = shel.z - 10f;
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
							if (door.transform.position.x <= num && door.transform.position.x >= num4 && door.transform.position.y <= num2 && door.transform.position.y >= num5 && door.transform.position.z <= num3 && door.transform.position.z >= num6)
							{
								sdoor = door;
							}
						}
					}
				}
			}
			checkshelterdoor2();
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			shPlayers.Clear();
			roundstart = false;
		}

		public void OnRoundRestart()
		{
			roundstart = false;
		}
		public void roundtime()
		{
			if (roundstart)
			{
				roundtimeint++;
				if (newspawnint == 135)
				{
					newspawnint = 0;
				}
				else
				{
					newspawnint++;
				}
				if (Player.List.Count() == 0)
				{
					PlayerManager.localPlayer.GetComponent<PlayerStats>()?.Roundrestart();
				}
			}
			else if (!roundstart)
			{
				roundtimeint = 0;
				newspawnint = 0;
			}
		}
		public void spawn()
		{
			if (roundstart && !plugin.plugin.mtfvsci.GamemodeEnabled && !hasData.Gethason())
			{
				int random = Random.Range(0, 100);
				if (40 >= random)
				{
					spawnci();
				}
				else if (80 >= random)
				{
					spawnmtf();
				}
				else
				{
					spawnsh();
				}
			}
		}
		public Vector3 getrandomspawnsh
		{
			get
			{
				return Spawnpoint(Random.Range(0, 10));
			}
		}
		private Vector3 Spawnpoint(int id)
		{
			if (id == 1)
			{
				return new Vector3(-180, 989, -57);
			}
			else if (id == 2)
			{
				return new Vector3(-178, 989, -57);
			}
			else if (id == 3)
			{
				return new Vector3(-176, 989, -57);
			}
			else if (id == 4)
			{
				return new Vector3(-180, 989, -55);
			}
			else if (id == 5)
			{
				return new Vector3(-178, 989, -55);
			}
			else if (id == 6)
			{
				return new Vector3(-176, 989, -55);
			}
			else if (id == 7)
			{
				return new Vector3(-180, 989, -59);
			}
			else if (id == 8)
			{
				return new Vector3(-178, 989, -59);
			}
			else if (id == 9)
			{
				return new Vector3(-176, 989, -59);
			}
			return new Vector3(-175, 989, -57);
		}
		internal IEnumerator<float> shtimee()
		{
			int spawnshtime = 15;
			for (; ; )
			{
				if (Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList().Count != 0)
				{
					Map.ClearBroadcasts();
					Map.Broadcast(2, $"<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Замечен отряд <color=#15ff00>Длань Змеи</color></color>\n<color=#0089c7>Они будут на территории Gate3 через {spawnshtime} секунд</color></size>");
				}
				if (spawnshtime == 0)
				{
					Timing.KillCoroutines(shc);
					shc.Clear();
					spawnshtime = 15;
					if (Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList().Count != 0)
					{
						FileStream reader = File.OpenRead(Path.Combine(mainpath, "chaosentry.raw"));
						AudioAPI.API.PlayStream(reader, 0.1f);
						Cassie.Message("SERPENTS HAND HASENTERED");
						Map.ClearBroadcasts();
						Map.Broadcast(10, $"<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Отряд <color=#15ff00>Длань Змеи</color></color> <color=#0089c7>замечен на Gate3</color></size>");
					}
					try { send.sendmsg($"<:help:671334152565424129> Приехал отряд длань змеи в кол-ве {Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList().Count} человек."); } catch { }
					List<Player> newsh = Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList();
					foreach (Player sh in newsh)
					{
						spawnonesh(sh);
					}
					//loadgate3();
				}
				spawnshtime--;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void spawnsh()
		{
			shc.Add(Timing.RunCoroutine(shtimee()));
		}
		public void spawnonesh(Player sh)
		{
			shPlayers.Add(sh.ReferenceHub.queryProcessor.PlayerId);
			sh.ReferenceHub.characterClassManager.SetClassID(RoleType.Tutorial);
			sh.ClearBroadcasts();
			//sh.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(sh.ReferenceHub.scp079PlayerScript.connectionToClient, "<size=30%><color=red>Вы</color>-<color=#15ff00>Длань змеи</color>\n<color=#00ffdc>Ваша задача убить всех, кроме <color=red>SCP</color></color>\n<color=#fdffbb>На <color=red>[<color=#00ffff>Q</color>]</color> разговаривать с <color=red>scp</color><color=#ff0>,</color> на <color=red>[<color=#00ffff>V</color>]</color> с людьми</color></size>", 10, 0);
			sh.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(sh.ReferenceHub.scp079PlayerScript.connectionToClient, "<size=30%><color=red>Вы</color>-<color=#15ff00>Длань змеи</color>\n<color=#00ffdc>Ваша задача убить всех, кроме <color=red>SCP</color></color></size>", 10, 0);
			sh.ReferenceHub.inventory.items.ToList().Clear();
			sh.ReferenceHub.inventory.AddNewItem(ItemType.KeycardChaosInsurgency);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.GunE11SR);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Radio);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Medkit);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Adrenaline);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.GrenadeFlash);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.GrenadeFrag);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Flashlight);
			sh.ReferenceHub.playerStats.Health = 150;
			bool spawnone = false;
			Timing.CallDelayed(0.3f, () =>
			{
				if (!spawnone)
				{
					spawnone = true;
					sh.ReferenceHub.playerMovementSync.OverridePosition(getrandomspawnsh, 0f);
				}
			});
		}
		public void spawnci()
		{
			newspawn = true;
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			bool yes = false;
			Timing.CallDelayed(15f, () => { if (!yes) { yes = true; RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency); } });
			bool yes2 = false;
			Timing.CallDelayed(35f, () => { if (!yes2) { yes2 = true; newspawn = false; } });
			bool yes3 = false;
			Timing.CallDelayed(170f, () => { if (!yes3) { yes3 = true; } });
		}
		public void spawnmtf()
		{
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
			bool yes = false;
			Timing.CallDelayed(15f, () => { if (!yes) { yes = true; RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox); } });
		}
		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
				Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Extensions.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));

			if (shPlayers.Contains(ev.Player.Id))
			{
				shPlayers.Remove(ev.Player.Id);
			}
		}
		public void OnPlayerSpawn(SpawningEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
				Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Extensions.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));
		}
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			Door door = ev.Door;
			ReferenceHub player = ev.Player.ReferenceHub;
			if (door.transform.position.x <= snum && door.transform.position.x >= snum4 && door.transform.position.y <= snum2 && door.transform.position.y >= snum5 && door.transform.position.z <= snum3 && door.transform.position.z >= snum6)
			{
				ev.IsAllowed = false;
				var playerIntentory = ev.Player.ReferenceHub.inventory.items;
				foreach (var item in playerIntentory)
				{
					var gameItem = GameObject.FindObjectOfType<Inventory>().availableItems.FirstOrDefault(i => i.id == item.id);

					if (gameItem == null)
						continue;

					if (gameItem.permissions == null || gameItem.permissions.Length == 0)
						continue;

					foreach (var itemPerm in gameItem.permissions)
					{
						if (itemPerm == "EXIT_ACC")
						{
							ev.IsAllowed = true;
							continue;
						}
					}
				}
			}
		}
		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ev.DamageType == DamageTypes.Wall)
			{
				Vector3 door1 = new Vector3(-105f, 980f, -67f);
				Vector3 door2 = new Vector3(18, 995f, -51f);
				if (ev.Target.ReferenceHub.transform.position.x <= door2.x && ev.Target.ReferenceHub.transform.position.x >= door1.x && ev.Target.ReferenceHub.transform.position.y <= door2.y && ev.Target.ReferenceHub.transform.position.y >= door1.y && ev.Target.ReferenceHub.transform.position.z <= door2.z && ev.Target.ReferenceHub.transform.position.z >= door1.z)
				{
					ev.Amount = 0f;
				}
			}
		}
		public void checkshelterdoor2()
		{
			List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
			foreach (ReferenceHub player in playerssl)
			{
				foreach (Collider c in Physics.OverlapSphere(player.transform.position, 3000000f))
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
										door.doorType = DoorTypes.HeavyGate;
										door.buttonType = ButtonTypes.Checkpoint;
									}
								}
							}
						}
					}
				}
			}
		}
		public void checkall()
		{
			if (!plugin.plugin.mtfvsci.GamemodeEnabled && !hasData.Gethason())
			{
				List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
				foreach (ReferenceHub player in playerssl)
				{
					try
					{
						Vector3 elevator = new Vector3(-81, 992, -68);
						if (player.transform.position.x <= pnum && player.transform.position.x >= pnum4 &&
							player.transform.position.y <= pnum2 && player.transform.position.y >= pnum5 &&
							player.transform.position.z <= pnum3 && player.transform.position.z >= pnum6)
						{
							if (Stats[player.characterClassManager.UserId].elevator == 3 || Stats[player.characterClassManager.UserId].elevator == 2)
							{
								player.Hint($"\nПодождите {Stats[player.characterClassManager.UserId].elevator} секунды", 1f);
								Stats[player.characterClassManager.UserId].elevator--;
							}
							else if (Stats[player.characterClassManager.UserId].elevator == 1)
							{
								player.Hint($"\nПодождите {Stats[player.characterClassManager.UserId].elevator} секунду", 1f);
								Stats[player.characterClassManager.UserId].elevator--;
							}
							else if (Stats[player.characterClassManager.UserId].elevator == 0)
							{
								player.playerMovementSync.OverridePosition(elevator, 0f);
								loadgate3();
								Stats[player.characterClassManager.UserId].elevator = 3;
							}
						}
						else
						{
							Stats[player.characterClassManager.UserId].elevator = 3;
						}
					}
					catch { }
					if (100 >= Vector3.Distance(player.transform.position, new Vector3(0, 1000, 0)))
					{
						try
						{
							Vector3 door1 = new Vector3(-84, 1000, -73);
							Vector3 door2 = new Vector3(-78, 980, -80);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x &&
								player.transform.position.y <= door1.y && player.transform.position.y >= door2.y &&
								player.transform.position.z <= door1.z && player.transform.position.z >= door2.z)
							{
								if (Stats[player.characterClassManager.UserId].shelter == 3 || Stats[player.characterClassManager.UserId].shelter == 2)
								{
									player.Hint($"\nПодождите {Stats[player.characterClassManager.UserId].shelter} секунды", 1f);
									Stats[player.characterClassManager.UserId].shelter--;
								}
								else if (Stats[player.characterClassManager.UserId].shelter == 1)
								{
									player.Hint($"\nПодождите {Stats[player.characterClassManager.UserId].shelter} секунду", 1f);
									Stats[player.characterClassManager.UserId].shelter--;
								}
								else if (Stats[player.characterClassManager.UserId].shelter == 0)
								{
									player.playerMovementSync.OverridePosition(new Vector3(sdoor.transform.position.x, sdoor.transform.position.y + 1f, sdoor.transform.position.z), 0f);
									Stats[player.characterClassManager.UserId].shelter = 3;
								}
							}
							else
							{
								Stats[player.characterClassManager.UserId].shelter = 3;
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(167, 990, 28);
							Vector3 door2 = new Vector3(173, 994, 31);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x, player.transform.position.y - 2f, player.transform.position.z), 0f);
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(-4.7f, 1002, -17.7004f);
							Vector3 door2 = new Vector3(-3.9f, 1004, -4.027884f);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x - 2f, player.transform.position.y, player.transform.position.z), 0f);
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(4.55f, 1002, -4f);
							Vector3 door2 = new Vector3(16f, 1004, -3.33f);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 2f), 0f);
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(-16f, 1002, -18.55f);
							Vector3 door2 = new Vector3(-4.55f, 1004, -17.5f);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 2f), 0f);
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(4.55f, 1002, -18.55f);
							Vector3 door2 = new Vector3(16f, 1004, -17.5f);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 2f), 0f);
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(3.9f, 1002, -17.7004f);
							Vector3 door2 = new Vector3(4.7f, 1004, -4.027884f);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x + 2f, player.transform.position.y, player.transform.position.z), 0f);
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(4.55f, 1002, -4f);
							Vector3 door2 = new Vector3(16f, 1004, -3.33f);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 2f), 0f);
							}
						}
						catch { }
						try
						{
							if (newspawn)
							{
								try
								{
									Vector3 door1 = new Vector3(-59, 980, -65);
									Vector3 door2 = new Vector3(-57, 991, -51);
									if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
									{
										Vector3 randomSP = new Vector3(-63f, player.transform.position.y, player.transform.position.z);
										player.playerMovementSync.OverridePosition(randomSP, 0f);
									}
								}
								catch { }
								try
								{
									Vector3 door1 = new Vector3(-62, 980, -65);
									Vector3 door2 = new Vector3(-59, 991, -51);
									if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
									{
										Vector3 randomSP = new Vector3(-55f, player.transform.position.y, player.transform.position.z);
										player.playerMovementSync.OverridePosition(randomSP, 0f);
									}
								}
								catch { }
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(-58, 991, -51);
							Vector3 door2 = new Vector3(-55, 987, -48);
							Vector3 randomSP = new Vector3(-80, 986, -52);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door1.y && player.transform.position.y >= door2.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(randomSP, 0f);
								Stats[player.characterClassManager.UserId].shelter = 3;
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(-84, 982, -50);
							Vector3 door2 = new Vector3(-77, 990, -44);
							Vector3 randomSP = new Vector3(-51, 989, -50);
							if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
							{
								player.playerMovementSync.OverridePosition(randomSP, 0f);
								Stats[player.characterClassManager.UserId].shelter = 3;
							}
						}
						catch { }
					}
				}
			}
		}
		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId))
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPocketDimensionEscaping(EscapingPocketDimensionEventArgs ev)
		{
			if (shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId))
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId))
			{
				ev.IsAllowed = false;
			}
		}
		private void TeleportTo106(Player player)
		{
			Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
			{
				player.ReferenceHub.playerMovementSync.OverridePosition(scp106.ReferenceHub.transform.position, 0f);
			}
			else
			{
				foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				{
					if (door.DoorName == "106_PRIMARY")
					{
						player.ReferenceHub.playerMovementSync.OverridePosition(new Vector3(door.transform.position.x, door.transform.position.y + 1, door.transform.position.z), 0f);
					}

				}
			}
		}
		internal ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}
		internal void hurt(HurtingEventArgs ev)
		{
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == 0) return;

			ReferenceHub scp035 = null;

			try
			{
				scp035 = TryGet035();
			}
			catch
			{ }
			ReferenceHub target = ev.Target.ReferenceHub;
			ReferenceHub attacker = ev.Attacker.ReferenceHub;
			if (((shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(target.queryProcessor.PlayerId) && (ev.Attacker.Team == Team.SCP || ev.DamageType == DamageTypes.Pocket)) ||
				(shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(attacker.queryProcessor.PlayerId) && (ev.Target.Team == Team.SCP || (scp035 != null && attacker.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId))) ||
				(shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(target.queryProcessor.PlayerId) && shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).Contains(attacker.queryProcessor.PlayerId) &&
				target.queryProcessor.PlayerId != attacker.queryProcessor.PlayerId)))
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
		}
		internal void died(DiedEventArgs ev)
		{
			if (shPlayers.Contains(ev.Target.ReferenceHub.queryProcessor.PlayerId))
			{
				shPlayers.Remove(ev.Target.ReferenceHub.queryProcessor.PlayerId);
			}
		}
		public void setrole(ChangingRoleEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
			{
				if (ev.NewRole != RoleType.Tutorial)
				{
					if (shPlayers.Contains(ev.Player.Id))
					{
						shPlayers.Remove(ev.Player.Id);
					}
				}
			}
		}
		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			ReferenceHub scp035 = TryGet035();


			bool MTFAlive = Extensions.CountRoles(Team.MTF) > 0;
			bool CiAlive = Extensions.CountRoles(Team.CHI) > 0;
			bool ScpAlive = Extensions.CountRoles(Team.SCP) + (scp035 != null && scp035.GetRole() != RoleType.Spectator ? 1 : 0) > 0;
			bool DClassAlive = Extensions.CountRoles(Team.CDP) > 0;
			bool ScientistsAlive = Extensions.CountRoles(Team.RSC) > 0;
			bool SHAlive = shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).ToList().Count > 0;
			if (!plugin.plugin.mtfvsci.GamemodeEnabled && !hasData.Gethason())
			{
				ev.IsAllowed = false;
				if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive/* && !CiAlive*/)
				{
					ev.LeadingTeam = LeadingTeam.Anomalies;
					ev.IsAllowed = true;
					ev.IsRoundEnded = true;
				}
				else if (!SHAlive && !ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
				{
					ev.LeadingTeam = LeadingTeam.FacilityForces;
					ev.IsAllowed = true;
					ev.IsRoundEnded = true;
				}
				else if (!SHAlive && !ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
				{
					ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
					ev.IsAllowed = true;
					ev.IsRoundEnded = true;
				}
			}
			if (plugin.plugin.mtfvsci.GamemodeEnabled) return;
			else if (Massacre.GamemodEnabled) return;
			else if (plugin.plugin.EventHandlersP.ps) return;
			else if (!ScientistsAlive)
			{
				if (Round.ElapsedTime.Minutes > 13)
				{
					if (alphadis && !alphaon)
					{
						alphaon = true;
						Map.ClearBroadcasts();
						Map.Broadcast(10, "<size=25%><color=#6f6f6f>Совет О5 согласился на <color=#0089c7>взрыв</color> <color=red>Альфа Боеголовки</color></color></size>");
						plugin.plugin.EventHandlers343.autowarheadstart = true;
						Warhead.Start();
					}
					else if (!alphaon)
					{
						alphaon = true;
						Map.ClearBroadcasts();
						Map.Broadcast(10, "<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер/сбежал</color>\n<color=#6f6f6f>Совет О5 дал приказ запуска <color=red>Альфа Боеголовки</color></color></size>");
						plugin.plugin.EventHandlers343.autowarheadstart = true;
						Warhead.Start();
					}
				}
				else
				{
					if (Round.ElapsedTime.Minutes > 1)
						if (!alphadis)
						{
							alphadis = true;
							Map.Broadcast(10, "<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер/сбежал</color>\n<color=#6f6f6f>Совет О5 <color=#0089c7>обсуждает</color> запуск <color=red>Альфа Боеголовки</color></color></size>");
						}
				}
			}
		}
		public void loadgate3()
		{
			if (plugin.plugin.mtfvsci.GamemodeEnabled) return;
			else if (Massacre.GamemodEnabled) return;
			//else editor.Editor.LoadMap(null, "gate3");
			else GameCore.Console.singleton.TypeCommand($"/mapeditor load gate3");
		}
	}
}