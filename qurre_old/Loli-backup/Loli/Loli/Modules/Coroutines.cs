using MEC;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		internal void WaitingForPlayers()
		{
			try
			{
				if (first)
				{
					first = false;
					ServerConsole.ReloadServerName();
					Timing.RunCoroutine(Сycle());
					Timing.RunCoroutine(FastСycle());
					Timing.RunCoroutine(SlowСycle());
					Timing.RunCoroutine(plugin.BroadCasts.Send());
					Thread thread = new(() => Logs.Api.SendOnline());
					thread.Start();
					Timing.RunCoroutine(CountTicks());
					Timing.RunCoroutine(CountTicksUpdate());
				}
			}
			catch { }
			Server.LaterJoinEnabled = false;
		}
		private IEnumerator<float> Сycle()
		{
			for (; ; )
			{
				try { AlphaDead(); } catch { }
				try { BetterHints.Manager.Cycle(); } catch { }
				try { AutoEndZeroPlayers(); } catch { }
				try { plugin.ScpHeal.Heal(); } catch { }
				try { plugin.Icom.Update(); } catch { }
				try { plugin.Scp035.CorrodeUpdate(); } catch { }
				try { plugin.Stalky.CooldownUpdate(); } catch { }
				try { FixLogicer(); } catch { }
				try { foreach (var pl in Player.List) try { CheckEscape(pl); } catch { } } catch { }
				try { Textures.Models.Rooms.Range.ClearWaitInv(); } catch { }
				try { Textures.Models.Rooms.Range.GetCheaters(); } catch { }

				yield return Timing.WaitForSeconds(1f);
			}
		}
		private IEnumerator<float> FastСycle()
		{
			for (; ; )
			{
				try { Addons.Gate3.DoorTeleport(); } catch { }
				yield return Timing.WaitForSeconds(0.3f);
			}
		}
		private IEnumerator<float> SlowСycle()
		{
			for (; ; )
			{
				try { plugin.Scp035.CorrodeHost(); } catch { }
				try { AMCRandom = UnityEngine.Random.Range(0, 15); } catch { }
				try { ScientistNull(); } catch { }
				try { AutoAlpha(); } catch { }
				try { PosCheck(); } catch { }
				try { Names(); } catch { }
				try { Addons.RolePlay.Roles.Scp106.DoGet(); } catch { }
				try { Addons.YtSAutoOW.Update(); } catch { }
				yield return Timing.WaitForSeconds(5f);
			}
		}
		private IEnumerator<float> CountTicks()
		{
			for (; ; )
			{
				Plugin.Ticks++;
				yield return Timing.WaitForOneFrame;
			}
		}
		private int LowTPS = 0;
		private IEnumerator<float> CountTicksUpdate()
		{
			for (; ; )
			{
				Plugin.TicksMinutes = Plugin.Ticks / 5;
				Plugin.Ticks = 0;
				new Thread(() =>
				{
					new Thread(() =>
					{
						if (Plugin.TicksMinutes > 0 && 20 >= Plugin.TicksMinutes)
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
					if (Plugin.ServerID != 0 && !Plugin.ClansWars && Player.List.Count() > 1)
						try { Plugin.Socket.Emit("server.tps", new object[] { Plugin.ServerID, Plugin.ServerName, Plugin.TicksMinutes, Player.List.Count() }); } catch { }
				}).Start();
				yield return Timing.WaitForSeconds(5);
			}
		}
	}
}