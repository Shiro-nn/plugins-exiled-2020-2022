using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roundtime
{
    public class Plugin : Plugin<Config>
    {
        public Events Events;
        public override PluginPriority Priority { get; } = PluginPriority.Default;

        public override void OnEnabled()
        {
            base.OnEnabled();

            /*RegisterEvents();*/
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            /*UnregisterEvents();*/
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
        private void RegisterEvents()
        {
            Events = new Events(this);
            Exiled.Events.Handlers.Server.RoundStarted += Events.wait;
            Exiled.Events.Handlers.Player.Hurting += Events.hurt;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= Events.wait;
            Exiled.Events.Handlers.Player.Hurting -= Events.hurt;
            Events = null;
        }
    }
    public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
	}
}
