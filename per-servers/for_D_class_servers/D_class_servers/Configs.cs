using System.Collections.Generic;

namespace D_class_servers
{
	internal static class Configs
	{
		internal static string newmtfsquad;
		internal static uint newmtfsquadt;
		internal static string newchaossquad;
		internal static uint newchaossquadt;
		internal static string plastm;
		internal static uint plastmt;
		internal static string plastp;
		internal static uint plastpt;
		internal static string d;
		internal static string sc;
		internal static string ci;
		internal static string t;
		internal static int initialCooldown;
		internal static string pl;
		internal static uint plt;
		internal static string lc;
		internal static uint lct;
		internal static string flc;
		internal static uint flct;
		internal static string llc;
		internal static uint llct;
		internal static string wm;
		internal static uint wmt;
		internal static string ga;
		internal static uint gat;
		internal static string wa;
		internal static uint wat;
		internal static string wd;
		internal static uint wdt;
		internal static uint rst;
		internal static string ht;
		internal static string hf;
		internal static string dontaccess;
		internal static string killer;
		internal static uint killerdur;
		internal static string cs;
		internal static uint csdur;
		internal static string asd;
		internal static string re;
		internal static uint ret;
		internal static string wnd;
		internal static string wbd;
		internal static string ld;
		internal static uint ldt;
		internal static ulong channel;
		internal static string ms;
		internal static string dcs;
		internal static string dlc;
		internal static string dllc;
		internal static string dld;
		internal static string dre;
		internal static void ReloadConfig()
		{
			Configs.initialCooldown = Plugin.Config.GetInt("d_time_106_containment", 120);
			Configs.newmtfsquad = Plugin.Config.GetString("d_new_mtf_squad_bc", "Arrived MTF squad %NtfLetter%%NtfNumber% SCP left %ScpsLeft%");
			Configs.newmtfsquadt = Plugin.Config.GetUInt("d_new_mtf_squad_bc_time", 5);
			Configs.newchaossquad = Plugin.Config.GetString("d_new_chaos_squad_bc", "Chaos!");
			Configs.newchaossquadt = Plugin.Config.GetUInt("d_new_chaos_squad_bc_time", 5);
			Configs.plastm = Plugin.Config.GetString("d_player_last_player_bc", "%user%(%user.role%) last!");
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


			Configs.channel = Plugin.Config.GetULong("d_channel", 639474623179784250);
			Configs.ms = Plugin.Config.GetString("d_discord_new_mtf_squad", "Arrived MTF squad %NtfLetter%%NtfNumber% SCP left %ScpsLeft%");
			Configs.dcs = Plugin.Config.GetString("d_discord_new_chaos_squad", "Chaos!");
			Configs.dlc = Plugin.Config.GetString("d_discord_lcz_containment", "LCZ containment across %time% min!");
			Configs.dllc = Plugin.Config.GetString("d_discord_last_lcz_containment", "LCZ containment across %time% sec!");
			Configs.dld = Plugin.Config.GetString("d_discord_lcz_containment_lockdown", "Lite containment zone is lockdown!");
			Configs.dre = Plugin.Config.GetString("d_discord_round_end", "Round end! [Playing time %round.time%] [scp contained %scp.contained%] [%escaped.scientists% Scientists escape] [%escaped.d% D-class escape] [kills %kills%] [kills by scp %kills.by.scp%] [Ntf teams: %ntf.teams%] [Chaos teams: %chaos.teams%] [Alpha Warhead has %warhead.info%]");
		}
	}
}