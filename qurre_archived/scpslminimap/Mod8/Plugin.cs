using System.Net;
using System.Net.Sockets;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace map
{
    public class Plugin : Plugin<Config>
    {
        public TcpClient tcp;
        public NetworkStream s;
        public RoomManager rm;
        private EventHandlers EventHandlers;

        public override void OnDisabled()
        {
            base.OnDisabled();
            tcp.Close();
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnDisconnect;
            EventHandlers = null;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            Log.Info("Map enabled.");
            Log.Info("Connecting to node.js server");
            tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 8080);
            s = tcp.GetStream();
            rm = new RoomManager();
            RegisterEvents();
        }
        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.Left += EventHandlers.OnDisconnect;
        }
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
