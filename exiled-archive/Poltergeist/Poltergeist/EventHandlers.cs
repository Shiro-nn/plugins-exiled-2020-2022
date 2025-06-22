using MEC;
using Exiled.API.Features;
using hideandseek.API;
using Respawning;
using Grenades;
using Mirror;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Poltergeist
{
    partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static System.Random Gen = new System.Random();
		public static bool dr = false;
		public static bool dd = false;
		public bool pe = false;
		internal bool c1 = false;
		internal bool c2 = false;
		internal bool c3 = false;
		internal bool c4 = false;
		internal bool c5 = false;
		internal bool ps = false;
		private bool Tryhason()
		{
			return hasData.Gethason();
		}
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
			c1 = false;
			c2 = false;
			c3 = false;
			c4 = false;
			c5 = false;
			int ran = Gen.Next(300, 500);
			float ranf = ran;
			float ranft = ran+10;
			float ranff = ran + 51;
			float ranfd1 = ran + 1.5f;
			float ranfd2 = ran + 11.4f;
			Timing.CallDelayed(ranf, () => spawnp());
			Timing.CallDelayed(ranf, () => {
                if (!c1)
                {
					c1 = true;
					RespawnEffectsController.PlayCassieAnnouncement("pitch_0.1 .g6", false, false);
				}
			});
			Timing.CallDelayed(ranft, () => {
				if (!c2)
				{
					c2 = true;
				    RespawnEffectsController.PlayCassieAnnouncement("pitch_0.1  .g1 . .g4 . .g2 . .g5 . . .g6 .", false, false);
				}
			});
			Timing.CallDelayed(ranff, () => {
				if (!c3)
				{
					c3 = true;
					RespawnEffectsController.PlayCassieAnnouncement("pitch_0.1 .g1 .g5", false, false);
				}
			});
			Timing.CallDelayed(1.5f, () => {
				if (!c4)
				{
					c4 = true;
					RespawnEffectsController.PlayCassieAnnouncement("pitch_0.1 .g1", false, false);
				}
			});
			Timing.CallDelayed(ranfd1, () => {
				if (!c4)
				{
					c4 = true;
					RespawnEffectsController.PlayCassieAnnouncement("pitch_0.1 .g1", false, false);
				}
			});
			Timing.CallDelayed(ranfd2, () => {
				if (!c5)
				{
					c5 = true;
					RespawnEffectsController.PlayCassieAnnouncement("pitch_0.1 .g5", false, false);
				}
			});
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
			if (Tryhason()) return;
			Map.Broadcast(5, "<color=aqua>Появился</color> <color=red>полтергейст</color>");
			//foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
				//Extensions.Postfix(item);
			EventHandlers.dr = true;
			EventHandlers.dd = true;
			Extensions.Checkopen();
			Map.TurnOffAllLights(65, false);
			Timing.CallDelayed(51.3f, () =>
			{
				ps = false;
				EventHandlers.dr = false;
				EventHandlers.dd = false;
				Extensions.canBeFloating.Clear();
				Extensions.haveBeenMoved.Clear();
			});
		}
	}
}