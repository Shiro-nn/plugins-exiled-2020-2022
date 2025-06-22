using System.Collections.Generic;

namespace scp228ruj
{
	internal static class Configs
	{

		internal static List<int> spawnitems;
		internal static int spawnchance;
		internal static int health;
		internal static bool dsreplace;
		internal static bool log;
		internal static bool moreoptions;
		internal static bool bettercondition;
		internal static bool scpFriendlyFire;
		internal static bool winWithTutorial;
		internal static bool tutorialFriendlyFire;
		internal static bool teleportTo106;
		internal static string error1;
		internal static string error2;
		internal static string error3;
		internal static string eb;
		internal static ushort ebt;
		internal static string db;
		internal static ushort dbt;
		internal static string sc;
		internal static string scc;
		internal static string sb;
		internal static ushort sbt;
		internal static string s;
		internal static string vppb;
		internal static ushort vppbt;
		internal static string vpb;
		internal static ushort vpbt;
		internal static string a;
		internal static string svb;
		internal static ushort svbt;
		internal static string eeb;
		internal static ushort eebt;
		internal static string com;
		internal static string suc;
		internal static string nf;

		internal static void ReloadConfig()
		{
			Configs.health = Plugin.Config.GetInt("scp228ruj_health", 300);
			Configs.spawnchance = Plugin.Config.GetInt("scp228ruj_spawn_chance", 30);
			Configs.spawnitems = Plugin.Config.GetIntList("scp228ruj_spawn_items");
			Configs.dsreplace = Plugin.Config.GetBool("scp228ruj_classd_replace_scientist", false);
			Configs.log = Plugin.Config.GetBool("scp228ruj_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("scp228ruj_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("scp228ruj_better_ec", false);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("scp228ruj_scp_friendly_fire", true);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("scp228ruj_tutorial_friendly_fire", true);
			Configs.winWithTutorial = Plugin.Config.GetBool("scp228ruj_win_with_tutorial", false);
			Configs.teleportTo106 = Plugin.Config.GetBool("scp228ruj_teleport_to_106", true);
			Configs.error1 = Plugin.Config.GetString("scp228ruj_error_bc", "<color=red>Error...</color>");
			Configs.error2 = Plugin.Config.GetString("scp228ruj_error_console", "Error...");
			Configs.error3 = Plugin.Config.GetString("scp228ruj_error_console_color", "red");
			Configs.eb = Plugin.Config.GetString("scp228ruj_escape_bc", "\n<color=lime>Last <color=red>SCP 228 RU J</color> escaped!</color>");
			Configs.ebt = Plugin.Config.GetUShort("scp228ruj_escape_bc_time", 10);
			Configs.db = Plugin.Config.GetString("scp228ruj_death_bc", "\n<color=lime>Last <color=red>SCP 228 RU J</color> death!</color>");
			Configs.dbt = Plugin.Config.GetUShort("scp228ruj_death_bc_time", 10);
			Configs.sc = Plugin.Config.GetString("scp228ruj_spawn_console", "All your fellow gopniks are dead :(\nYou have already run out of this wonderful vodka drink \nYou are thirsty, so find vodka and drink it, if you don’t do this, you will die \nYou have a super ability to open only doors that lead to vodka \nVodka for a rainy day is your vodka for a rainy day, it is contained in a bottle of cola, there is only one, do not mix it up!");
			Configs.scc = Plugin.Config.GetString("scp228ruj_spawn_console_color", "red");
			Configs.sb = Plugin.Config.GetString("scp228ruj_spawn_bc", "<color=red>All your fellow gopniks are dead</color><color=aqua>(</color>\n<color=lime>More information on [~]</color>");
			Configs.sbt = Plugin.Config.GetUShort("scp228ruj_spawn_bc_time", 10);
			Configs.s = Plugin.Config.GetString("scp228ruj_spawn", "On spawn");
			Configs.vppb = Plugin.Config.GetString("scp228ruj_vodka_pickup_player_bc", "<color=red>It's vodka SCP 228 RU J (%player%)</color>");
			Configs.vppbt = Plugin.Config.GetUShort("scp228ruj_vodka_pickup_player_bc_time", 5);
			Configs.vpb = Plugin.Config.GetString("scp228ruj_vodka_pickup_bc", "<color=aqua>%player%</color><color=red> tried to pick up your vodka</color>");
			Configs.vpbt = Plugin.Config.GetUShort("scp228ruj_vodka_pickup_bc_time", 5);
			Configs.a = Plugin.Config.GetString("scp228ruj_attacking", "<color=red>SCP 228 RU J</color> <color=aqua>attacking you</color><color=red>!</color>\n<color=lime>Run away from him, he is looking for vodka</color>");
			Configs.svb = Plugin.Config.GetString("scp228ruj_search_vodka_bc", "<color=lime>Hooray! Did you find vodka</color>\n<color=aqua>Drink it and run away, the main thing is to drink, because it's not cola, but vodka</color>");
			Configs.svbt = Plugin.Config.GetUShort("scp228ruj_search_vodka_bc_time", 10);
			Configs.eeb = Plugin.Config.GetString("scp228ruj_escape_error_bc", "<color=red>You didn't find vodka!</color>");
			Configs.eebt = Plugin.Config.GetUShort("scp228ruj_escape_error_bc_time", 10);
			Configs.com = Plugin.Config.GetString("scp228ruj_command", "scp228");
			Configs.nf = Plugin.Config.GetString("scp228ruj_not_found", "player not found");
			Configs.suc = Plugin.Config.GetString("scp228ruj_successfully", "successfully");

			if (Configs.spawnitems == null || Configs.spawnitems.Count == 0)
			{
				Configs.spawnitems = new List<int>() { 10, 20, 25, 12, 17, 14, 14, 14 };
			}
		}
	}
}
