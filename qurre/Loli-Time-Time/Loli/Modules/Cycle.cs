using Loli.Addons;
using Loli.Scps;
using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Loli.Modules
{
	static class Cycle
	{
		static bool first = true;

		[EventMethod(RoundEvents.Waiting)]
		static void WaitingForPlayers()
		{
			try
			{
				if (first)
				{
					first = false;
					ServerConsole.ReloadServerName();
					Timing.RunCoroutine(Сycle());
					Timing.RunCoroutine(SlowСycle());
					Timing.RunCoroutine(BroadCasts.Send());
					Thread thread = new(() => Logs.Api.SendOnline());
					thread.Start();
					Timing.RunCoroutine(CountTicks());
					Timing.RunCoroutine(CountTicksUpdate());
				}
			}
			catch { }
		}

		static IEnumerator<float> Сycle()
		{
			for (; ; )
			{
				try { AutoAlpha.AlphaDead(); } catch { }
				try { BetterHints.Manager.Cycle(); } catch { }
				try { Fixes.AutoEndZeroPlayers(); } catch { }
				try { ScpHeal.Heal(); } catch { }
				try { Icom.Update(); } catch { }
				try { Fixes.FixLogicer(); } catch { }

				yield return Timing.WaitForSeconds(1f);
			}
		}
		static IEnumerator<float> SlowСycle()
		{
			for (; ; )
			{
				try { AutoAlpha.ScientistNull(); } catch { }
				try { AutoAlpha.Check(); } catch { }
				try { Caches.PosCheck(); } catch { }
				yield return Timing.WaitForSeconds(5f);
			}
		}

		static int LowTPS = 0;
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
				new Thread(() =>
				{
					new Thread(() =>
					{
						if (Core.TicksMinutes > 0 && 20 >= Core.TicksMinutes)
						{
							if (LowTPS > 12 && !Round.Waiting)
							{
								Round.End();
								Thread.Sleep(10000);
								Server.Restart();
							}
							LowTPS++;
						}
						else LowTPS = 0;
					}).Start();
					if (Core.ServerID != 0 && Player.List.Count() > 1)
						try { Core.Socket.Emit("server.tps", new object[] { Core.ServerID, Core.ServerName, Core.TicksMinutes, Player.List.Count() }); } catch { }
				}).Start();
				yield return Timing.WaitForSeconds(5);
			}
		}
	}
}