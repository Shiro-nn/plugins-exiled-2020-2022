using MongoDB.Bson;
using MongoDB.Driver;
using Qurre.API;
using System.Linq;
using System.Threading;
namespace Loli.DataBase.Modules
{
	internal class Stats
	{
		internal readonly Manager Manager;
		public Stats(Manager manager) => Manager = manager;
		internal void Add(Player pl, int xp, int money)
		{
			new Thread(() =>
			{
				try
				{
					if (!Manager.Static.Data.Users.TryGetValue(pl.UserId, out var userData)) return;
					string uu = "steam";
					if (pl.UserId.Contains("@discord")) uu = "discord";
					var stata = Manager.DataBase.MongoDatabase.GetCollection<User_Stats>("stats");
					var datas = stata.Find(new BsonDocument(uu, pl.UserId.Replace($"@{uu}", ""))).ToList();
					if (datas.Count == 0) return;
					var data = datas.First();
					data.xp += xp;
					data.money += money;
					userData.money = data.money;
					if (data.xp >= data.to)
					{
						data.xp -= data.to;
						data.lvl++;
						data.to = data.lvl * 250 + 750;
						pl.Broadcast(10, $"<color=#fdffbb>Вы получили {data.lvl} уровень!\nДо следующего уровня вам не хватает {data.to - xp}xp.</color>");
						userData.xp = data.xp;
						userData.lvl = data.lvl;
						userData.to = data.to;
						Levels.SetPrefix(pl);
					}
					else userData.xp = data.xp;
					var filter = Builders<User_Stats>.Filter.Eq(uu, pl.UserId.Replace($"@{uu}", ""));
					stata.UpdateOne(filter, Builders<User_Stats>.Update.Set("xp", data.xp).Set("lvl", data.lvl).Set("to", data.to).Set("money", data.money));
				}
				catch { }
			}).Start();
		}
		internal void AddMoney(Player pl, int money)
		{
			new Thread(() =>
			{
				try
				{
					if (!Manager.Static.Data.Users.TryGetValue(pl.UserId, out var userData)) return;
					string uu = "steam";
					if (pl.UserId.Contains("@discord")) uu = "discord";
					var stata = Manager.DataBase.MongoDatabase.GetCollection<User_Stats>("stats");
					var datas = stata.Find(new BsonDocument(uu, pl.UserId.Replace($"@{uu}", ""))).ToList();
					if (datas.Count == 0) return;
					var data = datas.First();
					data.money += money;
					userData.money = data.money;
					var filter = Builders<User_Stats>.Filter.Eq(uu, pl.UserId.Replace($"@{uu}", ""));
					stata.UpdateOne(filter, Builders<User_Stats>.Update.Set("money", data.money));
				}
				catch { }
			}).Start();
		}
		internal void AddXP(Player pl, int xp)
		{
			new Thread(() =>
			{
				try
				{
					if (!Manager.Static.Data.Users.TryGetValue(pl.UserId, out var userData)) return;
					string uu = "steam";
					if (pl.UserId.Contains("@discord")) uu = "discord";
					var stata = Manager.DataBase.MongoDatabase.GetCollection<User_Stats>("stats");
					var datas = stata.Find(new BsonDocument(uu, pl.UserId.Replace($"@{uu}", ""))).ToList();
					if (datas.Count == 0) return;
					var data = datas.First();
					data.xp += xp;
					if (data.xp >= data.to)
					{
						data.xp -= data.to;
						data.lvl++;
						data.to = data.lvl * 250 + 750;
						pl.Broadcast(10, $"<color=#fdffbb>Вы получили {data.lvl} уровень!\nДо следующего уровня вам не хватает {data.to - xp}xp.</color>");
						userData.xp = data.xp;
						userData.lvl = data.lvl;
						userData.to = data.to;
						Levels.SetPrefix(pl);
					}
					else userData.xp = data.xp;
					var filter = Builders<User_Stats>.Filter.Eq(uu, pl.UserId.Replace($"@{uu}", ""));
					stata.UpdateOne(filter, Builders<User_Stats>.Update.Set("xp", data.xp).Set("lvl", data.lvl).Set("to", data.to));
				}
				catch { }
			}).Start();
		}
		internal bool Update(Player pl, out UserData _data)
		{
			_data = default;
			try
			{
				if (!Manager.Static.Data.Users.TryGetValue(pl.UserId, out var userData)) return false;
				string uu = "steam";
				if (pl.UserId.Contains("@discord")) uu = "discord";
				var stata = Manager.DataBase.MongoDatabase.GetCollection<User_Stats>("stats");
				var datas = stata.Find(new BsonDocument(uu, pl.UserId.Replace($"@{uu}", ""))).ToList();
				if (datas.Count == 0) return false;
				var data = datas.First();
				userData.xp = data.xp;
				userData.to = data.to;
				userData.lvl = data.lvl;
				userData.money = data.money;
				_data = userData;
				return true;
			}
			catch { return false; }
		}
	}
}