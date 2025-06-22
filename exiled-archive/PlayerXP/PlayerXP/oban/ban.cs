using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerXP.oban
{
    class ban
	{
		private readonly Plugin plugin;
		public ban(Plugin plugin) => this.plugin = plugin;
		internal void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string[] array = ev.Arguments.ToArray<string>();
			ReferenceHub player = Extensions.GetPlayer(ev.CommandSender.SenderId);
			if (ev.Name == "oban")
			{
				ev.IsAllowed = false;
				try
				{
					if (array.Length > 2 && (ev.Sender.UserId == "-@steam" || (ev.Sender.Nickname == "Dedicated Server" && ev.Sender.UserId == "")))
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
							string text5 = (player != null) ? Extensions.GetUserId(player) : "Console";
							ReferenceHub player2 = Extensions.GetPlayer(text3);
							if (player2 != null)
							{
								DateTime ExpireDate = DateTime.Now.AddHours(num);
								if (ev.Sender.Nickname == "Dedicated Server")
									Map.Broadcast(5, $"<color=#00ffff>{player2.GetNickname()}</color> <color=red>забанен</color> <color=#0089c7>Discord</color>'<color=#00ffff>ом</color> <color=red>до</color> <color=#00ffff> {ExpireDate.ToString("dd.MM.yyyy HH:mm")}</color> {text4}");
								else
									Map.Broadcast(5, $"<color=#00ffff>{player2.GetNickname()}</color> <color=red>забанен</color> <color=#00ffff>{ev.Sender.Nickname}</color> <color=red>до</color> <color=#00ffff> {ExpireDate.ToString("dd.MM.yyyy HH:mm")}</color> {text4}");
								Extensions.BanPlayer(player2, num * 60, text4, text5);
								ev.ReplyMessage = string.Concat(new string[]
								{
									Extensions.GetNickname(player2),
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
										Issuer = text5,
										OriginalName = "Offline Ban",
										Reason = text4
									}, BanHandler.BanType.UserId);
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