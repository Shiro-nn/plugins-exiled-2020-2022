using HarmonyLib;
using System;
using VoiceChatManager.Core.Utilities;
namespace Audio
{
    public class Plugin : Qurre.Plugin
    {
        internal Events Events;
        private static Harmony harmonyInstance;
        public override void Enable()
        {
            CachedProperties.MaximumVoiceChatDesync = TimeSpan.FromSeconds(60);
            Events = new Events();
            Qurre.Events.Round.Restart += Events.Restart;
            Qurre.Events.Round.Waiting += Events.Waiting;
            Qurre.Events.Server.SendingRA += Events.Ra;
            harmonyInstance = new Harmony($"fydne.audio");
            harmonyInstance.PatchAll();
        }
        public override void Disable()
        {
            Qurre.Events.Round.Restart -= Events.Restart;
            Qurre.Events.Round.Waiting -= Events.Waiting;
            Qurre.Events.Server.SendingRA -= Events.Ra;
            CachedProperties.MaximumVoiceChatDesync = TimeSpan.FromSeconds(60);
        }
    }
}