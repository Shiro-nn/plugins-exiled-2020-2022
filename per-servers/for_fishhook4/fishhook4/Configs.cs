using System.Collections.Generic;

namespace fishhook4
{
	internal static class Configs
	{
		internal static int mh;
		internal static int mmh;
		internal static string hb;
		internal static uint hbt;

		internal static int g3mi;
		internal static int g3ma;
		internal static string g3b;
		internal static uint g3bt;

		internal static int mfmi;
		internal static int mfma;
		internal static string mfb;
		internal static uint mfbt;

		internal static int ctbmi;
		internal static int ctbma;
		internal static string ctbb;
		internal static uint ctbbt;
		internal static float ctbf;

		internal static int cdmi;
		internal static int cdma;
		internal static string cdb;
		internal static uint cdbt;

		internal static int gwmi;
		internal static int gwma;
		internal static string gwb;
		internal static uint gwbt;

		internal static int hdmi;
		internal static int hdma;
		internal static string hdb;
		internal static uint hdbt;

		internal static int dbmi;
		internal static int dbma;
		internal static string dbb;
		internal static uint dbbt;

		internal static int rdmi;
		internal static int rdma;
		internal static string rdb;
		internal static uint rdbt;

		internal static int sdmi;
		internal static int sdma;
		internal static string sdb;
		internal static uint sdbt;

		internal static bool nuke;
		internal static int alphastart;
		internal static string aabc;
		internal static uint aabctime;

		internal static int gami;
		internal static int gama;
		internal static string gab;
		internal static uint gabt;

		internal static int lrmi;
		internal static int lrma;
		internal static string lrb;
		internal static uint lrbt;

		internal static int psmi;
		internal static int psma;
		internal static string psb;
		internal static uint psbt;

		internal static int yyomi;
		internal static int yyoma;
		internal static string yyob;
		internal static uint yyobt;

		internal static int lfmi;
		internal static int lfma;
		internal static int lft;
		internal static string lfb;
		internal static uint lfbt;

		internal static int aami;
		internal static int aama;
		internal static string aab;
		internal static uint aabt;

		internal static int pomi;
		internal static int poma;
		internal static string pob;
		internal static uint pobt;

		internal static int fhmi;
		internal static int fhma;
		internal static string fhb;
		internal static uint fhbt;

		internal static string p2b;
		internal static uint p2bt;

		internal static string scb;
		internal static uint scbt;

		internal static string sqb;
		internal static uint sqbt;
		internal static void ReloadConfig()
		{
			Configs.mh = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_home_elderly", 200);
			Configs.mmh = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_home_elderly", 1200);
			Configs.hb = Plugin.Config.GetString("fishhook4_home_elderly_bc", "Event: Home for the Elderly");
			Configs.hbt = Plugin.Config.GetUInt("fishhook4_home_elderly_bc_time", 5);

			Configs.g3mi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_gift_343", 200);
			Configs.g3ma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_gift_343", 1200);
			Configs.g3b = Plugin.Config.GetString("fishhook4_gift_343_bc", "Event: 343's gift");
			Configs.g3bt = Plugin.Config.GetUInt("fishhook4_gift_343_bc_time", 5);

			Configs.mfmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_mud_fun", 200);
			Configs.mfma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_mud_fun", 1200);
			Configs.mfb = Plugin.Config.GetString("fishhook4_mud_fun_bc", "Event: Mud is so fun");
			Configs.mfbt = Plugin.Config.GetUInt("fishhook4_mud_fun_bc_time", 5);

			Configs.ctbmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_comfortable_bath", 200);
			Configs.ctbma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_comfortable_bath", 1200);
			Configs.ctbb = Plugin.Config.GetString("fishhook4_comfortable_bath_bc", "Event: It's so comfortable taking a bath");
			Configs.ctbbt = Plugin.Config.GetUInt("fishhook4_comfortable_bath_bc_time", 5);
			Configs.ctbf = Plugin.Config.GetFloat("fishhook4_comfortable_bath_off_sec", 20);

			Configs.cdmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_chocolate_delicious", 200);
			Configs.cdma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_chocolate_delicious", 1200);
			Configs.cdb = Plugin.Config.GetString("fishhook4_chocolate_delicious_bc", "Event: Chocolate is delicious");
			Configs.cdbt = Plugin.Config.GetUInt("fishhook4_chocolate_delicious_bc_time", 5);

			Configs.gwmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_off_work", 200);
			Configs.gwma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_off_work", 1200);
			Configs.gwb = Plugin.Config.GetString("fishhook4_off_work_bc", "Event: I want to get off work");
			Configs.gwbt = Plugin.Config.GetUInt("fishhook4_off_work_bc_time", 5);

			Configs.hdmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_hug_daddy", 200);
			Configs.hdma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_hug_daddy", 1200);
			Configs.hdb = Plugin.Config.GetString("fishhook4_hug_daddy_bc", "Event: Hug Daddy");
			Configs.hdbt = Plugin.Config.GetUInt("fishhook4_hug_daddy_bc_time", 5);

			Configs.dbmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_drebellion", 200);
			Configs.dbma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_drebellion", 1200);
			Configs.dbb = Plugin.Config.GetString("fishhook4_drebellion_bc", "Event: D BUND");
			Configs.dbbt = Plugin.Config.GetUInt("fishhook4_drebellion_bc_time", 5);

			Configs.rdmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_revenged", 200);
			Configs.rdma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_revenged", 1200);
			Configs.rdb = Plugin.Config.GetString("fishhook4_revenged_bc", "Event: Revenge of the D-class");
			Configs.rdbt = Plugin.Config.GetUInt("fishhook4_revenged_bc_time", 5);

			Configs.sdmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_scientist_drop", 200);
			Configs.sdma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_scientist_drop", 1200);
			Configs.sdb = Plugin.Config.GetString("fishhook4_scientist_drop_bc", "Event: Scientist drop!");
			Configs.sdbt = Plugin.Config.GetUInt("fishhook4_scientist_drop_bc_time", 5);

			Configs.nuke = Plugin.Config.GetBool("fishhook4_auto_alpha_start", true);
			Configs.alphastart = Plugin.Config.GetInt("fishhook4_auto_alpha_start_time", 1800);
			Configs.aabc = Plugin.Config.GetString("fishhook4_auto_alpha_start_bc", "Auto Warhead Started!");
			Configs.aabctime = Plugin.Config.GetUInt("fishhook4_auto_alpha_start_bc_time", 5);

			Configs.gami = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_graduated_apprentice", 200);
			Configs.gama = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_graduated_apprentice", 1200);
			Configs.gab = Plugin.Config.GetString("fishhook4_graduated_apprentice_bc", "Event: Graduated apprentice!");
			Configs.gabt = Plugin.Config.GetUInt("fishhook4_graduated_apprentice_bc_time", 5);

			Configs.lrmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_license_revoked", 200);
			Configs.lrma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_license_revoked", 1200);
			Configs.lrb = Plugin.Config.GetString("fishhook4_license_revoked_bc", "Event: License revoked!");
			Configs.lrbt = Plugin.Config.GetUInt("fishhook4_license_revoked_bc_time", 5);

			Configs.psmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_practiced_sincerely", 200);
			Configs.psma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_practiced_sincerely", 1200);
			Configs.psb = Plugin.Config.GetString("fishhook4_practiced_sincerely_bc", "Event: I practiced it sincerely!");
			Configs.psbt = Plugin.Config.GetUInt("fishhook4_practiced_sincerely_bc_time", 5);

			Configs.yyomi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_orphanage", 200);
			Configs.yyoma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_orphanage", 1200);
			Configs.yyob = Plugin.Config.GetString("fishhook4_orphanage_bc", "Event: Yang Yongxin Orphanage!");
			Configs.yyobt = Plugin.Config.GetUInt("fishhook4_orphanage_bc_time", 5);

			Configs.lfmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_light_off", 200);
			Configs.lfma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_light_off", 1200);
			Configs.lft = Plugin.Config.GetInt("fishhook4_light_off_time", 300);
			Configs.lfb = Plugin.Config.GetString("fishhook4_light_off_bc", "Event: The foundation switch exploded!");
			Configs.lfbt = Plugin.Config.GetUInt("fishhook4_light_off_bc_time", 5);

			Configs.aami = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_airdrop_ammunition", 200);
			Configs.aama = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_airdrop_ammunition", 1200);
			Configs.aab = Plugin.Config.GetString("fishhook4_airdrop_ammunition_bc", "Event: Airdrop ammunition!");
			Configs.aabt = Plugin.Config.GetUInt("fishhook4_airdrop_ammunition_bc_time", 5);

			Configs.pomi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_peanut_oil", 200);
			Configs.poma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_peanut_oil", 1200);
			Configs.pob = Plugin.Config.GetString("fishhook4_peanut_oil_bc", "Event: Peanut oil!");
			Configs.pobt = Plugin.Config.GetUInt("fishhook4_peanut_oil_bc_time", 5);

			Configs.fhmi = Plugin.Config.GetInt("fishhook4_minimum_sec_for_spawn_fatty_house", 200);
			Configs.fhma = Plugin.Config.GetInt("fishhook4_maximum_sec_for_spawn_fatty_house", 1200);
			Configs.fhb = Plugin.Config.GetString("fishhook4_fatty_house_bc", "Event: Fatty House Happy D!");
			Configs.fhbt = Plugin.Config.GetUInt("fishhook4_fatty_house_bc_time", 5);

			Configs.p2b = Plugin.Config.GetString("fishhook4_pickup_2818_bc", "You pickup scp 2818");
			Configs.p2bt = Plugin.Config.GetUInt("fishhook4_pickup_2818_bc_time", 5);

			Configs.scb = Plugin.Config.GetString("fishhook4_pickup_2818_bc", "You scp-cxk");
			Configs.scbt = Plugin.Config.GetUInt("fishhook4_pickup_2818_bc_time", 5);

			Configs.sqb = Plugin.Config.GetString("fishhook4_pickup_2818_bc", "You scp-qbl");
			Configs.sqbt = Plugin.Config.GetUInt("fishhook4_pickup_2818_bc_time", 5);
		}
	}
}
