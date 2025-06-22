using EXILED;
using EXILED.ApiObjects;
using EXILED.Extensions;
using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MEC;
using Newtonsoft.Json;
using static MapEditor.Editor;
namespace MapEditor
{
    public class MainClass : Plugin
    {
        public static string pluginDir;
        public static MapEditorSettings settings;
        public override void OnEnable()
        {
            Log.Info("Textures loaded.");
            Events.RoundRestartEvent += EventHandlers.RoundRestartEvent;
            Events.PlayerLeaveEvent += EventHandlers.PlayerLeaveEvent;
            Events.RemoteAdminCommandEvent += EventHandlers.RemoteAdminCommandEvent;
            Events.WaitingForPlayersEvent += EventHandlers.WaitingForPlayersEvent;
            Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
            string appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            pluginDir = Path.Combine(appData, "Plugins", "Textures");
            if (!Directory.Exists(pluginDir))
                Directory.CreateDirectory(pluginDir);
            if (!Directory.Exists(Path.Combine(pluginDir, "maps")))
                Directory.CreateDirectory(Path.Combine(pluginDir, "maps"));
            if (!File.Exists(Path.Combine(pluginDir, "settings.yml")))
                File.WriteAllText(Path.Combine(pluginDir, "settings.yml"), JsonConvert.SerializeObject(new MapEditorSettings()
                {
                    MapToLoad = new Dictionary<int, List<string>>()
                    {
                        { 7777, new List<string>(){"gate3"} }
                    }
                }));
            settings = JsonConvert.DeserializeObject<MapEditorSettings>(File.ReadAllText(Path.Combine(pluginDir, "settings.yml")));
            Timing.RunCoroutine(InfinityLoop());

        }

        public IEnumerator<float> InfinityLoop()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1);
                try
                {
                    foreach (KeyValuePair<string, Editor.PlayerEditorStatus> ed in Editor.playerEditors)
                    {
                        ReferenceHub player = Player.GetPlayer(ed.Key);
                        if (player != null)
                        {
                            if (Editor.maps.ContainsKey(ed.Value.mapName))
                            {
                                string line = "";
                                if (ed.Value.selectedObject != null)
                                {
                                    line = string.Concat(
                                        ed.Value.isWorkstation ? "<size=30>SelectedObject ID: " + ed.Value.selectedObject.name.Split('|')[1] + " MAP: " + ed.Value.selectedObject.name.Split('|')[0] + " ROOM: " + Editor.GetObject(player).room + "</size>" : "<size=30>Object Name: " + ed.Value.selectedObject.name + "</size>",
                                        System.Environment.NewLine,
                                        "<size=30>Position</size> <size=30>X: " + ed.Value.selectedObject.transform.position.x + " Y: " + ed.Value.selectedObject.transform.position.y + " Z: " + ed.Value.selectedObject.transform.position.z + "</size> <color=red>|</color> <size=30>Rotation</size> <size=30>X: " + ed.Value.selectedObject.transform.rotation.x + " Y: " + ed.Value.selectedObject.transform.rotation.y + " Z: " + ed.Value.selectedObject.transform.rotation.z + "</size> <color=red>|</color> <size=30>Scale</size> <size=30>X: " + ed.Value.selectedObject.transform.localScale.x + " Y: " + ed.Value.selectedObject.transform.localScale.y + " Z: " + ed.Value.selectedObject.transform.localScale.z + "</size>"
                                   );
                                }
                                player.gameObject.GetComponent<Broadcast>().TargetClearElements(player.characterClassManager.connectionToClient);
                                player.gameObject.GetComponent<Broadcast>().TargetAddElement(player.characterClassManager.connectionToClient, string.Concat(
                                    " <color=red>|</color> MapEditor <color=red>|</color> ",
                                    System.Environment.NewLine,
                                    line
                                ), 2, false);
                            }
                            else
                            {
                                Editor.playerEditors.Remove(player.characterClassManager.UserId);
                                player.gameObject.GetComponent<Broadcast>().TargetClearElements(player.characterClassManager.connectionToClient);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }
        }

        public static GameObject GetWorkStationObject()
        {
            GameObject bench =
                UnityEngine.Object.Instantiate(
                    NetworkManager.singleton.spawnPrefabs.Find(p => p.gameObject.name == "Work Station"));
            return bench;
        }

        public static Vector3 GetRelativePosition(Room room, Vector3 position)
        {
            return Vec3ToVector3(room.Transform.InverseTransformPoint(Vector3To3(position)));
        }

        public static Vector3 GetRelativeRotation(Room room, Vector3 rotation)
        {
            return room.Transform.InverseTransformDirection(rotation);
        }

        public static Vector3 Vector3To3(Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector3 Vec3ToVector3(Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }


        public override string getName { get; }
        public override void OnDisable() { }
        public override void OnReload() { }
    }
}