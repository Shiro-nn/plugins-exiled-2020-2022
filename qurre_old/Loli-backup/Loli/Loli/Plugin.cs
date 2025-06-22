#region Links
using System.Text.RegularExpressions;
using System.Threading;
using HarmonyLib;
using MEC;
#endregion
#region Loli ❤️
using Loli.Items;
using Loli.Spawns;
using Loli.AutoEvents;
using Loli.Logs;
using Loli.DataBase;
using Loli.Modules;
using Loli.Scps;
using Loli.Addons;
using Loli.Spawns.Roles;
using Loli.DataBase.Modules.Controllers;
using System.Net;
using System.IO;
#endregion
namespace Loli
{
    public class Plugin : Qurre.Plugin
    {
        #region peremens
        internal static int ServerID { get; private set; } = 0;
        internal static int DonateID { get; private set; } = 0;
        internal static string ServerName = "[data deleted]";
        internal static bool YTAcess { get; private set; } = false;
        internal static bool RolePlay { get; private set; } = false;
        internal static bool ClansWars { get; private set; } = false;
        internal static bool Anarchy { get; private set; } = false;
        internal static bool HardRP { get; private set; } = false;
        internal static bool YouTubersServer { get; private set; } = false;
        internal static bool UseProxy { get; private set; } = false;
        internal static short Ticks { get; set; } = 0;
        internal static int TicksMinutes { get; set; } = 0;
        internal static ushort Port => Qurre.API.Server.Port;
        internal static string ApiToken => "-";
        internal static string SocketIP => "45.142.122.184";
        internal static string APIUrl => "http://api.scpsl.store";
        internal static readonly QurreSocket.Client Socket = new(2467, SocketIP);
        public static int MaxPlayers = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 100);
        #endregion
        #region nostatic
        public EventHandlers EventHandlers;
        internal Manager DataBase;
        public Shop shop;
        internal Levels Levels;
        public ScpHeal ScpHeal;
        public Spawner Spawner;
        public Spawn Spawn;
        public Force Force;
        public Icom Icom;
        public BroadCasts BroadCasts;
        public new Commands Commands;
        public Events LogsEvents;
        public Scp035 Scp035;
        public Stalky Stalky;
        public OmegaWarhead OmegaWarhead;
        public Main Main;
        public SpawnManager SpawnManager;
        public Scp0492Better Scp0492Better;
        public Scp079Better Scp079Better;
        public Prime Prime;
        internal Customize Customize;
        internal BetterAntiCheat BetterAntiCheat;
        internal Addons.RolePlay.Manager RolePlayManager;
        internal ClansWars.Manager ClansWarsManager;
        internal static Regex[] regices = new Regex[0];
        private Harmony hInstance;
        #endregion
        #region override
        public override System.Version Version => new(6, 6, 6);
        public override System.Version NeededQurreVersion => new(1, 9, 9);
        public override string Developer { get; } = "fydne";
        public override string Name { get; } = "Loli";

        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Fix
        internal void UpdateServers(string ip)
        {
            if (ip == "37.18.21.237")
            {
                if (Port == 7777)
                {
                    ServerID = 2;
                    ServerName = "ff:on";
                    DonateID = 2;
                }
                else if (Port == 7778)
                {
                    ServerID = 1;
                    ServerName = "ff:off";
                    DonateID = 2;
                }
                else if (Port == 7779)
                {
                    ServerID = 3;
                    ServerName = "NoRules #1";
                    YTAcess = true;
                    DonateID = 1;
                }
                else if (Port == 7666)
                {
                    ServerID = 8;
                    ServerName = "Medium RP";
                    RolePlay = true;
                }
                else if (Port == 7780)
                {
                    ServerID = 10;
                    ServerName = "Anarchy";
                    Anarchy = true;
                    YTAcess = true;
                    DonateID = 2;
                }
                else if (Port == 7781)
                {
                    ServerID = 11;
                    ServerName = "Hard RP";
                    RolePlay = true;
                    HardRP = true;
                }
                else if (Port == 7782)
                {
                    ServerID = 12;
                    ServerName = "YT Server";
                    YTAcess = true;
                    YouTubersServer = true;
                }
                else if (Port == 7788)
                {
                    ServerID = 9;
                    ServerName = "Клановые Войны";
                    ClansWars = true;
                }
            }
            else if (ip == "212.22.85.116")
            {
                if (Port == 7666)
                {
                    ServerID = 8;
                    ServerName = "Medium RP";
                    RolePlay = true;
                }
                else if (Port == 7780)
                {
                    ServerID = 10;
                    ServerName = "Anarchy";
                    Anarchy = true;
                    YTAcess = true;
                    DonateID = 2;
                }
                else if (Port == 7781)
                {
                    ServerID = 11;
                    ServerName = "Hard RP";
                    RolePlay = true;
                    HardRP = true;
                }
                else if (Port == 7782)
                {
                    ServerID = 12;
                    ServerName = "YT Server";
                    YTAcess = true;
                    YouTubersServer = true;
                }
            }
            else if (Port == 7779)
            {
                ServerName = "NoRules";
                if (ip == "212.22.92.124")
                {
                    ServerID = 4;
                    ServerName = "NoRules #2";
                    YTAcess = true;
                    DonateID = 1;
                }
            }
        }
        private void LoopFix()
        {
            while (ServerID == 0)
            {
                Thread.Sleep(5000);
                UpdateServers("https://checkip.fydne.xyz".HttpGet());
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
            this.hInstance = new Harmony("fydne.loli");
            this.hInstance.PatchAll();

            string ownerip = ServerConsole.Ip;
            UpdateServers(ownerip);
            if (ServerID == 0) new Thread(() => LoopFix()).Start();

            EventHandlers = new EventHandlers(this);
            DataBase = new Manager(this);
            shop = new Shop();
            Levels = new Levels();
            ScpHeal = new ScpHeal();
            Spawner = new Spawner();
            Spawn = new Spawn();
            Force = new Force();
            Icom = new Icom();
            BroadCasts = new BroadCasts();
            Commands = new Commands();
            LogsEvents = new Events(this);
            Scp035 = new Scp035(this);
            Stalky = new Stalky();
            Main = new Main(this);
            SpawnManager = new SpawnManager();
            OmegaWarhead = new OmegaWarhead();
            Scp0492Better = new Scp0492Better();
            Scp079Better = new Scp079Better();
            Prime = new Prime();
            Customize = new Customize();
            BetterAntiCheat = new BetterAntiCheat();
            RolePlayManager = new Addons.RolePlay.Manager();
            ClansWarsManager = new ClansWars.Manager();

            try { CommandsSystem.RegisterRemoteAdmin("oban", OfflineBan.Send); } catch { }
            try { Loli.DataBase.Modules.Patrol.Init(); } catch { }

            Qurre.Events.Server.SendingConsole += CommandsSystem.ConsoleInvoke;
            Qurre.Events.Server.SendingRA += CommandsSystem.RAInvoke;

            Qurre.Events.Round.Waiting += EventHandlers.WaitingForPlayers;
            Qurre.Events.Round.Waiting += EventHandlers.WaitingPlayers;
            Qurre.Events.Round.Waiting += EventHandlers.Waiting;
            Qurre.Events.Round.Waiting += EventHandlers.FixItemsLimits;
            Qurre.Events.Round.Waiting += EventHandlers.LoadTextures;
            Qurre.Events.Round.End += EventHandlers.RoundEnd;
            Qurre.Events.Alpha.Detonated += EventHandlers.Detonated;
            Qurre.Events.Alpha.Stopping += EventHandlers.AntiDisable;
            Qurre.Events.Round.Waiting += EventHandlers.AlphaRefresh;
            Qurre.Events.Round.Start += EventHandlers.AlphaRefresh;
            Qurre.Events.Map.MTFAnnouncement += EventHandlers.AnnouncingMTF;

            Qurre.Events.Player.TeslaTrigger += EventHandlers.Tesla;
            Qurre.Events.Player.InteractLift += EventHandlers.Elevator;
            Qurre.Events.Player.Join += EventHandlers.Join;
            Qurre.Events.Player.Leave += EventHandlers.Leave;
            Qurre.Events.Round.End += EventHandlers.ClearIps;
            Qurre.Events.Player.Dies += EventHandlers.Dying;
            Qurre.Events.Player.Damage += EventHandlers.Hurt;
            Qurre.Events.Report.Cheater += EventHandlers.CheaterReport;
            Qurre.Events.Report.Local += EventHandlers.LocalReport;
            Qurre.Events.Player.Join += EventHandlers.AntiCheaters;
            Qurre.Events.Player.Join += EventHandlers.AntiMaybeCheaters;
            Qurre.Events.Player.Spawn += EventHandlers.AntiMaybeCheaters;
            Qurre.Events.Player.RoleChange += EventHandlers.AntiMaybeCheaters;
            Qurre.Events.Player.DamageProcess += EventHandlers.ForProSkillPlayersYes;

            RolePlayManager.RegisterEvents();

            ClansWarsManager.Initizialize();

            Qurre.Events.Round.Waiting += Textures.Load.Waiting;
            Qurre.Events.Round.Waiting += Textures.Load.RoundStart;
            Qurre.Events.Round.End += Textures.Load.End;

            Qurre.Events.Alpha.Detonated += Textures.Models.Lift.Detonated;
            Qurre.Events.Round.Start += Textures.Models.Lift.SpawnDoors;
            Qurre.Events.Map.DoorDamage += Textures.Models.Lift.DoorEvents;
            Qurre.Events.Map.DoorLock += Textures.Models.Lift.DoorEvents;
            Qurre.Events.Map.DoorOpen += Textures.Models.Lift.DoorEvents;
            Qurre.Events.Scp079.InteractDoor += Textures.Models.Lift.DoorEvents;
            Qurre.Events.Scp079.LockDoor += Textures.Models.Lift.DoorEvents;
            Qurre.Events.Player.InteractDoor += Textures.Models.Lift.DoorEvents;

            Qurre.Events.Round.Start += Textures.Models.Rooms.Range.RoundStart;
            Qurre.Events.Map.DoorDamage += Textures.Models.Rooms.Range.DoorEvents;
            Qurre.Events.Map.DoorLock += Textures.Models.Rooms.Range.DoorEvents;
            Qurre.Events.Map.DoorOpen += Textures.Models.Rooms.Range.DoorEvents;
            Qurre.Events.Scp079.InteractDoor += Textures.Models.Rooms.Range.DoorEvents;
            Qurre.Events.Scp079.LockDoor += Textures.Models.Rooms.Range.DoorEvents;

            Qurre.Events.Round.Start += shop.Refresh;
            Qurre.Events.Round.End += shop.Refresh;
            Qurre.Events.Player.PickupItem += shop.Pickup;

            Qurre.Events.Round.Start += Spawner.RoundStart;

            Qurre.Events.Player.Spawn += Spawn.Update;

            Qurre.Events.Player.Join += Force.Join;
            Qurre.Events.Player.Spawn += Force.Spawn;

            Qurre.Events.Player.InteractDoor += RemoteKeycard.Door;
            Qurre.Events.Player.InteractLocker += RemoteKeycard.Locker;
            Qurre.Events.Player.InteractGenerator += RemoteKeycard.Generator;

            Qurre.Events.Server.SendingConsole += Commands.Console;

            Qurre.Events.Round.Start += Scp035.RoundStart;
            Qurre.Events.Player.Flashed += Scp035.AntiGrenade;
            Qurre.Events.Player.ScpAttack += Scp035.AntiScpAttack;
            Qurre.Events.Player.PickupItem += Scp035.Pickup;
            Qurre.Events.Player.Dies += Scp035.Dies;
            Qurre.Events.Player.Dead += Scp035.Dead;
            Qurre.Events.Player.DamageProcess += Scp035.Damage;
            Qurre.Events.Scp106.PocketEnter += Scp035.Pocket;
            Qurre.Events.Scp106.FemurBreakerEnter += Scp035.Femur;
            Qurre.Events.Player.Escape += Scp035.Escape;
            Qurre.Events.Player.Leave += Scp035.Leave;
            Qurre.Events.Scp106.Contain += Scp035.Contain;
            Qurre.Events.Player.Cuff += Scp035.Cuff;
            Qurre.Events.Player.InteractGenerator += Scp035.Generator;
            Qurre.Events.Scp106.PocketFailEscape += Scp035.Pocket;
            Qurre.Events.Player.Heal += Scp035.Med;

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

            Qurre.Events.Player.RoleChange += Stalky.SetClass;
            Qurre.Events.Scp106.PortalCreate += Stalky.CreatePortal;

            Main.Register();

            Qurre.Events.Round.Waiting += LogsEvents.Waiting;
            Qurre.Events.Round.Start += LogsEvents.RoundStart;
            Qurre.Events.Round.End += LogsEvents.RoundEnd;

            Qurre.Events.Round.Waiting += SpawnManager.ChaosInsurgency.WaitingForPlayers;
            Qurre.Events.Round.Start += SpawnManager.ChaosInsurgency.RoundStart;
            Qurre.Events.Player.Join += SpawnManager.ChaosInsurgency.DoSpawn;
            Qurre.Events.Player.Dead += SpawnManager.ChaosInsurgency.DoSpawn;
            Qurre.Events.Player.Dies += SpawnManager.ChaosInsurgency.FixDies;
            Qurre.Events.Player.Spawn += SpawnManager.ChaosInsurgency.Spawn;

            Qurre.Events.Player.Spawn += Hacker.FixPos;
            Qurre.Events.Player.RoleChange += Hacker.HackerZero;
            Qurre.Events.Player.Spawn += Hacker.HackerZero;
            Qurre.Events.Player.Dead += Hacker.HackerZero;

            Qurre.Events.Round.Waiting += SpawnManager.MobileTaskForces.Refresh;
            Qurre.Events.Round.Start += SpawnManager.MobileTaskForces.Refresh;
            Qurre.Events.Player.Dead += SpawnManager.MobileTaskForces.Dead;
            Qurre.Events.Player.DamageProcess += SpawnManager.MobileTaskForces.Damage;

            Qurre.Events.Round.Waiting += SpawnManager.SerpentsHand.Refresh;
            Qurre.Events.Round.Start += SpawnManager.SerpentsHand.Refresh;
            Qurre.Events.Player.Leave += SpawnManager.SerpentsHand.Leave;
            Qurre.Events.Scp106.PocketFailEscape += SpawnManager.SerpentsHand.Pocket;
            Qurre.Events.Scp106.PocketEscape += SpawnManager.SerpentsHand.Pocket;
            Qurre.Events.Scp106.PocketEnter += SpawnManager.SerpentsHand.Pocket;
            Qurre.Events.Scp106.FemurBreakerEnter += SpawnManager.SerpentsHand.Femur;
            Qurre.Events.Player.ScpAttack += SpawnManager.SerpentsHand.AntiScpAttack;
            Qurre.Events.Player.DamageProcess += SpawnManager.SerpentsHand.Damage;
            Qurre.Events.Player.Dead += SpawnManager.SerpentsHand.Dead;
            Qurre.Events.Player.RoleChange += SpawnManager.SerpentsHand.Spawn;
            Qurre.Events.Player.Spawn += SpawnManager.SerpentsHand.Spawn;
            Qurre.Events.Player.InteractGenerator += SpawnManager.SerpentsHand.Generator;

            Qurre.Events.Player.Damage += SpawnManager.SpawnProtect;
            Qurre.Events.Scp096.AddTarget += SpawnManager.SpawnProtect;
            Qurre.Events.Scp106.PocketEnter += SpawnManager.SpawnProtect;
            Qurre.Events.Round.Start += SpawnManager.SpawnCor;
            Qurre.Events.Round.End += SpawnManager.SpawnCor;

            Qurre.Events.Player.Spawn += Scp0492Better.Spawn;
            Qurre.Events.Player.DamageProcess += Scp0492Better.Damage;

            Qurre.Events.Player.Spawn += Scp079Better.Spawn;

            Qurre.Events.Player.Dead += Scp008.Dead;
            Qurre.Events.Player.Leave += Scp008.Leave;
            Qurre.Events.Player.RoleChange += Scp008.Spawn;
            Qurre.Events.Player.Spawn += Scp008.Spawn;
            Qurre.Events.Player.ItemUsed += Scp008.Medical;
            Qurre.Events.Player.DamageProcess += Scp008.Damage;

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

            Qurre.Events.Player.InteractDoor += BackupPower.HackActivated;
            Qurre.Events.Player.InteractLift += BackupPower.HackActivated;
            Qurre.Events.Scp079.InteractDoor += BackupPower.HackActivated;
            Qurre.Events.Scp079.InteractLift += BackupPower.HackActivated;
            Qurre.Events.Scp079.InteractTesla += BackupPower.HackActivated;
            Qurre.Events.Scp079.ChangeCamera += BackupPower.HackActivated;
            Qurre.Events.Scp079.Speaker += BackupPower.HackActivated;
            Qurre.Events.Scp079.ElevatorTeleport += BackupPower.HackActivated;
            CommandsSystem.RegisterRemoteAdmin("bp", BackupPower.Ra);
            CommandsSystem.RegisterRemoteAdmin("backup_power", BackupPower.Ra);

            Qurre.Events.Alpha.Starting += OmegaWarhead.AntiAlpha;
            Qurre.Events.Alpha.Stopping += OmegaWarhead.NotDisable;
            Qurre.Events.Alpha.Detonated += OmegaWarhead.AllKill;
            Qurre.Events.Map.UseLift += OmegaWarhead.LiftUse;
            Qurre.Events.Round.Waiting += OmegaWarhead.Waiting;
            Qurre.Events.Round.Start += OmegaWarhead.Refresh;

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

            Qurre.Events.Round.Waiting += AirDrop.Waiting;

            Qurre.Events.Round.Waiting += Gate3.Load;
            Qurre.Events.Player.Damage += Gate3.AntiMachineDead;

            Qurre.Events.Round.Start += BetterAirLocks.Init;

            Qurre.Events.Effect.Enabled += Nimb.Update;
            Qurre.Events.Effect.Disabled += Nimb.Update;
            Qurre.Events.Player.Leave += Nimb.Leave;
            Qurre.Events.Player.RoleChange += Nimb.RoleChange;
            Qurre.Events.Player.Spawn += Nimb.RoleChange;
            Qurre.Events.Round.Waiting += Nimb.Waiting;

            Qurre.Events.Effect.Enabled += Glow.Update;
            Qurre.Events.Effect.Disabled += Glow.Update;
            Qurre.Events.Player.Leave += Glow.Leave;
            Qurre.Events.Player.RoleChange += Glow.RoleChange;
            Qurre.Events.Player.Spawn += Glow.RoleChange;
            Qurre.Events.Round.Waiting += Glow.Waiting;

            Qurre.Events.Round.Waiting += WaitingMusic.Waiting;
            Qurre.Events.Round.Start += WaitingMusic.RoundStart;

            Qurre.Events.Player.DamageProcess += Priest.Damage;
            Qurre.Events.Player.DamageProcess += Priest.NoPriestTk;
            Qurre.Events.Scp106.PocketEnter += Priest.No106;
            Qurre.Events.Player.ScpAttack += Priest.NoSCPs;

            Qurre.Events.Round.Start += EventHandlers.AntiDisBalanceItems;
            Qurre.Events.Player.ItemChange += EventHandlers.FixLogicer;
            Qurre.Events.Player.Zooming += EventHandlers.FixLogicer;
            Qurre.Events.Player.PickupItem += EventHandlers.AntiWaiting;
            Qurre.Events.Player.InteractDoor += EventHandlers.AntiWaiting;
            Qurre.Events.Player.RagdollSpawn += EventHandlers.AntiWaiting;
            Qurre.Events.Player.DroppingItem += EventHandlers.AntiWaiting;
            Qurre.Events.Player.ThrowItem += EventHandlers.AntiWaiting;
            Qurre.Events.Map.NewBlood += EventHandlers.AntiWaiting;
            Qurre.Events.Map.CreatePickup += EventHandlers.AntiWaiting;
            Qurre.Events.Scp914.Upgrade += EventHandlers.Scp914;
            Qurre.Events.Round.Start += EventHandlers.ChopperRefresh;
            Qurre.Events.Round.End += EventHandlers.ChopperRefresh;
            Qurre.Events.Round.Start += EventHandlers.ClearRefresh;
            Qurre.Events.Round.End += EventHandlers.ClearRefresh;
            Qurre.Events.Player.Cuff += EventHandlers.AntiTeamCuff;
            Qurre.Events.Player.Dead += EventHandlers.DoSpawn;
            Qurre.Events.Player.ItemUsed += EventHandlers.Adrenaline;
            Qurre.Events.Player.RoleChange += EventHandlers.Spawn;
            Qurre.Events.Player.Spawn += EventHandlers.Spawn;
            Qurre.Events.Player.ItemUsed += EventHandlers.Scp500;
            Qurre.Events.Round.Waiting += EventHandlers.AdrenalineRefresh;
            Qurre.Events.Round.Start += EventHandlers.AdrenalineRefresh;
            Qurre.Events.Round.End += EventHandlers.ClearRam;
            Qurre.Events.Player.Escape += EventHandlers.Escape;
            Qurre.Events.Scp096.Windup += EventHandlers.FixZeroTargets;
            Qurre.Events.Scp096.PreWindup += EventHandlers.FixZeroTargets;
            Qurre.Events.Scp096.AddTarget += EventHandlers.FixRageScp096;
            Qurre.Events.Player.DamageProcess += EventHandlers.EndFF;
            Qurre.Events.Scp106.PocketEnter += EventHandlers.AntiCicleDamage;
            Qurre.Events.Scp106.PocketEscape += EventHandlers.Scp106Lure;
            Qurre.Events.Scp106.PocketFailEscape += EventHandlers.TeleportLure;
            Qurre.Events.Player.Escape += EventHandlers.AntiEscapeBag;
            Qurre.Events.Map.NewBlood += EventHandlers.AntiBloodFlood;
            Qurre.Events.Scp106.Contain += EventHandlers.AntiScp106Bag;
            Qurre.Events.Round.Check += EventHandlers.RoundEnding;
            Qurre.Events.Server.SendingRA += EventHandlers.AntiBan;
            Qurre.Events.Map.DoorDamage += EventHandlers.DoorAntiBreak;
            Qurre.Events.Map.DoorLock += EventHandlers.DoorAntiLock;
            Qurre.Events.Map.DoorOpen += EventHandlers.DoorAntiOpen;
            Qurre.Events.Map.ConvertUnitName += EventHandlers.ScpDeadFix;
            Qurre.Events.Player.DropAmmo += EventHandlers.AntiFlood;
            Qurre.Events.Map.CreatePickup += EventHandlers.AntiAmmo;
            Qurre.Events.Player.Dies += EventHandlers.FixAmmoFlood;
            Qurre.Events.Player.DamageProcess += EventHandlers.FixFF;
            Qurre.Events.Player.SinkholeWalking += EventHandlers.SinkHole;
            Qurre.Events.Player.TantrumWalking += EventHandlers.Tantrum;
            Qurre.Events.Scp914.UpgradePickup += EventHandlers.BalanceGun914;
            Qurre.Events.Scp914.UpgradedItemInventory += EventHandlers.BalanceGun914;
            Qurre.Events.Scp914.UpgradedItemPickup += EventHandlers.BalanceGun914;
            Qurre.Events.Player.ItemChange += EventHandlers.FixInvisible;
            Qurre.Events.Player.Shooting += EventHandlers.FixInvisible;
            Qurre.Events.Player.RagdollSpawn += EventHandlers.FixRagdollScale;
            Qurre.Events.Player.Dies += EventHandlers.UpdateDeaths;
            Qurre.Events.Voice.PressAltChat += EventHandlers.Voice;
            Qurre.Events.Player.PickupItem += EventHandlers.FixFreeze;

            /*Qurre.Events.Round.Waiting += BetterAntiCheat.Refresh;
            Qurre.Events.Round.Start += BetterAntiCheat.Refresh;
            Qurre.Events.Player.RoleChange += BetterAntiCheat.DisableNoclip;
            Qurre.Events.Player.Spawn += BetterAntiCheat.DisableNoclip;
            Qurre.Events.Player.Dead += BetterAntiCheat.DisableNoclip;
            Qurre.Events.Player.SyncData += BetterAntiCheat.DisableNoclip;
            Qurre.Events.Player.Dies += BetterAntiCheat.DisableNoclip;
            Qurre.Events.Player.TransmitPlayerData += BetterAntiCheat.AntiWallHack;
            Qurre.Events.Player.DamageProcess += BetterAntiCheat.AntiAimBot;
            Qurre.Events.Player.Shooting += BetterAntiCheat.AntiSpamShoot;
            Qurre.Events.Player.DamageProcess += BetterAntiCheat.AntiDamageCheat;*/

            Qurre.Events.Player.DamageProcess += EventHandlers.DamageHint;

            Scps.Scp294.Events.Init();

            CommandsSystem.RegisterRemoteAdmin("scp343", God.Ra);

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