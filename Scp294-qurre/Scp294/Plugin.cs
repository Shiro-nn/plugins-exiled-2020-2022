using HarmonyLib;
using Scp294.API;
using Scp294.Events;
using System;

namespace Scp294
{
    public sealed class Plugin : Qurre.Plugin
    {
        public override string Name => "SCP-294";

        public override string Developer => "AlexanderK";

        public override int Priority => -10000;

        public override Version Version { get; } = new Version(1, 0, 0);

        public override Version NeededQurreVersion { get; } = new Version(1, 14, 4);

        private Config _config;
        private Harmony _harmony;
        private PlayerHandler _playerHandler;
        private RoundHandler _roundHandler;

        public override void Enable()
        {
            _config = new Config();
            _harmony = new Harmony("com.alexanderk.scp294");
            _playerHandler = new PlayerHandler();
            _roundHandler = new RoundHandler();

            CustomConfigs.Add(_config);

            if (!_config.IsEnabled)
                return;

            _harmony.PatchAll();
            DrinksManager.Init();
            _playerHandler.RegisterEvents();
            _roundHandler.RegisterEvents();
        }

        public override void Disable()
        {
            CustomConfigs.Remove(_config);

            if (!_config.IsEnabled)
                return;

            _harmony.UnpatchAll();
            DrinksManager.Reset();
            _playerHandler.UnregisterEvents();
            _roundHandler.UnregisterEvents();
        }
    }
}
