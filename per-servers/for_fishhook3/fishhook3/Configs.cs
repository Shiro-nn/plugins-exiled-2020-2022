using System.Collections.Generic;

namespace fishhook3
{
	internal static class Configs
	{
		internal static bool ajks;
		internal static uint s0005pbct;
		internal static string s0005pbc;
		internal static uint s038bct;
		internal static string s038bc;
		internal static uint s079bct;
		internal static string s079bc;
		internal static string s079h;
		internal static string s079s;
		internal static string lbb;
		internal static uint lbbt;
		internal static string lbh;
		internal static string lbs;
		internal static void ReloadConfig()
		{
			Configs.ajks = Plugin.Config.GetBool("f3_all_janitor_keycard_scp005", false);
			Configs.s0005pbct = Plugin.Config.GetUInt("f3_pickup_scp005_bc_time", 10);
			Configs.s0005pbc = Plugin.Config.GetString("f3_pickup_scp005_bc", "You pickup scp005");
			Configs.s038bct = Plugin.Config.GetUInt("f3_scp038_bc_time", 10);
			Configs.s038bc = Plugin.Config.GetString("f3_scp038_bc", "Its scp 038");
			Configs.s079bct = Plugin.Config.GetUInt("f3_scp079_spawn_bc_time", 10);
			Configs.s079bc = Plugin.Config.GetString("f3_scp079_spawn_bc", ".scp, .human");
			Configs.s079h = Plugin.Config.GetString("f3_scp079_human_console", "Do you help humans");
			Configs.s079s = Plugin.Config.GetString("f3_scp079_scp_console", "Do you help scp");
			Configs.lbb = Plugin.Config.GetString("f3_lucky_boi_bc", "%player% lucky boi, send .lb for help");
			Configs.lbbt = Plugin.Config.GetUInt("f3_lucky_boi_bc_time", 10);
			Configs.lbh = Plugin.Config.GetString("f3_lucky_boi_help", "Lucky boi can become scp\nCommands:\n.s173\n.scp106\n.s096\n.s939\n.s049\n.s079");
			Configs.lbs = Plugin.Config.GetString("f3_lucky_boi_suc", "Successfully!");
		}
	}
}
