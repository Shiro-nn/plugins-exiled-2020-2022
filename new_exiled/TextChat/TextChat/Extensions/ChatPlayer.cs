using EXILED.Extensions;
using System;
using System.Collections.Generic;
using static MultiPlugin22.Database;

namespace MultiPlugin22.Extensions
{
	public static class ChatPlayer
	{
		public static void SendConsoleMessage(this ReferenceHub player, string message, string color)
		{
			((CharacterClassManager)player.characterClassManager).TargetConsolePrint(((Mirror.NetworkBehaviour)player.scp079PlayerScript).connectionToClient, message, color);
		}

		public static void SendConsoleMessage(
		  this IEnumerable<ReferenceHub> targets,
		  string message,
		  string color)
		{
			using (IEnumerator<ReferenceHub> enumerator = targets.GetEnumerator())
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ReferenceHub current = enumerator.Current;
					if ((UnityEngine.Object)current != (UnityEngine.Object)null)
						current.SendConsoleMessage(message, color);
				}
			}
		}

		public static string GetColor(this ReferenceHub player) => player.GetTeam().GetColor();

		public static string GetColor(this Team team)
		{
			switch (team)
			{
				case Team.SCP:
					return "#ff0000";
				case Team.MTF:
					return "#006dff";
				case Team.CHI:
					return "#006dff";
				case Team.RSC:
					return "#fdffbb";
				case Team.CDP:
					return "#ff8b00";
				case Team.TUT:
					return "#15ff00";
				case Team.RIP:
				default:
					return "#fdfdfd";
			}
		}

		public static string GetAuthentication(this ReferenceHub player) => player.GetUserId().Split('@')[1];

		public static string GetRawUserId(this ReferenceHub player) => player.GetUserId().Split('@')[0];

		public static bool IsChatMuted(this ReferenceHub player)
		{
			return LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.GetRawUserId() && mute.Expire > DateTime.Now);
		}

		public static List<Collections.Chat.Player> GetChatPlayers(this IEnumerable<ReferenceHub> players, Dictionary<ReferenceHub, Collections.Chat.Player> chatPlayers)
		{
			List<Collections.Chat.Player> chatPlayersList = new List<Collections.Chat.Player>();

			foreach (ReferenceHub player in players)
			{
				if (player != null && chatPlayers.TryGetValue(player, out Collections.Chat.Player chatPlayer))
				{
					chatPlayersList.Add(chatPlayer);
				}
			}

			return chatPlayersList;
		}
	}
}
