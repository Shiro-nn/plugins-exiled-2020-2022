using MEC;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace godoff
{
    public sealed class Events
    {
        [PluginEvent(ServerEventType.RoundStart)]
        void Start()
        {
            Timing.CallDelayed(Plugin.Instance.Config.Delayed, () =>
            {
                foreach (Player pl in Player.GetPlayers())
                {
                    if (pl.Role is RoleTypeId.Spectator)
                        continue;

                    if (Plugin.Instance.Config.GodHps.Contains((int)pl.Health))
                        continue;

                    pl.IsGodModeEnabled = false;
                }
            });
        }
    }
}
