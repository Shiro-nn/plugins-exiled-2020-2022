using Interactables.Interobjects.DoorUtils;
using Qurre.API.Events;
using Qurre.API.Objects;
namespace Loli.Addons
{
	static internal class RemoteKeycard
	{
		static internal void Door(InteractDoorEvent ev)
		{
			try
			{
				if (!ev.Allowed)
				{
					if (ev.Door?.Permissions?.RequiredPermissions == KeycardPermissions.None) return;
					if (ev.Player.Team == Team.SCP) return;
					foreach (var item in ev.Player.Inventory.UserInventory.Items)
					{
						if (item.Value == null) continue;
						if (ev.Door.Permissions.CheckPermissions(item.Value, ev.Player.ReferenceHub))
						{
							ev.Allowed = true;
							return;
						}
					}
				}
			}
			catch { }
		}

		static internal void Locker(InteractLockerEvent ev)
		{
			if (!ev.Allowed)
			{
				if (ev.Player.Team == Team.SCP) return;
				if (ev.Locker.Type == LockerType.Pedestal)
				{
					bool b1 = false;
					bool b2 = false;
					foreach (var item in ev.Player.Inventory.UserInventory.Items)
					{
						try
						{
							if (item.Value == null) continue;
							if (item.Value is InventorySystem.Items.Keycards.KeycardItem keycard)
							{
								if (keycard.Permissions.HasFlagFast(KeycardPermissions.ContainmentLevelTwo)) b1 = true;
								if (keycard.Permissions.HasFlagFast(KeycardPermissions.Checkpoints)) b2 = true;
							}
						}
						catch { }
					}
					if (b1 && b2) ev.Allowed = true;
				}
				else
				{
					foreach (var item in ev.Player.Inventory.UserInventory.Items)
					{
						try
						{
							if (item.Value == null) continue;
							if (item.Value is InventorySystem.Items.Keycards.KeycardItem keycard)
							{
								if (keycard.Permissions.HasFlagFast(ev.Chamber.Permissions))
								{
									ev.Allowed = true;
									return;
								}
							}
						}
						catch { }
					}
				}
			}
		}
		static internal void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Status != GeneratorStatus.Unlocked) return;
			ev.Allowed = false;
			if (ev.Player.Team == Team.SCP) return;
			foreach (var item in ev.Player.Inventory.UserInventory.Items)
			{
				if (item.Value == null) continue;
				try
				{
					if (item.Value.ItemTypeId == ItemType.KeycardContainmentEngineer)
					{
						ev.Allowed = true;
						return;
					}
					if (item.Value is InventorySystem.Items.Keycards.KeycardItem keycard &&
						keycard.Permissions.HasFlagFast(KeycardPermissions.ArmoryLevelOne))
					{
						ev.Allowed = true;
						return;
					}
				}
				catch { }
			}
		}
	}
}