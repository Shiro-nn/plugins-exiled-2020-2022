using System.Collections.Generic;

namespace scp343
{
	internal static class Configs
	{
		internal static string sucinra343;
		internal static string errorinra;
		internal static bool nuke;
		internal static int initialCooldown;
		internal static string dontaccess;
		internal static string scpgodripcassie;
		internal static string scpgodescapecassie;
		internal static ushort scpgodescapebctime;
		internal static string scpgodescapebc;
		internal static int minpeople;
		internal static string repbcmsg;
		internal static ushort repbctime;
		internal static int health;
		internal static ushort spawnbctime;
		internal static string spawnconsolemsg;
		internal static string spawnbcmsg;
		internal static string kickmsg;
		internal static string banmsg;
		internal static string ban;
		internal static string kick;
		internal static string before;
		internal static ushort kickbant;
		internal static string mapspawn;
		internal static ushort mapspawnt;
		internal static float healDistance;
		internal static float tranqdur;
		internal static string wait;
		internal static string tranq;
		internal static string vtranq;
		internal static int startalpha;
		internal static string autoabc;
		internal static ushort autoabct;

		internal static void ReloadConfig()
		{
			Configs.sucinra343 = Plugin.Config.GetString("scp343_suc_RA", "океей, ты scp343!");
			Configs.errorinra = Plugin.Config.GetString("scp343_error_RA", "Игрок не найден!");
			Configs.nuke = Plugin.Config.GetBool("scp343_nuke_interact", false);
			Configs.initialCooldown = Plugin.Config.GetInt("scp343_time_doors_open", 120);
			Configs.dontaccess = Plugin.Config.GetString("scp343_door_dont_access", "<color=red>Вы сможете открывать двери через {0} секунд!</color>");
			Configs.scpgodripcassie = Plugin.Config.GetString("scp343_rip_cassie", "scp 3 4 3 CONTAINMENT MINUTE");
			Configs.scpgodescapecassie = Plugin.Config.GetString("scp343_escape_cassie", "scp 3 4 3 escape");
			Configs.scpgodescapebctime = Plugin.Config.GetUShort("scp343_escape_bc_time", 10);
			Configs.scpgodescapebc = Plugin.Config.GetString("scp343_escape_bc", "<color=red>БОГ (SCP 343) сбежал!</color>");
			Configs.minpeople = Plugin.Config.GetInt("scp343_min_user", 4);
			Configs.repbcmsg = Plugin.Config.GetString("scp343_rep_bc", "<color=red>Вы заменили вышедшего БОГА (SCP 343).</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>");
			Configs.repbctime = Plugin.Config.GetUShort("scp343_rep_bc_time", 10);
			Configs.health = Plugin.Config.GetInt("scp343_health", 777);
			Configs.spawnbctime = Plugin.Config.GetUShort("scp343_spawn_bc_time", 10);
			Configs.spawnconsolemsg = Plugin.Config.GetString("scp343_spawn_console_msg", "Вы заспавнились за БОГА (SCP 343)\n Вы сможете открывать двери через 2 минуты\n Выбросив 7.62 вы станете невидимым\n Выбросив 9mm вы телепортируетесь к рандомному игроку\n Выбросив аптечку вы вылечите ближайшего игрока\n Выбросив SCP 500 вы вылечите группу людей в 5 метрах от себя\n Удачи");
			Configs.spawnbcmsg = Plugin.Config.GetString("scp343_spawn_bc_msg", "<color=red>Вы заспавнились за БОГА (SCP 343).</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>");
			Configs.kickmsg = Plugin.Config.GetString("scp343_ban_msg", "Вы были забанены администратором %admin%. Причина: %reason%");
			Configs.banmsg = Plugin.Config.GetString("scp343_kick_msg", "Вы были кикнуты администратором %admin%. Причина: %reason%");
			Configs.ban = Plugin.Config.GetString("scp343_ban", "забанен");
			Configs.kick = Plugin.Config.GetString("scp343_kick", "кикнут");
			Configs.before = Plugin.Config.GetString("scp343_before", "до");
			Configs.kickbant = Plugin.Config.GetUShort("scp343_kick_ban_bc_time", 10);
			Configs.mapspawn = Plugin.Config.GetString("scp343_spawn_bc_map", "В раунде появился <color=red>БОГ (SCP 343)</color>");
			Configs.mapspawnt = Plugin.Config.GetUShort("scp343_spawn_bc_map_time", 10);
			Configs.healDistance = Plugin.Config.GetFloat("scp343_heal_distance", 5);
			Configs.tranqdur = Plugin.Config.GetFloat("scp343_tranq_dur", 5);
			Configs.wait = Plugin.Config.GetString("scp343_wait_bc_map", "Подождите {0} секунд");
			Configs.tranq = Plugin.Config.GetString("scp343_tranq", "Вы успешно усыпили %player%");
			Configs.vtranq = Plugin.Config.GetString("scp343_victim_tranq", "Вас оглушил <color=red>Бог (SCP-343)</color>");
			Configs.startalpha = Plugin.Config.GetInt("scp343_auto_warhead_start_sec", 1500);
			Configs.autoabc = Plugin.Config.GetString("scp343_alpha_start_bc_map", "<color=red>Запущена авто-боеголовка</color>");
			Configs.autoabct = Plugin.Config.GetUShort("scp343_alpha_start_bc_map_time", 10);
		}
	}
}