using Qurre;
using System;
using System.Net.Sockets;
namespace ScpMap
{
    public class Plugin : Qurre.Plugin
    {
        public override Version Version => new Version(1, 0, 0);
        public override Version NeededQurreVersion => new Version(1, 1, 1);
        public override string Developer => "fydne";
        public override string Name => "Scp Web Map";
        public TcpClient tcp;
        public NetworkStream s;
        public RoomManager rm;
        public Events Events;
        public override void Enable()
        {
            Log.Info("Connecting to node.js server");
            tcp = new TcpClient();
            tcp.Connect("127.0.0.1", 8080);
            s = tcp.GetStream();
            rm = new RoomManager();
            RegisterEvents();
        }
        public override void Disable()
        {
            Qurre.Events.Player.Join -= Events.Join;
            Qurre.Events.Player.Leave -= Events.Leave;
            Events = null;
        }
        public override void Reload()
        {
        }
        private void RegisterEvents()
        {
            Events = new Events(this);
            Qurre.Events.Player.Join += Events.Join;
            Qurre.Events.Player.Leave += Events.Leave;
            MEC.Timing.RunCoroutine(Events.SendData());
        }
    }
}