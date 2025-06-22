using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Modules
{
	static class Waiting
	{
		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
			string str = "\n<color=#00fffb>Добро пожаловать на сервер</color> ";
			string project = "<color=#ff0000>f</color><color=#ff004c>y</color><color=#ff007b>d</color><color=#ff00a2>n</color><color=#e600ff>e</color>";
			string str2 = "<b><color=#09ff00>Приятной игры!</color></b>";
			ev.Player.Client.ShowHint($"<b>{str}{project}</b>\n{str2}".Trim(), 10);

			try { if (!Round.Ended) Core.Socket.Emit("server.addip", new object[] { Core.ServerID, ev.Player.UserInfomation.Ip }); } catch { }
		}
	}
}