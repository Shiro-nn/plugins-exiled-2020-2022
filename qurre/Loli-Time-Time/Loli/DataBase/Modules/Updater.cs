using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;

namespace Loli.DataBase.Modules
{
	static class Updater
	{
		[EventMethod(PlayerEvents.ChangeRole)]
		static void Spawn(ChangeRoleEvent ev) => ChangeRole(ev.Player, ev.Role.GetTeam());

		[EventMethod(PlayerEvents.Spawn)]
		static void Spawn(SpawnEvent ev) => ChangeRole(ev.Player, ev.Role.GetTeam());

		static void ChangeRole(Player pl, Team tm)
		{
			if (tm is not Team.SCPs) return;
			if (Data.scp_play.ContainsKey(pl.UserInfomation.UserId)) Data.scp_play[pl.UserInfomation.UserId] = true;
			else Data.scp_play.Add(pl.UserInfomation.UserId, true);
		}
		static bool _endSaving = false;

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting() => _endSaving = false;

		[EventMethod(RoundEvents.End)]
		static void End()
		{
			_endSaving = true;
			foreach (var pl in Player.List) try
				{
					if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var data) && data.found)
						Core.Socket.Emit("database.add.time", new object[] { data.id, 1, (int)(DateTime.Now - data.entered).TotalSeconds });
				}
				catch { }
		}

		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (Module.Prefixs.ContainsKey(ev.Player.UserInfomation.UserId)) Module.Prefixs.Remove(ev.Player.UserInfomation.UserId);
			if (Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var data))
			{
				Data.Users.Remove(ev.Player.UserInfomation.UserId);
				if (_endSaving) return;
				if (data.found)
					Core.Socket.Emit("database.add.time", new object[] { data.id, 1, (int)(DateTime.Now - data.entered).TotalSeconds });
			}
		}
	}
}