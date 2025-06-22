namespace gate3.Patches
{
    using System;
    using HarmonyLib;
    [HarmonyPatch(typeof(ServerConsole), "ReloadServerName")]
    internal class ServerName
    {
        public static void Postfix()
        {
            bool del = false;
            string[] spearator = { "<color=#00000000>" };
            string[] strlist = ServerConsole._serverName.Split(spearator, 2,
               System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strlist)
            {
                if (del)
                {
                    ServerConsole._serverName = ServerConsole._serverName.Replace(s, "").Replace("<color=#00000000>", "");
                }
                del = true;
            }
            ServerConsole._serverName += $"<color=#00000000><size=1>Qurre 1.0.6</size></color>";
            ServerConsole.AddLog("Server name Updated", System.ConsoleColor.Gray);
        }
    }
}
namespace gate3
{
    using System;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using MEC;
    using Exiled.Events.EventArgs;
    using Hints;
    using static Door;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Mail;
    using HarmonyLib;

    public class Plugin : Plugin<Config>
    {
        public System.Random Gen = new System.Random();
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        private Harmony hInstance;
        public override void OnEnabled()
        {
            base.OnEnabled();

            RegisterEvents();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            UnregisterEvents();
        }
        public void RegisterEvents()
        {
            this.hInstance = new Harmony("fydne.mongodb");
            this.hInstance.PatchAll();
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.yes;
            string ownerip = ServerConsole.Ip;
            var url = "http://checkip.dyndns.org";
            var req = System.Net.WebRequest.Create(url);
            var resp = req.GetResponse();
            using (var sr = new System.IO.StreamReader(resp.GetResponseStream()))
            {
                var response = sr.ReadToEnd().Trim();
                var a = response.Split(':');
                var a2 = a[1].Substring(1);
                var a3 = a2.Split('<');
                var a4 = a3[0];
                ownerip = a4;
            }
            string ip = "194.87.99.156";
            if (ownerip == ip)
            {
                Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
                Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.OnRoundRestart;


                Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
                Exiled.Events.Handlers.Player.Spawning += EventHandlers.OnPlayerSpawn;
                Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.RunOnDoorOpen;
                Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnPlayerHurt;
            }
            else
            {
                Log.Info($"IP не сервера лучника, плагин не запущен\nТвой iP: {ownerip}\niP лучника: {ip}");
            }
        }
        private void UnregisterEvents()
        {
            this.hInstance.UnpatchAll(null);
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.yes;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.OnRoundRestart;


            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnPlayerHurt;
        }

    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
    public static class Extensions
    {
        public static void SetPosition(this ReferenceHub player, Vector3 position) => player.SetPosition(position.x, position.y, position.z);
        public static void SetPosition(this ReferenceHub player, float x, float y, float z) => player.playerMovementSync.OverridePosition(new Vector3(x, y, z), player.transform.rotation.eulerAngles.y);
        public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
        public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
        internal static Stats LoadStats(string userId)
        {
            return new Stats()
            {
                UserId = userId,
                shelter = 3,
                elevator = 3,
            };
        }
        public static ReferenceHub GetPlayer(int playerId)
        {
            if (IdHubs.ContainsKey(playerId))
                return IdHubs[playerId];

            foreach (ReferenceHub hub in GetHubs())
            {
                if (hub.GetPlayerId() == playerId)
                {
                    IdHubs.Add(playerId, hub);

                    return hub;
                }
            }

            return null;
        }
        public static Dictionary<int, ReferenceHub> IdHubs = new Dictionary<int, ReferenceHub>();
        public static int GetPlayerId(this ReferenceHub player) => player.queryProcessor.PlayerId;
    }
    [Serializable]
    public class Stats
    {
        public string UserId;
        public int shelter;
        public int elevator;
    }
    partial class EventHandlers
    {
        public Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        public static List<CoroutineHandle> shc = new List<CoroutineHandle>();
        public static bool roundstart = false;
        public static int roundtimeint = 0;
        public static Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
        public static bool tp = false;
        public static Door d;
        public static Door sdoor;
        public static float pnum;
        public static float pnum2;
        public static float pnum3;
        public static float pnum4;
        public static float pnum5;
        public static float pnum6;
        public static float snum;
        public static float snum2;
        public static float snum3;
        public static float snum4;
        public static float snum5;
        public static float snum6;
        internal static bool newspawn = false;
        internal static int newspawnint = 0;
        public static List<int> shPlayers = new List<int>();
        internal static int spawnshtime = 15;
        internal static bool first = true;
        public static void yes()
        {
            try
            {
                bool del = false;
                string[] spearator = { "<color=#00000000>" };
                string[] strlist = ServerConsole._serverName.Split(spearator, 2,
                System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in strlist)
                {
                    if (del)
                    {
                        ServerConsole._serverName = ServerConsole._serverName.Replace(s, "").Replace("<color=#00000000>", "");
                    }
                    del = true;
                }
                ServerConsole._serverName += $"<color=#00000000><size=1>Qurre 1.0.5</size></color>";
            }
            catch { }
        }
        public static void OnWaitingForPlayers()
        {
            spawnshtime = 15;
            newspawn = false;
            Coroutines.Add(Timing.RunCoroutine(roundtime()));
            Coroutines.Add(Timing.RunCoroutine(checkdoor1()));
            Coroutines.Add(Timing.RunCoroutine(checkdoor2()));
            Coroutines.Add(Timing.RunCoroutine(checkshelter()));
            Coroutines.Add(Timing.RunCoroutine(checkelevator()));
            MapEditor.Editor.LoadMap(null, "gate3static");
        }
        public static void OnRoundStart()
        {
            shPlayers.Clear();
            foreach (GameObject work in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (work.gameObject.name == "Work Station")
                {
                    work.GetComponent<WorkStation>().NetworkisTabletConnected = true;
                    work.GetComponent<WorkStation>().Network_playerConnected = work;
                }
            }
            newspawn = false;
            checkshelterdoor2();
            tp = false;
            roundstart = true;
            List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
            foreach (ReferenceHub player in playerssl)
            {
                foreach (Collider c in Physics.OverlapSphere(player.transform.position, 3000000f))
                {
                    if (c.gameObject.name == "Nodoor")
                    {
                        Log.Info(c.transform.position);
                        Vector3 randomSP = c.transform.position;
                        pnum = randomSP.x + 4f;
                        pnum2 = randomSP.y + 4f;
                        pnum3 = randomSP.z + 4f;
                        pnum4 = randomSP.x - 4f;
                        pnum5 = randomSP.y - 4f;
                        pnum6 = randomSP.z - 4f;
                        Vector3 shel = c.transform.position;
                        snum = shel.x + 10f;
                        snum2 = shel.y + 10f;
                        snum3 = shel.z + 10f;
                        snum4 = shel.x - 10f;
                        snum5 = shel.y - 10f;
                        snum6 = shel.z - 10f;
                        float num = shel.x + 10f;
                        float num2 = shel.y + 10f;
                        float num3 = shel.z + 10f;
                        float num4 = shel.x - 10f;
                        float num5 = shel.y - 10f;
                        float num6 = shel.z - 10f;
                        Vector3 door1 = new Vector3(-84, 1000, -73);
                        Vector3 door2 = new Vector3(-78, 980, -80);
                        foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
                        {
                            if (door.transform.position.x <= num && door.transform.position.x >= num4 && door.transform.position.y <= num2 && door.transform.position.y >= num5 && door.transform.position.z <= num3 && door.transform.position.z >= num6)
                            {
                                sdoor = door;
                            }
                        }
                    }
                }
            }
        }

        public static void OnRoundEnd(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(Coroutines);
            Coroutines.Clear();
            roundstart = false;
        }

        public static void OnRoundRestart()
        {
            Timing.KillCoroutines(Coroutines);
            Coroutines.Clear();
            roundstart = false;
        }
        public static IEnumerator<float> roundtime()
        {
            for (; ; )
            {
                if (roundstart)
                {
                    roundtimeint++;
                }
                else if (!roundstart)
                {
                    roundtimeint = 0;
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public static void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
                return;
            if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
                Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Extensions.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));

        }
        public static void OnPlayerSpawn(SpawningEventArgs ev)
        {
            if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
                return;

            if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
                Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Extensions.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));
        }
        public static void RunOnDoorOpen(InteractingDoorEventArgs ev)
        {
            Door door = ev.Door;
            ReferenceHub player = ev.Player.ReferenceHub;
            if (door.transform.position.x <= snum && door.transform.position.x >= snum4 && door.transform.position.y <= snum2 && door.transform.position.y >= snum5 && door.transform.position.z <= snum3 && door.transform.position.z >= snum6)
            {
                ev.IsAllowed = false;
                var playerIntentory = ev.Player.ReferenceHub.inventory.items;
                foreach (var item in playerIntentory)
                {
                    var gameItem = GameObject.FindObjectOfType<Inventory>().availableItems.FirstOrDefault(i => i.id == item.id);

                    if (gameItem == null)
                        continue;

                    if (gameItem.permissions == null || gameItem.permissions.Length == 0)
                        continue;

                    foreach (var itemPerm in gameItem.permissions)
                    {
                        if (itemPerm == "EXIT_ACC")
                        {
                            ev.IsAllowed = true;
                            continue;
                        }
                    }
                }
            }
        }
        public static void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (ev.DamageType == DamageTypes.Wall)
            {
                Vector3 door1 = new Vector3(-105f, 980f, -67f);
                Vector3 door2 = new Vector3(18, 995f, -51f);
                if (ev.Target.ReferenceHub.transform.position.x <= door2.x && ev.Target.ReferenceHub.transform.position.x >= door1.x && ev.Target.ReferenceHub.transform.position.y <= door2.y && ev.Target.ReferenceHub.transform.position.y >= door1.y && ev.Target.ReferenceHub.transform.position.z <= door2.z && ev.Target.ReferenceHub.transform.position.z >= door1.z)
                {
                    ev.Amount = 0f;
                }
            }
        }
        public static void checkshelterdoor2()
        {
            List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
            foreach (ReferenceHub player in playerssl)
            {
                foreach (Collider c in Physics.OverlapSphere(player.transform.position, 3000000f))
                {
                    if (c.gameObject.name == "Nodoor")
                    {
                        Vector3 shel = c.transform.position;
                        float num = shel.x + 10f;
                        float num2 = shel.y + 10f;
                        float num3 = shel.z + 10f;
                        float num4 = shel.x - 10f;
                        float num5 = shel.y - 10f;
                        float num6 = shel.z - 10f;
                        foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
                        {
                            if (door.transform.position.x <= num && door.transform.position.x >= num4 && door.transform.position.y <= num2 && door.transform.position.y >= num5 && door.transform.position.z <= num3 && door.transform.position.z >= num6)
                            {
                                d = door;
                                door.GrenadesResistant = true;
                                door.permissionLevel = "EXIT_ACC";
                                door.doorType = DoorTypes.Checkpoint;
                                door.buttonType = ButtonTypes.Checkpoint;
                            }
                        }
                    }
                }
            }
        }
        public static IEnumerator<float> checkshelter()
        {
            for (; ; )
            {
                List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
                foreach (ReferenceHub player in playerssl)
                {
                    Vector3 elevator = new Vector3(-81, 992, -68);
                    if (player.transform.position.x <= pnum && player.transform.position.x >= pnum4)
                    {
                        if (player.transform.position.y <= pnum2 && player.transform.position.y >= pnum5)
                        {
                            if (player.transform.position.z <= pnum3 && player.transform.position.z >= pnum6)
                            {
                                if (Stats[player.characterClassManager.UserId].elevator == 3 || Stats[player.characterClassManager.UserId].elevator == 2)
                                {
                                    player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].elevator} секунды", new HintParameter[]
                                    {
new StringHintParameter("")
                                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
                                    Stats[player.characterClassManager.UserId].elevator--;
                                }
                                else if (Stats[player.characterClassManager.UserId].elevator == 1)
                                {
                                    player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].elevator} секунду", new HintParameter[]
                                    {
new StringHintParameter("")
                                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
                                    Stats[player.characterClassManager.UserId].elevator--;
                                }
                                else if (Stats[player.characterClassManager.UserId].elevator == 0)
                                {
                                    player.playerMovementSync.OverridePosition(elevator, 0f);
                                    MapEditor.Editor.LoadMap(null, "gate3");
                                    Stats[player.characterClassManager.UserId].elevator = 3;
                                }
                                else
                                {
                                    Stats[player.characterClassManager.UserId].elevator = 3;
                                }
                            }
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public static IEnumerator<float> checkelevator()
        {
            for (; ; )
            {
                List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
                foreach (ReferenceHub player in playerssl)
                {
                    Vector3 door1 = new Vector3(-84, 1000, -73);
                    Vector3 door2 = new Vector3(-78, 980, -80);
                    if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x)
                    {
                        if (player.transform.position.y <= door1.y && player.transform.position.y >= door2.y)
                        {
                            if (player.transform.position.z <= door1.z && player.transform.position.z >= door2.z)
                            {
                                if (Stats[player.characterClassManager.UserId].shelter == 3 || Stats[player.characterClassManager.UserId].shelter == 2)
                                {
                                    player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].shelter} секунды", new HintParameter[]
                                    {
new StringHintParameter("")
                                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
                                    Stats[player.characterClassManager.UserId].shelter--;
                                }
                                else if (Stats[player.characterClassManager.UserId].shelter == 1)
                                {
                                    player.hints.Show(new TextHint($"\nПодождите {Stats[player.characterClassManager.UserId].shelter} секунду", new HintParameter[]
                                    {
new StringHintParameter("")
                                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 1f));
                                    Stats[player.characterClassManager.UserId].shelter--;
                                }
                                else if (Stats[player.characterClassManager.UserId].shelter == 0)
                                {
                                    player.playerMovementSync.OverridePosition(new Vector3(sdoor.transform.position.x, sdoor.transform.position.y + 1f, sdoor.transform.position.z), 0f);
                                    Stats[player.characterClassManager.UserId].shelter = 3;
                                }
                                else
                                {
                                    Stats[player.characterClassManager.UserId].shelter = 3;
                                }
                            }
                        }
                    }

                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public static IEnumerator<float> checkdoor1()
        {
            for (; ; )
            {
                Vector3 door1 = new Vector3(-58, 991, -51);
                Vector3 door2 = new Vector3(-55, 987, -48);
                Vector3 randomSP = new Vector3(-80, 984, -52);
                List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
                foreach (ReferenceHub player in playerssl)
                {
                    if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door1.y && player.transform.position.y >= door2.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
                    {
                        player.playerMovementSync.OverridePosition(randomSP, 0f);
                        MapEditor.Editor.LoadMap(null, "gate3");
                        Stats[player.characterClassManager.UserId].shelter = 3;
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public static IEnumerator<float> checkdoor2()
        {
            for (; ; )
            {
                Vector3 door1 = new Vector3(-84, 982, -50);
                Vector3 door2 = new Vector3(-77, 990, -44);
                Vector3 randomSP = new Vector3(-51, 989, -50);
                List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
                foreach (ReferenceHub player in playerssl)
                {
                    if (player.transform.position.x <= door2.x && player.transform.position.x >= door1.x && player.transform.position.y <= door2.y && player.transform.position.y >= door1.y && player.transform.position.z <= door2.z && player.transform.position.z >= door1.z)
                    {
                        player.playerMovementSync.OverridePosition(randomSP, 0f);
                        MapEditor.Editor.LoadMap(null, "gate3");
                        Stats[player.characterClassManager.UserId].shelter = 3;
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
namespace MapEditor
{
    using Mirror;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using Newtonsoft.Json;
    using static MapEditor.Editor;
    using Exiled.API.Interfaces;
    using Exiled.API.Features;
    using Exiled.API.Enums;
    using Exiled.Events.EventArgs;
    using gate3;
    using System;
    using System.Linq;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;
    public class MainClass : Plugin<Config>
    {
        private static string appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        public static string pluginDir = Path.Combine(appData, "Plugins", "Textures");
        public static MapEditorSettings settings;
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override void OnEnabled()
        {
            base.OnEnabled();
            Log.Info("Textures loaded.");
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayersEvent;

            Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;

            var f = new gate3.Plugin();
            f.RegisterEvents();
            //string appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            //pluginDir = Path.Combine(appData, "Plugins", "Textures");
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
        }

        public static GameObject GetWorkStationObject()
        {
            GameObject bench = UnityEngine.Object.Instantiate(NetworkManager.singleton.spawnPrefabs.Find(p => p.gameObject.name == "Work Station"));
            return bench;
        }
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
    public class EventHandlers
    {
        public static bool loaded = false;
        public static void WaitingForPlayersEvent()
        {
            if (!loaded)
            {
                loaded = true;
                foreach (KeyValuePair<int, List<string>> map in MainClass.settings.MapToLoad)
                {
                    if (map.Key == ServerConsole.Port)
                    {
                        foreach (string str in map.Value)
                        {
                            Editor.LoadMap(null, str);
                        }
                    }
                }
            }
        }
        public static void OnPlayerJoin(JoinedEventArgs ev)
        {
            ev.Player.SetRole(RoleType.Tutorial);
            ev.Player.ReferenceHub.SetPosition(new Vector3(-178, 990, -57));
            Editor.LoadMap(null, "gate3");
        }
    }
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