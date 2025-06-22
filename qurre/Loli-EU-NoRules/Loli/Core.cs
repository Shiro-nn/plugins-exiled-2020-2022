using HarmonyLib;
using Loli.Addons;
using Loli.Scps;
using MEC;
using Qurre.API.Attributes;
using System.Threading;

namespace Loli
{
    [PluginInit("Loli", "fydne", "6.6.6")]
    static public class Core
    {
        #region peremens
        static internal string ServerName = "[data deleted]";
        static internal readonly int MaxPlayers = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 66);

        static internal QurreSocket.Client Socket { get; private set; } = new(2524, SocketIP);
        static internal int ServerID { get; private set; } = 0;
        static internal bool YTAcess { get; private set; } = false;
        static internal bool UseProxy { get; private set; } = false;
        static internal short Ticks { get; set; } = 0;
        static internal int TicksMinutes { get; set; } = 0;
        static internal string ApiToken => "SJHV78svtsyugciuhwfy78gGFYUtwegyug783yfuGTWF3y6ygf367fg";
        static internal string SocketIP => "127.0.0.1"; // 45.142.122.184
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

            UpdateServers(ServerConsole.Ip);
            //if (ServerID == 0) new Thread(() => LoopFix()).Start();

            try { CommandsSystem.RegisterRemoteAdmin("oban", OfflineBan.Send); } catch { }

            CommandsSystem.RegisterRemoteAdmin("bp", BackupPower.Ra);
            CommandsSystem.RegisterRemoteAdmin("backup_power", BackupPower.Ra);

            CommandsSystem.RegisterConsole("чат", Chat.Console);
            CommandsSystem.RegisterConsole("chat", Chat.Console);

            Scps.Scp294.Events.Init();

            new Harmony("fydne.loli").PatchAll();
        }

        [PluginDisable]
        static internal void Disable() => Qurre.API.Server.Restart();
        #endregion

        #region Updater
        static void UpdateServers(string ip)
        {
            ServerID = 1;
            ServerName = "NoRules";
            YTAcess = true;
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