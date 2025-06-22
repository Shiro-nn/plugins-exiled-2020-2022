using System.Collections.Generic;

namespace PlayerXP
{
	internal static class Configs
	{
		internal static string lvl;
		internal static string pn;
		internal static string jm;
		internal static string lvlup;
		internal static string eb;
		internal static string kb;
		internal static string cc;
		internal static void ReloadConfig()
		{
			Configs.lvl = Plugin.Config.GetString("px_lvl", "lvl");
			Configs.pn = Plugin.Config.GetString("px_project_name", "fydne");
			Configs.jm = Plugin.Config.GetString("px_join_bc", "<color=red>If you write in a nickname</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>then you will get 2 times more experience</color>");
			Configs.lvlup = Plugin.Config.GetString("px_lvl_up", "<color=#fdffbb>You got %lvl% level! Up to the next level you are missing %to.xp% xp.</color>");
			Configs.eb = Plugin.Config.GetString("px_escape_bc", "<color=#fdffbb>You got %xp%xp for escape</color>");
			Configs.kb = Plugin.Config.GetString("px_kill_bc", "<color=#fdffbb>You got %xp%xp for kill</color> <color=red>%player%</color>");
			Configs.cc = Plugin.Config.GetString("px_console_command", "level");
		}
	}
}
