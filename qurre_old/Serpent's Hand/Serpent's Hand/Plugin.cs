using System;
using System.Collections.Generic;
namespace SerpentsHand
{
    public class Plugin : Qurre.Plugin
    {
        public static bool Enabled { get; internal set; }
        public EventHandlers EventHandlers;
        #region override
        public override int Priority => 1000;
        public override string Developer => "fydne";
        public override string Name => "Serpent's Hand";
        public override Version Version => new Version(1, 0, 8);
        public override Version NeededQurreVersion => new Version(1, 11, 0);
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Enabled = Config.GetBool("SerpentsHand_enable", true);
            if (!Enabled) return;
            Cfg.Reload();
            EventHandlers = new EventHandlers();
            Qurre.Events.Round.Waiting += EventHandlers.Waiting;
            Qurre.Events.Server.SendingRA += EventHandlers.Ra;
            Qurre.Events.Round.TeamRespawn += EventHandlers.TeamRespawn;
            Qurre.Events.Scp106.PocketFailEscape += EventHandlers.PocketFail;
            Qurre.Events.Scp106.PocketEnter += EventHandlers.PocketEnter;
            Qurre.Events.Player.ScpAttack += EventHandlers.AntiScpAttack;
            Qurre.Events.Player.DamageProcess += EventHandlers.Damage;
            Qurre.Events.Player.Dead += EventHandlers.Dead;
            Qurre.Events.Player.RoleChange += EventHandlers.RoleChange;
            Qurre.Events.Player.Spawn += EventHandlers.Spawn;
            Qurre.Events.Scp096.AddTarget += EventHandlers.AddTarget;
            Qurre.Events.Round.Check += EventHandlers.Check;
        }
        private void UnregisterEvents()
        {
            if (!Enabled) return;
            Qurre.Events.Round.Waiting -= EventHandlers.Waiting;
            Qurre.Events.Server.SendingRA -= EventHandlers.Ra;
            Qurre.Events.Round.TeamRespawn -= EventHandlers.TeamRespawn;
            Qurre.Events.Scp106.PocketFailEscape -= EventHandlers.PocketFail;
            Qurre.Events.Scp106.PocketEnter -= EventHandlers.PocketEnter;
            Qurre.Events.Player.ScpAttack -= EventHandlers.AntiScpAttack;
            Qurre.Events.Player.DamageProcess -= EventHandlers.Damage;
            Qurre.Events.Player.Dead -= EventHandlers.Dead;
            Qurre.Events.Player.RoleChange -= EventHandlers.RoleChange;
            Qurre.Events.Player.Spawn -= EventHandlers.Spawn;
            Qurre.Events.Scp096.AddTarget -= EventHandlers.AddTarget;
            Qurre.Events.Round.Check -= EventHandlers.Check;
            EventHandlers = null;
        }
        #endregion
    }
    public class Cfg
    {
        private static string PlugName => "SerpentsHand_";
        public static string Spawn_bc;
        public static ushort Spawn_bc_time;
        public static string Map_Spawn_bc;
        public static ushort Map_Spawn_bc_time;
        public static int Max_players;
        public static int Chance;
        public static int Hp;
        public static string UnitName;
        public static List<int> SpawnItems = new List<int>() { 10, 20, 12, 14, 33, 25, 26, 15 };
        public static void Reload()
        {
            UnitName = Plugin.Config.GetString(PlugName + "UnitName", "Serpent's Hand", "Unit Name");
            Spawn_bc = Plugin.Config.GetString(PlugName + "spawn_bc", "<size=30%><color=red>You</color> are <color=#15ff00>Serpent's Hand</color>\n" +
                "<color=#00ffdc>Your task is to kill everyone except <color=red>SCP</color></color></size>",
                "Private broadcast text about the arrival of the serpent's hand at the complex");
            Spawn_bc_time = Plugin.Config.GetUShort(PlugName + "spawn_bc_time", 10, "Duration of the private broadcast about the arrival of the serpent's hand at the complex");
            Map_Spawn_bc = Plugin.Config.GetString(PlugName + "map_spawn_bc", "<size=30%><color=red>Attention to all personnel!</color>\n" +
                "<color=#00ffff><color=#15ff00>Serpent's Hand</color> Squad</color> <color=#0089c7>has arrived at the complex</color></size>",
                "Text broadcast about the arrival of the serpent's hand at the complex");
            Map_Spawn_bc_time = Plugin.Config.GetUShort(PlugName + "map_spawn_bc_time", 10, "Duration of the broadcast about the arrival of the serpent's hand at the complex");
            Max_players = Plugin.Config.GetInt(PlugName + "max_players", 15, "The maximum number of players that can spawn in a squad");
            Chance = Plugin.Config.GetInt(PlugName + "chance", 40, "Spawn chance");
            Hp = Plugin.Config.GetInt(PlugName + "hp", 150, "Health at the Serpent's Hand");
            string[] lst = Plugin.Config.GetString(PlugName + "spawn_items", "10, 20, 12, 14, 33, 25, 26, 15", "Items").Split(',');
            SpawnItems.Clear();
            foreach (var str in lst) { try { SpawnItems.Add(Convert.ToInt32(str)); } catch { } }
        }
    }
}