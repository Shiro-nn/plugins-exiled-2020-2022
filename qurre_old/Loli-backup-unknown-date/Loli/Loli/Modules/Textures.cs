using Interactables.Interobjects.DoorUtils;
using Mirror;
using Qurre.API;
using Qurre.API.Objects;
using UnityEngine;
using Qurre.API.Events;
using Qurre.API.Controllers;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		public void LoadTextures()
		{
			RoundStarted = true;
			Color32 perilla = new(94, 94, 94, 255);
			new Primitive(PrimitiveType.Cube, new Vector3(114.71f, 993.55f, -51.74f), perilla,
				default, new Vector3(63.8f, 1.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(82.82f, 993.55f, -49.38f), perilla,
				Quaternion.Euler(new Vector3(0, 90, 0)), new Vector3(4.7f, 1.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(114.71f, 996.8f, -51.74f), perilla,
				default, new Vector3(63.8f, 1.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(82.82f, 996.8f, -49.38f), perilla,
				Quaternion.Euler(new Vector3(0, 90, 0)), new Vector3(4.7f, 1.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(82.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(87.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(92.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(97.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(102.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(107.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(112.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(117.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(122.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(127.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(132.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(137.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(142.87f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			new Primitive(PrimitiveType.Cube, new Vector3(146.55f, 997.4f, -51.74f), perilla,
				default, new Vector3(0.1f, 6.7f, 0.01f));
			Door door = Qurre.API.Extensions.GetDoor(DoorType.Surface_Nuke);
			Map.Doors.Remove(door);
			Door newdoor = Door.Spawn(door.Position, DoorPrefabs.DoorEZ, door.Rotation, door.Permissions);
			NetworkServer.UnSpawn(newdoor.GameObject);
			newdoor.Name = door.Name;
			if (newdoor.DoorVariant.TryGetComponent<DoorNametagExtension>(out var nametag)) nametag.UpdateName(door.Name);
			NetworkServer.Destroy(door.GameObject);
			NetworkServer.Spawn(newdoor.GameObject);
			Door escapedoor = Door.Spawn(new Vector3(14.3999996f, 995, -43.5f), DoorPrefabs.DoorHCZ);
			escapedoor.Name = "ESCAPE_STAIRS";
			NetworkServer.UnSpawn(escapedoor.GameObject);
			escapedoor.GameObject.transform.localScale = new Vector3(1, 1, 1.5f);
			NetworkServer.Spawn(escapedoor.GameObject);
			escapedoor = Door.Spawn(new Vector3(14.3999996f, 995, -23.2999992f), DoorPrefabs.DoorHCZ);
			escapedoor.Name = "ESCAPE_STAIRS";
			NetworkServer.UnSpawn(escapedoor.GameObject);
			escapedoor.GameObject.transform.localScale = new Vector3(1, 1, 1.5f);
			NetworkServer.Spawn(escapedoor.GameObject);
			escapedoor = Door.Spawn(new Vector3(174.419998f, 983.2399902f, 29.1000004f), DoorPrefabs.DoorHCZ, Quaternion.Euler(new Vector3(0, 90, 0)));
			escapedoor.Name = "ESCAPE_STAIRS";
			NetworkServer.UnSpawn(escapedoor.GameObject);
			escapedoor.GameObject.transform.localScale = new Vector3(1, 1, 1.5f);
			NetworkServer.Spawn(escapedoor.GameObject);
			escapedoor = Door.Spawn(new Vector3(176.199997f, 983.2399902f, 35.2299995f), DoorPrefabs.DoorHCZ);
			escapedoor.Name = "ESCAPE_STAIRS";
			NetworkServer.UnSpawn(escapedoor.GameObject);
			escapedoor.GameObject.transform.localScale = new Vector3(1, 1, 1.5f);
			NetworkServer.Spawn(escapedoor.GameObject);
		}
		internal void DoorAntiBreak(DoorDamageEvent ev)
		{
			if (ev.Door == null) return;
			if (ev.Door.Name == "SURFACE_NUKE" || ev.Door.Name == "ESCAPE_STAIRS" || ev.Door.Name.Contains("GR18_INNER") || ev.Door.Type == DoorType.HID)
			{
				ev.Allowed = false;
			}
		}
		internal void DoorAntiLock(DoorLockEvent ev)
		{
			if (ev.Door == null) return;
			if ((ev.Door.Name == "SURFACE_NUKE" || ev.Door.Name == "ESCAPE_STAIRS" || ev.Door.Name.Contains("GR18_INNER")) && ev.Reason == DoorLockReason.Warhead)
			{
				ev.Allowed = false;
			}
		}
		internal void DoorAntiOpen(DoorOpenEvent ev)
		{
			if (ev.Door == null) return;
			if ((ev.Door.Name == "SURFACE_NUKE" || ev.Door.Name == "ESCAPE_STAIRS" || ev.Door.Name.Contains("GR18_INNER")) &&
				(ev.EventType == DoorEventOpenerExtension.OpenerEventType.WarheadStart || ev.EventType == DoorEventOpenerExtension.OpenerEventType.WarheadCancel))
			{
				ev.Allowed = false;
			}
		}
	}
}