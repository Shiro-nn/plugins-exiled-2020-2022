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

		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (Module.Prefixs.ContainsKey(ev.Player.UserInfomation.UserId)) Module.Prefixs.Remove(ev.Player.UserInfomation.UserId);
			if (Data.Users.ContainsKey(ev.Player.UserInfomation.UserId)) Data.Users.Remove(ev.Player.UserInfomation.UserId);
		}
	}
}