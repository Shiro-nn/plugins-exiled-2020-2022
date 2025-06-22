using MEC;
using UnityEngine;
using Loli.Builds.Models;
using Loli.Builds.Models.Rooms;
using Qurre.Events;
using Qurre.API.Attributes;
using Qurre.Events.Structs;
using Qurre.Events.Structs.Map;

namespace Loli.Builds
{
    internal static class Load
    {
        internal const string StaticDoorName = "StaticDoorNameInvisible";
        internal static readonly Color32 WhiteColor = new(175, 175, 175, 255);

        [EventMethod(RoundEvents.Waiting)]
        internal static void Waiting()
        {
            try { Servers.ThisList.Clear(); } catch { }
            try { Servers._doorOpen.Clear(); } catch { }
            try { Servers.Monitors.Clear(); } catch { }
            try { Control.Monitors.Clear(); } catch { }
            try { Control.Buttons.Clear(); } catch { }
            try { Server.Doors.Clear(); } catch { }
            try { Panel.Panels.Clear(); } catch { }
            try { Lift.List.Clear(); } catch { }
            Timing.KillCoroutines("DoorOpenCloseInServer");
            Timing.KillCoroutines("DoorOpenCloseInServersRoom");
            Timing.KillCoroutines("ServerLightBlink");
            Timing.KillCoroutines("SpawnServersInServersRoom");
            Timing.KillCoroutines("CustomLiftRunning");
            Timing.KillCoroutines("TexturesChildAndNotPrefereCoroutine");
            Timing.KillCoroutines("NeonLightModel");
            Timing.KillCoroutines(Panel.TimeCoroutinesName);
            Timing.KillCoroutines(Control.TimeCoroutinesName);
            Timing.CallDelayed(0.5f, () => Initialize());
        }

        [EventMethod(RoundEvents.Start)]
        internal static void RoundStart()
        {
            Timing.CallDelayed(3f, () => Bashni.Load());
        }

        [EventMethod(RoundEvents.End)]
        internal static void End()
        {
            Timing.KillCoroutines("ServerLightBlink");
            Timing.KillCoroutines("SpawnServersInServersRoom");
        }
        internal static void Initialize()
        {
            new Radar(new(-5.86f, 1008.38f, -23.69f), Vector3.zero);
            new Radar(new(179, 996.5f, 29), Vector3.zero);
            new Podval(new(115, 974, 80), Vector3.zero);
            Timing.CallDelayed(0.1f, () =>
            {
                try
                {
                    new Control();
                    ServersManager.Initialize();
                    Panel.Initialize();
                    RadarLoc.Load();
                }
                catch (System.Exception e)
                {
                    Qurre.API.Log.Error(e);
                }
            });
        }

        [EventMethod(PlayerEvents.PrePickupItem)]
        static void PrePickup(PrePickupItemEvent ev)
        {
            {
                if (Server.Doors.TryGetValue(ev.Pickup.Base, out var _data))
                {
                    _data.InteractDoor();
                    ev.Allowed = false;
                    return;
                }
            }
        }

        [EventMethod(MapEvents.DamageDoor)]
        internal static void DoorEvents(DamageDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not StaticDoorName) return;
            ev.Allowed = false;
        }

        [EventMethod(MapEvents.LockDoor)]
        internal static void DoorEvents(LockDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not StaticDoorName) return;
            ev.Allowed = false;
        }

        [EventMethod(MapEvents.OpenDoor)]
        internal static void DoorEvents(OpenDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not StaticDoorName) return;
            ev.Allowed = false;
        }
    }
}