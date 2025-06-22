using GameCore;
using MEC;
using Handlers = Exiled.Events.Handlers;
namespace MongoDB.logs
{
    public class DiscordLogs
    {
        public Plugin plugin;
        public static Plugin statplug;
        public DiscordLogs(Plugin plugin)
        {
            this.plugin = plugin;
            statplug = plugin;
        }

        public int MaxPlayers = ConfigFile.ServerConfig.GetInt("max_players", 100);
        private EventHandlers EventHandlers;
        public void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Handlers.Map.Decontaminating += EventHandlers.OnDecon;
            Handlers.Map.GeneratorActivated += EventHandlers.OnGenFinish;
            Handlers.Warhead.Starting += EventHandlers.OnWarheadStart;
            Handlers.Warhead.Stopping += EventHandlers.OnWarheadCancelled;
            Handlers.Warhead.Detonated += EventHandlers.OnWarheadDetonation;
            Handlers.Scp914.UpgradingItems += EventHandlers.OnScp194Upgrade;

            Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnCommand;
            Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Handlers.Server.SendingConsoleCommand += EventHandlers.OnConsoleCommand;
            Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
            Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
            Handlers.Server.RespawningTeam += EventHandlers.OnRespawn;
            Handlers.Server.ReportingCheater += EventHandlers.OnCheaterReport;

            Handlers.Scp914.ChangingKnobSetting += EventHandlers.On914KnobChange;
            Handlers.Player.UsingMedicalItem += EventHandlers.OnMedicalItem;
            Handlers.Scp079.InteractingTesla += EventHandlers.On079Tesla;
            Handlers.Player.PickingUpItem += EventHandlers.OnPickupItem;
            Handlers.Player.InsertingGeneratorTablet += EventHandlers.OnGenInsert;
            Handlers.Player.EjectingGeneratorTablet += EventHandlers.OnGenEject;
            Handlers.Player.UnlockingGenerator += EventHandlers.OnGenUnlock;
            Handlers.Player.OpeningGenerator += EventHandlers.OnGenOpen;
            Handlers.Player.ClosingGenerator += EventHandlers.OnGenClosed;
            Handlers.Scp079.GainingLevel += EventHandlers.On079GainLvl;
            Handlers.Scp079.GainingExperience += EventHandlers.On079GainExp;
            Handlers.Player.EscapingPocketDimension += EventHandlers.OnPocketEscape;
            Handlers.Player.EnteringPocketDimension += EventHandlers.OnPocketEnter;
            Handlers.Scp106.CreatingPortal += EventHandlers.On106CreatePortal;
            Handlers.Player.ActivatingWarheadPanel += EventHandlers.OnWarheadAccess;
            Handlers.Player.TriggeringTesla += EventHandlers.OnTriggerTesla;
            Handlers.Player.ThrowingGrenade += EventHandlers.OnGrenadeThrown;
            Handlers.Player.Hurting += EventHandlers.OnPlayerHurt;
            Handlers.Player.Died += EventHandlers.OnPlayerDeath;
            Handlers.Player.Banned += EventHandlers.OnPlayerBanned;
            Handlers.Player.InteractingDoor += EventHandlers.OnDoorInteract;
            Handlers.Player.InteractingElevator += EventHandlers.OnElevatorInteraction;
            Handlers.Player.InteractingLocker += EventHandlers.OnLockerInteraction;
            Handlers.Player.IntercomSpeaking += EventHandlers.OnIntercomSpeak;
            Handlers.Player.Handcuffing += EventHandlers.OnPlayerHandcuffed;
            Handlers.Player.RemovingHandcuffs += EventHandlers.OnPlayerFreed;
            Handlers.Scp106.Teleporting += EventHandlers.On106Teleport;
            Handlers.Player.ReloadingWeapon += EventHandlers.OnPlayerReload;
            Handlers.Player.ItemDropped += EventHandlers.OnDropItem;
            Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
            Handlers.Player.Left += EventHandlers.OnPlayerLeave;
            Handlers.Player.ChangingRole += EventHandlers.OnSetClass;
            Handlers.Player.ChangingGroup += EventHandlers.OnSetGroup;
            Handlers.Player.ChangingItem += EventHandlers.OnItemChanged;
            Handlers.Scp914.Activating += EventHandlers.On914Activation;
            Handlers.Scp106.Containing += EventHandlers.On106Contain;
        }
        public void UnregisterEvents()
        {
            Handlers.Map.Decontaminating -= EventHandlers.OnDecon;
            Handlers.Map.GeneratorActivated -= EventHandlers.OnGenFinish;
            Handlers.Warhead.Starting -= EventHandlers.OnWarheadStart;
            Handlers.Warhead.Stopping -= EventHandlers.OnWarheadCancelled;
            Handlers.Warhead.Detonated -= EventHandlers.OnWarheadDetonation;
            Handlers.Scp914.UpgradingItems -= EventHandlers.OnScp194Upgrade;

            Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnCommand;
            Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Handlers.Server.SendingConsoleCommand -= EventHandlers.OnConsoleCommand;
            Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Handlers.Server.RespawningTeam -= EventHandlers.OnRespawn;
            Handlers.Server.ReportingCheater -= EventHandlers.OnCheaterReport;

            Handlers.Scp914.ChangingKnobSetting -= EventHandlers.On914KnobChange;
            Handlers.Player.UsingMedicalItem -= EventHandlers.OnMedicalItem;
            Handlers.Scp079.InteractingTesla -= EventHandlers.On079Tesla;
            Handlers.Player.PickingUpItem -= EventHandlers.OnPickupItem;
            Handlers.Player.InsertingGeneratorTablet -= EventHandlers.OnGenInsert;
            Handlers.Player.EjectingGeneratorTablet -= EventHandlers.OnGenEject;
            Handlers.Player.UnlockingGenerator -= EventHandlers.OnGenUnlock;
            Handlers.Player.OpeningGenerator -= EventHandlers.OnGenOpen;
            Handlers.Player.ClosingGenerator -= EventHandlers.OnGenClosed;
            Handlers.Scp079.GainingLevel -= EventHandlers.On079GainLvl;
            Handlers.Scp079.GainingExperience -= EventHandlers.On079GainExp;
            Handlers.Player.EscapingPocketDimension -= EventHandlers.OnPocketEscape;
            Handlers.Player.EnteringPocketDimension -= EventHandlers.OnPocketEnter;
            Handlers.Scp106.CreatingPortal -= EventHandlers.On106CreatePortal;
            Handlers.Player.ActivatingWarheadPanel -= EventHandlers.OnWarheadAccess;
            Handlers.Player.TriggeringTesla -= EventHandlers.OnTriggerTesla;
            Handlers.Player.ThrowingGrenade -= EventHandlers.OnGrenadeThrown;
            Handlers.Player.Hurting -= EventHandlers.OnPlayerHurt;
            Handlers.Player.Died -= EventHandlers.OnPlayerDeath;
            Handlers.Player.Banned -= EventHandlers.OnPlayerBanned;
            Handlers.Player.InteractingDoor -= EventHandlers.OnDoorInteract;
            Handlers.Player.InteractingElevator -= EventHandlers.OnElevatorInteraction;
            Handlers.Player.InteractingLocker -= EventHandlers.OnLockerInteraction;
            Handlers.Player.IntercomSpeaking -= EventHandlers.OnIntercomSpeak;
            Handlers.Player.Handcuffing -= EventHandlers.OnPlayerHandcuffed;
            Handlers.Player.RemovingHandcuffs -= EventHandlers.OnPlayerFreed;
            Handlers.Scp106.Teleporting -= EventHandlers.On106Teleport;
            Handlers.Player.ReloadingWeapon -= EventHandlers.OnPlayerReload;
            Handlers.Player.ItemDropped -= EventHandlers.OnDropItem;
            Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Handlers.Player.Left -= EventHandlers.OnPlayerLeave;
            Handlers.Player.ChangingRole -= EventHandlers.OnSetClass;
            Handlers.Player.ChangingGroup -= EventHandlers.OnSetGroup;
            Handlers.Player.ChangingItem -= EventHandlers.OnItemChanged;
            Handlers.Scp914.Activating -= EventHandlers.On914Activation;
            Handlers.Scp106.Containing -= EventHandlers.On106Contain;

            EventHandlers = null;
            Timing.KillCoroutines("handle");
        }
    }
}
