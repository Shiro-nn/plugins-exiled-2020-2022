using System.Text.RegularExpressions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using PlayerXP.player;
using PlayerXP.items;
using PlayerXP.heal;
using PlayerXP.cspawn;
using PlayerXP.force;
using PlayerXP.remote;
using PlayerXP.icom;
using PlayerXP.scp343;
using PlayerXP.scp228;
using HarmonyLib;
using PlayerXP.bc;
using PlayerXP.console;
using PlayerXP.events;
using PlayerXP.events.hideandseek;
using PlayerXP.events.Poltergeist;
using PlayerXP.oban;
using PlayerXP.gate3;
using PlayerXP.gate3.editor;

namespace PlayerXP
{
    public class Plugin : Plugin<Config>
    {
        public static donate donatestatic;



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
        public mtfvsci mtfvsci;
        public Massacre Massacre;
        private EventHandlersP EventHandlersP;
        private ban ban;
        public gate3p gate3p;
        public gate3e gate3e;
        internal static Regex[] regices = new Regex[0];
        private Harmony hInstance;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        public override void OnEnabled()
        {
            base.OnEnabled();

            RegisterEvents();
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            UnregisterEvents();
        }
        private void RegisterEvents()
        {
            this.hInstance = new Harmony("fydne.mongodb");
            this.hInstance.PatchAll();
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
            mtfvsci = new mtfvsci(this);
            Massacre = new Massacre(this);
            EventHandlersP = new EventHandlersP(this);
            ban = new ban(this);
            gate3p = new gate3p(this);
            gate3e = new gate3e(this);

            donatestatic = donate;

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.roundend;

            Exiled.Events.Handlers.Player.TriggeringTesla += EventHandlers.tesla;
            Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;

            Exiled.Events.Handlers.Player.UsingMedicalItem += med.medical;
            Exiled.Events.Handlers.Player.Died += died.die;
            Exiled.Events.Handlers.Server.WaitingForPlayers += donate.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Died += donate.OnPlayerDeath;
            Exiled.Events.Handlers.Server.RoundEnded += donate.RoundEnd;
            Exiled.Events.Handlers.Player.Banning += donate.ban;
            Exiled.Events.Handlers.Player.Kicking += donate.kick;
            Exiled.Events.Handlers.Player.Left += donate.left;
            Exiled.Events.Handlers.Player.Joined += donate.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning += donate.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping += donate.OnCheckEscape;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += donate.OnPocketDimensionDie;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += donate.console;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += donate.ra;

            Exiled.Events.Handlers.Server.RoundStarted += shop.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += shop.OnRoundEnd;
            Exiled.Events.Handlers.Player.PickingUpItem += shop.pickup;

            Exiled.Events.Handlers.Player.Shooting += physic.Shoot;

            Exiled.Events.Handlers.Server.RoundStarted += scpheal.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += scpheal.RoundEnd;

            Exiled.Events.Handlers.Server.RoundStarted += spawns.OnRoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor += spawns.RunOnDoorOpen;

            Exiled.Events.Handlers.Player.Spawning += spawn.nspawn;

            Exiled.Events.Handlers.Server.RoundStarted += forceclass.OnRoundStart;
            Exiled.Events.Handlers.Player.Joined += forceclass.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning += forceclass.OnPlayerSpawn;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += forceclass.OnConsoleCommand;

            Exiled.Events.Handlers.Player.InteractingDoor += keycard.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.InteractingLocker += keycard.OnLockerInteraction;
            Exiled.Events.Handlers.Player.UnlockingGenerator += keycard.OnGenOpen;

            Exiled.Events.Handlers.Server.RoundEnded += text.RoundEnd;
            Exiled.Events.Handlers.Server.WaitingForPlayers += text.WaitingForPlayers;

            Exiled.Events.Handlers.Player.Hurting += hurte.hurt;

            scp343.Configs.ReloadConfig();
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers343.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers343.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers343.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound += EventHandlers343.OnRoundRestart;
            Exiled.Events.Handlers.Player.Died += EventHandlers343.OnPlayerDie;
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers343.OnCheckRoundEnd;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers343.OnPlayerHurt;
            Exiled.Events.Handlers.Scp096.Enraging += EventHandlers343.scpzeroninesixe;
            Exiled.Events.Handlers.Player.Shooting += EventHandlers343.OnShoot;
            Exiled.Events.Handlers.Player.Escaping += EventHandlers343.OnCheckEscape;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers343.OnSetClass;
            Exiled.Events.Handlers.Player.Left += EventHandlers343.OnPlayerLeave;
            Exiled.Events.Handlers.Scp106.Containing += EventHandlers343.OnContain106;
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
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers228.OnCheckRoundEnd;
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
            Exiled.Events.Handlers.Player.InteractingDoor += EventHandlers228.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Handcuffing += EventHandlers228.OnPlayerHandcuffed;

            Exiled.Events.Handlers.Server.RoundStarted += mec.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += mec.OnRoundEnd;

            Exiled.Events.Handlers.Server.SendingConsoleCommand += cmsg.console;
            Exiled.Events.Handlers.Server.RoundStarted += cmsg.roundstart;
            Exiled.Events.Handlers.Server.RoundEnded += cmsg.roundend;

            Exiled.Events.Handlers.Server.WaitingForPlayers += mtfvsci.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += mtfvsci.roundstart;
            Exiled.Events.Handlers.Server.RoundEnded += mtfvsci.roundend;
            Exiled.Events.Handlers.Player.Joined += mtfvsci.OnPlayerJoin;
            Exiled.Events.Handlers.Server.EndingRound += mtfvsci.OnCheckRoundEnd;
            Exiled.Events.Handlers.Player.Died += mtfvsci.OnPlayerDie;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += mtfvsci.OnRaCommand;

            Massacre.regEvents();

            Exiled.Events.Handlers.Warhead.Starting += EventHandlersP.OnWarheadStart;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlersP.OnRoundStart;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += ban.ra;

            gate3p.RegisterEvents();

            gate3e.RegisterEvents();
        }
        private void UnregisterEvents()
        {
            this.hInstance.UnpatchAll(null);
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.roundend;

            Exiled.Events.Handlers.Player.TriggeringTesla -= EventHandlers.tesla;
            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;

            Exiled.Events.Handlers.Player.UsingMedicalItem -= med.medical;
            Exiled.Events.Handlers.Player.Died -= died.die;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= donate.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Died -= donate.OnPlayerDeath;
            Exiled.Events.Handlers.Server.RoundEnded -= donate.RoundEnd;
            Exiled.Events.Handlers.Player.Banning -= donate.ban;
            Exiled.Events.Handlers.Player.Kicking -= donate.kick;
            Exiled.Events.Handlers.Player.Left -= donate.left;
            Exiled.Events.Handlers.Player.Joined -= donate.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= donate.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping -= donate.OnCheckEscape;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= donate.OnPocketDimensionDie;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= donate.console;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= donate.ra;

            Exiled.Events.Handlers.Server.RoundStarted -= shop.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= shop.OnRoundEnd;
            Exiled.Events.Handlers.Player.PickingUpItem -= shop.pickup;

            Exiled.Events.Handlers.Player.Shooting -= physic.Shoot;

            Exiled.Events.Handlers.Server.RoundStarted -= scpheal.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= scpheal.RoundEnd;

            Exiled.Events.Handlers.Server.RoundStarted -= spawns.OnRoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor -= spawns.RunOnDoorOpen;

            Exiled.Events.Handlers.Player.Spawning -= spawn.nspawn;

            Exiled.Events.Handlers.Server.RoundStarted -= forceclass.OnRoundStart;
            Exiled.Events.Handlers.Player.Joined -= forceclass.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Spawning -= forceclass.OnPlayerSpawn;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= forceclass.OnConsoleCommand;

            Exiled.Events.Handlers.Player.InteractingDoor -= keycard.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.InteractingLocker -= keycard.OnLockerInteraction;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= keycard.OnGenOpen;

            Exiled.Events.Handlers.Server.RoundEnded -= text.RoundEnd;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= text.WaitingForPlayers;

            Exiled.Events.Handlers.Player.Hurting -= hurte.hurt;

            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers343.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers343.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers343.OnRoundEnd;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers343.OnRoundRestart;
            Exiled.Events.Handlers.Player.Died -= EventHandlers343.OnPlayerDie;
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers343.OnCheckRoundEnd;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers343.OnPlayerHurt;
            Exiled.Events.Handlers.Scp096.Enraging -= EventHandlers343.scpzeroninesixe;
            Exiled.Events.Handlers.Player.Shooting -= EventHandlers343.OnShoot;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers343.OnCheckEscape;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers343.OnSetClass;
            Exiled.Events.Handlers.Player.Left -= EventHandlers343.OnPlayerLeave;
            Exiled.Events.Handlers.Scp106.Containing -= EventHandlers343.OnContain106;
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
            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers228.OnCheckRoundEnd;
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
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandlers228.RunOnDoorOpen;
            Exiled.Events.Handlers.Player.Handcuffing -= EventHandlers228.OnPlayerHandcuffed;

            Exiled.Events.Handlers.Server.RoundStarted -= mec.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= mec.OnRoundEnd;

            Exiled.Events.Handlers.Server.SendingConsoleCommand -= cmsg.console;
            Exiled.Events.Handlers.Server.RoundStarted -= cmsg.roundstart;
            Exiled.Events.Handlers.Server.RoundEnded -= cmsg.roundend;

            Exiled.Events.Handlers.Server.WaitingForPlayers -= mtfvsci.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= mtfvsci.roundstart;
            Exiled.Events.Handlers.Server.RoundEnded -= mtfvsci.roundend;
            Exiled.Events.Handlers.Player.Joined -= mtfvsci.OnPlayerJoin;
            Exiled.Events.Handlers.Server.EndingRound -= mtfvsci.OnCheckRoundEnd;
            Exiled.Events.Handlers.Player.Died -= mtfvsci.OnPlayerDie;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= mtfvsci.OnRaCommand;

            Massacre.unregEvents();

            Exiled.Events.Handlers.Warhead.Starting -= EventHandlersP.OnWarheadStart;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlersP.OnRoundStart;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= ban.ra;

            gate3p.UnregisterEvents();

            gate3e.UnregisterEvents();

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
            mec = null;
            cmsg = null;
            mtfvsci = null;
            Massacre = null;
            EventHandlersP = null;
            ban = null;
            gate3p = null;
            gate3e = null;

            donatestatic = null;
        }
    }
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
	}
}