using System.Collections.Generic;
using System;
using System.IO;
using Mirror;
using Exiled.API.Features;

namespace scp228ruj
{
	internal static class Configs
	{
		private static readonly string translationPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED-PTB"), "configs");
		private static readonly string cfgr = Path.Combine(translationPath, $"custom-{Server.Port}-configs.yml");

		internal static bool dsreplace = false;
		internal static bool scpFriendlyFire = true;
		internal static bool tutorialFriendlyFire = true;
		internal static string error1 = "<color=red>Ошибка...</color>";
		internal static string error2 = "Ошибка...";
		internal static string error3 = "red";
		internal static string eb = "\n<color=lime>Последний <color=red>SCP 228 RU J</color> сбежал!</color>";
		internal static ushort ebt = 10;
		internal static string db = "\n<color=lime>Последний <color=red>SCP 228 RU J</color> помер!</color>";
		internal static ushort dbt = 10;
		internal static string sc = "Все ваши друзья-гопники померли :(\nУ вас закончилась водка \nВы хотите пить, поэтому найдите водку и выпейте ее, если вы этого не сделаете, вы умрете \nУ вас есть супер способность: открывать только те двери, которые ведут к водке \nВодка на черный день-это ваша водка на черный день, она содержится в бутылке колы, есть только одна, не перепутайте!";
		internal static string scc = "red";
		internal static string sb = "<color=red>Все ваши друзья-гопники померли</color><color=aqua>(</color>\n<color=lime>Больше информации на [~]</color>";
		internal static ushort sbt = 10;
		internal static string s = "На спавне";
		internal static string vppb = "<color=red>Это водка SCP 228 RU J (%player%)</color>";
		internal static ushort vppbt = 5;
		internal static string vpb = "<color=aqua>%player%</color><color=red> попытался подобрать вашу водку</color>";
		internal static ushort vpbt = 5;
		internal static string a = "<color=aqua>Вас атакует <color=red>SCP 228 RU J</color>!</color>\n<color=lime>Убегайте от него, он ищет водку</color>";
		internal static string svb = "<color=lime>Ура! Вы нашли водку</color>\n<color=aqua>Пей ее и сбегай, главное выпить, потому что это не кола, а водка</color>";
		internal static ushort svbt = 10;
		internal static string eeb = "<color=red>Вы не нашли водку!</color>";
		internal static ushort eebt = 10;
		internal static string com = "scp228";
		internal static string suc = "успешно";
		internal static string nf = "игрок не найден";

		internal static void ReloadConfig()
		{
			if (!File.Exists(cfgr))
			{
				if (!Directory.Exists(translationPath))
				{
					Directory.CreateDirectory(translationPath);
				}
				if (!File.Exists(cfgr))
					File.Create(cfgr).Close();
				Plugin.cfg = new YamlConfig(cfgr);

			}
			Configs.dsreplace = Plugin.cfg.GetBool("scp228ruj_classd_replace_scientist", false);
			Configs.scpFriendlyFire = Plugin.cfg.GetBool("scp228ruj_scp_friendly_fire", true);
			Configs.tutorialFriendlyFire = Plugin.cfg.GetBool("scp228ruj_tutorial_friendly_fire", true);
			Configs.error1 = Plugin.cfg.GetString("scp228ruj_error_bc", "<color=red>Ошибка...</color>");
			Configs.error2 = Plugin.cfg.GetString("scp228ruj_error_console", "Ошибка...");
			Configs.error3 = Plugin.cfg.GetString("scp228ruj_error_console_color", "red");
			Configs.eb = Plugin.cfg.GetString("scp228ruj_escape_bc", "\n<color=lime>Последний <color=red>SCP 228 RU J</color> сбежал!</color>");
			Configs.ebt = Plugin.cfg.GetUShort("scp228ruj_escape_bc_time", 10);
			Configs.db = Plugin.cfg.GetString("scp228ruj_death_bc", "\n<color=lime>Последний <color=red>SCP 228 RU J</color> помер!</color>");
			Configs.dbt = Plugin.cfg.GetUShort("scp228ruj_death_bc_time", 10);
			Configs.sc = Plugin.cfg.GetString("scp228ruj_spawn_console", "Все ваши друзья-гопники померли :(\nУ вас закончилась водка \nВы хотите пить, поэтому найдите водку и выпейте ее, если вы этого не сделаете, вы умрете \nУ вас есть супер способность: открывать только те двери, которые ведут к водке \nВодка на черный день-это ваша водка на черный день, она содержится в бутылке колы, есть только одна, не перепутайте!");
			Configs.scc = Plugin.cfg.GetString("scp228ruj_spawn_console_color", "red");
			Configs.sb = Plugin.cfg.GetString("scp228ruj_spawn_bc", "<color=red>Все ваши друзья-гопники померли</color><color=aqua>(</color>\n<color=lime>Больше информации на [~]</color>");
			Configs.sbt = Plugin.cfg.GetUShort("scp228ruj_spawn_bc_time", 10);
			Configs.s = Plugin.cfg.GetString("scp228ruj_spawn", "На спавне");
			Configs.vppb = Plugin.cfg.GetString("scp228ruj_vodka_pickup_player_bc", "<color=red>Это водка SCP 228 RU J (%player%)</color>");
			Configs.vppbt = Plugin.cfg.GetUShort("scp228ruj_vodka_pickup_player_bc_time", 5);
			Configs.vpb = Plugin.cfg.GetString("scp228ruj_vodka_pickup_bc", "<color=aqua>%player%</color><color=red> попытался подобрать вашу водку</color>");
			Configs.vpbt = Plugin.cfg.GetUShort("scp228ruj_vodka_pickup_bc_time", 5);
			Configs.a = Plugin.cfg.GetString("scp228ruj_attacking", "<color=aqua>Вас атакует <color=red>SCP 228 RU J</color>!</color>\n<color=lime>Убегайте от него, он ищет водку</color>");
			Configs.svb = Plugin.cfg.GetString("scp228ruj_search_vodka_bc", "<color=lime>Ура! Вы нашли водку</color>\n<color=aqua>Пей ее и сбегай, главное выпить, потому что это не кола, а водка</color>");
			Configs.svbt = Plugin.cfg.GetUShort("scp228ruj_search_vodka_bc_time", 10);
			Configs.eeb = Plugin.cfg.GetString("scp228ruj_escape_error_bc", "<color=red>Вы не нашли водку!</color>");
			Configs.eebt = Plugin.cfg.GetUShort("scp228ruj_escape_error_bc_time", 10);
			Configs.com = Plugin.cfg.GetString("scp228ruj_command", "scp228");
			Configs.suc = Plugin.cfg.GetString("scp228ruj_successfully", "успешно");
			Configs.nf = Plugin.cfg.GetString("scp228ruj_not_found", "игрок не найден");
		}
	}
}