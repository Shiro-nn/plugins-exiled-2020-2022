using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.DataBase.Modules.Controllers
{
    internal class Nimb
    {
        internal static readonly List<Nimb> Nimbs = new();
        internal Nimb(Player pl)
        {
            var _pos = new Vector3(0, 1.28f, 0);
            var prim = new Primitive(PrimitiveType.Cylinder, pl.Position, new Color(4, 3, 2), default, new Vector3(0.3f, 0.01f, 0.3f), false);
            prim.Base.transform.parent = pl.Transform;
            prim.Base.transform.localPosition = _pos;
            Player = pl;
            Primitive = prim;
            Nimbs.Add(this);
        }
        internal readonly Player Player;
        internal readonly Primitive Primitive;
        internal void Destroy()
        {
            try { Nimbs.Remove(this); } catch { }
            try { Primitive.Destroy(); } catch { }
        }
        internal static void Update(EffectEnabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Primitive.Base.transform.parent = null; } catch { }
            try { obj.Primitive.Base.transform.position = Vector3.zero; } catch { }
        }
        internal static void Update(EffectDisabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Primitive.Base.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Primitive.Base.transform.localPosition = new Vector3(0, 1.28f * ev.Player.Scale.y, 0); } catch { }
        }
        internal static void Leave(LeaveEvent ev)
        {
            if (!TryGet(ev.Player, out var obj)) return;
            Nimbs.Remove(obj);
            try { obj.Primitive.Destroy(); } catch { }
        }
        internal static void RoleChange(RoleChangeEvent ev)
        {
            if (ev.NewRole != RoleType.Spectator) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Primitive.Base.transform.parent = null; } catch { }
            try { obj.Primitive.Base.transform.position = Vector3.zero; } catch { }
        }
        internal static void RoleChange(SpawnEvent ev)
        {
            if (ev.RoleType == RoleType.Spectator || ev.RoleType == RoleType.None) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Primitive.Base.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Primitive.Base.transform.localPosition = new Vector3(0, 1.28f * ev.Player.Scale.y, 0); } catch { }
        }
        internal static void Waiting() => Nimbs.Clear();
        private static bool TryGet(Player pl, out Nimb nimb)
        {
            var list = Nimbs.ToArray();
            foreach (var item in list)
                if (item.Player == pl)
                {
                    nimb = item;
                    return true;
                }
            nimb = default;
            return false;
        }
    }
}