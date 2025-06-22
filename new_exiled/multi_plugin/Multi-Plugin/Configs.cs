using System.Collections.Generic;

namespace MultiPlugin
{
	internal static class Configs
	{

		internal static int spawnchance;
		internal static int spawnchance343;
		internal static float dmg;
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
		internal static ushort spawnbctime;
		internal static ushort spawndogbctime;
		internal static string spawnconsolemsg;
		internal static string spawnbcmsg;
		internal static string spawndogbcmsg;
		internal static bool nuke;
		internal static string safeuserbc;
		internal static string safeattackerbc;
		internal static string dooropenbc;
		internal static string repdogbcmsg;
		internal static ushort repdogbctime;
		internal static string repbcmsg;
		internal static ushort repbctime;
		internal static ushort scpgodescapebctime;
		internal static string scpgodescapebc;
		internal static string errorinra;
		internal static string sucinra682;
		internal static string sucinra343;
		internal static string scpgodescapecassie;
		internal static string scpgodripcassie;
		internal static string scprepripcassie;
		internal static string scpspawnripcassie;
		internal static string pnf;
		internal static string soon;
		internal static uint warningTime;
		internal static string warningText;
		internal static uint durmin;
		internal static uint durmax;
		internal static int tranqAmmo;
		internal static uint noAmmoDuration;
		internal static string dontaccess;
		internal static string banmsg;
		internal static string kickmsg;
		internal static string ban;
		internal static string kick;
		internal static string before;
		internal static string dontopen181bc;
		internal static ushort dontopen181bct;
		internal static string nf;
		internal static string tp;
		internal static string jh;

		internal static void ReloadConfig()
		{
			Configs.dooropenbc = Plugin.Config.GetString("mp_181_door_open_bc", "\n<color=#54ff00>SCP 181 открыл вам дверь</color>");
			Configs.safeuserbc = Plugin.Config.GetString("mp_181_safe_user_bc", "\n<color=#54ff00>Вам помог SCP 181</color>");
			Configs.safeattackerbc = Plugin.Config.GetString("mp_181_safe_attacker_bc", "\n<color=red>Этой жертве помог SCP 181</color>");
			Configs.dontopen181bc = Plugin.Config.GetString("mp_181_dont_open_bc", "<color=red>SCP 181 не любит войну</color>");
			Configs.dontopen181bct = Plugin.Config.GetUShort("mp_181_dont_open_bc_time", 5);
			Configs.health = Plugin.Config.GetInt("mp_682_health", 400);
			Configs.spawnchance = Plugin.Config.GetInt("mp_682_spawn_chance", 100);
			Configs.spawnchance343 = Plugin.Config.GetInt("mp_343_spawn_chance", 100);
			Configs.dmg = Plugin.Config.GetFloat("mp_682_dmg", 1000f);
			Configs.dsreplace = Plugin.Config.GetBool("mp_682_force_scp93953", true);
			Configs.log = Plugin.Config.GetBool("mp_getlog", false);
			Configs.moreoptions = Plugin.Config.GetBool("mp_more_options", false);
			Configs.bettercondition = Plugin.Config.GetBool("mp_better_ec", false);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("mp_682_scp_friendly_fire", false);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("mp_682_tutorial_friendly_fire", false);
			Configs.winWithTutorial = Plugin.Config.GetBool("mp_682_win_with_tutorial", false);
			Configs.initialCooldown = Plugin.Config.GetInt("mp_343_time_doors_open", 120);
			Configs.minpeople = Plugin.Config.GetInt("mp_343_min_users_for_spawn", 5);
			Configs.spawnconsolemsg = Plugin.Config.GetString("mp_343_spawn_console_msg", "Вы SCP 343\nВы сможете открывать двери через 2 минуты\nВы должны помочь выжить людям\nВыбросив 7.62 вы увеличитесь\nВыбросив 5.56 вы уменьшитесь\nВыбросив 9mm вы телепортируетесь к рандомному игроку\nУдачи");
			Configs.spawnbcmsg = Plugin.Config.GetString("mp_343_spawn_bc_msg", "<color=red>Вы SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>");
			Configs.spawndogbcmsg = Plugin.Config.GetString("mp_682_spawn_bc", "<color=red>Вы заспавнены за SCP 682.</color>\n<color=red>Вы неуязвимая рептилия</color>\n<size=10>Магия рептилий</size>\nНаписан fydne");
			Configs.spawnbctime = Plugin.Config.GetUShort("mp_343_spawn_bc_time", 10);
			Configs.spawndogbctime = Plugin.Config.GetUShort("mp_682_spawn_bc_time", 10);
			Configs.nuke = Plugin.Config.GetBool("mp_343_nuke_interact", false);
			Configs.repdogbcmsg = Plugin.Config.GetString("mp_682_rep_bc", "<color=red>Вы заменили вышедшего SCP 682.</color>\n<color=red>Вы неуязвимая рептилия</color>\n<size=10>Магия рептилий</size>\nНаписан fydne");
			Configs.repdogbctime = Plugin.Config.GetUShort("mp_682_rep_bc_time", 10);
			Configs.repbcmsg = Plugin.Config.GetString("mp_343_rep_bc", "<color=red>Вы заменили вышедшего SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>");
			Configs.repbctime = Plugin.Config.GetUShort("mp_343_rep_bc_time", 10);
			Configs.scpgodescapebc = Plugin.Config.GetString("mp_343_escape_bc", "<color=red>SCP 343 сбежал!</color>");
			Configs.scpgodescapebctime = Plugin.Config.GetUShort("mp_343_escape_bc_time", 10);
			Configs.scpgodescapecassie = Plugin.Config.GetString("mp_343_escape_cassie", "scp 3 4 3 escape");
			Configs.scpgodripcassie = Plugin.Config.GetString("mp_343_rip_cassie", "scp 3 4 3 CONTAINMENT MINUTE");
			Configs.scprepripcassie = Plugin.Config.GetString("mp_682_rip_cassie", "scp 6 8 2 CONTAINMENT MINUTE");
			Configs.scpspawnripcassie = Plugin.Config.GetString("mp_682_spawn_cassie", "ATTENTION TO ALL PERSONNEL ESCAPE SCP 6 8 2 ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B");
			Configs.errorinra = Plugin.Config.GetString("mp_682-343_error_RA", "Please enter in an ID from a player that exists in the game!");
			Configs.sucinra682 = Plugin.Config.GetString("mp_682_suc_RA", "okaay, (s)he SCP 682!");
			Configs.sucinra343 = Plugin.Config.GetString("mp_343_suc_RA", "okaay, (s)he SCP 343!");
			Configs.soon = Plugin.Config.GetString("mp_343_soon", "Скоро...");
			Configs.dontaccess = Plugin.Config.GetString("mp_343_door_dont_access", "<color=red>Вы сможете открывать двери через {0} секунд!</color>");
			Configs.kickmsg = Plugin.Config.GetString("mp_ban_msg", "Вы были забанены администратором %admin%. Причина: %reason%");
			Configs.banmsg = Plugin.Config.GetString("mp_kick_msg", "Вы были кикнуты администратором %admin%. Причина: %reason%");
			Configs.ban = Plugin.Config.GetString("mp_ban", "забанен");
			Configs.kick = Plugin.Config.GetString("mp_kick", "кикнут");
			Configs.before = Plugin.Config.GetString("mp_before", "до");
			Configs.nf = Plugin.Config.GetString("mp_not_found", "Player not found");
			Configs.tp = Plugin.Config.GetString("mp_343_tp", "You are teleported to %player%");
			Configs.jh = Plugin.Config.GetString("mp_join_hint", "<color=#00fffb>Добро пожаловать на сервер</color> <color=#ffa600>f</color><color=#ffff00>y<color=#1eff00>d</color><color=#0004ff>n</color><color=#9d00ff>e</color>\n<color=#09ff00>Если вам понравятся какие-нибудь плагины, то вы сможете купить их на сайте scpsl.store</color>");


			//Soon...
			//Configs.warningTime = Plugin.Config.GetUInt("mp_343_sleep_bc_time", 10);
			//Configs.durmin = Plugin.Config.GetUInt("mp_343_sleep_dur_min", 3);
			//Configs.durmax = Plugin.Config.GetUInt("mp_343_sleep_dur_max", 5);
			//Configs.warningText = Plugin.Config.GetString("mp_343_sleep_bc", "<color=red>Вас усыпили транквилизатором...</color>");
			//Configs.tranqAmmo = Plugin.Config.GetInt("mp_343_tgun_ammo", 11);
			//Configs.noAmmoDuration = Plugin.Config.GetUInt("mp_343_no_ammo_bc_time", 2);
			//Configs.warningText = Plugin.Config.GetString("mp_343_no_ammo_bc", "<color=red>Вам надо %ammo патрон для выстрела!</color>").Replace("%ammo", $"{tranqAmmo + 1}");
		}
	}
}
