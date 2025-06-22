using Assets._Scripts.Dissonance;
using Qurre.API;
using Qurre.API.Addons.Audio;
using Qurre.API.Controllers;
using Qurre.API.Events;
using System.IO;
using UnityEngine;
namespace BetterAudio
{
    public class Class1 : Qurre.Plugin
    {
        public override void Enable()
        {
            Qurre.Events.Server.SendingRA += (SendingRAEvent ev) =>
            {
                if (ev.Name == "qqtt") Start();
                else if (ev.Name == "q2")
                {
                    Qurre.Log.Info(ev.Player.Radio);
                    Qurre.Log.Info(ev.Player.Radio.mirrorIgnorancePlayer);
                    Qurre.Log.Info(ev.Player.Radio.mirrorIgnorancePlayer._comms);
                    Qurre.Log.Info(ev.Player.Radio.mirrorIgnorancePlayer._comms.gameObject);
                }
            };
        }
        public override void Disable()
        {
        }
        private void Start()
        {
            var bot = Bot.Create(Map.GetRandomSpawnPoint(RoleType.Scp173), Vector2.zero);
            bot.Player.Radio.Start();
            bot.Player.Dissonance.Start();
            bot.Player.Radio.mirrorIgnorancePlayer.Network_playerId = bot.Player.UserId;
            bot.Player.Radio.mirrorIgnorancePlayer.OnStartAuthority();
            bot.Player.Radio.mirrorIgnorancePlayer.OnStartClient();

            bot.Player.Radio.Network_syncAltVoicechatButton = true;
            bot.Player.Dissonance.NetworkspeakingFlags = SpeakingFlags.MimicAs939;

            Qurre.Log.Info(bot.Player.Radio);
            Qurre.Log.Info(bot.Player.Radio.mirrorIgnorancePlayer);
            Qurre.Log.Info(bot.Player.Radio.mirrorIgnorancePlayer._comms);
            Qurre.Log.Info(bot.Player.Radio.mirrorIgnorancePlayer._comms.gameObject);
            var __ = bot.Player.Radio.mirrorIgnorancePlayer._comms.gameObject.AddComponent<AudioMicrophone>().Create(
                new FileStream(Path.Combine(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio"), "OmegaWarhead.raw"), FileMode.Open),
                100, 1920, 48000);
            __.ResetMicrophone(__.Name, true);
            __.StartCapture(__.Name);
            
            /*
            Server.Host.Radio.Network_syncPrimaryVoicechatButton = false;
            Server.Host.Radio.Network_syncAltVoicechatButton = true;
            Server.Host.Dissonance.NetworkspeakingFlags = SpeakingFlags.MimicAs939;
            Server.Host.Position = Map.GetRandomSpawnPoint(RoleType.Scp173);
            */
        }
    }
}