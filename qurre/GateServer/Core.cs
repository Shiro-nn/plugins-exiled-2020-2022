using HarmonyLib;
using MEC;
using Qurre.API;
using Qurre.API.Addons;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace GateServer
{
    [PluginInit("Gate", "fydne", "0.0.0")]
    static public class Core
    {
        static JsonConfig Config { get; set; }
        static ushort Port { get; set; }

        [PluginEnable]
        static void Enable()
        {
            new Harmony("fydne.hub").PatchAll();

            Config = new JsonConfig("Gate");
            Port = Config.SafeGetValue("Port", Server.Port, "redirect port");
            JsonConfig.UpdateFile();

            Log.Info("Plugin Enabled");
        }


        [EventMethod(PlayerEvents.Join)]
        static void Redirect(JoinEvent ev)
        {
            ev.Player.Client.ShowHint("<color=#eb34e5>Идет проверка подключения...</color>\n" +
                "<color=#a834eb>После проверки, Вы будете подключены к серверу</color>", 666);

            Timing.CallDelayed(5f, () =>
            {
                if (ev.Player.Disconnected)
                    return;

                ev.Player.Client.Redirect(Port);
            });
        }
    }
}