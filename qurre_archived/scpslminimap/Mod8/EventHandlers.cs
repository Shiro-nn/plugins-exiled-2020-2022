using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace map
{
    internal class EventHandlers
    {
        public Plugin plugin;
        private ImageGenerator img;
        private float pTime;
        public Dictionary<string, string> conns;
        public Dictionary<string, string> keys;
        internal bool firststart = true;
        public const string pswd = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.characterClassManager.IsHost);


        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
            this.conns = new Dictionary<string, string>();
            this.keys = new Dictionary<string, string>();
        }

        public void OnPlayerJoin(JoinedEventArgs ev)
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
                newkey += pswd[UnityEngine.Random.Range(0, pswd.Length)];
            }
            Log.Info(ev.Player.IPAddress);
            conns.Add(ev.Player.IPAddress, ev.Player.UserId);
            keys.Add(ev.Player.UserId, newkey);
            ev.Player.SendConsoleMessage("Ваш ID: " + ev.Player.UserId, "red");
            ev.Player.SendConsoleMessage("Ваш новый ключ: " + newkey, "red");
        }

        public void OnDisconnect(LeftEventArgs ev)
        {
            string keyrm = string.Empty;
            string connrm = string.Empty;
            foreach (var conn in conns)
            {
                bool hasfound = false;
                //Log.Info(conn.Value);
                foreach (var item in GetHubs())
                {
                    if (item.characterClassManager.UserId == conn.Value)
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
        internal void OnWaitingForPlayers()
        {
            if (firststart)
            {
                wait();
                firststart = false;
            }
        }
        private void wait()
        {
            MEC.Timing.CallDelayed(1f, () =>
            {
                OnFixedUpdate();
            });

        }
        public void OnFixedUpdate()
        {
            wait();
            pTime -= Time.fixedDeltaTime;
            //if (pTime < 0)
            if(true)
            {
                pTime = 1;
                    var mod8 = plugin;

                    if (!mod8.tcp.Connected || !mod8.s.CanWrite)
                    {
                        mod8.tcp.Close();
                        mod8.tcp = new System.Net.Sockets.TcpClient();
                        try
                        {
                            mod8.tcp.Connect("127.0.0.1", 8080);
                            mod8.s = mod8.tcp.GetStream();
                        }
                        catch
                        {

                        }
                    }

                    string str = string.Empty;
                    str += " { ";
                    str += " \"rooms\": [ ";
                    var rooms = Map.Rooms;
                    bool first = true;
                    foreach (var room in rooms)
                    {
                        if (room.Zone != ZoneType.Unspecified)
                        {
                            if (first)
                            {
                                str += " { ";
                                first = false;
                            }
                            else
                                str += ", { ";
                            //var obj = (GameObject)room.GetGameObject();
                            str += " \"posx\": \"" + room.Transform.position.x.ToString() + "\", ";
                            str += " \"posy\": \"" + room.Transform.position.y.ToString() + "\", ";
                            str += " \"posz\": \"" + room.Transform.position.z.ToString() + "\", ";

                            str += " \"rotx\": \"" + room.Transform.rotation.eulerAngles.x.ToString() + "\", ";
                            /*if (room.ZoneType == ZoneType.ENTRANCE && (room.RoomType == RoomType.INTERCOM || room.RoomType == RoomType.CURVE))
                            {
                                str += " \"roty\": \"" + (obj.transform.localRotation.eulerAngles.y - 270).ToString() + "\", ";
                            }
                            else if (room.ZoneType == ZoneType.ENTRANCE && !(room.RoomType == RoomType.INTERCOM || room.RoomType == RoomType.CURVE))
                            {
                                str += " \"roty\": \"" + (obj.transform.localRotation.eulerAngles.y + 90).ToString() + "\", ";
                            }*/
                            if (room.Zone == ZoneType.Unspecified)
                                str += " \"roty\": \"" + (room.Transform.rotation.eulerAngles.y + 90f).ToString() + "\", ";
                            else
                                str += " \"roty\": \"" + (room.Transform.rotation.eulerAngles.y).ToString() + "\", ";
                            str += " \"rotz\": \"" + room.Transform.rotation.eulerAngles.z.ToString() + "\", ";

                            str += " \"id\": \"" + room.Type + "\" ";
                            str += " }";
                        }
                    }
                    str += " ], ";
                    str += " \"players\": [ ";
                    var pls = GetHubs().ToList();
                    for (int i = 0; i < pls.Count; i++)
                    {
                        var play = pls[i];
                        ReferenceHub obj = pls[i];

                        str += " { ";

                        str += " \"posx\": \"" + obj.transform.position.x.ToString() + "\", ";
                        str += " \"posy\": \"" + obj.transform.position.y.ToString() + "\", ";
                        str += " \"posz\": \"" + obj.transform.position.z.ToString() + "\", ";

                        str += " \"rotx\": \"" + obj.transform.rotation.eulerAngles.x.ToString() + "\", ";
                        str += " \"roty\": \"" + obj.transform.rotation.eulerAngles.y.ToString() + "\", ";
                        str += " \"rotz\": \"" + obj.transform.rotation.eulerAngles.z.ToString() + "\", ";

                        str += " \"team\": \"" + play.GetTeam().ToString() + "\", ";
                        str += " \"role\": \"" + play.GetTeam().ToString() + "\", ";
                        //str += " \"ip\": \"" + play.queryProcessor._ipAddress + "\", ";
                        str += " \"steamid\": \"" + play.characterClassManager.UserId + "\", ";
                        if (keys.ContainsKey(play.characterClassManager.UserId))
                        {
                            str += " \"key\": \"" + keys[play.characterClassManager.UserId] + "\", ";
                        }
                        else
                        {
                            str += " \"key\": \"errorfindingkey\", ";
                        }
                        str += " \"name\": \"" + play.nicknameSync.Network_myNickSync + "\" ";
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
                    /*var objs = GameObject.Find("LightRooms").transform;
                    for (int i = 0; i < objs.childCount; i++)
                    {
                        var obj = objs.GetChild(i).gameObject;
                        str += " { ";
                        str += " \"posx\": \"" + obj.transform.position.x.ToString() + "\", ";
                        str += " \"posy\": \"" + obj.transform.position.y.ToString() + "\", ";
                        str += " \"posz\": \"" + obj.transform.position.z.ToString() + "\", ";

                        str += " \"rotx\": \"" + obj.transform.rotation.eulerAngles.x.ToString() + "\", ";
                        str += " \"roty\": \"" + obj.transform.rotation.eulerAngles.y.ToString() + "\", ";
                        str += " \"rotz\": \"" + obj.transform.rotation.eulerAngles.z.ToString() + "\", ";

                        str += " \"id\": \"" + obj.name + "\" ";
                        if (i + 1 >= objs.childCount)
                        {
                            str += " } ";
                        }
                        else
                        {
                            str += " }, ";
                        }
                    }
                    str += " ], ";

                    str += " \"hczrooms\": [ ";
                    var objs2 = GameObject.Find("HeavyRooms").transform;
                    for (int i = 0; i < objs2.childCount; i++)
                    {
                        var obj = objs2.GetChild(i).gameObject;
                        str += " { ";
                        str += " \"posx\": \"" + obj.transform.position.x.ToString() + "\", ";
                        str += " \"posy\": \"" + obj.transform.position.y.ToString() + "\", ";
                        str += " \"posz\": \"" + obj.transform.position.z.ToString() + "\", ";

                        str += " \"rotx\": \"" + obj.transform.rotation.eulerAngles.x.ToString() + "\", ";
                        str += " \"roty\": \"" + obj.transform.rotation.eulerAngles.y.ToString() + "\", ";
                        str += " \"rotz\": \"" + obj.transform.rotation.eulerAngles.z.ToString() + "\", ";

                        str += " \"id\": \"" + obj.name + "\" ";
                        if (i + 1 >= objs2.childCount)
                        {
                            str += " } ";
                        }
                        else
                        {
                            str += " }, ";
                        }
                    }
                    str += " ], ";

                    str += " \"ezrooms\": [ ";
                    var objs3 = GameObject.Find("EntranceRooms").transform;
                    for (int i = 0; i < objs3.childCount; i++)
                    {
                        var obj = objs3.GetChild(i).gameObject;
                        str += " { ";
                        str += " \"posx\": \"" + obj.transform.position.x.ToString() + "\", ";
                        str += " \"posy\": \"" + obj.transform.position.y.ToString() + "\", ";
                        str += " \"posz\": \"" + obj.transform.position.z.ToString() + "\", ";

                        str += " \"rotx\": \"" + obj.transform.rotation.eulerAngles.x.ToString() + "\", ";
                        str += " \"roty\": \"" + obj.transform.rotation.eulerAngles.y.ToString() + "\", ";
                        str += " \"rotz\": \"" + obj.transform.rotation.eulerAngles.z.ToString() + "\", ";

                        str += " \"id\": \"" + obj.name + "\" ";
                        if (i + 1 >= objs3.childCount)
                        {
                            str += " } ";
                        }
                        else
                        {
                            str += " }, ";
                        }
                    }
                    str += " ], ";

                    str += " \"players\": [ ";
                    var pls = plugin.Server.GetPlayers();
                    for (int i = 0; i < pls.Count; i++)
                    {
                        var play = pls[i];
                        GameObject obj = (GameObject)pls[i].GetGameObject();
                        
                        str += " { ";

                        str += " \"posx\": \"" + obj.transform.position.x.ToString() + "\", ";
                        str += " \"posy\": \"" + obj.transform.position.y.ToString() + "\", ";
                        str += " \"posz\": \"" + obj.transform.position.z.ToString() + "\", ";

                        str += " \"rotx\": \"" + obj.transform.rotation.eulerAngles.x.ToString() + "\", ";
                        str += " \"roty\": \"" + obj.transform.rotation.eulerAngles.y.ToString() + "\", ";
                        str += " \"rotz\": \"" + obj.transform.rotation.eulerAngles.z.ToString() + "\", ";

                        str += " \"team\": \"" + play.TeamRole.Team.ToString() + "\", ";
                        str += " \"role\": \"" + play.TeamRole.Role.ToString() + "\", ";
                        str += " \"ip\": \"" + play.IpAddress + "\", ";
                        str += " \"steamid\": \"" + play.SteamId + "\", ";
                        if (keys.ContainsKey(play.SteamId))
                        {
                            str += " \"key\": \"" + keys[play.SteamId] + "\", ";
                        }
                        else
                        {
                            str += " \"key\": \"errorfindingkey\", ";
                        }
                        str += " \"name\": \"" + play.Name + "\" ";
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
                    str += ";";*/
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);

                    mod8.s.Write(ba, 0, ba.Length);
            }
        }
    }
    public static class Extensions
    {
        public static RoleType GetRole(this ReferenceHub player) => player.characterClassManager.CurClass;
        public static Team GetTeam(this ReferenceHub player) => GetTeam(GetRole(player));
        public static Team GetTeam(this RoleType roleType)
        {
            switch (roleType)
            {
                case RoleType.ChaosInsurgency:
                    return Team.CHI;
                case RoleType.Scientist:
                    return Team.RSC;
                case RoleType.ClassD:
                    return Team.CDP;
                case RoleType.Scp049:
                case RoleType.Scp93953:
                case RoleType.Scp93989:
                case RoleType.Scp0492:
                case RoleType.Scp079:
                case RoleType.Scp096:
                case RoleType.Scp106:
                case RoleType.Scp173:
                    return Team.SCP;
                case RoleType.Spectator:
                    return Team.RIP;
                case RoleType.FacilityGuard:
                case RoleType.NtfCadet:
                case RoleType.NtfLieutenant:
                case RoleType.NtfCommander:
                case RoleType.NtfScientist:
                    return Team.MTF;
                case RoleType.Tutorial:
                    return Team.TUT;
                default:
                    return Team.RIP;
            }
        }
    }
}
