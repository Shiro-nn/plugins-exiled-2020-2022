using Interactables.Interobjects.DoorUtils;
using Qurre.API.Events;
namespace remote_keycard
{
	public class Plugin : Qurre.Plugin
	{
		#region override
		public override System.Version Version => new System.Version(1, 0, 8);
		public override System.Version NeededQurreVersion => new System.Version(1, 10, 0);
		public override string Developer => "fydne";
		public override string Name => "remote keycard";
		public override void Enable() => RegisterEvents();
		public override void Disable() => UnregisterEvents();
		#endregion
		#region events
		private void RegisterEvents()
		{
			Qurre.Events.Player.InteractDoor += Door;
			Qurre.Events.Player.InteractLocker += Locker;
			Qurre.Events.Player.InteractGenerator += Generator;
		}
		private void UnregisterEvents()
		{
			Qurre.Events.Player.InteractDoor -= Door;
			Qurre.Events.Player.InteractLocker -= Locker;
			Qurre.Events.Player.InteractGenerator -= Generator;
		}
		#endregion
		#region main
		public void Door(InteractDoorEvent ev)
		{
			try
			{
				if (!ev.Allowed)
				{
					if (ev.Door?.Permissions?.RequiredPermissions == KeycardPermissions.None) return;
					if (ev.Player.Team == Team.SCP) return;
					var playerIntentory = ev.Player.AllItems;
					foreach (var item in playerIntentory)
					{
						if (item == null)
							continue;
						if (ev.Door.Permissions.CheckPermissions(item.Base, ev.Player.ReferenceHub)) ev.Allowed = true;
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
							if (keycardItem.Permissions.HasFlagFast(KeycardPermissions.ContainmentLevelTwo)) b1 = true;
							if (keycardItem.Permissions.HasFlagFast(KeycardPermissions.Checkpoints)) b2 = true;
						}
					}
					catch { }
				}
				if (b1 && b2) ev.Allowed = true;
			}
		}
		public void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Status != Qurre.API.Objects.GeneratorStatus.Unlocked) return;
			ev.Allowed = false;
			if (ev.Player.Team == Team.SCP) return;
			var playerIntentory = ev.Player.AllItems;
			foreach (var item in playerIntentory)
			{
				try
				{
					InventorySystem.Items.Keycards.KeycardItem keycardItem;
					if (ev.Player.Inventory.CurInstance != null && (keycardItem = item.Base as InventorySystem.Items.Keycards.KeycardItem) != null &&
						keycardItem.Permissions.HasFlagFast(KeycardPermissions.ArmoryLevelTwo))
						ev.Allowed = true;
				}
				catch { }
				if (item == null)
					continue;
			}
		}
		#endregion
	}
}