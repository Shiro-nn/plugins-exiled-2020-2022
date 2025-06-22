using Interactables.Interobjects.DoorUtils;
using Mirror;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;

namespace Loli.Builds.Models.Rooms
{
	static class SurfaceObjects
	{
		[EventMethod(RoundEvents.Start)]
		static void Load()
		{
			Door door = DoorType.Surface_Nuke.GetDoor();
			Map.Doors.Remove(door);
			Door newdoor = new(door.Position, DoorPrefabs.DoorEZ, door.Rotation, door.Permissions);
			NetworkServer.UnSpawn(newdoor.GameObject);
			newdoor.Name = door.Name;
			if (newdoor.DoorVariant.TryGetComponent<DoorNametagExtension>(out var nametag)) nametag.UpdateName(door.Name);
			NetworkServer.Destroy(door.GameObject);
			NetworkServer.Spawn(newdoor.GameObject);
		}
	}
}