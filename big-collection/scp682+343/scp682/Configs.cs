using System.Collections.Generic;

namespace scp682343
{
	internal static class Configs
	{

		internal static int spawnchance;
		internal static int spawnchance343;
		internal static int dmg;
		internal static int health;
		internal static bool dsreplace;
		internal static bool log;
		internal static bool moreoptions;
		internal static bool bettercondition;
		internal static bool scpFriendlyFire;
		internal static bool winWithTutorial;
		internal static bool tutorialFriendlyFire;
		internal static int count;
		internal static int initialCooldown;
		internal static int minpeople;
		internal static uint spawnbctime;
		internal static uint spawndogbctime;
		internal static string spawnconsolemsg;
		internal static string spawnbcmsg;
		internal static string spawndogbcmsg;
		internal static bool nuke;
		internal static string safeuserbc;
		internal static string safeattackerbc;
		internal static string dooropenbc;
		internal static string repdogbcmsg;
		internal static uint repdogbctime;
		internal static string repbcmsg;
		internal static uint repbctime;
		internal static uint scpgodescapebctime;
		internal static string scpgodescapebc;
		internal static string errorinra;
		internal static string sucinra682;
		internal static string sucinra343;
		internal static string scpgodescapecassie;
		internal static string scpgodripcassie;
		internal static string scprepripcassie;
		internal static string scpspawnripcassie;
		internal static string pnf;
		internal static string tt;
		internal static string soon;
		internal static uint warningTime;
		internal static string warningText;
		internal static uint durmin;
		internal static uint durmax;
		internal static int tranqAmmo;
		internal static uint noAmmoDuration;
		internal static string dontaccess;

		internal static void ReloadConfig()
		{
			Configs.dooropenbc = Plugin.Config.GetString("scp181_door_open_bc", "\n<color=#54ff00>SCP 181 открыл вам дверь</color>");
			Configs.safeuserbc = Plugin.Config.GetString("scp181_safe_user_bc", "\n<color=#54ff00>Вам помог SCP 181</color>");
			Configs.safeattackerbc = Plugin.Config.GetString("scp181_safe_attacker_bc", "\n<color=red>Этой жертве помог SCP 181</color>");
			Configs.health = Plugin.Config.GetInt("scp682_health", 6666);
			Configs.spawnchance = Plugin.Config.GetInt("scp682_spawn_chance", 100);
			Configs.spawnchance343 = Plugin.Config.GetInt("scp343_spawn_chance", 100);
			Configs.dmg = Plugin.Config.GetInt("scp682_dmg", 100);
			Configs.dsreplace = Plugin.Config.GetBool("scp682_force_scp93953", true);
			Configs.log = Plugin.Config.GetBool("scp682_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("scp682_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("scp682_better_ec", false);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("scp682_scp_friendly_fire", false);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("scp682_tutorial_friendly_fire", false);
			Configs.winWithTutorial = Plugin.Config.GetBool("scp682_win_with_tutorial", false);
			Configs.count = Plugin.Config.GetInt("scp682_players_count", 5);
			Configs.initialCooldown = Plugin.Config.GetInt("scp343_time_doors_open", 120);
			Configs.minpeople = Plugin.Config.GetInt("scp343_min_user", 5);
			Configs.spawnconsolemsg = Plugin.Config.GetString("scp343_spawn_console_msg", "Вы SCP 343\nВы сможете открывать двери через 2 минуты\nВы должны помочь выжить людям\nУ вас в инвентаре есть транквилизатор, используйте его с умом\nВыбросив патроны вы сможете перемещаться между игроками\nУдачи");
			Configs.spawnbcmsg = Plugin.Config.GetString("scp343_spawn_bc_msg", "<color=red>Вы SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>");
			Configs.spawndogbcmsg = Plugin.Config.GetString("scp682_spawn_bc", "<color=red>Вы заспавнены за SCP 682.</color>\n<color=red>Вы неуязвимая рептилия</color>\n<size=10>Магия рептилий</size>\nНаписан fydne");
			Configs.spawnbctime = Plugin.Config.GetUInt("scp343_spawn_bc_time", 10);
			Configs.spawndogbctime = Plugin.Config.GetUInt("scp682_spawn_bc_time", 10);
			Configs.nuke = Plugin.Config.GetBool("scp343_nuke_interact", false);
			Configs.repdogbcmsg = Plugin.Config.GetString("scp682_rep_bc", "<color=red>Вы заменили вышедшего SCP 682.</color>\n<color=red>Вы неуязвимая рептилия</color>\n<size=10>Магия рептилий</size>\nНаписан fydne");
			Configs.repdogbctime = Plugin.Config.GetUInt("scp682_rep_bc_time", 10);
			Configs.repbcmsg = Plugin.Config.GetString("scp343_rep_bc", "<color=red>Вы заменили вышедшего SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>");
			Configs.repbctime = Plugin.Config.GetUInt("scp343_rep_bc_time", 10);
			Configs.scpgodescapebc = Plugin.Config.GetString("scp343_escape_bc", "<color=red>SCP 343 сбежал!</color>");
			Configs.scpgodescapebctime = Plugin.Config.GetUInt("scp343_escape_bc_time", 10);
			Configs.scpgodescapecassie = Plugin.Config.GetString("scp343_escape_cassie", "scp 3 4 3 escape");
			Configs.scpgodripcassie = Plugin.Config.GetString("scp343_rip_cassie", "scp 3 4 3 CONTAINMENT MINUTE");
			Configs.scprepripcassie = Plugin.Config.GetString("scp682_rip_cassie", "scp 6 8 2 CONTAINMENT MINUTE");
			Configs.scpspawnripcassie = Plugin.Config.GetString("scp682_spawn_cassie", "ATTENTION TO ALL PERSONNEL ESCAPE SCP 6 8 2 ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B");
			Configs.errorinra = Plugin.Config.GetString("scp682-343_error_RA", "Please enter in an ID from a player that exists in the game!");
			Configs.sucinra682 = Plugin.Config.GetString("scp682_suc_RA", "okaay, (s)he SCP 682!");
			Configs.sucinra343 = Plugin.Config.GetString("scp343_suc_RA", "okaay, (s)he SCP 343!");
			Configs.pnf = Plugin.Config.GetString("scp343_players_not_found", "Игроки не найдены.");
			Configs.tt = Plugin.Config.GetString("scp343_tp_to", "Вы телепортированы к ");
			Configs.soon = Plugin.Config.GetString("scp343_soon", "Скоро...");
			Configs.warningTime = Plugin.Config.GetUInt("scp343_sleep_bc_time", 10);
			Configs.durmin = Plugin.Config.GetUInt("scp343_sleep_dur_min", 3);
			Configs.durmax = Plugin.Config.GetUInt("scp343_sleep_dur_max", 5);
			Configs.warningText = Plugin.Config.GetString("scp343_sleep_bc", "<color=red>Вас усыпили транквилизатором...</color>");

			Configs.tranqAmmo = Plugin.Config.GetInt("scp343_tgun_ammo", 11);
			Configs.noAmmoDuration = Plugin.Config.GetUInt("scp343_no_ammo_bc_time", 2);
			Configs.warningText = Plugin.Config.GetString("scp343_no_ammo_bc", "<color=red>Вам надо %ammo патрон для выстрела!</color>").Replace("%ammo", $"{tranqAmmo + 1}");



			Configs.dontaccess = Plugin.Config.GetString("scp343_door_dont_access", "<color=red>Вы сможете открывать двери через {0} секунд!</color>");
		}
	}
}
