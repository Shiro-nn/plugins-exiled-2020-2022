using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using Exiled.Events.EventArgs;
using Exiled.API.Features;
using MEC;

namespace MongoDB.jc3
{
    public class cat_hook
    {
        private readonly Plugin plugin;
        public cat_hook(Plugin plugin) => this.plugin = plugin;
        internal Player hook_owner;
        internal Pickup hook;
        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        internal void RoundStart()
        {
            hook_owner = null;
            hook = null;
            Vector3 spawn = UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.durability != 100000 && x.durability != 10000 && x.durability != 999999999).FirstOrDefault().transform.position;
            hook = Extensions.SpawnItem(ItemType.Disarmer, 10000, spawn + Vector3.up * 1);
        }
        internal void Console(SendingConsoleCommandEventArgs ev)
        {
            try
            {
                if (ev.Name == "hook" || ev.Name == "cat_hook")
                {
                    ev.IsAllowed = false;
                    if (ev.Arguments.Count > 0)
                    {
                        try
                        {
                            if (ev.Player.ReferenceHub.queryProcessor.PlayerId == hook_owner?.ReferenceHub?.queryProcessor.PlayerId)
                            {
                                if (ev.Arguments[0] == "run")
                                {
                                    Coroutines.Add(Timing.RunCoroutine(Teleport(ev.Player)));
                                }
                                else if (ev.Arguments[0] == "drop")
                                {
                                    hook_owner = null;
                                    hook = Extensions.SpawnItem(ItemType.Disarmer, 10000, ev.Player.ReferenceHub.transform.position);
                                    try { plugin.donate.setprefix(ev.Player.ReferenceHub); } catch { }
                                }
                            }
                        }
                        catch { }
                        if (ev.Arguments[0] == "info")
                        {
                            ev.ReturnMessage = "крюк-кошка - прибор из серии игр Just Cause™, который позволяет быстро перемещаться по-воздуху\nпоможет убежать!";
                        }
                        if (ev.Arguments[0] == "owner")
                        {
                            if (ev.Player.UserId == "-@steam")
                            {
                                hook_owner = ev.Player;
                                try { hook.Delete(); } catch { }
                                hook = Extensions.SpawnItem(ItemType.Disarmer, 10000, ev.Player.ReferenceHub.transform.position);
                            }
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = "\n.hook run\n.hook drop\n.hook info";
                    }
                }
            }
            catch
            {
                if (ev.Name == "hook" || ev.Name == "cat_hook")
                {
                    ev.IsAllowed = false;
                    ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
                    ev.Color = "red";
                }
            }
        }
        internal void Pickup(PickingUpItemEventArgs ev)
        {
            try
            {
                if (ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId || ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
                {
                    if (ev.Pickup == hook)
                    {
                        ev.IsAllowed = false;
                        ev.Pickup.Delete();
                        hook = Extensions.SpawnItem(ItemType.Disarmer, 10000, ev.Pickup.transform.position, ev.Pickup.transform.rotation);
                    }
                }
                else if (ev.Pickup == hook)
                {
                    ev.IsAllowed = false;
                    ev.Pickup.Delete();
                    hook_owner = ev.Player;
                    try { plugin.donate.setprefix(ev.Player.ReferenceHub); } catch { }
                    ev.Player.Broadcast(5, "<b><color=lime>Вы подобрали <color=#0089c7>крюк</color>-<color=#0089c7>кошку</color></color></b>\n<color=#fdffbb>Подробнее:</color> <color=red>.</color><color=cyan>cat_hook info</color>");
                }
            }
            catch { }
        }

        internal IEnumerator<float> Teleport(Player player)
        {
            int ok = 0;
            Vector3 position = Getcampos(player.ReferenceHub.gameObject);
            if (position.x != 0)
            {
                float startTime = Time.time;
                float duration = 5.0f;
                float t = (Time.time - startTime) / duration;
                float x = Mathf.SmoothStep(player.ReferenceHub.transform.position.x, position.x, t);
                float y = Mathf.SmoothStep(player.ReferenceHub.transform.position.y, position.y, t);
                float z = Mathf.SmoothStep(player.ReferenceHub.transform.position.z, position.z, t);
                float lx = x;
                float ly = y;
                float lz = z;
                for (; ; )
                {
                    t = (Time.time - startTime) / duration;
                    x = Mathf.SmoothStep(lx, position.x, t);
                    y = Mathf.SmoothStep(ly, position.y, t);
                    z = Mathf.SmoothStep(lz, position.z, t);
                    if (Vector3.Distance(position, new Vector3(lx, ly, lz)) > 1f && (lx != x || ly != y))
                    {
                        try
                        {
                            lx = x;
                            ly = y;
                            lz = z;
                            player.Position = new Vector3(x, y, z);
                            ok = 0;
                        }
                        catch { }
                    }
                    else
                    {
                        ok++;
                        if (ok > 5)
                        {
                            ok = 0;
                            yield return Timing.WaitForSeconds(10000f);
                        }
                    }
                    yield return Timing.WaitForSeconds(0.05f);
                }
            }
        }
        internal static Vector3 Getcampos(GameObject gobject)
        {
            Scp049_2PlayerScript component = gobject.GetComponent<Scp049_2PlayerScript>();
            Scp106PlayerScript component2 = gobject.GetComponent<Scp106PlayerScript>();
            Vector3 forward = component.plyCam.transform.forward;
            RaycastHit raycastHit;
            Physics.Raycast(component.plyCam.transform.position, forward, out raycastHit, 40f, component2.teleportPlacementMask);
            Vector3 position = Vec3ToVector3(raycastHit.point);
            if (gobject.transform.position.y > raycastHit.point.y)
            {
                position = Vec3ToVector3(raycastHit.point) + Vector3.up * 1f;
            }
            return position;
        }
        public static Vector3 Vec3ToVector3(Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}
