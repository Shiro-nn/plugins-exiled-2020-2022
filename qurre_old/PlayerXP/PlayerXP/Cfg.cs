using Qurre.API.Addons;

namespace PlayerXP
{
    internal class Cfg
    {
        static internal JsonConfig Config { get; private set; }

        static internal string Lvl { get; private set; }
        static internal string Pn { get; private set; }
        static internal string Jm { get; private set; }
        static internal string Lvlup { get; private set; }
        static internal string Eb { get; private set; }
        static internal string Kb { get; private set; }
        static internal string Prefixs { get; private set; }
        static internal int Cf { get; private set; }

        static internal void Reload()
        {
            if (Config is null)
                Config = new("PlayerXP");

            Lvl = Config.SafeGetValue("playerxp_lvl", "level", "needed for translation (ex: 1 level, 2 level, etc)");
            Pn = Config.SafeGetValue("playerxp_project", "fydne", "If a player writes in his nickname \"# + the name of your project\", he will receive 2 times more experience");
            Jm = Config.SafeGetValue("playerxp_join",
                "<color=red>If you write in the nickname</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>then you will get 2 times more experience</color>",
                "BroadCast to the player when he joins the server.\n#fydne - # + the \"playerxp_project\" you specified");
            Lvlup = Config.SafeGetValue("playerxp_lvl_up", "<color=#fdffbb>You got %lvl% level! Until the next level you are missing %to.xp% xp.</color>",
                "BroadCast to a player when they level up");
            Eb = Config.SafeGetValue("playerxp_escape", "<color=#fdffbb>You got %xp%xp for escaping</color>",
                "BroadCast to a player when they get xp for escaping");
            Kb = Config.SafeGetValue("playerxp_kill", "<color=#fdffbb>You got %xp%xp for killing</color> <color=red>%player%</color>",
                "BroadCast to a player when they get xp for killing a player");
            Prefixs = Config.SafeGetValue("playerxp_prefixs", "", "Level prefixes\n" +
                "Example: (playerxp_prefixs: 1:beginner,2:player,3:thinking) - (1 level | beginner ; 2 level | player ; 3 level | thinking ; etc..");
            Cf = Config.SafeGetValue("playerxp_coefficient", 1, "Experience multiplier");
        }
    }
}