using Exiled.API.Interfaces;
using System.ComponentModel;

namespace PlayerXP
{
    public sealed class Configs : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public static string Lvl { get; set; } = "Уровень";
        /// <summary>
        /// Название проекта.
        /// </summary>
        [Description("Название проекта")]
        public static string Pn { get; set; } = "SimetriaProject";
        /// <summary>
        /// Join msg.
        /// </summary>
        [Description("Join msg")]
        public static string Jm { get; set; } = "<color=orange>Если вы напишите в нике #SimetriaProject\nто вы будете получать в 2 раза больше опыта</color>";
        /// <summary>
        /// lvl up bc.
        /// </summary>
        [Description("lvl up bc")]
        public static string Lvlup { get; set; } = "<color=orange>Вы получили %lvl% уровень! До следующего уровня вам не хватает %to.xp% xp.</color>";
        /// <summary>
        /// Escape bc.
        /// </summary>
        [Description("Escape bc")]
        public static string Eb { get; set; } = "<color=orange>Вы получили %xp%xp за побег</color>";
        /// <summary>
        /// kill bc.
        /// </summary>
        [Description("kill bc")]
        public static string Kb { get; set; } = "<color=orange>Вы получили %xp%xp за убийство</color> <color=red>%player%</color>";
        /// <summary>
        /// console command.
        /// </summary>
        [Description("console command")]
        public static string Cc { get; set; } = "level";
    }
}
