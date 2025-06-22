using Loli.Discord;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.Addons
{
    internal class Chat
    {
        internal class Message
        {
            internal DateTime Date = DateTime.Now;
            internal string Text = "";
            internal Player Author;
            internal MessageType Type = MessageType.Public;
            internal RoleTypeId Role = RoleTypeId.None;
            internal Player PrivateTo;
            internal Vector3 Position;
            internal Message(string text, Player author, MessageType type, RoleTypeId role, Player privateTo, Vector3 pos)
            {
                Text = text;
                Author = author;
                Type = type;
                Role = role;
                PrivateTo = privateTo;
                Date = DateTime.Now;
                Position = pos;
            }
        }
        public enum MessageType
        {
            Public,
            Team,
            Ally,
            Private,
            Position,
        }
        internal static readonly List<Message> messages = new();
        internal static readonly List<string> Disables = new();

        [EventMethod(RoundEvents.Waiting)]
        internal static void Waiting() => messages.Clear();

        [EventMethod(PlayerEvents.Leave)]
        internal static void Leave(LeaveEvent ev)
        {
            while (messages.Where(x => x.Author == ev.Player).Count() != 0)
                messages.Remove(messages.Where(x => x.Author == ev.Player).First());
        }

        internal static void Console(GameConsoleCommandEvent ev)
        {
            if (ev.Name != "чат" && ev.Name != "chat") return;
            ev.Allowed = false;
            if (ev.Args.Count() == 0)
            {
                ev.Reply = $"\n" +
                    $"/ — разделитель (или)\n" +
                    $".чат / .chat — команда чата\n" +
                    $".чат публичный / .chat (public/пб/п) <Сообщение> - отправить сообщение в публичный чат\n" +
                    $".чат ближний / .chat position <Сообщение> - отправить сообщение в радиусе 5ти метров\n" +
                    $".чат командный / .chat (team/км) <Сообщение> - отправить сообщение своей команде (хаос - хаос, мог - мог, д - д)\n" +
                    $".чат союзный / .chat (ally/сз) <Сообщение> - отправить сообщение своим союзникам (хаос - хаос & д, мог - мог & ученые)\n" +
                    $".чат лс / .chat private <SteamID64, Ник (пробелы заменяйте _)> <Сообщение> - отправить сообщение определенному игроку\n" +
                    $".чат отключить / .chat disable - отключить чат\n" +
                    $".чат включить / .chat enable - включить чат\n" +
                    //$".чат мьют / .chat mute <SteamID64, Ник (пробелы заменяйте _)> <Длительность в часах> - замьютить пользователя\n" +
                    $"*Если вы не указали тип чата, то он автоматически станет ближним";
                return;
            }
            string type = ev.Args[0].ToLower();
            switch (type)
            {
                case "публичный" or "public" or "пб" or "п":
                    {
                        var content = string.Join(" ", ev.Args.Skip(1)).Trim();
                        Send(content, MessageType.Public, Vector3.zero);
                        break;
                    }
                case "ближний" or "position":
                    {
                        var content = string.Join(" ", ev.Args.Skip(1)).Trim();
                        Send(content, MessageType.Position, ev.Player.MovementState.Position);
                        break;
                    }
                case "командный" or "team" or "км":
                    {
                        var content = string.Join(" ", ev.Args.Skip(1)).Trim();
                        Send(content, MessageType.Team, Vector3.zero);
                        break;
                    }
                case "союзный" or "ally" or "сз":
                    {
                        var content = string.Join(" ", ev.Args.Skip(1)).Trim();
                        Send(content, MessageType.Ally, Vector3.zero);
                        break;
                    }
                case "приватный" or "лс" or "private":
                    {
                        var content = string.Join(" ", ev.Args.Skip(2)).Trim();

                        Player player = ev.Args[1].Replace("_", " ").GetPlayer();
                        if (player == null) ev.Reply = "Игрок не найден";
                        else Send(content, MessageType.Private, Vector3.zero, player);
                        break;
                    }
                case "отключить" or "disable":
                    {
                        if (!Disables.Contains(ev.Player.UserInfomation.UserId))
                        {
                            Disables.Add(ev.Player.UserInfomation.UserId);
                            ev.Reply = "Успешно.";
                        }
                        else ev.Reply = "У вас уже отключен чат.";
                        break;
                    }
                case "включить" or "enable":
                    {
                        if (Disables.Contains(ev.Player.UserInfomation.UserId))
                        {
                            Disables.Remove(ev.Player.UserInfomation.UserId);
                            ev.Reply = "Успешно.";
                        }
                        else ev.Reply = "У вас не отключен чат.";
                        break;
                    }
                case "очистить" or "clear":
                    {
                        if (!(DataBase.Modules.Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var _data) && _data.administration.owner))
                        {
                            ev.Reply = "Отказано в доступе";
                            return;
                        }
                        messages.Clear();
                        ev.Reply = "Успешно";
                        ev.Color = "green";
                        break;
                    }
                default:
                    {
                        var content = string.Join(" ", ev.Args).Trim();
                        Send(content, MessageType.Position, ev.Player.MovementState.Position);
                        break;
                    }
            }
            void Send(string _text, MessageType type, Vector3 pos, Player to = null)
            {
                string text = _text.Replace("<", "").Replace(">", "");
                if (string.IsNullOrEmpty(text))
                {
                    ev.Reply = "Ваше сообщение пустое";
                    ev.Color = "red";
                    return;
                }
                var check = text.ToLower();
                if (check.Contains("midnight"))
                {
                    ev.Reply = "ай, ай, ай";
                    ev.Color = "red";
                    return;
                }/*
                if (ev.Player.Muted)
                {
                    ev.ReturnMessage = "Вы замьючены";
                    ev.Color = "red";
                    return;
                }*/
                if (text.Length > 50)
                {
                    ev.Reply = "Ваше сообщение слишком длинное";
                    ev.Color = "red";
                    return;
                }
                if (text.Contains("http") && text.Split(new string[] { "http" }, StringSplitOptions.None).Select(x => x.Replace("http", "")).Where(x => x.Length > 0 &&
                (x.Contains(".") || x.Contains(",") || x.Contains("точка")) && !x.Substring(0, x.Length > 15 ? 15 : x.Length).ToLower().Contains("scpsl.store")).Count() != 0)
                {
                    ev.Reply = "Реклама запрещена";
                    ev.Color = "red";
                    return;
                }
                if (text.Contains("discord") && text.Split(new string[] { "discord" }, StringSplitOptions.None).Select(x => x.Replace("discord", "")).Where(x =>
                x.Length > 0 && x.Contains("/") && !x.Substring(0, x.Length > 12 ? 12 : x.Length).Contains("UCUBU2z")).Count() != 0)
                {
                    ev.Reply = "Реклама запрещена";
                    ev.Color = "red";
                    return;
                }
                if (messages.Where(x => x.Author == ev.Player && (x.Date.AddSeconds(5) - DateTime.Now).TotalSeconds > 0).Count() > 0)
                {
                    ev.Reply = "Сообщения можно отправлять раз в 5 секунд";
                    ev.Color = "red";
                    return;
                }

                var msg = new Message(text, ev.Player, type, ev.Player.RoleInfomation.Role, to, pos);
                messages.Add(msg);
                ev.Reply = "Успешно";
                ev.Color = "green";
                Timing.CallDelayed(60, () => { if (messages.Contains(msg)) messages.Remove(msg); });
                if (type == MessageType.Private) ev.Reply = $"Сообщение отправлено {to?.UserInfomation.Nickname}";
                string hook = "https://discord.com/api/webhooks/";
                Webhook webhk = new(hook);
                List<Embed> listEmbed = new();
                EmbedAuthor reporterName = new() { Name = $"{ev.Player.UserInfomation.Nickname} - {ev.Player.UserInfomation.UserId}" };
                Embed embed = new()
                {
                    Title = "Новое сообщение",
                    Color = 1,
                    Author = reporterName,
                    Description = text
                };
                listEmbed.Add(embed);
                webhk.Send("", Core.ServerName, null, false, embeds: listEmbed);
            }
        }
    }
}