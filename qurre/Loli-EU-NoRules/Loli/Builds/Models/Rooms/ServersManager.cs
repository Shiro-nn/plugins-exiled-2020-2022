using InventorySystem.Items.Pickups;
using Loli.Addons;
using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Builds.Models.Rooms
{
    static class ServersManager
    {
        [EventMethod(PlayerEvents.InteractDoor)]
        static internal void Event(InteractDoorEvent ev)
        {
            if (!Buttons.Contains(ev.Door))
                return;
            ev.Allowed = false;
            InteractHack(ev.Player);
        }

        internal static HackMode Status { get; private set; } = HackMode.Safe;
        internal static int Proccess { get; private set; } = 0;
        private static bool Hacking = false;
        internal static readonly List<Door> Buttons = new();
        internal static Servers ServersRoom { get; private set; }
        internal static Vector3 PanelPosition = Vector3.zero;
        internal const string TimeCoroutinesName = "TimeCoroutine_ServersManager_LoliPlugin";
        internal static void InteractHack(Player pl)
        {
            if (Hacking) return;
            if (Status == HackMode.Hacked) return;
            if (Control.Status != HackMode.Hacked) return;
            if (pl == null) return;
            if (!pl.ItsHacker()) return;
            Hacking = true;
            Status = HackMode.Hacking;
            Timing.RunCoroutine(UpdateColorMonitor(), TimeCoroutinesName);
            Timing.RunCoroutine(CheckDistance());
            IEnumerator<float> UpdateColorMonitor()
            {
                yield return Timing.WaitForSeconds(0.3f);
                for (; ; )
                {
                    foreach (var m in Servers.Monitors) try { m.Primitive.Color = Panel._yellow; } catch { }
                    yield return Timing.WaitForSeconds(1.5f);
                    foreach (var m in Servers.Monitors) try { m.Primitive.Color = Panel._yellowTusklo; } catch { }
                    yield return Timing.WaitForSeconds(1.5f);
                }
            }
            IEnumerator<float> CheckDistance()
            {
                while (Hacking)
                {
                    yield return Timing.WaitForSeconds(1f);
                    try { Vector3.Distance(PanelPosition, pl.MovementState.Position); }
                    catch
                    {
                        Proccess = 0;
                        Status = HackMode.Safe;
                        Timing.KillCoroutines(TimeCoroutinesName);
                        foreach (var m in Servers.Monitors) try { m.Primitive.Color = Control.GetRandomMonitorColor(); } catch { }
                        Hacking = false;
                        yield break;
                    }
                    if (Vector3.Distance(PanelPosition, pl.MovementState.Position) > 15)
                    {
                        Proccess = 0;
                        Status = HackMode.Safe;
                        Timing.KillCoroutines(TimeCoroutinesName);
                        yield return Timing.WaitForSeconds(0.3f);
                        foreach (var m in Servers.Monitors) try { m.Primitive.Color = Control.GetRandomMonitorColor(); } catch { }
                        Hacking = false;
                        yield break;
                    }
                    Proccess += 1;
                    if (Proccess > 99)
                    {
                        Proccess = 100;
                        Status = HackMode.Hacked;
                        Timing.KillCoroutines(TimeCoroutinesName);
                        yield return Timing.WaitForSeconds(0.3f);
                        foreach (var m in Servers.Monitors) try { m.Primitive.Color = Panel._red; } catch { }
                        Hacking = false;
                        var str = $"<color=rainbow><b>Внимание всему персоналу</b></color>\n" +
                            $"<size=70%><color=#6f6f6f><color=red>Хакер</color> <color=green>Повстанцев Хаоса</color> успешно выкачал <color=red>все</color> данные комплекса.\n" +
                            $"Запущена <color=#0089c7>ОМЕГА Боеголовка</color></color></size>";
                        OmegaWarhead.Start();
                        var bc = Map.Broadcast(str, 30, true);
                        Timing.RunCoroutine(RainbowColorBc(bc, str));
                        yield break;
                    }
                }
            }
            static IEnumerator<float> RainbowColorBc(MapBroadcast bc, string str)
            {
                bool red_color = false;
                for (int i = 0; i < 16; i++)
                {
                    yield return Timing.WaitForSeconds(1f);
                    var color = "#fdffbb";
                    if (red_color)
                    {
                        color = "#ff0000";
                        red_color = false;
                    }
                    else red_color = true;
                    bc.Message = str.Replace("rainbow", color);
                }
            }
        }
        internal static void Initialize()
        {
            Status = HackMode.Safe;
            Proccess = 0;
            Hacking = false;
            Servers.Monitors.Clear();
            ServersRoom = new Servers(new Vector3(-88.47f, -810, -69.76f), Vector3.zero);
            new Lift(new(ServersRoom.Model.GameObject.transform, new(7.22f, 2.5f, -4.14f), Vector3.zero, true),
                new(null, new(-59.108f, 992.9f, -53.641f), Vector3.zero, true), Color.white, true);
        }
    }
}