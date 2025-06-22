using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
namespace RESURRECTION_OF_THE_DEAD
{
    public partial class EventHandlers
    {
        public Dictionary<int, RoleType> DRole = new Dictionary<int, RoleType>();
        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Target.Team == Team.SCP || ev.Target.Team == Team.TUT) return;
            if (DRole.ContainsKey(ev.Target.ReferenceHub.queryProcessor.PlayerId)) DRole[ev.Target.ReferenceHub.queryProcessor.PlayerId] = ev.Target.Role;
            else DRole.Add(ev.Target.ReferenceHub.queryProcessor.PlayerId, ev.Target.Role);
        }
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Item.id != ItemType.SCP500) return;
            foreach (Ragdoll doll in Object.FindObjectsOfType<Ragdoll>())
            {
                if (Vector3.Distance(ev.Player.Position + Vector3.down, doll.transform.position) <= 2f)
                {
                    Player dead = Player.Get(doll.owner.PlayerId);
                    if (dead != null)
                    {
                        if (dead.Role == RoleType.Spectator)
                        {
                            NetworkServer.Destroy(doll.gameObject);
                            if (DRole.ContainsKey(dead.ReferenceHub.queryProcessor.PlayerId))
                            {
                                ev.IsAllowed = false;
                                ev.Player.RemoveItem(ev.Item);
                                dead.Role = DRole[dead.ReferenceHub.queryProcessor.PlayerId];
                                dead.Health = 20;
                                Timing.CallDelayed(0.5f, () => dead.Position = ev.Player.Position);
                                ev.Player.Broadcast(12, $"<color=red>Вы вылечили игрока {dead.Nickname}!</color>");
                                dead.Broadcast(12, $"<color=red>Вас вылечил {ev.Player.Nickname}!</color>");
                            }
                        }
                    }
                }
            }
        }
    }
}