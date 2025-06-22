using System;
using System.Collections.Generic;
using MultiPlugin22.Collections.Chat;
using MultiPlugin22.Enums;
using MultiPlugin22.Extensions;

namespace MultiPlugin22.Commands.Console
{
	public class Chat
	{
		protected readonly ChatRoomType type;
		protected string color;

		protected Chat(ChatRoomType type) => this.type = type;

		protected Chat(ChatRoomType type, string color) : this(type) => this.color = color;

		protected (string message, bool isValid) CheckMessageValidity(string message, Player messageSender, ReferenceHub sender)
		{
			if (string.IsNullOrEmpty(message.Trim())) return (Configs.l30, true);
			else if (sender.IsChatMuted()) return (Configs.l33, true);
			else if (messageSender.IsFlooding(Configs.slowModeCooldown)) return (Configs.l31, true);
			else if (message.Length > Configs.maxMessageLength) return (Configs.l32.Replace("%length%", $"{Configs.maxMessageLength}"), true);

			return (message, true);
		}

		protected void SendConsoleMessage(ref string message, Player sender, IEnumerable<ReferenceHub> targets)
		{
			targets.SendConsoleMessage(message = Configs.censorBadWords ? message.Sanitize(Configs.badWords, Configs.censorBadWordsChar) : message, color);

			sender.lastMessageSentTimestamp = DateTime.Now;
		}
	}
}
