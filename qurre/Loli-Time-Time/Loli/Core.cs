using HarmonyLib;
using Loli.Addons;
using Loli.Scps;
using MEC;
using Qurre.API.Attributes;
using System.Threading;

namespace Loli
{
    [PluginInit("Loli", "fydne", "666")]
    static public class Core
    {
        #region peremens
        static internal string ServerName = "[data deleted]";
        static internal readonly QurreSocket.Client Socket = new(2467, SocketIP);
        public static int MaxPlayers = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 100);

        static internal int ServerID { get; private set; } = 0;
        static internal int DonateID { get; private set; } = 0;
        static internal bool YTAcess { get; private set; } = false;
        static internal bool UseProxy { get; private set; } = false;
        static internal short Ticks { get; set; } = 0;
        static internal int TicksMinutes { get; set; } = 0;
        static internal ushort Port => Qurre.API.Server.Port;
        static internal string ApiToken => "-";
        static internal string SteamToken => "C31CBA5EA1018FABEA411F37E8845EA9";
        static internal string SocketIP => "45.142.122.184";
        static internal string APIUrl => "http://api.scpsl.store";
        #endregion

        #region Enable / Disable
        [PluginEnable]
        static internal void Enable()
        {
            Socket.On("connect", data => Socket.Emit("SCPServerInit", new string[] { ApiToken }));
            Socket.On("connect", data =>
            {
                Timing.CallDelayed(1f, () => Socket.Emit("server.clearips", new object[] { ServerID }));
                Timing.CallDelayed(2f, () =>
                {
                    try { foreach (var pl in Qurre.API.Player.List) Socket.Emit("server.addip", new object[] { ServerID, pl.UserInfomation.Ip }); } catch { }
                });
            });

            CustomNetworkManager.HeavilyModded = true;

            new Harmony("fydne.loli").PatchAll();

            string ownerip = ServerConsole.Ip;
            UpdateServers(ownerip);
            if (ServerID == 0) new Thread(() => LoopFix()).Start();

            try { CommandsSystem.RegisterRemoteAdmin("oban", OfflineBan.Send); } catch { }
            try { DataBase.Modules.Patrol.Init(); } catch { }

            CommandsSystem.RegisterRemoteAdmin("bp", BackupPower.Ra);
            CommandsSystem.RegisterRemoteAdmin("backup_power", BackupPower.Ra);

            CommandsSystem.RegisterConsole("чат", Chat.Console);
            CommandsSystem.RegisterConsole("chat", Chat.Console);

            CommandsSystem.RegisterRemoteAdmin("scp343", God.Ra);

            ClansRecommendation.Init();
        }

        [PluginDisable]
        static internal void Disable() => Qurre.API.Server.Restart();
        #endregion

        #region Updater
        static void UpdateServers(string ip)
        {
            if (Port == 7779)
            {
                ServerName = "NoRules";
                if (ip == "79.137.206.68")
                {
                    ServerID = 4;
                    ServerName = "NoRules #2";
                    YTAcess = true;
                    DonateID = 1;
                }
            }
        }
        static void LoopFix()
        {
            while (ServerID == 0)
            {
                Thread.Sleep(5000);
                UpdateServers("https://myexternalip.com/raw".HttpGet());
            }
        }
        #endregion
    }
}