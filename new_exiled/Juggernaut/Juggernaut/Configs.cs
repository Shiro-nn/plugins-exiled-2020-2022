using System.Collections.Generic;

namespace Juggernaut
{
	internal static class Configs
	{

		internal static List<int> spawnitems;
		internal static int spawnchance;
		internal static int health;
		internal static int tp;
		internal static bool dsreplace;
		internal static bool log;
		internal static bool moreoptions;
		internal static bool bettercondition;
		internal static bool scpFriendlyFire;
		internal static bool winWithTutorial;
		internal static bool tutorialFriendlyFire;
		internal static bool teleportTo106;
		internal static string command;
		internal static string suc;
		internal static string nf;
		internal static string jug;
		internal static string jugc;
		internal static string jd;
		internal static ushort jdt;
		internal static string js;
		internal static ushort jst;
		internal static string je;
		internal static ushort jet;

		internal static void ReloadConfig()
		{
			Configs.health = Plugin.Config.GetInt("j_health", 300);
			Configs.spawnchance = Plugin.Config.GetInt("j_spawn_chance", 30);
			Configs.tp = Plugin.Config.GetInt("j_minimum_team_player", 4);
			Configs.spawnitems = Plugin.Config.GetIntList("j_spawn_items");
			Configs.dsreplace = Plugin.Config.GetBool("j_facility_replace", true);
			Configs.log = Plugin.Config.GetBool("j_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("j_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("j_better_ec", false);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("j_scp_friendly_fire", false);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("j_tutorial_friendly_fire", false);
			Configs.winWithTutorial = Plugin.Config.GetBool("j_win_with_tutorial", true);
			Configs.teleportTo106 = Plugin.Config.GetBool("j_teleport_to_106", true);
			Configs.command = Plugin.Config.GetString("j_command", "JG");
			Configs.suc = Plugin.Config.GetString("j_successfully", "successfully");
			Configs.nf = Plugin.Config.GetString("j_not_found", "hmm, player not found. It's a pity(");
			Configs.jug = Plugin.Config.GetString("j_juggernaut_prefix", "Juggernaut");
			Configs.jugc = Plugin.Config.GetString("j_juggernaut_prefix_color", "magenta");
			Configs.jd = Plugin.Config.GetString("j_dead_bc", "\n<color=lime>Juggernaut is dead :(</color>");
			Configs.jdt = Plugin.Config.GetUShort("j_dead_bc_time", 10);
			Configs.js = Plugin.Config.GetString("j_spawn_bc", "<color=red>You are spawned for a Juggernaut.</color>\n <color=red>You are against everyone except scp and hand</color>\n You will be released in 2 minutes");
			Configs.jst = Plugin.Config.GetUShort("j_spawn_bc_time", 10);
			Configs.je = Plugin.Config.GetString("j_escape_bc", "<color=magenta>Juggernaut Appears</color>\n<color=red>He is against everyone except SCP and Hand</color>");
			Configs.jet = Plugin.Config.GetUShort("j_escape_bc_time", 10);
			if (Configs.spawnitems == null || Configs.spawnitems.Count == 0)
			{
				Configs.spawnitems = new List<int>() { 10, 20, 25, 12, 17, 14, 14, 14 };
			}
		}
	}
}
