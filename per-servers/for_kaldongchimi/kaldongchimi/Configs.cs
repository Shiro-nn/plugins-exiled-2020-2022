using System.Collections.Generic;

namespace kaldongchimi
{
	internal static class Configs
	{
		internal static string killer;
		internal static uint killerdur;
		internal static int alphastart;
		internal static bool nuke;
		internal static string aabc;
		internal static string pig;
		internal static string pc;
		internal static string am;
		internal static string ad;
		internal static string ame;
		internal static string amd;
		internal static string sl;
		internal static string asd;
		internal static uint aabctime;

		internal static void ReloadConfig()
		{
			Configs.nuke = Plugin.Config.GetBool("k_auto_alpha_start", true);
			Configs.alphastart = Plugin.Config.GetInt("k_alpha_start_time", 900);
			Configs.aabc = Plugin.Config.GetString("k_auto_alpha_start_bc", "<color=red>Auto Warhead Started!</color>");
			Configs.pig = Plugin.Config.GetString("k_players_in_game", "The following players are currently playing:");
			Configs.pc = Plugin.Config.GetString("k_players_command", "players");
			Configs.am = Plugin.Config.GetString("k_admin_mode_command", "am");
			Configs.ad = Plugin.Config.GetString("k_access_denied_command", "Access denied");
			Configs.ame = Plugin.Config.GetString("k_admin_mode_enabled_command", "Admin mode enabled(you ghost)");
			Configs.amd = Plugin.Config.GetString("k_admin_mode_disabled_command", "Admin mode disabled");
			Configs.sl = Plugin.Config.GetString("k_scp_list_command", "scplist");
			Configs.asd = Plugin.Config.GetString("k_all_scp_dead", "All scp dead");
			Configs.aabctime = Plugin.Config.GetUInt("k_auto_alpha_start_bc_time", 5);
			Configs.killerdur = Plugin.Config.GetUInt("k_death_bc_time", 5);
			Configs.killer = Plugin.Config.GetString("k_death_bc", "<size=20><color=aqua>%killer.name%</color><color=red>(</color><color=aqua>%killer.role%</color><color=red>) killed</color> <color=aqua>%player.name%</color><color=red>(</color><color=aqua>%player.role%</color><color=red>)</color></size>");
		}
	}
}
