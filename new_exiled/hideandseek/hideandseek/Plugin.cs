using System;
using EXILED;

namespace hideandseek
{
	public class Massacre : Plugin
	{
		public Methods Functions { get; private set; }
		public EventHandlers EventHandlers { get; private set; }
		public Commands Commands { get; private set; }
		public Random Gen = new Random();
		MTFRespawn CASSIE;
		public bool Enabled;
		public int MaxPeanuts;
		public string RoleIDsToPing;
		public string Tessage;

		internal bool GamemodeEnabled;
		internal static bool GamemodEnabled;
		internal bool RoundStarted;
		internal bool sd;
		internal bool sp;
		internal bool rp;
		internal bool ew;
		internal bool Ga = true;
		internal string jbc;
		internal ushort jbct;
		internal string wpbc;
		internal ushort wpbct;
		internal string wbc;
		internal ushort wbct;
		internal long botid;
		internal string webhook;
		internal string ge;
		internal string ae;
		internal string c;
		internal string s;
		internal string e;
		internal string has;
		internal string nr;
		internal string c1;
		internal string c2;

		public override void OnEnable()
		{
			ReloadConfig();
			if (!Enabled)
				return;

			EventHandlers = new EventHandlers(this);
			Functions = new Methods(this);
			Commands = new Commands(this);

			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RemoteAdminCommandEvent += Commands.OnRaCommand;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			Events.DoorInteractEvent += EventHandlers.RunOnDoorOpen;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.WarheadCancelledEvent += EventHandlers.OnWarheadCancel;
			Events.TriggerTeslaEvent += EventHandlers.OnTriggerTesla;
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.RemoteAdminCommandEvent -= Commands.OnRaCommand;
			Events.DoorInteractEvent -= EventHandlers.RunOnDoorOpen;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.WarheadCancelledEvent -= EventHandlers.OnWarheadCancel;
			Events.TriggerTeslaEvent -= EventHandlers.OnTriggerTesla;
			EventHandlers = null;
			Functions = null;
			Commands = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "has";
		public void cassieMessage(string message)
		{
			if (CASSIE == null)
				CASSIE = PlayerManager.localPlayer.GetComponent<MTFRespawn>();
			CASSIE.RpcPlayCustomAnnouncement(message, false, true);
		}
		public void ReloadConfig()
		{
			RoleIDsToPing = Config.GetString("has_roleids_ping", "718176462846558238");
			Tessage = Config.GetString("server", "fydne ff:on");
			jbc = Config.GetString("has_join_bc", "<color=red>������ ������� ����� <color=aqua>������</color></color>");
			jbct = Config.GetUShort("has_join_bc_time", 5);
			wpbc = Config.GetString("has_win_player_bc", "<color=red>��������� �������, �� ������ �������</color>");
			wpbct = Config.GetUShort("has_win_player_bc_time", 10);
			wbc = Config.GetString("has_win_map_bc", "<color=#ff5a00>%player%</color> <color=aqua>������� � ������!</color>\n<color=red>������ ���!</color>");
			wbct = Config.GetUShort("has_win_map_bc_time", 15);
			Enabled = Config.GetBool("has_enabled", true);
			MaxPeanuts = Config.GetInt("has_max_nuts", 2);
			botid = Config.GetLong("has_bot_id", 708338407138394192);
			webhook = Config.GetString("has_webhook", "https://discordapp.com/api/webhooks/-");
			ge = Config.GetString("has_event_enabled", "����� �������");
			ae = Config.GetString("has_auto_event", "����-�����");
			c = Config.GetString("has_conducts", "��������:");
			s = Config.GetString("has_server", "������:");
			e = Config.GetString("has_event", "�����:");
			has = Config.GetString("has_has", "Hide and Seek");
			nr = Config.GetString("has_next_round", "� ��������� ������");
			c1 = Config.GetString("has_command_1", "has");
			c2 = Config.GetString("has_command_2", "������");
		}
	}
}