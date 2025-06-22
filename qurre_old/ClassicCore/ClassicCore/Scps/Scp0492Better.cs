using CustomPlayerEffects;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace ClassicCore.Scps
{
    public class Scp0492Better
    {
        internal void Spawn(SpawnEvent ev)
        {
            if (ev.Player.Scale.x != 1 || ev.Player.Scale.y != 1 || ev.Player.Scale.z != 1) ev.Player.Scale = new Vector3(1, 1, 1);
            ev.Player.Tag = ev.Player.Tag.Replace("BigZombie", "").Replace("SpeedZombie", "");
            if (ev.RoleType is not RoleType.Scp0492) return;
            Timing.CallDelayed(0.5f, () =>
            {
                int random = Random.Range(0, 100);
                if (30 >= random) SpawnZombie(ev.Player, "BigZombie");
                else if (60 >= random) SpawnZombie(ev.Player, "SpeedZombie");
            });
        }
        internal void Damage(DamageProcessEvent ev)
        {
            if (!ev.Allowed) return;
            if (ev.Attacker.Role is not RoleType.Scp0492) return;
            if (ev.Attacker.Tag.Contains("BigZombie")) ev.Amount *= 1.5f;
        }
        internal static void SpawnZombie(Player pl, string type)
        {
            pl.Tag = pl.Tag.Replace("BigZombie", "").Replace("SpeedZombie", "");
            if (pl.Role is RoleType.Spectator) return;
            if (pl.Role is not RoleType.Scp0492)
            {
                pl.BlockSpawnTeleport = true;
                pl.SetRole(RoleType.Scp0492);
            }
            pl.DisableAllEffects();
            if (type == "BigZombie")
            {
                pl.Hp = 1000;
                pl.MaxHp = 1000;
                pl.Scale = new Vector3(1.2f, 0.9f, 1.2f);
                pl.Tag = "BigZombie";
                Timing.CallDelayed(0.5f, () =>
                {
                    pl.Hp = 1000;
                    pl.MaxHp = 1000;
                });
            }
            else if (type == "SpeedZombie")
            {
                float scale = Random.Range(85, 90);
                pl.Hp = 350;
                pl.MaxHp = 350;
                pl.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
                pl.Tag = "SpeedZombie";
                if (pl.TryGetEffect(EffectType.MovementBoost, out PlayerEffect playerEffect))
                {
                    playerEffect.Intensity = 30;
                    pl.EnableEffect(playerEffect);
                }
                Timing.CallDelayed(0.5f, () =>
                {
                    pl.Hp = 350;
                    pl.MaxHp = 350;
                });
            }
        }
    }
}