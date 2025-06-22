using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;

namespace ScpMap
{
    public class Events
    {
        private readonly Plugin plugin;
        public Dictionary<string, string> conns;
        public Dictionary<string, string> keys;
        public const string pswd = "abcdefghijklmnopqrstuvwxyz0123456789";
        private System.Random random = new System.Random();
        internal Events(Plugin plugin)
        {
            this.plugin = plugin;
            this.conns = new Dictionary<string, string>();
            this.keys = new Dictionary<string, string>();
        }
        public void Join(JoinEvent ev)
        {
            if (keys.ContainsKey(ev.Player.UserId))
            {
                ev.Player.SendConsoleMessage("Ваш ID: " + ev.Player.UserId, "red");
                ev.Player.SendConsoleMessage("Ваш ключ: " + keys[ev.Player.UserId], "red");
                return;
            }
            string newkey = "";
            for (int i = 0; i < 5; i++)
            {
                newkey += pswd[random.Next(0, pswd.Length)];
            }
            conns.Add(ev.Player.IP, ev.Player.UserId);
            keys.Add(ev.Player.UserId, newkey);
            ev.Player.SendConsoleMessage($"\nВаш ID: {ev.Player.UserId}\nВаш новый ключ: {newkey}", "red");
        }
        public void Leave(LeaveEvent ev)
        {
            string keyrm = string.Empty;
            string connrm = string.Empty;
            foreach (var conn in conns)
            {
                bool hasfound = false;
                foreach (var item in Player.List)
                {
                    if (item.UserId == conn.Value)
                    {
                        hasfound = true;
                        break;
                    }
                }
                if (!hasfound)
                {
                    keyrm = conn.Value;
                    connrm = conn.Key;
                }
            }
            if (keyrm != string.Empty && connrm != string.Empty)
            {
                conns.Remove(connrm);
                keys.Remove(keyrm);
            }
        }
        public void Ra(SendingRAEvent ev)
        {
            if(ev.Name == "room")
            {
                //Player.Roo
            }
        }
        public IEnumerator<float> SendData()
        {
            for (; ; )
            {
                if (!plugin.tcp.Connected || !plugin.s.CanWrite)
                {
                    plugin.tcp.Close();
                    plugin.tcp = new System.Net.Sockets.TcpClient();
                    try
                    {
                        plugin.tcp.Connect("127.0.0.1", 8080);
                        plugin.s = plugin.tcp.GetStream();
                    }
                    catch { }
                }
                string str = string.Empty;
                str += " { ";
                str += " \"rooms\": [ ";
                var rooms = Map.Rooms;
                bool first = true;
                foreach (var room in rooms)
                {
                    if (first)
                    {
                        str += " { ";
                        first = false;
                    }
                    else str += ", { ";
                    str += " \"posx\": \"" + room.Transform.position.x.ToString() + "\", ";
                    str += " \"posy\": \"" + room.Transform.position.y.ToString() + "\", ";
                    str += " \"posz\": \"" + room.Transform.position.z.ToString() + "\", ";

                    str += " \"rotx\": \"" + room.Transform.rotation.eulerAngles.x.ToString() + "\", ";
                    if (room.Zone == ZoneType.Unspecified)
                        str += " \"roty\": \"" + (room.Transform.rotation.eulerAngles.y + 90f).ToString() + "\", ";
                    else
                        str += " \"roty\": \"" + (room.Transform.rotation.eulerAngles.y).ToString() + "\", ";
                    str += " \"rotz\": \"" + room.Transform.rotation.eulerAngles.z.ToString() + "\", ";

                    str += " \"id\": \"" + room.Name.Split(' ')[0] + "\" ";
                    str += " }";
                }
                str += " ], ";
                str += " \"players\": [ ";
                var pls = Player.List.ToList();
                for (int i = 0; i < pls.Count; i++)
                {
                    var play = pls[i];
                    Player obj = pls[i];
                    str += " { ";
                    str += " \"posx\": \"" + obj.Position.x.ToString() + "\", ";
                    str += " \"posy\": \"" + obj.Position.y.ToString() + "\", ";
                    str += " \"posz\": \"" + obj.Position.z.ToString() + "\", ";

                    str += " \"rotx\": \"" + obj.ReferenceHub.transform.rotation.eulerAngles.x.ToString() + "\", ";
                    str += " \"roty\": \"" + obj.ReferenceHub.transform.rotation.eulerAngles.y.ToString() + "\", ";
                    str += " \"rotz\": \"" + obj.ReferenceHub.transform.rotation.eulerAngles.z.ToString() + "\", ";

                    str += " \"team\": \"" + play.Team.ToString() + "\", ";
                    str += " \"role\": \"" + play.Role.ToString() + "\", ";
                    //str += " \"ip\": \"" + play.queryProcessor._ipAddress + "\", ";
                    str += " \"steamid\": \"" + play.UserId + "\", ";
                    if (keys.ContainsKey(play.UserId))
                    {
                        str += " \"key\": \"" + keys[play.UserId] + "\", ";
                    }
                    else
                    {
                        str += " \"key\": \"errorfindingkey\", ";
                    }
                    str += " \"name\": \"" + play.Nickname + "\" ";
                    if (i + 1 >= pls.Count)
                    {
                        str += " } ";
                    }
                    else
                    {
                        str += " }, ";
                    }
                }
                str += " ] ";
                str += " } ";
                str += ";";
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                plugin.s.Write(ba, 0, ba.Length);
                yield return MEC.Timing.WaitForSeconds(1f);
            }
        }
    }
}