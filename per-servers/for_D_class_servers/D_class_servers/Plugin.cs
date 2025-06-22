using EXILED;
using Harmony;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MEC;

namespace D_class_servers
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;
		public Random Gen = new Random();
		public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;
		private bool enabled;
		public static string[] owner;
		public bool Scp173Healing = true;
		public int Scp173HealAmount = 150;
		public bool Scp049Healing = true;
		public int Scp049HealAmount = 25;
		public float Scp049HealPow = 1.25f;
		public bool Scp0492Healing = true;
		public int Scp0492HealAmount = 25;
		public bool Scp106Healing = true;
		public int Scp106HealAmount = 75;
		public bool Scp096Healing = true;
		public int Scp096Heal = 150;
		public bool Scp939Healing = true;
		public int Scp939Heal = 125;
		public static Plugin Instance { private set; get; }
		public override void OnEnable()
		{
			enabled = Config.GetBool("D_class_servers_enabled", true);
			if (!enabled) return;
			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"D_class_servers{harmonyCounter}");
			harmonyInstance.PatchAll();
			EventHandlers = new EventHandlers(this);
			Events.RoundStartEvent += EventHandlers.RoundStart;
			Events.RoundEndEvent += EventHandlers.RoundEnd;


			Events.AnnounceNtfEntranceEvent += EventHandlers.bc1;
			Events.TeamRespawnEvent += EventHandlers.bc2;
			Events.CheckRoundEndEvent += EventHandlers.bc3;
			Events.PlayerDeathEvent += EventHandlers.bc4;
			Events.PlayerLeaveEvent += EventHandlers.bc5;
			Events.AnnounceDecontaminationEvent += EventHandlers.bc6;
			Events.PlayerJoinEvent += EventHandlers.bc7;
			Events.RoundEndEvent += EventHandlers.bc8;
			Events.GeneratorFinishedEvent += EventHandlers.bc9;
			Events.WarheadStartEvent += EventHandlers.bc10;
			Events.WarheadCancelledEvent += EventHandlers.bc11;
			Events.RoundStartEvent += EventHandlers.bc12;
			Events.Scp106ContainEvent += EventHandlers.bc13;
			Events.WarheadDetonationEvent += EventHandlers.OnWarheadDetonation;
			Events.RoundStartEvent += EventHandlers.RoundStart2;
			Events.Scp096CalmEvent += EventHandlers.OnCalm;
			Events.Scp096EnrageEvent += EventHandlers.OnEnrage;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDeath; 
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDeath;
			Configs.initialCooldown = Plugin.Config.GetInt("d_time_106_containment", 120);
			Configs.newmtfsquad = Plugin.Config.GetString("d_new_mtf_squad_bc", "Arrived MTF squad %NtfLetter%%NtfNumber% SCP left %ScpsLeft%");
			Configs.newmtfsquadt = Plugin.Config.GetUInt("d_new_mtf_squad_bc_time", 5);
			Configs.newchaossquad = Plugin.Config.GetString("d_new_chaos_squad_bc", "Chaos!");
			Configs.newchaossquadt = Plugin.Config.GetUInt("d_new_chaos_squad_bc_time", 5);
			Configs.plastm = Plugin.Config.GetString("d_map_last_player_bc", "%user%(%user.role%) last!");
			Configs.plastmt = Plugin.Config.GetUInt("d_map_last_player_bc_time", 5);
			Configs.plastp = Plugin.Config.GetString("d_player_last_player_bc", "You last!");
			Configs.plastpt = Plugin.Config.GetUInt("d_player_last_player_bc_time", 5);
			Configs.d = Plugin.Config.GetString("d_dclass", "D-class");
			Configs.sc = Plugin.Config.GetString("d_scientist", "Scientist");
			Configs.ci = Plugin.Config.GetString("d_chaos", "Chaos");
			Configs.t = Plugin.Config.GetString("d_tut", "Tutorial");
			Configs.plt = Plugin.Config.GetUInt("d_scp_leave_bc_time", 5);
			Configs.pl = Plugin.Config.GetString("d_scp_leave_bc", "SCP %user.role%(%user%) leave!");
			Configs.lct = Plugin.Config.GetUInt("d_lcz_containment_bc_time", 5);
			Configs.lc = Plugin.Config.GetString("d_lcz_containment_bc", "LCZ containment across %time% min!");
			Configs.flct = Plugin.Config.GetUInt("d_first_lcz_containment_bc_time", Configs.lct);
			Configs.flc = Plugin.Config.GetString("d_first_lcz_containment_bc", "<color=rainbow>LCZ containment across %time% min!</color>");
			Configs.llct = Plugin.Config.GetUInt("d_last_lcz_containment_bc_time", Configs.lct);
			Configs.llc = Plugin.Config.GetString("d_last_lcz_containment_bc", "LCZ containment across %time% sec!");
			Configs.dontaccess = Plugin.Config.GetString("d_scp106_containment_bc", "<color=red>Please wait {0} sec!</color>");
			Configs.wmt = Plugin.Config.GetUInt("d_welcome_bc_time", 5);
			Configs.wm = Plugin.Config.GetString("d_welcome_bc", "weeelcome to Class-D server!\n%players.Count% players\nRound duration: %round.time% min");
			Configs.gat = Plugin.Config.GetUInt("d_gen_act_bc_time", 5);
			Configs.ga = Plugin.Config.GetString("d_gen_act_bc", "Generator %gen.activated%/5 activated (%Generator.Room% room)");
			Configs.wat = Plugin.Config.GetUInt("d_warhead_start_bc_time", 5);
			Configs.wa = Plugin.Config.GetString("d_warhead_start_bc", "%player%(%player.role%) activated Alpha Warhead!");
			Configs.wdt = Plugin.Config.GetUInt("d_warhead_cancel_bc_time", 5);
			Configs.wd = Plugin.Config.GetString("d_warhead_cancel_bc", "%player%(%player.role%) desabled Alpha Warhead!");
			Configs.asd = Plugin.Config.GetString("d_all_scp", "All scp:");
			Configs.rst = Plugin.Config.GetUInt("d_round_start_bc_time", 5);
			Configs.killerdur = Plugin.Config.GetUInt("d_death_bc_time", 5);
			Configs.killer = Plugin.Config.GetString("d_death_bc", "<size=20><color=aqua>%killer.name%</color><color=red>(</color><color=aqua>%killer.role%</color><color=red>)(%killer.id%) killed</color> <color=aqua>%player.name%</color><color=red>(</color><color=aqua>%player.role%</color><color=red>) player %handcuffer%</color></size>");
			Configs.csdur = Plugin.Config.GetUInt("d_scp_containment_bc_time", 5);
			Configs.cs = Plugin.Config.GetString("d_scp_containment_bc", "<size=20><color=aqua>%killer.name%</color><color=red>(</color><color=aqua>%killer.role%</color><color=red>) containment scp </color> <color=aqua>%player.name%</color><color=red>(</color><color=aqua>%player.role%</color><color=red>)</color></size>");

			Configs.ht = Plugin.Config.GetString("d_handcuffer_true", "Was handcuffed");
			Configs.hf = Plugin.Config.GetString("d_handcuffer_false", "Wasn't handcuffed");
			Configs.re = Plugin.Config.GetString("d_round_end_bc", "Round end! [Playing time %round.time%] [scp contained %scp.contained%] [%escaped.scientists% Scientists escape] [%escaped.d% D-class escape] [kills %kills%] [kills by scp %kills.by.scp%] [Ntf teams: %ntf.teams%] [Chaos teams: %chaos.teams%] [Alpha Warhead has %warhead.info%]");
			Configs.wnd = Plugin.Config.GetString("d_warhead_not_detonated", "not Detonated");
			Configs.wbd = Plugin.Config.GetString("d_warhead_been_detonated", "been Detonated");
			Configs.ldt = Plugin.Config.GetUInt("d_lcz_containment_lockdown_bc_time", 5);
			Configs.ld = Plugin.Config.GetString("d_lcz_containment_lockdown_bc", "<color=rainbow>Lite containment zone is lockdown</color>!");
			Configs.ret = Plugin.Config.GetUInt("d_round_end_bc_time", 5);
			Scp049Healing = Plugin.Config.GetBool("d_049_healing", true);
			Scp049HealAmount = Plugin.Config.GetInt("d_049_heal_amount", 25);
			Scp049HealPow = Plugin.Config.GetFloat("d_049_heal_power", 1.25f);
			Scp0492Healing = Plugin.Config.GetBool("d_0492_healing", true);
			Scp0492HealAmount = Plugin.Config.GetInt("d_0492_heal_amount", 25);
			Scp096Healing = Plugin.Config.GetBool("d_096_healing", true);
			Scp096Heal = Plugin.Config.GetInt("d_096_heal_amount", 150);
			Scp106Healing = Plugin.Config.GetBool("d_106_healing", true);
			Scp106HealAmount = Plugin.Config.GetInt("d_106_heal_amount", 75);
			Scp173Healing = Plugin.Config.GetBool("d_173_healing", true);
			Scp173HealAmount = Plugin.Config.GetInt("d_173_heal_amount", 150);
			Scp939Healing = Plugin.Config.GetBool("d_939_healing", true);
			Scp939Heal = Plugin.Config.GetInt("d_939_heal_amount", 125);
		}
		public override void OnDisable()
		{
			Events.RoundStartEvent -= EventHandlers.RoundStart;
			Events.RoundEndEvent -= EventHandlers.RoundEnd;


			Events.AnnounceNtfEntranceEvent -= EventHandlers.bc1;
			Events.TeamRespawnEvent -= EventHandlers.bc2;
			Events.CheckRoundEndEvent -= EventHandlers.bc3;
			Events.PlayerDeathEvent -= EventHandlers.bc4;
			Events.PlayerLeaveEvent -= EventHandlers.bc5;
			Events.AnnounceDecontaminationEvent -= EventHandlers.bc6;
			Events.PlayerJoinEvent -= EventHandlers.bc7;
			Events.RoundEndEvent -= EventHandlers.bc8;
			Events.GeneratorFinishedEvent -= EventHandlers.bc9;
			Events.WarheadStartEvent -= EventHandlers.bc10;
			Events.WarheadCancelledEvent -= EventHandlers.bc11;
			Events.RoundStartEvent -= EventHandlers.bc12;
			Events.Scp106ContainEvent -= EventHandlers.bc13;
			Events.WarheadDetonationEvent -= EventHandlers.OnWarheadDetonation;
			Events.RoundStartEvent -= EventHandlers.RoundStart2;
			Events.Scp096CalmEvent -= EventHandlers.OnCalm;
			Events.Scp096EnrageEvent -= EventHandlers.OnEnrage;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDeath;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDeath;
			EventHandlers = null;
		}

		public override void OnReload()
		{
		}

		public override string getName { get; } = "D-class servers";
	}
}
