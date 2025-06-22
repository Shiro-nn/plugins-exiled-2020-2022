using Dissonance;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using VoiceChatManager.Core.Audio.Capture;
using VoiceChatManager.Core.Audio.Playback;
using VoiceChatManager.Core.Extensions;
namespace Audio
{
    internal class Events
    {
        public const string ConvertedFileExtension = ".f32le";
        internal void Restart()
        {
            foreach (var streamedMicrophone in StreamedMicrophone.List)
                streamedMicrophone.Dispose();
            StreamedMicrophone.List.Clear();
        }
        internal void Waiting()
        {
            Server.Host.GameObject.AddComponent<VoiceReceiptTrigger>().RoomName = "SCP";
        }
        internal void Ra(SendingRAEvent ev)
        {
            if (ev.Name == "play")
            {
                var _ = Execute(new ArraySegment<string>(new string[] {
                    "C:\\Users\\fydne\\Desktop\\TheHunt.f32le p", "100", "proximity", "180", "994", "-63" }), out string response);
                Qurre.Log.Info(response + " -- " + _);
            }
            if (ev.Name == "play2")
            {
                var _ = Execute(new ArraySegment<string>(new string[] {
                    "C:\\Users\\fydne\\Desktop\\TheHunt.f32le p", "100", "Intercom" }), out string response);
                Qurre.Log.Info(response + " -- " + _);
            }
            if (ev.Name == "play3")
            {
                var _ = Execute(new ArraySegment<string>(new string[] {
                    "C:\\Users\\fydne\\Desktop\\TheHunt.f32le p", "100", "ghost" }), out string response);
                Qurre.Log.Info(response + " -- " + _);
            }
            if (ev.Name == "play4")
            {
                var _ = Execute(new ArraySegment<string>(new string[] {
                    "C:\\Users\\fydne\\Desktop\\TheHunt.f32le p", "100", "scp" }), out string response);
                Qurre.Log.Info(response + " -- " + _);
            }
        }
        public bool Execute(ArraySegment<string> arguments, out string response)
        {
            if (arguments.Count < 2 || arguments.Count > 6 || arguments.Count == 5)
            {
                response = "\nvoicechatmanager play [File alias/File path] [Volume (0-100)]" +
                    "\nvoicechatmanager play [File alias/File path] [Volume (0-100)] [Channel name (SCP, Intercom, Proximity, Ghost)]" +
                    "\nvoicechatmanager play [File alias/File path] [Volume (0-100)] proximity [Player ID/Player Name/Player]" +
                    "\nvoicechatmanager play [File alias/File path] [Volume (0-100)] proximity [X] [Y] [Z]";
                return false;
            }
            if (!float.TryParse(arguments.At(1), out var volume))
            {
                response = string.Format("{0} is not a valid volume, range varies from 0 to 100!", arguments.At(1));
                return false;
            }

            var channelName = arguments.Count == 2 ? "Intercom" : arguments.At(2).GetChannelName();
            
            var path = arguments.At(0);

            var convertedFilePath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ConvertedFileExtension;

            if (File.Exists(path) && !path.EndsWith(ConvertedFileExtension) && !File.Exists(convertedFilePath))
            {
                response = string.Format("Converting \"{0}\"...", path);

                path.ConvertFileAsync().ContinueWith(
                    task =>
                    {
                        if (task.IsCompleted)
                        {
                            var newArguments = new List<string>(arguments)
                            {
                                [0] = convertedFilePath,
                            };
                            Execute(new ArraySegment<string>(newArguments.ToArray()), out var otherResponse);
                        }
                        else
                        {
                            Qurre.Log.Error(string.Format("Failed to convert \"{0}\": {1}", path, task.Exception));
                        }
                    }, TaskContinuationOptions.ExecuteSynchronously);

                return true;
            }

            if (int.TryParse(path, out var id) && id.TryPlay(volume, channelName, out var streamedMicrophone))
            {
                response = string.Format("Playing \"{0}\" with {1} volume on \"{2}\" channel, duration: {3}",
                    id, volume, streamedMicrophone.ChannelName, streamedMicrophone.Duration.ToString("hh\\:mm\\:ss\\.ff"));

                return true;
            }

            if (!path.EndsWith(ConvertedFileExtension))
                path = convertedFilePath;

            if (arguments.Count == 2 || arguments.Count == 3)
            {
                if (path.TryPlay(volume, channelName, out streamedMicrophone))
                {
                    response = string.Format("Playing \"{0}\" with {1} volume on \"{2}\" channel, duration: {3}",
                        path, volume, streamedMicrophone.ChannelName, streamedMicrophone.Duration.ToString("hh\\:mm\\:ss\\.ff"));

                    return true;
                }
            }
            else if (arguments.Count == 4)
            {
                if (!(Player.Get(arguments.At(3)) is Player player))
                {
                    response = string.Format("Player \"{0}\" not found!", arguments.At(3));
                    return false;
                }
                else if (path.TryPlay(Talker.GetOrCreate(player.GameObject), volume, channelName, out streamedMicrophone))
                {
                    response = string.Format("Playing \"{0}\" with {1} volume, in the proximity of \"{2}\", duration: {3}",
                        path, volume, player.Nickname, streamedMicrophone.Duration.ToString("hh\\:mm\\:ss\\.ff"));
                    return true;
                }
            }
            else
            {
                if (!float.TryParse(arguments.At(3), out var x))
                {
                    response = string.Format("\"{0}\" is not a valid {1} coordinate!", arguments.At(3), "x");
                    return false;
                }
                else if (!float.TryParse(arguments.At(4), out var y))
                {
                    response = string.Format("\"{0}\" is not a valid {1} coordinate!", arguments.At(4), "y");
                    return false;
                }
                else if (!float.TryParse(arguments.At(5), out var z))
                {
                    response = string.Format("\"{0}\" is not a valid {1} coordinate!", arguments.At(5), "z");
                    return false;
                }
                else if (path.TryPlay(new Vector3(x, y, z), volume, channelName, out streamedMicrophone))
                {
                    response = string.Format("Playing \"{0}\" with {1} volume, in the proximity of ({2}, {3}, {4}) duration: {5}",
                        path, volume, x, y, z, streamedMicrophone.Duration.ToString("hh\\:mm\\:ss\\.ff"));
                    return true;
                }
            }

            response = string.Format("Audio \"{0}\" not found or it's already playing!", path);
            return false;
        }
    }
}