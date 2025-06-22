using System.Linq;
using Loli.DataBase;
using Loli.DataBase.Modules;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using UnityEngine;

namespace Loli.Addons
{
	static class Commands
	{
        [EventMethod(RoundEvents.Waiting)]
        static void NullCall() { }
        
		static Commands()
		{
			CommandsSystem.RegisterConsole("help", Help);
			CommandsSystem.RegisterConsole("хелп", Help);
			CommandsSystem.RegisterConsole("хэлп", Help);

			CommandsSystem.RegisterConsole("kys", Suicide);
            CommandsSystem.RegisterConsole("kill", Suicide);
			CommandsSystem.RegisterConsole("tps", Tps);
            
            CommandsSystem.RegisterRemoteAdmin("list", List);
            CommandsSystem.RegisterRemoteAdmin("stafflist", StaffList);
		}

        static void StaffList(RemoteAdminCommandEvent ev)
        {
            ev.Allowed = false;
            string names = string.Empty;
            
            foreach (Player player in Player.List)
            {
                if (!Data.Users.TryGetValue(player.UserInformation.UserId, out UserData main)) continue;
                
                string role = string.Empty;
                
                if (main.trainee) role = "Стажер";
                else if (main.helper) role = "Хелпер";
                else if (main.mainhelper) role = "Главный Хелпер";
                else if (main.admin) role = "Админ";
                else if (main.mainadmin) role = "Главный Админ";
                else if (main.control) role = "Контроль ???";
                else if (main.maincontrol) role = "Контроль Администрации";
                
                if (!string.IsNullOrEmpty(role))
                    names += $"{player.UserInformation.Nickname} - {role}\n";
            }

            ev.Reply = $"{Player.List.Count()}/{Core.MaxPlayers}\n";
            ev.Reply += !string.IsNullOrEmpty(names) ? names : "Нет администрации онлайн.";
        }

        static void List(RemoteAdminCommandEvent ev)
        {
            ev.Allowed = false;
            string message = string.Empty;

            foreach (Player player in Player.List)
            {
                message += $"{player.UserInformation.Nickname} - {player.UserInformation.UserId} " +
                           $"({player.UserInformation.Id}) [{player.RoleInformation.Role}]";
                
                if (player.UserInformation.DoNotTrack)
                    message += " {DNT}";
                
                message += "\n";
            }

            ev.Reply = $"{Player.List.Count()}/{Core.MaxPlayers}\n";
            ev.Reply += !string.IsNullOrEmpty(message) ? message : "Нет игроков онлайн.";
        }

		static void Help(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Reply = "\n";
			ev.Color = "white";

			Color mainA = new Color32(255, 117, 117, 255);
            
			ev.Reply += ColorText("─── ⋆⋅ ✝ ⋅⋆ ─── / Команды сервера \\ ─── ⋆⋅ ✝ ⋅⋆ ───", mainA);
			ev.Reply += "\n\n";


			Color helpA = new Color32(255, 117, 159, 255);
            
			ev.Reply += ColorText("       ╭─────────────────────────.★..─╮", helpA) + '\n';
			ev.Reply += ColorText("       | ⋮♯   Команда помощи: .help         |", helpA) + '\n';
			ev.Reply += ColorText("       | ☆   Выводит доступные команды     |", helpA) + '\n';
			ev.Reply += ColorText("       | ☣   Доступные алиасы:             |", helpA) + '\n';
			ev.Reply += ColorText("       |    ⊱ .хелп ⊰    ⊱ .хэлп ⊰          |", helpA) + '\n';
			ev.Reply += ColorText("       ╰─..★.─────────────────────────╯", helpA) + '\n';
			ev.Reply += "\n\n";


			Color otherA = new Color32(255, 241, 117, 255);

			ev.Reply += ColorText("      ╭─────────── · · ୨୧ · · ───────────╮", otherA) + '\n';
			ev.Reply += ColorText("      | ⋮♯   Команда связи: .bug             |", otherA) + '\n';
			ev.Reply += ColorText("      | ☆   Позволяет сообщить тех. отделу  |", otherA) + '\n';
			ev.Reply += ColorText("      |      о найденном вами баге.          |", otherA) + '\n';
			ev.Reply += ColorText("      |      Обычные администраторы не       |", otherA) + '\n';
			ev.Reply += ColorText("      |      видят отправленные баги, поэтому|", otherA) + '\n';
			ev.Reply += ColorText("      |      вытащить из текстур не смогут   |", otherA) + '\n';
			ev.Reply += ColorText("      | ♦️    Аргументы:  『 сообщение 』      |", otherA) + '\n';
			ev.Reply += ColorText("      | ➺   Пример:  「 .баг Я дед инсайд 」 |", otherA) + '\n';
			ev.Reply += ColorText("      | ☣   Доступные алиасы:               |", otherA) + '\n';
			ev.Reply += ColorText("      |                ⊱ .баг ⊰              |", otherA) + '\n';
			ev.Reply += ColorText("      ╰─────────── · · ୨୧ · · ───────────╯", otherA) + '\n';
			ev.Reply += "\n";

			ev.Reply += ColorText("      ╭─────────── · · ୨୧ · · ───────────╮", otherA) + '\n';
			ev.Reply += ColorText("      | ⋮♯   Команда интересностей: .tps     |", otherA) + '\n';
			ev.Reply += ColorText("      | ☆   Позволяет посмотреть TPS сервера|", otherA) + '\n';
			ev.Reply += ColorText("      ╰─────────── · · ୨୧ · · ───────────╯", otherA) + '\n';
			ev.Reply += "\n";

			ev.Reply += ColorText("      ╭──────── · · ☠ · · ────────╮", otherA) + '\n';
			ev.Reply += ColorText("      | ⋮♯   Команда ####: .kys       |", otherA) + '\n';
			ev.Reply += ColorText("      | ☆   Покиньте этот бренный    |", otherA) + '\n';
			ev.Reply += ColorText("      |      мир быстро и без мучений |", otherA) + '\n';
			ev.Reply += ColorText("      | ☣   Доступные алиасы:        |", otherA) + '\n';
			ev.Reply += ColorText("      |           ⊱ .kill ⊰           |", otherA) + '\n';
			ev.Reply += ColorText("      ╰──────── · · ☠ · · ────────╯", otherA) + '\n';
			ev.Reply += "\n";


			ev.Reply += ColorText("─── ⋆⋅ ✝ ⋅⋆ ─── / Команды сервера \\ ─── ⋆⋅ ✝ ⋅⋆ ───", mainA);
            return;
            // ✧ ❤️ ➜ → ➤ » ➥ ➺ ➛ ➯ ♢ ♦️ ✦ ◊ ▶ ☣


			static string ColorText(string original, Color color)
			{
				return $"<color={color.ToHex()}>{original}</color>";
			}
		}


		static void Suicide(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			var role = ev.Player.RoleInformation.Role;

			switch (role)
            {
                case RoleTypeId.Overwatch or RoleTypeId.Filmmaker or RoleTypeId.Spectator:
                    return;
                case RoleTypeId.ClassD:
                {
                    const string tag = " NotForce";
                    ev.Player.Tag += tag;
                    Timing.CallDelayed(1f, () => ev.Player.Tag = ev.Player.Tag.Replace(tag, ""));
                    break;
                }
            }

            ev.Player.HealthInformation.Kill("Вскрыты вены");
		}

		static void Tps(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Reply = $"TPS: {Core.TicksMinutes}";
			ev.Player.Client.SendConsole($"Альтернативный TPS: {Server.Tps}", "white");
		}
	}
}