//using System;
//using System.IO;
//using Exiled.API.Features;

namespace PlayerXP
{
    internal static class Configs
	{
		//private static readonly string translationPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "configs");
		//public static string cfgr { get; private set; } = Path.Combine(translationPath, $"{Server.Port}-configs.yml");
		internal static string lvl = "уровень";
		internal static string pn = "fydne";
		internal static string jm = "<color=red>Если вы напишите в нике</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>то вы будете получать в 2 раза больше опыта+монет</color>";
		internal static string lvlup = "<color=#fdffbb>Вы получили %lvl% уровень! До следующего уровня вам не хватает %to.xp% xp.</color>";
		internal static string eb = "<color=#fdffbb>Вы получили %xp%xp+%money% монет за побег</color>";
		internal static string kb = "<color=#fdffbb>Вы получили %xp%xp+%money% монет за убийство</color> <color=red>%player%</color>";
		internal static string cc = "level";
		/*
		internal static void ReloadConfig()
		{
			if (!File.Exists(cfgr))
			{
				if (!Directory.Exists(translationPath))
				{
					Directory.CreateDirectory(translationPath);
				}
				if (!File.Exists(cfgr))
					File.Create(cfgr).Close();
				Plugin.cfg = new YamlConfig(cfgr);

			}
			Configs.lvl = Plugin.cfg.GetString("px_lvl", "уровень");
			Configs.pn = Plugin.cfg.GetString("px_project_name", "fydne");
			Configs.jm = Plugin.cfg.GetString("px_join_bc", "<color=red>Если вы напишите в нике</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>то вы будете получать в 2 раза больше опыта</color>");
			Configs.lvlup = Plugin.cfg.GetString("px_lvl_up", "<color=#fdffbb>Вы получили %lvl% уровень! До следующего уровня вам не хватает %to.xp% xp.</color>");
			Configs.eb = Plugin.cfg.GetString("px_escape_bc", "<color=#fdffbb>Вы получили %xp%xp за побег</color>");
			Configs.kb = Plugin.cfg.GetString("px_kill_bc", "<color=#fdffbb>Вы получили %xp%xp за убийство</color> <color=red>%player%</color>");
			Configs.cc = Plugin.cfg.GetString("px_console_command", "level");
		}*/
	}
}
