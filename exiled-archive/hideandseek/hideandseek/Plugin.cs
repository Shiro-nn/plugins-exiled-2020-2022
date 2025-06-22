using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Warhead = Exiled.Events.Handlers.Warhead;
namespace hideandseek
{
	public class Massacre : Plugin<Config>
	{
		public Methods Functions { get; private set; }
		public EventHandlers EventHandlers { get; private set; }
		public Commands Command { get; private set; }
		public Random Gen = new Random();
		internal bool Ga = true;

		internal bool GamemodeEnabled;
		internal static bool GamemodEnabled;
		internal bool RoundStarted;





		internal bool sd;
		internal bool sp;
		internal bool rp;
		internal bool ew;
		public string RoleIDsToPing { get; set; } = "654616522513448960";
		public string Tessage { get; set; } = "fydne ff:on";
		public string jbc { get; set; } = "<color=red>������ ������� ����� <color=aqua>������</color></color>";
		public ushort jbct { get; set; } = 5;
		public string wpbc { get; set; } = "<color=red>��������� �������, �� ������ �������</color>";
		public ushort wpbct { get; set; } = 5;
		public string wbc { get; set; } = "<color=#ff5a00>%player%</color> <color=aqua>������� � ������!</color>\n<color=red>������ ���!</color>";
		public ushort wbct { get; set; } = 15;
		public ushort MaxPeanuts { get; set; } = 2;
		public long botid { get; set; } = 696670959410610247;
		public string webhook { get; set; } = "https://discordapp.com/api/webhooks/";
		public string ge { get; set; } = "����� �������";
		public string ae { get; set; } = "����-�����";
		public string c { get; set; } = "��������:";
		public string s { get; set; } = "������:";
		public string e { get; set; } = "�����:";
		public string has { get; set; } = "Hide and Seek";
		public string nr { get; set; } = "� ��������� ������";
		public string c1 { get; set; } = "has";
		public string c2 { get; set; } = "������";

		public override void OnEnabled()
		{
			EventHandlers = new EventHandlers(this);
			Functions = new Methods(this);
			Command = new Commands(this);

			Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
			Server.RoundStarted += EventHandlers.OnRoundStart;
			Server.RoundEnded += EventHandlers.OnRoundEnd;
			Server.SendingRemoteAdminCommand += Command.OnRaCommand;
			Player.Joined += EventHandlers.OnPlayerJoin;
			Player.InteractingDoor += EventHandlers.RunOnDoorOpen;
			Player.Died += EventHandlers.OnPlayerDie;
			Server.EndingRound += EventHandlers.OnCheckRoundEnd;
			Warhead.Stopping += EventHandlers.OnWarheadCancel;
		}

		public override void OnDisabled()
		{
			Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
			Server.RoundStarted -= EventHandlers.OnRoundStart;
			Server.RoundEnded -= EventHandlers.OnRoundEnd;
			Server.SendingRemoteAdminCommand -= Command.OnRaCommand;
			Player.Joined -= EventHandlers.OnPlayerJoin;
			Player.InteractingDoor -= EventHandlers.RunOnDoorOpen;
			Player.Died -= EventHandlers.OnPlayerDie;
			Server.EndingRound -= EventHandlers.OnCheckRoundEnd;
			Warhead.Stopping -= EventHandlers.OnWarheadCancel;
			EventHandlers = null;
			Functions = null;
			Command = null;
		}
	}
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public string RoleIDsToPing { get; set; } = "718176462846558238";
		public string Tessage { get; set; } = "fydne ff:on";
		public string jbc { get; set; } = "<color=red>������ ������� ����� <color=aqua>������</color></color>";
		public ushort jbct { get; set; } = 5;
		public string wpbc { get; set; } = "<color=red>��������� �������, �� ������ �������</color>";
		public ushort wpbct { get; set; } = 5;
		public string wbc { get; set; } = "<color=#ff5a00>%player%</color> <color=aqua>������� � ������!</color>\n<color=red>������ ���!</color>";
		public ushort wbct { get; set; } = 15;
		public ushort MaxPeanuts { get; set; } = 2;
		public long botid { get; set; } = 708338407138394192;
		public string webhook { get; set; } = "https://discordapp.com/api/webhooks/";
		public string ge { get; set; } = "����� �������";
		public string ae { get; set; } = "����-�����";
		public string c { get; set; } = "��������:";
		public string s { get; set; } = "������:";
		public string e { get; set; } = "�����:";
		public string has { get; set; } = "Hide and Seek";
		public string nr { get; set; } = "� ��������� ������";
		public string c1 { get; set; } = "has";
		public string c2 { get; set; } = "������";
	}
}