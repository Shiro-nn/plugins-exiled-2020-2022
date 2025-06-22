using Qurre.API;
using Qurre.API.Events;
using System;
namespace ClassicCore.DataBase.Modules
{
	internal class Updater
	{
		internal readonly Manager Manager;
		internal Updater(Manager manager) => Manager = manager;
		internal void Spawn(RoleChangeEvent ev) => ChangeRole(ev.Player, ev.NewRole.GetTeam());
		internal void Spawn(SpawnEvent ev) => ChangeRole(ev.Player, ev.RoleType.GetTeam());
		private void ChangeRole(Player pl, Team tm)
		{
			if (tm is not Team.SCP) return;
			if (Manager.Data.scp_play.ContainsKey(pl.UserId)) Manager.Data.scp_play[pl.UserId] = true;
			else Manager.Data.scp_play.Add(pl.UserId, true);
		}
		private bool _endSaving = false;
		internal void Waiting() => _endSaving = false;
		internal void End(RoundEndEvent _)
		{
			_endSaving = true;
			foreach (var pl in Player.List) try
				{
					if (Manager.Data.Users.TryGetValue(pl.UserId, out var data) && data.found)
						Init.Socket.Emit("database.add.time", new object[] { data.id, 1, (int)(DateTime.Now - data.entered).TotalSeconds });
				}
				catch { }
		}
		internal void Leave(LeaveEvent ev)
		{
			if (Module.Prefixs.ContainsKey(ev.Player.UserId)) Module.Prefixs.Remove(ev.Player.UserId);
			if (Manager.Data.Users.TryGetValue(ev.Player.UserId, out var data))
			{
				Manager.Data.Users.Remove(ev.Player.UserId);
				if (_endSaving) return;
				if (data.found)
					Init.Socket.Emit("database.add.time", new object[] { data.id, 1, (int)(DateTime.Now - data.entered).TotalSeconds });
			}
		}
	}
}