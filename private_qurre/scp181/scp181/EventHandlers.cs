using MEC;
using Qurre.API;
using Qurre.API.Events;
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
		public void WaitingForPlayers() => Cfg.Reload();
		public void RoundStart()
		{
			tries = 0;
			door_tries = 0;
		}
		public void door(InteractDoorEvent ev)
		{
			if (ev.Door.Permissions.RequiredPermissions == Interactables.Interobjects.DoorUtils.KeycardPermissions.None) return;
			if (ev.Player.Hp > 700 && ev.Player.Role == RoleType.ClassD) return;
			if (ev.Allowed == false && ev.Player.Team == Team.CDP && Random.Range(0, Cfg.MaxDoorTries + 1) > door_tries)
			{
				try
				{
					foreach (string doorName in oneno)
					{
						try
						{
							if (ev.Door.Name.Contains(doorName))
							{
								ev.Player.ClearBroadcasts();
								ev.Player.Broadcast(Cfg.dontopent, Cfg.dontopen);
								return;
							}
						}
						catch { }
					}
				}
				catch { }
				door_tries++;
				ev.Allowed = true;
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Cfg.opent, Cfg.open);
			}
		}
		public void hurt(DamageEvent ev)
		{
			if (ev.Target.Hp > 700 && ev.Target.Role == RoleType.ClassD) return;
			if (ev.Attacker.Hp > 700 && ev.Attacker.Role == RoleType.ClassD) return;
			if (ev.Attacker.Team == Team.SCP && ev.Target.Team == Team.CDP && Random.Range(0, Cfg.MaxTries) > tries)
			{
				ev.Amount = 0f;
				ev.Target.ClearBroadcasts();
				ev.Target.Broadcast(Cfg.safet, Cfg.safe);
				ev.Attacker.ClearBroadcasts();
				ev.Attacker.Broadcast(Cfg.anti_safet, Cfg.anti_safe);
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