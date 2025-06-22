using EXILED;
using System.Collections.Generic;

namespace CISpy
{
	class Configs
	{
		internal static List<int> spyRoles;

		internal static bool spawnWithGrenade;

		internal static int spawnChance;
		internal static int guardSpawnChance;
		internal static int minimumSquadSize;
		internal static ushort shootbct;
		internal static string shootbc;
		internal static string scientist;
		internal static string role;
		internal static string rolec;
		internal static ushort teamshootbct;
		internal static string teamshootbc;
		internal static ushort spawnbct;
		internal static string spawnbc;
		internal static string cm;
		internal static string cmc;
		internal static ushort acdbt;
		internal static string acdb;

		internal static void ReloadConfigs()
		{
			spyRoles = Plugin.Config.GetIntList("cis_spy_roles");
			if (spyRoles == null || spyRoles.Count == 0)
			{
				spyRoles = new List<int>() { 11, 13 };
			}

			spawnWithGrenade = Plugin.Config.GetBool("cis_spawn_with_grenade", true);

			spawnChance = Plugin.Config.GetInt("cis_spawn_chance", 60);
			guardSpawnChance = Plugin.Config.GetInt("cis_guard_chance", 50);
			minimumSquadSize = Plugin.Config.GetInt("cis_minimum_size", 6);
			shootbc = Plugin.Config.GetString("cis_shoot_bc", "You attacked a %team%, you are now able to be killed by <color=#00b0fc>Nine Tailed Fox</color> and <color=#fcff8d>Scientists</color>");
			shootbct = Plugin.Config.GetUShort("cis_shoot_bc_time", 10);
			scientist = Plugin.Config.GetString("cis_scientist", "Scientist");
			role = Plugin.Config.GetString("cis_role", "CISpy");
			rolec = Plugin.Config.GetString("cis_role_color", "green");
			teamshootbc = Plugin.Config.GetString("cis_team_shoot_bc", "You are shooting a <b><color=\"green\">CISpy!</color></b>");
			teamshootbct = Plugin.Config.GetUShort("cis_team_shoot_bc_time", 3);
			spawnbc = Plugin.Config.GetString("cis_spawn_bc", "<size=60>You are a <b><color=\"green\">CISpy</color></b></size>\nCheck your console by pressing [`] or [~] for more info.");
			spawnbct = Plugin.Config.GetUShort("cis_spawn_bc_time", 3);
			cm = Plugin.Config.GetString("cis_console_msg", "You are a Chaos Insurgency Spy! You are immune to MTF for now, but as soon as you damage an MTF, your spy immunity will turn off.\n\nHelp Chaos win the round and kill as many MTF and Scientists as you can.");
			cmc = Plugin.Config.GetString("cis_console_msg_color", "yellow");
			acdb = Plugin.Config.GetString("cis_all_ci_dead_bc", "Your fellow <color=\"green\">Chaos Insurgency</color> have died.\nYou have been revealed!");
			acdbt = Plugin.Config.GetUShort("cis_all_ci_dead_bc_time", 10);
		}
	}
}