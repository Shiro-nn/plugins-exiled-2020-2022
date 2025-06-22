using CustomPlayerEffects;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace Loli.Addons.RolePlay.Roles
{
    static internal class Scp106
    {
        static internal void Spawn(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player.Role is not RoleType.Scp106) return;
            if (ev.Player.TryGetEffect(EffectType.SinkHole, out var effect))
            {
                if (effect is SinkHole hole) hole.slowAmount = 10;
                ev.Player.PlayerEffectsController.EnableEffect(effect, 0, false);
            }

        }
        static internal void DoGet()
        {
            if (!Plugin.RolePlay) return;
            foreach (var pl in Player.List)
            {
                if (pl.Role is not RoleType.Scp106) continue;
                if (pl.TryGetEffect(EffectType.SinkHole, out var effect))
                {
                    if (effect is SinkHole hole) hole.slowAmount = 10;
                    pl.PlayerEffectsController.EnableEffect(effect, 0, false);
                }
            }
        }
        static internal void Damage(DamageProcessEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Target.Role is not RoleType.Scp106) return;
            var rand = Random.Range(0, 10);
            if (rand > 6 || ev.Target.Room?.Type is RoomType.Pocket)
            {
                ev.Allowed = false;
                return;
            }
            switch (ev.PrimitiveType)
            {
                case DamageTypesPrimitive.Explosion: ev.Amount = 50; break;
                case DamageTypesPrimitive.Firearm or DamageTypesPrimitive.MicroHid: ev.Amount = 1; break;
                case DamageTypesPrimitive.Scp018: ev.Amount = 0; break;
            }
        }
    }
}