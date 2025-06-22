using Qurre.API.Events;
using System.Linq;
namespace Loli.Modules
{
    public partial class EventHandlers
    {
        internal void FixFreeze(PickupItemEvent ev)
        {
            if (ev.Player.AllItems.Count(x => x.Serial == ev.Pickup.Serial) > 0)
            {
                //Qurre.Log.Warn("Player_PickupItem SERVER CRASH ROUND => " + ev.Pickup.Serial.ToString() + ", TYPE  => " + ev.Pickup.Type.ToString());
                ev.Allowed = false;
                var item = ev.Player.AddItem(ev.Pickup.Type);
                var ser = ev.Pickup.Serial;
                ev.Pickup.Destroy();
                if (Pickups.Contains(ser)) Pickups.Add(item.Serial);
            }
        }
    }
}