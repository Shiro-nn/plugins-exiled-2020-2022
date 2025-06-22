using Loli.BetterHints;
using Loli.Discord;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;

namespace Loli.Modules
{
	static class Reports
	{
		[EventMethod(ServerEvents.CheaterReport)]
		static void CheaterReport(CheaterReportEvent ev)
		{
			string hook = "https://discord.com/api/webhooks/-";
			if (ev.Target.UserInfomation.UserId == ev.Issuer.UserInfomation.UserId)
			{
				ev.Issuer.Hint(new(-20, 6, "<align=left><color=#737885>Вы не можете отправить репорт на себя</color></align>", 10, false));
				ev.Allowed = false;
				return;
			}

			Webhook webhk = new(hook);

			List<Embed> listEmbed = new();


			EmbedField reporterName = new()
			{
				Name = "Репорт отправил:",
				Value = $"{ev.Issuer.UserInfomation.Nickname} - {ev.Issuer.UserInfomation.UserId}",
				Inline = true
			};

			EmbedField reportedName = new()
			{
				Name = "Зарепорчен:",
				Value = $"{ev.Target.UserInfomation.Nickname} - {ev.Target.UserInfomation.UserId}",
				Inline = true
			};

			EmbedField Reason = new()
			{
				Name = "Причина",
				Value = ev.Reason,
				Inline = true
			};

			Embed embed = new()
			{
				Title = "Новый репорт",
				Color = 1
			};
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);

			webhk.Send("Новый репорт **на читера**", Core.ServerName, null, false, embeds: listEmbed);
		}

		[EventMethod(ServerEvents.LocalReport)]
		static void LocalReport(LocalReportEvent ev)
		{
			string hook = "https://discord.com/api/webhooks/-";
			if (ev.Target.UserInfomation.UserId == ev.Issuer.UserInfomation.UserId)
			{
				ev.Issuer.Hint(new(-20, 6, "<align=left><color=#737885>Вы не можете отправить репорт на себя</color></align>", 10, false));
				ev.Allowed = false;
				return;
			}

			Webhook webhk = new(hook);

			List<Embed> listEmbed = new();


			EmbedField reporterName = new()
			{
				Name = "Репорт отправил:",
				Value = $"{ev.Issuer.UserInfomation.Nickname} - {ev.Issuer.UserInfomation.UserId}",
				Inline = true
			};

			EmbedField reportedName = new()
			{
				Name = "Зарепорчен:",
				Value = $"{ev.Target.UserInfomation.Nickname} - {ev.Target.UserInfomation.UserId}",
				Inline = true
			};

			EmbedField Reason = new()
			{
				Name = "Причина",
				Value = ev.Reason,
				Inline = true
			};

			Embed embed = new()
			{
				Title = "Новый репорт",
				Color = 1
			};
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);

			webhk.Send("Новый репорт", Core.ServerName, null, false, embeds: listEmbed);
		}
	}
}