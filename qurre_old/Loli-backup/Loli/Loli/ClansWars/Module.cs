using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace Loli.ClansWars
{
    internal class Module
    {
        internal void Waiting()
        {
            if (!Plugin.ClansWars) return;
            Round.LobbyLock = true;
            Map.Rooms.Find(x => x.Type == RoomType.Surface).Lights.Color = new Color(0, 0, 0);
            Round.Start();
        }
        internal void Check(CheckEvent ev)
        {
            if (!Plugin.ClansWars) return;
            ev.RoundEnd = false;
        }
        internal void Join(JoinEvent ev)
        {
            if (!Plugin.ClansWars) return;
            Timing.CallDelayed(0.5f, () =>
            {
                ev.Player.BlockSpawnTeleport = true;
                ev.Player.Role = RoleType.Tutorial;
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.Position = new UnityEngine.Vector3(135, 979, -20);
                    ev.Player.EnableEffect(EffectType.Invigorated);
                    ev.Player.AddItem(ItemType.Flashlight);
                });
            });
        }
        internal void Damage(DamageEvent ev)
        {
            if (!Plugin.ClansWars) return;
            ev.Allowed = false;
        }
    }
}