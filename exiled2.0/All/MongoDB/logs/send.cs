using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;
using MEC;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MongoDB.logs
{
    public class send
    {
        public static NetworkStream ss;
        internal static string msg = "";
        internal static string last_msg = "";

        internal static void sendplayers()
        {
            Timing.CallDelayed(5f, () =>
            {
                gosendplayers();
                sendplayersinfo();
            });
        }
        internal static void sendinfo()
        {
            Timing.CallDelayed(5f, () =>
            {
                gosendinfo();
                EventHandlers.UpdateServerStatus();
            });
        }
        internal static void fatalsendmsgtime()
        {
            Timing.CallDelayed(5f, () =>
            {
                fatalsendmsgtime();
                fatalsendmsg();
            });
        }
        internal static void fatalsendmsg()
        {
            try
            {
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", ServerConsole.Port);
                ss = stcp.GetStream();
                string str = $"msg=;={msg}";
                byte[] ba = Encoding.UTF8.GetBytes(str);
                msg = "";
                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
        }
        private static void gosendplayers()
        {
            sendplayers();
        }
        private static void gosendinfo()
        {
            sendinfo();
        }
        public static void sendmsg(string cdata)
        {
            if (msg.Length > 1500)
            {
                fatalsendmsg();
            }
            if (last_msg == cdata)
            {
                last_msg = cdata;
                return;
            }
            last_msg = cdata;
            msg += $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata}";
            msg += "\n";
        }
        public static void sendplayersinfo()
        {
            try
            {
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", ServerConsole.Port);
                ss = stcp.GetStream();
                int players = Player.List.ToList().Count;
                string str = $"players=;={players}";
                byte[] ba = Encoding.UTF8.GetBytes(str);
                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
            try
            {
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", ServerConsole.Port);
                ss = stcp.GetStream();
                int players = Player.List.ToList().Count;
                int maxplay = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
                string str = $"online=;={Plugin.ServerName}=;={players}=;={maxplay}=;={Server.IpAddress}";
                byte[] ba = Encoding.UTF8.GetBytes(str);
                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
        }
        public static void sendchanneltopic(string cdata)
        {
            try
            {
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", ServerConsole.Port);
                ss = stcp.GetStream();
                string str = $"channelstatus=;={cdata}";
                byte[] ba = Encoding.UTF8.GetBytes(str);

                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
        }
        public static void sendra(string cdata)
        {
            try
            {
                if (EventHandlers.roundstart)
                {
                    TcpClient stcp = new TcpClient();
                    stcp.Connect($"localhost", ServerConsole.Port);
                    ss = stcp.GetStream();
                    string str = $"ra=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata}";
                    byte[] ba = Encoding.UTF8.GetBytes(str);

                    ss.Write(ba, 0, ba.Length);
                    stcp.Close();
                }
            }
            catch { }
        }
        public static void sendralog(string cdata)
        {
            try
            {
                string mode = "ral";
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", 111);
                ss = stcp.GetStream();
                string str = $"{mode}=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata.Replace(":keyboard:", "⌨️")}=;={Plugin.ServerName}=;={ServerConsole.Port}=;={DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                byte[] ba = Encoding.UTF8.GetBytes(str);

                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
        }
        public static void senddonateralog(string cdata)
        {
            try
            {
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", 111);
                ss = stcp.GetStream();
                string str = $"radl=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata.Replace(":keyboard:", "⌨️")}=;={Plugin.ServerName}=;={ServerConsole.Port}=;={DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                byte[] ba = Encoding.UTF8.GetBytes(str);

                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
        }
        public static void sendtk(string cdata)
        {
            try
            {
                if (EventHandlers.roundstart)
                {
                    TcpClient stcp = new TcpClient();
                    stcp.Connect($"localhost", ServerConsole.Port);
                    ss = stcp.GetStream();
                    string str = $"tk=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata}";
                    byte[] ba = Encoding.UTF8.GetBytes(str);

                    ss.Write(ba, 0, ba.Length);
                    stcp.Close();
                }
            }
            catch { }
        }
        public static void sendban(string reason, string banned, string banner, string time)
        {
            try
            {
                if (EventHandlers.roundstart)
                {
                    TcpClient stcp = new TcpClient();
                    stcp.Connect($"localhost", ServerConsole.Port);
                    ss = stcp.GetStream();
                    string str = "";
                    if (time == "kick")
                    {
                        str = $"kick=;={banned}=;={banner}=;={reason}";
                    }
                    else
                    {
                        str = $"ban=;={banned}=;={banner}=;={reason}=;={time}";
                    }
                    byte[] ba = Encoding.UTF8.GetBytes(str);

                    ss.Write(ba, 0, ba.Length);
                    stcp.Close();
                }
            }
            catch { }
        }
        public static void sendreply(string cdata, int id)
        {
            try
            {
                TcpClient stcp = new TcpClient();
                stcp.Connect($"localhost", ServerConsole.Port);
                ss = stcp.GetStream();
                string str = $"reply=;={id}=;=[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {cdata}";
                byte[] ba = Encoding.UTF8.GetBytes(str);

                ss.Write(ba, 0, ba.Length);
                stcp.Close();
            }
            catch { }
        }
        public static void HandleQueuedItems()
        {
            var client = new MongoClient(DiscordLogs.statplug.mongodburl);
            var database = client.GetDatabase("login");
            var collection = database.GetCollection<BsonDocument>($"ra-{ServerConsole.Port}");
            var list = collection.Find(new BsonDocument()).ToList();
            foreach (var document in list)
            {
                BsonDocument mi = (BsonDocument)document["commands"][0];
                BsonDocument m = (BsonDocument)mi["id"];
                GameCore.Console.singleton.TypeCommand($"/{m["cmd"]}", new BotSender($"{m["author"]}", (int)m["id"]));
            }
        }

        public static void Handle()
        {
            try
            {
                HandleQueuedItems();
            }
            catch { }
        }
    }
    public class BotSender : CommandSender
    {
        public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
        {
            send.sendreply($"{text}", id);
        }

        public override void Print(string text)
        {
            send.sendreply($"{text}", id);
        }

        public string Name;
        public int id;
        public BotSender(string name, int iD)
        {
            Name = name;
            id = iD;
        }
        public override string SenderId => "SERVER CONSOLE";
        public override string Nickname => Name;
        public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
        public override byte KickPower => byte.MaxValue;
        public override bool FullPermissions => true;
    }
}