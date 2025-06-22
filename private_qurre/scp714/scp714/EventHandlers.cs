using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
namespace scp714
{
	public class EventHandlers
	{
		private ushort ser = 0;
		internal void RoundStart()
		{
			ser = new Item(ItemType.Flashlight).Spawn(Map.GetRandomSpawnPoint(RoleType.Scp049)).Serial;
		}
		internal void Pickup(PickupItemEvent ev)
		{
			if (ev.Pickup.Serial == ser)
				ev.Player.Broadcast(Cfg.PickupBcTime, Cfg.PickupBc);
		}
		internal void Dying(DiesEvent ev)
		{
			if (ev.Target.ItemInHand == null) return;
			if (ev.Target.ItemInHand.Serial == ser)
			{
				if (ev.Killer.Role == RoleType.Scp049) ev.Allowed = false;
				if (ev.Killer.Role == RoleType.Scp93953 || ev.Killer.Role == RoleType.Scp93989)
					ev.Target.DisableEffect(EffectType.Amnesia);
			}
		}
		internal IEnumerator<float> aok()
		{
			for (; ; )
			{
				foreach (Player player in Player.List.Where(x => x.ItemInHand?.Serial == ser))
					player.EnableEffect(EffectType.SinkHole);
				foreach (Player player in Player.List.Where(x => x.ItemInHand?.Serial != ser && x.Room.Type != RoomType.Pocket))
					player.DisableEffect(EffectType.SinkHole);
				yield return MEC.Timing.WaitForSeconds(1f);
			}
		}
	}
}