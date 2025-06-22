using Loli.Addons;
using MEC;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;
using Qurre.API.Attributes;
using Qurre.Events;

namespace Loli.Modules
{
	static class Cycle
	{
        [EventMethod(RoundEvents.Waiting)]
        static void NullCall() { }
        
		static Cycle()
		{
			ServerConsole.ReloadServerName();
			Timing.RunCoroutine(Сycle());
			Timing.RunCoroutine(SlowСycle());
			Timing.RunCoroutine(CountTicks());
			Timing.RunCoroutine(CountTicksUpdate());
		}

		static IEnumerator<float> Сycle()
		{
			for (; ; )
			{
				try { Fixes.FixRoundFreeze(); } catch { }
				try { Fixes.CheckBcAlive(); } catch { }
				try { Fixes.FixFreezeEnd(); } catch { }
				try { VoteRestart.CycleCheck(); } catch { }
				try { Fixes.AntiAllFreeze(); } catch { }
				try { VoteRestart.CycleCheck(); } catch { }

				yield return Timing.WaitForSeconds(1f);
			}
		}
		static IEnumerator<float> SlowСycle()
		{
			for (; ; )
			{
				try { AutoAlpha.Check(); } catch { }
				try { Caches.PosCheck(); } catch { }

				yield return Timing.WaitForSeconds(5f);
			}
		}

		static IEnumerator<float> CountTicks()
		{
			for (; ; )
			{
				Core.Ticks++;
				yield return Timing.WaitForOneFrame;
			}
		}
		static IEnumerator<float> CountTicksUpdate()
		{
			for (; ; )
			{
				Core.TicksMinutes = Core.Ticks / 5;
				Core.Ticks = 0;

				if (Core.ServerID != 0 && Player.List.Any())
					try
					{
						Core.Socket.Emit("server.tps", new object[] {
							Core.ServerID,
							Core.ServerName,
							Core.TicksMinutes, Player.List.Count()
						});
					}
					catch { }

				yield return Timing.WaitForSeconds(5);
			}
		}
	}
}