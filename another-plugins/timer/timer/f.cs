using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using HarmonyLib;
using MEC;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace timer
{
	public partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		private int time = 0;
		private string squad = "";
		public void OnRoundStart()
		{
			Coroutines.Add(Timing.RunCoroutine(spawns()));
			Coroutines.Add(Timing.RunCoroutine(bc()));
		}
		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
		}
		public IEnumerator<float> spawns()
		{
			for (; ; )
			{
				int time_static = UnityEngine.Random.Range(Configs.min_spawn_time, Configs.max_spawn_time);
				time = time_static;
				spawn(time_static);
				yield return Timing.WaitForSeconds(time);
			}
		}
		public void spawn(int time_static)
		{
			if (Round.IsStarted)
			{
				int random = UnityEngine.Random.Range(0, 100);
				int m = Configs.cc + Configs.mc;
				int s = Configs.cc + Configs.mc + Configs.sc;
				if (Configs.cc >= random)
				{
					squad = Configs.chaos;
					spawnci(time_static - 15);
				}
				else if (m >= random)
				{
					squad = Configs.mtf;
					spawnmtf(time_static - 15);
				}
				else if (s >= random)
				{
					squad = Configs.sh;
					spawnsh(time_static);
                }
                else
                {
					spawn(time_static);

				}
			}
		}
		public void spawnci(int time_static)
		{
			bool yes = false;
			Timing.CallDelayed(time_static, () =>
			{
				if (!yes)
				{
					yes = true;
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
					bool yes1 = false;
					Timing.CallDelayed(15f, () => { if (!yes1) { yes1 = true; RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency); } });
				}
			});
		}
		public void spawnmtf(int time_static)
		{
			bool yes = false;
			Timing.CallDelayed(time_static, () =>
			{
				if (!yes)
				{
					yes = true;
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
					bool yes1 = false;
					Timing.CallDelayed(15f, () => { if (!yes1) { yes1 = true; RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox); } });
				}
			});
		}
		public void spawnsh(int time_static)
		{
			bool yes = false;
			Timing.CallDelayed(time_static, () =>
			{
				if (!yes)
				{
					yes = true;
					SerpentsHand.API.SerpentsHand.SpawnSquad(Configs.smsc);
				}
			});
		}
		public IEnumerator<float> bc()
		{
			for (; ; )
			{
				time--;
				if(time > 0)
				{
					TimeSpan times = TimeSpan.FromSeconds(time);
					string str = times.ToString(@"hh\:mm\:ss\:fff");
					foreach (Player spec in Player.List.Where(x => x.Role == RoleType.Spectator))
					{
						spec.ClearBroadcasts();
						spec.Broadcast(2, Configs.bc.Replace("{squad}", squad).Replace("{time}", str));
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
	internal static class Configs
	{
		internal static int min_spawn_time;
		internal static int max_spawn_time;
		internal static bool spe;
		internal static string bc;
		internal static string mtf;
		internal static string chaos;
		internal static string sh;
		internal static int smsc;
		internal static int cc;
		internal static int mc;
		internal static int sc;

		internal static void ReloadConfig(Config Config)
		{
			Configs.min_spawn_time = Config.min_spawn_time;
			Configs.max_spawn_time = Config.max_spawn_time;
			Configs.spe = Config.spe;
			Configs.bc = Config.bc;
			Configs.mtf = Config.mtf;
			Configs.chaos = Config.chaos;
			Configs.sh = Config.sh;
			Configs.smsc = Config.smsc;
			Configs.cc = Config.cc;
			Configs.mc = Config.mc;
			Configs.sc = Config.sc;
		}
	}
}

namespace timer
{
    using System.ComponentModel;
    using System.Text.RegularExpressions;
	using Exiled.API.Enums;
	using Exiled.API.Features;
	using Exiled.API.Interfaces;
	public class Plugin : Plugin<Config>
	{
		#region nostatic
		public EventHandlers EventHandlers;
		private Harmony hInstance;
		#endregion
		#region override
		public override PluginPriority Priority { get; } = PluginPriority.Low;
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
			this.hInstance = new Harmony("fydne.mongodb");
			this.hInstance.PatchAll();
			EventHandlers = new EventHandlers(this);
			Configs.ReloadConfig(base.Config);
			ServerConsole.ReloadServerName();
			Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
		}
		#endregion
		#region UnregEvents
		private void UnregisterEvents()
		{
			this.hInstance.UnpatchAll(null);
			Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			EventHandlers = null;
		}
		#endregion
	}
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		[Description("minimum spawn time(sec)")]
		public int min_spawn_time { get; set; } = 120;
		[Description("maximum spawn time(sec)")]
		public int max_spawn_time { get; set; } = 180;
		[Description("SerpentsHand enabled?")]
		public bool spe { get; set; } = false;
		[Description("bc")]
		public string bc { get; set; } = "next squad - {squad}\n They will arrive in {time}";
		[Description("mtf squad")]
		public string mtf { get; set; } = "mtf";
		[Description("chaos squad")]
		public string chaos { get; set; } = "CI";
		[Description("SerpentsHand squad")]
		public string sh { get; set; } = "SerpentsHand";
		[Description("sh max squad count")]
		public int smsc { get; set; } = 10;
		[Description("chaos chance")]
		public int cc { get; set; } = 33;
		[Description("mtf chance")]
		public int mc { get; set; } = 33;
		[Description("SerpentsHand chance")]
		public int sc { get; set; } = 33;
	}
}
namespace timer.Patches
{
#pragma warning disable SA1313
	using HarmonyLib;

	/// <summary>
	/// Patch the <see cref="ServerConsole.ReloadServerName"/>.
	/// </summary>
	[HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.ReloadServerName))]
	internal static class ServerNamePatch
	{
		private static void Postfix()
		{
			bool del = false;
			string[] spearator = { "<color=#00000000>" };
			string[] strlist = ServerConsole._serverName.Split(spearator, 2,
				   System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in strlist)
			{
				if (del)
				{
					ServerConsole._serverName = ServerConsole._serverName.Replace(s, "").Replace("<color=#00000000>", "");
				}
				del = true;
			}
			ServerConsole._serverName += $"<color=#00000000><size=1>Qurre 1.0.6</size></color>";
			ServerConsole.AddLog("Server name Updated", System.ConsoleColor.Gray);
		}
	}
}