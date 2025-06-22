using Loli.Addons;
using Loli.Discord;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
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
				ev.ReturnMessage = "Вы не написали про баг.";
				ev.Color = "red";
				return;
			}
			string desc = string.Join(" ", ev.Args).Trim();
			if (desc == "")
			{
				ev.ReturnMessage = "Вы не написали про баг.";
				ev.Color = "red";
				return;
			}
			if (LastBug == desc)
			{
				ev.ReturnMessage = "Вы уже написали про этот баг.";
				ev.Color = "red";
				return;
			}
			if (ev.Player.UserId == "-@steam")
			{
				ev.ReturnMessage = "Вам был закрыт доступ к отправке багов.";
				ev.Color = "red";
				return;
			}
			LastBug = desc;
			ev.ReturnMessage = "Успешно.";
			ev.Color = "green";
			string hook = "https://discord.com/api/webhooks";
			Webhook webhk = new(hook);
			List<Embed> listEmbed = new();
			EmbedAuthor reporterName = new();
			reporterName.Name = $"{ev.Player.Nickname} - {ev.Player.UserId}";
			Embed embed = new();
			embed.Title = "Новый баг";
			embed.Color = 1;
			embed.Author = reporterName;
			embed.Description = desc;
			listEmbed.Add(embed);
			webhk.Send("", Plugin.ServerName, null, false, embeds: listEmbed);
		}
		public void CheaterReport(ReportCheaterEvent ev)
		{
			if (ev.Sender.UserId == "-@steam")
			{
				ev.Sender.Broadcast(5, "<size=30%><color=red>Вам был закрыт доступ к репортам</color></size>");
				ev.Allowed = false;
				return;
            }
			string hook = "https://discord.com/api/webhooks";
			if (Plugin.ServerID == 1 || Plugin.ServerID == 2)
				hook = "https://discord.com/api/webhooks";
			else if (Plugin.RolePlay)
				hook = "https://discord.com/api/webhooks";
			string[] ignoreKeywords = { "fly", "flying", "flyed", "летает", "читер", "пин", "летчик", "пилот", "cheat", "читы", "cheater", "clip", "клип" };
			bool keywordFound = ignoreKeywords.Any(s => ev.Reason.ToLower().IndexOf(s, System.StringComparison.OrdinalIgnoreCase) >= 0);
			if (ev.Target.Id == CatHook.hook_owner?.Id)
			{
				if (keywordFound)
				{
					ev.Sender.Broadcast(5, "<size=30%><color=#fdffbb>Вы кидаете репорт на владельца <color=#0089c7>крюк<color=#9bff00>-</color>кошки</color> за полет</color>\n" +
						"<color=#f47fff>Прочитайте про <color=#0089c7>крюк<color=#9bff00>-</color>кошку</color> в консоли на <color=#ffffff>[<color=#e6ffa1>ё</color>]</color><color=lime>," +
						"</color> написав <color=#ff5e70>.</color><color=#387aff>cat_hook info</color></color></size>");
					ev.Allowed = false;
					return;
				}
			}
			if (ev.Target.UserId == ev.Sender.UserId)
			{
				ev.Sender.Broadcast(5, "<size=30%><color=#737885>Вы не можете отправить репорт на себя</color></size>");
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
				ev.Issuer.Broadcast(5, "<size=30%><color=red>Вам был закрыт доступ к репортам</color></size>");
				ev.Allowed = false;
				return;
			}
			string hook = "https://discord.com/api/webhooks";
			if (Plugin.ServerID == 1 || Plugin.ServerID == 2)
				hook = "https://discord.com/api/webhooks";
			else if (Plugin.RolePlay)
				hook = "https://discord.com/api/webhooks";
			if (ev.Target.UserId == ev.Issuer.UserId)
			{
				ev.Issuer.Broadcast(5, "<size=30%><color=#737885>Вы не можете отправить репорт на себя</color></size>");
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