using MEC;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

#pragma warning disable IDE0051
namespace capybara_fix106
{
    public sealed class Events
    {
        [PluginEvent(ServerEventType.RoundStart)]
        void Respawn106()
        {
            Timing.CallDelayed(5f, () =>
            {
                foreach (Player pl in Player.GetPlayers())
                {
                    if (pl.Role != RoleTypeId.Scp106)
                        continue;

                    pl.SendBroadcast(Plugin.Instance.Config.BroadCast, Plugin.Instance.Config.BroadCastSeconds);
                    pl.SetRole(RoleTypeId.Spectator, RoleChangeReason.RemoteAdmin);

                    Timing.CallDelayed(Plugin.Instance.Config.RespawnSecs, () =>
                    {
                        if (pl.Role != RoleTypeId.Spectator)
                            return;

                        pl.SetRole(RoleTypeId.Scp106, RoleChangeReason.RemoteAdmin);
                    });
                }
            });
        }
    }
}
#pragma warning restore IDE0051