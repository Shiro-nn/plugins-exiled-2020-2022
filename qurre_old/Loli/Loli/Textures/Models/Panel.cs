using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Loli.Textures.Models
{
    internal class Panel
    {
        internal HackMode Status { get; private set; } = HackMode.Safe;
        internal int Proccess { get; private set; } = 0;
        internal readonly Model Model;
        internal readonly ModelPrimitive Monitor;
        internal readonly Room Room;
        internal static readonly Dictionary<ItemPickupBase, Panel> Panels = new();
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
                    try { Vector3.Distance(Model.GameObject.transform.position, pl.Position); }
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
                    if (Vector3.Distance(Model.GameObject.transform.position, pl.Position) > 5)
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
            Model DoModel = null; if (root is not null) DoModel = new("", root.position, root.rotation.eulerAngles);
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
            Timing.CallDelayed(1f, () => Model.Primitives.ForEach(prim => { try { prim.Primitive.Break(); } catch { } }));
            {
                Vector3 _p = new(0, -0.5749f, -0.5053f);
                Vector3 _r = new(55.14f, 0);
                var item = Qurre.API.Server.InventoryHost.CreateItemInstance(ItemType.Painkillers, false);
                ushort ser = ItemSerialGenerator.GenerateNext();
                item.PickupDropModel.Info.Serial = ser;
                item.PickupDropModel.Info.ItemId = ItemType.Painkillers;
                item.PickupDropModel.Info.Position = Model.GameObject.transform.position + _p;
                item.PickupDropModel.Info.Weight = item.Weight;
                item.PickupDropModel.Info.Rotation = new LowPrecisionQuaternion(Quaternion.Euler(Model.GameObject.transform.rotation.eulerAngles + _r));
                item.PickupDropModel.NetworkInfo = item.PickupDropModel.Info;
                ItemPickupBase ipb = Object.Instantiate(item.PickupDropModel);
                var gameObject = ipb.gameObject;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.transform.parent = Model.GameObject.transform;
                gameObject.transform.localPosition = _p;
                gameObject.transform.localRotation = Quaternion.Euler(_r);
                gameObject.transform.localScale = new Vector3(1.4f, 1.4f, 0.21f);
                NetworkServer.Spawn(gameObject);
                ipb.InfoReceived(default, item.PickupDropModel.NetworkInfo);
                if (!Panels.ContainsKey(ipb)) Panels.Add(ipb, this);
            }
        }
        internal static void Initialize()
        {
            Panels.Clear();
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.HczServers);
                new Panel(new(-5.5f, 2.3f, 4.26f), new(0, 90), room.Transform, room);
            }
            catch { }
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.Hcz079);
                new Panel(new(-10.4f, -5.5f, -13), new(0, 270), room.Transform, room);
            }
            catch { }
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.HczHid);
                new Panel(new(-6.955f, 2.2f, -8.75f), new(0, 180), room.Transform, room);
            }
            catch { }
            try
            {
                var room = Map.Rooms.Find(x => x.Type == RoomType.HczNuke);
                new Panel(new(3.56f, 401.9f, 15), Vector3.zero, room.Transform, room);
            }
            catch { }
        }
    }
}