/*using MEC;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Linq;
using UnityEngine;
namespace Loli.Addons.RolePlay.CutScenes
{
    public class Scp173Scene
    {
        internal void FixTP(SpawnEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom173Pos")) ev.Position = Map.GetRandomSpawnPoint(RoleType.Scp173);
        }
        internal void FixTP(DroppingItemEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void FixTP(PickupItemEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void FixTP(ScpAttackEvent ev)
        {
            if (Plugin.RolePlay && ev.Target.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void FixTP(DiesEvent ev)
        {
            if (Plugin.RolePlay && ev.Target.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void FixTP(DamageEvent ev)
        {
            if (Plugin.RolePlay && ev.Target.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void FixTP(ContainEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void FixTP(InteractLockerEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom173Pos")) ev.Allowed = false;
        }
        internal void StartRound()
        {
            if (!Plugin.RolePlay) return;
            int round = Round.CurrentRound;
            {
                var pl173 = Player.List.Where(x => x.Role == RoleType.Scp173).FirstOrDefault();
                if (pl173 != null)
                {
                    pl173.Tag += "Custom173Pos";
                    pl173.Invisible = true;
                    pl173.Role = RoleType.Tutorial;
                    pl173.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=red>SCP 173</color>\n" +
                        "После катсцены, вы появитесь за SCP 173.</color></size>", 10, true);
                    pl173.AddItem(ItemType.Flashlight);
                    Timing.CallDelayed(23f, () =>
                    {
                        if (round != Round.CurrentRound) return;
                        pl173.Tag = pl173.Tag.Replace("Custom173Pos", "");
                        pl173.BlockSpawnTeleport = true;
                        pl173.Role = RoleType.Scp173;
                        Timing.CallDelayed(0.5f, () =>
                        {
                            pl173.Invisible = false;
                            var rms = Map.Rooms.Where(x => x.Type == RoomType.HczCurve || x.Type == RoomType.HczTCross || x.Type == RoomType.HczCrossing).ToArray();
                            pl173.Position = rms[Random.Range(0, rms.Length - 1)].Position + Vector3.up * 2f;
                        });
                    });
                }
                var door = Qurre.API.Extensions.GetDoor(DoorType.LCZ_173_Connector);
                var rot = door.Rotation.eulerAngles;
                var scp173 = Bot.Create(Map.GetRandomSpawnPoint(RoleType.Scp173), new Vector3(rot.x, rot.y, rot.z), RoleType.Scp173, "Scp173", "5 уровень", "yellow");
                scp173.Direction = MovementDirection.BackWards;
                MovementDirection direct = MovementDirection.Forward;
                int id = 0;
                Vector3 rp = Vector3.zero;
                if (rot.y.Difference(0) < 1)
                {
                    DoSpawn(new Vector3(door.Position.x - 11, door.Position.y + 1.5f, door.Position.z + 5.5f), rot, new Vector3(0, 0, 0.5f));
                    DoSpawn(new Vector3(door.Position.x - 17.5f, door.Position.y + 5.5f, door.Position.z + 10.5f), new Vector3(rot.x, rot.y + 60, rot.z));
                    Timing.CallDelayed(0.5f, () => DTeleport(new Vector3(door.Position.x - 11, door.Position.y + 6, door.Position.z + 3.5f),
                        new Vector3(door.Position.x - 11, door.Position.y + 2, door.Position.z + 7)));
                    rp = new Vector3(door.Position.x, door.Position.y + 1.5f, door.Position.z + 5.5f);
                    direct = MovementDirection.Forward;
                }
                else if (rot.y.Difference(90) < 1)
                {
                    DoSpawn(new Vector3(door.Position.x + 5.5f, door.Position.y + 1.5f, door.Position.z + 11), rot, new Vector3(0.5f, 0, 0));
                    DoSpawn(new Vector3(door.Position.x + 10.5f, door.Position.y + 5.5f, door.Position.z + 17.5f), new Vector3(rot.x, rot.y + 60, rot.z));
                    Timing.CallDelayed(0.5f, () => DTeleport(new Vector3(door.Position.x + 3.5f, door.Position.y + 6, door.Position.z + 11),
                        new Vector3(door.Position.x + 7, door.Position.y + 2, door.Position.z + 11)));
                    rp = new Vector3(door.Position.x + 5.5f, door.Position.y + 1.5f, door.Position.z);
                    direct = MovementDirection.Left;
                }
                else if (rot.y.Difference(180) < 1)
                {
                    DoSpawn(new Vector3(door.Position.x + 11, door.Position.y + 1.5f, door.Position.z - 5.5f), rot, new Vector3(0, 0, -0.5f));
                    DoSpawn(new Vector3(door.Position.x + 17.5f, door.Position.y + 5.5f, door.Position.z - 10.5f), new Vector3(rot.x, rot.y + 60, rot.z));
                    Timing.CallDelayed(0.5f, () => DTeleport(new Vector3(door.Position.x + 11, door.Position.y + 6, door.Position.z - 3.5f),
                        new Vector3(door.Position.x + 11, door.Position.y + 2, door.Position.z - 7)));
                    rp = new Vector3(door.Position.x, door.Position.y + 1.5f, door.Position.z - 5.5f);
                    direct = MovementDirection.BackWards;
                }
                else if (rot.y.Difference(270) < 1)
                {
                    DoSpawn(new Vector3(door.Position.x - 5.5f, door.Position.y + 1.5f, door.Position.z - 11), rot, new Vector3(-0.5f, 0, 0));
                    DoSpawn(new Vector3(door.Position.x - 10.5f, door.Position.y + 5.5f, door.Position.z - 17.5f), new Vector3(rot.x, 0, rot.z));
                    Timing.CallDelayed(0.5f, () => DTeleport(new Vector3(door.Position.x - 3.5f, door.Position.y + 6, door.Position.z - 11),
                        new Vector3(door.Position.x - 7, door.Position.y + 2, door.Position.z - 11)));
                    rp = new Vector3(door.Position.x - 5.5f, door.Position.y + 1.5f, door.Position.z);
                    direct = MovementDirection.Right;
                }
                void DoSpawn(Vector3 pos, Vector3 rot, Vector3 cof = new Vector3())
                {
                    id++;
                    int localId = id;
                    var bot = Bot.Create(pos, rot, RoleType.FacilityGuard, "Facility Guard", "99 уровень", "pumpkin");
                    bot.ItemInHand = ItemType.GunFSP9;
                    float later = 20;
                    if (localId == 1) later = 17;
                    Timing.CallDelayed(later, () =>
                    {
                        if (round != Round.CurrentRound) return;
                        if (localId == 1) scp173.Position = new Vector3(pos.x + cof.x, pos.y + cof.y, pos.z + cof.z);
                        Qurre.API.Controllers.Ragdoll.Create(RoleType.FacilityGuard, pos + Vector3.up, Quaternion.Euler(rot),
                            new UniversalDamageHandler(-1, DeathTranslations.Scp173), "Facility Guard", 0);
                        Map.PlaceBlood(pos + Vector3.up, 1, 3);
                        bot.Destroy();
                    });
                    if (localId == 2)
                    {
                        Timing.CallDelayed(later - 2f, () =>
                        {
                            if (round != Round.CurrentRound) return;
                            var direction = (bot.Position - scp173.Position).normalized;
                            direction.y = 0f;
                            scp173.Player.Transform.rotation = Quaternion.LookRotation(direction);
                            scp173.Movement = PlayerMovementState.Sprinting;
                            scp173.Direction = direct;
                        });
                    }
                }
                void DTeleport(Vector3 pos, Vector3 postpos)
                {
                    int count = 0;
                    var list = Player.List.Where(x => x.Role == RoleType.ClassD).ToList();
                    list.Shuffle();
                    foreach (var pl in list)
                    {
                        count++;
                        if (count == 1) DoTeleport(pl, new Vector3(pos.x - 0.5f, pos.y, pos.z + 0.5f), new Vector3(postpos.x - 0.5f, postpos.y, postpos.z + 0.5f));
                        else if (count == 2) DoTeleport(pl, pos, postpos);
                        else if (count == 3) DoTeleport(pl, new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f), new Vector3(postpos.x + 0.5f, postpos.y, postpos.z + 0.5f));
                        else SeeD(pl);
                    }
                    void DoTeleport(Player pl, Vector3 pos, Vector3 postpos)
                    {
                        pl.Position = pos;
                        Timing.CallDelayed(23f, () =>
                        {
                            if (round != Round.CurrentRound) return;
                            if (pl.Role != RoleType.ClassD) return;
                            pl.Position = postpos;
                        });
                    }
                    void SeeD(Player pl)
                    {
                        pl.Tag += "Custom173Pos";
                        pl.Invisible = true;
                        pl.Role = RoleType.Tutorial;
                        pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#ff9900>Class-D</color>\n" +
                            "После катсцены, вы появитесь в своей камере содержания.</color></size>", 10, true);
                        pl.AddItem(ItemType.Flashlight);
                        Timing.CallDelayed(23f, () =>
                        {
                            if (round != Round.CurrentRound) return;
                            pl.Invisible = false;
                            pl.Tag = pl.Tag.Replace("Custom173Pos", "");
                            pl.Role = RoleType.ClassD;
                        });
                    }
                }
                Timing.CallDelayed(15f, () =>
                {
                    if (round != Round.CurrentRound) return;
                    Qurre.API.Extensions.GetDoor(DoorType.LCZ_173_Gate).Open = true;
                });
                Timing.CallDelayed(21f, () =>
                {
                    if (round != Round.CurrentRound) return;
                    scp173.Position = rp;
                });
                Timing.CallDelayed(22f, () =>
                {
                    if (round != Round.CurrentRound) return;
                    scp173.Destroy();
                });
            }
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
}*/