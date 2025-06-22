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
			Configs.lvl = Plugin.Config.GetString("px_lvl", "уровень");
			Configs.pn = Plugin.Config.GetString("px_project_name", "fydne");
			Configs.jm = Plugin.Config.GetString("px_join_bc", "<color=red>Если вы напишите в нике</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>то вы будете получать в 2 раза больше опыта</color>");
			Configs.lvlup = Plugin.Config.GetString("px_lvl_up", "<color=#fdffbb>Вы получили %lvl% уровень! До следующего уровня вам не хватает %to.xp% xp.</color>");
			Configs.eb = Plugin.Config.GetString("px_escape_bc", "<color=#fdffbb>Вы получили %xp%xp за побег</color>");
			Configs.kb = Plugin.Config.GetString("px_kill_bc", "<color=#fdffbb>Вы получили %xp%xp за убийство</color> <color=red>%player%</color>");
			Configs.cc = Plugin.Config.GetString("px_console_command", "level");
		}
	}
}
