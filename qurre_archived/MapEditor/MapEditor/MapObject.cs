using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;
using MapGeneration;
using Mirror;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using static MapEditor.MapEditorModels;
using Light = Qurre.API.Controllers.Light;

namespace MapEditor
{
    public class MapObject
    {
        public int ObjectID { get; set; } = 0;
        public RoomName Room { get; set; } = RoomName.Unnamed;
        public ObjectType ObjectType { get; set; } = ObjectType.WorkStation;
        public DoorSettings DoorSettings { get; set; } = null;
        public ItemType ItemType { get; set; } = ItemType.KeycardJanitor;
        public PrimitiveType Primitive { get; set; } = PrimitiveType.Sphere;
        [YamlIgnore]
        public Color color
        {
            get => new Color(color1 / 255, color2 / 255, color3 / 255);
            set
            {
                color1 = value.r * 255;
                color2 = value.g * 255;
                color3 = value.b * 255;
            }
        }
        public float color1 = 255;
        public float color2 = 255;
        public float color3 = 255;
        public ObjectPosition position { get; set; } = new ObjectPosition();
        public ObjectPosition scale { get; set; } = new ObjectPosition();
        public ObjectPosition rotation { get; set; } = new ObjectPosition();
        [YamlIgnore]
        public GameObject orginalPrefab;

        public void Init(Map map, int id, ObjectType type, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            ObjectID = id;
            ObjectType = type;
            if (type == ObjectType)
                DoorSettings = new DoorSettings();
            this.position.SetVector(position);
            this.scale.SetVector(scale);
            this.rotation.SetVector(rotation.eulerAngles);
            Load(map);
        }
        public MapObject Clone()
        {
            return new MapObject()
            {
                ObjectType = this.ObjectType,
                position = this.position,
                scale = this.scale,
                rotation = this.rotation
            };
        }
        public void UpdateData()
        {
            var room = GetRoom();
            if (room != null)
            {
                this.position.SetVector(room.Transform.InverseTransformPoint(orginalPrefab.transform.position));
                this.rotation.SetVector(room.Transform.InverseTransformDirection(orginalPrefab.transform.eulerAngles));
                this.Room = room.RoomName;
            }
            else
            {
                this.position.SetVector(orginalPrefab.transform.position);
                this.rotation.SetVector(orginalPrefab.transform.eulerAngles);
                this.Room = RoomName.Unnamed;
            }
            this.scale.SetVector(orginalPrefab.transform.localScale);
        }
        public Room GetRoom()
        {
            return null;
            Vector3 end = orginalPrefab.transform.position - new Vector3(0f, 10f, 0f);
            bool flag = Physics.Linecast(orginalPrefab.transform.position, end, out RaycastHit raycastHit, -84058629);

            if (!flag || raycastHit.transform == null)
                return null;

            Transform latestParent = raycastHit.transform;
            while (latestParent.parent?.parent != null)
                latestParent = latestParent.parent;

            foreach (Room room in Qurre.API.Map.Rooms)
            {
                if (room.Transform == latestParent)
                    return room;
            }
            return null;
        }
        public void Load(Map map)
        {
            if (orginalPrefab != null) NetworkServer.Destroy(orginalPrefab);
            if (this.Room != RoomName.Unnamed)
            {
                var t = Qurre.API.Map.Rooms.Where(p => p.RoomName == this.Room).First().Transform;
                Load(t.transform.TransformPoint(position.Vector), t.transform.TransformDirection(rotation.Vector), scale.Vector);
            }
            else Load(position.Vector, rotation.Vector, scale.Vector);
            void Load(Vector3 pos, Vector3 rot, Vector3 scale)
            {
                if (scale == Vector3.zero) scale = Vector3.one;
                if (ObjectType == ObjectType.WorkStation)
                {
                    var __ = WorkStation.Create(pos, rot, scale);
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.DoorLCZ)
                {
                    var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") && x.Type == DoorType.GR18_Inner);
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomDoor_MapEditor";
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.DoorHCZ)
                {
                    var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") && x.Type == DoorType.PrisonDoor);
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomDoor_MapEditor";
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.DoorEZ)
                {
                    var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") && (x.Type == DoorType.Escape_Secondary || x.Type == DoorType.Escape_Primary));
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomDoor_MapEditor";
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.DoorGate)
                {
                    var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") &&
                    (x.Type == DoorType.GR18 || x.Type == DoorType.Surface_Gate));
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomDoor_MapEditor";
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.Window)
                {
                    var ___ = Qurre.API.Map.Windows.Where(x => !x.Name.Contains("MapEditor") && 
                    !x.GameObject.name.Contains("Window") && !x.GameObject.name.Contains("VTGLASS")).OrderBy(x => x.GameObject.name);
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomWindow_MapEditor";
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.Item)
                {
                    var __ = new Item(ItemType).Spawn(pos, Quaternion.Euler(rot));
                    __.Locked = true;
                    __.Base.Rb.isKinematic = false;
                    __.Base.Rb.useGravity = false;
                    __.Base.Rb.velocity = Vector3.zero;
                    __.Base.Rb.angularVelocity = Vector3.zero;
                    __.Base.Rb.freezeRotation = true;
                    __.Base.Rb.mass = 100000;
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    orginalPrefab = __.Base.gameObject;
                }
                else if (ObjectType == ObjectType.Generator)
                {
                    var ___ = Qurre.API.Map.Generators.Where(x => !x.Name.Contains("MapEditor")).OrderBy(x => x.GameObject.name);
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomGenerator_MapEditor";
                    orginalPrefab = __.GameObject;
                }
                else if (ObjectType == ObjectType.Primitive)
                {
                    var __ = new Primitive(Primitive, pos, color, Quaternion.Euler(rot), scale);
                    orginalPrefab = __.Base.gameObject;
                }
                else if (ObjectType == ObjectType.Light)
                {
                    var __ = new Light(pos, color, 1, scale.y)
                    {
                        Rotation = Quaternion.Euler(rot)
                    };
                    orginalPrefab = __.Base.gameObject;
                }
                else if (ObjectType == ObjectType.Invisible)
                {
                    var ___ = Qurre.API.Map.Windows.Where(x => !x.Name.Contains("MapEditor") &&
                    (x.GameObject.name.Contains("Window") || x.GameObject.name.Contains("VTGLASS"))).OrderBy(x => x.GameObject.name);
                    if (___.Count() == 0) return;
                    var __ = ___.First();
                    __.Position = pos;
                    __.Rotation = Quaternion.Euler(rot);
                    __.Scale = scale;
                    __.Name = "CustomWindow_MapEditor";
                    orginalPrefab = __.GameObject;
                }
            }
            orginalPrefab.name = $"{map.MapName}|{ObjectID}";
            UpdateData();
        }
        public void PreLoad()
        {
            if (orginalPrefab == null) return;
            var pos = orginalPrefab.transform.position;
            var rot = orginalPrefab.transform.rotation;
            var sle = orginalPrefab.transform.localScale;
            NetworkServer.Destroy(orginalPrefab);
            if (ObjectType == ObjectType.WorkStation)
            {
                var __ = WorkStation.Create(pos, rot.eulerAngles, sle);
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.DoorLCZ)
            {
                var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") && x.Type == DoorType.GR18_Inner);
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomDoor_MapEditor";
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.DoorHCZ)
            {
                var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") && x.Type == DoorType.PrisonDoor);
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomDoor_MapEditor";
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.DoorEZ)
            {
                var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") && (x.Type == DoorType.Escape_Secondary || x.Type == DoorType.Escape_Primary));
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomDoor_MapEditor";
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.DoorGate)
            {
                var ___ = Qurre.API.Map.Doors.Where(x => !x.Name.Contains("MapEditor") &&
                (x.Type == DoorType.GR18 || x.Type == DoorType.Surface_Gate));
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomDoor_MapEditor";
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.Window)
            {
                var ___ = Qurre.API.Map.Windows.Where(x => !x.Name.Contains("MapEditor") &&
                !x.GameObject.name.Contains("Window") && !x.GameObject.name.Contains("VTGLASS")).OrderBy(x => x.GameObject.name);
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomWindow_MapEditor";
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.Item)
            {
                var __ = new Item(ItemType).Spawn(pos, rot);
                __.Locked = true;
                __.Base.Rb.isKinematic = false;
                __.Base.Rb.useGravity = false;
                __.Base.Rb.velocity = Vector3.zero;
                __.Base.Rb.angularVelocity = Vector3.zero;
                __.Base.Rb.freezeRotation = true;
                __.Base.Rb.mass = 100000;
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                orginalPrefab = __.Base.gameObject;
            }
            else if (ObjectType == ObjectType.Generator)
            {
                var ___ = Qurre.API.Map.Generators.Where(x => !x.Name.Contains("MapEditor")).OrderBy(x => x.GameObject.name);
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomGenerator_MapEditor";
                orginalPrefab = __.GameObject;
            }
            else if (ObjectType == ObjectType.Primitive)
            {
                var __ = new Primitive(Primitive, pos, color, rot, sle);
                orginalPrefab = __.Base.gameObject;
            }
            else if (ObjectType == ObjectType.Light)
            {
                var __ = new Light(pos, color, 1, sle.y)
                {
                    Rotation = rot,
                };
                orginalPrefab = __.Base.gameObject;
            }
            else if (ObjectType == ObjectType.Invisible)
            {
                var ___ = Qurre.API.Map.Windows.Where(x => !x.Name.Contains("MapEditor") &&
                (x.GameObject.name.Contains("Window") || x.GameObject.name.Contains("VTGLASS"))).OrderBy(x => x.GameObject.name);
                if (___.Count() == 0) return;
                var __ = ___.First();
                __.Position = pos;
                __.Rotation = rot;
                __.Scale = sle;
                __.Name = "CustomWindow_MapEditor";
                orginalPrefab = __.GameObject;
            }
        }
    }
    public class DoorSettings
    {
        public bool Locked { get; set; } = false;
        public KeycardPermissions Permissions { get; set; } = KeycardPermissions.None;
    }
}