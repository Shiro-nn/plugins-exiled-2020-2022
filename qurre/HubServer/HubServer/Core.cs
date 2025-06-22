using HarmonyLib;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Addons;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using RoundRestarting;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace HubServer
{
    [PluginInit("HubServer", "fydne", "6.6.6")]
    static public class Core
    {
        static JsonConfig Config;
        static uint ServerID = 0;

        [PluginEnable]
        static internal void Enable()
        {
            new Harmony("fydne.hub").PatchAll();

            Config = new("HubServer");
            ServerID = Config.SafeGetValue("ServerID", 0U);
            JsonConfig.UpdateFile();

            RateLimit.Enable();
        }

        [EventMethod(RoundEvents.Waiting)]
        static void Waiting()
        {
            Round.LobbyLock = true;
            try { GameObject.Find("StartRound").transform.localScale = Vector3.zero; } catch { }
        }

        [EventMethod(PlayerEvents.Join)]
        static void Join(JoinEvent ev)
        {
            ev.Player.Client.ShowHint("<color=#eb34e5>Идет проверка подключения...</color>\n" +
                "<color=#a834eb>После проверки, Вы будете подключены к серверу</color>", 666);
            new Thread(() =>
            {
                /*
                {
                    var url2 = $"http://10.66.55.1:4536/ips/add?ip={ev.Player.UserInfomation.Ip}&userid={ev.Player.UserInfomation.UserId}";
                    var request2 = WebRequest.Create(url2);
                    request2.Method = "POST";
                    using var webResponse2 = request2.GetResponse();
                }
                */
                var url = $"http://127.0.0.1:5231/verify?ip={ev.Player.UserInfomation.Ip}&type={ServerID}";
                var request = WebRequest.Create(url);
                request.Method = "POST";
                using var webResponse = request.GetResponse();
                using var webStream = webResponse.GetResponseStream();
                using var reader = new StreamReader(webStream);
                var data = reader.ReadToEnd();
                var reply = JsonConvert.DeserializeObject<ReqReply>(data);

                try
                {
                    reader.Close();
                    webStream.Close();
                    webResponse.Close();
                }
                catch { }

                if (ev.Player.Disconnected)
                    return;

                if (reply.Error)
                {
                    ev.Player.Client.Broadcast("<color=red>Произошла ошибка при проверке подключения..</color>\n" +
                        "Попробуйте снова или напишите в тех. поддержку", 60, true);
                    Log.Error(reply.Message);
                    return;
                }

                ev.Player.Connection.Send(new RoundRestartMessage(RoundRestartType.RedirectRestart, 1f, reply.Port, true, false));
            }).Start();
        }
    }
}