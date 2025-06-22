using Loli.Patches;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Addons.Audio;
using Qurre.API.Addons.Audio.Objects;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VoiceChat;

namespace Loli.Modules.Voices
{
    static class VoiceCore
    {
        static internal void PlayAudio(List<string> pathes)
        {
            AudioPlayer audioPlayer = Qurre.API.Audio.CreateNewAudioPlayer("C.A.S.S.I.E.", RoleTypeId.Spectator, Vector3.zero, Vector3.zero);
            audioPlayer.RunCoroutine();

            foreach (string path in pathes)
            {
                var audioTask = audioPlayer.Play(new StreamAudio(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)), VoiceChatChannel.Intercom);
            }

            Timing.RunCoroutine(audioPlayer.CheckPlayingAndDestroy());
            Timing.RunCoroutine(HideFromList(audioPlayer));

            static IEnumerator<float> HideFromList(AudioPlayer audioPlayer)
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return Timing.WaitForSeconds(0.2f);
                    audioPlayer.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Spectator, RoleChangeReason.None);
                }
            }
        }

        static internal AudioPlayer PlayInIntercom(string file, string botName = "Dummy")
        {
            AudioPlayer audioPlayer = AudioExtensions.PlayInIntercom(file, botName);
            return audioPlayer;
        }
    }
}