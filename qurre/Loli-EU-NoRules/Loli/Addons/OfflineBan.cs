using System;
using System.Collections.Generic;
using System.Linq;
using Loli.Discord;
using Loli.Telegram;
using Qurre.API;
using Qurre.Events.Structs;

namespace Loli.Addons
{
    internal static class OfflineBan
    {
        static internal Dictionary<string, BansCounts> _bans = new();
        internal static void Send(RemoteAdminCommandEvent ev)
        {
            ev.Allowed = false;
            ev.Prefix = "oban";
            if (!(ev.Sender.SenderId == "-@steam" || (ev.Sender.Nickname == "Dedicated Server" && ev.Sender.SenderId == "")
                || ev.Sender.SenderId == "SERVER CONSOLE"))
            {
                ev.Reply = "Вам не разрешено использовать данную команду.";
                return;
            }
            if (ev.Args.Length < 3)
            {
                ev.Reply = "oban <userid> <длительность> <причина>\nДлительность в часах";
                return;
            }
            if (!int.TryParse(ev.Args[1], out int num))
            {
                ev.Reply = "Аргумент 2 должен быть действительным временем в часах: " + ev.Args[1];
                return;
            }
            try
            {
                if (!_bans.TryGetValue(ev.Sender.Nickname, out BansCounts cl))
                {
                    cl = new();
                    _bans.Add(ev.Sender.Nickname, cl);
                }
                if (cl.Counts > 10)
                {
                    new Webhook("https://discord.com/api/webhooks/")
                        .Send("", embeds: new List<Embed>()
                        {
                            new()
                            {
                                Color = 16711680,
                                Author = new() { Name = "Попытка краша | Превышен лимит Offline-Банов" },
                                Footer = new() { Text = Server.Ip + ":" + Server.Port },
                                TimeStamp = DateTimeOffset.Now,
                                Description = $"Нарушил: {ev.Sender.Nickname} | {ev.Sender.SenderId}"
                            }
                        });
                    TgWebhook.Send($"Попытка краша | Превышен лимит Offline-Банов\n" +
                        $"{Server.Ip}:{Server.Port}\n" +
                        $"{DateTimeOffset.Now}\n" +
                        $"Нарушил: {ev.Sender.Nickname} | {ev.Sender.SenderId}");
                    return;
                }
                cl.Add();
            }
            catch { }
            Player player = ev.Args[0].GetPlayer();
            string Reason = string.Join(" ", ev.Args.Skip(2));
            int SecondsBan = num * 60 * 60;
            long BanExpieryTime = TimeBehaviour.GetBanExpirationTime((uint)SecondsBan);
            long IssuanceTime = TimeBehaviour.CurrentTimestamp();
            if (player != null)
            {
                DateTime ExpireDate = DateTime.Now.AddHours(num);
                Map.Broadcast($"<size=70%><color=#6f6f6f><color=#ff0000>{ev.Sender.Nickname}</color> забанил <color=#ff0000>{player.UserInfomation.Nickname}</color> " +
                    $"до <color=#ff0000> {ExpireDate:dd.MM.yyyy HH:mm}</color>. <color=#ff0000>Причина</color>: {Reason}</color></size>", 15);
                player.Administrative.Ban(SecondsBan, Reason, ev.Sender.Nickname);
                ev.Reply = $"{player.UserInfomation.Nickname} успешно забанен на {ev.Args[1]} час(а/ов), причина: {Reason}";
            }
            else
            {
                IEnumerable<string> source = ev.Args[0].Split('@');
                if (source.Count() != 2)
                    ev.Reply = $"Кривой userID: {ev.Args[0]}";
                else if (!long.TryParse(source.First(), out _))
                    ev.Reply = $"Кривой userID: {source.First()}";
                else
                {
                    BanHandler.IssueBan(new BanDetails
                    {
                        Expires = BanExpieryTime,
                        Id = ev.Args[0],
                        IssuanceTime = IssuanceTime,
                        Issuer = ev.Sender.Nickname,
                        OriginalName = "Offline Ban",
                        Reason = Reason
                    }, BanHandler.BanType.UserId);
                    try
                    {
                        DateTime ExpireDate = DateTime.Now.AddHours(num);
                        Map.Broadcast($"<size=70%><color=#6f6f6f><color=#ff0000>{ev.Sender.Nickname}</color> забанил <color=#ff0000>{ev.Args[0]}</color> " +
                            $"до <color=#ff0000>{ExpireDate:dd.MM.yyyy HH:mm}</color>. <color=#ff0000>Причина</color>: {Reason}\noffline ban</color></size>", 15);
                        try { SCPDiscordLogs.Api.SendBanOrKick(Reason, ev.Args[0], ev.Sender.Nickname, ExpireDate.ToString("dd.MM.yyyy HH:mm")); } catch { }
                    }
                    catch { }
                    ev.Reply = $"{ev.Args[0]} успешно забанен на {ev.Args[1]} час(а/ов), причина: {Reason}";
                }
            }
        }
    }
}