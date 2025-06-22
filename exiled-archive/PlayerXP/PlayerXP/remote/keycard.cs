using Exiled.Events.EventArgs;
using System.Linq;

namespace PlayerXP.remote
{
    public class keycard
	{
		private readonly Plugin plugin;
		public keycard(Plugin plugin) => this.plugin = plugin;
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (ev.Player.Team == Team.SCP) return;
			if (!ev.IsAllowed)
			{
				var playerIntentory = ev.Player.ReferenceHub.inventory.items;
				foreach (var item in playerIntentory)
				{
					var gameItem = UnityEngine.Object.FindObjectOfType<Inventory>().availableItems.FirstOrDefault(i => i.id == item.id);

					if (gameItem == null)
						continue;

					if (gameItem.permissions == null || gameItem.permissions.Length == 0)
						continue;

					foreach (var itemPerm in gameItem.permissions)
					{
						if (itemPerm == ev.Door.permissionLevel)
						{
							ev.IsAllowed = true;
							continue;
						}
					}
				}
			}
		}

		public void OnLockerInteraction(InteractingLockerEventArgs ev)
		{
			if (!ev.IsAllowed)
			{
				if (ev.Player.Team == Team.SCP) return;
				var playerIntentory = ev.Player.ReferenceHub.inventory.items;
				bool chcb = false;
				bool lvl2per = false;
				foreach (var item in playerIntentory)
				{
					var gameItem = UnityEngine.Object.FindObjectOfType<Inventory>().availableItems.FirstOrDefault(i => i.id == item.id);

					if (gameItem == null)
						continue;

					if (gameItem.permissions == null || gameItem.permissions.Length == 0)
						continue;

					foreach (var itemPerm in gameItem.permissions)
					{
						if (itemPerm == "PEDESTAL_ACC")
						{
							ev.IsAllowed = true;
							continue;
						}
						if (itemPerm == "CHCKPOINT_ACC")
						{
							chcb = true;
						}
						if (itemPerm == "CONT_LVL_2")
						{
							lvl2per = true;
						}
					}
					if (chcb && lvl2per)
					{
						ev.IsAllowed = true;
						continue;
					}
				}
			}
		}
		public void OnGenOpen(UnlockingGeneratorEventArgs ev)
		{
			if (ev.Player.Team == Team.SCP) return;
			var playerIntentory = ev.Player.ReferenceHub.inventory.items;
			foreach (var item in playerIntentory)
			{
				var gameItem = UnityEngine.Object.FindObjectOfType<Inventory>().availableItems.FirstOrDefault(i => i.id == item.id);

				if (gameItem == null)
					continue;

				if (gameItem.permissions == null || gameItem.permissions.Length == 0)
					continue;

				foreach (var itemPerm in gameItem.permissions)
				{
					if (itemPerm == "ARMORY_LVL_1")
					{
						ev.IsAllowed = true;
						continue;
					}
				}
			}
		}
	}
}
