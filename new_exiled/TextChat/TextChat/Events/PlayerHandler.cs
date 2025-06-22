using EXILED;
using EXILED.Extensions;
using System;
using System.Linq;
using MultiPlugin22.Extensions;
using MultiPlugin22.Interfaces;
using static MultiPlugin22.Database;
using Log = EXILED.Log;
namespace MultiPlugin22.Events
{
	public class PlayerHandler
	{
		private readonly MultiPlugin22 pluginInstance;

		public PlayerHandler(MultiPlugin22 pluginInstance) => this.pluginInstance = pluginInstance;

		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (ev.Player == null) return;

			if (ev.Player.gameObject == PlayerManager.localPlayer)
			{
				ev.ReturnMessage = Configs.l1;
				ev.Color = "red";

				return;
			}

			(string commandName, string[] commandArguments) = ev.Command.ExtractCommand();

			if (!pluginInstance.ConsoleCommands.TryGetValue(commandName, out ICommand command)) return;

			try
			{
				(string response, string color) = command.OnCall(ev.Player, commandArguments);

				ev.ReturnMessage = response;
				ev.Color = color;
			}
			catch (Exception exception)
			{
				Log.Error($"{commandName} command error: {exception}");
				ev.ReturnMessage = Configs.l2;
				ev.Color = "red";
			}
		}

		public void OnRemoteAdminCommand(ref RACommandEvent ev)
		{
			(string commandName, string[] commandArguments) = ev.Command.ExtractCommand();

			if (!pluginInstance.RemoteAdminCommands.TryGetValue(commandName, out ICommand command)) return;

			try
			{
				(string response, string color) = command.OnCall(Player.GetPlayer(ev.Sender.SenderId), commandArguments);
				ev.Sender.RAMessage($"<color={color}>{response}</color>", color == "green");
				ev.Allow = false;
			}
			catch (Exception exception)
			{
				Log.Error($"{commandName} command error: {exception}");
				ev.Sender.RAMessage(Configs.l2, true);
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			ChatPlayers.Add(ev.Player, new Collections.Chat.Player()
			{
				Id = ev.Player.GetRawUserId(),
				Authentication = ev.Player.GetAuthentication(),
				Name = ev.Player.GetNickname()
			});
			((CharacterClassManager)ev.Player.characterClassManager).TargetConsolePrint(((Mirror.NetworkBehaviour)ev.Player.scp079PlayerScript).connectionToClient, Configs.l4, "green");
			ev.Player.Broadcast(10, Configs.l3, true);
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			Player.GetHubs().Where(player => player != ev.Player).SendConsoleMessage(Configs.l5.Replace("%player%", $"{ev.Player.GetNickname()}"), "red");

			ChatPlayers.Remove(ev.Player);
		}
	}
}