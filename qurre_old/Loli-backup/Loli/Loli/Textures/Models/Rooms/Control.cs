using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Collections.Generic;
using UnityEngine;
using Light = Qurre.API.Controllers.Light;
namespace Loli.Textures.Models.Rooms
{
    public class Control
    {
        internal static HackMode Status { get; private set; } = HackMode.Safe;
        internal static int Proccess { get; private set; } = 0;
        internal static readonly Dictionary<ItemPickupBase, Control> Buttons = new();
        internal static readonly List<ModelPrimitive> Monitors = new();
        private static bool Hacking = false;
        internal const string TimeCoroutinesName = "TimeCoroutine_ControlRoom_LoliPlugin";
        private Vector3 PanelPosition = Vector3.zero;
        private bool SendedBC = false;
        private readonly List<Model> ModelsToBreak = new();
        internal void Interact(Player pl)
        {
            if (Hacking) return;
            if (Status == HackMode.Hacked) return;
            if (pl == null) return;
            if (!Panel.AllHacked) return;
            if (!pl.ItsHacker() && !pl.ItsSpyFacilityManager()) return;
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
                            $"<size=30%><color=#6f6f6f>Замечено хакерское вторжение в системы комплекса на уровне</color></size>\n" +
                            $"<size=30%><color=#6f6f6f>пункта управления, требуется реакция средств самообороны</color></size>";
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
                    try { Vector3.Distance(PanelPosition, pl.Position); }
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
                    if (Vector3.Distance(PanelPosition, pl.Position) > 15)
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
        public Control(Vector3 position, Vector3 rotation)
        {
            Status = HackMode.Safe; Proccess = 0; Hacking = false;
            Model = new Model("Сontrol_Room", position, rotation);

            Floor = new Model("Floor", new Vector3(-1.15f, -3.9258f, 6.75f), Vector3.zero, Model);
            Plintus = new Model("Plintus", new Vector3(1.15f, 3.9258f, -6.75f), Vector3.zero, Floor);
            Walls = new Model("Walls", Vector3.zero, Vector3.zero, Model);
            SpawnFloor();
            SpawnWalls();
            SpawnPlintus();
            SpawnShkaf(new Vector3(-10.948f, -3.047f, 9.152f));
            SpawnShkaf(new Vector3(-10.948f, -3.047f, 13.03f));
            SpawnTable(new Vector3(-1, -2.91f, 8.15f), new Vector3(0, 268.532f, 0));
            SpawnTable(new Vector3(-0.902f, -2.91f, 13.051f), new Vector3(0, -87.798f, 0));
            SpawnTable(new Vector3(-6.309f, -2.91f, 7.918f), new Vector3(0, 171.284f, 0));

            Model.AddPart(new ModelLight(Model, new Color32(67, 77, 96, 255), new Vector3(-0.586f, 0.397f, 9.242f), 0.63f, 81.4f));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, new Color32(192, 191, 197, 255), new Vector3(-1.52f, 4.435f, 6.87f), Vector3.right * 270, new Vector3(22, 23, 1)));

            SpawnPolotno();

            SpawnProjector(new Vector3(-5.911f, -2.245f, 8.278f), Vector3.zero);

            ModelsToBreak.Add(Model);
            ModelsToBreak.Add(Floor);
            ModelsToBreak.Add(Plintus);
            ModelsToBreak.Add(Walls);

            SpawnManagement();

            Timing.CallDelayed(1.5f, () =>
            {
                try
                {
                    ModelsToBreak.ForEach(model =>
                    {
                        try { model.Primitives.ForEach(prim => { try { prim.Primitive.Break(); } catch { } }); } catch { }
                    });
                    ModelsToBreak.Clear();
                }
                catch { }
            });

            Door door = DoorType.GR18_Inner.GetDoor();
            try
            {
                Door nd = Door.Spawn(door.Position, DoorPrefabs.DoorLCZ, door.Rotation, door.Permissions);
                nd.Name = door.Name;
            }
            catch { }
            door.Name = "Control Room";
            door.Position = new Vector3(2.75f + position.x, position.y - 3.95f, 18.25f + position.z);
            door.Rotation = Quaternion.Euler(rotation);
            door.Scale = new Vector3(1, 1, 1.5f);

            Locker.Create(new(9.07f + position.x, position.y - 3.9f, 11.7f + position.z), LockerPrefabs.MiscLocker, Quaternion.Euler(new(rotation.x, rotation.y - 90, rotation.z)));
            Locker.Create(new(9.04f + position.x, position.y - 3.92f, 13.61f + position.z), LockerPrefabs.Pedestal500, Quaternion.Euler(new(rotation.x, rotation.y - 90, rotation.z)));
            Locker.Create(new(9.198f + position.x, position.y - 3.92f, 15f + position.z), LockerPrefabs.AdrenalineMedkit, Quaternion.Euler(new(rotation.x, rotation.y - 90, rotation.z)));
        }

        private void SpawnFloor()
        {
            Floor.AddPart(new ModelPrimitive(Floor, PrimitiveType.Quad, new Color32(192, 191, 197, 255), Vector3.zero, Vector3.right * 90, new Vector3(20.6f, 22.3f, 1)));
            for (int i = 0; i < 29; i++)
            {
                SpawnLine(new Vector3(-9.68f + i * 0.69f, 0.01f), Vector3.right * 90, new Vector3(0.01f, 22.3f, 1));
            }
            for (int i = 0; i < 29; i++)
            {
                SpawnLine(new Vector3(0, 0.01f, -10.48f + i * 0.76f), new Vector3(90, 0, 90), new Vector3(0.01f, 20.6f, 1));
            }
        }

        private void SpawnLine(Vector3 pos, Vector3 rot, Vector3 scl)
        {
            var line = new ModelPrimitive(Floor, PrimitiveType.Quad, Color.black, pos, rot, scl);
            Object.DestroyImmediate(line.GameObject.GetComponent<Collider>());
            Floor.AddPart(line);
        }

        private void SpawnPlintus()
        {
            Plintus.AddPart(new ModelPrimitive(Plintus, PrimitiveType.Quad, new Color32(56, 49, 38, 255), new Vector3(-5.16f, -3.835f, 17.918f), Vector3.right * 35, new Vector3(14, 0.25f, 1)));
            Plintus.AddPart(new ModelPrimitive(Plintus, PrimitiveType.Quad, new Color32(56, 49, 38, 255), new Vector3(6.79f, -3.835f, 17.918f), Vector3.right * 35, new Vector3(6, 0.25f, 1)));

            Plintus.AddPart(new ModelPrimitive(Plintus, PrimitiveType.Quad, new Color32(56, 49, 38, 255), new Vector3(9.159f, -3.835f, 6.7f), new Vector3(35, 90), new Vector3(22.5f, 0.25f, 1)));

            Plintus.AddPart(new ModelPrimitive(Plintus, PrimitiveType.Quad, new Color32(56, 49, 38, 255), new Vector3(-11.51f, -3.835022f, 6.7f), new Vector3(35, 270), new Vector3(22.5f, 0.25f, 1)));

            Plintus.AddPart(new ModelPrimitive(Plintus, PrimitiveType.Quad, new Color32(56, 49, 38, 255), new Vector3(-1.25f, -3.835022f, -4.42f), new Vector3(35, 180), new Vector3(21, 0.25f, 1)));
        }

        private void SpawnWalls()
        {
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(101, 100, 100, 255), new Vector3(-1.15f, 0.3f, -4.4538f), Vector3.right * 180, new Vector3(20.75f, 8.3f, 1)));
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(59, 57, 56, 255), new Vector3(-1.15f, 4.25f, -4.4528f), Vector3.right * 180, new Vector3(20.75f, 0.5f, 1)));

            // Wall with Door
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(101, 100, 100, 255), new Vector3(-4.851f, 0.2999878f, 17.95f), new Vector3(180, 180, -1.525879e-05f), new Vector3(13.38123f, 8.3f, 1)));
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(101, 100, 100, 255), new Vector3(6.583185f, 0.2999878f, 17.95f), new Vector3(180, 180, -1.525879e-05f), new Vector3(5.588285f, 8.3f, 1)));
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(101, 100, 100, 255), new Vector3(2.814302f, 1.885455f, 17.95f), new Vector3(180, 180, -1.525879e-05f), new Vector3(1.94963f, 5.128448f, 1)));
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(59, 57, 56, 255), new Vector3(-1.149994f, 4.25f, 17.949f), new Vector3(180, 180, -1.525879e-05f), new Vector3(20.75f, 0.5f, 1)));

            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(101, 100, 100, 255), new Vector3(9.191025f, 0.2999878f, 6.748093f), new Vector3(180, -90), new Vector3(22.55f, 8.3f, 1)));
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(59, 57, 56, 255), new Vector3(9.19f, 4.25f, 6.748085f), new Vector3(180, -90), new Vector3(22.55f, 0.5f, 1)));

            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(101, 100, 100, 255), new Vector3(-11.53102f, 0.2999878f, 6.748131f), new Vector3(180, 90, -1.525879e-05f), new Vector3(22.55f, 8.3f, 1)));
            Walls.AddPart(new ModelPrimitive(Walls, PrimitiveType.Quad, new Color32(59, 57, 56, 255), new Vector3(-11.53f, 4.25f, 6.748146f), new Vector3(180, 90, -1.525879e-05f), new Vector3(22.55f, 0.5f, 1)));
        }

        private void SpawnPolotno()
        {
            var Polotno = new Model("Polotno", new Vector3(-6.78f, 0, 17.44f), Vector3.zero, Model);
            Polotno.AddPart(new ModelPrimitive(Polotno, PrimitiveType.Cube, new Color32(33, 33, 33, 255), new Vector3(0.8718672f, -0.3475722f, 0.5123649f), Vector3.zero, new Vector3(6.972076f, 5.031799f, 0.894464f)));
            Polotno.AddPart(new ModelPrimitive(Polotno, PrimitiveType.Cube, new Color32(170, 170, 170, 255), new Vector3(0.8718579f, -0.347608f, 0.1378f), Vector3.zero, new Vector3(6.789643f, 4.839232f, 0.1736615f)));
            ModelsToBreak.Add(Polotno);
        }

        private void SpawnTable(Vector3 pos, Vector3 rot)
        {
            var Table = new Model("Table" + pos, pos, rot, Model);
            Table.AddPart(new ModelPrimitive(Table, PrimitiveType.Cube, new Color32(143, 131, 128, 255), new Vector3(-0.02162f, 0.1633f, -0.13968f), Vector3.zero, new Vector3(1.951784f, 0.09307861f, 2.293263f)));
            Table.AddPart(new ModelPrimitive(Table, PrimitiveType.Cube, new Color32(143, 131, 128, 255), new Vector3(0.011f, 0.2244f, -0.141f), Vector3.zero, new Vector3(0.02843184f, 1.44165f, 2.220153f)));
            Table.AddPart(new ModelPrimitive(Table, PrimitiveType.Cube, new Color32(50, 50, 50, 255), new Vector3(-0.015f, -0.4097f, -1.2199f), Vector3.zero, new Vector3(1.956075f, 1.215393f, 0.1058086f)));
            Table.AddPart(new ModelPrimitive(Table, PrimitiveType.Cube, new Color32(50, 50, 50, 255), new Vector3(-0.015f, -0.4097f, 0.953f), Vector3.zero, new Vector3(1.956075f, 1.215393f, 0.1058086f)));
            ModelsToBreak.Add(Table);
        }

        private void SpawnShkaf(Vector3 pos)
        {
            var Shkaf = new Model("Shkaf_" + pos, pos, Vector3.zero, Model);
            Shkaf.AddPart(new ModelPrimitive(Shkaf, PrimitiveType.Cube, new Color32(59, 57, 56, 255), new Vector3(-0.12f, 0.1223f, 0.2094f), Vector3.zero, new Vector3(0.8533323f, 1.979492f, 3.833826f)));
            Shkaf.AddPart(new ModelPrimitive(Shkaf, PrimitiveType.Cube, new Color32(94, 88, 73, 255), new Vector3(0.30267f, 0.10643f, 0.206069f), Vector3.zero, new Vector3(0.04156493f, 1.971436f, 3.833818f)));

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Shkaf.AddPart(new ModelPrimitive(Shkaf, PrimitiveType.Cube, new Color32(55, 57, 60, 255), new Vector3(0.32689f, 0.95697f - i * 0.49997f, -1.24072f + j * 0.96912f), Vector3.zero, new Vector3(0.01844787f, 0.119751f, 0.2189801f)));
                    Shkaf.AddPart(new ModelPrimitive(Shkaf, PrimitiveType.Cube, new Color32(192, 191, 197, 255), new Vector3(0.34145f, 0.68786f - i * 0.49997f, -1.2402f + j * 0.96912f), Vector3.zero, new Vector3(0.04754641f, 0.02862548f, 0.2577675f)));
                }
            }

            ModelsToBreak.Add(Shkaf);
        }

        private void SpawnProjector(Vector3 pos, Vector3 rot)
        {
            var Projector = new Model("Projector_" + pos, pos, rot, Model);
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cube, new Color32(108, 108, 108, 255), new Vector3(-0.0014f, -0.0833f, -0.0569f), Vector3.right * -12f, new Vector3(0.2794647f, 0.5255723f, 0.9128779f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cube, new Color32(108, 108, 108, 255), new Vector3(-0.0092f, -0.2801f, 0.3538f), Vector3.right * -12f, new Vector3(0.05020143f, 0.4045977f, 0.05267729f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cube, new Color32(108, 108, 108, 255), new Vector3(-0.0092f, -0.2801f, -0.291f), Vector3.zero, new Vector3(0.05020143f, 0.4045977f, 0.05267729f)));

            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(108, 108, 108, 255), new Vector3(0, 0.07f, 0.586f), Vector3.right * 80, new Vector3(0.15f, 0.2f, 0.15f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(108, 108, 108, 255), new Vector3(0, 0.0857f, 0.6783f), Vector3.right * 80, new Vector3(0.1753535f, 0.1119377f, 0.1753535f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(108, 108, 108, 255), new Vector3(0, 0.1001f, 0.7223f), Vector3.right * 80, new Vector3(0.2117027f, 0.09403558f, 0.2117027f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color(20, 20, 20), new Vector3(0, 0.1001f, 0.7261f), Vector3.right * 80, new Vector3(0.18f, 0.09730013f, 0.18f)));

            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cube, new Color32(50, 50, 50, 255), new Vector3(-0.047f, 0.1741f, -0.507f), new Vector3(-12, 0, -12), new Vector3(0.03089911f, 0.2363658f, 0.05791191f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cube, new Color32(50, 50, 50, 255), new Vector3(-0.047f, 0.3537f, 0.282f), new Vector3(-12, 0, -12), new Vector3(0.03089911f, 0.3152799f, 0.05791196f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cube, new Color32(50, 50, 50, 255), new Vector3(-0.021f, 0.382f, -0.134f), new Vector3(-15, 0, -12), new Vector3(0.03089907f, 0.0477551f, 0.8998896f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(50, 50, 50, 255), new Vector3(0.082f, 0.466f, 0.259f), new Vector3(-15, -12, 90), new Vector3(0.05f, 0.1f, 0.05f)));
            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(50, 50, 50, 255), new Vector3(0.084f, 0.271f, -0.501f), new Vector3(-15, -12, 90), new Vector3(0.05f, 0.1f, 0.05f)));

            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(20, 20, 20, 255), new Vector3(0.1735f, 0.277f, -0.5458f), new Vector3(90, 0, 90), new Vector3(0.6828181f, 0.04290074f, 0.6787719f)));

            Projector.AddPart(new ModelPrimitive(Projector, PrimitiveType.Cylinder, new Color32(20, 20, 20, 255), new Vector3(0.1735f, 0.503f, 0.285f), new Vector3(90, 0, 90), new Vector3(0.6828181f, 0.04290074f, 0.6787719f)));

            Projector.AddPart(new ModelLight(Projector, new Color32(105, 255, 168, 255), new Vector3(0.2651563f, -0.2206373f, -0.002845764f), 1, 0.4f, false));

            ModelsToBreak.Add(Projector);
        }

        internal static readonly Color _cyan = new(0, 3, 3);
        internal static readonly Color _red = new(3, 0, 0);
        internal static readonly Color _yellow = new(3, 3, 0);
        internal static readonly Color _lime = new(0, 3, 0);
        internal static readonly Color _blue = new(0, 0, 3);
        internal static readonly Color _magenta = new(3, 0, 3);
        private void SpawnManagement()
        {
            Color32 stol = new(85, 85, 85, 255);
            Vector3 pos = new(0, 0, 2.9f);
            Vector3 rot = Vector3.zero;
            var MSR = new Model("ManagementSubRoom", pos, rot, Model);
            #region Стены, плитка и т.д
            {
                Color32 main = new(45, 45, 43, 255);
                Color32 steklo = new(0, 133, 155, 150);
                float d = 0.2f;
                float d2 = 0.17f;
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, main, new Vector3(4.21f, -3.832f, 2.39f), Vector3.zero, new Vector3(10, d, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, main, new Vector3(-8.08f, -3.832f, 2.39f), Vector3.zero, new Vector3(7, d, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, main, new Vector3(-4.68f, -1.35f, 2.39f), new Vector3(0, 0, 90), new Vector3(4.8f, d, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, main, new Vector3(-0.685f, -1.35f, 2.39f), new Vector3(0, 0, 90), new Vector3(4.8f, d, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, main, new Vector3(-1.2f, 1.11f, 2.39f), Vector3.zero, new Vector3(21, d, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, main, new Vector3(-1.2f, 4.31f, 2.39f), Vector3.zero, new Vector3(21, 0.3f, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, steklo, new Vector3(-1.2f, 2.7f, 2.39f), Vector3.zero, new Vector3(21, 3, d2)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, steklo, new Vector3(4.3f, -1.35f, 2.39f), Vector3.zero, new Vector3(9.8f, 4.8f, d2)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, steklo, new Vector3(-8.15f, -1.35f, 2.39f), Vector3.zero, new Vector3(6.8f, 4.8f, d2)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, steklo, new Vector3(9.1f, 0.16f, 2.39f), new Vector3(0, 0, 90), new Vector3(8, d, d)));
                MSR.AddPart(new ModelPrimitive(MSR, PrimitiveType.Cube, steklo, new Vector3(-11.43f, 0.16f, 2.39f), new Vector3(0, 0, 90), new Vector3(8, d, d)));
                ModelsToBreak.Add(MSR);
            }
            #endregion
            #region Management
            {
                var Management = new Model("Management", new Vector3(0, -1.448f, -6.085f), Vector3.zero, MSR);
                SpawnLightMonitor(new Vector3(8.524f, 2.26f, 6.9f), new Vector3(340, 60), new Vector3(2, 1, 0.1f));
                SpawnLightMonitor(new Vector3(9, 2.26f, 4.901f), new Vector3(340, 90), new Vector3(2, 1, 0.1f));
                SpawnLightMonitor(new Vector3(9, 2.26f, 2.77f), new Vector3(340, 90), new Vector3(2, 1, 0.1f));
                SpawnLightMonitor(new Vector3(8.524f, 2.26f, 0.75f), new Vector3(340, 120), new Vector3(2, 1, 0.1f));
                SpawnLightMonitor(new Vector3(8.15f, 3.73f, 3.78f), new Vector3(320, 90), new Vector3(8, 2, 0.1f));

                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Cube, stol, new Vector3(8.205f, -1.27f, 3.78f), new Vector3(90, 90), new Vector3(8, 2, 0.1f)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Cube, stol, new Vector3(6.713f, -1.27f, 6.282f), new Vector3(90, 90), new Vector3(3, 1, 0.1f)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Cube, stol, new Vector3(6.713f, -1.27f, 1.28f), new Vector3(90, 90), new Vector3(3, 1, 0.1f)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Cube, stol, new Vector3(6.656f, -1.977f, 3.801f), new Vector3(90, 90), new Vector3(1, 0.7f, 1)));

                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Quad, ButtonColor(), new Vector3(6.829f, -1.471f, 4.141f), new Vector3(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Quad, ButtonColor(), new Vector3(6.829f, -1.471f, 3.913f), new Vector3(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Quad, ButtonColor(), new Vector3(6.829f, -1.471f, 3.671f), new Vector3(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Quad, ButtonColor(), new Vector3(6.829f, -1.471f, 3.462f), new Vector3(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Quad, ButtonColor(), new Vector3(6.627f, -1.471f, 4.141f), new Vector3(90, 0), new Vector3(0.1f, 0.1f, 1)));
                Management.AddPart(new ModelPrimitive(Management, PrimitiveType.Quad, ButtonColor(), new Vector3(6.627f, -1.471f, 3.462f), new Vector3(90, 0), new Vector3(0.1f, 0.1f, 1)));

                {
                    var obj = new ModelPrimitive(Management, PrimitiveType.Quad, _blue, new Vector3(-1.83f, 1.65f, -1.22f), new Vector3(0, 180), new Vector3(13, 7.5f, 1));
                    Management.AddPart(obj, false);
                    Monitors.Add(obj);
                }

                ModelsToBreak.Add(Management);

                void SpawnLightMonitor(Vector3 _pos, Vector3 _rot, Vector3 _scale)
                {
                    var obj = new ModelPrimitive(Management, PrimitiveType.Cube, GetRandomMonitorColor(), _pos, _rot, _scale);
                    Management.AddPart(obj, false);
                    Monitors.Add(obj);
                }

                SpawnMonitor(new Vector3(8.892f, -0.679f, 3.807f), Vector3.zero);
                SpawnMonitor(new Vector3(8.649f, -0.679f, 2.534f), new Vector3(0, 20));
                SpawnMonitor(new Vector3(8.649f, -0.679f, 5.1f), new Vector3(0, -20));
                SpawnMonitor(new Vector3(7.939f, -0.679f, 6.13f), new Vector3(0, -50));
                SpawnMonitor(new Vector3(7.939f, -0.679f, 1.48f), new Vector3(0, 50));

                void SpawnMonitor(Vector3 _p, Vector3 _r)
                {
                    var Monitor = new Model("Monitor", _p, _r, Management);
                    var _c = new Color32(143, 148, 154, 255);
                    Monitor.AddPart(new ModelPrimitive(Monitor, PrimitiveType.Cube, _c, Vector3.zero, Vector3.zero, new Vector3(0.1f, 0.7f, 1)));
                    Monitor.AddPart(new ModelPrimitive(Monitor, PrimitiveType.Cube, _c, new Vector3(0, -0.446f), Vector3.zero, new Vector3(0.05f, 0.2f, 0.1f)));
                    var obj = new ModelPrimitive(Monitor, PrimitiveType.Quad, GetRandomMonitorColor(), new Vector3(-0.06f, 0), new Vector3(0, 90), new Vector3(0.9f, 0.6f, 1));
                    Monitor.AddPart(obj, false);
                    Monitors.Add(obj);
                }
                {
                    Vector3 _p = new(6.579f, -1.477f, 3.765f);
                    Vector3 _r = new(0, 0, 270);
                    PanelPosition = Management.GameObject.transform.position + _p;
                    var item = Qurre.API.Server.InventoryHost.CreateItemInstance(ItemType.Adrenaline, false);
                    ushort ser = ItemSerialGenerator.GenerateNext();
                    item.PickupDropModel.Info.Serial = ser;
                    item.PickupDropModel.Info.ItemId = ItemType.Adrenaline;
                    item.PickupDropModel.Info.Position = PanelPosition;
                    item.PickupDropModel.Info.Weight = item.Weight;
                    item.PickupDropModel.Info.Rotation = new LowPrecisionQuaternion(Quaternion.Euler(Management.GameObject.transform.rotation.eulerAngles + _r));
                    item.PickupDropModel.NetworkInfo = item.PickupDropModel.Info;
                    ItemPickupBase ipb = Object.Instantiate(item.PickupDropModel);
                    var gameObject = ipb.gameObject;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.transform.parent = Management.GameObject.transform;
                    gameObject.transform.localPosition = _p;
                    gameObject.transform.localRotation = Quaternion.Euler(_r);
                    gameObject.transform.localScale = new Vector3(0.1f, 3, 1.2f);
                    NetworkServer.Spawn(gameObject);
                    ipb.InfoReceived(default, item.PickupDropModel.NetworkInfo);
                    if (!Buttons.ContainsKey(ipb)) Buttons.Add(ipb, this);
                }
            }
            #endregion
            new Item(ItemType.KeycardFacilityManager).Spawn(new Vector3(-177.359f, 989.107f, -98.908f), Quaternion.Euler(new Vector3(0, -146)));
            Color ButtonColor()
            {
                var rand = Random.Range(1, 6);
                if (rand == 1) return _cyan;
                if (rand == 2) return _lime;
                if (rand == 3) return _blue;
                if (rand == 4) return _magenta;
                if (rand == 5) return _yellow;
                return _red;
            }
        }
        internal static Color GetRandomMonitorColor()
        {
            var rand = Random.Range(1, 5);
            if (rand == 1) return _cyan;
            if (rand == 2) return _lime;
            if (rand == 3) return _blue;
            if (rand == 4) return _magenta;
            return _red;
        }

        public Model Model;
        private readonly Model Floor;
        private readonly Model Walls;
        private readonly Model Plintus;

        public Light Light;
    }
}