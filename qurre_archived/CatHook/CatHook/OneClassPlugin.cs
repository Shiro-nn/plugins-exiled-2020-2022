using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
namespace CatHook
{
    public class CatHook : Qurre.Plugin
    {
        private string _access = "-@steam";
        private string _m1 = "<b><color=red>Крюк-кошку нельзя использовать в данной местности.</color></b>";
        private string _m2 = "<b><color=red>Для использования крюк-кошки,</color></b>\n<b><color=red>вы должны убрать предмет из руки.</color></b>";
        private string _m3 = "Крюк-Кошка - прибор из серии игр Just Cause™, который позволяет быстро перемещаться по воздуху\nПоможет убежать!";
        private string _m4 = "<b><color=lime>Вы подобрали <color=#0089c7>крюк</color>-<color=#0089c7>кошку</color></color></b>\n<color=#fdffbb>Подробнее:</color> <color=red>.</color><color=cyan>cat_hook info</color>";

        public override string Developer => "fydne";
        public override string Name => "Cat Hook";
        public override void Enable()
        {
            _access = Config.GetString("cathook_access", "-@steam", "Те, кто смогут заспавнить крюк командой .hook owner");
            _m1 = Config.GetString("cathook_BlockInSurface", "<b><color=red>Крюк-кошку нельзя использовать в данной местности.</color></b>",
                "Хинт, сообщающий о том, что на улице нельзя использовать крюк-кошку");
            _m2 = Config.GetString("cathook_hideItem", "<b><color=red>Для использования крюк-кошки,</color></b>\n<b><color=red>вы должны убрать предмет из руки.</color></b>",
                "Хинт, сообщающий о том, что для использования крюка, необходимо убрать предмет из руки");
            _m3 = Config.GetString("cathook_desc", "Крюк-Кошка - прибор из серии игр Just Cause™, который позволяет быстро перемещаться по воздуху\nПоможет убежать!",
                "Описание крюка");
            _m4 = Config.GetString("cathook_pickupBC", "<b><color=lime>Вы подобрали <color=#0089c7>крюк</color>-<color=#0089c7>кошку</color></color></b>\n" +
                "<color=#fdffbb>Подробнее:</color> <color=red>.</color><color=cyan>cat_hook info</color>", "Сообщение игроку при подъеме крюка");
            Qurre.Events.Round.Waiting += Waiting;
            Qurre.Events.Round.Start += RoundStart;
            Qurre.Events.Player.Dies += Dead;
            Qurre.Events.Player.PickupItem += Pickup;
            Qurre.Events.Server.SendingConsole += Console;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= Waiting;
            Qurre.Events.Round.Start -= RoundStart;
            Qurre.Events.Player.Dies -= Dead;
            Qurre.Events.Player.PickupItem -= Pickup;
            Qurre.Events.Server.SendingConsole -= Console;
        }
        internal const string Tag = "CatHookActivated";
        internal Pickup hook;
        private int hook_id = 0;
        internal void Waiting()
        {
            hook = null;
        }
        internal void RoundStart()
        {
            hook = null;
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
                if (ev.Player.Tag.Contains(Tag))
                {
                    if (ev.Args[0].ToLower() == "run")
                    {
                        if (ev.Player.ItemTypeInHand == ItemType.None || ev.Player.ItemTypeInHand == ItemType.Flashlight)
                        {
                            if (ev.Player.Room?.Zone == ZoneType.Surface) ev.Player.ShowHint(_m1, 5);
                            else Timing.RunCoroutine(Teleport(ev.Player));
                        }
                        else ev.Player.ShowHint(_m2, 5);
                    }
                    else if (ev.Args[0].ToLower() == "drop")
                    {
                        ev.Player.Tag = ev.Player.Tag.Replace(Tag, "");
                        hook = new Item(ItemType.Flashlight).Spawn(ev.Player.Position);
                    }
                }
            }
            catch { }
            if (ev.Args[0].ToLower() == "info")
            {
                ev.ReturnMessage = _m3;
            }
            else if (ev.Args[0].ToLower() == "owner" && _access.Split(',').Where(x => x.Contains(ev.Player.UserId)).Count() > 0)
            {
                ev.Player.Tag += Tag;
                try { hook.Destroy(); } catch { }
            }
        }
        internal void Dead(DiesEvent ev)
        {
            if (ev.Target.Tag.Contains(Tag) && ev.Allowed)
            {
                ev.Target.Tag = ev.Target.Tag.Replace(Tag, "");
                hook = new Item(ItemType.Flashlight).Spawn(ev.Target.Position);
            }
        }
        internal void Pickup(PickupItemEvent ev)
        {
            try
            {
                if (ev.Pickup == hook)
                {
                    ev.Player.Broadcast(5, _m4);
                    ev.Allowed = false;
                    ev.Pickup.Destroy();
                    ev.Player.Tag += Tag;
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
            Physics.Raycast(component.plyCam.transform.position, forward, out RaycastHit raycastHit, 40f, component2.teleportPlacementMask);
            Vector3 position = raycastHit.point;
            if (gobject.transform.position.y > raycastHit.point.y)
                position = raycastHit.point + Vector3.up * 1f;
            else if (gobject.transform.position.y < raycastHit.point.y)
                position = raycastHit.point + Vector3.down;
            return position;
        }
    }
}