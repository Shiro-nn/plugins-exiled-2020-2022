using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using System.Linq;
using UnityEngine;
namespace scp181
{
    public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		string[] oneno = { "HID", "LCZ_ARMORY", "ARMORY", "012_BOTTOM" };
		private int tries = 0;
		private int door_tries = 0;
		public void WaitingForPlayers() => plugin.cfg1();
		public void RoundStart()
		{
			tries = 0;
			door_tries = 0;
		}
		public void door(InteractingDoorEventArgs ev)
		{
			if (ev.Player.Health > 700 && ev.Player.Role == RoleType.ClassD) return;
			if (ev.IsAllowed == false && ev.Player.Team == Team.CDP && Random.Range(0, plugin.config.MaxDoorTries + 1) > door_tries)
			{
				try
				{
					foreach (string doorName in oneno)
					{
						try
						{
							if (ev.Door.GetComponent<DoorNametagExtension>().GetName.Equals(doorName))
							{
								ev.Player.ClearBroadcasts();
								ev.Player.Broadcast(plugin.config.dontopent, plugin.config.dontopen);
								return;
							}
						}
						catch { }
					}
				}
				catch { }
				door_tries++;
				ev.IsAllowed = true;
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(plugin.config.opent, plugin.config.open);
			}
		}
		public void hurt(HurtingEventArgs ev)
		{
			if (ev.Target.Health > 700 && ev.Target.Role == RoleType.ClassD) return;
			if (ev.Attacker.Health > 700 && ev.Attacker.Role == RoleType.ClassD) return;
			if (ev.Attacker.Team == Team.SCP && ev.Target.Team == Team.CDP && Random.Range(0, plugin.config.MaxTries) > tries)
			{
				ev.Amount = 0f;
				ev.Target.ClearBroadcasts();
				ev.Target.Broadcast(plugin.config.safet, plugin.config.safe);
				ev.Attacker.ClearBroadcasts();
				ev.Attacker.Broadcast(plugin.config.anti_safet, plugin.config.anti_safe);
				tries++;
				if (ev.Attacker.Role == RoleType.Scp106)
					TeleportTo106(ev.Target);
			}
		}
		private void TeleportTo106(Player player)
		{
			Vector3 toded = Map.GetRandomSpawnPoint(RoleType.Scp096);
			Timing.CallDelayed(1f, () => player.Position = toded);
		}
	}
}