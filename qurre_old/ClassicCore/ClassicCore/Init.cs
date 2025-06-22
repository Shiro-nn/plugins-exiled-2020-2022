#region Links
using System.Text.RegularExpressions;
using System.Threading;
using HarmonyLib;
using MEC;
#endregion
#region Core
using ClassicCore.Spawns;
using ClassicCore.Logs;
using ClassicCore.DataBase;
using ClassicCore.Scps;
using ClassicCore.Addons;
using ClassicCore.DataBase.Modules.Controllers;
#endregion
namespace ClassicCore
{
    public class Init : Qurre.Plugin
    {
        #region peremens
        internal static int ServerID { get; private set; } = 0;
        internal static string ServerName = "[data deleted]";
        internal static bool UseProxy { get; private set; } = false;
        internal static short Ticks { get; set; } = 0;
        internal static int TicksMinutes { get; set; } = 0;
        internal static ushort Port => Qurre.API.Server.Port;
        internal static string ApiToken => "-";
        internal static string SocketIP => "37.18.21.237";
        internal static string APIUrl => "https://api.scpsl.store";
        internal static readonly QurreSocket.Client Socket = new(2467, SocketIP);
        public static int MaxPlayers = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 100);
        #endregion
        #region override
        public override System.Version Version => new(6, 6, 6);
        public override System.Version NeededQurreVersion => new(1, 15, 0);
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "Classic Core";

        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Fix
        internal void UpdateServers()
        {
            if (Port == 7777)
            {
                ServerID = 2;
                ServerName = "Classic NoRules";
            }
            else if (Port == 7778)
            {
                ServerID = 1;
                ServerName = "Classic Standart";
            }
        }
        #endregion
        #region RegEvents
        private void RegisterEvents()
        {
            Socket.On("connect", data => Socket.Emit("SCPServerInit", new string[] { ApiToken }));
            Socket.On("connect", data =>
            {
                Timing.CallDelayed(1f, () => Socket.Emit("server.clearips", new object[] { ServerID }));
                Timing.CallDelayed(2f, () =>
                {
                    try { foreach (var pl in Qurre.API.Player.List) Socket.Emit("server.addip", new object[] { ServerID, pl.Ip }); } catch { }
                });
            });
            CustomNetworkManager.HeavilyModded = true;
            new Harmony("fydne.classic.core").PatchAll();

            UpdateServers();

            var DataBase = new Manager(this);
            var Levels = new Levels();
            new Force();
            new Commands();
            var SpawnManager = new SpawnManager();
            var Scp0492Better = new Scp0492Better();
            var Scp079Better = new Scp079Better();
            var Prime = new Prime();
            var Customize = new Customize();

            try { CommandsSystem.RegisterRemoteAdmin("oban", OfflineBan.Send); } catch { }
            try { ClassicCore.DataBase.Modules.Patrol.Init(); } catch { }

            Qurre.Events.Server.SendingConsole += CommandsSystem.ConsoleInvoke;
            Qurre.Events.Server.SendingRA += CommandsSystem.RAInvoke;

            Qurre.Events.Player.Spawn += Spawn.Update;

            Qurre.Events.Player.Join += Force.Join;
            Qurre.Events.Player.Spawn += Force.Spawn;

            Qurre.Events.Player.InteractDoor += RemoteKeycard.Door;
            Qurre.Events.Player.InteractLocker += RemoteKeycard.Locker;
            Qurre.Events.Player.InteractGenerator += RemoteKeycard.Generator;

            Qurre.Events.Server.RaRequestPlayerList += DataBase.Admins.Prefixs;
            Qurre.Events.Player.Ban += DataBase.Admins.Ban;
            Qurre.Events.Player.Kick += DataBase.Admins.Kick;
            Qurre.Events.Round.Waiting += DataBase.Data.Waiting;
            Qurre.Events.Round.End += DataBase.Data.RoundEnd;
            Qurre.Events.Server.SendingRA += DataBase.Donate.Ra;
            Qurre.Events.Player.Join += DataBase.Loader.Join;
            Qurre.Events.Player.RoleChange += DataBase.Updater.Spawn;
            Qurre.Events.Player.Spawn += DataBase.Updater.Spawn;
            Qurre.Events.Round.End += DataBase.Updater.End;
            Qurre.Events.Round.Waiting += DataBase.Updater.Waiting;
            Qurre.Events.Player.Leave += DataBase.Updater.Leave;

            Qurre.Events.Round.Waiting += Events.Waiting;
            Qurre.Events.Round.Start += Events.RoundStart;
            Qurre.Events.Round.End += Events.RoundEnd;

            Qurre.Events.Round.Waiting += SpawnManager.MobileTaskForces.Refresh;
            Qurre.Events.Round.Start += SpawnManager.MobileTaskForces.Refresh;
            Qurre.Events.Player.Dead += SpawnManager.MobileTaskForces.Dead;
            Qurre.Events.Player.DamageProcess += SpawnManager.MobileTaskForces.Damage;

            Qurre.Events.Player.Damage += SpawnManager.SpawnProtect;
            Qurre.Events.Scp096.AddTarget += SpawnManager.SpawnProtect;
            Qurre.Events.Scp106.PocketEnter += SpawnManager.SpawnProtect;
            Qurre.Events.Round.Start += SpawnManager.SpawnCor;
            Qurre.Events.Round.End += SpawnManager.SpawnCor;

            Qurre.Events.Player.Spawn += Scp0492Better.Spawn;
            Qurre.Events.Player.DamageProcess += Scp0492Better.Damage;

            Qurre.Events.Player.Spawn += Scp079Better.Spawn;

            Qurre.Events.Player.RadioUsing += Prime.RadioUnlimited;
            Qurre.Events.Player.MicroHidUsing += Prime.HidMore;

            Qurre.Events.Player.Leave += Customize.Leave;
            Qurre.Events.Player.Spawn += Customize.Spawn;
            Qurre.Events.Player.RoleChange += Customize.Spawn;
            Qurre.Events.Player.DamageProcess += Customize.Damage;

            Qurre.Events.Round.Waiting += Levels.Refresh;
            Qurre.Events.Round.Start += Levels.Refresh;
            Qurre.Events.Player.DamageProcess += Levels.Damage;
            Qurre.Events.Player.Spawn += Levels.Spawn;
            Qurre.Events.Player.Leave += Levels.Leave;
            Qurre.Events.Player.Join += Levels.Join;
            Qurre.Events.Player.Spawn += Levels.PlayerSpawn;
            Qurre.Events.Round.End += Levels.XpEnd;
            Qurre.Events.Player.Dead += Levels.XpDeath;
            Qurre.Events.Player.Escape += Levels.XpEscape;

            Qurre.Events.Round.Waiting += Chat.Waiting;
            Qurre.Events.Player.Leave += Chat.Leave;
            CommandsSystem.RegisterConsole("чат", Chat.Console);
            CommandsSystem.RegisterConsole("chat", Chat.Console);

            Qurre.Events.Round.Start += EndStats.Refresh;
            Qurre.Events.Round.Waiting += EndStats.Refresh;
            Qurre.Events.Player.Dead += EndStats.Dead;
            Qurre.Events.Scp106.PocketFailEscape += EndStats.Pocket;
            Qurre.Events.Player.DamageProcess += EndStats.Damage;
            Qurre.Events.Player.Escape += EndStats.Escape;

            Qurre.Events.Round.Start += BetterAirLocks.Init;

            Qurre.Events.Player.DamageProcess += Priest.Damage;
            Qurre.Events.Player.DamageProcess += Priest.NoPriestTk;
            Qurre.Events.Scp106.PocketEnter += Priest.No106;
            Qurre.Events.Player.ScpAttack += Priest.NoSCPs;

            Scps.Scp294.Events.Init();

            AutoAlpha.Init();

            Qurre.Events.Server.SendingRA += CrashProtect.SetGroup;
            Qurre.Events.Player.Ban += CrashProtect.AntiBan;
            Qurre.Events.Player.Kick += CrashProtect.AntiKick;
        }
        #endregion
        #region UnregEvents
        private void UnregisterEvents() => Qurre.API.Server.Restart();
        #endregion
    }
}