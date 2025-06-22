using HarmonyLib;
using Loli.Addons;
using Qurre.API;
using Qurre.API.Attributes;
using System.Text.RegularExpressions;

namespace Loli
{
    [PluginInit("Core", "fydne", "6.6.6")]
    static public class Core
    {
        #region peremens
        static internal string ServerName = "[data deleted]";
        static internal readonly QurreSocket.Client Socket = new(6543, SocketIP);
        public static int MaxPlayers = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 100);

        static internal int ServerID { get; private set; } = 0;
        static internal int DonateID { get; private set; } = 0;
        static internal short Ticks { get; set; } = 0;
        static internal int TicksMinutes { get; set; } = 0;
        static internal string ApiToken => "vsgyiuY7T7WRFg368sfv3qwGw3hf3678gYFFqtyf68GFG7gRF62wgtcGF6fw2f";
        static internal string SocketIP => "185.217.198.46";
        #endregion

        #region Enable / Disable
        [PluginEnable]
        static internal void Enable()
        {
            Socket.On("connect", data => Socket.Emit("SCPServerInit", new string[] { ApiToken }));

            CustomNetworkManager.HeavilyModded = true;

            UpdateServers(ServerConsole.Ip);

            try { CommandsSystem.RegisterRemoteAdmin("oban", OfflineBan.Send); } catch { }

            CommandsSystem.RegisterConsole("чат", Chat.Console);
            CommandsSystem.RegisterConsole("chat", Chat.Console);

            new Harmony("fydne.loli").PatchAll();
        }

        [PluginDisable]
        static internal void Disable() => Server.Restart();
        #endregion

        #region Updater
        static int _trying = 0;
        static void UpdateServers(string ip)
        {
            _trying++;
            string name = Regex.Replace(GameCore.ConfigFile.ServerConfig.GetString("server_name", "").Replace("</color>", ""), @"<color=#([a-zA-Z0-9]+?)>", "").ToLower().Replace(" ", "");
            if (!name.Contains("fydneclassic"))
            {
                if (_trying < 5)
                {
                    MEC.Timing.CallDelayed(5f, () => UpdateServers(ip));
                    return;
                }
                Server.Exit();
                return;
            }
            /*
            if (ip != "185.221.196.72")
            {
                Server.Exit();
                return;
            }
            */
            ServerID = 5;
            ServerName = "Classic";
            DonateID = 1;
        }
        #endregion
    }
}