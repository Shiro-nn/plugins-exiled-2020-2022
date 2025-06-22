using Hints;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace gate3
{
	public static class Extensions
	{
		public static void Hint(this Player player, string message, float time)
		{
			player.ReferenceHub.hints.Show(new TextHint(message.Trim(), new HintParameter[] { new StringHintParameter("") }, HintEffectPresets.FadeInAndOut(0.25f, time, 0f), 10f));
		}
	}
	[Serializable]
	public class Stats
	{
		public string UserId;
		public int shelter;
		public int elevator;
	}
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public DoorVariant d;
		public DoorVariant sdoor;
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
		public Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public bool detonated = false;

		internal void Waiting() => detonated = false;
		public void RoundStart()
		{
			detonated = false;
			Player player = Player.List.ToList()[1];
			foreach (Collider c in Physics.OverlapSphere(player.Position, 3000000f))
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
					foreach (DoorVariant door in UnityEngine.Object.FindObjectsOfType<DoorVariant>())
					{
						if (door.transform.position.x <= num && door.transform.position.x >= num4 && door.transform.position.y <= num2 && door.transform.position.y >= num5 && door.transform.position.z <= num3 && door.transform.position.z >= num6)
						{
							sdoor = door;
						}
					}
				}
			}
			checkshelterdoor2();
		}
		public void Detonated() => detonated = true;
		public void DoorOpen(InteractDoorEvent ev)
		{
			DoorVariant door = ev.Door;
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
		public IEnumerator<float> etc()
		{
			for (; ; )
			{
				if (!detonated && Round.IsStarted)
				{
					foreach (Player player in Player.List)
					{
						try
						{
							Vector3 to = new Vector3(-51, 989, -50);
							if (player.Position.x <= pnum && player.Position.x >= pnum4 &&
								player.Position.y <= pnum2 && player.Position.y >= pnum5 &&
								player.Position.z <= pnum3 && player.Position.z >= pnum6)
							{
								if (Stats[player.UserId].elevator == 3 || Stats[player.UserId].elevator == 2)
								{
									player.Hint($"\nПодождите {Stats[player.UserId].elevator} секунды", 1f);
									Stats[player.UserId].elevator--;
								}
								else if (Stats[player.UserId].elevator == 1)
								{
									player.Hint($"\nПодождите {Stats[player.UserId].elevator} секунду", 1f);
									Stats[player.UserId].elevator--;
								}
								else if (Stats[player.UserId].elevator == 0)
								{
									player.Position = to;
									Stats[player.UserId].elevator = 3;
								}
							}
							else
							{
								Stats[player.UserId].elevator = 3;
							}
						}
						catch { }
						try
						{
							Vector3 door1 = new Vector3(-58, 991, -51);
							Vector3 door2 = new Vector3(-55, 987, -48);
							Vector3 randomSP = new Vector3(sdoor.transform.position.x, sdoor.transform.position.y + 1f, sdoor.transform.position.z);
							if (player.Position.x <= door2.x && player.Position.x >= door1.x && player.Position.y <= door1.y && player.Position.y >= door2.y && player.Position.z <= door2.z && player.Position.z >= door1.z)
							{
								if (Stats[player.UserId].shelter == 3 || Stats[player.UserId].shelter == 2)
								{
									player.Hint($"\nПодождите {Stats[player.UserId].shelter} секунды", 1f);
									Stats[player.UserId].shelter--;
								}
								else if (Stats[player.UserId].shelter == 1)
								{
									player.Hint($"\nПодождите {Stats[player.UserId].shelter} секунду", 1f);
									Stats[player.UserId].shelter--;
								}
								else if (Stats[player.UserId].shelter == 0)
								{
									player.Position = randomSP;
									Stats[player.UserId].shelter = 3;
								}
							}
							else
							{
								Stats[player.UserId].shelter = 3;
							}
						}
						catch { }
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void checkshelterdoor2()
		{
			List<Player> playerssl = Player.List.ToList();
			foreach (Player player in playerssl)
			{
				foreach (Collider c in Physics.OverlapSphere(player.Position, 3000000f).Where(x => x.gameObject.name == "Nodoor"))
				{
					Vector3 shel = c.transform.position;
					float num = shel.x + 10f;
					float num2 = shel.y + 10f;
					float num3 = shel.z + 10f;
					float num4 = shel.x - 10f;
					float num5 = shel.y - 10f;
					float num6 = shel.z - 10f;
					foreach (DoorVariant door in UnityEngine.Object.FindObjectsOfType<DoorVariant>())
					{
						if (door.transform.position.x <= num && door.transform.position.x >= num4 &&
							door.transform.position.y <= num2 && door.transform.position.y >= num5 &&
							door.transform.position.z <= num3 && door.transform.position.z >= num6)
						{
							d = door;
							door.RequiredPermissions.RequiredPermissions = KeycardPermissions.ExitGates;
							door.GetComponent<DoorNametagExtension>().UpdateName("static");
							try { door.GetComponent<BreakableDoor>()._prevDestroyed = false; } catch { }
							try { door.GetComponent<BreakableDoor>()._maxHealth = float.MaxValue; } catch { }
							try { door.GetComponent<BreakableDoor>()._remainingHealth = float.MaxValue; } catch { }
							try { door.GetComponent<BreakableDoor>()._ignoredDamageSources = DoorDamageType.Scp096; } catch { }
						}
					}
				}
			}
		}
		public void PlayerJoin(JoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server")
				return;
			if (!Stats.ContainsKey(ev.Player.UserId))
				Stats.Add(ev.Player.UserId, new Stats()
				{
					UserId = ev.Player.UserId,
					shelter = 3,
					elevator = 3,
				});
		}
	}
}