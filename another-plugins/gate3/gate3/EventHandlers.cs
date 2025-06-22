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

namespace gate3
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public static List<CoroutineHandle> shc = new List<CoroutineHandle>();
		public static bool roundstart = false;
		public static int roundtimeint = 0;
		public static Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public static bool tp = false;
		public static Door d;
		public static Door sdoor;
		public static float pnum;
		public static float pnum2;
		public static float pnum3;
		public static float pnum4;
		public static float pnum5;
		public static float pnum6;
		public static float snum;
		public static float snum2;
		public static float snum3;
		public static float snum4;
		public static float snum5;
		public static float snum6;
		internal static bool newspawn = false;
		internal static int newspawnint = 0;
		public static List<int> shPlayers = new List<int>();
		internal static int spawnshtime = 15;
		public static void OnWaitingForPlayers()
		{
			spawnshtime = 15;
			newspawn = false;
			Coroutines.Add(Timing.RunCoroutine(roundtime()));
			Coroutines.Add(Timing.RunCoroutine(checkdoor1()));
			Coroutines.Add(Timing.RunCoroutine(checkdoor2()));
			Coroutines.Add(Timing.RunCoroutine(checkshelter()));
			Coroutines.Add(Timing.RunCoroutine(checkelevator()));
			MapEditor.Editor.LoadMap(null, "gate3static");
		}
		public static void OnRoundStart()
		{
			shPlayers.Clear();
			foreach (GameObject work in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				if (work.gameObject.name == "Work Station")
				{
					work.GetComponent<WorkStation>().NetworkisTabletConnected = true;
					work.GetComponent<WorkStation>().Network_playerConnected = work;
				}
			}
			newspawn = false;
			checkshelterdoor2();
			tp = false;
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
		}

		public static void OnRoundEnd(RoundEndedEventArgs ev)
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
			roundstart = false;
		}

		public static void OnRoundRestart()
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
			roundstart = false;
		}
		public static IEnumerator<float> roundtime()
		{
			for (; ; )
			{
				if (roundstart)
				{
					roundtimeint++;
				}
				else if (!roundstart)
				{
					roundtimeint = 0;
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public static void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
				Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Extensions.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));

		}
		public static void OnPlayerSpawn(SpawningEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
				Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Extensions.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));
		}
		public static void RunOnDoorOpen(InteractingDoorEventArgs ev)
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
		public static void OnPlayerHurt(HurtingEventArgs ev)
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
		public static void checkshelterdoor2()
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
							if (door.transform.position.x <= num && door.transform.position.x >= num4 && door.transform.position.y <= num2 && door.transform.position.y >= num5 && door.transform.position.z <= num3 && door.transform.position.z >= num6)
							{
								d = door;
								door.GrenadesResistant = true;
								door.permissionLevel = "EXIT_ACC";
								door.doorType = DoorTypes.Checkpoint;
								door.buttonType = ButtonTypes.Checkpoint;
							}
						}
					}
				}
			}
		}
		public static IEnumerator<float> checkshelter()
		{
			for (; ; )
			{
				List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
				foreach (ReferenceHub player in playerssl)
				{
					Vector3 elevator = new Vector3(-81, 990, -68);
					if (player.transform.position.x <= pnum && player.transform.position.x >= pnum4 && player.transform.position.y <= pnum2 && player.transform.position.y >= pnum5 && player.transform.position.z <= pnum3 && player.transform.position.z >= pnum6)
					{
						if (Stats[player.characterClassManager.UserId].elevator == 3 || Stats[player.characterClassManager.UserId].elevator == 2)
						{
							player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].elevator} секунды", new HintParameter[]
							{
								new StringHintParameter("")
							}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
							Stats[player.characterClassManager.UserId].elevator--;
						}
						else if (Stats[player.characterClassManager.UserId].elevator == 1)
						{
							player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].elevator} секунду", new HintParameter[]
							{
											new StringHintParameter("")
							}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
							Stats[player.characterClassManager.UserId].elevator--;
						}
						else if (Stats[player.characterClassManager.UserId].elevator == 0)
						{
							player.playerMovementSync.OverridePosition(elevator, 0f);
							MapEditor.Editor.LoadMap(null, "gate3");
							Stats[player.characterClassManager.UserId].elevator = 3;
						}
					}
					else
					{
						Stats[player.characterClassManager.UserId].elevator = 3;
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}

		public static IEnumerator<float> checkelevator()
		{
			for (; ; )
			{
				if (!Warhead.IsDetonated)
				{
					List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
					foreach (ReferenceHub player in playerssl)
					{
						Vector3 door1 = new Vector3(-84, 1000, -73);
						Vector3 door2 = new Vector3(-78, 980, -80);
						if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door1.y && player.transform.position.y >= door2.y && player.transform.position.z <= door1.z && player.transform.position.z >= door2.z)
						{
							if (Stats[player.characterClassManager.UserId].shelter == 3 || Stats[player.characterClassManager.UserId].shelter == 2)
							{
								player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].shelter} секунды", new HintParameter[]
								{
											new StringHintParameter("")
								}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
								Stats[player.characterClassManager.UserId].shelter--;
							}
							else if (Stats[player.characterClassManager.UserId].shelter == 1)
							{
								player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].shelter} секунду", new HintParameter[]
								{
											new StringHintParameter("")
								}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
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
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public static IEnumerator<float> checkdoor1()
		{
			for (; ; )
			{
				Vector3 door1 = new Vector3(-58, 991, -51);
				Vector3 door2 = new Vector3(-55, 987, -48);
				Vector3 randomSP = new Vector3(-80, 984, -52);
				List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
				foreach (ReferenceHub player in playerssl)
				{
					if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door1.y && player.transform.position.y >= door2.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
					{
						player.playerMovementSync.OverridePosition(randomSP, 0f);
						MapEditor.Editor.LoadMap(null, "gate3");
						Stats[player.characterClassManager.UserId].shelter = 3;
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public static IEnumerator<float> checkdoor2()
		{
			for (; ; )
			{
				Vector3 door1 = new Vector3(-84, 982, -50);
				Vector3 door2 = new Vector3(-77, 990, -44);
				Vector3 randomSP = new Vector3(-51, 989, -50);
				List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
				foreach (ReferenceHub player in playerssl)
				{
					if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
					{
						player.playerMovementSync.OverridePosition(randomSP, 0f);
						MapEditor.Editor.LoadMap(null, "gate3");
						Stats[player.characterClassManager.UserId].shelter = 3;
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
}

