using CustomPlayerEffects;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using UnityEngine;

namespace Loli.Scps
{
    static class Scp0492Better
    {
        [EventMethod(PlayerEvents.Spawn)]
        static void Spawn(SpawnEvent ev)
        {
            if (ev.Player.MovementState.Scale.x != 1 || ev.Player.MovementState.Scale.y != 1 || ev.Player.MovementState.Scale.z != 1)
                ev.Player.MovementState.Scale = Vector3.one;

            ev.Player.Tag = ev.Player.Tag.Replace("BigZombie", "").Replace("SpeedZombie", "");

            if (ev.Player.Tag.Contains("Scp008Invisible"))
                return;
            if (ev.Role is not RoleTypeId.Scp0492)
                return;

            Timing.CallDelayed(0.5f, () => SpawnZombieRandom(ev.Player));
        }

        [EventMethod(PlayerEvents.Attack)]
        static void Damage(AttackEvent ev)
        {
            if (!ev.Allowed)
                return;
            if (ev.Attacker.RoleInfomation.Role is not RoleTypeId.Scp0492)
                return;

            if (ev.Attacker.Tag.Contains("BigZombie"))
                ev.Damage *= 1.5f;
        }


        internal static void SpawnZombieRandom(Player pl)
        {
            int random = Random.Range(0, 100);
            if (30 >= random) SpawnZombie(pl, "BigZombie");
            else if (60 >= random) SpawnZombie(pl, "SpeedZombie");
        }
        internal static void SpawnZombie(Player pl, string type)
        {
            pl.Tag = pl.Tag.Replace("BigZombie", "").Replace("SpeedZombie", "");

            if (pl.RoleInfomation.Role is not RoleTypeId.Scp0492)
            {
                pl.Tag += "Scp008Invisible";
                pl.RoleInfomation.SetNew(RoleTypeId.Scp0492, RoleChangeReason.Respawn);
                Timing.CallDelayed(0.5f, () => pl.Tag = pl.Tag.Replace("Scp008Invisible", ""));
            }

            pl.Effects.DisableAll();

            if (type == "BigZombie")
            {
                pl.HealthInfomation.Hp = 1000;
                pl.HealthInfomation.MaxHp = 1000;
                pl.MovementState.Scale = new Vector3(1.2f, 0.9f, 1.2f);
                pl.Tag = "BigZombie";
                Timing.CallDelayed(0.5f, () =>
                {
                    pl.HealthInfomation.Hp = 1000;
                    pl.HealthInfomation.MaxHp = 1000;
                });
            }
            else if (type == "SpeedZombie")
            {
                float scale = Random.Range(85, 90);
                pl.HealthInfomation.Hp = 350;
                pl.HealthInfomation.MaxHp = 350;
                pl.MovementState.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
                pl.Tag = "SpeedZombie";
                if (pl.Effects.TryGet(EffectType.MovementBoost, out StatusEffectBase playerEffect))
                {
                    playerEffect.Intensity = 30;
                    pl.Effects.Enable(playerEffect);
                }
                Timing.CallDelayed(0.5f, () =>
                {
                    pl.HealthInfomation.Hp = 350;
                    pl.HealthInfomation.MaxHp = 350;
                });
            }
        }
    }
}