using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCore.Modules
{
    static internal class Socket
	{
		static internal void Leave(LeaveEvent ev)
		{
			if (!Round.Ended) Init.Socket.Emit("server.leave", new object[] { Init.ServerID, ev.Player.Ip });
		}
		static internal void ClearIps(RoundEndEvent _) => Init.Socket.Emit("server.clearips", new object[] { Init.ServerID });
	}
}
