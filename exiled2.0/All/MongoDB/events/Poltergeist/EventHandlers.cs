using MEC;
using Exiled.API.Features;
using Respawning;

namespace MongoDB.events.Poltergeist
{
	partial class EventHandlersP
	{
		public Plugin plugin;
		public EventHandlersP(Plugin plugin) => this.plugin = plugin;
		public static bool dr = false;
		public static bool dd = false;
		public bool pe = false;
		internal bool ps = false;
		internal int ran = MongoDB.Extensions.Random.Next(300, 500);
		public void OnWarheadStart(Exiled.Events.EventArgs.StartingEventArgs ev)
		{
			if (ps)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnRoundStart()
		{
			ps = false;
			pe = false;
			dr = false;
			dd = false;
			ran = MongoDB.Extensions.Random.Next(300, 500);
		}
		internal void enpolt()
		{
			if (plugin.EventHandlers343.roundtimeint == ran) spawnp();
		}
		public void spawnp()
		{
			if (pe) return;
			if (Warhead.IsInProgress)
			{
				Warhead.Stop();
			}
			ps = true;
			pe = true;
			Map.Broadcast(5, "<color=aqua>Появился</color> <color=red>полтергейст</color>");
			Cassie.Message("pitch_0.1 .g6", false, false);
			Timing.CallDelayed(10f, () => Cassie.Message("pitch_0.1  .g1 . .g4 . .g2 . .g5 . . .g6 .", false, false));
			Timing.CallDelayed(51f, () => Cassie.Message("pitch_0.1 .g1 .g5", false, false));
			Timing.CallDelayed(1.5f, () => Cassie.Message("pitch_0.1 .g1", false, false));
			Timing.CallDelayed(11.4f, () => Cassie.Message("pitch_0.1 .g5", false, false));
			dr = true;
			dd = true;
			Extensions.Checkopen();
			Map.TurnOffAllLights(65, false);
			Timing.CallDelayed(51.3f, () =>
			{
				ps = false;
				dr = false;
				dd = false;
				Extensions.canBeFloating.Clear();
				Extensions.haveBeenMoved.Clear();
			});
		}
	}
}