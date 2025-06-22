using System.Collections.Generic;

namespace fishhook2
{
	internal static class Configs
	{
		internal static bool scp074god;
		internal static bool scp074ci;
		internal static string prefix049;
		internal static string lvlup049;
		internal static uint lvlup049t;
		internal static string prefix049c;
		internal static uint spawn074bctime;
		internal static string spawn074bcmsg;
		internal static uint spawn939bctime;
		internal static string spawn939bcmsg;
		internal static uint spawndogbctime;
		internal static string spawndogbcmsg;
		internal static uint pngt;
		internal static string png;
		internal static string scpspawnripcassie;
		internal static void ReloadConfig()
		{
			Configs.scp074god = Plugin.Config.GetBool("f2_scp074_goodmode", true);
			Configs.scp074ci = Plugin.Config.GetBool("f2_scp074_clear_inventory", true);
			Configs.prefix049 = Plugin.Config.GetString("f2_049_prefix", "%lvl%lvl %xp%xp");
			Configs.lvlup049 = Plugin.Config.GetString("f2_049_lvl_up_bc", "You up lvl to %lvl%, xp: %xp%");
			Configs.lvlup049t = Plugin.Config.GetUInt("f2_049_lvl_up_bc_time", 10);
			Configs.prefix049c = Plugin.Config.GetString("f2_049_prefix_color", "cyan");
			Configs.spawn074bctime = Plugin.Config.GetUInt("f2_spawn_074_bc_time", 10);
			Configs.spawn074bcmsg = Plugin.Config.GetString("f2_spawn_074_bc", "<color=red>You scp 074</color>");
			Configs.spawn939bctime = Plugin.Config.GetUInt("f2_spawn_939_bc_time", 10);
			Configs.spawn939bcmsg = Plugin.Config.GetString("f2_spawn_949_bc", "<color=red>You scp 939ex</color>");
			Configs.spawndogbctime = Plugin.Config.GetUInt("f2_spawn_682_bc_time", 10);
			Configs.spawndogbcmsg = Plugin.Config.GetString("f2_spawn_682_bc", "<color=red>You scp 682</color>");
			Configs.pngt = Plugin.Config.GetUInt("f2_pickup_nice_gun_bc_time", 10);
			Configs.png = Plugin.Config.GetString("f2_pickup_nice_gun_bc", "<color=red>You pickup nice gun</color>");
			Configs.scpspawnripcassie = Plugin.Config.GetString("f2_spawn_cassie", "ATTENTION TO ALL PERSONNEL ESCAPE SCP 6 8 2 ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B");
		}
	}
}
