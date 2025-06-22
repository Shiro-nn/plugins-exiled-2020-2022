using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Loli.Addons
{
    internal static class NetSocket
    {
        internal static void Send(byte[] data)
        {
            if (!Connected()) Connect();
            socket.Send(data);
        }
        internal static void Send(string data)
        {
            if (!Connected()) Connect();
            byte[] ba = Encoding.UTF8.GetBytes(data);
            socket.Send(ba);
        }
        internal static void ThreadUpdateConnect()
        {
            Thread.Sleep(1000);
            for (; ; )
            {
                Thread.Sleep(5000);
                try { if (!Connected()) Connect(); } catch { }
            }
        }
        internal static void Disconnect() => socket.Disconnect(false);
        internal static void Connect()
        {
            try
            {
                if (socket != null && socket.IsBound)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect("127.0.0.1", 111);
            }
            catch { }
        }
        private static Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static bool Connected()
        {
            if (socket == null) return false;
            try { return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected); }
            catch { return false; }
        }
    }
}