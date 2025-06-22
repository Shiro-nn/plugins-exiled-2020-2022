using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Addons
{
    static class Caches
    {
        static internal Dictionary<string, VecPos> Positions { get; } = new();
        static internal Dictionary<int, RoleTypeId> Role { get; } = new();

        static internal bool IsAlive(string userid)
        {
            if (!Positions.TryGetValue(userid, out var _data))
                return true;

            return _data.Alive;
        }

        static internal void PosCheck()
        {
            try
            {
                foreach (Player pl in Player.List)
                {
                    if (!Positions.ContainsKey(pl.UserInfomation.UserId))
                        Positions.Add(pl.UserInfomation.UserId, new VecPos());

                    if (pl.RoleInfomation.Role is not RoleTypeId.Spectator &&
                        Vector3.Distance(Positions[pl.UserInfomation.UserId].Pos, pl.MovementState.Position) < 0.1)
                    {
                        if (Positions[pl.UserInfomation.UserId].sec > 30)
                        {
                            Positions[pl.UserInfomation.UserId].Alive = false;
                        }
                        else
                        {
                            Positions[pl.UserInfomation.UserId].sec += 5;
                            Positions[pl.UserInfomation.UserId].Pos = pl.MovementState.Position;
                        }
                    }
                    else
                    {
                        Positions[pl.UserInfomation.UserId].Alive = true;
                        Positions[pl.UserInfomation.UserId].sec = 0;
                        Positions[pl.UserInfomation.UserId].Pos = pl.MovementState.Position;
                    }
                }
            }
            catch { }
        }

        [EventMethod(PlayerEvents.Join)]
        static void Join(JoinEvent ev)
        {
            if (Role.ContainsKey(ev.Player.UserInfomation.Id))
                Role.Remove(ev.Player.UserInfomation.Id);

            Role.Add(ev.Player.UserInfomation.Id, RoleTypeId.Spectator);
        }

        [EventMethod(PlayerEvents.Dies, int.MinValue)]
        static void Dies(DiesEvent ev)
        {
            if (!ev.Allowed)
                return;

            if (!Role.ContainsKey(ev.Target.UserInfomation.Id))
                Role.Add(ev.Target.UserInfomation.Id, ev.Target.RoleInfomation.Role);
            else
                Role[ev.Target.UserInfomation.Id] = ev.Target.RoleInfomation.Role;
        }

        [Serializable]
        internal class VecPos
        {
            internal int sec = 0;
            internal Vector3 Pos = new();
            internal bool Alive { get; set; } = true;
        }
    }
}