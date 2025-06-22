using HarmonyLib;
using Hints;
using MEC;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace BetterHints
{
	internal class Loader : Qurre.Plugin
	{
		internal static string HintToken = "sufh*&Y(s7ghf7wY67f8WFGG*F&^WS&8";
		internal static short Ticks { get; set; } = 0;
		internal static int TicksMinutes { get; set; } = 0;
		public override string Name => "Better Hints";
		public override string Developer => "fydne";
		public override Version Version => new(0, 0, 3);
		private const string CoroutinesName = "CoroutinesByBetterHintsYes";
		private static Harmony hInstance;
		private static MethodInfo _patch;
		internal static bool ShowTps = Config.GetBool("BetterHints_ShowTPS", true);
		public override void Enable()
		{
			Ticks = 0;
			TicksMinutes = 0;
			hInstance = new Harmony("fydne.BetterHints");
			Timing.RunCoroutine(Cor(), CoroutinesName);
			Timing.RunCoroutine(CountTicks(), CoroutinesName);
			Timing.RunCoroutine(CountTicksUpdate(), CoroutinesName);
			{
				var original = AccessTools.Method(typeof(HintDisplay), nameof(HintDisplay.Show));
				var method = AccessTools.Method(typeof(Manager), nameof(Manager.PatchYes));
				_patch = hInstance.Patch(original, new HarmonyMethod(method));
			}
		}
		public override void Disable()
		{
			Timing.KillCoroutines(CoroutinesName);
			hInstance.UnpatchAll();
		}
		private IEnumerator<float> Cor()
		{
			for (; ; )
			{
				Manager.Cycle();
				yield return Timing.WaitForSeconds(1);
			}
		}
		private IEnumerator<float> CountTicks()
		{
			for (; ; )
			{
				Ticks++;
				yield return Timing.WaitForOneFrame;
			}
		}
		private IEnumerator<float> CountTicksUpdate()
		{
			for (; ; )
			{
				TicksMinutes = Ticks / 5;
				Ticks = 0;
				yield return Timing.WaitForSeconds(5);
			}
		}
	}
}