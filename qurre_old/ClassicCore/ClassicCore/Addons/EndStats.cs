using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
namespace ClassicCore.Addons
{
    internal static class EndStats
    {
        internal static readonly Dictionary<string, TimeSpan> Escapes = new();
        internal static readonly Dictionary<string, int> Kills = new();
        internal static readonly Dictionary<string, int> ScpKills = new();
        internal static readonly Dictionary<string, int> Damages = new();
        internal static void Refresh()
        {
            Escapes.Clear();
            Kills.Clear();
            ScpKills.Clear();
        }
        internal static void Dead(DeadEvent ev)
        {
            if (ev.Killer == ev.Target) return;
            if (ev.Killer == null || ev.Target == null) return;
            if (ev.Killer.Team == Team.SCP)
            {
                if (!ScpKills.ContainsKey(ev.Killer.Nickname)) ScpKills.Add(ev.Killer.Nickname, 1);
                else ScpKills[ev.Killer.Nickname]++;
            }
            else
            {
                if (!Kills.ContainsKey(ev.Killer.Nickname)) Kills.Add(ev.Killer.Nickname, 1);
                else Kills[ev.Killer.Nickname]++;
            }
        }
        internal static void Pocket(PocketFailEscapeEvent _)
        {
            if (!Player.List.TryFind(out var pl, x => x.Role is RoleType.Scp106)) return;
            if (!ScpKills.ContainsKey(pl.Nickname)) ScpKills.Add(pl.Nickname, 1);
            else ScpKills[pl.Nickname]++;
        }
        internal static void Damage(DamageProcessEvent ev)
        {
            if (!ev.Allowed) return;
            if (ev.FriendlyFire && !Server.FriendlyFire) return;
            if (ev.Attacker == ev.Target) return;
            if (ev.Attacker == null || ev.Target == null) return;
            if (ev.Attacker.Team == Team.SCP) return;
            if (ev.Amount < 1) return;
            int am = (int)ev.Amount;
            if (!Damages.ContainsKey(ev.Attacker.Nickname)) Damages.Add(ev.Attacker.Nickname, am);
            else Damages[ev.Attacker.Nickname] += am;
        }
        internal static void Escape(EscapeEvent ev)
        {
            if (!ev.Allowed) return;
            if (Escapes.ContainsKey(ev.Player.Nickname)) return;
            Escapes.Add(ev.Player.Nickname, Round.ElapsedTime);
        }
    }
}