using System.Collections.Generic;
using HarmonyLib;
using Loli.Addons;
using Loli.DataBase.Modules;
using MEC;
using Qurre.API;
using Qurre.API.Addons;
using Qurre.API.Attributes;

namespace Loli
{
    [PluginInit("Loli", "fydne", "6.6.6")]
    static public class Core
    {
        #region peremens

        static internal string ServerName = "[data deleted]";
        static internal readonly QurreSocket.Client Socket = new(2467, SocketIP);
        public static int MaxPlayers = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 100);
        internal const string CDNUrl = "http://cdn.scpsl.shop";

        static internal JsonConfig Config { get; private set; }

        static internal int ServerID { get; private set; }
        static internal bool UseProxy { get; private set; } = false;
        static internal bool BlockStats { get; private set; } = false;
        static internal short Ticks { get; set; } = 0;
        static internal int TicksMinutes { get; set; } = 0;
        static internal ushort Port => 7779; // Qurre.API.Server.Port;
        static internal string ApiToken => "-";
        static internal string SteamToken => "-";
        static internal string SocketIP => "80.87.194.116";
        static internal string APIUrl => "https://api.scpsl.shop";
        static internal double PreMorningStatsCf => 1.75;
        static internal double MorningStatsCf => 1.5;
        static internal double DayStatsCf => 1.2;
        static internal double PreNightCf => 1.13;
        static internal double NightCf => 1.25;
        static internal double AverageCf => 1.1;

        #endregion

        #region Enable / Disable

        [PluginEnable]
        static internal void Enable()
        {
            Config ??= new("Loli");

            Socket.On("token.required", data => SocketConnected());
            Socket.On("connect", data =>
            {
                Log.Custom("Connected to Socket", "Connect", System.ConsoleColor.Blue);
                SocketConnected();
            });

            static void SocketConnected()
            {
                Socket.Emit("SCPServerInit", new string[] { ApiToken });
                Timing.CallDelayed(1f, () => Socket.Emit("server.clearips", new object[] { ServerID }));
                Timing.CallDelayed(2f, () =>
                {
                    try
                    {
                        foreach (var pl in Player.List)
                        {
                            Socket.Emit("server.addip", new object[]
                            {
                                ServerID,
                                pl.UserInformation.Ip,
                                pl.UserInformation.UserId,
                                pl.UserInformation.Nickname
                            });
                        }
                    }
                    catch
                    {
                    }
                });
            }

            UpdateServers();
            Timing.RunCoroutine(UpdateVerkey());

            Patrol.Init();
            Admins.Call();

            new Harmony("fydne.loli").PatchAll();
        }

        [PluginDisable]
        static internal void Disable()
            => Server.Restart();

        #endregion

        #region Updater

        static void UpdateServers()
        {
            ServerID = 5;
            ServerName = "Vanilla";
        }


        static IEnumerator<float> UpdateVerkey()
        {
            string _token = Config.SafeGetValue("verkey", "default");

            if (_token == "default")
                yield break;

            while (true)
            {
                ServerConsole.Password = _token;
                yield return Timing.WaitForSeconds(2f);
            }
        }

        #endregion
    }
}