using Qurre.API;
using Qurre.API.Addons.Models;
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
            Vector3 _pos = new(0, 1.28f, 0);
            Vector3 _size = new(0.064f, 0.064f, 0.064f);
            Color col = new(4, 4, 0);
            Model = new("Nimb", _pos);
            Model.GameObject.transform.parent = pl.Transform;
            Model.GameObject.transform.localPosition = _pos;

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0.237376f, 0, -0.002016022f), new(0, -90, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(-0.2373761f, 0, -0.00604802f), new(0, 90, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(-0.1188481f, 0, -0.20848f), new(0, 30, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(-0.2055041f, 0, -0.122464f), new(0, 60, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0, 0, -0.24048f), new(0, 0, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(-0.2055041f, 0, 0.11312f), new(0, 120, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(-0.1181441f, 0, 0.200544f), new(0, 150, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0.118848f, 0, 0.200416f), new(0, -150, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0.205504f, 0, 0.1144f), new(0, -120, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0, 0, 0.232416f), new(0, 180, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0.205504f, 0, -0.121184f), new(0, -60, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, col, new(0.118144f, 0, -0.208608f), new(0, -30, 90), _size));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(-0.06291205f, 0, 0.232416f), new(0, 150, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(-0.1735041f, 0, 0.168544f), new(0, 120, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(-0.2373761f, 0, 0.05795198f), new(0, 0, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(-0.2373761f, 0, -0.06729602f), new(0, 60, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(-0.1744f, 0, -0.176416f), new(0, 30, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(-0.064f, 0, -0.24048f), new(0, 0, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(0.06291199f, 0, -0.24048f), new(0, -30, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(0.173504f, 0, -0.176608f), new(0, -60, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(0.237376f, 0, -0.06601603f), new(0, 180, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(0.237376f, 0, 0.05923199f), new(0, -120, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(0.1744f, 0, 0.168352f), new(0, -150, 90), _size));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Sphere, col, new(0.064f, 0, 0.232416f), new(0, 180, 90), _size));

            foreach (var prim in Model.Primitives)
            {
                prim.Primitive.MovementSmoothing = 64;
                prim.Primitive.Collider = false;
            }

            Model.GameObject.AddComponent<FixPrimitiveSmoothing>().Model = Model;

            Player = pl;

            Nimbs.Add(this);
        }
        internal readonly Player Player;
        internal readonly Model Model;
        internal void Destroy()
        {
            try { Nimbs.Remove(this); } catch { }
            try { Model.Destroy(); } catch { }
        }
        internal static void Update(EffectEnabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Model.GameObject.transform.parent = null; } catch { }
            try { obj.Model.GameObject.transform.position = Vector3.zero; } catch { }
        }
        internal static void Update(EffectDisabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Model.GameObject.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Model.GameObject.transform.localPosition = new Vector3(0, 1.28f * ev.Player.Scale.y, 0); } catch { }
        }
        internal static void Leave(LeaveEvent ev)
        {
            if (!TryGet(ev.Player, out var obj)) return;
            Nimbs.Remove(obj);
            try { obj.Model.Destroy(); } catch { }
        }
        internal static void RoleChange(RoleChangeEvent ev)
        {
            if (ev.NewRole != RoleType.Spectator) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Model.GameObject.transform.parent = null; } catch { }
            try { obj.Model.GameObject.transform.position = Vector3.zero; } catch { }
        }
        internal static void RoleChange(SpawnEvent ev)
        {
            if (ev.RoleType == RoleType.Spectator || ev.RoleType == RoleType.None) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.Model.GameObject.transform.parent = ev.Player.Transform; } catch { }
            try { obj.Model.GameObject.transform.localPosition = new Vector3(0, 1.28f * ev.Player.Scale.y, 0); } catch { }
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