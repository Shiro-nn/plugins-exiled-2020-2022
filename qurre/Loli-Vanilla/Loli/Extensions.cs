using Loli.Webhooks;
using Qurre.API;
using System;
using System.Collections.Generic;
using System.Linq;
using Loli.DataBase.Modules;

namespace Loli
{
    static class Extensions
    {
        static internal void RestartCrush(string reason)
        {
            Server.Restart();

            new Dishook("https://discord.com/api/webhooks/")
                .Send("", Core.ServerName, embeds: new List<Embed>()
                {
                    new()
                    {
                        Color = 16711680,
                        Author = new() { Name = "Краш сервера" },
                        Footer = new() { Text = Server.Ip + ":" + Server.Port },
                        TimeStamp = DateTimeOffset.Now,
                        Description = reason
                    }
                });
        }

        static internal bool ItsAdmin(this Player pl, bool owner = true)
        {
            return Data.Users.TryGetValue(pl.UserInformation.UserId, out var data) &&
                   (data.trainee || data.helper || data.mainhelper ||
                    data.admin || data.mainadmin || data.control ||
                    data.maincontrol || (owner && data.id == 1));
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int num)
        {
            T[] enumerable = [..source];
            return enumerable.Skip(Math.Max(0, enumerable.Length - num));
        }
    }
}