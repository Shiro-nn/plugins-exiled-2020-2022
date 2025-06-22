using System;
using System.Collections.Generic;
using System.Linq;
using Qurre.API.Events;
using Qurre.API;
namespace ClassicCore.Addons
{
    internal static class OfflineBan
    {
        internal static void Send(SendingRAEvent ev)
        {
            ev.Allowed = false;
            ev.Prefix = "oban";
            if (!(ev.CommandSender.SenderId == "-@steam" || (ev.CommandSender.Nickname == "Dedicated Server" && ev.CommandSender.SenderId == "")
                || ev.CommandSender.SenderId == "SERVER CONSOLE"))
            {
                ev.ReplyMessage = "Вам не разрешено использовать данную команду.";
                return;
            }
            if (ev.Args.Length < 3)
            {
                ev.ReplyMessage = "oban <userid> <длительность> <причина>\nДлительность в часах";
                return;
            }
            if (!int.TryParse(ev.Args[1], out int num))
            {
                ev.ReplyMessage = "Аргумент 2 должен быть действительным временем в часах: " + ev.Args[1];
                return;
            }
            Player player = Player.Get(ev.Args[0]);
            string Reason = string.Join(" ", ev.Args.Skip(2));
            int SecondsBan = num * 60 * 60;
            long BanExpieryTime = TimeBehaviour.GetBanExpirationTime((uint)SecondsBan);
            long IssuanceTime = TimeBehaviour.CurrentTimestamp();
            if (player != null)
            {
                DateTime ExpireDate = DateTime.Now.AddHours(num);
                Map.Broadcast($"<size=30%><color=#6f6f6f><color=#ff0000>{ev.CommandSender.Nickname}</color> забанил <color=#ff0000>{player.Nickname}</color> " +
                    $"до <color=#ff0000> {ExpireDate:dd.MM.yyyy HH:mm}</color>. <color=#ff0000>Причина</color>: {Reason}</color></size>", 15);
                player.Ban(SecondsBan, Reason, ev.CommandSender.Nickname);
                ev.ReplyMessage = $"{player.Nickname} успешно забанен на {ev.Args[1]} час(а/ов), причина: {Reason}";
            }
            else
            {
                IEnumerable<string> source = ev.Args[0].Split('@');
                if (source.Count() != 2) ev.ReplyMessage = $"Кривой userID: {ev.Args[0]}";
                else if (!long.TryParse(source.First(), out _)) ev.ReplyMessage = $"Кривой userID: {source.First()}";
                else
                {
                    BanHandler.IssueBan(new BanDetails
                    {
                        Expires = BanExpieryTime,
                        Id = ev.Args[0],
                        IssuanceTime = IssuanceTime,
                        Issuer = ev.CommandSender.Nickname,
                        OriginalName = "Offline Ban",
                        Reason = Reason
                    }, BanHandler.BanType.UserId);
                    try
                    {
                        DateTime ExpireDate = DateTime.Now.AddHours(num);
                        Map.Broadcast($"<size=30%><color=#6f6f6f><color=#ff0000>{ev.CommandSender.Nickname}</color> забанил <color=#ff0000>{ev.Args[0]}</color> " +
                            $"до <color=#ff0000>{ExpireDate:dd.MM.yyyy HH:mm}</color>. <color=#ff0000>Причина</color>: {Reason}\noffline ban</color></size>", 15);
                        try { SCPDiscordLogs.Api.SendBanOrKick(Reason, ev.Args[0], ev.CommandSender.Nickname, ExpireDate.ToString("dd.MM.yyyy HH:mm")); } catch { }
                    }
                    catch { }
                    ev.ReplyMessage = $"{ev.Args[0]} успешно забанен на {ev.Args[1]} час(а/ов), причина: {Reason}";
                }
            }
        }
    }
}