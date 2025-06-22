using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace scp714
{
	public static class Extensions
	{
		private static Inventory _hostInventory;
		public static Inventory HostInventory
		{
			get
			{
				if (_hostInventory == null)
					_hostInventory = ReferenceHub.GetHub(PlayerManager.localPlayer).inventory;

				return _hostInventory;
			}
		}
		public static Pickup SpawnItem(ItemType itemType, float durability, Vector3 position, Quaternion rotation = default, int sight = 0, int barrel = 0, int other = 0)
			=> HostInventory.SetPickup(itemType, durability, position, rotation, sight, barrel, other);
	}
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		private float dur = 5753;
		internal void RoundStart()
		{
			Extensions.SpawnItem(ItemType.Flashlight, dur, Map.GetRandomSpawnPoint(RoleType.Scp049));
		}
		internal void Pickup(PickingUpItemEventArgs ev)
		{
			if (ev.Pickup.durability == dur)
				ev.Player.Broadcast(plugin.config.PickupBcTime, plugin.config.PickupBc);
		}
		internal void Dying(DyingEventArgs ev)
		{
			if (ev.Target.Inventory.GetItemInHand().durability == dur)
			{
				if (ev.Killer.Role == RoleType.Scp049) ev.IsAllowed = false;
				if (ev.Killer.Role == RoleType.Scp93953 || ev.Killer.Role == RoleType.Scp93989)
					ev.Target.ReferenceHub.playerEffectsController.DisableEffect<Amnesia>();
			}
		}
		internal IEnumerator<float> aok()
		{
			for (; ; )
			{
				foreach (Player player in Player.List.Where(x => x.Inventory.GetItemInHand().durability == dur))
				{
					player.ReferenceHub.playerEffectsController.EnableEffect<SinkHole>(1);
				}
				yield return MEC.Timing.WaitForSeconds(1f);
			}
		}
	}
}