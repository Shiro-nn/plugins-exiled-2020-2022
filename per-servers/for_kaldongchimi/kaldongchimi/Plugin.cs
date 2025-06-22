using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace kaldongchimi
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;
		public Random Gen = new Random();
		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;
		private bool enabled;
		public static string[] owner;
		public override void OnEnable()
		{
			enabled = Config.GetBool("kaldongchimi_enabled", true);
			if (!enabled) return;
			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"kaldongchimi{harmonyCounter}");
			harmonyInstance.PatchAll();
			EventHandlers = new EventHandlers(this);
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.ShootEvent += EventHandlers.Shoot;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.ShootEvent += EventHandlers.Shot;
			Events.CheckRoundEndEvent += EventHandlers.CheckRoundEnd;
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.RoundEndEvent += EventHandlers.RoundEnd;
			Events.WarheadCancelledEvent += EventHandlers.WarheadCancel;
			Events.PlayerDeathEvent += EventHandlers.PlayerDeath;
			owner = Config.GetStringList("owner").ToArray();//owner role
		}
		public override void OnDisable()
		{
			Events.RemoteAdminCommandEvent -= EventHandlers.RunOnRACommandSent;
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.ShootEvent -= EventHandlers.Shoot;
			Events.ShootEvent -= EventHandlers.Shot;
			Events.CheckRoundEndEvent -= EventHandlers.CheckRoundEnd;
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.RoundEndEvent -= EventHandlers.RoundEnd;
			Events.WarheadCancelledEvent -= EventHandlers.WarheadCancel;
			Events.PlayerDeathEvent -= EventHandlers.PlayerDeath;
			EventHandlers = null;
		}

		public override void OnReload()
		{
			Events.RemoteAdminCommandEvent -= EventHandlers.RunOnRACommandSent;
			Events.ShootEvent -= EventHandlers.OnShoot;
			Events.ShootEvent -= EventHandlers.Shoot;
			Events.ShootEvent -= EventHandlers.Shot;
			Events.CheckRoundEndEvent -= EventHandlers.CheckRoundEnd;
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.RoundEndEvent -= EventHandlers.RoundEnd;
			Events.WarheadCancelledEvent -= EventHandlers.WarheadCancel;
			Events.PlayerDeathEvent -= EventHandlers.PlayerDeath;
			enabled = Config.GetBool("kaldongchimi_enabled", true);
			if (!enabled) return;
			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"kaldongchimi{harmonyCounter}");
			harmonyInstance.PatchAll();
			EventHandlers = new EventHandlers(this);
			Events.ShootEvent += EventHandlers.OnShoot;
			Events.ShootEvent += EventHandlers.Shoot;
			Events.RemoteAdminCommandEvent += EventHandlers.RunOnRACommandSent;
			Events.ShootEvent += EventHandlers.Shot;
			Events.CheckRoundEndEvent += EventHandlers.CheckRoundEnd;
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.RoundEndEvent += EventHandlers.RoundEnd;
			Events.WarheadCancelledEvent += EventHandlers.WarheadCancel;
			Events.PlayerDeathEvent += EventHandlers.PlayerDeath;
			owner = Config.GetStringList("owner").ToArray();//owner role
		}

		public override string getName { get; } = "kaldongchimi";
	}
}
