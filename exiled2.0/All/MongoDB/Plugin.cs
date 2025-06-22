using System.Text.RegularExpressions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using MEC;
#region MongoDB
using MongoDB.player;
using MongoDB.items;
using MongoDB.heal;
using MongoDB.cspawn;
using MongoDB.force;
using MongoDB.remote;
using MongoDB.icom;
using MongoDB.scp343;
using MongoDB.scp228;
using HarmonyLib;
using MongoDB.bc;
using MongoDB.console;
using MongoDB.events.Poltergeist;
using MongoDB.oban;
using MongoDB.logs;
using MongoDB.scp035;
using MongoDB.Juggernaut;
using MongoDB.jc3;
using System.Collections.Generic;
using MongoDB.sh;
using MongoDB.auto_events;
#endregion
namespace MongoDB
{
    public class Plugin : Plugin<Config>
    {
        internal static int ServerID = 0;
        internal static string ServerName = "[data deleted]";
        internal static bool YTAcess = false;
        internal string mongodburl = "mongodb://fydne:cxlkss89asasadaad@mongo.scpsl.store/login?authSource=admin";
        public static List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        #region nostatic
        internal Config config;
        public static YamlConfig cfg;
        public EventHandlers EventHandlers;
        public donate donate;
        public shop shop;
        public physic physic;
        public scpheal scpheal;
        public spawns spawns;
        public spawn spawn;
        public forceclass forceclass;
        public keycard keycard;
        public text text;
        public EventHandlers343 EventHandlers343;
        public EventHandlers228 EventHandlers228;
        public mec mec;
        public cmsg cmsg;
        internal EventHandlersP EventHandlersP;
        private ban ban;
        public shEventHandlers shEventHandlers;
        public DiscordLogs DiscordLogs;
        public Main035 Main035;
        public armor armor;
        public cat_hook cat_hook;
        public stalky.stalky stalky;
        public Main Main;
        internal static Regex[] regices = new Regex[0];
        private Harmony hInstance;
        #endregion
        #region override
        //public override PluginPriority Priority { get; } = PluginPriority.Highest;
        public override PluginPriority Priority { get; } = PluginPriority.Low;
        public override string Author { get; } = "fydne";

        public override void OnEnabled()
        {
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
        }
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();

            RegisterEvents();
        }
        public override void OnUnregisteringCommands()
        {
            base.OnUnregisteringCommands();

            UnregisterEvents();
        }
        #endregion
        #region cfg
        internal void cfg1()
        {
            config = base.Config;
        }
        #endregion
        #region RegEvents
        private void RegisterEvents()
        {
            new Driver.MongoClient(mongodburl).GetDatabase("login").GetCollection<Bson.BsonDocument>("accounts");
            config = base.Config;
            this.hInstance = new Harmony("fydne.mongodb");
            this.hInstance.PatchAll();

            string ownerip = ServerConsole.Ip;
            var url = "http://checkip.dyndns.org";
            var req = System.Net.WebRequest.Create(url);
            var resp = req.GetResponse();
            using (var sr = new System.IO.StreamReader(resp.GetResponseStream()))
            {
                var response = sr.ReadToEnd().Trim();
                var a = response.Split(':');
                var a2 = a[1].Substring(1);
                var a3 = a2.Split('<');
                var a4 = a3[0];
                ownerip = a4;
            }
            string ip = "212.22.92.143";
            string ip2 = "212.22.92.124";
            string ip3 = "212.22.92.125";
            if (ownerip == "212.22.92.124" && Server.Port == 7777)
            {
                ServerID = 1;
                ServerName = "ff:off";
            }
            else if (ownerip == "212.22.92.124" && Server.Port == 7778)
            {
                ServerID = 2;
                ServerName = "ff:on";
            }
            else if (Server.Port == 7779)
            {
                ServerName = "NoRules";
                if (ownerip == "212.22.92.143")
                {
                    ServerID = 3;
                    ServerName = "NoRules #1";
                }
                if (ownerip == "212.22.92.124")
                {
                    ServerID = 4;
                    ServerName = "NoRules #2";
                    YTAcess = true;
                }
                if (ownerip == "212.22.92.123") ServerName = "NoRules #3";
            }
            if (ownerip == ip || ownerip == ip2 || ownerip == ip3)
            {
                EventHandlers = new EventHandlers(this);
                donate = new donate(this);
                shop = new shop(this);
                physic = new physic(this);
                scpheal = new scpheal(this);
                spawns = new spawns(this);
                spawn = new spawn(this);
                forceclass = new forceclass(this);
                keycard = new keycard(this);
                text = new text(this);
                EventHandlers343 = new EventHandlers343(this);
                EventHandlers228 = new EventHandlers228(this);
                mec = new mec(this);
                cmsg = new cmsg(this);
                EventHandlersP = new EventHandlersP(this);
                ban = new ban(this);
                DiscordLogs = new DiscordLogs(this);
                Main035 = new Main035(this);
                shEventHandlers = new shEventHandlers(this);
                armor = new armor(this);
                cat_hook = new cat_hook(this);
                stalky = new stalky.stalky(this);
                Main = new Main(this);
                Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.RoundEnd;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.roundend;
                Exiled.Events.Handlers.Warhead.Detonated += EventHandlers.detonated;
                Exiled.Events.Handlers.Scp914.UpgradingItems += EventHandlers.scp914;
                Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.OnTeamRespawn;
                Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += EventHandlers.AnnouncingMTF;

                Exiled.Events.Handlers.Player.TriggeringTesla += EventHandlers.tesla;
                Exiled.Events.Handlers.Player.InteractingElevator += EventHandlers.Elevator;
                Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
                Exiled.Events.Handlers.Player.Dying += EventHandlers.Dying;
                Exiled.Events.Handlers.Player.Hurting += EventHandlers.Hurt;
                Exiled.Events.Handlers.Server.ReportingCheater += EventHandlers.CheaterReport;
                Exiled.Events.Handlers.Server.LocalReporting += EventHandlers.LocalReport;

                Exiled.Events.Handlers.Player.UsingMedicalItem += med.medical;
                Exiled.Events.Handlers.Player.Died += died.die;

                Exiled.Events.Handlers.Server.RoundStarted += shop.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += shop.OnRoundEnd;
                Exiled.Events.Handlers.Player.PickingUpItem += shop.pickup;

                Exiled.Events.Handlers.Player.Shooting += physic.Shoot;

                Exiled.Events.Handlers.Server.RoundStarted += spawns.OnRoundStart;

                Exiled.Events.Handlers.Player.Spawning += spawn.nspawn;

                Exiled.Events.Handlers.Player.Joined += forceclass.OnPlayerJoin;
                Exiled.Events.Handlers.Player.Spawning += forceclass.OnPlayerSpawn;
                Exiled.Events.Handlers.Server.SendingConsoleCommand += forceclass.OnConsoleCommand;

                Exiled.Events.Handlers.Player.InteractingDoor += keycard.RunOnDoorOpen;
                Exiled.Events.Handlers.Player.InteractingLocker += keycard.OnLockerInteraction;
                Exiled.Events.Handlers.Player.UnlockingGenerator += keycard.OnGenOpen;

                Exiled.Events.Handlers.Player.Hurting += hurte.hurt;

                Exiled.Events.Handlers.Server.RoundStarted += cat_hook.RoundStart;
                Exiled.Events.Handlers.Server.SendingConsoleCommand += cat_hook.Console;
                Exiled.Events.Handlers.Player.PickingUpItem += cat_hook.Pickup;

                scp343.Configs.ReloadConfig();
                Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers343.OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += EventHandlers343.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers343.OnRoundEnd;
                Exiled.Events.Handlers.Player.Died += EventHandlers343.OnPlayerDie;
                Exiled.Events.Handlers.Player.Died += EventHandlers343.OnDied;
                Exiled.Events.Handlers.Player.Hurting += EventHandlers343.OnPlayerHurt;
                Exiled.Events.Handlers.Scp096.Enraging += EventHandlers343.scpzeroninesixe;
                Exiled.Events.Handlers.Scp096.AddingTarget += EventHandlers343.scpzeroninesixeadd;
                Exiled.Events.Handlers.Player.Shooting += EventHandlers343.OnShoot;
                Exiled.Events.Handlers.Player.Escaping += EventHandlers343.OnCheckEscape;
                Exiled.Events.Handlers.Player.ChangingRole += EventHandlers343.OnSetClass;
                Exiled.Events.Handlers.Player.Left += EventHandlers343.OnPlayerLeave;
                Exiled.Events.Handlers.Scp106.Containing += EventHandlers343.OnContain106;
                Exiled.Events.Handlers.Player.EnteringPocketDimension += EventHandlers343.PocketEnter;
                Exiled.Events.Handlers.Player.EscapingPocketDimension += EventHandlers343.OnPocketDimensionEnter;
                Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers343.OnPocketDimensionDie;
                Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers343.RunOnDoorOpen;
                Exiled.Events.Handlers.Player.DroppingItem += EventHandlers343.OnDropItem;
                Exiled.Events.Handlers.Player.Handcuffing += EventHandlers343.OnPlayerHandcuffed;
                Exiled.Events.Handlers.Player.EnteringFemurBreaker += EventHandlers343.OnFemurEnter;
                Exiled.Events.Handlers.Player.PickingUpItem += EventHandlers343.OnPickupItem;
                Exiled.Events.Handlers.Player.Spawning += EventHandlers343.OnTeamRespawn;
                Exiled.Events.Handlers.Warhead.Stopping += EventHandlers343.OnWarheadCancel;
                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers343.ra;
                Exiled.Events.Handlers.Player.UsingMedicalItem += EventHandlers343.medical;
                Exiled.Events.Handlers.Scp914.UpgradingItems += EventHandlers343.scp914;
                Exiled.Events.Handlers.Player.InteractingLocker += EventHandlers343.OnLockerInteraction;
                Exiled.Events.Handlers.Player.UnlockingGenerator += EventHandlers343.OnGenOpen;

                Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers228.console;
                Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers228.OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += EventHandlers228.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers228.OnRoundEnd;
                Exiled.Events.Handlers.Server.RestartingRound += EventHandlers228.OnRoundRestart;
                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers228.RunOnRACommandSent;
                Exiled.Events.Handlers.Player.Died += EventHandlers228.OnPlayerDeath;
                Exiled.Events.Handlers.Player.Hurting += EventHandlers228.OnPlayerHurt;
                Exiled.Events.Handlers.Player.Escaping += EventHandlers228.OnCheckEscape;
                Exiled.Events.Handlers.Player.ChangingRole += EventHandlers228.OnSetClass;
                Exiled.Events.Handlers.Player.Left += EventHandlers228.OnPlayerLeave;
                Exiled.Events.Handlers.Player.EnteringFemurBreaker += EventHandlers228.OnContain106;
                Exiled.Events.Handlers.Player.EnteringPocketDimension += EventHandlers228.OnPocketDimensionEnter;
                Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers228.OnPocketDimensionDie;
                Exiled.Events.Handlers.Player.EscapingPocketDimension += EventHandlers228.OnPocketDimensionExit;
                Exiled.Events.Handlers.Player.PickingUpItem += EventHandlers228.OnPickupItem;
                Exiled.Events.Handlers.Scp096.Enraging += EventHandlers228.scpzeroninesixe;
                Exiled.Events.Handlers.Scp096.AddingTarget += EventHandlers228.scpzeroninesixeadd;
                Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers228.RunOnDoorOpen;
                Exiled.Events.Handlers.Player.Handcuffing += EventHandlers228.OnPlayerHandcuffed;

                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += shEventHandlers.ra;
                Exiled.Events.Handlers.Server.WaitingForPlayers += shEventHandlers.OnWaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += shEventHandlers.OnRoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += shEventHandlers.OnRoundEnd;
                Exiled.Events.Handlers.Player.Joined += shEventHandlers.OnPlayerJoin;
                Exiled.Events.Handlers.Player.Left += shEventHandlers.Leave;
                Exiled.Events.Handlers.Player.FailingEscapePocketDimension += shEventHandlers.OnPocketDimensionDie;
                Exiled.Events.Handlers.Player.EscapingPocketDimension += shEventHandlers.OnPocketDimensionEscaping;
                Exiled.Events.Handlers.Player.EnteringPocketDimension += shEventHandlers.OnPocketDimensionEnter;
                Exiled.Events.Handlers.Player.Hurting += shEventHandlers.hurt;
                Exiled.Events.Handlers.Player.Died += shEventHandlers.died;
                Exiled.Events.Handlers.Player.ChangingRole += shEventHandlers.setrole;
                Exiled.Events.Handlers.Scp096.Enraging += shEventHandlers.scpzeroninesixe;
                Exiled.Events.Handlers.Scp096.AddingTarget += shEventHandlers.scpzeroninesixeadd;

                Exiled.Events.Handlers.Server.RoundStarted += armor.RoundStart;
                Exiled.Events.Handlers.Server.RoundEnded += armor.RoundEnd;
                Exiled.Events.Handlers.Player.PickingUpItem += armor.Pickup;
                Exiled.Events.Handlers.Player.ItemDropped += armor.Drop;
                Exiled.Events.Handlers.Player.Died += armor.Died;
                Exiled.Events.Handlers.Player.Escaping += armor.CheckEscape;
                Exiled.Events.Handlers.Player.UsingMedicalItem += armor.medical;

                Exiled.Events.Handlers.Server.SendingConsoleCommand += cmsg.console;
                Exiled.Events.Handlers.Server.RoundStarted += cmsg.roundstart;
                Exiled.Events.Handlers.Server.RoundEnded += cmsg.roundend;

                Exiled.Events.Handlers.Warhead.Starting += EventHandlersP.OnWarheadStart;
                Exiled.Events.Handlers.Server.RoundStarted += EventHandlersP.OnRoundStart;

                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += ban.ra;

                Main035.RegisterEvents();

                Exiled.Events.Handlers.Player.Dying += died.dying;

                Exiled.Events.Handlers.Server.WaitingForPlayers += donate.WaitingForPlayers;
                Exiled.Events.Handlers.Server.RoundStarted += donate.RoundStarted;
                Exiled.Events.Handlers.Player.Dying += donate.PlayerDeath;
                Exiled.Events.Handlers.Server.RoundEnded += donate.RoundEnd;
                Exiled.Events.Handlers.Player.Banning += donate.Ban;
                Exiled.Events.Handlers.Player.Kicking += donate.Kick;
                Exiled.Events.Handlers.Player.Left += donate.Left;
                Exiled.Events.Handlers.Player.Joined += donate.PlayerJoin;
                Exiled.Events.Handlers.Player.Spawning += donate.PlayerSpawn;
                Exiled.Events.Handlers.Player.Escaping += donate.CheckEscape;
                Exiled.Events.Handlers.Player.FailingEscapePocketDimension += donate.PocketDimensionDie;
                Exiled.Events.Handlers.Player.Died += donate.Dead;
                Exiled.Events.Handlers.Server.SendingConsoleCommand += donate.Console;
                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += donate.Ra;

                Exiled.Events.Handlers.Player.ChangingRole += stalky.SetClass;
                Exiled.Events.Handlers.Scp106.CreatingPortal += stalky.CreatePortal;

                DiscordLogs.RegisterEvents();

                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += donate.CustomRA;
                Exiled.Events.Handlers.Player.Escaping += EventHandlers.AntiEscapeBag;
                Exiled.Events.Handlers.Map.PlacingBlood += EventHandlers.AntiBloodFlood;
                Exiled.Events.Handlers.Scp106.Containing += EventHandlers.AntiScp106Bag;
                Exiled.Events.Handlers.Server.EndingRound += EventHandlers.RoundEnding;
                Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.AntiBan;

                Main.Register();
            }
        }
        #endregion
        #region UnregEvents
        private void UnregisterEvents()
        {
            this.hInstance.UnpatchAll(null);
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.RoundEnd;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.roundend;
            Exiled.Events.Handlers.Warhead.Detonated -= EventHandlers.detonated;
            Exiled.Events.Handlers.Scp914.UpgradingItems -= EventHandlers.scp914;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.OnTeamRespawn;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= EventHandlers.AnnouncingMTF;

            Exiled.Events.Handlers.Player.TriggeringTesla -= EventHandlers.tesla;
            Exiled.Events.Handlers.Player.InteractingElevator -= EventHandlers.Elevator;
            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Dying -= EventHandlers.Dying;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.Hurt;
            Exiled.Events.Handlers.Server.ReportingCheater -= EventHandlers.CheaterReport;
            Exiled.Events.Handlers.Server.LocalReporting -= EventHandlers.LocalReport;

            Exiled.Events.Handlers.Player.UsingMedicalItem -= med.medical;
            Exiled.Events.Handlers.Player.Died -= died.die;

            Exiled.Events.Handlers.Server.RoundStarted -= shop.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= shop.OnRoundEnd;
            Exiled.Events.Handlers.Player.PickingUpItem -= shop.pickup;

            Exiled.Events.Handlers.Player.Shooting -= physic.Shoot;

            Exiled.Events.Handlers.Server.RoundStarted -= spawns.OnRoundStart;

            Exiled.Events.Handlers.Player.Spawning -= spawn.nspawn;

            Exiled.Events.Handlers.Player.Joined -= forceclass.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= forceclass.OnPlayerSpawn;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= forceclass.OnConsoleCommand;

            Exiled.Events.Handlers.Player.InteractingDoor -= keycard.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.InteractingLocker -= keycard.OnLockerInteraction;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= keycard.OnGenOpen;

            Exiled.Events.Handlers.Player.Hurting -= hurte.hurt;

            Exiled.Events.Handlers.Server.RoundStarted -= armor.RoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= armor.RoundEnd;
            Exiled.Events.Handlers.Player.PickingUpItem -= armor.Pickup;
            Exiled.Events.Handlers.Player.ItemDropped -= armor.Drop;
            Exiled.Events.Handlers.Player.Died -= armor.Died;
            Exiled.Events.Handlers.Player.Escaping -= armor.CheckEscape;
            Exiled.Events.Handlers.Player.UsingMedicalItem -= armor.medical;

            Exiled.Events.Handlers.Server.RoundStarted -= cat_hook.RoundStart;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= cat_hook.Console;
            Exiled.Events.Handlers.Player.PickingUpItem -= cat_hook.Pickup;

            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers343.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers343.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers343.OnRoundEnd;
            Exiled.Events.Handlers.Player.Died -= EventHandlers343.OnPlayerDie;
            Exiled.Events.Handlers.Player.Died += EventHandlers343.OnDied;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers343.OnPlayerHurt;
            Exiled.Events.Handlers.Scp096.Enraging -= EventHandlers343.scpzeroninesixe;
            Exiled.Events.Handlers.Scp096.AddingTarget -= EventHandlers343.scpzeroninesixeadd;
            Exiled.Events.Handlers.Player.Shooting -= EventHandlers343.OnShoot;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers343.OnCheckEscape;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers343.OnSetClass;
            Exiled.Events.Handlers.Player.Left -= EventHandlers343.OnPlayerLeave;
            Exiled.Events.Handlers.Scp106.Containing -= EventHandlers343.OnContain106;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= EventHandlers343.PocketEnter;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= EventHandlers343.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers343.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers343.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.DroppingItem -= EventHandlers343.OnDropItem;
            Exiled.Events.Handlers.Player.Handcuffing -= EventHandlers343.OnPlayerHandcuffed;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker -= EventHandlers343.OnFemurEnter;
            Exiled.Events.Handlers.Player.PickingUpItem -= EventHandlers343.OnPickupItem;
            Exiled.Events.Handlers.Player.Spawning -= EventHandlers343.OnTeamRespawn;
            Exiled.Events.Handlers.Warhead.Stopping -= EventHandlers343.OnWarheadCancel;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers343.ra;
            Exiled.Events.Handlers.Player.UsingMedicalItem -= EventHandlers343.medical;
            Exiled.Events.Handlers.Scp914.UpgradingItems -= EventHandlers343.scp914;
            Exiled.Events.Handlers.Player.InteractingLocker -= EventHandlers343.OnLockerInteraction;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= EventHandlers343.OnGenOpen;

            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers228.console;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers228.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers228.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers228.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers228.OnRoundRestart;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers228.RunOnRACommandSent;
            Exiled.Events.Handlers.Player.Died -= EventHandlers228.OnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers228.OnPlayerHurt;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers228.OnCheckEscape;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers228.OnSetClass;
            Exiled.Events.Handlers.Player.Left -= EventHandlers228.OnPlayerLeave;
            Exiled.Events.Handlers.Player.EnteringFemurBreaker -= EventHandlers228.OnContain106;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= EventHandlers228.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers228.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= EventHandlers228.OnPocketDimensionExit;
            Exiled.Events.Handlers.Player.PickingUpItem -= EventHandlers228.OnPickupItem;
            Exiled.Events.Handlers.Scp096.Enraging -= EventHandlers228.scpzeroninesixe;
            Exiled.Events.Handlers.Scp096.AddingTarget -= EventHandlers228.scpzeroninesixeadd;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers228.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Handcuffing -= EventHandlers228.OnPlayerHandcuffed;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= shEventHandlers.ra;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= shEventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= shEventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= shEventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Player.Joined -= shEventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Left -= shEventHandlers.Leave;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= shEventHandlers.OnPocketDimensionDie;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= shEventHandlers.OnPocketDimensionEscaping;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= shEventHandlers.OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.Hurting -= shEventHandlers.hurt;
            Exiled.Events.Handlers.Player.Died -= shEventHandlers.died;
            Exiled.Events.Handlers.Player.ChangingRole -= shEventHandlers.setrole;
            Exiled.Events.Handlers.Scp096.Enraging -= shEventHandlers.scpzeroninesixe;
            Exiled.Events.Handlers.Scp096.AddingTarget -= shEventHandlers.scpzeroninesixeadd;

            Exiled.Events.Handlers.Server.SendingConsoleCommand -= cmsg.console;
            Exiled.Events.Handlers.Server.RoundStarted -= cmsg.roundstart;
            Exiled.Events.Handlers.Server.RoundEnded -= cmsg.roundend;

            Exiled.Events.Handlers.Warhead.Starting -= EventHandlersP.OnWarheadStart;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlersP.OnRoundStart;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= ban.ra;

            DiscordLogs.UnregisterEvents();

            Main035.UnregisterEvents();

            Exiled.Events.Handlers.Player.Dying -= died.dying;

            Exiled.Events.Handlers.Server.WaitingForPlayers -= donate.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= donate.RoundStarted;
            Exiled.Events.Handlers.Player.Dying -= donate.PlayerDeath;
            Exiled.Events.Handlers.Server.RoundEnded -= donate.RoundEnd;
            Exiled.Events.Handlers.Player.Banning -= donate.Ban;
            Exiled.Events.Handlers.Player.Kicking -= donate.Kick;
            Exiled.Events.Handlers.Player.Left -= donate.Left;
            Exiled.Events.Handlers.Player.Joined -= donate.PlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= donate.PlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping -= donate.CheckEscape;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= donate.PocketDimensionDie;
            Exiled.Events.Handlers.Player.Died -= donate.Dead;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= donate.Console;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= donate.Ra;

            Exiled.Events.Handlers.Player.ChangingRole -= stalky.SetClass;
            Exiled.Events.Handlers.Scp106.CreatingPortal -= stalky.CreatePortal;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= donate.CustomRA;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers.AntiEscapeBag;
            Exiled.Events.Handlers.Map.PlacingBlood -= EventHandlers.AntiBloodFlood;
            Exiled.Events.Handlers.Scp106.Containing -= EventHandlers.AntiScp106Bag;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.RoundEnding;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.AntiBan;

            Main.UnRegister();

            EventHandlers = null;
            donate = null;
            shop = null;
            physic = null;
            scpheal = null;
            spawns = null;
            spawn = null;
            forceclass = null;
            keycard = null;
            text = null;
            EventHandlers343 = null;
            EventHandlers228 = null;
            shEventHandlers = null;
            mec = null;
            cmsg = null;
            EventHandlersP = null;
            ban = null;
            DiscordLogs = null;
            Main035 = null;
            armor = null;
            cat_hook = null;
            stalky = null;
            Main = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}