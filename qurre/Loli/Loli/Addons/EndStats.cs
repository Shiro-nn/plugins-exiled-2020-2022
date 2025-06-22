using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
namespace Loli.Addons
{
    internal static class EndStats
    {
        internal static readonly Dictionary<string, TimeSpan> Escapes = new();
        internal static readonly Dictionary<string, int> Kills = new();
        internal static readonly Dictionary<string, int> ScpKills = new();
        internal static readonly Dictionary<string, int> Damages = new();

        [EventMethod(RoundEvents.Waiting)]
        [EventMethod(RoundEvents.Start)]
        internal static void Refresh()
        {
            Escapes.Clear();
            Kills.Clear();
            ScpKills.Clear();
        }

        [EventMethod(PlayerEvents.Dead)]
        internal static void Dead(DeadEvent ev)
        {
            if (ev.Attacker == ev.Target) return;
            if (ev.Attacker == null || ev.Target == null) return;
            if (ev.Attacker.RoleInfomation.Team == Team.SCPs)
            {
                if (!ScpKills.ContainsKey(ev.Attacker.UserInfomation.Nickname)) ScpKills.Add(ev.Attacker.UserInfomation.Nickname, 1);
                else ScpKills[ev.Attacker.UserInfomation.Nickname]++;
            }
            else
            {
                if (!Kills.ContainsKey(ev.Attacker.UserInfomation.Nickname)) Kills.Add(ev.Attacker.UserInfomation.Nickname, 1);
                else Kills[ev.Attacker.UserInfomation.Nickname]++;
            }
        }
        internal static void Pocket(PocketFailEscapeEvent _)
        {
            if (!Player.List.TryFind(out var pl, x => x.RoleInfomation.Role is RoleTypeId.Scp106)) return;
            if (!ScpKills.ContainsKey(pl.UserInfomation.Nickname)) ScpKills.Add(pl.UserInfomation.Nickname, 1);
            else ScpKills[pl.UserInfomation.Nickname]++;
        }

        [EventMethod(PlayerEvents.Attack)]
        internal static void Damage(AttackEvent ev)
        {
            if (!ev.Allowed) return;
            if (ev.FriendlyFire && !Server.FriendlyFire) return;
            if (ev.Attacker == ev.Target) return;
            if (ev.Attacker == null || ev.Target == null) return;
            if (ev.Attacker.RoleInfomation.Team == Team.SCPs) return;
            if (ev.Damage < 1) return;
            int am = (int)ev.Damage;
            if (!Damages.ContainsKey(ev.Attacker.UserInfomation.Nickname))
                Damages.Add(ev.Attacker.UserInfomation.Nickname, am);
            else Damages[ev.Attacker.UserInfomation.Nickname] += am;
        }

        [EventMethod(PlayerEvents.Escape)]
        internal static void Escape(EscapeEvent ev)
        {
            if (!ev.Allowed) return;
            if (Escapes.ContainsKey(ev.Player.UserInfomation.Nickname)) return;
            Escapes.Add(ev.Player.UserInfomation.Nickname, Round.ElapsedTime);
        }
    }
}