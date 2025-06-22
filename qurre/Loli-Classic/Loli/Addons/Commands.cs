using UnityEngine;
using Qurre.API;
using System.Linq;
using Qurre.Events.Structs;
using Loli.DataBase.Modules;
using PlayerRoles;
using System;

namespace Loli.Addons
{
	static class Commands
	{
		static Commands()
		{
			CommandsSystem.RegisterConsole("help", Help);
			CommandsSystem.RegisterConsole("хелп", Help);
			CommandsSystem.RegisterConsole("хэлп", Help);

			CommandsSystem.RegisterConsole("kill", Suicide);
			CommandsSystem.RegisterConsole("tps", TPS);
			CommandsSystem.RegisterConsole("size", Size);
		}

		static void Help(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Reply = $"\n" +
				$".help / .хелп / .хэлп — Команда помощи" +
				$"\n\n" +
				$"Другое:" +
				$"\n" +
				$".kill — Суицид не выход" +
				$"\n" +
				$".s — Стать ученым, будучи охраной" +
				$"\n" +
				$".force — Стать другим SCP, будучи другим SCP\nНапример: .force 106" +
				$"\n" +
				$".079 — Список команд SCP 079" +
				$"\n\n" +
				$"Общение:" +
				$"\n" +
				$".chat / .чат — Текстовой чат";
			ev.Color = "white";
		}

		static void Suicide(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (ev.Player.RoleInfomation.Role is RoleTypeId.ClassD)
			{
				string tag = " NotForce";
				ev.Player.Tag += tag;
				MEC.Timing.CallDelayed(1f, () => ev.Player.Tag.Replace(tag, ""));
			}
			ev.Player.HealthInfomation.Kill("Вскрыты вены");
		}

		static void TPS(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Reply = $"TPS: {Core.TicksMinutes}";
			ev.Player.Client.SendConsole($"Альтернативный TPS: {Math.Round(1f / Time.smoothDeltaTime)}", "white");
		}

		static bool ThisAdmin(string userid)
		{
			try
			{
				if (Data.Users.TryGetValue(userid, out var data) &&
					(data.id == 1 ||
					data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.control || data.maincontrol)) return true;
				else return false;
			}
			catch { return false; }
		}

		static void Size(GameConsoleCommandEvent ev)
		{
			ev.Allowed = false;
			if (!ThisAdmin(ev.Player.UserInfomation.UserId))
			{
				ev.Reply = "Отказано в доступе";
				return;
			}
			if (ev.Args.Length < 3)
			{
				ev.Reply = "Вводите 3 аргумента <x> <y> <z>";
				return;
			}
			if (!float.TryParse(ev.Args[0], out float x) || x < 0.1)
			{
				ev.Reply = "Неправильно указана координата <x>";
				return;
			}
			if (!float.TryParse(ev.Args[1], out float y) || y < 0.1)
			{
				ev.Reply = "Неправильно указана координата <y>";
				return;
			}
			if (!float.TryParse(ev.Args[2], out float z) || z < 0.1)
			{
				ev.Reply = "Неправильно указана координата <z>";
				return;
			}
			var target = ev.Player;
			try
			{
				string name = string.Join(" ", ev.Args.Skip(3));
				if (name.Trim() != "")
				{
					var pl = name.GetPlayer();
					if (pl != null) target = pl;
				}
			}
			catch { }
			target.MovementState.Scale = new(x, y, z);
			ev.Reply = $"Успешно изменен размер у {target.UserInfomation.Nickname}";
		}
	}
}