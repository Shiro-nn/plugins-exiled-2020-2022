using Loli.BetterHints;
using Loli.Discord;
using Qurre.API.Events;
using System.Collections.Generic;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		private string LastBug = "";
		internal void BugReport(SendingConsoleEvent ev)
		{
			if (ev.Name != "bug" && ev.Name != "bag" && ev.Name != "баг") return;
			ev.Allowed = false;
			if (ev.Args.Length == 0)
			{
				ev.ReturnMessage = "Вы не написали про баг";
				ev.Color = "red";
				return;
			}
			string desc = string.Join(" ", ev.Args).Trim();
			if (desc == "")
			{
				ev.ReturnMessage = "Вы не написали про баг";
				ev.Color = "red";
				return;
			}
			if (LastBug == desc)
			{
				ev.ReturnMessage = "Вы уже написали про этот баг";
				ev.Color = "red";
				return;
			}
			if (ev.Player.UserId == "-@steam")
			{
				ev.ReturnMessage = "Вам был закрыт доступ к отправке багов";
				ev.Color = "red";
				return;
			}
			LastBug = desc;
			ev.ReturnMessage = "Успешно";
			ev.Color = "green";
			string hook = "https://discord.com/api/webhooks";
			new Webhook(hook).Send("", Plugin.ServerName, null, false, embeds: new List<Embed>()
			{
				new()
				{
					Title = "Новый баг",
					Color = 1,
					Author = new() { Name = $"{ev.Player.Nickname} - {ev.Player.UserId}" },
					Description = desc
				}
			});
		}
		public void CheaterReport(ReportCheaterEvent ev)
		{
			string hook = "https://discord.com/api/webhooks";
			if (ev.Target.UserId == ev.Sender.UserId)
			{
				ev.Sender.Hint(new(-20, 6, "<align=left><color=#737885>Вы не можете отправить репорт на себя</color></align>", 10, false));
				ev.Allowed = false;
				return;
			}

			Webhook webhk = new(hook);

			List<Embed> listEmbed = new();


			EmbedField reporterName = new();
			reporterName.Name = "Репорт отправил:";
			reporterName.Value = $"{ev.Sender.Nickname} - {ev.Sender.UserId}";
			reporterName.Inline = true;

			EmbedField reportedName = new();
			reportedName.Name = "Зарепорчен:";
			reportedName.Value = $"{ev.Target.Nickname} - {ev.Target.UserId}";
			reportedName.Inline = true;

			EmbedField Reason = new();
			Reason.Name = "Причина";
			Reason.Value = ev.Reason;
			Reason.Inline = true;

			Embed embed = new();
			embed.Title = "Новый репорт";
			embed.Color = 1;
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);

			webhk.Send("Новый репорт **на читера**", Plugin.ServerName, null, false, embeds: listEmbed);
		}
		public void LocalReport(ReportLocalEvent ev)
		{
			if (ev.Issuer.UserId == "-@steam")
			{
				ev.Issuer.Hint(new(-20, 6, "<align=left><color=red>Вам был закрыт доступ к репортам</color></align>", 10, false));
				ev.Allowed = false;
				return;
			}
			string hook = "https://discord.com/api/webhooks";
			if (ev.Target.UserId == ev.Issuer.UserId)
			{
				ev.Issuer.Hint(new(-20, 6, "<align=left><color=#737885>Вы не можете отправить репорт на себя</color></align>", 10, false));
				ev.Allowed = false;
				return;
			}

			Webhook webhk = new(hook);

			List<Embed> listEmbed = new();


			EmbedField reporterName = new();
			reporterName.Name = "Репорт отправил:";
			reporterName.Value = $"{ev.Issuer.Nickname} - {ev.Issuer.UserId}";
			reporterName.Inline = true;

			EmbedField reportedName = new();
			reportedName.Name = "Зарепорчен:";
			reportedName.Value = $"{ev.Target.Nickname} - {ev.Target.UserId}";
			reportedName.Inline = true;

			EmbedField Reason = new();
			Reason.Name = "Причина";
			Reason.Value = ev.Reason;
			Reason.Inline = true;

			Embed embed = new();
			embed.Title = "Новый репорт";
			embed.Color = 1;
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);

			webhk.Send("Новый репорт", Plugin.ServerName, null, false, embeds: listEmbed);
		}
	}
}