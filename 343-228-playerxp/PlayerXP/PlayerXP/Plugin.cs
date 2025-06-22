using System;
using System.IO;
using EXILED;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace PlayerXP
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		internal static Regex[] regices = new Regex[0];
		internal static string StatFilePath =
			Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"),
				"PlayerXP");

		public override void OnEnable()
		{
			if (!Directory.Exists(StatFilePath))
				Directory.CreateDirectory(StatFilePath);
			
			EventHandlers = new EventHandlers(this);
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.PlayerSpawnEvent += EventHandlers.OnPlayerSpawn;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDeath;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.ConsoleCommandEvent += EventHandlers.OnConsoleCommand;
			Configs.ReloadConfig();
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerSpawnEvent -= EventHandlers.OnPlayerSpawn;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDeath;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.ConsoleCommandEvent -= EventHandlers.OnConsoleCommand;
			EventHandlers = null;
		}

		public override void OnReload()
		{
			
		}

		public override string getName { get; } = "Player XP";
	}
}