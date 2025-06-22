using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using UnityEngine;
using Light = Qurre.API.Controllers.Light;
namespace Loli.DataBase.Modules.Controllers
{
    internal class Glow
    {
        internal static readonly List<Glow> Glows = new();
        private static readonly Vector3 Position = new(-0.572f, 1.425f, -0.155f);
        internal Glow(Player pl, Color32 color)
        {
            var lg = new Light(pl.Position, color, 1, 5);
            lg.Base.transform.parent = pl.Transform;
            lg.Base.transform.localPosition = Position;
            lg.EnableShadows = false;
            Player = pl;
            Light = lg;
            Glows.Add(this);
        }
        internal readonly Player Player;
        internal readonly Light Light;
        internal void Destroy()
        {
            try { Glows.Remove(this); } catch { }
            try { Light.Destroy(); } catch { }
        }
        internal static void Update(EffectEnabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = null; } catch { }
            try { obj.Light.Base.transform.position = Vector3.zero; } catch { }
        }
        internal static void Update(EffectDisabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Light.Base.transform.localPosition = Position; } catch { }
        }
        internal static void Leave(LeaveEvent ev)
        {
            if (!TryGet(ev.Player, out var obj)) return;
            Glows.Remove(obj);
            try { obj.Light.Destroy(); } catch { }
        }
        internal static void RoleChange(RoleChangeEvent ev)
        {
            if (ev.NewRole != RoleType.Spectator) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = null; } catch { }
            try { obj.Light.Base.transform.position = Vector3.zero; } catch { }
        }
        internal static void RoleChange(SpawnEvent ev)
        {
            if (ev.RoleType == RoleType.Spectator || ev.RoleType == RoleType.None) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Light.Base.transform.localPosition = Position; } catch { }
        }
        internal static void Waiting() => Glows.Clear();
        private static bool TryGet(Player pl, out Glow glow)
        {
            var list = Glows.ToArray();
            foreach (var item in list)
                if (item.Player == pl)
                {
                    glow = item;
                    return true;
                }
            glow = default;
            return false;
        }
    }
}