using EXILED;
using EXILED.ApiObjects;
using EXILED.Extensions;
using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace MapEditor
{
    public class Editor
    {
        public static Dictionary<string, Map> maps = new Dictionary<string, Map>();
        public static Dictionary<string, PlayerEditorStatus> playerEditors = new Dictionary<string, PlayerEditorStatus>();
        public class MapEditorSettings
        {
            public Dictionary<int, List<string>> MapToLoad { get; set; } = new Dictionary<int, List<string>>();
        }
        public class PlayerEditorStatus
        {
            public bool editingMap { get; set; } = false;
            public string mapName { get; set; } = "";
            public bool isWorkstation { get; set; } = true;
            public GameObject selectedObject { get; set; } = null;
        }

        public class MapObject
        {
            public int id { get; set; } = 0;
            public string room { get; set; } = "none";
            public ObjectPosition position { get; set; } = new ObjectPosition();
            public ObjectPosition scale { get; set; } = new ObjectPosition();
            public ObjectPosition rotation { get; set; } = new ObjectPosition();
        }

        public class ObjectPosition
        {
            public float x { get; set; } = 0f;
            public float y { get; set; } = 0f;
            public float z { get; set; } = 0f;
        }

        public class MapObjectLoaded
        {
            public int id { get; set; } = 0;
            public string room { get; set; } = "none";
            public string name { get; set; } = "";
            public Vector3 position { get; set; } = new Vector3();
            public Vector3 scale { get; set; } = new Vector3();
            public Vector3 rotation { get; set; } = new Vector3();
            public GameObject workStation { get; set; } = null;
        }

        public class Map
        {
            public string Name { get; set; } = "";
            public List<MapObjectLoaded> objects { get; set; } = new List<MapObjectLoaded>();
        }

        public class YML
        {
            public List<MapObject> objects { get; set; } = new List<MapObject>();
        }

        public static void UnloadMap(CommandSender sender, string Name)
        {
            //foreach (MapObjectLoaded obj in maps[Name].objects)
               // NetworkManager.DestroyImmediate(obj.workStation, true);
            maps.Remove(Name);
            //Log.Info("Map " + Name + " unloaded.");
        }

        public static void CreateMap(CommandSender sender, string Name)
        {
            string path = Path.Combine(MainClass.pluginDir, "maps", Name + ".yml");
            if (File.Exists(path))
            {
                sender.RaReply("MapEditor#File " + Name + ".yml does exist.", true, true, string.Empty);
                return;
            }
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            File.WriteAllText(path, serializer.Serialize(new YML() { objects = new List<MapObject>() }));
            sender.RaReply("MapEditor#Map " + Name + ".yml created.", true, true, string.Empty);
        }

        public static void DeleteMap(CommandSender sender, string Name)
        {
            string path = Path.Combine(MainClass.pluginDir, "maps", Name + ".yml");
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

        public static void PrepareMap()
        {
            List<string> mapsToLoad = new List<string>();
            foreach (KeyValuePair<string, Map> map in maps)
            {
                mapsToLoad.Add(map.Value.Name);
            }
            maps.Clear();
            foreach (string Name in mapsToLoad)
            {
                string path = Path.Combine(MainClass.pluginDir, "maps", Name + ".yml");
                if (!File.Exists(path))
                    continue;
                string yml = File.ReadAllText(path);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                List<MapObject> objects = deserializer.Deserialize<YML>(yml).objects;
                Map map = new Map();
                map.Name = Name;
                foreach (MapObject obj in objects)
                {
                    MapObjectLoaded objLoaded = new MapObjectLoaded();
                    objLoaded.workStation = MainClass.GetWorkStationObject();
                    List<int> ints = new List<int>();
                    foreach (MapObjectLoaded x in map.objects)
                    {
                        ints.Add(x.id);
                    }
                    objLoaded.id = Enumerable.Range(1, int.MaxValue).Except(ints).First();
                    objLoaded.name = Name;
                    objLoaded.room = obj.room;
                    objLoaded.workStation.name = Name + "|" + objLoaded.id;
                    Offset offset = new Offset();
                    if (obj.room != "none")
                    {
                        foreach (EXILED.ApiObjects.Room room in EXILED.Extensions.Map.Rooms)
                        {
                            if (room.Name == obj.room)
                            {
                                var transf = room.Transform;
                                Vector3 position = new Vector3(obj.position.x, obj.position.y, obj.position.z);
                                Vector3 rotation = new Vector3(obj.rotation.x, obj.rotation.y, obj.rotation.z);
                                objLoaded.position = transf.TransformPoint(position);
                                Quaternion epicDirection = new Quaternion(transf.rotation.x + rotation.x, transf.rotation.y + rotation.y, transf.rotation.z + rotation.z, transf.rotation.z + rotation.z);
                                objLoaded.rotation = epicDirection.eulerAngles;
                                objLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(epicDirection.eulerAngles);
                            }
                        }
                    }
                    else
                    {
                        objLoaded.position = new Vector3(obj.position.x, obj.position.y, obj.position.z);
                        objLoaded.rotation = new Vector3(obj.rotation.x, obj.rotation.y, obj.rotation.z);
                        objLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(objLoaded.rotation);
                    }
                    objLoaded.scale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
                    offset.position = objLoaded.position;
                    offset.rotation = objLoaded.rotation;
                    offset.scale = Vector3.one;
                    objLoaded.workStation.gameObject.transform.localScale = objLoaded.scale;
                    objLoaded.workStation.AddComponent<WorkStationUpgrader>();
                    NetworkServer.Spawn(objLoaded.workStation);
                    objLoaded.workStation.GetComponent<WorkStation>().Networkposition = offset;
                    map.objects.Add(objLoaded);
                }
                maps.Add(Name, map);
            }
        }

        public static Vector3 GetOffset(Vector3 euler, float forward, float right)
        {
            switch (euler.y)
            {
                default:
                    return new Vector3(-forward, 0, right);
                case 90:
                    return new Vector3(right, 0, forward);
                case 180:
                    return new Vector3(forward, 0, right);
                case 270:
                    return new Vector3(-right, 0, forward);
            }
        }

        public static void LoadMap(CommandSender sender, string Name)
        {
            try
            {
                string path = Path.Combine(MainClass.pluginDir, "maps", Name + ".yml");
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
                List<MapObject> objects = deserializer.Deserialize<YML>(yml).objects;
                Map map = new Map();
                map.Name = Name;
                foreach (MapObject obj in objects)
                {
                    MapObjectLoaded objLoaded = new MapObjectLoaded();
                    objLoaded.workStation = MainClass.GetWorkStationObject();
                    List<int> ints = new List<int>();
                    foreach (MapObjectLoaded x in map.objects)
                        ints.Add(x.id);
                    objLoaded.id = Enumerable.Range(1, int.MaxValue).Except(ints).First();
                    objLoaded.name = Name;
                    objLoaded.room = obj.room;
                    objLoaded.workStation.name = Name + "|" + objLoaded.id;
                    Offset offset = new Offset();
                    if (obj.room != "none")
                    {
                        foreach (EXILED.ApiObjects.Room room in EXILED.Extensions.Map.Rooms)
                        {
                            if (room.Name == obj.room)
                            {
                                var transf = room.Transform;
                                Vector3 position = new Vector3(obj.position.x, obj.position.y, obj.position.z);
                                Vector3 rotation = new Vector3(obj.rotation.x, obj.rotation.y, obj.rotation.z);
                                Vector3 relativeDirection = transf.TransformDirection(rotation);
                                Quaternion relativeRotation = Quaternion.Euler(relativeDirection);
                                objLoaded.position = transf.TransformPoint(position);
                                Quaternion epicDirection = new Quaternion(transf.rotation.x + rotation.x, transf.rotation.y + rotation.y, transf.rotation.z + rotation.z, transf.rotation.z + rotation.z);
                                objLoaded.rotation = epicDirection.eulerAngles;
                                objLoaded.workStation.gameObject.transform.rotation = relativeRotation;
                            }
                        }
                    }
                    else
                    {
                        objLoaded.position = new Vector3(obj.position.x, obj.position.y, obj.position.z);
                        objLoaded.rotation = new Vector3(obj.rotation.x, obj.rotation.y, obj.rotation.z);
                        objLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(objLoaded.rotation);
                    }
                    objLoaded.scale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
                    offset.position = objLoaded.position;
                    offset.rotation = objLoaded.rotation;
                    offset.scale = Vector3.one;
                    objLoaded.workStation.gameObject.transform.localScale = objLoaded.scale;
                    objLoaded.workStation.AddComponent<WorkStationUpgrader>();
                    NetworkServer.Spawn(objLoaded.workStation);
                    objLoaded.workStation.GetComponent<WorkStation>().Networkposition = offset;
                    map.objects.Add(objLoaded);
                }
                maps.Add(Name, map);
                if (sender != null)
                    sender.RaReply("MapEditor#Map " + Name + " loaded with " + map.objects.Count + " objects.", true, true, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static MapObjectLoaded GetObject(ReferenceHub hub)
        {
            foreach (MapObjectLoaded ob in maps[playerEditors[hub.GetUserId()].mapName].objects)
                if (ob.workStation == playerEditors[hub.GetUserId()].selectedObject)
                    return ob;
            return null;
        }

        public static void SetPositionObject(CommandSender sender, float x, float y, float z)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            if (playerEditors[sender.SenderId].selectedObject == null)
                return;
            if (playerEditors[sender.SenderId].isWorkstation)
            {
                foreach (MapObjectLoaded ob in maps[playerEditors[sender.SenderId].mapName].objects)
                {
                    if (ob.workStation == playerEditors[sender.SenderId].selectedObject)
                    {
                        Offset old = ob.workStation.GetComponent<WorkStation>().Networkposition;
                        NetworkServer.UnSpawn(ob.workStation);
                        ob.workStation.transform.rotation = Quaternion.Euler(ob.rotation);
                        old.position = new Vector3(x, y, z);
                        ob.position = old.position;
                        NetworkServer.Spawn(ob.workStation);
                        ob.workStation.GetComponent<WorkStation>().Networkposition = old;
                    }
                }
            }
            else
            {
                NetworkServer.UnSpawn(playerEditors[sender.SenderId].selectedObject);
                playerEditors[sender.SenderId].selectedObject.transform.position = new Vector3(x, y, z);
                NetworkServer.Spawn(playerEditors[sender.SenderId].selectedObject);
            }
        }

        public static Room GetRoomFromObject(GameObject obj)
        {
            Vector3 objPos = obj.transform.position;
            Vector3 end = objPos - new Vector3(0f, 10f, 0f);
            bool flag = Physics.Linecast(objPos, end, out RaycastHit raycastHit, -84058629);

            if (!flag || raycastHit.transform == null)
                return null;

            Transform transform = raycastHit.transform;

            while (transform.parent != null && transform.parent.parent != null)
                transform = transform.parent;

            foreach (Room room in EXILED.Extensions.Map.Rooms)
                if (room.Position == transform.position)
                    return room;

            return new Room
            {
                Name = transform.name,
                Position = transform.position,
                Transform = transform
            };
        }

        public static void SetRotationObject(CommandSender sender, float x, float y, float z)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            if (playerEditors[sender.SenderId].selectedObject == null)
                return;
            if (playerEditors[sender.SenderId].isWorkstation)
            {
                foreach (MapObjectLoaded ob in maps[playerEditors[sender.SenderId].mapName].objects)
                {
                    if (ob.workStation == playerEditors[sender.SenderId].selectedObject)
                    {
                        Offset old = ob.workStation.GetComponent<WorkStation>().Networkposition;
                        NetworkServer.UnSpawn(ob.workStation);
                        old.rotation = new Vector3(x, y, z);
                        ob.workStation.transform.rotation = Quaternion.Euler(x, y, z);
                        ob.rotation = new Vector3(x, y, z);
                        old.rotation = ob.rotation;
                        NetworkServer.Spawn(ob.workStation);
                        ob.workStation.GetComponent<WorkStation>().Networkposition = new Offset()
                        {
                            position = old.position,
                            rotation = old.rotation,
                            scale = old.scale
                        };
                    }
                }
            }
            else
            {
                NetworkServer.UnSpawn(playerEditors[sender.SenderId].selectedObject);
                playerEditors[sender.SenderId].selectedObject.transform.rotation = Quaternion.Euler(x, y, z);
                NetworkServer.Spawn(playerEditors[sender.SenderId].selectedObject);
            }
        }

        public static void DeleteObject(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            if (playerEditors[sender.SenderId].selectedObject == null)
                return;
            if (playerEditors[sender.SenderId].isWorkstation)
            {
                foreach (MapObjectLoaded ob in maps[playerEditors[sender.SenderId].mapName].objects)
                {
                    if (ob.workStation == playerEditors[sender.SenderId].selectedObject)
                    {
                        NetworkManager.DestroyImmediate(ob.workStation, true);
                        maps[playerEditors[sender.SenderId].mapName].objects.Remove(ob);
                    }
                }
            }
            else
                NetworkManager.DestroyImmediate(playerEditors[sender.SenderId].selectedObject, true);
        }

        public static void SetScaleObject(CommandSender sender, float x, float y, float z)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            if (playerEditors[sender.SenderId].selectedObject == null)
                return;
            if (playerEditors[sender.SenderId].isWorkstation)
            {
                foreach (MapObjectLoaded ob in maps[playerEditors[sender.SenderId].mapName].objects)
                {
                    if (ob.workStation == playerEditors[sender.SenderId].selectedObject)
                    {
                        NetworkServer.UnSpawn(ob.workStation);
                        ob.workStation.transform.localScale = new Vector3(x, y, z);
                        ob.scale = new Vector3(x, y, z);
                        NetworkServer.Spawn(ob.workStation);
                    }
                }
            }
            else
            {
                NetworkServer.UnSpawn(playerEditors[sender.SenderId].selectedObject);
                playerEditors[sender.SenderId].selectedObject.transform.localScale = new Vector3(x, y, z);
                NetworkServer.Spawn(playerEditors[sender.SenderId].selectedObject);
            }
        }

        public static void CreateObject(CommandSender sender)
        {
            try
            {
                if (!playerEditors.ContainsKey(sender.SenderId))
                    return;
                ReferenceHub hub = Player.GetPlayer(sender.SenderId);
                playerEditors.TryGetValue(sender.SenderId, out PlayerEditorStatus editor);
                Map p = maps[editor.mapName];
                MapObjectLoaded objLoaded = new MapObjectLoaded();
                List<int> ints = new List<int>();
                foreach (MapObjectLoaded xobj in p.objects)
                    ints.Add(xobj.id);
                objLoaded.id = Enumerable.Range(1, int.MaxValue).Except(ints).First();
                objLoaded.workStation = MainClass.GetWorkStationObject();
                objLoaded.name = editor.mapName;
                objLoaded.workStation.name = editor.mapName + "|" + objLoaded.id;
                Offset offset = new Offset();
                var scp049Component = hub.gameObject.GetComponent<Scp049_2PlayerScript>();
                var scp106Component = hub.gameObject.GetComponent<Scp106PlayerScript>();
                Vector3 plyRot = scp049Component.plyCam.transform.forward;
                Physics.Raycast(scp049Component.plyCam.transform.position, plyRot, out RaycastHit where, 40f, scp106Component.teleportPlacementMask);
                Vector3 rotation = new Vector3(-plyRot.x, plyRot.y, -plyRot.z), position = Vec3ToVector3(where.point) + (Vector3.up * 0.1f);
                objLoaded.position = position;
                objLoaded.rotation = rotation;
                objLoaded.scale = objLoaded.workStation.transform.localScale;
                offset.position = position;
                offset.rotation = rotation;
                offset.scale = objLoaded.workStation.transform.localScale;
                objLoaded.workStation.AddComponent<WorkStationUpgrader>();
                NetworkServer.Spawn(objLoaded.workStation);
                objLoaded.workStation.GetComponent<WorkStation>().Networkposition = offset;
                p.objects.Add(objLoaded);
                playerEditors[sender.SenderId].selectedObject = objLoaded.workStation;
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
                if (playerEditors[sender.SenderId].selectedObject == null)
                    return;
                ReferenceHub hub = Player.GetPlayer(sender.SenderId);
                playerEditors.TryGetValue(sender.SenderId, out PlayerEditorStatus editor);
                if (editor.isWorkstation)
                {
                    Map p = maps[editor.mapName];
                    string room = "none";
                    foreach (MapObjectLoaded i in p.objects)
                        if (playerEditors[sender.SenderId].selectedObject == i.workStation)
                            room = i.room;
                    MapObjectLoaded objLoaded = new MapObjectLoaded();
                    List<int> ints = new List<int>();
                    foreach (MapObjectLoaded xobj in p.objects)
                        ints.Add(xobj.id);
                    objLoaded.id = Enumerable.Range(1, int.MaxValue).Except(ints).First();
                    objLoaded.workStation = MainClass.GetWorkStationObject();
                    objLoaded.name = editor.mapName;
                    objLoaded.room = room;
                    objLoaded.workStation.name = editor.mapName + "|" + objLoaded.id;
                    Offset offset = new Offset();
                    Vector3 position = playerEditors[sender.SenderId].selectedObject.transform.position;
                    Vector3 rotation = playerEditors[sender.SenderId].selectedObject.transform.rotation.eulerAngles;
                    objLoaded.position = position;
                    objLoaded.rotation = rotation;
                    objLoaded.scale = playerEditors[sender.SenderId].selectedObject.transform.localScale;
                    offset.position = position;
                    offset.rotation = rotation;
                    offset.scale = playerEditors[sender.SenderId].selectedObject.transform.localScale;
                    objLoaded.workStation.transform.localScale = playerEditors[sender.SenderId].selectedObject.transform.localScale;
                    NetworkServer.Spawn(objLoaded.workStation);
                    objLoaded.workStation.GetComponent<WorkStation>().Networkposition = offset;
                    objLoaded.workStation.AddComponent<WorkStationUpgrader>();
                    p.objects.Add(objLoaded);
                    playerEditors[sender.SenderId].selectedObject = objLoaded.workStation;
                }
                else
                {
                    GameObject obj = UnityEngine.Object.Instantiate(editor.selectedObject);
                    obj.transform.localScale = editor.selectedObject.transform.localScale;
                    obj.transform.rotation = editor.selectedObject.transform.rotation;
                    obj.transform.position = editor.selectedObject.transform.position;
                    obj.name = obj.name + UnityEngine.Random.Range(0, 99999);
                    NetworkServer.Spawn(obj);
                    playerEditors[sender.SenderId].isWorkstation = false;
                    playerEditors[sender.SenderId].selectedObject = obj;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void SetRoomObject(CommandSender sender, string room)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
                return;
            if (playerEditors[sender.SenderId].selectedObject == null)
                return;
            ReferenceHub hub = Player.GetPlayer(sender.SenderId);
            playerEditors.TryGetValue(sender.SenderId, out PlayerEditorStatus editor);
            if (editor.isWorkstation)
            {
                Map p = maps[editor.mapName];
                foreach (MapObjectLoaded i in p.objects)
                {
                    if (playerEditors[sender.SenderId].selectedObject == i.workStation)
                    {
                        if (room.ToLower() == "current")
                        {
                            i.room = hub.GetCurrentRoom().Name;
                            sender.RaReply("MapEditor#Room set to " + i.room, true, true, string.Empty);
                        }
                        else if (room.ToLower() == "currentobject")
                        {
                            i.room = GetRoomFromObject(i.workStation).Name;
                            sender.RaReply("MapEditor#Room set to " + i.room, true, true, string.Empty);
                        }
                        else
                        {
                            if (room.ToLower() == "none")
                            {
                                i.room = "none";
                            }
                            else
                            {
                                foreach (EXILED.ApiObjects.Room roo in EXILED.Extensions.Map.Rooms)
                                {
                                    if (roo.Name.ToUpper() == room.ToUpper())
                                    {
                                        i.room = roo.Name;
                                        room = "set";
                                        sender.RaReply("MapEditor#Room set to " + i.room, true, true, string.Empty);
                                    }
                                }
                                if (room != "set")
                                    sender.RaReply("MapEditor#Unknown room.", true, true, string.Empty);
                            }
                        }
                    }
                }
            }
        }

        public static void SelectObject(CommandSender sender)
        {
            if (!playerEditors.ContainsKey(sender.SenderId))
            {
                sender.RaReply("MapEditor#Enable editing mode.", true, true, string.Empty);
                return;
            }
            ReferenceHub hub = Player.GetPlayer(sender.SenderId);
            var scp049Component = hub.gameObject.GetComponent<Scp049_2PlayerScript>();
            Vector3 plyRot = scp049Component.plyCam.transform.forward;
            Physics.Raycast(scp049Component.plyCam.transform.position, plyRot, out RaycastHit where, 40f);
            if (where.point.Equals(Vector3.zero))
                return;
            else
            {
                if (where.transform.gameObject.name.Split('|')[0] == playerEditors[sender.SenderId].mapName)
                {
                    playerEditors[sender.SenderId].isWorkstation = true;
                    playerEditors[sender.SenderId].selectedObject = where.transform.gameObject;
                    return;
                }
                if (where.transform.parent.gameObject != null)
                {
                    if (where.transform.parent.gameObject.name.Contains("|"))
                    {
                        if (where.transform.parent.gameObject.name.Split('|')[0] == playerEditors[sender.SenderId].mapName)
                        {
                            playerEditors[sender.SenderId].isWorkstation = true;
                            playerEditors[sender.SenderId].selectedObject = where.transform.parent.gameObject;
                            return;
                        }
                    }
                }
                if (where.transform.parent.transform.parent != null)
                {
                    if (where.transform.parent.transform.parent.gameObject.name.Contains("|"))
                    {
                        if (where.transform.parent.transform.parent.gameObject.name.Split('|')[0] == playerEditors[sender.SenderId].mapName)
                        {
                            playerEditors[sender.SenderId].isWorkstation = true;
                            playerEditors[sender.SenderId].selectedObject = where.transform.parent.transform.parent.gameObject;
                            return;
                        }
                    }
                }
                if (where.transform.gameObject.GetComponent<NetworkIdentity>() != null)
                {
                    playerEditors[sender.SenderId].isWorkstation = false;
                    playerEditors[sender.SenderId].selectedObject = where.transform.gameObject;
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
            List<MapObject> objects = new List<MapObject>();
            foreach (MapObjectLoaded obj in maps[mapToSave].objects)
            {
                ObjectPosition position = new ObjectPosition()
                {
                    x = obj.position.x,
                    y = obj.position.y,
                    z = obj.position.z
                };
                ObjectPosition rotation = new ObjectPosition()
                {
                    x = obj.rotation.x,
                    y = obj.rotation.y,
                    z = obj.rotation.z
                };
                ObjectPosition scale = new ObjectPosition()
                {
                    x = obj.scale.x,
                    y = obj.scale.y,
                    z = obj.scale.z
                };
                if (obj.room != "none")
                {
                    foreach (EXILED.ApiObjects.Room room in EXILED.Extensions.Map.Rooms)
                    {
                        if (room.Name == obj.room)
                        {
                            Vector3 newpos = MainClass.GetRelativePosition(room, obj.workStation.transform.position);
                            Vector3 newrot = MainClass.GetRelativeRotation(room, obj.workStation.transform.rotation.eulerAngles);
                            rotation = new ObjectPosition()
                            {
                                x = newrot.x,
                                y = newrot.y,
                                z = newrot.z
                            };
                            position = new ObjectPosition()
                            {
                                x = newpos.x,
                                y = newpos.y,
                                z = newpos.z
                            };
                        }
                    }
                }
                objects.Add(new MapObject()
                {
                    id = obj.id,
                    room = obj.room,
                    position = position,
                    rotation = rotation,
                    scale = scale
                });
            }
            string path = Path.Combine(MainClass.pluginDir, "maps", mapToSave + ".yml");
            File.WriteAllText(path, serializer.Serialize(new YML()
            {
                objects = objects
            }));
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