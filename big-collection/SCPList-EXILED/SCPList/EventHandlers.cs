using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using Grenades;
using MEC;

namespace SCPList
{
	public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		internal void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (ev.Player.GetTeam() == Team.SCP && ev.Command.ToLower() == "scplist")
			{
				IEnumerable<ReferenceHub> SCPs = Player.GetHubs().Where(rh => rh.GetTeam() == Team.SCP);

				string response = "";

				foreach (ReferenceHub scp in SCPs)
				{
					response += $"\n{scp.GetNickname()} - {scp.characterClassManager.Classes.SafeGet(scp.GetRole()).fullName}";
				}

				ev.ReturnMessage = response;
				ev.Color = "cyan";
			}
		}

		internal void OnRoundStart()
		{
			if (!plugin.displayBroadcast) return;

			IEnumerable<ReferenceHub> SCPs = Player.GetHubs().Where(rh => rh.GetTeam() == Team.SCP);

			if (!SCPs.Any()) return;

			List<string> response = new List<string>();

			foreach (ReferenceHub scp in SCPs)
			{
				response.Add($"{scp.GetNickname()} - {scp.characterClassManager.Classes.SafeGet(scp.GetRole()).fullName}");
			}

			string msg = string.Join(" | ", response);

			foreach (ReferenceHub scp in SCPs)
			{
				scp.Broadcast(5, msg);
			}
		}
	}
}