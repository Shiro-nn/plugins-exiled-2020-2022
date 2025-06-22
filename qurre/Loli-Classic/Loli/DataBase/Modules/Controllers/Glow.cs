using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.DataBase.Modules.Controllers
{
    internal class Glow
    {
        internal static readonly List<Glow> Glows = new();
        private static readonly Vector3 Position = new(-0.572f, 1.425f, -0.155f);
        internal Glow(Player pl, Color32 color)
        {
            var lg = new LightPoint(pl.MovementState.Position, color, 1, 5);
            lg.Base.transform.parent = pl.MovementState.Transform;
            lg.Base.transform.localPosition = Position;
            lg.EnableShadows = false;
            Player = pl;
            Light = lg;
            Glows.Add(this);
        }
        internal readonly Player Player;
        internal readonly LightPoint Light;
        internal void Destroy()
        {
            try { Glows.Remove(this); } catch { }
            try { Light.Destroy(); } catch { }
        }

        [EventMethod(EffectEvents.Enabled)]
        internal static void Update(EffectEnabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = null; } catch { }
            try { obj.Light.Base.transform.position = Vector3.zero; } catch { }
        }

        [EventMethod(EffectEvents.Disabled)]
        internal static void Update(EffectDisabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Light.Base.transform.localPosition = Position; } catch { }
        }

        [EventMethod(PlayerEvents.Leave)]
        static void Leave(LeaveEvent ev)
        {
            if (!TryGet(ev.Player, out var obj)) return;
            Glows.Remove(obj);
            try { obj.Light.Destroy(); } catch { }
        }

        [EventMethod(PlayerEvents.ChangeRole)]
        static void RoleChange(ChangeRoleEvent ev)
        {
            if (ev.Role != RoleTypeId.Spectator) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = null; } catch { }
            try { obj.Light.Base.transform.position = Vector3.zero; } catch { }
        }

        [EventMethod(PlayerEvents.Spawn)]
        static void RoleChange(SpawnEvent ev)
        {
            if (ev.Role == RoleTypeId.Spectator || ev.Role == RoleTypeId.None) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Light.Base.transform.parent = ev.Player.MovementState.Transform; } catch { }
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