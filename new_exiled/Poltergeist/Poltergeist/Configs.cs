using System.Collections.Generic;

namespace Poltergeist
{
	internal static class Configs
	{
		internal static string ab;
		internal static ushort abt;
		internal static string psb;
		internal static ushort psbt;
		internal static string pdb;
		internal static ushort pdbt;
		internal static string phb;
		internal static ushort phbt;
		internal static bool open012;
		internal static void ReloadConfig()
		{
			Plugin.InitialDelay = Plugin.Config.GetFloat("p_initial_delay", 400f);
			Plugin.DurationMin = Plugin.Config.GetFloat("p_dur_min", 50f);
			Plugin.DurationMax = Plugin.Config.GetFloat("p_dur_max", 100);
			Plugin.RandomEvents = Plugin.Config.GetBool("p_random_events", true);
			Plugin.DelayMin = Plugin.Config.GetInt("p_delay_min", 180);
			Plugin.DelayMax = Plugin.Config.GetInt("p_delay_max", 500);
			Configs.ab = Plugin.Config.GetString("p_poltergeist_attack_bc", "<color=red>Вас атакует полтергейст!</color>\n<color=lime>Возьмите фонарик</color><color=aqua>!</color>");
			Configs.abt = Plugin.Config.GetUShort("p_poltergeist_attack_bc_time", 5);
			Configs.psb = Plugin.Config.GetString("p_poltergeist_spawn_bc", "<color=aqua>Появился</color> <color=red>полтергейст</color>");
			Configs.psbt = Plugin.Config.GetUShort("p_poltergeist_spawn_bc_time", 10);
			Configs.pdb = Plugin.Config.GetString("p_player_death_bc", "<color=lime>Полтергейст получил свою жертву.</color>\n<color=aqua>Теперь он дружелюбней</color>");
			Configs.pdbt = Plugin.Config.GetUShort("p_player_death_bc_time", 10);
			Configs.phb = Plugin.Config.GetString("p_poltergeist_house_bc", "<color=red>Это дом полтергейста, уходи!</color>");
			Configs.phbt = Plugin.Config.GetUShort("p_poltergeist_house_bc_time", 5);
			Configs.open012 = Plugin.Config.GetBool("p_open_012", false);
		}
	}
}
