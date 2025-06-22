using Loli.Discord;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
namespace Loli.Addons
{
    internal class BetterAntiCheat
    {
        [Serializable]
        public class Positions
        {
            public Vector3 Pos = new();
            public RoleType Role = RoleType.None;
        }
        internal Dictionary<Player, DateTime> Shoots = new();
        internal static Dictionary<Player, Positions> PosCache = new();
        internal static int Mask { get; private set; } = 0;
        internal void Refresh()
        {
            Shoots.Clear();
            PosCache.Clear();
            Mask = LayerMask.NameToLayer("Door");
        }
        internal void DisableNoclip(RoleChangeEvent ev)
        {
            if (PosCache.ContainsKey(ev.Player)) PosCache.Remove(ev.Player);
        }
        internal void DisableNoclip(SpawnEvent ev)
        {
            if (PosCache.ContainsKey(ev.Player)) PosCache.Remove(ev.Player);
        }
        internal void DisableNoclip(DeadEvent ev)
        {
            if (PosCache.ContainsKey(ev.Target)) PosCache.Remove(ev.Target);
        }
        internal void DisableNoclip(SyncDataEvent ev)
        {
            var pl = ev.Player;
            if (pl.Role == RoleType.Spectator) return;
            if (pl.Noclip) return;
            if (!PosCache.TryGetValue(pl, out var data))
            {
                PosCache.Add(pl, new Positions() { Pos = pl.Position, Role = pl.Role });
                return;
            }
            int _mask = pl.Role == RoleType.Scp106 ? Mask : pl.Movement.CollidableSurfaces;
            var cfs = GetCf();
            Vector3 upcf = Vector3.up * (pl.Scale.y * cfs.Up);
            Vector3 downcf = Vector3.down * (pl.Scale.y * cfs.Down);
            Vector3 bokcf = Vector3.left * (pl.Scale.x * cfs.Bok);
            bool mb = !(pl.Role == RoleType.Scp106 && pl.Scp106Controller.UsingPortal);
            if (mb && pl.Role == data.Role && Vector3.Distance(pl.Position, data.Pos) < (pl.Room?.Type == RoomType.Lcz914 ? 9 : 24) &&
                (Physics.Linecast(data.Pos, pl.Position, _mask) ||
                Physics.Linecast(data.Pos + upcf, pl.Position + upcf, _mask) || Physics.Linecast(data.Pos + downcf, pl.Position + downcf, _mask) ||
                Physics.Linecast(data.Pos + bokcf, pl.Position + bokcf, _mask) || Physics.Linecast(data.Pos - bokcf, pl.Position - bokcf, _mask)))
            {
                pl.Position = data.Pos;
                /*pl.Kill("Казнен анти-читом");
                Extensions.RunThread(() =>
                {
                    string hook = "https://discord.com/api/webhooks/";
                    Webhook webhk = new(hook);
                    List<Embed> listEmbed = new();
                    Embed embed = new();
                    embed.Title = "Обнаружен NoClip";
                    embed.Color = 1;
                    embed.Description = $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Ip})";
                    embed.TimeStamp = DateTimeOffset.Now;
                    listEmbed.Add(embed);
                    webhk.Send("Лишь теоритически, возможно, просто совпадение", Plugin.ServerName, null, false, embeds: listEmbed);
                });*/
            }
            /*else if (mb && !pl.ReferenceHub.fpc.isJumping && !Physics.Linecast(pl.Position + downcf, pl.Position + downcf + Vector3.down + downcf, _mask))
            {
                pl.Position += Vector3.down;
                data.Pos = pl.Position;
                data.Role = pl.Role;
            }*/
            else
            {
                data.Pos = pl.Position;
                data.Role = pl.Role;
            }
            DNCF GetCf()
            {
                if(pl.Role == RoleType.Scp106)
                {
                    return new() { Up = 1.2f, Down = 1.2f, Bok = 0.4f };
                }
                if (pl.Role == RoleType.Scp173)
                {
                    return new() { Up = 1.3f, Down = 0, Bok = 0.5f };
                }
                if (pl.Role == RoleType.Scp93953)
                {
                    return new() { Up = 0.9f, Down = 1.2f, Bok = 0.4f };
                }
                if (pl.Role == RoleType.Scp93989)
                {
                    return new() { Up = 0.7f, Down = 1.2f, Bok = 0.5f };
                }
                return new() { Up = 1.1f, Down = 1.2f, Bok = 0.3f };
            }
        }
        private class DNCF
        {
            internal float Up = 0;
            internal float Down = 0;
            internal float Bok = 0;
        }
        internal void DisableNoclip(DiesEvent ev)
        {
            if (!ev.Allowed) return;
            if (ev.PrimitiveType != DamageTypesPrimitive.ScpDamage && ev.PrimitiveType != DamageTypesPrimitive.Firearm) return;
            var pl = ev.Killer;
            if (pl.Role == RoleType.Spectator) return;
            if (ev.Target.Role == RoleType.Spectator) return;
            if (!PosCache.TryGetValue(pl, out var data))
            {
                var _data = new Positions() { Pos = pl.Position, Role = pl.Role };
                PosCache.Add(pl, _data);
                data = _data;
            }
            if (pl.Role == data.Role && Physics.Linecast(data.Pos, ev.Target.Position, pl.Movement.CollidableSurfaces))
            {
                ev.Allowed = false;
                /*pl.Kill("Казнен анти-читом");
                Extensions.RunThread(() =>
                {
                    string hook = "https://discord.com/api/webhooks/";
                    Webhook webhk = new(hook);
                    List<Embed> listEmbed = new();
                    Embed embed = new();
                    embed.Title = "Обнаружен NoClip";
                    embed.Color = 1;
                    embed.Description = $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Ip})";
                    embed.TimeStamp = DateTimeOffset.Now;
                    listEmbed.Add(embed);
                    webhk.Send("Лишь теоритически, возможно, просто совпадение", Plugin.ServerName, null, false, embeds: listEmbed);
                });*/
            }
        }
        internal Dictionary<Player, List<AntiWhTimer>> AntiWhRules = new();
        internal class AntiWhTimer
        {
            internal Player Target { get; set; }
            internal int Count { get; set; }
        }
        internal void AntiWallHack(TransmitPlayerDataEvent ev) // надо маски подправить
        {/*
            if (ev.Invisible) return;
            if (ev.Player.Role == RoleType.Scp079) return;
            if (ev.Player.Role == RoleType.Scp93953 || ev.Player.Role == RoleType.Scp93989 || ev.Player.GetEffect(EffectType.Visuals939).IsEnabled) return;
            if (!AntiWhRules.ContainsKey(ev.Player)) AntiWhRules.Add(ev.Player, new());
            var physic = Physics.Linecast(ev.Player.Position, ev.PlayerToShow.Position, ev.PlayerToShow.Movement.CollidableSurfaces);
            if (Vector3.Distance(ev.Player.Position, ev.PlayerToShow.Position) > 10 && physic ||
                (AntiWhRules.TryGetValue(ev.Player, out var _checker) && SafeGetFromList(_checker, ev.PlayerToShow, out var _check) && GetOrUpdate(_check, physic)))
            {
                ev.Invisible = true;
            }
            bool SafeGetFromList(List<AntiWhTimer> first, Player target, out AntiWhTimer ret)
            {
                var tl = first.Where(x => x.Target == target);
                if (tl.Count() == 0)
                {
                    ret = new AntiWhTimer()
                    {
                        Target = target,
                        Count = 0,
                    };
                    first.Add(ret);
                    return true;
                }
                ret = tl.First();
                return true;
            }
            bool GetOrUpdate(AntiWhTimer mod, bool physic)
            {
                if (!physic)
                {
                    mod.Count = 0;
                    return false;
                }
                if(mod.Count > 5)
                {
                    return true;
                }
                else
                {
                    mod.Count++;
                    return false;
                }
            }
            /*
            else if (ev.Player.Room != ev.PlayerToShow.Room)
            {
                if (ev.Player.Room == null)
                {
                    ev.Invisible = true;
                    return;
                }
                if (ev.PlayerToShow.Room == null) return;
                if (ev.PlayerToShow.Room.Zone == ZoneType.Office) return;
                if (ev.PlayerToShow.Room.Zone == ZoneType.Surface) return;
                var doorsf = ev.Player.Room.Doors.Where(x => Openned(x) && ev.PlayerToShow.Room.Doors.Where(y => x == y).Count() > 0);
                if (doorsf.Count() != 0) return;
                var doors = Map.Rooms.Where(x => x.Doors.Where(y => Openned(y) && ev.Player.Room.Doors.Where(z => z == y).Count() > 0).Count() > 0 &&
                x.Doors.Where(y => Openned(y) && ev.PlayerToShow.Room.Doors.Where(z => z == y).Count() > 0).Count() > 0);
                if (doors.Count() == 0) ev.Invisible = true;
                static bool Openned(Door door)
                {
                    try
                    {
                        return door.Open;
                    }
                    catch { return false; }
                }
            }*/
        }
        internal readonly List<string> AlreadyAimSended = new();
        internal void AntiAimBot(DamageProcessEvent ev) // работало плохо
        {/*
            if (ev.PrimitiveType != DamageTypesPrimitive.Firearm) return;
            if (ev.Attacker == ev.Target) return;
            if (Physics.Raycast(ev.Attacker.CameraTransform.transform.position, ev.Attacker.CameraTransform.transform.forward, out RaycastHit hit, 30f))
            {
                if (hit.transform == null)
                {
                    ev.Allowed = false;
                    SendReport(hit.transform);
                    return;
                }
                if (hit.transform.name == "default")
                {
                    ev.Allowed = false;
                    return;
                }
                if (ev.Target.Transform != hit.transform)
                {
                    if (hit.transform.IsChildOf(ev.Target.Transform)) return;
                    if (hit.transform.name.Trim() == "Player") return;
                    ev.Allowed = false;
                    SendReport(hit.transform);
                    return;
                }
            }
            void SendReport(Transform target)
            {
                if (AlreadyAimSended.Contains(ev.Attacker.UserId)) return;
                AlreadyAimSended.Add(ev.Attacker.UserId);
                Extensions.RunThread(() =>
                {
                    string hook = "https://discord.com/api/webhooks/";
                    Webhook webhk = new(hook);
                    List<Embed> listEmbed = new();
                    Embed embed = new();
                    embed.Title = "Обнаружен AimBot";
                    embed.Color = 1;
                    embed.Description = $"Подозреваемый: {ev.Attacker.Nickname} - {ev.Attacker.UserId} ({ev.Attacker.Role}) - {ev.Attacker.Ip}\n" +
                    $"Жертва: {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.Role})\nTransform: {target.name}";
                    embed.TimeStamp = DateTimeOffset.Now;
                    listEmbed.Add(embed);
                    webhk.Send("Лишь теоритически, возможно, просто совпадение", Plugin.ServerName, null, false, embeds: listEmbed);
                });
            }*/
        }
        internal void AntiSpamShoot(ShootingEvent ev)
        {/*
            if (!Shoots.TryGetValue(ev.Shooter, out var LastShoot))
            {
                Shoots.Add(ev.Shooter, DateTime.Now);
                return;
            }
            Shoots[ev.Shooter] = DateTime.Now;
            if ((DateTime.Now - LastShoot).TotalMilliseconds < GetMinimum()) ev.Allowed = false;
            double GetMinimum()
            {
                double minimum = 200;
                try
                {
                    if (ev.Shooter.ItemInfoInHand.id == ItemType.GunCOM15)
                    {
                        minimum = 168;
                    }
                    else if (ev.Shooter.ItemInfoInHand.id == ItemType.GunE11SR)
                    {
                        if (ev.Shooter.ItemInfoInHand.modBarrel == 0 || ev.Shooter.ItemInfoInHand.modBarrel == 1 || ev.Shooter.ItemInfoInHand.modBarrel == 2) minimum = 133;
                        else if (ev.Shooter.ItemInfoInHand.modBarrel == 3) minimum = 166;
                        else if (ev.Shooter.ItemInfoInHand.modBarrel == 4) minimum = 100;
                    }
                    else if (ev.Shooter.ItemInfoInHand.id == ItemType.GunLogicer || ev.Shooter.ItemInfoInHand.id == ItemType.GunMP7 || ev.Shooter.ItemInfoInHand.id == ItemType.GunProject90)
                    {
                        minimum = 100;
                    }
                    else if (ev.Shooter.ItemInfoInHand.id == ItemType.GunUSP)
                    {
                        if (ev.Shooter.ItemInfoInHand.modBarrel == 2) minimum = 400;
                        else minimum = 200;
                    }
                }
                catch { }
                return minimum;
            }*/
        }
        internal void AntiDamageCheat(DamageProcessEvent ev)
        {/*
            if (!ev.Allowed || ev.Amount == 0) return;
            if (ev.Amount > GetMaximum() && (ev.DamageType == DamageTypes.E11StandardRifle || ev.DamageType == DamageTypes.Logicer || 
                ev.DamageType == DamageTypes.Com15 || ev.DamageType == DamageTypes.Mp7 || ev.DamageType == DamageTypes.P90 || ev.DamageType == DamageTypes.Usp)) ev.Amount = 0;
            float GetMaximum()
            {
                float max = 100;
                try
                {
                    if (ev.Target.Role == RoleType.Scp106)
                    {
                        if (ev.DamageType == DamageTypes.E11StandardRifle) max = 2.3f;
                        else if (ev.DamageType == DamageTypes.Logicer) max = 2;
                        else if (ev.DamageType == DamageTypes.Com15) max = 2.2f;
                        else if (ev.DamageType == DamageTypes.Mp7) max = 1.4f;
                        else if (ev.DamageType == DamageTypes.P90) max = 1.8f;
                        else if (ev.DamageType == DamageTypes.Usp) max = 3.2f;
                    }
                    else if (ev.Target.Team == Team.SCP && ev.Target.Role != RoleType.Scp0492)
                    {
                        if (ev.DamageType == DamageTypes.Com15) max = 20;
                        else if (ev.DamageType == DamageTypes.E11StandardRifle) max = 23;
                        else if (ev.DamageType == DamageTypes.Logicer) max = 20;
                        else if (ev.DamageType == DamageTypes.Mp7) max = 13.3f;
                        else if (ev.DamageType == DamageTypes.P90) max = 18;
                        else if (ev.DamageType == DamageTypes.Usp) max = 32;
                    }
                    else
                    {
                        if (ev.DamageType == DamageTypes.Com15) max = 86;
                        else if (ev.DamageType == DamageTypes.E11StandardRifle) max = 91;
                        else if (ev.DamageType == DamageTypes.Logicer) max = 80;
                        else if (ev.DamageType == DamageTypes.Mp7) max = 53;
                        else if (ev.DamageType == DamageTypes.P90) max = 71;
                        else if (ev.DamageType == DamageTypes.Usp) max = 128;
                    }
                    if (ev.Attacker.Tag.Contains("Shiper") && ev.DamageType == DamageTypes.E11StandardRifle &&
                        (ev.Attacker.ItemInfoInHand.modSight == 3 || ev.Attacker.ItemInfoInHand.modSight == 4) &&
                        (ev.Attacker.ItemInfoInHand.modBarrel == 3 || ev.Attacker.ItemInfoInHand.modBarrel == 1) &&
                        (ev.Attacker.ItemInfoInHand.modOther == 3 || ev.Attacker.ItemInfoInHand.modOther == 1))
                        max *= 1.25f;
                }
                catch { }
                return max;
            }*/
        }
    }
}