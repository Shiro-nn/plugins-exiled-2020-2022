using MEC;
using UnityEngine;
using Qurre.API.Events;
using Loli.Textures.Models;
using Loli.Textures.Models.Rooms;
namespace Loli.Textures
{
    internal static class Load
    {
        internal static void Waiting()
        {
            try { Servers.ThisList.Clear(); } catch { }
            try { Servers.Doors.Clear(); } catch { }
            try { Servers.Monitors.Clear(); } catch { }
            try { ServersManager.Lift.Doors.Clear(); } catch { }
            try { ServersManager.Lift.Sensors.Clear(); } catch { }
            try { Control.Monitors.Clear(); } catch { }
            try { Control.Buttons.Clear(); } catch { }
            try { Server.Doors.Clear(); } catch { }
            try { Panel.Panels.Clear(); } catch { }
            try { Range.CustomDoors.Clear(); } catch { }
            try { Models.Lift.List.Clear(); } catch { }
            Timing.KillCoroutines("DoorOpenCloseInServer");
            Timing.KillCoroutines("DoorOpenCloseInServersRoom");
            Timing.KillCoroutines("ServerLightBlink");
            Timing.KillCoroutines("SpawnServersInServersRoom");
            Timing.KillCoroutines("CustomLiftRunning");
            Timing.KillCoroutines("TexturesChildAndNotPrefereCoroutine");
            Timing.KillCoroutines(Panel.TimeCoroutinesName);
            Timing.KillCoroutines(Control.TimeCoroutinesName);
            Timing.CallDelayed(0.5f, () => Initialize());
        }

        internal static void End(RoundEndEvent _)
        {
            Timing.KillCoroutines("ServerLightBlink");
            Timing.KillCoroutines("SpawnServersInServersRoom");
        }
        internal static void Initialize()
        {
            new Turret(new(3.982191f, 1000, -57.69f), Vector3.up * 90, Color.red, 2.5f);
            new Pizza(new(-184.599f, 989.074f, -93.458f), Vector3.up * 25.36f);
            new Radar(new(39, 1003.5f, -33), Vector3.zero);
            new Radar(new(179, 996.5f, 29), Vector3.zero);
            new Podval(new(115, 974, 80), Vector3.zero);
            new Range();
            Timing.CallDelayed(5f, () =>
            {
                new Control(new(-184, 991.75f, -101.15f), Vector3.zero);
                ServersManager.Initialize();
                Panel.Initialize();
            });
        }
    }
}