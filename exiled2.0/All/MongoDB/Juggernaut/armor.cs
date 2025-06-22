

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Waits;

namespace MongoDB.Juggernaut
{
	public class armor
	{
		private readonly Plugin plugin;
		public armor(Plugin plugin) => this.plugin = plugin;
		internal Pickup jugarmor;
		internal Player jaowner;
		internal float ahpc = 750;
		public void RoundStart()
		{
			ahpc = 750;
			jaowner = null;
			int random = Random.Range(0, 100);
			foreach (DoorVariant door in Map.Doors)
			{
				try
				{
					if (33 > random)
					{
						if (door.GetComponent<DoorNametagExtension>().GetName == "HCZ_ARMORY")
						{
							foreach (GameObject work in UnityEngine.Object.FindObjectsOfType<GameObject>())
							{
								if (work.gameObject.name == "Work Station")
								{
									if (Vector3.Distance(door.transform.position, work.transform.position) <= 10f)
									{
										float x = work.transform.position.x;
										float y = work.transform.position.y + 2f;
										float z = work.transform.position.z;
										jugarmor = Extensions.SpawnItem(ItemType.GunLogicer, 5000, new Vector3(x, y, z));
									}
								}
							}
						}
					}
					else if (66 > random)
					{
						if (door.GetComponent<DoorNametagExtension>().GetName == "049_ARMORY")
						{
							foreach (GameObject work in UnityEngine.Object.FindObjectsOfType<GameObject>())
							{
								if (work.gameObject.name == "Work Station")
								{
									if (Vector3.Distance(door.transform.position, work.transform.position) <= 10f)
									{
										float x = work.transform.position.x;
										float y = work.transform.position.y + 2f;
										float z = work.transform.position.z;
										jugarmor = Extensions.SpawnItem(ItemType.GunLogicer, 5000, new Vector3(x, y, z));
									}
								}
							}
						}
					}
					else
					{
						if (door.GetComponent<DoorNametagExtension>().GetName == "NUKE_ARMORY")
						{
							foreach (GameObject work in UnityEngine.Object.FindObjectsOfType<GameObject>())
							{
								if (work.gameObject.name == "Work Station")
								{
									if (Vector3.Distance(door.transform.position, work.transform.position) <= 10f)
									{
										float x = work.transform.position.x;
										float y = work.transform.position.y + 2f;
										float z = work.transform.position.z;
										jugarmor = Extensions.SpawnItem(ItemType.GunLogicer, 5000, new Vector3(x, y, z));
									}
								}
							}
						}
					}
				}
				catch { }
			}
		}
		public void RoundEnd(RoundEndedEventArgs ev)
		{
			jugarmor = null;
			jaowner = null;
		}
		internal void Pickup(PickingUpItemEventArgs ev)
		{
			try
			{
				if (ev.Pickup == jugarmor && ev.IsAllowed)
				{
					float dur = jugarmor.durability;
					ItemType iti = jugarmor.itemId;
					jugarmor.Delete();
					jaowner = ev.Player;
					jaowner.Broadcast(5, "<b><color=lime>Вы подобрали броню Джаггернаута</color></b>");
					jaowner.MaxAdrenalineHealth = 750;
					jaowner.AdrenalineHealth = ahpc;
					try
					{
						PlayerEffectsController componentInParent2 = jaowner.ReferenceHub.GetComponentInParent<PlayerEffectsController>();
						componentInParent2.DisableEffect<CustomPlayerEffects.Scp207>();
					}
					catch { }
					Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
					foreach (var item in jaowner.ReferenceHub.inventory.items) items.Add(item);
					Timing.CallDelayed(0.3f, () => jaowner.ReferenceHub.inventory.items.Clear());
					Timing.CallDelayed(0.5f, () =>
					{
						jaowner.ReferenceHub.inventory.AddNewItem(iti, dur);
						foreach (var item in items)
						{
							if (item.id == ItemType.GunCOM15 ||
								item.id == ItemType.MicroHID ||
								item.id == ItemType.GunE11SR ||
								item.id == ItemType.GunProject90 ||
								item.id == ItemType.GunMP7 ||
								item.id == ItemType.GunLogicer ||
								item.id == ItemType.GunUSP)
							{
								MongoDB.Extensions.SpawnItem(item.id, item.durability, jaowner.ReferenceHub.transform.position);
							}
							else
							{
								jaowner.ReferenceHub.inventory.AddNewItem(item.id);
							}
						}
					});
					jugarmor = null;
				}
				else if (ev.Pickup.durability == 100000)
				{
					ev.IsAllowed = false;
				}
				else if (ev.Player.ReferenceHub.queryProcessor.PlayerId == jaowner.ReferenceHub.queryProcessor.PlayerId)
				{
					if (ev.Pickup.itemId == ItemType.GunCOM15 ||
						ev.Pickup.itemId == ItemType.MicroHID ||
						ev.Pickup.itemId == ItemType.GunE11SR ||
						ev.Pickup.itemId == ItemType.GunProject90 ||
						ev.Pickup.itemId == ItemType.GunMP7 ||
						ev.Pickup.itemId == ItemType.GunLogicer ||
						ev.Pickup.itemId == ItemType.GunUSP)
					{
						ev.IsAllowed = false;
						ev.Pickup.Delete();
						plugin.armor.jugarmor = MongoDB.Extensions.SpawnItem(ev.Pickup.itemId,
							ev.Pickup.durability,
							ev.Pickup.transform.position,
							ev.Pickup.transform.rotation,
							ev.Pickup.weaponMods.Sight,
							ev.Pickup.weaponMods.Barrel,
							ev.Pickup.weaponMods.Other);
					}
				}
			}
			catch { }
		}
		internal void Drop(ItemDroppedEventArgs ev)
		{
			try
			{
				float dur = 0;
				if (ev.Player.ReferenceHub.queryProcessor.PlayerId == jaowner.ReferenceHub.queryProcessor.PlayerId /*&& ev.Pickup.itemId == jugarmor.itemId && ev.Pickup.durability == jugarmor.durability*/)
				{
					Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
					foreach (var item in jaowner.ReferenceHub.inventory.items)
						if (item.id == ItemType.GunLogicer)
							items.Add(item);
					foreach (var item in items)
					{
						if (item.durability > dur)
						{
							dur = item.durability;
						}
					}
					Timing.CallDelayed(0.5f, () =>
					{
						if (ev.Pickup.durability == dur)
						{
							jugarmor = ev.Pickup;
							ahpc = jaowner.AdrenalineHealth;
							jaowner.AdrenalineHealth = 0;
							jaowner.MaxAdrenalineHealth = 150;
							jaowner = null;
						}
					});
					Timing.CallDelayed(0.7f, () =>
					{
						if (jaowner != null)
						{
							jugarmor = ev.Pickup;
							ahpc = jaowner.AdrenalineHealth;
							jaowner.AdrenalineHealth = 0;
							jaowner.MaxAdrenalineHealth = 150;
							jaowner = null;
						}
					});
				}
			}
			catch
			{
				try
				{
					if (ev.Player.ReferenceHub.queryProcessor.PlayerId == jaowner.ReferenceHub.queryProcessor.PlayerId && ev.Pickup.itemId == jugarmor.itemId/* && ev.Pickup.durability == jugarmor.durability*/)
					{
						try { jugarmor = ev.Pickup; } catch { }
						try { ahpc = jaowner.AdrenalineHealth; } catch { }
						try { jaowner.AdrenalineHealth = 0; } catch { }
						try { jaowner.MaxAdrenalineHealth = 150; } catch { }
						try { jaowner = null; } catch { }
					}
				}
				catch { }
			}
		}
		public void medical(UsingMedicalItemEventArgs ev)
		{
			if (ev.Item == ItemType.SCP207)
				if (ev.Player.ReferenceHub.queryProcessor.PlayerId == jaowner?.ReferenceHub?.queryProcessor.PlayerId)
					ev.IsAllowed = false;
		}
		public void Died(DiedEventArgs ev)
		{
			try
			{
				if (ev.Target.ReferenceHub.queryProcessor.PlayerId == jaowner?.ReferenceHub?.queryProcessor.PlayerId)
				{
					List<Pickup> jughmm = UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.itemId == ItemType.GunLogicer && Vector3.Distance(ev.Target.ReferenceHub.transform.position, x.transform.position) <= 1.5f).ToList();
					float dur = 0;
					foreach (Pickup pick in jughmm)
					{
						if (pick.durability > dur)
						{
							dur = pick.durability;
							jugarmor = pick;
						}
					}
					ahpc = jaowner.AdrenalineHealth;
					jaowner = null;
				}
			}
			catch { }
		}
		public void CheckEscape(EscapingEventArgs ev)
		{
			try
			{
				if (ev.Player.Id == jaowner?.Id)
					spawn(ev.Player);
			}
			catch { }
		}


		internal void spawn(Player player)
		{
			try
			{
				if (player.ReferenceHub.queryProcessor.PlayerId == jaowner?.ReferenceHub?.queryProcessor.PlayerId)
				{
					ahpc = jaowner.AdrenalineHealth;
					jaowner = null;
					Timing.CallDelayed(0.5f, () =>
					{
						List<Pickup> jughmm = UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.itemId == ItemType.GunLogicer && Vector3.Distance(player.ReferenceHub.transform.position, x.transform.position) <= 1.5f).ToList();
						float dur = 0;
						foreach (Pickup pick in jughmm)
						{
							if (pick.durability > dur)
							{
								dur = pick.durability;
								jugarmor = pick;
							}
						}
						if (player.Role != RoleType.Spectator)
							jugarmor.transform.position = player.ReferenceHub.transform.position;
					});
				}
			}
			catch { }
		}
		internal IEnumerator<float> Offstamina()
		{
			for (; ; )
			{
				try { jaowner.Stamina.RemainingStamina = 0f; }
				catch { }
				yield return Timing.WaitForSeconds(0.3f);
			}
		}
	}
}