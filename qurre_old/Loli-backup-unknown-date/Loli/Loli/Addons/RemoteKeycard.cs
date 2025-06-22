using Interactables.Interobjects.DoorUtils;
using Qurre.API.Events;
using Qurre.API.Objects;
namespace Loli.Addons
{
	public class RemoteKeycard
	{
		public void Door(InteractDoorEvent ev)
		{
			//ev.Player.Broadcast(ev.Door.Name + " - " + ev.Door.Type, 5);
			try
			{
				if (!ev.Allowed)
				{
					if (ev.Door?.Permissions?.RequiredPermissions == Interactables.Interobjects.DoorUtils.KeycardPermissions.None) return;
					if (ev.Player.Team == Team.SCP) return;
					var playerIntentory = ev.Player.AllItems;
					foreach (var item in playerIntentory)
					{
						if (item == null)
							continue;
						if(ev.Door.Permissions.CheckPermissions(item.Base, ev.Player.ReferenceHub)) ev.Allowed = true;
					}
				}
			}
			catch { }
		}

		public void Locker(InteractLockerEvent ev)
		{
			if (!ev.Allowed)
			{
				if (ev.Player.Team == Team.SCP) return;
				var playerIntentory = ev.Player.AllItems;
				bool b1 = false;
				bool b2 = false;
				foreach (var item in playerIntentory)
				{
					try
					{
						if (item == null)
							continue;
						InventorySystem.Items.Keycards.KeycardItem keycardItem = item.Base as InventorySystem.Items.Keycards.KeycardItem;
						if (ev.Player.Inventory.CurInstance != null && keycardItem != null)
						{
							if (keycardItem.Permissions.HasFlagFast(Interactables.Interobjects.DoorUtils.KeycardPermissions.ContainmentLevelTwo)) b1 = true;
							if (keycardItem.Permissions.HasFlagFast(Interactables.Interobjects.DoorUtils.KeycardPermissions.Checkpoints)) b2 = true;
						}
					}
					catch { }
				}
				if (b1 && b2) ev.Allowed = true;
			}
		}
		public void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Status != GeneratorStatus.Unlocked) return;
			ev.Allowed = false;
			if (ev.Player.Team == Team.SCP) return;
			var playerIntentory = ev.Player.AllItems;
			foreach (var item in playerIntentory)
			{
				if (item == null) continue;
				try
				{
					if(item?.Type == ItemType.KeycardContainmentEngineer)
                    {
						ev.Allowed = true;
						return;
                    }
					InventorySystem.Items.Keycards.KeycardItem keycardItem;
					if (ev.Player.Inventory.CurInstance != null && (keycardItem = item.Base as InventorySystem.Items.Keycards.KeycardItem) != null &&
						keycardItem.Permissions.HasFlagFast(Interactables.Interobjects.DoorUtils.KeycardPermissions.ArmoryLevelOne))
						ev.Allowed = true;
				}
                catch { }
			}
		}
	}
}