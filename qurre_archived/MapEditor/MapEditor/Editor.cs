using Interactables.Interobjects.DoorUtils;
using Mirror;
using Qurre;
using Qurre.API;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static MapEditor.MapEditorModels;
namespace MapEditor
{
    public class Editor
    {
        public static Dictionary<string, Map> maps = new Dictionary<string, Map>();
        public static Dictionary<string, PlayerEditorStatus> playerEditors = new Dictionary<string, PlayerEditorStatus>();
        public static void UnloadMap(CommandSender _, string Name)
        {
            var list = maps[Name].objects.ToArray();
            var list1 = list.Where(x => GetPer(x));
            var list2 = list.Where(x => !GetPer(x));
            foreach (var obj in list1) UnityEngine.Object.DestroyImmediate(obj.orginalPrefab, true);
            foreach (var obj in list2)
            {
                if (obj.orginalPrefab == null) continue;
                if (obj.ObjectType == ObjectType.DoorEZ || obj.ObjectType == ObjectType.DoorGate || obj.ObjectType == ObjectType.DoorHCZ || obj.ObjectType == ObjectType.DoorLCZ)
                {
                    var controller = obj.orginalPrefab.GetDoor();
                    if (controller == null) continue;
                    controller.Name = "";
                    controller.Position = Vector3.zero;
                }
                else if (obj.ObjectType == ObjectType.Generator)
                {
                    var controller = obj.orginalPrefab.GetGenerator();
                    if (controller == null) continue;
                    controller.Name = "";
                    controller.Position = Vector3.zero;
                }
                else if (obj.ObjectType == ObjectType.Window)
                {
                    var controller = obj.orginalPrefab.GetWindow();
                    if (controller == null) continue;
                    controller.Name = "";
                    controller.Position = Vector3.zero;
                }
            }
            maps.Remove(Name);
            Log.Info("Map " + Name + " unloaded.");
            bool GetPer(MapObject x) => x.ObjectType != ObjectType.DoorEZ && x.ObjectType != ObjectType.DoorGate && x.ObjectType != ObjectType.Generator &&
            x.ObjectType != ObjectType.DoorHCZ && x.ObjectType != ObjectType.DoorLCZ && x.ObjectType != ObjectType.Window;
        }
        public static void CreateMap(CommandSender sender, string Name)
        {
            string path = Path.Combine(MainClass.pluginDir, Name + ".yml");
            if (File.Exists(path))
            {
                sender.RaReply("MapEditor#File " + Name + ".yml does exist.", true, true, string.Empty);
                return;
            }
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            File.WriteAllText(path, serializer.Serialize(new Map() { MapName = Name, objects = new List<MapObject>() }));
            sender.RaReply("MapEditor#Map " + Name + ".yml created.", true, true, string.Empty);
        }
        public static void DeleteMap(CommandSender sender, string Name)
        {
            string path = Path.Combine(MainClass.pluginDir, Name + ".yml");
            if (!File.Exists(path))
            {
                sender.RaReply("MapEditor#File " + Name + ".yml does not exist.", true, true, string.Empty);
                return;
            }
            if (maps.ContainsKey(Name))
            {
                UnloadMap(sender, Name);
            }
            File.Delete(path);
            sender.RaReply("MapEditor#Map " + Name + ".yml deleted.", true, true, string.Empty);
        }
        public static void LoadMap(CommandSender sender, string Name)
        {
            string path = Path.Combine(MainClass.pluginDir, Name + ".yml");
            if (!File.Exists(path))
            {
                if (sender != null)
                    sender.RaReply("MapEditor#File " + Name + ".yml does not exist.", true, true, string.Empty);
                return;
            }
            if (maps.ContainsKey(Name))
            {
                UnloadMap(sender, Name);
            }
            string yml = File.ReadAllText(path);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var map = deserializer.Deserialize<Map>(yml);
            foreach (MapObject obj in map.objects)
            {
                obj.Load(map);
            }
            maps.Add(Name, map);
            if (sender != null)
                sender.RaReply("MapEditor#Map " + Name + " loaded with " + map.objects.Count + " objects.", true, true, string.Empty);
        }
        public static void SetPositionObject(CommandSender sender, float x, float y, float z)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType == ObjectType.Window)
            {
                var contrl = obj.GetWindow();
                contrl.Position = new Vector3(x, y, z);
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Generator)
            {
                var contrl = obj.GetGenerator();
                contrl.Position = new Vector3(x, y, z);
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Primitive)
            {
                var contrl = obj.GetPrimitive();
                contrl.Position = new Vector3(x, y, z);
                obj2.UpdateData();
            }
            else
            {
                NetworkServer.UnSpawn(obj);
                obj.transform.position = new Vector3(x, y, z);
                obj2.UpdateData();
                NetworkServer.Spawn(obj);
            }
        }
        public static void SetScaleObject(CommandSender sender, float x, float y, float z)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType == ObjectType.Window)
            {
                var contrl = obj.GetWindow();
                contrl.Scale = new Vector3(x, y, z);
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Generator)
            {
                var contrl = obj.GetGenerator();
                contrl.Scale = new Vector3(x, y, z);
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Primitive)
            {
                var contrl = obj.GetPrimitive();
                contrl.Scale = new Vector3(x, y, z);
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Light)
            {
                var contrl = obj.GetLight();
                obj.transform.localScale = new Vector3(x, y, z);
                contrl.Range = y;
                obj2.UpdateData();
            }
            else
            {
                NetworkServer.UnSpawn(obj);
                obj.transform.localScale = new Vector3(x, y, z);
                obj2.UpdateData();
                NetworkServer.Spawn(obj);
            }
        }
        public static void SetRotationObject(CommandSender sender, float x, float y, float z)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType == ObjectType.Window)
            {
                var contrl = obj.GetWindow();
                contrl.Rotation = Quaternion.Euler(new Vector3(x, y, z));
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Generator)
            {
                var contrl = obj.GetGenerator();
                contrl.Rotation = Quaternion.Euler(new Vector3(x, y, z));
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Primitive)
            {
                var contrl = obj.GetPrimitive();
                contrl.Rotation = Quaternion.Euler(new Vector3(x, y, z));
                obj2.UpdateData();
            }
            else
            {
                NetworkServer.UnSpawn(obj);
                obj.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
                obj2.UpdateData();
                NetworkServer.Spawn(obj);
            }
        }
        public static void DeleteObject(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            foreach (var ob in maps[playerEditors[sender.SenderId].mapName].objects)
            {
                if (ob.orginalPrefab == playerEditors[sender.SenderId].selectedObject)
                {
                    NetworkManager.DestroyImmediate(ob.orginalPrefab, true);
                    maps[playerEditors[sender.SenderId].mapName].objects.Remove(ob);
                }
            }
        }
        public static void BringObject(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            var pos = (sender as PlayerCommandSender).ReferenceHub.transform.position;
            if (obj2.ObjectType == ObjectType.Window)
            {
                var contrl = obj.GetWindow();
                contrl.Position = pos;
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Generator)
            {
                var contrl = obj.GetGenerator();
                contrl.Position = pos;
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Primitive)
            {
                var contrl = obj.GetPrimitive();
                contrl.Position = pos;
                obj2.UpdateData();
            }
            else
            {
                NetworkServer.UnSpawn(obj);
                obj.transform.position = pos;
                obj2.UpdateData();
                NetworkServer.Spawn(obj);
            }
        }
        public static void SetColor(CommandSender sender, float c1, float c2, float c3)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType == ObjectType.Primitive)
            {
                var contrl = obj.GetPrimitive();
                var color = new Color(c1 / 255, c2 / 255, c3 / 255, 1f);
                contrl.Color = color;
                obj2.color = color;
                obj2.UpdateData();
            }
            else if (obj2.ObjectType == ObjectType.Light)
            {
                var contrl = obj.GetLight();
                var color = new Color(c1 / 255, c2 / 255, c3 / 255, 1f);
                contrl.Color = color;
                obj2.color = color;
                obj2.UpdateData();
            }
        }
        public static void SetPrimitive(CommandSender sender, PrimitiveType pr)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType == ObjectType.Primitive)
            {
                var contrl = obj.GetPrimitive();
                contrl.Type = pr;
                obj2.Primitive = pr;
                obj2.UpdateData();
            }
        }
        public static void SetLockObject(CommandSender sender, bool locked)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType != ObjectType.WorkStation)
            {
                obj2.orginalPrefab.GetComponent<DoorVariant>().ServerChangeLock(DoorLockReason.AdminCommand, locked);
                obj2.DoorSettings.Locked = locked;
            }
        }
        public static void SetPermsObject(CommandSender sender, ushort perm)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            if (obj2.ObjectType != ObjectType.WorkStation)
            {
                obj2.orginalPrefab.GetComponent<DoorVariant>().RequiredPermissions.RequiredPermissions = (KeycardPermissions)perm;
                obj2.DoorSettings.Permissions = (KeycardPermissions)perm;
            }
        }
        public static void ItemSet(CommandSender sender, string type)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            var obj = playerEditors[sender.SenderId].selectedObject;
            if (obj == null)
                return;
            var obj2 = maps[playerEditors[sender.SenderId].mapName].objects.Where(p => p.orginalPrefab == obj).First();
            obj2.ItemType = (ItemType)Enum.Parse(typeof(ItemType), type);
            obj2.PreLoad();
        }
        public static void CreateObject(CommandSender sender, string type)
        {
            try
            {
                if (!playerEditors.ContainsKey(sender.SenderId))
                    return;
                bool player = false;
                if (sender is PlayerCommandSender)
                    player = true;
                if (!player)
                    return;
                Enum.TryParse<ObjectType>(type, true, out ObjectType res);
                var hub = Player.Get(sender.SenderId);
                playerEditors.TryGetValue(sender.SenderId, out PlayerEditorStatus editor);
                var p = maps[editor.mapName];
                MapObject obj = new MapObject();
                List<int> ints = new List<int>();
                foreach (var xobj in p.objects)
                    ints.Add(xobj.ObjectID);

                var scp049Component = hub.CameraTransform;
                var scp106Component = hub.GameObject.GetComponent<Scp106PlayerScript>();
                Vector3 plyRot = scp049Component.forward;
                Physics.Raycast(scp049Component.position, plyRot, out RaycastHit where, 40f, scp106Component.teleportPlacementMask);
                Vector3 rotation = new Vector3(-plyRot.x, plyRot.y, -plyRot.z), position = Vec3ToVector3(where.point) + (Vector3.up * 0.1f);
                obj.Init(p, Enumerable.Range(1, int.MaxValue).Except(ints).First(), res, position, Vector3.zero, Quaternion.Euler(Vector3.zero));
                p.objects.Add(obj);
                playerEditors[sender.SenderId].selectedObject = obj.orginalPrefab;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public static void CloneObject(CommandSender sender)
        {
            try
            {
                if (!playerEditors.ContainsKey(sender.SenderId))
                    return;
                bool player = false;
                if (sender is PlayerCommandSender)
                    player = true;
                if (!player)
                    return;
                if (playerEditors[sender.SenderId].selectedObject == null)
                    return;
                var hub = Player.Get(sender.SenderId);
                playerEditors.TryGetValue(sender.SenderId, out PlayerEditorStatus editor);
                if (editor.isWorkstation)
                {
                    var p = maps[editor.mapName];
                    var type = p.objects.Where(p2 => p2.orginalPrefab == playerEditors[sender.SenderId].selectedObject).First();
                    MapObject obj = type.Clone();
                    List<int> ints = new List<int>();
                    foreach (var xobj in p.objects)
                        ints.Add(xobj.ObjectID);
                    obj.ObjectID = Enumerable.Range(1, int.MaxValue).Except(ints).First();
                    obj.Load(p);
                    p.objects.Add(obj);
                    playerEditors[sender.SenderId].selectedObject = obj.orginalPrefab;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public static void SelectObject(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
            {
                sender.RaReply("MapEditor#Enable editing mode.", true, true, string.Empty);
                return;
            }
            bool player = false;
            if (sender is PlayerCommandSender)
                player = true;
            if (!player)
                return;
            var hub = Player.Get(sender.SenderId);
            var scp049Component = hub.CameraTransform;
            Vector3 plyRot = scp049Component.transform.forward;
            Physics.Raycast(scp049Component.transform.position, plyRot, out RaycastHit where, 40f);
            if (where.point.Equals(Vector3.zero))
                return;
            else
            {
                if (where.transform.root.gameObject.name.Split('|')[0] == playerEditors[sender.SenderId].mapName)
                {
                    playerEditors[sender.SenderId].isWorkstation = true;
                    playerEditors[sender.SenderId].selectedObject = where.transform.root.gameObject;
                    return;
                }
            }
        }
        public static Vector3 Vec3ToVector3(Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
        public static void StopMapEditing(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
            {
                sender.RaReply("MapEditor#You are not currently editing any maps.", true, true, string.Empty);
                return;
            }
            playerEditors.Remove(sender.SenderId);
            sender.RaReply("MapEditor#Editing map finished", true, true, string.Empty);
        }
        public static void SaveMap(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
            {
                sender.RaReply("MapEditor#Enable editing mode.", true, true, string.Empty);
                return;
            }
            string mapToSave = playerEditors[sender.SenderId].mapName;
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string path = Path.Combine(MainClass.pluginDir, mapToSave + ".yml");
            File.WriteAllText(path, serializer.Serialize(maps[mapToSave]));
            sender.RaReply("MapEditor#Map " + mapToSave + " saved.", true, true, string.Empty);
        }
        public static void EditMap(CommandSender sender, string Name)
        {
            if (!maps.ContainsKey(Name))
            {
                sender.RaReply("MapEditor#Map " + Name + " not loaded.", true, true, string.Empty);
                return;
            }
            if (playerEditors.ContainsKey(sender.SenderId))
            {
                playerEditors.TryGetValue(sender.SenderId, out PlayerEditorStatus editor);
                sender.RaReply("MapEditor#You are currently editing the <color=green>" + editor.mapName + "</color> map, to stop editing type <color=green>mapeditor cancel</color>", true, true, string.Empty);
                return;
            }
            playerEditors.Add(sender.SenderId, new PlayerEditorStatus()
            {
                editingMap = true,
                mapName = Name,
                selectedObject = null
            });
        }
    }
}