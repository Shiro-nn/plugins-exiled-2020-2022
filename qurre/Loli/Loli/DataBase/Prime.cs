using InventorySystem.Items.MicroHID;
using Loli.DataBase.Modules;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.DataBase
{
    static class Prime
    {
        [EventMethod(PlayerEvents.UsingRadio)]
        static void RadioUnlimited(UsingRadioEvent ev)
        {
            if (!Data.Roles.TryGetValue(ev.Player.UserInfomation.UserId, out var data)) return;
            if (data.Prime)
            {
                ev.Consumption = 0;
                ev.Battery = 100;
            }
        }

        static void HidMore(MicroHidUsingEvent ev)
        {
            if (!Data.Roles.TryGetValue(ev.Player.UserInfomation.UserId, out var data)) return;
            if (ev.State != HidState.Idle && data.Prime) ev.Coefficient = 0.7f;
        }
    }
}