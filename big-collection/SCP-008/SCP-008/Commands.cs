using EXILED;
using EXILED.Extensions;

namespace SCP008
{
	public class Commands
	{
		private readonly Plugin plugin;
		public Commands(Plugin plugin) => this.plugin = plugin;

		public void OnRaCommand(ref RACommandEvent ev)
		{
			string[] args = ev.Command.Split(' ');

			switch (args[0].ToLower())
			{
				case "i":
				{
					ev.Allow = true;
					if (args.Length < 2)
					{
						ev.Sender.RAMessage("Вы должны предоставить игрока по имени.", true);
						return;
					}

					ReferenceHub player = Plugin.GetPlayer(args[1]);
					if (player == null)
					{
						ev.Sender.RAMessage($"Игрок не найден: {args[1]}", true);
						return;
					}

					if (plugin.InfectedPlayers.Contains(player))
					{
						ev.Sender.RAMessage($"Игрок: {args[1]} заражен.");
						return;
					}
					
					plugin.Functions.InfectPlayer(player);
					ev.Sender.RAMessage($"{args[1]} был заражен SCP-008.");
					return;
				}
				case "c":
				{
					ev.Allow = true;
					if (args.Length < 2)
					{
						ev.Sender.RAMessage("Вы должны предоставить игрока по имени.", true);
						return;
					}

					ReferenceHub player = Plugin.GetPlayer(args[1]);
					if (player == null)
					{
						ev.Sender.RAMessage($"Игрок не найден: {args[1]}", true);
						return;
					}

					if (!plugin.InfectedPlayers.Contains(player))
					{
						ev.Sender.RAMessage($"Игрок: {args[1]} не заражен.");
						return;
					}
					
					plugin.Functions.CurePlayer(player);
					ev.Sender.RAMessage($"{args[1]} был вылечен от SCP-008.");
					return;
				}
			}
		}
	}
}