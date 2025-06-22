using MEC;
using Qurre;
using Qurre.API;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static MapEditor.Editor;
using static MapEditor.MapEditorModels;
using Version = System.Version;
namespace MapEditor
{
    public class MainClass : Plugin
    {
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "MapEditor";
        public override Version Version { get; } = new Version(2, 0, 0);
        public override Version NeededQurreVersion { get; } = new Version(1, 9, 0);
        public override int Priority { get; } = 1000;
        public static string pluginDir;
        public override void Enable()
        {
            Qurre.Events.Round.Waiting += Server_WaitingForPlayers;
            Qurre.Events.Round.Restart += EventHandlers.RoundRestartEvent;
            Qurre.Events.Player.Leave += EventHandlers.PlayerLeaveEvent;
            Qurre.Events.Server.SendingRA += EventHandlers.RemoteAdminCommandEvent;
            pluginDir = Path.Combine(PluginManager.PluginsDirectory, "MapEditor");
            if (!Directory.Exists(pluginDir)) Directory.CreateDirectory(pluginDir);
            Timing.RunCoroutine(InfinityLoop());
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= Server_WaitingForPlayers;
            Qurre.Events.Round.Restart -= EventHandlers.RoundRestartEvent;
            Qurre.Events.Player.Leave -= EventHandlers.PlayerLeaveEvent;
            Qurre.Events.Server.SendingRA -= EventHandlers.RemoteAdminCommandEvent;
        }
        public static bool loaded = false;
        private void Server_WaitingForPlayers()
        {
            foreach (var file in Directory.GetFiles(pluginDir))
            {
                string yml = File.ReadAllText(file);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                var map = deserializer.Deserialize<Map>(yml);
                if (map.LoadOnStart.Contains(Server.Port))
                {
                    foreach (MapObject obj in map.objects) obj.Load(map);
                    maps.Add(Name, map);
                }
            }
        }
        public IEnumerator<float> InfinityLoop()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.5f);
                try
                {
                    foreach (KeyValuePair<string, PlayerEditorStatus> ed in playerEditors)
                    {
                        var player = Player.Get(ed.Key);
                        if (player != null)
                        {
                            if (maps.ContainsKey(ed.Value.mapName))
                            {
                                string line = "";
                                if (ed.Value.selectedObject != null)
                                {
                                    line = string.Concat(
                                        ed.Value.isWorkstation ? "<size=30>SelectedObject ID: " + ed.Value.selectedObject.name.Split('|')[1] + " MAP: " + ed.Value.selectedObject.name.Split('|')[0] + "</size>" : "<size=30>Object Name: " + ed.Value.selectedObject.name + "</size>",
                                        Environment.NewLine,
                                        "<size=30>Position</size> <size=30>X: " + (int)ed.Value.selectedObject.transform.position.x + " Y: " + (int)ed.Value.selectedObject.transform.position.y + " Z: " + (int)ed.Value.selectedObject.transform.position.z + "</size>",
                                        Environment.NewLine,
                                        "<size=30>Rotation</size> <size=30>X: " + (int)ed.Value.selectedObject.transform.rotation.x + " Y: " + (int)ed.Value.selectedObject.transform.rotation.y + " Z: " + (int)ed.Value.selectedObject.transform.rotation.z + "</size>",
                                        Environment.NewLine,
                                        "<size=30>Scale</size> <size=30>X: " + (int)ed.Value.selectedObject.transform.localScale.x + " Y: " + (int)ed.Value.selectedObject.transform.localScale.y + " Z: " + (int)ed.Value.selectedObject.transform.localScale.z + "</size>"
                                   );
                                }
                                player.ShowHint($"\n\n <color=red>|</color> MapEditor <color=red>|</color> \n{line}");
                            }
                            else
                            {
                                Editor.playerEditors.Remove(player.UserId);
                                player.ClearBroadcasts();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}