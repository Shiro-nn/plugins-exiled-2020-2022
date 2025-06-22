using MEC;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.Builds.Models
{
    internal class Panel
    {
        [EventMethod(PlayerEvents.InteractDoor)]
        internal static void Event(InteractDoorEvent ev)
        {
            if (!Panels.TryGetValue(ev.Door, out Panel panel))
                return;
            ev.Allowed = false;
            panel.Interact(ev.Player);
        }

        internal HackMode Status { get; private set; } = HackMode.Safe;
        internal int Proccess { get; private set; } = 0;
        internal readonly Model Model;
        internal readonly ModelPrimitive Monitor;
        internal readonly Room Room;
        internal static readonly Dictionary<Door, Panel> Panels = new();
        internal readonly static Color _red = new(3, 0, 0);
        internal readonly static Color _yellow = new(3, 3, 0);
        internal readonly static Color _yellowTusklo = new(1, 1, 0);
        internal readonly static Color _lime = new(0, 3, 0);
        private static int Id = 0;
        private bool Hacking = false;
        internal const string TimeCoroutinesName = "TimeCoroutine_PanelModel_LoliPlugin";
        internal static bool AllHacked => Panels.Where(x => x.Value.Proccess < 100).Count() == 0;
        internal void Interact(Player pl)
        {
            if (Hacking) return;
            if (Status == HackMode.Hacked) return;
            if (pl == null) return;
            if (!pl.ItsHacker()) return;
            Hacking = true;
            Status = HackMode.Hacking;
            var _color = GetRoomColor();
            try
            {
                if (Room is not null)
                {
                    Room.LightsOff(1);
                    Timing.CallDelayed(1f, () =>
                    {
                        Room.Lights.LockChange = false;
                        Room.Lights.Color = _color;
                        Room.Lights.LockChange = true;
                    });
                }
            }
            catch { }
            Timing.RunCoroutine(UpdateColorMonitor(), TimeCoroutinesName);
            Timing.RunCoroutine(CheckDistance());
            IEnumerator<float> UpdateColorMonitor()
            {
                yield return Timing.WaitForSeconds(0.3f);
                for (; ; )
                {
                    try { Monitor.Primitive.Color = _yellow; } catch { }
                    yield return Timing.WaitForSeconds(1.5f);
                    try { Monitor.Primitive.Color = _yellowTusklo; } catch { }
                    yield return Timing.WaitForSeconds(1.5f);
                }
            }
            IEnumerator<float> CheckDistance()
            {
                while (Hacking)
                {
                    yield return Timing.WaitForSeconds(1f);
                    try { Vector3.Distance(Model.GameObject.transform.position, pl.MovementState.Position); }
                    catch
                    {
                        Proccess = 0;
                        Status = HackMode.Safe;
                        try
                        {
                            if (Room is not null)
                            {
                                Room.LightsOff(1);
                                Timing.CallDelayed(1f, () =>
                                {
                                    Room.Lights.LockChange = false;
                                    Room.Lights.Override = false;
                                });
                            }
                        }
                        catch { }
                        Timing.KillCoroutines(TimeCoroutinesName);
                        try { Monitor.Primitive.Color = _lime; } catch { }
                        Hacking = false;
                        yield break;
                    }
                    if (Vector3.Distance(Model.GameObject.transform.position, pl.MovementState.Position) > 3)
                    {
                        Proccess = 0;
                        Status = HackMode.Safe;
                        try
                        {
                            if (Room is not null)
                            {
                                Room.LightsOff(1);
                                Timing.CallDelayed(1f, () =>
                                {
                                    Room.Lights.LockChange = false;
                                    Room.Lights.Override = false;
                                });
                            }
                        }
                        catch { }
                        Timing.KillCoroutines(TimeCoroutinesName);
                        yield return Timing.WaitForSeconds(0.3f);
                        try { Monitor.Primitive.Color = _lime; } catch { }
                        Hacking = false;
                        yield break;
                    }
                    Proccess += 3;
                    if (Proccess > 99)
                    {
                        Proccess = 100;
                        Status = HackMode.Hacked;
                        try
                        {
                            if (Room is not null)
                            {
                                Room.LightsOff(1);
                                Timing.CallDelayed(1f, () =>
                                {
                                    Room.Lights.LockChange = false;
                                    Room.Lights.Color = Color.red;
                                    Room.Lights.LockChange = true;
                                });
                            }
                        }
                        catch { }
                        Timing.KillCoroutines(TimeCoroutinesName);
                        yield return Timing.WaitForSeconds(0.3f);
                        try { Monitor.Primitive.Color = _red; } catch { }
                        Hacking = false;
                        yield break;
                    }
                }
            }
            Color GetRoomColor()
            {
                if (Status == HackMode.Hacked) return Color.red;
                if (Status == HackMode.Hacking) return Color.yellow;
                return Color.white;
            }
        }
        internal Panel(Vector3 pos, Vector3 rot, Transform root = null, Room room = null)
        {
            Id++; Room = room;
            Model DoModel = null; if (root is not null) DoModel = new("", root.position, root.rotation.eulerAngles, new Vector3(0.725f, 0.725f, 0.725f));
            Model = new Model($"Panel #{Id}", pos, rot, DoModel);
            Color32 bp = new(0, 57, 81, 255);

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.gray, new(0, 0, -0.028f), Vector3.zero, new(0.9f, 0.7f, 0.07f)));
            {
                ModelPrimitive _obj = new(Model, PrimitiveType.Cube, _lime, new(0, 0.032f, -0.063f), Vector3.zero, new(0.85f, 0.6f, 0.01f));
                Model.AddPart(_obj, false);
                Monitor = _obj;
            }
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.gray, new(0, -0.45f, -0.25f), new(56, 0), new(0.9f, 0.5f, 0.07f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.gray, new(0, -0.5833f, -0.481f), new(56, 0), new(0.1f, 0.1f, 0.02f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.green, new(-0.332f, -0.33f, -0.16f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.blue, new(-0.199f, -0.33f, -0.16f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.yellow, new(-0.066f, -0.33f, -0.16f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, bp, new(0.073f, -0.33f, -0.16f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.cyan, new(0.212f, -0.33f, -0.16f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.green, new(0.343f, -0.33f, -0.16f), new(56, 0), new(0.1f, 0.1f, 0.03f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.red, new(-0.332f, -0.403f, -0.264f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, bp, new(-0.199f, -0.403f, -0.264f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.cyan, new(-0.066f, -0.403f, -0.264f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.blue, new(0.073f, -0.403f, -0.264f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.red, new(0.212f, -0.403f, -0.264f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.yellow, new(0.343f, -0.403f, -0.264f), new(56, 0), new(0.1f, 0.1f, 0.03f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.blue, new(-0.332f, -0.476f, -0.369f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.yellow, new(-0.199f, -0.476f, -0.369f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.green, new(-0.066f, -0.476f, -0.369f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, bp, new(0.073f, -0.476f, -0.369f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.yellow, new(0.212f, -0.476f, -0.369f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.cyan, new(0.343f, -0.476f, -0.369f), new(56, 0), new(0.1f, 0.1f, 0.03f)));
            {
                var door = new ModelDoor(Model, DoorPrefabs.DoorHCZ, new(0.0024f, -0.5881f, -0.5387f), new(55.14f, 0, 0), new(0.0714286f, 0.02857143f, 0.2857143f));
                Model.AddPart(door);
                Panels.Add(door.Door, this);
                door.Door.Name = Load.StaticDoorName;
            }
        }
        internal static void Initialize()
        {
            Panels.Clear();
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.HczServers);
                new Panel(new(-4 / 0.725f, 1.69f / 0.725f, 3.106f / 0.725f), new(0, 90), room.Transform, room);
            }
            catch { }
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.Hcz079);
                new Panel(new(-7.57f / 0.725f, -3.87f / 0.725f, -9.752f / 0.725f), new(0, 270), room.Transform, room);
            }
            catch { }
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.HczHid);
                new Panel(new(-5.04f / 0.725f, 1.63f / 0.725f, -5.91f / 0.725f), new(0, 180), room.Transform, room);
            }
            catch { }
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.HczNuke);
                new Panel(new(2.555f / 0.725f, 292.43f / 0.725f, 10.9f / 0.725f), Vector3.zero, room.Transform, room);
            }
            catch { }
        }
    }
}