using EXILED;
using Harmony;
using System;
using System.IO;
namespace FORCECLASS
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;
		public Random Gen = new Random();
		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;
		private bool enabled;
		public static string[] premiumRoles;
		public static string[] vipRoles;
		public static string[] vipplusRoles;
		public static string[] staRoles;

		public override void OnEnable()
		{
			enabled = Config.GetBool("force_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"FORCECLASS{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.RemoteAdminCommandEvent += EventHandlers.RemoteAdminCommand;
			Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
			premiumRoles = Config.GetStringList("premium").ToArray();
			vipRoles = Config.GetStringList("vip").ToArray();
			vipplusRoles = Config.GetStringList("vipplus").ToArray();
			staRoles = Config.GetStringList("sta").ToArray();
		}
		public override void OnDisable()
		{
			// Unregister events
			Events.RemoteAdminCommandEvent -= EventHandlers.RemoteAdminCommand;
			Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;

			EventHandlers = null;
		}

		public override void OnReload() 
		{
		}

		public override string getName { get; } = "FORCECLASS";
	}
}
