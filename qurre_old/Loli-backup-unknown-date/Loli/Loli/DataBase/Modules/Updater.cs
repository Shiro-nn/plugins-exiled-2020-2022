using MongoDB.Bson;
using MongoDB.Driver;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Threading;
namespace Loli.DataBase.Modules
{
	internal class Updater
	{
		internal readonly Manager Manager;
		internal Updater(Manager manager) => Manager = manager;
		internal void Spawn(RoleChangeEvent ev) => ChangeRole(ev.Player, Extensions.GetTeam((Loli.Module.RoleType)ev.NewRole));
		internal void Spawn(SpawnEvent ev) => ChangeRole(ev.Player, Extensions.GetTeam((Loli.Module.RoleType)ev.RoleType));
		private void ChangeRole(Player pl, Team tm)
		{
			if (tm == Team.SCP)
			{
				if (Manager.Data.scp_play.ContainsKey(pl.UserId)) Manager.Data.scp_play[pl.UserId] = true;
				else Manager.Data.scp_play.Add(pl.UserId, true);
			}
		}
		internal void End(RoundEndEvent _)
		{
			foreach (var pl in Player.List)
			{
				if (Manager.Data.Users.TryGetValue(pl.UserId, out var data))
				{
					if (data.find)
					{
						new Thread(() =>
						{
							try
							{
								string uu = "steam";
								if (pl.UserId.Contains("@discord")) uu = "discord";
								var collection = Manager.DataBase.GetCollection("accounts");
								var filter = Builders<BsonDocument>.Filter.Eq(uu, pl.UserId.Replace($"@{uu}", ""));
								int game = (int)(DateTime.Now - data.now).TotalSeconds;
								int adminmin = data.admintime + game;
								collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", adminmin));
							}
							catch { }
						}).Start();
					}
				}
			}
		}
		internal void Leave(LeaveEvent ev)
		{
			if (Module.Prefixs.ContainsKey(ev.Player.UserId)) Module.Prefixs.Remove(ev.Player.UserId);
			try { var _ = Controllers.Priest.Get(ev.Player); if (_ is not null) _.Break(); } catch { }
			try { var _ = Controllers.Beginner.Get(ev.Player); if (_ is not null) _.Break(); } catch { }
			try { var _ = Controllers.Star.Get(ev.Player); if (_ is not null) _.Break(); } catch { }
			if (Manager.Data.Users.TryGetValue(ev.Player.UserId, out var data))
			{
				Manager.Data.Users.Remove(ev.Player.UserId);
				if (data.find)
				{
					new Thread(() =>
					{
						try
						{
							string uu = "steam";
							if (ev.Player.UserId.Contains("@discord")) uu = "discord";
							var collection = Manager.DataBase.GetCollection("accounts");
							var filter = Builders<BsonDocument>.Filter.Eq(uu, ev.Player.UserId.Replace($"@{uu}", ""));
							int game = (int)(DateTime.Now - data.now).TotalSeconds;
							int adminmin = data.admintime + game;
							collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", adminmin));
						}
						catch { }
					}).Start();
				}
			}
		}
	}
}