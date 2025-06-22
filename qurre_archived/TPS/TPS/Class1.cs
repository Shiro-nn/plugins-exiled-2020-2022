using MEC;
using System.Collections.Generic;
namespace TPS
{
	public class Module : Qurre.Plugin
	{
		public override void Enable()
		{
			Timing.RunCoroutine(CountTicks());
			Timing.RunCoroutine(CountTicksUpdate());
			Timing.RunCoroutine(ShowHint());
		}
		public override void Disable()
		{
		}
		internal static short Ticks { get; set; } = 0;
		internal static int TicksMinutes { get; set; } = 0;
		private IEnumerator<float> ShowHint()
		{
			while (true)
			{
				string tps = $"\n<align=left><voffset={-20}em><b><color=red><size=80%><pos=10%>TPS: {TicksMinutes}" +
					$"</pos></size></color></b></voffset></align>";
				Qurre.API.Map.ShowHint(tps, 0.9f);
				yield return Timing.WaitForSeconds(1);
			}
		}
		private IEnumerator<float> CountTicks()
		{
			while (true)
			{
				Ticks++;
				yield return Timing.WaitForOneFrame;
			}
		}
		private IEnumerator<float> CountTicksUpdate()
		{
			while (true)
			{
				TicksMinutes = Ticks / 5;
				Ticks = 0;
				yield return Timing.WaitForSeconds(5);
			}
		}
	}
}