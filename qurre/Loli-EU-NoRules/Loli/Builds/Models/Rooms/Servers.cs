using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using Mirror;
using MEC;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Builds.Models.Rooms
{
    internal class Servers
    {
        internal readonly Model Model;
        internal readonly GameObject DoorToRoom;
        private readonly GameObject Door1;
        private readonly GameObject Door2;
        private bool Opened = false;
        private bool Opening = false;
        internal static readonly List<Servers> ThisList = new();
        internal static readonly List<ModelPrimitive> Monitors = new();
        static internal readonly Dictionary<Door, Servers> _doorOpen = new();


        [EventMethod(PlayerEvents.InteractDoor)]
        internal static void Event(InteractDoorEvent ev)
        {
            if (!_doorOpen.TryGetValue(ev.Door, out var servers))
                return;
            ev.Allowed = false;
            if (servers.Opening) return;
            servers.InteractDoor();
            ev.Door.Open = !ev.Door.Open;
        }

        internal void InteractDoor()
        {
            if (Opening) return;
            if (Opened) Timing.RunCoroutine(CloseDoor(), "DoorOpenCloseInServersRoom");
            else Timing.RunCoroutine(OpenDoor(), "DoorOpenCloseInServersRoom");
            IEnumerator<float> OpenDoor()
            {
                Opening = true;
                var p1 = Door1.transform.localPosition;
                var p2 = Door2.transform.localPosition;
                Door1.transform.localPosition = new Vector3(p1.x, p1.y, 1);
                Door2.transform.localPosition = new Vector3(p2.x, p2.y, -1);
                for (float i = 0; 100 >= i; i++)
                {
                    try { Door1.transform.localPosition = new Vector3(p1.x, p1.y, 1 + 0.02f * i); } catch { }
                    try { Door2.transform.localPosition = new Vector3(p2.x, p2.y, -1 - 0.02f * i); } catch { }
                    yield return Timing.WaitForSeconds(0.05f);
                }
                Opened = true;
                Opening = false;
            }
            IEnumerator<float> CloseDoor()
            {
                Opening = true;
                var p1 = Door1.transform.localPosition;
                var p2 = Door2.transform.localPosition;
                Door1.transform.localPosition = new Vector3(p1.x, p1.y, 3);
                Door2.transform.localPosition = new Vector3(p2.x, p2.y, -3);
                for (float i = 0; 100 >= i; i++)
                {
                    try { Door1.transform.localPosition = new Vector3(p1.x, p1.y, 3 - 0.02f * i); } catch { }
                    try { Door2.transform.localPosition = new Vector3(p2.x, p2.y, -3 + 0.02f * i); } catch { }
                    yield return Timing.WaitForSeconds(0.05f);
                }
                Opened = false;
                Opening = false;
            }
        }
        internal Servers(Vector3 position, Vector3 rotation)
        {
            Color32 steklo = new(0, 133, 155, 150);
            Color32 steklo_black = new(0, 0, 0, 150);

            Model = new Model("Servers_Room", position, rotation, new Vector3(0.725f, 0.725f, 0.725f));
            ThisList.Add(this);
            #region Decoration

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, new Vector3(0, -3f), new Vector3(90, 0), new Vector3(20, 20, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, Vector3.zero, new Vector3(90, 0), new Vector3(20, 20, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(0, 4.87f), new Vector3(270, 0), new Vector3(20, 20, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(0, 2.447021f, 10), Vector3.zero, new Vector3(20, 4.9f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(0, 2.447021f, -10), new Vector3(0, 180), new Vector3(20, 4.9f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(10, 2.447021f, 0), new Vector3(0, 90), new Vector3(20, 4.9f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-10, 2.447021f, 0), new Vector3(0, 270), new Vector3(20, 4.9f, 0.1f)));

            /*
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, Vector3.zero, new Vector3(90, 0), new Vector3(20, 20, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, new Vector3(0, 4.87f), new Vector3(270, 0), new Vector3(20, 20, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, new Vector3(0, 2.447021f, 10), Vector3.zero, new Vector3(20, 4.9f, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, new Vector3(0, 2.447021f, -10), new Vector3(0, 180), new Vector3(20, 4.9f, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, new Vector3(10, 2.447021f, 0), new Vector3(0, 90), new Vector3(20, 4.9f, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Color.white, new Vector3(-10, 2.447021f, 0), new Vector3(0, 270), new Vector3(20, 4.9f, 1)));
            */

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(4.25f, 2.447f, -5), new Vector3(0, 270), new Vector3(10, 4.9f, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(4.25f, 2.447f, 6.89f), new Vector3(0, 270), new Vector3(6, 4.9f, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(4.25f, 4.07f, 1.95f), new Vector3(0, 90), new Vector3(3.9f, 1.6f, 0.4f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(0, 0.33f, -3.7f), new Vector3(0, 120), new Vector3(1.9f, 0.7f, 0.7f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-4, 0.33f, -3.7f), new Vector3(0, 120), new Vector3(1.9f, 0.7f, 0.7f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-7.51f, 0.33f, -3.7f), new Vector3(0, 120), new Vector3(1.9f, 0.7f, 0.7f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(0, 0.33f, 7), new Vector3(0, 220), new Vector3(1.9f, 0.7f, 0.7f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-4, 0.33f, 7), new Vector3(0, 220), new Vector3(1.9f, 0.7f, 0.7f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-7.51f, 0.33f, 7), new Vector3(0, 220), new Vector3(1.9f, 0.7f, 0.7f)));

            {
                var Door = new Model("Door", new Vector3(4.25f, 1.635f, 1.95f), Vector3.zero, Model);
                var obj1 = new ModelPrimitive(Door, PrimitiveType.Cube, steklo, new Vector3(0, 0, 1), new Vector3(0, 90), new Vector3(2, 3.265f, 0.35f));
                Door.AddPart(obj1);
                Door1 = obj1.GameObject;
                var obj2 = new ModelPrimitive(Door, PrimitiveType.Cube, steklo, new Vector3(0, 0, -1), new Vector3(0, 90), new Vector3(2, 3.265f, 0.35f));
                Door.AddPart(obj2);
                Door2 = obj2.GameObject;
            }

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-3.911f, 0.74f, -6.224f), Vector3.zero, new Vector3(12, 1.5f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, steklo_black, new Vector3(-3.911f, 2.99f, -6.224f), Vector3.zero, new Vector3(12, 3, 0.09f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-3.911f, 4.6761f, -6.224f), Vector3.zero, new Vector3(12, 0.37f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(2.133f, 2.44f, -6.224f), Vector3.zero, new Vector3(0.1f, 4.9f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.white, new Vector3(-9.952f, 2.44f, -6.224f), Vector3.zero, new Vector3(0.1f, 4.9f, 0.1f)));
            {

                var _obj = new ModelPrimitive(Model, PrimitiveType.Cube, new Color32(33, 37, 37, 255),
                    new Vector3(3.106f, 2.44f, -6.224f), Vector3.zero, new Vector3(1.9f, 4.9f, 0.08f));
                Model.AddPart(_obj, false);
                DoorToRoom = _obj.GameObject;
            }

            CreateMonitor(new(2.414f, 3.82f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(-0.086f, 3.82f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(-2.59f, 3.82f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(-5.09f, 3.82f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(-7.59f, 3.82f, -9.94f), new(0, 90), Vector3.one);

            CreateMonitor(new(-6.304f, 2.716f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(-3.8f, 2.716f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(-1.3f, 2.716f, -9.94f), new(0, 90), Vector3.one);
            CreateMonitor(new(1.1f, 2.716f, -9.94f), new(0, 90), Vector3.one);

            CreateMonitor(new(-9.941f, 2.94f, -8.093f), new(0, 180), new(1, 5, 3.5f));

            CreatePanel(new(-9.435f, 0.491f, -8.163f), new(0, 180));


            Model.AddPart(new ModelLight(Model, new Color32(33, 46, 82, 255), new Vector3(-2, 3.5f, 2), 2, 10));
            Model.AddPart(new ModelLight(Model, new Color32(33, 46, 82, 255), new Vector3(6.891f, 3.539f, 6.686f), 1, 10));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.red, new(4.391f, 1.689f, 4.857f), new(0, 0, 90), new(0.3f, 0.1f, 0.3f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.red, new(4.122f, 1.689f, 4.857f), new(0, 0, 90), new(0.3f, 0.1f, 0.3f)));

            {
                GameObject zero = new();
                zero.transform.parent = Model.GameObject.transform;
                zero.transform.localPosition = new(4.351722f, 3.434469f, 6.121384f);
                zero.transform.localRotation = Quaternion.Euler(new(180, 90));

                Door door = new(zero.transform.position, DoorPrefabs.DoorHCZ, zero.transform.rotation);
                _doorOpen.Add(door, this);
                door.Name = Load.StaticDoorName;
            }
            {
                GameObject zero = new();
                zero.transform.parent = Model.GameObject.transform;
                zero.transform.localPosition = new(4.146107f, -0.09378367f, 6.121384f);
                zero.transform.localRotation = Quaternion.Euler(new(0, 90));

                Door door = new(zero.transform.position, DoorPrefabs.DoorHCZ, zero.transform.rotation);
                _doorOpen.Add(door, this);
                door.Name = Load.StaticDoorName;
            }

            void CreateMonitor(Vector3 _pos, Vector3 _rot, Vector3 scale)
            {
                var Monitor = new Model("Monitor", _pos, _rot, Model);
                Monitor.AddPart(new ModelPrimitive(Monitor, PrimitiveType.Cube, new Color32(48, 48, 48, 255), Vector3.zero, Vector3.zero,
                    new Vector3(0.02f * scale.x, 0.7f * scale.y, 1 * scale.z)));
                var mon = new ModelPrimitive(Monitor, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(-0.015f, 0), new(0, 90),
                    new Vector3(0.97f * scale.z, 0.67f * scale.y, 1));
                Monitor.AddPart(mon);
                Monitors.Add(mon);
            }
            void CreatePanel(Vector3 _pos, Vector3 _rot)
            {
                var Panel = new Model("Panel", _pos, _rot, Model);
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Cube, Color.gray, Vector3.zero, new(90, 90), new Vector3(1, 0.7f, 1)));
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(0.173f, 0.506f, 0.34f), new(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(0.173f, 0.506f, 0.112f), new(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(0.173f, 0.506f, -0.13f), new(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(0.173f, 0.506f, -0.339f), new(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(-0.029f, 0.506f, 0.34f), new(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Panel.AddPart(new ModelPrimitive(Panel, PrimitiveType.Quad, Control.GetRandomMonitorColor(), new(-0.029f, 0.506f, -0.339f), new(90, 0), new Vector3(0.1f, 0.1f, 1)));
                {
                    var door = new ModelDoor(Model, DoorPrefabs.DoorHCZ, new(-9.371f, 0.971f, -8.016f), new(90, 180), new(0.094f, 0.13f, 1));
                    Model.AddPart(door);
                    ServersManager.Buttons.Add(door.Door);
                    door.Door.Name = Load.StaticDoorName;
                    ServersManager.PanelPosition = door.Door.Position;
                }
            }
            Timing.RunCoroutine(SpawnServers());
            IEnumerator<float> SpawnServers()
            {
                yield return Timing.WaitForSeconds(1);
                new Server(new Vector3(0, 1.5f, 7), new Vector3(0, 220), Model);
                new Server(new Vector3(-4, 1.5f, 7), new Vector3(0, 220), Model);
                new Server(new Vector3(-7.51f, 1.5f, 7), new Vector3(0, 220), Model);

                new Server(new Vector3(0, 1.5f, -3.69f), new Vector3(0, 120), Model);
                new Server(new Vector3(-4, 1.5f, -3.69f), new Vector3(0, 120), Model);
                new Server(new Vector3(-7.51f, 1.5f, -3.69f), new Vector3(0, 120), Model);
                yield break;
            }
            #endregion
        }
    }
}