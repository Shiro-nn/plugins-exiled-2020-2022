using InventorySystem.Items.MicroHID;
using Qurre.API.Events;
namespace ClassicCore.DataBase
{
    public class Prime
    {
        internal void RadioUnlimited(RadioUsingEvent ev)
        {
            if (!Manager.Static.Data.Roles.TryGetValue(ev.Player.UserId, out var data)) return;
            if (data.Prime)
            {
                ev.Consumption = 0;
                ev.Battery = 100;
            }
        }
        internal void HidMore(MicroHidUsingEvent ev)
        {
            if (!Manager.Static.Data.Roles.TryGetValue(ev.Player.UserId, out var data)) return;
            if (ev.State != HidState.Idle && data.Prime) ev.Coefficient = 0.7f;
        }
    }
}