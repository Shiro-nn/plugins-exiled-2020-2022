using MEC;
using Qurre.API;
using System.Collections.Generic;
namespace RainbowWaitingMessages
{
    public class Plugin : Qurre.Plugin
    {
        public override string Name { get; } = "RainbowWaitingScreen";
        public override string Developer { get; } = "JesusQC";
        public override System.Version NeededQurreVersion { get; } = new System.Version(1, 5, 0);
        int i = 0;
        private readonly string[] Colors = { "#f54242", "#f56042", "#f57e42", "#f59c42", "#f5b942", "#f5d742", "#f5f542", "#d7f542", "#b9f542", "#9cf542", "#7ef542", "#60f542",
            "#42f542", "#42f560", "#42f57b", "#42f599", "#42f5b6", "#42f5d4", "#42f5f2", "#42ddf5", "#42bcf5", "#429ef5", "#4281f5", "#4263f5", "#4245f5", "#5a42f5", "#7842f5",
            "#9642f5", "#b342f5", "#d142f5", "#ef42f5", "#f542dd", "#f542c2", "#f542aa", "#f5428d", "#f5426f", "#f54251" };

        public override void Enable()
        {
            Qurre.Events.Round.Restart += OnRestarting;
            Qurre.Events.Round.Waiting += OnWaiting;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Restart -= OnRestarting;
            Qurre.Events.Round.Waiting -= OnWaiting;
        }
        public void OnWaiting() => Timing.RunCoroutine(RainbowHint(), "RainbowWaitingText");
        public void OnRestarting() => Timing.KillCoroutines("RainbowWaitingText");
        private string WaitingTextText => Config.GetString("Rainbow_Waiting_Text", "\n\n\n\n<b>discord.gg/yourdiscord</b>\n<color=%rainbow%><b>Rainbow Server Name</b></color>",
            "The hint in Waiting For Players screen (use <color=%rainbow%> for a rainbow color (remember to close it with </color>))");
        private IEnumerator<float> RainbowHint()
        {
            while (!Round.Started)
            {
                Map.ShowHint(WaitingTextText.Replace("%rainbow%", Colors[i++ % Colors.Length]), 2);
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}