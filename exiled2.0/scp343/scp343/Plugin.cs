using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using MEC;
namespace scp343
{
	public class Plugin : Plugin<Config>
	{
		#region nostatic
		public EventHandlers EventHandlers;
		public Config config;
		internal static string s1;
		internal static string s2;
		internal static ushort u1;
		internal static ushort u2;
		internal static string c;
		#endregion
		#region override
		public override PluginPriority Priority { get; } = PluginPriority.Higher;
		public override string Author { get; } = "fydne";

		public override void OnEnabled()
		{
			base.OnEnabled();
		}
		public override void OnDisabled()
		{
			base.OnDisabled();
		}
		public override void OnRegisteringCommands()
		{
			base.OnRegisteringCommands();
			RegisterEvents();
		}
		public override void OnUnregisteringCommands()
		{
			base.OnUnregisteringCommands();

			UnregisterEvents();
		}
		#endregion
		#region RegEvents
		private void RegisterEvents()
		{
			EventHandlers = new EventHandlers(this);
			config = base.Config;
			s1 = base.Config.mapspawn;
			s2 = base.Config.spawnbcmsg;
			u1 = base.Config.mapspawnt;
			u2 = base.Config.spawnbctime;
			c = base.Config.spawnconsolemsg;
			ServerConsole.ReloadServerName();
			Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
			Exiled.Events.Handlers.Player.Died += EventHandlers.OnPlayerDie;
			Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnPlayerHurt;
			Exiled.Events.Handlers.Scp096.Enraging += EventHandlers.scpzeroninesixe;
			Exiled.Events.Handlers.Player.Shooting += EventHandlers.OnShoot;
			Exiled.Events.Handlers.Player.Escaping += EventHandlers.OnCheckEscape;
			Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnSetClass;
			Exiled.Events.Handlers.Player.Left += EventHandlers.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing += EventHandlers.OnContain106;
			Exiled.Events.Handlers.Player.EscapingPocketDimension += EventHandlers.OnPocketDimensionEnter;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
			Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers.RunOnDoorOpen;
			Exiled.Events.Handlers.Player.DroppingItem += EventHandlers.OnDropItem;
			Exiled.Events.Handlers.Player.Handcuffing += EventHandlers.OnPlayerHandcuffed;
			Exiled.Events.Handlers.Player.EnteringFemurBreaker += EventHandlers.OnFemurEnter;
			Exiled.Events.Handlers.Player.PickingUpItem += EventHandlers.OnPickupItem;
			Exiled.Events.Handlers.Player.Spawning += EventHandlers.OnTeamRespawn;
			Exiled.Events.Handlers.Warhead.Stopping += EventHandlers.OnWarheadCancel;
			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.ra;
			Exiled.Events.Handlers.Player.UsingMedicalItem += EventHandlers.medical;
			Exiled.Events.Handlers.Scp914.UpgradingItems += EventHandlers.scp914;
			Exiled.Events.Handlers.Player.InteractingLocker += EventHandlers.OnLockerInteraction;
			Exiled.Events.Handlers.Player.UnlockingGenerator += EventHandlers.OnGenOpen;
			Exiled.Events.Handlers.Player.TriggeringTesla += EventHandlers.tesla;
			Timing.RunCoroutine(EventHandlers.time());
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			Exiled.Events.Handlers.Player.Died -= EventHandlers.OnPlayerDie;
			Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnPlayerHurt;
			Exiled.Events.Handlers.Scp096.Enraging -= EventHandlers.scpzeroninesixe;
			Exiled.Events.Handlers.Player.Shooting -= EventHandlers.OnShoot;
			Exiled.Events.Handlers.Player.Escaping -= EventHandlers.OnCheckEscape;
			Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnSetClass;
			Exiled.Events.Handlers.Player.Left -= EventHandlers.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing -= EventHandlers.OnContain106;
			Exiled.Events.Handlers.Player.EscapingPocketDimension -= EventHandlers.OnPocketDimensionEnter;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
			Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers.RunOnDoorOpen;
			Exiled.Events.Handlers.Player.DroppingItem -= EventHandlers.OnDropItem;
			Exiled.Events.Handlers.Player.Handcuffing -= EventHandlers.OnPlayerHandcuffed;
			Exiled.Events.Handlers.Player.EnteringFemurBreaker -= EventHandlers.OnFemurEnter;
			Exiled.Events.Handlers.Player.PickingUpItem -= EventHandlers.OnPickupItem;
			Exiled.Events.Handlers.Player.Spawning -= EventHandlers.OnTeamRespawn;
			Exiled.Events.Handlers.Warhead.Stopping -= EventHandlers.OnWarheadCancel;
			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.ra;
			Exiled.Events.Handlers.Player.UsingMedicalItem -= EventHandlers.medical;
			Exiled.Events.Handlers.Scp914.UpgradingItems -= EventHandlers.scp914;
			Exiled.Events.Handlers.Player.InteractingLocker -= EventHandlers.OnLockerInteraction;
			Exiled.Events.Handlers.Player.UnlockingGenerator -= EventHandlers.OnGenOpen;
			Exiled.Events.Handlers.Player.TriggeringTesla -= EventHandlers.tesla;
			EventHandlers = null;
		}
		#endregion
	}
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public string sucinra343 { get; set; } = "океей, ты scp343";
		public string errorinra { get; set; } = "Игрок не найден";
		public int initialCooldown { get; set; } = 120;
		public string dontaccess { get; set; } = "<b><color=red>Вы сможете открывать двери через {0} секунд!</color></b>";
		public string scpgodripcassie { get; set; } = "scp 3 4 3 CONTAINMENT MINUTE";
		public string scpgodescapecassie { get; set; } = "scp 3 4 3 escape";
		public ushort scpgodescapebctime { get; set; } = 10;
		public string scpgodescapebc { get; set; } = "<color=red>SCP 343 сбежал!</color>";
		public int minpeople { get; set; } = 4;
		public string repbcmsg { get; set; } = "<color=red>Вы заменили вышедшего SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>";
		public ushort repbctime { get; set; } = 10;
		public ushort spawnbctime { get; set; } = 10;
		public string spawnconsolemsg { get; set; } = "Вы появились за SCP 343 (Бога)\nВы сможете открывать двери через 2 минуты\nНадев SCP 268 (кепку), вы станете невидимым\nВыбросив 9mm, вы телепортируетесь к рандомному игроку\n Выбросив аптечку, вы вылечите ближайшего игрока\nВыбросив SCP 500, вы вылечите группу людей в 5 метрах от себя\nПоднимая оружие, вы делаете его аптечкой\nПоднимая гранаты, вы делаете их адреналином\nПоднимая MICRO-HID, вы его заряжаете\nУдачи";
		public string spawnbcmsg { get; set; } = "<color=red>Вы заспавнились за SCP 343.</color>\n <color=red>Больше информации в вашей консоли на `ё`</color>";
		public string mapspawn { get; set; } = "В раунде появился <color=red>SCP 343</color>";
		public ushort mapspawnt { get; set; } = 10;
		public int healDistance { get; set; } = 5;
		public string wait { get; set; } = "<b><color=#ff0000>Подождите {0} секунд</color></b>";
		public string tranq { get; set; } = "<b><color=#15ff00>Вы успешно усыпили %player%</color></b>";
		public string vtranq { get; set; } = "<b>Вас оглушил <color=red>SCP-343</color></b>";
		public string light { get; set; } = "<b>Вы отключили свет.</b>";
		public float light_off_duration { get; set; } = 10;
		public string kep_on { get; set; } = "<b>Вы надели SCP 268</b>";
		public string kep_off { get; set; } = "<b>Вы сняли SCP 268</b>";
		public string icom { get; set; } = "<b>Вы перезарядили интерком</b>";
	}
}