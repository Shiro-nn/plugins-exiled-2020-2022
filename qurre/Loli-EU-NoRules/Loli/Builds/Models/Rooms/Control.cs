using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using SchematicUnity.API.Objects;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Loli.Builds.Models.Rooms
{
    public class Control
    {
        [EventMethod(PlayerEvents.InteractDoor)]
        internal static void Event(InteractDoorEvent ev)
        {
            if (!Buttons.TryGetValue(ev.Door, out Control control))
                return;
            ev.Allowed = false;
            control.Interact(ev.Player);
        }

        internal static HackMode Status { get; private set; } = HackMode.Safe;
        internal static int Proccess { get; private set; } = 0;
        internal static readonly Dictionary<Door, Control> Buttons = new();
        internal static readonly List<ModelPrimitive> Monitors = new();
        private static bool Hacking = false;
        internal const string TimeCoroutinesName = "TimeCoroutine_ControlRoom_LoliPlugin";
        private Vector3 PanelPosition = Vector3.zero;
        private bool SendedBC = false;
        internal void Interact(Player pl)
        {
            if (Hacking) return;
            if (Status == HackMode.Hacked) return;
            if (pl == null) return;
            if (!Panel.AllHacked) return;
            if (!pl.ItsHacker()) return;
            Hacking = true;
            Status = HackMode.Hacking;
            var _color = GetRoomsColor();
            try
            {
                GlobalLights.TurnOff(1);
                Timing.CallDelayed(1f, () => GlobalLights.ChangeColor(_color));
            }
            catch { }
            Timing.RunCoroutine(UpdateColorMonitor(), TimeCoroutinesName);
            Timing.RunCoroutine(CheckDistance());
            IEnumerator<float> UpdateColorMonitor()
            {
                yield return Timing.WaitForSeconds(0.3f);
                for (; ; )
                {
                    try { GlobalLights.ChangeColor(_color); } catch { }
                    foreach (var m in Monitors) try { m.Primitive.Color = Panel._yellow; } catch { }
                    yield return Timing.WaitForSeconds(1.5f);
                    foreach (var m in Monitors) try { m.Primitive.Color = Panel._yellowTusklo; } catch { }
                    yield return Timing.WaitForSeconds(1.5f);
                }
            }
            IEnumerator<float> CheckDistance()
            {
                if (!SendedBC)
                {
                    try
                    {
                        var str = $"<color=rainbow><b>Внимание всему персоналу</b></color>\n" +
                            $"<size=70%><color=#6f6f6f>Замечено хакерское вторжение в системы комплекса на уровне</color></size>\n" +
                            $"<size=70%><color=#6f6f6f>пункта управления, требуется реакция средств самообороны</color></size>";
                        var bc = Map.Broadcast(str.Replace("rainbow", "#ff0000"), 40, true);
                        Timing.RunCoroutine(BcChange(bc, str));
                        Cassie.Send(".g6 .g4 Attention .g2 to all personnel .g4 a hacker Inside is noticed at the level of the control center .g2 .g4 " +
                            "the armed forces are required to go .g3 .g3 to the control center .g5 to clean the conditions . pitch_0.1 .g4 . .g3");
                        SendedBC = true;
                        static IEnumerator<float> BcChange(MapBroadcast bc, string str)
                        {
                            bool red_color = false;
                            for (int i = 0; i < 16; i++)
                            {
                                yield return Timing.WaitForSeconds(1f);
                                var color = "#fdffbb";
                                if (red_color)
                                {
                                    color = "#ff0000";
                                    red_color = false;
                                }
                                else red_color = true;
                                bc.Message = str.Replace("rainbow", color);
                            }
                        }
                    }
                    catch { }
                }
                while (Hacking)
                {
                    yield return Timing.WaitForSeconds(1f);
                    try { Vector3.Distance(PanelPosition, pl.MovementState.Position); }
                    catch
                    {
                        Proccess = 0;
                        Status = HackMode.Safe;
                        try
                        {
                            GlobalLights.TurnOff(1);
                            Timing.CallDelayed(1, () => GlobalLights.SetToDefault());
                        }
                        catch { }
                        Timing.KillCoroutines(TimeCoroutinesName);
                        foreach (var m in Monitors) try { m.Primitive.Color = GetRandomMonitorColor(); } catch { }
                        Hacking = false;
                        yield break;
                    }
                    if (Vector3.Distance(PanelPosition, pl.MovementState.Position) > 7)
                    {
                        Proccess = 0;
                        Status = HackMode.Safe;
                        try
                        {
                            GlobalLights.TurnOff(1);
                            Timing.CallDelayed(1, () => GlobalLights.SetToDefault());
                        }
                        catch { }
                        Timing.KillCoroutines(TimeCoroutinesName);
                        yield return Timing.WaitForSeconds(0.3f);
                        foreach (var m in Monitors) try { m.Primitive.Color = GetRandomMonitorColor(); } catch { }
                        Hacking = false;
                        yield break;
                    }
                    Proccess += 1;
                    if (Proccess > 99)
                    {
                        Proccess = 100;
                        Status = HackMode.Hacked;
                        try
                        {
                            GlobalLights.TurnOff(1);
                            Timing.CallDelayed(1, () => GlobalLights.ChangeColor(Color.red, true, true, true));
                        }
                        catch { }
                        Timing.KillCoroutines(TimeCoroutinesName);
                        yield return Timing.WaitForSeconds(0.3f);
                        foreach (var m in Monitors) try { m.Primitive.Color = _red; } catch { }
                        Hacking = false;
                        try
                        {
                            if (ServersManager.ServersRoom is not null && ServersManager.ServersRoom.DoorToRoom is not null &&
                                ServersManager.ServersRoom.DoorToRoom.transform is not null)
                            {
                                var _transform = ServersManager.ServersRoom.DoorToRoom.transform;
                                var _p = _transform.localPosition;
                                Timing.RunCoroutine(OpenDoorThis(), "TexturesChildAndNotPrefereCoroutine");
                                IEnumerator<float> OpenDoorThis()
                                {
                                    for (float i = 0; 45 >= i; i++)
                                    {
                                        try { _transform.localPosition = new Vector3(_p.x, _p.y + 0.1f * i, _p.z); } catch { }
                                        yield return Timing.WaitForSeconds(0.05f);
                                    }
                                    try { _transform.localPosition = new Vector3(_p.x, _p.y + 4.5f, _p.z); } catch { }
                                    yield break;
                                }
                            }
                        }
                        catch { }
                        yield break;
                    }
                }
            }
            Color GetRoomsColor()
            {
                if (Status == HackMode.Hacked) return Color.red;
                if (Status == HackMode.Hacking) return Color.yellow;
                return Color.white;
            }
        }
        public Control()
        {
            Status = HackMode.Safe; Proccess = 0; Hacking = false;
            Scheme = SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Pathes.Plugins, "Schemes", "Waiting_Room.json"), Vector3.zero, default);
            foreach (var _obj in Scheme.Objects)
                findObjects(_obj);
            static void findObjects(SObject obj)
            {
                switch (obj.Name)
                {
                    case "Steklo":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color32(0, 133, 155, 150);
                            }
                            break;
                        }
                    case "Monitor":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = GetRandomMonitorColor();
                            }
                            break;
                        }
                    case "ProjectorLight":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color(20, 20, 20);
                            }
                            break;
                        }
                    case "Polotno_":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color(3, 3, 3);
                            }
                            break;
                        }
                    case "ButtonClick":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color(5, 0, 0);
                            }
                            break;
                        }
                }
                foreach (var _obj in obj.Childrens)
                    findObjects(_obj);
            }

            {
                Door _door = new(new(-129.088f, 991.736f, -73.263f), DoorPrefabs.DoorHCZ, Quaternion.Euler(new(90, 0)))
                {
                    Scale = new(0.094f, 0.13f, 1),
                    Name = Load.StaticDoorName
                };
                if (!Buttons.ContainsKey(_door)) Buttons.Add(_door, this);
                PanelPosition = _door.Position;
            }



            try { new Item(ItemType.KeycardFacilityManager).Spawn(new(-128.787f, 991.92f, -71.456f), Quaternion.Euler(new(0, -146))); }
            catch (System.Exception e) { Log.Warn(e); }

            Door door = DoorType.GR18_Inner.GetDoor();
            try
            {
                new Door(door.Position, DoorPrefabs.DoorLCZ, door.Rotation, door.Permissions)
                {
                    Name = door.Name
                };
            }
            catch { }
            door.Name = "Control Room";
            door.Position = new Vector3(-131.846f, 991.1392f, -60.288f);
            door.Rotation = Quaternion.Euler(Vector3.zero);
            door.Scale = new Vector3(1, 1, 1.5f);
        }

        internal static readonly Color _cyan = new(0, 3, 3);
        internal static readonly Color _red = new(3, 0, 0);
        internal static readonly Color _yellow = new(3, 3, 0);
        internal static readonly Color _lime = new(0, 3, 0);
        internal static readonly Color _blue = new(0, 0, 3);
        internal static readonly Color _magenta = new(3, 0, 3);
        internal static Color GetRandomMonitorColor()
        {
            var rand = Random.Range(1, 5);
            if (rand == 1) return _cyan;
            if (rand == 2) return _lime;
            if (rand == 3) return _blue;
            if (rand == 4) return _magenta;
            return _red;
        }

        public readonly Scheme Scheme;
    }
}