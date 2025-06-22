using MEC;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Linq;
using UnityEngine;
namespace Loli.Addons.RolePlay.CutScenes
{
    internal class Default
    {
        internal void Start()
        {
            if (!Plugin.RolePlay) return;
            {
                var scp939spawn = Map.GetRandomSpawnPoint(RoleType.Scp93953);
                foreach (Door _door in Map.Doors.Where(x => x.Type == DoorType.PrisonDoor || x.Type == DoorType.LCZ_173_Connector ||
                (x.Type == DoorType.Airlock && x.Position.y.Difference(scp939spawn.y) < 3)))
                {
                    _door.Locked = true;
                    _door.Open = false;
                }
            }
            int round = Round.CurrentRound;
            var door = Qurre.API.Extensions.GetDoor(DoorType.LCZ_173_Connector);
            var rot = door.Rotation.eulerAngles;
            int id = 0;
            if (rot.y.Difference(0) < 1)
            {
                DoSpawn(new Vector3(door.Position.x - 11, door.Position.y + 1.5f, door.Position.z + 5.5f), rot, new Vector3(0, 0, 0.5f));
                DoSpawn(new Vector3(door.Position.x - 17.5f, door.Position.y + 5.5f, door.Position.z + 10.5f), new Vector3(rot.x, rot.y + 60, rot.z));
            }
            else if (rot.y.Difference(90) < 1)
            {
                DoSpawn(new Vector3(door.Position.x + 5.5f, door.Position.y + 1.5f, door.Position.z + 11), rot, new Vector3(0.5f, 0, 0));
                DoSpawn(new Vector3(door.Position.x + 10.5f, door.Position.y + 5.5f, door.Position.z + 17.5f), new Vector3(rot.x, rot.y + 60, rot.z));
            }
            else if (rot.y.Difference(180) < 1)
            {
                DoSpawn(new Vector3(door.Position.x + 11, door.Position.y + 1.5f, door.Position.z - 5.5f), rot, new Vector3(0, 0, -0.5f));
                DoSpawn(new Vector3(door.Position.x + 17.5f, door.Position.y + 5.5f, door.Position.z - 10.5f), new Vector3(rot.x, rot.y + 60, rot.z));
            }
            else if (rot.y.Difference(270) < 1)
            {
                DoSpawn(new Vector3(door.Position.x - 5.5f, door.Position.y + 1.5f, door.Position.z - 11), rot, new Vector3(-0.5f, 0, 0));
                DoSpawn(new Vector3(door.Position.x - 10.5f, door.Position.y + 5.5f, door.Position.z - 17.5f), new Vector3(rot.x, 0, rot.z));
            }
            void DoSpawn(Vector3 pos, Vector3 rot, Vector3 cof = new Vector3())
            {
                id++;
                int localId = id;
                float later = 20;
                if (localId == 1) later = 17;
                Timing.CallDelayed(later, () =>
                {
                    if (round != Round.CurrentRound) return;
                    Qurre.API.Controllers.Ragdoll.Create(RoleType.FacilityGuard, pos + Vector3.up, Quaternion.Euler(rot),
                        new UniversalDamageHandler(-1, DeathTranslations.Scp173), "Facility Guard", 0);
                    Map.PlaceBlood(pos + Vector3.up, 1, 3);
                });
            }
            Timing.CallDelayed(15f, () =>
            {
                if (round != Round.CurrentRound) return;
                Qurre.API.Extensions.GetDoor(DoorType.LCZ_173_Gate).Open = true;
            });
            Cassie.Send("Attention to all personnel . . Now there will be an REDACTED with scp 1 7 3 . . pitch_.2 .g4 .g4 pitch_1 Security Error detected . pitch_.2 .g4 .g4 pitch_1 . Containment breach for Keter and Euclid level detected on site . Full site lockdown initiated");
            Timing.CallDelayed(15f, () =>
            {
                if (round != Round.CurrentRound) return;
                Lights.TurnOff(30f);
            });
            Timing.CallDelayed(20f, () =>
            {
                if (round != Round.CurrentRound) return;
                var room = Qurre.API.Extensions.GetRoom(RoomType.LczClassDSpawn);
                Qurre.API.Controllers.Ragdoll.Create(RoleType.FacilityGuard, room.Position + Vector3.up, Quaternion.identity,
                    new UniversalDamageHandler(-1, DeathTranslations.Scp173), "Facility Guard", 0);
                Scp173.PlaceTantrum(room.Position + Vector3.up);
                Map.PlaceBlood(room.Position + Vector3.up, 1, 3);
            });
            Timing.CallDelayed(25f, () =>
            {
                if (round != Round.CurrentRound) return;
                var scp939spawn = Map.GetRandomSpawnPoint(RoleType.Scp93953);
                foreach (Door door in Map.Doors.Where(x => x.Type == DoorType.PrisonDoor || x.Type == DoorType.LCZ_173_Connector ||
                (x.Type == DoorType.Airlock && x.Position.y.Difference(scp939spawn.y) < 3)))
                {
                    door.Locked = true;
                    door.Open = true;
                }
            });
            Timing.CallDelayed(43f, () =>
            {
                if (round != Round.CurrentRound) return;
                Lights.ChangeColor(Color.red);
                Timing.CallDelayed(15f, () =>
                {
                    if (round != Round.CurrentRound) return;
                    Cassie.Send("sending the error to the o5 . . . no answer from O5 . . . an attempt to manually enable the security system . . . . security system enabled");
                    Timing.CallDelayed(14f, () =>
                    {
                        if (round != Round.CurrentRound) return;
                        Lights.TurnOff(2f);
                    });
                    Timing.CallDelayed(15f, () =>
                    {
                        if (round != Round.CurrentRound) return;
                        foreach (var room in Map.Rooms)
                            room.LightOverride = false;
                        Qurre.API.Extensions.GetRoom(RoomType.LczClassDSpawn).LightColor = Color.red;
                        Qurre.API.Extensions.GetRoom(RoomType.Lcz173).LightColor = Color.red;
                        Qurre.API.Extensions.GetRoom(RoomType.Hcz106).LightColor = Color.red;
                    });
                });
            });
        }
    }
}