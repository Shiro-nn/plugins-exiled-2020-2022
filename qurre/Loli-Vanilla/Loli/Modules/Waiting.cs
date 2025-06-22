using MEC;
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
            if (ev.Player.UserInformation.Id != Server.Host.UserInformation.Id)
                Timing.CallDelayed(5f, () => Core.Socket.Emit("server.join", [ev.Player.UserInformation.UserId, ev.Player.UserInformation.Ip]));
            
            if (!Round.Ended)
                Core.Socket.Emit("server.addip", [Core.ServerID, ev.Player.UserInformation.Ip, ev.Player.UserInformation.UserId, ev.Player.UserInformation.Nickname]);
		}
	}
}