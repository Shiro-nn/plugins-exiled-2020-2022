using System;
using System.Linq;
using CommandSystem;
using Qurre.API;

namespace AutoEvent.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ScpInfectCommand : ICommand
    {
        public string Command => "zombie";
        public string[] Aliases => new string[] { };
        public string Description => "Создать авто-ивент инфекция сцп: zombie";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.Started)
            {
                if (Player.List.Count() >= 15)
                {
                    response = $"Вы запустили ивент Заражение!.";
                    return true;
                }
                response = $"Игроков недостаточно! Игроков должно быть 15.";
                return false;
            }
            response = $"Раунд уже начался!";
            return false;
        }
    }
}
