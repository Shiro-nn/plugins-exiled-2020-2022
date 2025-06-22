using HarmonyLib;
using MongoDB.Driver;
namespace PlayerXP
{
    public class Plugin : Qurre.Plugin
    {
        public EventHandlers EventHandlers;
        internal static MongoClient Client;
        #region override
        public override int Priority { get; } = -10000;
        public override string Developer { get; } = "fydne";
        public override void Enable()
        {
            Configs.Reload();
            Client = new MongoClient(Configs.MongoURL);
            RegisterEvents();
        }
        public override void Disable() => UnregisterEvents();
        #endregion
        private Harmony hInstance;


        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers();
            Qurre.Events.Round.Waiting += EventHandlers.OnWaitingForPlayers;
            Qurre.Events.Round.Start += EventHandlers.OnRoundStart;
            Qurre.Events.Round.End += EventHandlers.OnRoundEnd;

            Qurre.Events.Player.Join += EventHandlers.OnPlayerJoin;
            Qurre.Events.Player.RoleChange += EventHandlers.OnPlayerSpawn;
            Qurre.Events.Player.Escape += EventHandlers.OnCheckEscape;
            Qurre.Events.Player.Dies += EventHandlers.OnPlayerDeath;
            Qurre.Events.Scp106.PocketFailEscape += EventHandlers.OnPocketDimensionDie;
            Qurre.Events.Server.SendingConsole += EventHandlers.Console;
            hInstance = new Harmony("fydne.playerxp");
            hInstance.PatchAll();
        }
        private void UnregisterEvents()
        {
            Qurre.Events.Round.Waiting -= EventHandlers.OnWaitingForPlayers;
            Qurre.Events.Round.Start -= EventHandlers.OnRoundStart;
            Qurre.Events.Round.End -= EventHandlers.OnRoundEnd;

            Qurre.Events.Player.Join -= EventHandlers.OnPlayerJoin;
            Qurre.Events.Player.RoleChange -= EventHandlers.OnPlayerSpawn;
            Qurre.Events.Player.Escape -= EventHandlers.OnCheckEscape;
            Qurre.Events.Player.Dies -= EventHandlers.OnPlayerDeath;
            Qurre.Events.Scp106.PocketFailEscape -= EventHandlers.OnPocketDimensionDie;
            Qurre.Events.Server.SendingConsole -= EventHandlers.Console;
            EventHandlers = null;
            hInstance.UnpatchAll(null);
        }
    }
}