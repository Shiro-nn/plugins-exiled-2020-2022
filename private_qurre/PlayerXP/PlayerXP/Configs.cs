namespace PlayerXP
{
    internal static class Configs
    {
        internal static string MongoURL { get; set; } = "";
        internal static string Lvl { get; set; } = "LVL";
        internal static string Pn { get; set; } = "Хлебные";
        internal static string Jm { get; set; } = "<i>Если вы напишите в нике</i> <color=orange><b>#Хлебные</b></color>\n<i>То вы будете получать в 2 раза больше опыта!</i>";
        internal static string Lvlup { get; set; } = "<i>Вы получили</i> <color=orange><b>%lvl% уровень!</b></color> <i>До следующего уровня вам не хватает</i> <color=orange><b>%to.xp% XP</b></color>";
        internal static string Eb { get; set; } = "<i> Вы получили</i> <b><color=orange>%xp%xp</color></b> <i>за побег</i>";
        internal static string Kb { get; set; } = "<i> Вы получили</i> <b><color=orange>%xp%xp</color></b> <i>за убийство</i> <b><color=red>%player%</color></b>";
        internal static string Prefixs { get; set; } = "1:НеХлеб,2:Сбежал из Дурки,3:Кот-Мафиози,4:Мог Хлеба";
        internal static int Cf { get; set; } = 1;
        internal static void Reload()
        {
            MongoURL = Plugin.Config.GetString("PlayerXP_MongoURL", MongoURL);
            Lvl = Plugin.Config.GetString("PlayerXP_Lvl", Lvl);
            Pn = Plugin.Config.GetString("PlayerXP_Project", Pn, "Название проекта(для x2 опыта)");
            Jm = Plugin.Config.GetString("PlayerXP_Join_BC", Jm, "BC при заходе на сервер(через 100 сек)");
            Lvlup = Plugin.Config.GetString("PlayerXP_Lvlup", Lvlup, "BC при повышении уровня");
            Eb = Plugin.Config.GetString("PlayerXP_Escape_BC", Eb, "BC при побеге");
            Kb = Plugin.Config.GetString("PlayerXP_Kill_BC", Kb, "BC при убийстве");
            Prefixs = Plugin.Config.GetString("PlayerXP_Prefixs", Prefixs, "Префиксы ролей");
            Cf = Plugin.Config.GetInt("PlayerXP_Coefficient", 1, "Множитель опыта");
        }
    }
}