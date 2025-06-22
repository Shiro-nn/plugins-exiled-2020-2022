using MEC;
using Mirror;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ClansWars.Objects
{
    internal class Lift
    {
        private static readonly Color _red = new(3, 0, 0);
        private static readonly Color _cyan = new(0, 4, 4);
        private static readonly Color _yellow = new(4, 4, 0);
        private static readonly Color _lime = new(0, 4, 0);
        private const string LiftDoorName = "CustomLift";
        internal static List<Lift> List = new();
        internal int Speed = 5;
        private bool Using = false;
        private bool InFirst = true;
        internal readonly LiftObjects Objects;
        private readonly List<DoSpawnDoor> DSD = new();
        internal Lift(LiftCreator first, LiftCreator second, Color color)
        {
            List<LiftDoor> _doors = new();
            var _first = Create(first, true);
            var _second = Create(second, false);
            Objects = new(first, second, _first.Positions, _second.Positions, _doors, _first.Sensor, _second.Sensor, _first.CustomDoor, _second.CustomDoor);
            List.Add(this);
            LocalCreator Create(LiftCreator cre, bool first)
            {
                Model Model = new("Lift", cre.Position, cre.Rotation);
                NetworkServer.UnSpawn(Model.GameObject);
                Model.GameObject.transform.parent = cre.Parent;
                Model.GameObject.transform.localPosition = cre.Position;
                Model.GameObject.transform.localRotation = Quaternion.Euler(cre.Rotation);
                NetworkServer.Spawn(Model.GameObject);
                if (Round.Waiting) DSD.Add(new(Model.GameObject.transform.position + Vector3.down * 2.46f, Model.GameObject.transform.rotation, first));
                else
                {
                    Door door = Door.Spawn(Model.GameObject.transform.position + Vector3.down * 2.46f, DoorPrefabs.DoorHCZ, Model.GameObject.transform.rotation);
                    door.Name = LiftDoorName;
                    if (first) door.Open = true;
                    _doors.Add(new(door, first));
                }
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(-1.87f, 0), Vector3.zero, new(1.8f, 4.9f, 0.4f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(1.87f, 0), Vector3.zero, new(1.8f, 4.9f, 0.4f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(-0.0169f, 1.63f), Vector3.zero, new(2, 1.64f, 0.4f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(-2.72f, 0, -2.995f), Vector3.zero, new(0.1f, 4.9f, 5.6f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(2.72f, 0, -2.995f), Vector3.zero, new(0.1f, 4.9f, 5.6f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(0, 0, -5.745f), new(0, 90), new(0.1f, 4.9f, 5.5f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(0, -2.485f, -2.75f), new(0, 0, 90), new(0.1f, 5.5f, 5.9f)));
                Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, color, new(0, 2.4f, -2.94f), new(0, 0, 90), new(0.1f, 5.5f, 5.6f)));
                Model.AddPart(new ModelLight(Model, new Color32(120, 120, 120, 255), new(0, 1.655f, -2.427f), lightRange: 7));
                ModelPrimitive sensor = new(Model, PrimitiveType.Quad, _lime, new(-1.25f, 0.3f, 0.206f), new(0, 180), new(0.2f, 0.4f, 1));
                Model.AddPart(sensor, false);
                try { Timing.CallDelayed(1f, () => Model.Primitives.ForEach(prim => { try { prim.Primitive.Break(); } catch { } })); } catch { }
                if (cre.NoShowDoor)
                {
                    Model CustomDoor = new("CustomHCZDoor", Vector3.zero, Vector3.zero, Model);
                    CreateButton(new(-1.242f, -0.713f, 0.210f), Vector3.zero);
                    CreateButton(new(1.242f, -0.713f, -0.210f), new(0, 180));
                    void CreateButton(Vector3 pos, Vector3 rot)
                    {
                        Model Button = new("Button", pos, rot, CustomDoor);
                        Button.AddPart(new ModelPrimitive(Button, PrimitiveType.Cube, new Color32(60, 60, 60, 255), Vector3.zero, Vector3.zero, new(0.25f, 0.25f, 0.1f)));
                        Button.AddPart(new ModelPrimitive(Button, PrimitiveType.Cylinder, Color.red, new(0, 0, 0.05f), new(90, 0), new(0.15f, 0.01f, 0.15f)));
                    }
                    Model DoorLeft = new("DoorLeft", new(first ? -1.5f : -0.5f, -0.815f), Vector3.zero, CustomDoor);
                    DoorLeft.AddPart(new ModelPrimitive(DoorLeft, PrimitiveType.Cube, new Color32(48, 48, 48, 255), Vector3.zero, Vector3.zero, new(1, 3.25f, 0.2f)));
                    DoorLeft.AddPart(new ModelPrimitive(DoorLeft, PrimitiveType.Quad, _red, new(0.17f, 0, -0.101f), Vector3.zero, new(0.3f, 1.5f, 0.22f)));
                    DoorLeft.AddPart(new ModelPrimitive(DoorLeft, PrimitiveType.Quad, _red, new(0.348f, 0.65f, -0.101f), Vector3.zero, new(0.3f, 0.2f, 0.22f)));
                    DoorLeft.AddPart(new ModelPrimitive(DoorLeft, PrimitiveType.Quad, _red, new(0.348f, 0.075f, -0.101f), Vector3.zero, new(0.3f, 0.2f, 0.22f)));
                    DoorLeft.AddPart(new ModelPrimitive(DoorLeft, PrimitiveType.Quad, _red, new(0.302f, 0.65f, 0.101f), new(0, 180), new(0.4f, 0.2f, 0.22f)));
                    DoorLeft.AddPart(new ModelPrimitive(DoorLeft, PrimitiveType.Quad, _red, new(0.302f, 0.075f, 0.101f), new(0, 180), new(0.4f, 0.2f, 0.22f)));

                    Model DoorRight = new("DoorRight", new(first ? 1.5f : 0.5f, -0.815f), Vector3.zero, CustomDoor);
                    DoorRight.AddPart(new ModelPrimitive(DoorRight, PrimitiveType.Cube, new Color32(48, 48, 48, 255), Vector3.zero, Vector3.zero, new(1, 3.25f, 0.2f)));
                    DoorRight.AddPart(new ModelPrimitive(DoorRight, PrimitiveType.Quad, _red, new(-0.302f, 0.65f, -0.101f), Vector3.zero, new(0.4f, 0.2f, 0.22f)));
                    DoorRight.AddPart(new ModelPrimitive(DoorRight, PrimitiveType.Quad, _red, new(-0.302f, 0.075f, -0.101f), Vector3.zero, new(0.4f, 0.2f, 0.22f)));
                    DoorRight.AddPart(new ModelPrimitive(DoorRight, PrimitiveType.Quad, _red, new(-0.348f, 0.65f, 0.101f), new(0, 180), new(0.3f, 0.2f, 0.22f)));
                    DoorRight.AddPart(new ModelPrimitive(DoorRight, PrimitiveType.Quad, _red, new(-0.348f, 0.075f, 0.101f), new(0, 180), new(0.3f, 0.2f, 0.22f)));
                    DoorRight.AddPart(new ModelPrimitive(DoorRight, PrimitiveType.Quad, _red, new(-0.17f, 0, 0.101f), new(0, 180), new(0.3f, 1.5f, 0.22f)));
                    return new(sensor, new(Model.GameObject.transform), new(DoorLeft.GameObject.transform, DoorRight.GameObject.transform));
                }
                return new(sensor, new(Model.GameObject.transform), null);
            }
        }
        internal static void SpawnDoors()
        {
            Timing.CallDelayed(5f, () =>
            {
                foreach (Lift lift in List)
                {
                    List<LiftDoor> _doors = new();
                    foreach (var dsd in lift.DSD)
                    {
                        Door door = Door.Spawn(dsd.Position, DoorPrefabs.DoorHCZ, dsd.Rotation);
                        door.Name = LiftDoorName;
                        if (dsd.First) door.Open = true;
                        _doors.Add(new(door, dsd.First));
                    }
                    lift.Objects.Doors.AddRange(_doors);
                    lift.DSD.Clear();
                }
            });
        }
        internal static void DoorEvents(DoorDamageEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not LiftDoorName) return;
            ev.Allowed = false;
        }
        internal static void DoorEvents(DoorLockEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not LiftDoorName) return;
            ev.Allowed = false;
        }
        internal static void DoorEvents(DoorOpenEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not LiftDoorName) return;
            ev.Allowed = false;
        }
        internal static void DoorEvents(Scp079InteractDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not LiftDoorName) return;
            ev.Allowed = false;
        }
        internal static void DoorEvents(Scp079LockDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not LiftDoorName) return;
            ev.Allowed = false;
        }
        internal static void DoorEvents(InteractDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not LiftDoorName) return;
            if (!List.TryFind(out Lift lift, x => x.Objects.Doors.Any(x => x.Door == ev.Door))) return;
            ev.Allowed = false;
            lift.Interact();
        }
        internal void Interact()
        {
            if (Round.Waiting) return;
            if (Using) return;
            if (InFirst) Timing.RunCoroutine(ToSecond(), "CustomLiftRunning");
            else Timing.RunCoroutine(ToFirst(), "CustomLiftRunning");
            IEnumerator<float> ToFirst()
            {
                Using = true;
                try { Objects.FirstPanel.Primitive.Color = _yellow; } catch { }
                try { Objects.SecondPanel.Primitive.Color = _yellow; } catch { }
                var _low = Objects.SecondPositions.Low;
                var _big = Objects.SecondPositions.Big;
                Vector3 LowPos = new(Mathf.Min(_low.x, _big.x), Mathf.Min(_low.y, _big.y), Mathf.Min(_low.z, _big.z));
                Vector3 BigPos = new(Mathf.Max(_low.x, _big.x), Mathf.Max(_low.y, _big.y), Mathf.Max(_low.z, _big.z));
                if (Objects.SecondCustomDoors is not null)
                {
                    for (float i = 0; 10 >= i; i++)
                    {
                        try { Objects.SecondCustomDoors.Left.localPosition = new Vector3(-1.5f + 0.1f * i, -0.815f); } catch { }
                        try { Objects.SecondCustomDoors.Right.localPosition = new Vector3(1.5f - 0.1f * i, -0.815f); } catch { }
                        yield return Timing.WaitForSeconds(0.05f);
                    }
                    try { Objects.SecondCustomDoors.Left.localPosition = new Vector3(-0.5f, -0.815f); } catch { }
                    try { Objects.SecondCustomDoors.Right.localPosition = new Vector3(0.5f, -0.815f); } catch { }
                }
                if (Objects.FirstCustomDoors is not null)
                {
                    try { Objects.FirstCustomDoors.Left.localPosition = new Vector3(-0.5f, -0.815f); } catch { }
                    try { Objects.FirstCustomDoors.Right.localPosition = new Vector3(0.5f, -0.815f); } catch { }
                }
                try { foreach (var door in Objects.Doors) door.Door.Open = false; } catch { }
                float time = Speed / 2;
                yield return Timing.WaitForSeconds(time);
                foreach (var pl in Player.List.Where(x => InLift(x.Position)))
                    try
                    {
                        pl.Movement.OverridePosition(pl.Position + Objects.FirstPositions.Center - Objects.SecondPositions.Center,
                      new(Objects.FirstPositions.Rotation.x - Objects.SecondPositions.Rotation.x,
                      Objects.FirstPositions.Rotation.y - Objects.SecondPositions.Rotation.y));
                    }
                    catch { }
                foreach (var pl in Map.Pickups.Where(x => InLift(x.Position)))
                    try { pl.Position += Objects.FirstPositions.Center - Objects.SecondPositions.Center; } catch { }
                yield return Timing.WaitForSeconds(time);
                try { foreach (var door in Objects.Doors) if (door.ThisFirst) door.Door.Open = true; } catch { }
                if (Objects.FirstCustomDoors is not null)
                {
                    for (float i = 0; 10 >= i; i++)
                    {
                        try { Objects.FirstCustomDoors.Left.localPosition = new Vector3(-0.5f - 0.1f * i, -0.815f); } catch { }
                        try { Objects.FirstCustomDoors.Right.localPosition = new Vector3(0.5f + 0.1f * i, -0.815f); } catch { }
                        yield return Timing.WaitForSeconds(0.05f);
                    }
                    try { Objects.FirstCustomDoors.Left.localPosition = new Vector3(-1.5f, -0.815f); } catch { }
                    try { Objects.FirstCustomDoors.Right.localPosition = new Vector3(1.5f, -0.815f); } catch { }
                }
                try { Objects.FirstPanel.Primitive.Color = _lime; } catch { }
                try { Objects.SecondPanel.Primitive.Color = _cyan; } catch { }
                InFirst = true;
                yield return Timing.WaitForSeconds(1.5f);
                Using = false;
                bool InLift(Vector3 pos) => pos.x > LowPos.x && BigPos.x > pos.x && pos.y > LowPos.y &&
                    BigPos.y > pos.y && pos.z > LowPos.z && BigPos.z > pos.z;
            }
            IEnumerator<float> ToSecond()
            {
                Using = true;
                try { Objects.FirstPanel.Primitive.Color = _yellow; } catch { }
                try { Objects.SecondPanel.Primitive.Color = _yellow; } catch { }
                var _low = Objects.FirstPositions.Low;
                var _big = Objects.FirstPositions.Big;
                Vector3 LowPos = new(Mathf.Min(_low.x, _big.x), Mathf.Min(_low.y, _big.y), Mathf.Min(_low.z, _big.z));
                Vector3 BigPos = new(Mathf.Max(_low.x, _big.x), Mathf.Max(_low.y, _big.y), Mathf.Max(_low.z, _big.z));
                if (Objects.FirstCustomDoors is not null)
                {
                    for (float i = 0; 10 >= i; i++)
                    {
                        try { Objects.FirstCustomDoors.Left.localPosition = new Vector3(-1.5f + 0.1f * i, -0.815f); } catch { }
                        try { Objects.FirstCustomDoors.Right.localPosition = new Vector3(1.5f - 0.1f * i, -0.815f); } catch { }
                        yield return Timing.WaitForSeconds(0.05f);
                    }
                    try { Objects.FirstCustomDoors.Left.localPosition = new Vector3(-0.5f, -0.815f); } catch { }
                    try { Objects.FirstCustomDoors.Right.localPosition = new Vector3(0.5f, -0.815f); } catch { }
                }
                if (Objects.SecondCustomDoors is not null)
                {
                    try { Objects.SecondCustomDoors.Left.localPosition = new Vector3(-0.5f, -0.815f); } catch { }
                    try { Objects.SecondCustomDoors.Right.localPosition = new Vector3(0.5f, -0.815f); } catch { }
                }
                try { foreach (var door in Objects.Doors) door.Door.Open = false; } catch { }
                float time = Speed / 2;
                yield return Timing.WaitForSeconds(time);
                foreach (var pl in Player.List.Where(x => InLift(x.Position)))
                    try
                    {
                        pl.Movement.OverridePosition(pl.Position + Objects.SecondPositions.Center - Objects.FirstPositions.Center,
                      new(Objects.SecondPositions.Rotation.x - Objects.FirstPositions.Rotation.x,
                      Objects.SecondPositions.Rotation.y - Objects.FirstPositions.Rotation.y));
                    }
                    catch { }
                foreach (var pl in Map.Pickups.Where(x => InLift(x.Position)))
                    try { pl.Position += Objects.SecondPositions.Center - Objects.FirstPositions.Center; } catch { }
                yield return Timing.WaitForSeconds(time);
                try { foreach (var door in Objects.Doors) if (!door.ThisFirst) door.Door.Open = true; } catch { }
                if (Objects.SecondCustomDoors is not null)
                {
                    for (float i = 0; 10 >= i; i++)
                    {
                        try { Objects.SecondCustomDoors.Left.localPosition = new Vector3(-0.5f - 0.1f * i, -0.815f); } catch { }
                        try { Objects.SecondCustomDoors.Right.localPosition = new Vector3(0.5f + 0.1f * i, -0.815f); } catch { }
                        yield return Timing.WaitForSeconds(0.05f);
                    }
                    try { Objects.SecondCustomDoors.Left.localPosition = new Vector3(-1.5f, -0.815f); } catch { }
                    try { Objects.SecondCustomDoors.Right.localPosition = new Vector3(1.5f, -0.815f); } catch { }
                }
                try { Objects.SecondPanel.Primitive.Color = _lime; } catch { }
                try { Objects.FirstPanel.Primitive.Color = _cyan; } catch { }
                InFirst = false;
                yield return Timing.WaitForSeconds(1.5f);
                Using = false;
                bool InLift(Vector3 pos) => pos.x > LowPos.x && BigPos.x > pos.x && pos.y > LowPos.y &&
                    BigPos.y > pos.y && pos.z > LowPos.z && BigPos.z > pos.z;
            }
        }
        private class DoSpawnDoor
        {
            internal readonly Vector3 Position;
            internal readonly Quaternion Rotation;
            internal readonly bool First;
            internal DoSpawnDoor(Vector3 position, Quaternion rotation, bool first)
            {
                Position = position;
                Rotation = rotation;
                First = first;
            }
        }
        internal class LiftDoor
        {
            internal readonly Door Door;
            internal readonly bool ThisFirst;
            internal LiftDoor(Door door, bool thisFirst)
            {
                Door = door;
                ThisFirst = thisFirst;
            }
        }
        internal class CustomDoor
        {
            internal readonly Transform Left;
            internal readonly Transform Right;
            internal CustomDoor(Transform left, Transform right)
            {
                Left = left;
                Right = right;
            }
        }
        private class LocalCreator
        {
            internal readonly ModelPrimitive Sensor;
            internal readonly InLiftPositions Positions;
            internal readonly CustomDoor CustomDoor;
            internal LocalCreator(ModelPrimitive sensor, InLiftPositions positions, CustomDoor customDoor)
            {
                Sensor = sensor;
                Positions = positions;
                CustomDoor = customDoor;
            }
        }
        internal class InLiftPositions
        {
            internal readonly Vector3 Big;
            internal readonly Vector3 Low;
            internal readonly Vector3 Center;
            internal readonly Vector3 Rotation;
            internal InLiftPositions(Transform transform)
            {
                Rotation = transform.rotation.eulerAngles;
                {
                    GameObject obj = new();
                    var tr = obj.transform;
                    tr.parent = transform;
                    tr.localPosition = new(2.675f, 2.344f, -0.174f);
                    Big = tr.position;
                }
                {
                    GameObject obj = new();
                    var tr = obj.transform;
                    tr.parent = transform;
                    tr.localPosition = new(-2.668f, -2.433f, -5.696f);
                    Low = tr.position;
                }
                {
                    GameObject obj = new();
                    var tr = obj.transform;
                    tr.parent = transform;
                    tr.localPosition = new(0.007f, -0.089f, -2.935f);
                    Center = tr.position;
                }
            }
        }
        internal class LiftCreator
        {
            internal readonly Transform Parent;
            internal readonly Vector3 Position;
            internal readonly Vector3 Rotation;
            internal readonly bool NoShowDoor;
            internal LiftCreator(Transform parent, Vector3 position, Vector3 rotation, bool noShowDoor = false)
            {
                Parent = parent;
                Position = position;
                Rotation = rotation;
                NoShowDoor = noShowDoor;
            }
        }
        internal class LiftObjects
        {
            internal readonly LiftCreator FirstCreator;
            internal readonly LiftCreator SecondCreator;
            internal readonly InLiftPositions FirstPositions;
            internal readonly InLiftPositions SecondPositions;
            internal readonly List<LiftDoor> Doors;
            internal readonly ModelPrimitive FirstPanel;
            internal readonly ModelPrimitive SecondPanel;
            internal readonly CustomDoor FirstCustomDoors;
            internal readonly CustomDoor SecondCustomDoors;
            public LiftObjects(LiftCreator firstCreator, LiftCreator secondCreator, InLiftPositions firstPositions,
                InLiftPositions secondPositions, List<LiftDoor> doors, ModelPrimitive firstPanel, ModelPrimitive secondPanel,
                CustomDoor firstCustomDoors, CustomDoor secondCustomDoors)
            {
                FirstCreator = firstCreator;
                SecondCreator = secondCreator;
                FirstPositions = firstPositions;
                SecondPositions = secondPositions;
                Doors = doors;
                FirstPanel = firstPanel;
                SecondPanel = secondPanel;
                FirstCustomDoors = firstCustomDoors;
                SecondCustomDoors = secondCustomDoors;
            }
        }
    }
}
