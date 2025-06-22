using System.Text;
using MultiPlugin22.Interfaces;

namespace MultiPlugin22.Commands.Console
{
	public class Help : ICommand
	{
		private readonly MultiPlugin22 pluginInstance;

		public Help(MultiPlugin22 pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => Configs.l26;

		public string Usage => $".help/.help [{Configs.command}]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (pluginInstance.ConsoleCommands.Count == 0) return (Configs.l27, "red");

			if (args.Length == 0)
			{
				StringBuilder commands = new StringBuilder($"\n\n[{Configs.commandlist}: ({pluginInstance.ConsoleCommands.Count})]");

				foreach (ICommand command in pluginInstance.ConsoleCommands.Values)
				{
					commands.Append($"\n\n{command.Usage}\n\n{command.Description}");
				}

				return (commands.ToString(), "green");
			}
			else if (args.Length == 1)
			{
				if (!pluginInstance.ConsoleCommands.TryGetValue(args[0].Replace(".", ""), out ICommand command)) return (Configs.l28.Replace("%command%", $"{args[0]}"), "red");

				return ($"\n\n{command.Usage}\n\n{command.Description}", "green");
			}

			return (Configs.l29, "red");
		}
	}
}
