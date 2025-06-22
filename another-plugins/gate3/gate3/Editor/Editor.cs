using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using gate3;

namespace MapEditor
{
    public class Editor
    {
        public static Dictionary<string, Map> maps = new Dictionary<string, Map>();
        public class MapEditorSettings
        {
            public Dictionary<int, List<string>> MapToLoad { get; set; } = new Dictionary<int, List<string>>();
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
            foreach (MapObjectLoaded obj in maps[Name].objects)
               NetworkManager.DestroyImmediate(obj.workStation, true);
            maps.Remove(Name);
        }

        public static void LoadMap(CommandSender sender, string Name)
        {
            try
            {
                string path = Path.Combine(MainClass.pluginDir, "maps", Name + ".yml");
                if (!File.Exists(path)) return;
                if (maps.ContainsKey(Name)) UnloadMap(sender, Name);
                string yml = File.ReadAllText(path);
                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
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
                    objLoaded.workStation.name = "Work Station";
                    Offset offset = new Offset();
                    objLoaded.position = new Vector3(obj.position.x, obj.position.y, obj.position.z);
                    objLoaded.rotation = new Vector3(obj.rotation.x, obj.rotation.y, obj.rotation.z);
                    objLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(objLoaded.rotation);
                    objLoaded.scale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
                    offset.position = objLoaded.position;
                    offset.rotation = objLoaded.rotation;
                    offset.scale = Vector3.one;
                    objLoaded.workStation.gameObject.transform.localScale = objLoaded.scale;
                    objLoaded.workStation.AddComponent<WorkStationUpgrader>();
                    NetworkServer.Spawn(objLoaded.workStation);
                    objLoaded.workStation.GetComponent<WorkStation>().Networkposition = offset;
                    objLoaded.workStation.GetComponent<WorkStation>().NetworkisTabletConnected = true;
                    objLoaded.workStation.GetComponent<WorkStation>().Network_playerConnected = objLoaded.workStation;
                    map.objects.Add(objLoaded);
                }
                maps.Add(Name, map);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}