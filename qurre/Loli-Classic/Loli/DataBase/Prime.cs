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
    }
}