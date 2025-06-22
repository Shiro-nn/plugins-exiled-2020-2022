using System.Collections.Generic;
namespace gok
{
    public class Plugin : Qurre.Plugin
    {
        public static bool Enabled { get; internal set; }
        private EventHandlers EventHandlers;
        #region override
        public override int Priority { get; } = 9;
        public override string Developer { get; } = "fydne from ~2020";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Enabled = Plugin.Config.GetBool("gok_enable", true);
            if (!Enabled) return;
            Cfg.Reload();
            EventHandlers = new EventHandlers();
            Qurre.Events.Server.SendingRA += EventHandlers.Ra;
            Qurre.Events.Round.TeamRespawn += EventHandlers.TeamRespawn;
            Qurre.Events.Round.Waiting += EventHandlers.Waiting;
            //Qurre.Events.Round.Check += EventHandlers.CheckRound;
            Qurre.Events.Player.Dead += EventHandlers.Dead;
            Qurre.Events.Player.RoleChange += EventHandlers.Spawn;
            Qurre.Events.Player.Spawn += EventHandlers.Spawn;
        }
        private void UnregisterEvents()
        {
            if (!Enabled) return;
            Qurre.Events.Server.SendingRA -= EventHandlers.Ra;
            Qurre.Events.Round.TeamRespawn -= EventHandlers.TeamRespawn;
            Qurre.Events.Round.Waiting -= EventHandlers.Waiting;
            //Qurre.Events.Round.Check -= EventHandlers.CheckRound;
            Qurre.Events.Player.Dead -= EventHandlers.Dead;
            Qurre.Events.Player.RoleChange -= EventHandlers.Spawn;
            Qurre.Events.Player.Spawn -= EventHandlers.Spawn;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Cfg
    {
        public static string All_Spawn_bc { get; set; }
        public static ushort All_Spawn_bc_time { get; set; }
        public static string Spawn_bc { get; set; }
        public static ushort Spawn_bc_time { get; set; }
        public static int Max_players { get; set; }
        public static int Hp { get; set; }
        public static int Chance { get; set; }
        public static List<int> SpawnItems { get; set; } = new List<int>() { 10, 20, 12, 14, 33, 25, 26, 15 };
        public static void Reload()
        {
            Cfg.All_Spawn_bc = Plugin.Config.GetString("gok_map_spawn_bc", "<size=30%><color=red>Приехал отряд ГОК'а</color></size>");
            Cfg.All_Spawn_bc_time = Plugin.Config.GetUShort("gok_map_spawn_bc_time", 10);
            Cfg.Spawn_bc = Plugin.Config.GetString("gok_spawn_bc", "<size=30%><color=red>Вы - ГОК</color></size>");
            Cfg.Spawn_bc_time = Plugin.Config.GetUShort("gok_spawn_bc_time", 10);
            Cfg.Max_players = Plugin.Config.GetInt("gok_max_players", 15);
            Cfg.Hp = Plugin.Config.GetInt("gok_hp", 150);
            Cfg.Chance = Plugin.Config.GetInt("gok_chance", 40);
            string[] lst = Plugin.Config.GetString("gok_spawn_items", "10, 20, 12, 14, 33, 25, 26, 15").Split(',');
            SpawnItems.Clear();
            foreach (var str in lst) { try { SpawnItems.Add(System.Convert.ToInt32(str.Trim())); } catch { } }
        }
    }
}