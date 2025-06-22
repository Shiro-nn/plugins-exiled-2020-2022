using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using MEC;
using System.Collections.Generic;
namespace auto_alpha
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.First;
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
            Exiled.Events.Handlers.Server.RoundStarted += RoundStart;
            Exiled.Events.Handlers.Warhead.Stopping += WarheadCancel;
            Exiled.Events.Handlers.Warhead.Starting += Starting;
            Exiled.Events.Handlers.Warhead.Detonated += Detonated;
            Exiled.Events.Handlers.Server.EndingRound += CheckRoundEnd;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= RoundStart;
            Exiled.Events.Handlers.Warhead.Stopping -= WarheadCancel;
            Exiled.Events.Handlers.Warhead.Starting -= Starting;
            Exiled.Events.Handlers.Warhead.Detonated -= Detonated;
            Exiled.Events.Handlers.Server.EndingRound -= CheckRoundEnd;
        }
        #endregion
        #region events
        public bool autowarheadstart = false;
        public bool detonated = false;
        public bool progress = false;
        public void RoundStart()
        {
            autowarheadstart = false;
            detonated = false;
            progress = false;
        }
        public void WarheadCancel(Exiled.Events.EventArgs.StoppingEventArgs ev)
        {
            try
            {
                if (autowarheadstart)
                    ev.IsAllowed = false;
            }
            catch
            { ev.IsAllowed = false; }
            if (ev.IsAllowed) progress = false;
        }
        public void Starting(Exiled.Events.EventArgs.StartingEventArgs ev)
        {
            if (ev.IsAllowed) progress = true;
        }
        public void Detonated() => detonated = true;
        public void CheckRoundEnd(Exiled.Events.EventArgs.EndingRoundEventArgs ev)
        {
            if (Round.ElapsedTime.Seconds >= config.Seconds && !autowarheadstart && !detonated)
            {
                Map.Broadcast(config.bctime, config.bc);
                autowarheadstart = true;
                if (!Warhead.IsInProgress) Warhead.Start();
            }
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Seconds { get; set; } = 900;
        public string bc { get; set; } = "По приказу совета о5 запущена альфа";
        public ushort bctime { get; set; } = 10;
    }
}