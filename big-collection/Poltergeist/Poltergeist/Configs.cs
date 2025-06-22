using System.Collections.Generic;

namespace Poltergeist
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

		internal static void ReloadConfig()
		{
			Configs.health = Plugin.Config.GetInt("j_health", 300);
			Configs.spawnchance = Plugin.Config.GetInt("j_spawn_chance", 30);
			Configs.spawnitems = Plugin.Config.GetIntList("j_spawn_items");
			Configs.dsreplace = Plugin.Config.GetBool("j_classd_replace_scientist", false);
			Configs.log = Plugin.Config.GetBool("j_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("j_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("j_better_ec", false);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("j_scp_friendly_fire", false);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("j_tutorial_friendly_fire", false);
			Configs.winWithTutorial = Plugin.Config.GetBool("j_win_with_tutorial", false);
			Configs.teleportTo106 = Plugin.Config.GetBool("j_teleport_to_106", true);

			if (Configs.spawnitems == null || Configs.spawnitems.Count == 0)
			{
				Configs.spawnitems = new List<int>() { 10, 20, 25, 12, 17, 14, 14, 14 };
			}
		}
	}
}
