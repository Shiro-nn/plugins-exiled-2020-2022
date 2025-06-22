using System.Collections.Generic;

namespace MongoDB.scp343
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
		internal static string mapspawn;
		internal static ushort mapspawnt;
		internal static float healDistance;
		internal static float tranqdur;
		internal static string wait;
		internal static string tranq;
		internal static string vtranq;
		internal static int startalpha = 1500;
		internal static string autoabc;
		internal static ushort autoabct;

		internal static void ReloadConfig()
		{
			Configs.sucinra343 = "океей, ты scp343!";
			Configs.errorinra = "Игрок не найден!";
			Configs.nuke = false;
			Configs.initialCooldown = 120;
			Configs.dontaccess = "<b><color=red>Вы сможете открывать двери через {0} секунд!</color></b>";
			Configs.scpgodripcassie = "scp 3 4 3 CONTAINMENT MINUTE";
			Configs.scpgodescapecassie = "scp 3 4 3 escape";
			Configs.scpgodescapebctime = 10;
			Configs.scpgodescapebc = "<color=red>SCP 343 сбежал!</color>";
			Configs.minpeople = 4;
			Configs.repbcmsg = "<color=red>Вы заменили вышедшего SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>";
			Configs.repbctime = 10;
			Configs.health = 777;
			Configs.spawnbctime = 10;
			Configs.spawnconsolemsg = "Вы заспавнились за SCP 343" +
				"\n" +
				"Вы сможете открывать двери через 2 минуты" +
				"\n" +
				"Выбросив 9mm-вы телепортируетесь к рандомному игроку" +
				"\n" +
				"Выбросив обезболивающее-вы вылечите ближайшего игрока" +
				"\n" +
				"Выбросив аптечку-вы вылечите группу людей в 5 метрах от себя" +
				"\n" +
				"Выбросив SCP 500-вы оживите труп" +
				"\n" +
				"Удачи";
			Configs.spawnbcmsg = "<color=red>Вы заспавнились за SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>";
			Configs.mapspawn = "В раунде появился <color=red>SCP 343</color>";
			Configs.mapspawnt = 10;
			Configs.healDistance = 5;
			Configs.tranqdur = 5;
			Configs.wait = "<b><color=#ff0000>Подождите {0} секунд</color></b>";
			Configs.tranq = "<b><color=#15ff00>Вы успешно усыпили %player%</color></b>";
			Configs.vtranq = "<b>Вас оглушил <color=red>SCP-343</color></b>";
			Configs.startalpha = 1500;
			Configs.autoabc = "<size=25%><color=#6f6f6f>По приказу совета О5 запущена <color=red>Альфа Боеголовка</color></color></size>";
			Configs.autoabct = 10;
		}
	}
}