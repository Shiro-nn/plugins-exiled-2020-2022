using System.Collections.Generic;

namespace fishhook
{
	internal static class Configs
	{
		internal static int healwm;
		internal static int health;
		internal static float healDistance;
		internal static float healInterval;
		internal static string bb;
		internal static uint bbt;
		internal static string mb;
		internal static uint mbt;
		internal static string db;
		internal static uint dbt;
		internal static float gbg;
		internal static float gmm;
		internal static bool tpplayer;
		internal static int mins;
		internal static int maxs;
		internal static float gp;
		internal static string ob;
		internal static uint obt;
		internal static uint mps;
		internal static bool bp;
		internal static bool mp;
		internal static float dd;
		internal static void ReloadConfig()
		{
			Configs.healwm = Plugin.Config.GetInt("f_health", 5);
			Configs.health = Plugin.Config.GetInt("f_max_health_hp", 150);
			Configs.healDistance = Plugin.Config.GetFloat("f_heal_distance", 2);
			Configs.healInterval = Plugin.Config.GetFloat("f_heal_interval", 1);
			Configs.bbt = Plugin.Config.GetUInt("f_spawn_bomber_bc_time", 5);
			Configs.bb = Plugin.Config.GetString("f_spawn_bomber_bc", "You bomber");
			Configs.mbt = Plugin.Config.GetUInt("f_spawn_medical_bc_time", 5);
			Configs.mb = Plugin.Config.GetString("f_spawn_medical_bc", "You medical");
			Configs.dbt = Plugin.Config.GetUInt("f_spawn_bright_bc_time", 5);
			Configs.db = Plugin.Config.GetString("f_spawn_bright_bc", "You Dr.bright");
			Configs.gbg = Plugin.Config.GetFloat("f_give_bomber_ofter_use_grenade", 10);
			Configs.gmm = Plugin.Config.GetFloat("f_give_med_ofter_use_med", 10);
			Configs.tpplayer = Plugin.Config.GetBool("f_tp_player", true);
			Configs.mins = Plugin.Config.GetInt("f_minimum_round_time_for_spawn", 100);
			Configs.maxs = Plugin.Config.GetInt("f_max_round_time_for_spawn", 600);
			Configs.gp = Plugin.Config.GetFloat("f_players_walk_sec", 60);//after how many seconds the players can walk
			Configs.ob = Plugin.Config.GetString("f_spawn_o_bc", "You O..i don't know");
			Configs.obt = Plugin.Config.GetUInt("f_spawn_o_bc_time", 5);
			Configs.mps = Plugin.Config.GetUInt("f_min_players_for_spawn_o", 3);
			Configs.bp = Plugin.Config.GetBool("f_bomber_pickup_items", false);
			Configs.mp = Plugin.Config.GetBool("f_med_pickup_items", false);
			Configs.dd = Plugin.Config.GetFloat("f_dio_dmg", 39);
		}
	}
}
