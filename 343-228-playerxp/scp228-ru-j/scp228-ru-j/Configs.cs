using System.Collections.Generic;

namespace scp228ruj
{
	internal static class Configs
	{

		internal static bool dsreplace;
		internal static bool scpFriendlyFire;
		internal static bool tutorialFriendlyFire;
		internal static string error1;
		internal static string error2;
		internal static string error3;
		internal static string eb;
		internal static ushort ebt;
		internal static string db;
		internal static ushort dbt;
		internal static string sc;
		internal static string scc;
		internal static string sb;
		internal static ushort sbt;
		internal static string s;
		internal static string vppb;
		internal static ushort vppbt;
		internal static string vpb;
		internal static ushort vpbt;
		internal static string a;
		internal static string svb;
		internal static ushort svbt;
		internal static string eeb;
		internal static ushort eebt;
		internal static string com;
		internal static string suc;
		internal static string nf;

		internal static void ReloadConfig()
		{
			Configs.dsreplace = Plugin.Config.GetBool("scp228ruj_classd_replace_scientist", false);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("scp228ruj_scp_friendly_fire", true);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("scp228ruj_tutorial_friendly_fire", true);
			Configs.error1 = Plugin.Config.GetString("scp228ruj_error_bc", "<color=red>Ошибка...</color>");
			Configs.error2 = Plugin.Config.GetString("scp228ruj_error_console", "Ошибка...");
			Configs.error3 = Plugin.Config.GetString("scp228ruj_error_console_color", "red");
			Configs.eb = Plugin.Config.GetString("scp228ruj_escape_bc", "\n<color=lime>Последний <color=red>SCP 228 RU J</color> сбежал!</color>");
			Configs.ebt = Plugin.Config.GetUShort("scp228ruj_escape_bc_time", 10);
			Configs.db = Plugin.Config.GetString("scp228ruj_death_bc", "\n<color=lime>Последний <color=red>SCP 228 RU J</color> помер!</color>");
			Configs.dbt = Plugin.Config.GetUShort("scp228ruj_death_bc_time", 10);
			Configs.sc = Plugin.Config.GetString("scp228ruj_spawn_console", "Все ваши друзья-гопники померли :(\nУ вас закончилась водка \nВы хотите пить, поэтому найдите водку и выпейте ее, если вы этого не сделаете, вы умрете \nУ вас есть супер способность: открывать только те двери, которые ведут к водке \nВодка на черный день-это ваша водка на черный день, она содержится в бутылке колы, есть только одна, не перепутайте!");
			Configs.scc = Plugin.Config.GetString("scp228ruj_spawn_console_color", "red");
			Configs.sb = Plugin.Config.GetString("scp228ruj_spawn_bc", "<color=red>Все ваши друзья-гопники померли</color><color=aqua>(</color>\n<color=lime>Больше информации на [~]</color>");
			Configs.sbt = Plugin.Config.GetUShort("scp228ruj_spawn_bc_time", 10);
			Configs.s = Plugin.Config.GetString("scp228ruj_spawn", "На спавне");
			Configs.vppb = Plugin.Config.GetString("scp228ruj_vodka_pickup_player_bc", "<color=red>Это водка SCP 228 RU J (%player%)</color>");
			Configs.vppbt = Plugin.Config.GetUShort("scp228ruj_vodka_pickup_player_bc_time", 5);
			Configs.vpb = Plugin.Config.GetString("scp228ruj_vodka_pickup_bc", "<color=aqua>%player%</color><color=red> попытался подобрать вашу водку</color>");
			Configs.vpbt = Plugin.Config.GetUShort("scp228ruj_vodka_pickup_bc_time", 5);
			Configs.a = Plugin.Config.GetString("scp228ruj_attacking", "<color=aqua>Вас атакует <color=red>SCP 228 RU J</color>!</color>\n<color=lime>Убегайте от него, он ищет водку</color>");
			Configs.svb = Plugin.Config.GetString("scp228ruj_search_vodka_bc", "<color=lime>Ура! Вы нашли водку</color>\n<color=aqua>Пей ее и сбегай, главное выпить, потому что это не кола, а водка</color>");
			Configs.svbt = Plugin.Config.GetUShort("scp228ruj_search_vodka_bc_time", 10);
			Configs.eeb = Plugin.Config.GetString("scp228ruj_escape_error_bc", "<color=red>Вы не нашли водку!</color>");
			Configs.eebt = Plugin.Config.GetUShort("scp228ruj_escape_error_bc_time", 10);
			Configs.com = Plugin.Config.GetString("scp228ruj_command", "scp228");
			Configs.nf = Plugin.Config.GetString("scp228ruj_not_found", "игрок не найден");
			Configs.suc = Plugin.Config.GetString("scp228ruj_successfully", "успешно");
		}
	}
}
