using System;
using System.IO;
using EXILED;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Shop
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;
		public static string[] helperRoles;
		public static string[] goodRoles;
		public static string[] eliteRoles;
		public static string[] premiumRoles;
		public static string[] vipRoles;
		public static string[] vipplusRoles;
		public static string[] staRoles;
		public static string[] viphelperRoles;
		public static string[] adminRoles;
		public static string[] lgbtRoles;

		internal static Regex[] regices = new Regex[0];
		internal static string StatFilePath =
			Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"),
				"Shop");

		public override void OnEnable()
		{
			if (!Directory.Exists(StatFilePath))
				Directory.CreateDirectory(StatFilePath);
			
			EventHandlers = new EventHandlers(this);
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDeath;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.ConsoleCommandEvent += EventHandlers.OnConsoleCommand;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.PlayerSpawnEvent += EventHandlers.OnPlayerSpawn;
			helperRoles = Config.GetStringList("helper").ToArray();
			goodRoles = Config.GetStringList("good").ToArray();
			eliteRoles = Config.GetStringList("elite").ToArray();
			premiumRoles = Config.GetStringList("premium").ToArray();
			vipRoles = Config.GetStringList("vip").ToArray();
			vipplusRoles = Config.GetStringList("vipplus").ToArray();
			staRoles = Config.GetStringList("sta").ToArray();
			viphelperRoles = Config.GetStringList("viphelper").ToArray();
			adminRoles = Config.GetStringList("admin").ToArray();
			lgbtRoles = Config.GetStringList("lgbt").ToArray();
		}

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDeath;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.ConsoleCommandEvent -= EventHandlers.OnConsoleCommand;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.PlayerSpawnEvent -= EventHandlers.OnPlayerSpawn;
			EventHandlers = null;
		}

		public override void OnReload()
		{
			
		}

		public override string getName { get; } = "Player XP";
	}
}