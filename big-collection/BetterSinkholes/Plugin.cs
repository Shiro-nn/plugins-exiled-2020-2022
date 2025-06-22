using EXILED;
using Harmony;

namespace BetterSinkholes
{
    public class Plugin : EXILED.Plugin
    {
        public override string getName { get; } = "Better Sinkholes - blackruby";

        public static bool EnableBetterSinkholes;
        public static float TeleportDistance;
        public static float SlowDistance;
        public static string TeleportMessage;
        public static uint TeleportMessageDuration;
        public static uint PatchCount;

        public static HarmonyInstance HarmonyInstance { set; get; }
        public override void OnEnable()
        {
            
            EnableBetterSinkholes = Plugin.Config.GetBool("bs_enable", true);

            if(EnableBetterSinkholes == false)
            {
                Plugin.Info("BetterSinkholes is disabled on this server.");
                return;
            }

            Plugin.Info("Thank you for using BetterSinkholes - blackruby");
            SlowDistance = Plugin.Config.GetFloat("bs_slow_distance", 1.15f);
            TeleportDistance = Plugin.Config.GetFloat("bs_teleport_distance", 0.7f);
            TeleportMessage = Plugin.Config.GetString("bs_teleport_message", "You fell into the pocket dimension.");
            TeleportMessageDuration = Plugin.Config.GetUInt("bs_teleport_message_duration", 5U);;

            HarmonyInstance = HarmonyInstance.Create($"blackruby.exiled.bettersinkholes{PatchCount}");
            PatchCount++;
            HarmonyInstance.PatchAll();

        }

        public override void OnDisable()
        {
            HarmonyInstance.UnpatchAll();
        }

        public override void OnReload()
        {
            
        }
    }
}
