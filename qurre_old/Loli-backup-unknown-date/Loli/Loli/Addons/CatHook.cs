using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Loli.Scps.Api;
using Loli.DataBase;
namespace Loli.Addons
{
    public class CatHook
    {
        internal static Player hook_owner;
        internal Pickup hook;
        private int hook_id = 0;
        internal void Waiting()
        {
            hook_owner = null;
            hook = null;
        }
        internal void RoundStart()
        {
            hook_owner = null;
            hook = null;
            if (Plugin.RolePlay) return;
            if (Plugin.ClansWars) return;
            var _list = Map.Pickups;
            _list.Shuffle();
            Vector3 spawn = _list.FirstOrDefault().Position;
            hook = new Item(ItemType.Flashlight).Spawn(spawn + Vector3.up * 1);
        }
        internal void Console(SendingConsoleEvent ev)
        {
            if (ev.Name != "hook" && ev.Name != "cat_hook") return;
            ev.Allowed = false;
            if (ev.Args.Count() == 0) ev.ReturnMessage = "\n.hook run\n.hook drop\n.hook info";
            try
            {
                if (ev.Player.Id == hook_owner?.Id)
                {
                    if (ev.Args[0].ToLower() == "run")
                    {
                        if (ev.Player.ItemTypeInHand == ItemType.None || ev.Player.ItemTypeInHand == ItemType.Flashlight)
                        {
                            if (ev.Player.Room?.Zone == ZoneType.Surface) ev.Player.ShowHint("<b><color=red>Крюк-кошку нельзя использовать в данной местности.</color></b>", 5);
                            else Timing.RunCoroutine(Teleport(ev.Player));
                        }
                        else ev.Player.ShowHint("<b><color=red>Для использования крюк-кошки,</color></b>\n<b><color=red>вы должны убрать предмет из руки.</color></b>", 5);
                    }
                    else if (ev.Args[0].ToLower() == "drop")
                    {
                        hook_owner = null;
                        hook = new Item(ItemType.Flashlight).Spawn(ev.Player.Position);
                        try { Levels.SetPrefix(ev.Player); } catch { }
                    }
                }
            }
            catch { }
            if (ev.Args[0].ToLower() == "info")
            {
                ev.ReturnMessage = "крюк-кошка - прибор из серии игр Just Cause™, который позволяет быстро перемещаться по-воздуху\nпоможет убежать!";
            }
            else if (ev.Args[0].ToLower() == "owner" && ev.Player.UserId == "-@steam")
            {
                hook_owner = ev.Player;
                try { hook.Destroy(); } catch { }
                hook = new Item(ItemType.Flashlight).Spawn(ev.Player.Position);
            }
        }
        internal void Dead(DiesEvent ev)
        {
            if (Plugin.RolePlay) return;
            if (Plugin.ClansWars) return;
            if (ev.Target?.Id == hook_owner?.Id && ev.Allowed)
            {
                hook_owner = null;
                hook = new Item(ItemType.Flashlight).Spawn(ev.Target.Position);
                try { Levels.SetPrefix(ev.Target); } catch { }
            }
        }
        internal void Pickup(PickupItemEvent ev)
        {
            try
            {
                if (Plugin.RolePlay) return;
                if (Plugin.ClansWars) return;
                else if (ev.Pickup == hook)
                {
                    ev.Player.Broadcast(5, "<b><color=lime>Вы подобрали <color=#0089c7>крюк</color>-<color=#0089c7>кошку</color></color></b>\n<color=#fdffbb>Подробнее:</color> <color=red>.</color><color=cyan>cat_hook info</color>");
                    ev.Allowed = false;
                    ev.Pickup.Destroy();
                    hook_owner = ev.Player;
                    try { Levels.SetPrefix(ev.Player); } catch { }
                }
            }
            catch { }
        }

        internal IEnumerator<float> Teleport(Player player)
        {
            Vector3 position = Getcampos(player.GameObject);
            if (position.x != 0 && player.Team != Team.SCP)
            {
                hook_id++;
                int _id = hook_id;
                float startTime = Time.time;
                float duration = 5.0f;
                float t = (Time.time - startTime) / duration;
                float x = Mathf.SmoothStep(player.Position.x, position.x, t);
                float y = Mathf.SmoothStep(player.Position.y, position.y, t);
                float z = Mathf.SmoothStep(player.Position.z, position.z, t);
                float lx = x;
                float ly = y;
                float lz = z;
                for (int ok = 0; ok < 30;)
                {
                    t = (Time.time - startTime) / duration;
                    x = Mathf.SmoothStep(lx, position.x, t);
                    y = Mathf.SmoothStep(ly, position.y, t);
                    z = Mathf.SmoothStep(lz, position.z, t);
                    if (Vector3.Distance(position, new Vector3(lx, ly, lz)) > 1f && (lx != x || ly != y) && _id == hook_id)
                    {
                        try
                        {
                            lx = x;
                            ly = y;
                            lz = z;
                            var pos = new Vector3(x, y, z);
                            if (!Physics.Linecast(player.Position, pos, player.Movement.CollidableSurfaces))
                            {
                                player.Position = pos;
                                ok = 0;
                            }
                            else ok = 30;
                        }
                        catch { }
                    }
                    else ok++;
                    yield return Timing.WaitForSeconds(0.01f);
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
            else if (gobject.transform.position.y < raycastHit.point.y)
            {
                position = Vec3ToVector3(raycastHit.point) + Vector3.down;
            }
            return position;
        }
        public static Vector3 Vec3ToVector3(Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}