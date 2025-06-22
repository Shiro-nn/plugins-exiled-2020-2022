using System;
using System.Collections.Generic;
using Qurre.API.Events;
using Loli.Discord;
using Qurre.API;
using System.Linq;
namespace Loli.Addons
{
    static internal class CrashProtect
    {
        static internal Dictionary<string, BansCounts> BansDict = new();
        static internal void SetGroup(SendingRAEvent ev)
        {
            if (ev.Name == "setgroup" || ev.Name == "sg" || ev.Name == "sgroup")
            {
                ev.Allowed = false;
                new Webhook("https://discord.com/api/webhooks")
                    .Send("", embeds: new List<Embed>()
                    {
                        new()
                        {
                            Color = 16711680,
                            Author = new() { Name = "Использование setgroup" },
                            Footer = new() { Text = Server.Ip + ":" + Server.Port },
                            TimeStamp = DateTimeOffset.Now,
                            Description = $"Использовал: {ev.CommandSender.Nickname} | {ev.CommandSender.SenderId}"
                        }
                    });
            }
        }
        static internal void AntiBan(BanEvent ev)
        {
            Method(ev.Issuer, out bool allowed);
            ev.Allowed = allowed;
        }
        static internal void AntiKick(KickEvent ev)
        {
            Method(ev.Issuer, out bool allowed);
            ev.Allowed = allowed;
        }
        static private void Method(Player issuer, out bool allowed)
        {
            allowed = true;
            if (issuer is null) return;
            if (issuer.IsHost) return;
            if (!BansDict.TryGetValue(issuer.UserId, out BansCounts cl))
            {
                cl = new();
                BansDict.Add(issuer.UserId, cl);
            }
            cl.Add();
            if (cl.Counts > 10)
            {
                allowed = false;
                if (!DataBase.Manager.Static.Data.Users.TryGetValue(issuer.UserId, out var _data))
                {
                    SendHook($"Нарушил: {issuer.Nickname} | {issuer.UserId}");
                    return;
                }
                Plugin.Socket.Emit("database.remove.admin", new object[] { _data.id });
                SendHook($"Нарушил: {issuer.Nickname} | {issuer.UserId} | {_data.name} (<@!{_data.discord}>)");
            }
            static void SendHook(string desc)
            {
                new Webhook("https://discord.com/api/webhooks")
                    .Send("", embeds: new List<Embed>()
                    {
                        new()
                        {
                            Color = 16711680,
                            Author = new() { Name = "Попытка краша | Лимит банов" },
                            Footer = new() { Text = Server.Ip + ":" + Server.Port },
                            TimeStamp = DateTimeOffset.Now,
                            Description = desc
                        }
                    });
            }
        }

        internal class BansCounts
        {
            internal int Counts => dates.Where(x => (DateTime.Now - x).TotalMinutes < 1).Count();
            internal List<DateTime> dates = new();
            internal void Add() => dates.Add(DateTime.Now);
        }
    }
}