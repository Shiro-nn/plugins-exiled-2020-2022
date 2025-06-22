using Qurre;
namespace Stalk
{
    internal static class Cfg
    {
        internal static string SpawnBC { get; set; } = "";
        internal static string TargetsZero { get; set; } = "";
        internal static string TryAgain { get; set; } = "";
        internal static string Cooldown { get; set; } = "";
        internal static string Target { get; set; } = "";
        internal static uint CoolDown { get; set; } = 30;
        internal static void Reload()
        {
            SpawnBC = Plugin.Config.GetString("Stalk_Spawn_BroadCast", "<size=30%><color=#6f6f6f>If you click on create a portal 2 times, you will teleport to a random target</color></size>");
            TargetsZero = Plugin.Config.GetString("Stalk_TargetsNotFound", "<size=30%><color=#6f6f6f>Alas, targets not found.</color></size>");
            TryAgain = Plugin.Config.GetString("Stalk_TryAgain", "<size=30%><color=#6f6f6f>Try again</color></size>");
            Cooldown = Plugin.Config.GetString("Stalk_Cooldown", "<size=30%><color=#6f6f6f>Wait <color=red>%cooldown%</color> seconds</color></size>");
            Target = Plugin.Config.GetString("Stalk_Target", "<size=30%><color=#6f6f6f>Your victim is <color=red>%target.Nickname%</color></color></size>\n" +
                "<size=30%><color=#6f6f6f>Looks like he's a <color=%color%>%target.Role%</color></color></size>");
            CoolDown = Plugin.Config.GetUInt("Stalk_CoolDown_Time", 30);
        }
    }
}