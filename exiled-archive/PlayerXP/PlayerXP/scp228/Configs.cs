using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerXP.scp228
{
	internal static class Configs
	{
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
			Configs.dsreplace = false;
			Configs.scpFriendlyFire = true;
			Configs.tutorialFriendlyFire = true;
			Configs.error1 = "<color=red>Ошибка...</color>";
			Configs.error2 = "Ошибка...";
			Configs.error3 = "red";
			Configs.eb = "\n<color=lime>Последний <color=red>SCP 228 RU J</color> сбежал!</color>";
			Configs.ebt = 10;
			Configs.db = "\n<color=lime>Последний <color=red>SCP 228 RU J</color> помер!</color>";
			Configs.dbt = 10;
			Configs.sc = "Все ваши друзья-гопники померли :(\nУ вас закончилась водка \nВы хотите пить, поэтому найдите водку и выпейте ее, если вы этого не сделаете, вы умрете \nУ вас есть супер способность: открывать только те двери, которые ведут к водке \nВодка на черный день-это ваша водка на черный день, она содержится в бутылке колы, есть только одна, не перепутайте!";
			Configs.scc = "red";
			Configs.sb = "<color=red>Все ваши друзья-гопники померли</color><color=aqua>(</color>\n<color=lime>Больше информации на [~]</color>";
			Configs.sbt = 10;
			Configs.s = "На спавне";
			Configs.vppb = "<color=red>Это водка SCP 228 RU J (%player%)</color>";
			Configs.vppbt = 5;
			Configs.vpb = "<color=aqua>%player%</color><color=red> попытался подобрать вашу водку</color>";
			Configs.vpbt = 5;
			Configs.a = "<color=aqua>Вас атакует <color=red>SCP 228 RU J</color>!</color>\n<color=lime>Убегайте от него, он ищет водку</color>";
			Configs.svb = "<color=lime>Ура! Вы нашли водку</color>\n<color=aqua>Пей ее и сбегай, главное выпить, потому что это не кола, а водка</color>";
			Configs.svbt = 10;
			Configs.eeb = "<color=red>Вы не нашли водку!</color>";
			Configs.eebt = 10;
			Configs.com = "scp228";
			Configs.suc = "успешно";
			Configs.nf = "игрок не найден";
		}
	}
}
