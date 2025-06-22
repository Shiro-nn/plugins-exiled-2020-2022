using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;
using HarmonyLib;
using MEC;
using Player = Exiled.Events.Handlers.Player;
using Scp096 = Exiled.Events.Handlers.Scp096;
using Server = Exiled.Events.Handlers.Server;
namespace scp035
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        public EventHandlers EventHandlers;
        private Harmony hInstance;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Low;
        public override string Author { get; } = "fydne";
        public override void OnEnabled() => base.OnEnabled();
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
            cfg1();
            RegisterEvents();
        }
        public override void OnUnregisteringCommands()
        {
            base.OnUnregisteringCommands();
            UnregisterEvents();
        }
        #endregion
        #region cfg
        internal void cfg1() => config = base.Config;
        #endregion
        #region Events
        private void RegisterEvents()
        {
            this.hInstance = new Harmony("fydne.scp035");
            this.hInstance.PatchAll();
            EventHandlers = new EventHandlers(this);
            Server.WaitingForPlayers += EventHandlers.WFP;
            Server.RoundStarted += EventHandlers.OnRoundStart;
            Player.PickingUpItem += EventHandlers.OnPickupItem;
            Player.Died += EventHandlers.OnPlayerDie;
            Player.Hurting += EventHandlers.OnPlayerHurt;
            Player.EnteringPocketDimension += EventHandlers.OnPocketDimensionEnter;
            Player.EnteringFemurBreaker += EventHandlers.OnFemurBreaker;
            Player.Escaping += EventHandlers.OnCheckEscape;
            Player.ChangingRole += EventHandlers.OnSetClass;
            Player.Left += EventHandlers.OnPlayerLeave;
            Scp106.Containing += EventHandlers.OnContain106;
            Player.Handcuffing += EventHandlers.OnPlayerHandcuffed;
            Player.InsertingGeneratorTablet += EventHandlers.OnInsertTablet;
            Player.EjectingGeneratorTablet += EventHandlers.OnEjectTablet;
            Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
            Player.Shooting += EventHandlers.OnShoot;
            Server.SendingRemoteAdminCommand += EventHandlers.RunOnRACommandSent;
            Scp096.Enraging += EventHandlers.scpzeroninesixe;
            Scp096.AddingTarget += EventHandlers.scpzeroninesixeadd;
            Timing.RunCoroutine(EventHandlers.CorrodeUpdate());
            Timing.RunCoroutine(EventHandlers.CorrodeHost());
        }
        private void UnregisterEvents()
        {
            this.hInstance.UnpatchAll(null);
            Server.WaitingForPlayers -= EventHandlers.WFP;
            Server.RoundStarted -= EventHandlers.OnRoundStart;
            Player.PickingUpItem -= EventHandlers.OnPickupItem;
            Player.Died -= EventHandlers.OnPlayerDie;
            Player.Hurting -= EventHandlers.OnPlayerHurt;
            Player.EnteringPocketDimension -= EventHandlers.OnPocketDimensionEnter;
            Player.EnteringFemurBreaker -= EventHandlers.OnFemurBreaker;
            Player.Escaping -= EventHandlers.OnCheckEscape;
            Player.ChangingRole -= EventHandlers.OnSetClass;
            Player.Left -= EventHandlers.OnPlayerLeave;
            Scp106.Containing -= EventHandlers.OnContain106;
            Player.Handcuffing -= EventHandlers.OnPlayerHandcuffed;
            Player.InsertingGeneratorTablet -= EventHandlers.OnInsertTablet;
            Player.EjectingGeneratorTablet -= EventHandlers.OnEjectTablet;
            Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
            Player.Shooting -= EventHandlers.OnShoot;
            Server.SendingRemoteAdminCommand -= EventHandlers.RunOnRACommandSent;
            Scp096.Enraging -= EventHandlers.scpzeroninesixe;
            Scp096.AddingTarget -= EventHandlers.scpzeroninesixeadd;
            EventHandlers = null;
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string Distance_bc { get; set; } = "<size=25%><color=#f47fff>*<color=#0089c7>принюхивается</color>*</color>\n<color=#6f6f6f>Вы чувствуете запах гнили, похоже это <color=red>SCP 035</color></color></size>";
        public string Damage_bc { get; set; } = "<size=25%><color=#6f6f6f>Вас атакует <color=red>SCP 035</color></color></size>";
        public string Spawn_bc { get; set; } = "<size=60>Вы-<color=red><b>SCP-035</b></color></size>\nВы заразили тело и получили контроль над ним, используйте его, чтобы помочь другим SCP!";
        public ushort Spawn_bc_time { get; set; } = 10;
        public string Cassie { get; set; } = "ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE . ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B";
        public string Role { get; set; } = "SCP 035";
        public int Hp { get; set; } = 150;
        public string Cassie_dead { get; set; } = "scp 0 3 5 containment minute";
        public int MaskCount { get; set; } = 3;
    }
}
