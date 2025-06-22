using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Qurre.API;
using Qurre.API.Addons.Audio;
namespace Loli.Addons
{
	internal static class WaitingMusic
	{
		internal static void Waiting()
		{
			//if (!File.Exists(AudioPath)) return;
			//Task = Audio.Play(new FileStream(AudioPath, FileMode.Open, FileAccess.Read, FileShare.Read), 7, true);
		}
		internal static void RoundStart()
		{
			//if (Audio.Microphone.Tasks.FirstOrDefault() == Task) Audio.Microphone.StopCapture();
		}
		//internal static AudioTask Task;
		static WaitingMusic() => DownloadAudio("https://cdn.scpsl.store/qurre/audio/Fragmented.raw");//PhonkCore.mp3
		private static string AudioPath => Path.Combine(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio"), "Fragmented.raw");
		private static void DownloadAudio(string url)
		{
			var dir = Path.Combine(Qurre.PluginManager.PluginsDirectory, "Audio");
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			if (File.Exists(AudioPath)) return;
			new Thread(() =>
			{
				WebRequest request = WebRequest.Create(url);
				WebResponse response = request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				Stream fileStream = File.OpenWrite(AudioPath);
				byte[] buffer = new byte[4096];
				int bytesRead = responseStream.Read(buffer, 0, 4096);
				while (bytesRead > 0)
				{
					fileStream.Write(buffer, 0, bytesRead);
					bytesRead = responseStream.Read(buffer, 0, 4096);
				}
			}).Start();
		}
	}
}