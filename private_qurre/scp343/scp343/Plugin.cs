using MEC;
namespace scp343
{
	public class Plugin : Qurre.Plugin
	{
		public static bool Enabled { get; internal set; }
		#region nostatic
		public EventHandlers EventHandlers;
		#endregion
		#region override
		public override int Priority { get; } = 10000;
		public override string Developer { get; } = "fydne";
		public override string Name { get; } = "scp343";

		public override void Enable() => RegisterEvents();
		public override void Disable() => UnregisterEvents();
		#endregion
		#region RegEvents
		private void RegisterEvents()
		{
			Enabled = Config.GetBool("scp343_enable", true);
			if (!Enabled) return;
			Cfg.Reload();
			EventHandlers = new EventHandlers(this);
			Qurre.Events.Round.Start += EventHandlers.OnRoundStart;
			Qurre.Events.Round.End += EventHandlers.OnRoundEnd;
			Qurre.Events.Player.Dead += EventHandlers.OnPlayerDie;
			Qurre.Events.Player.Damage += EventHandlers.OnPlayerHurt;
			Qurre.Events.Scp096.Enrage += EventHandlers.scpzeroninesixe;
			Qurre.Events.Player.Shooting += EventHandlers.OnShoot;
			Qurre.Events.Player.Escape += EventHandlers.OnCheckEscape;
			Qurre.Events.Player.RoleChange += EventHandlers.OnSetClass;
			Qurre.Events.Player.Leave += EventHandlers.OnPlayerLeave;
			Qurre.Events.Scp106.Contain += EventHandlers.OnContain106;
			Qurre.Events.Scp106.PocketDimensionEnter += EventHandlers.OnPocketDimensionEnter;
			Qurre.Events.Scp106.PocketDimensionFailEscape += EventHandlers.OnPocketDimensionDie;
			Qurre.Events.Player.InteractDoor += EventHandlers.RunOnDoorOpen;
			Qurre.Events.Player.DroppingItem += EventHandlers.OnDropItem;
			Qurre.Events.Player.Cuff += EventHandlers.OnPlayerHandcuffed;
			Qurre.Events.Scp106.FemurBreakerEnter += EventHandlers.OnFemurEnter;
			Qurre.Events.Player.PickupItem += EventHandlers.OnPickupItem;
			Qurre.Events.Player.Spawn += EventHandlers.OnTeamRespawn;
			Qurre.Events.Alpha.Stopping += EventHandlers.OnWarheadCancel;
			Qurre.Events.Server.SendingRA += EventHandlers.ra;
			Qurre.Events.Player.MedicalUsing += EventHandlers.medical;
			Qurre.Events.Scp914.UpgradePlayer += EventHandlers.scp914;
			Qurre.Events.Player.InteractLocker += EventHandlers.OnLockerInteraction;
			Qurre.Events.Player.InteractGenerator += EventHandlers.OnGenOpen;
			Qurre.Events.Player.TeslaTrigger += EventHandlers.tesla;
			Timing.RunCoroutine(EventHandlers.time());
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			if (!Enabled) return;
			Qurre.Events.Round.Start -= EventHandlers.OnRoundStart;
			Qurre.Events.Round.End -= EventHandlers.OnRoundEnd;
			Qurre.Events.Player.Dead -= EventHandlers.OnPlayerDie;
			Qurre.Events.Player.Damage -= EventHandlers.OnPlayerHurt;
			Qurre.Events.Scp096.Enrage -= EventHandlers.scpzeroninesixe;
			Qurre.Events.Player.Shooting -= EventHandlers.OnShoot;
			Qurre.Events.Player.Escape -= EventHandlers.OnCheckEscape;
			Qurre.Events.Player.RoleChange -= EventHandlers.OnSetClass;
			Qurre.Events.Player.Leave -= EventHandlers.OnPlayerLeave;
			Qurre.Events.Scp106.Contain -= EventHandlers.OnContain106;
			Qurre.Events.Scp106.PocketDimensionEnter -= EventHandlers.OnPocketDimensionEnter;
			Qurre.Events.Scp106.PocketDimensionFailEscape -= EventHandlers.OnPocketDimensionDie;
			Qurre.Events.Player.InteractDoor -= EventHandlers.RunOnDoorOpen;
			Qurre.Events.Player.DroppingItem -= EventHandlers.OnDropItem;
			Qurre.Events.Player.Cuff -= EventHandlers.OnPlayerHandcuffed;
			Qurre.Events.Scp106.FemurBreakerEnter -= EventHandlers.OnFemurEnter;
			Qurre.Events.Player.PickupItem -= EventHandlers.OnPickupItem;
			Qurre.Events.Player.Spawn -= EventHandlers.OnTeamRespawn;
			Qurre.Events.Alpha.Stopping -= EventHandlers.OnWarheadCancel;
			Qurre.Events.Server.SendingRA -= EventHandlers.ra;
			Qurre.Events.Player.MedicalUsing -= EventHandlers.medical;
			Qurre.Events.Scp914.UpgradePlayer -= EventHandlers.scp914;
			Qurre.Events.Player.InteractLocker -= EventHandlers.OnLockerInteraction;
			Qurre.Events.Player.InteractGenerator -= EventHandlers.OnGenOpen;
			Qurre.Events.Player.TeslaTrigger -= EventHandlers.tesla;
			EventHandlers = null;
		}
		#endregion
	}
	public class Cfg
	{
		private static string Prefix = "scp343_";

		public static string sucinra343 { get; set; }
		public static string errorinra { get; set; }
		public static int initialCooldown { get; set; }
		public static string dontaccess { get; set; }
		public static string scpgodripcassie { get; set; }
		public static string scpgodescapecassie { get; set; }
		public static ushort scpgodescapebctime { get; set; }
		public static string scpgodescapebc { get; set; }
		public static int minpeople { get; set; }
		public static ushort repbctime { get; set; }
		public static string repbcmsg { get; set; }
		public static ushort spawnbctime { get; set; }
		public static string spawnbcmsg { get; set; }
		public static string spawnconsolemsg { get; set; }
		public static ushort mapspawnt { get; set; }
		public static string mapspawn { get; set; }
		public static int healDistance { get; set; }
		public static string wait { get; set; }
		public static string tranq { get; set; }
		public static string vtranq { get; set; }
		public static string light { get; set; }
		public static float light_off_duration { get; set; }
		public static string kep_on { get; set; }
		public static string kep_off { get; set; }
		public static string icom { get; set; }
		public static void Reload()
		{
			Cfg.sucinra343 = Plugin.Config.GetString(Prefix + "sucinra343", "океей, ты scp343");
			Cfg.errorinra = Plugin.Config.GetString(Prefix + "errorinra", "Игрок не найден");
			Cfg.initialCooldown = Plugin.Config.GetInt(Prefix + "initial_cooldown", 120);
			Cfg.dontaccess = Plugin.Config.GetString(Prefix + "dontaccess", "<b><color=red>Вы сможете открывать двери через {0} секунд!</color></b>");
			Cfg.scpgodripcassie = Plugin.Config.GetString(Prefix + "rip_cassie", "scp 3 4 3 CONTAINMENT MINUTE");
			Cfg.scpgodescapecassie = Plugin.Config.GetString(Prefix + "escape_cassie", "scp 3 4 3 escape");
			Cfg.scpgodescapebctime = Plugin.Config.GetUShort(Prefix + "escape_bc_time", 10);
			Cfg.scpgodescapebc = Plugin.Config.GetString(Prefix + "escape_bc", "<color=red>SCP 343 сбежал!</color>");
			Cfg.minpeople = Plugin.Config.GetUShort(Prefix + "min_people", 4);
			Cfg.repbctime = Plugin.Config.GetUShort(Prefix + "replace_bc_time", 10);
			Cfg.repbcmsg = Plugin.Config.GetString(Prefix + "replace_bc", "<color=red>Вы заменили вышедшего SCP 343.</color>\n<color=red>Больше информации в вашей консоли на `ё`</color>");
			Cfg.spawnbctime = Plugin.Config.GetUShort(Prefix + "spawn_bc_time", 10);
			Cfg.spawnbcmsg = Plugin.Config.GetString(Prefix + "spawn_bc", "<color=red>Вы заспавнились за SCP 343.</color>\n<color=red>Больше информации в вашей консоли на `ё`</color>");
			Cfg.spawnconsolemsg = Plugin.Config.GetString(Prefix + "spawn_console", "Вы появились за SCP 343 (Бога)\nВы сможете открывать двери через 2 минуты\n" +
				"Надев SCP 268 (кепку), вы станете невидимым\nВыбросив 9mm, вы телепортируетесь к рандомному игроку\n Выбросив аптечку, вы вылечите ближайшего игрока\n" +
				"Выбросив SCP 500, вы вылечите группу людей в 5 метрах от себя\nПоднимая оружие, вы делаете его аптечкой\nПоднимая гранаты, вы делаете их адреналином\n" +
				"Поднимая MICRO-HID, вы его заряжаете\nУдачи");
			Cfg.mapspawnt = Plugin.Config.GetUShort(Prefix + "map_spawn_bc_time", 10);
			Cfg.mapspawn = Plugin.Config.GetString(Prefix + "map_spawn_bc", "В раунде появился <color=red>SCP 343</color>");
			Cfg.healDistance = Plugin.Config.GetUShort(Prefix + "heal_distance", 5);
			Cfg.wait = Plugin.Config.GetString(Prefix + "wait", "<b><color=#ff0000>Подождите {0} секунд</color></b>");
			Cfg.tranq = Plugin.Config.GetString(Prefix + "tranq", "<b><color=#15ff00>Вы успешно усыпили %player%</color></b>");
			Cfg.vtranq = Plugin.Config.GetString(Prefix + "vtranq", "<b>Вас оглушил <color=red>SCP-343</color></b>");
			Cfg.light = Plugin.Config.GetString(Prefix + "light", "<b>Вы отключили свет.</b>");
			Cfg.light_off_duration = Plugin.Config.GetUShort(Prefix + "light_off_duration", 10);
			Cfg.kep_on = Plugin.Config.GetString(Prefix + "kep_on", "<b>Вы надели SCP 268</b>");
			Cfg.kep_off = Plugin.Config.GetString(Prefix + "kep_off", "<b>Вы сняли SCP 268</b>");
			Cfg.icom = Plugin.Config.GetString(Prefix + "icom", "<b>Вы перезарядили интерком</b>");
		}
	}
}