using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MongoDB.logs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace MongoDB.oban
{
    class ban
	{
		private readonly Plugin plugin;
		public ban(Plugin plugin) => this.plugin = plugin;
		internal void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string[] array = ev.Arguments.ToArray<string>();
			if (ev.Name == "oban")
			{
				ev.IsAllowed = false;
				try
				{
					if (array.Length > 2 && (ev.Sender.UserId == "-@steam" || (ev.Sender.Nickname == "Dedicated Server" && ev.Sender.UserId == "") || ev.CommandSender.SenderId == "SERVER CONSOLE"))
					{
						string text3 = array[0];
						int num;
						if (!int.TryParse(array[1], NumberStyles.Float, new CultureInfo("en"), out num))
						{
							ev.ReplyMessage = "oban#Аргумент 2 должен быть действительным временем в часах: " + array[1];
						}
						else
						{
							string text4 = string.Join(" ", array.Skip(2));
							long banExpieryTime = TimeBehaviour.GetBanExpirationTime((uint)(num * 60));
							long issuanceTime = TimeBehaviour.CurrentTimestamp();
							Player player2 = Player.Get(text3);
							if (player2 != null)
							{
								DateTime ExpireDate = DateTime.Now.AddHours(num);
								Map.Broadcast(5, $"<color=#00ffff>{player2.Nickname}</color> <color=red>забанен</color> <color=#00ffff>{ev.CommandSender.Nickname}</color> <color=red>до</color> <color=#00ffff> {ExpireDate.ToString("dd.MM.yyyy HH:mm")}</color> {text4}");
								Extensions.BanPlayer(player2.ReferenceHub, num * 60, text4, ev.CommandSender.Nickname);
								string banner = ev.CommandSender.Nickname;
                                try
								{
									send.sendban(text4, $"{player2.Nickname}({player2.UserId})", banner, ExpireDate.ToString("dd.MM.yyyy HH:mm"));
								}
                                catch { }
								ev.ReplyMessage = string.Concat(new string[]
								{
									player2.Nickname,
									" успешно забанен на ",
									array[1],
									" час(а/ов), причина: ",
									text4
								});
							}
							else
							{
								IEnumerable<string> source = text3.Split(new char[]
								{
									'@'
								});
								long num2;
								if (source.Count<string>() != 2)
								{
									ev.ReplyMessage = "oban#Кривой userID: " + text3;
								}
								else if (!long.TryParse(source.First<string>(), NumberStyles.Integer, new CultureInfo("en"), out num2))
								{
									ev.ReplyMessage = "oban#Кривой userID: " + source.First<string>();
								}
								else
								{
									BanHandler.IssueBan(new BanDetails
									{
										Expires = banExpieryTime,
										Id = text3,
										IssuanceTime = issuanceTime,
										Issuer = ev.CommandSender.Nickname,
										OriginalName = "Offline Ban",
										Reason = text4
									}, BanHandler.BanType.UserId);
									try
									{
										string banner = $"<color=#00ffff>{ev.CommandSender.Nickname}</color>";
										DateTime ExpireDate = DateTime.Now.AddHours(num);
										Map.Broadcast(5, $"<color=#00ffff>{text3}</color> <color=red>забанен</color> {banner} <color=red>до</color> <color=#00ffff> {ExpireDate.ToString("dd.MM.yyyy HH:mm")}</color> {text4}\n<color=#fdffbb>offline</color> <color=red>BAN</color>");
										try
										{
											send.sendban(text4, text3, ev.CommandSender.Nickname, ExpireDate.ToString("dd.MM.yyyy HH:mm"));
										}
										catch { }
									}
									catch { }
									ev.ReplyMessage = string.Concat(new string[]
									{
										array[0],
										" успешно забанен на ",
										array[1],
										" час(а/ов), причина: ",
										text4
									});
								}
							}
						}
					}
					else
					{
						ev.ReplyMessage = "oban#oban <userid> <длительность> <причина>\nДлительность в часах";
					}
				}
				catch (Exception ex)
				{
					ev.ReplyMessage = ex.Message;
				}
			}
		}
	}
}