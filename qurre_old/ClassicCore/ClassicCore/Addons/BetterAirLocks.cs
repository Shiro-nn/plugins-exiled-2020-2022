using Interactables.Interobjects.DoorUtils;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Linq;
using UnityEngine;
namespace ClassicCore.Addons
{
    internal static class BetterAirLocks
    {
        internal static void Init()
        {
            var list = Map.Rooms.Where(x => x.Type == RoomType.LczAirlock);
            foreach (var room in list) Spawn(room);
            static void Spawn(Room room)
            {
                Model model = new("Better AirLocks", room.Position, room.Rotation.eulerAngles);
                DoorPermissions perms = new()
                {
                    RequiredPermissions = KeycardPermissions.ArmoryLevelOne,
                    RequireAll = false,
                    Bypass2176 = true
                };
                model.AddPart(new ModelDoor(model, DoorPrefabs.DoorEZ, new(0, 0, -4.81f), Vector3.zero, Vector3.one, perms));
                GameObject gm = new();
                gm.transform.parent = model.GameObject.transform;
                gm.transform.localPosition = new(-4.05f, 0, -7.4f);
                gm.transform.localRotation = Quaternion.Euler(new(0, 90));
                WorkStation.Create(gm.transform.position, gm.transform.rotation.eulerAngles, Vector3.one);
                model.AddPart(new ModelLocker(model, LockerPrefabs.RifleRack, new(4.13f, 0, -7.41f), new(0, -90), Vector3.one));
            }
        }
    }
}