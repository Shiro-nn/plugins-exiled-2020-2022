using Newtonsoft.Json;
using Qurre.API;
namespace Loli.DataBase.Modules
{
	internal class Stats
	{
		internal readonly Manager Manager;
		public Stats(Manager manager)
        {
			Manager = manager;
			InitStats();
		}
		private void InitStats()
		{
			Plugin.Socket.On("database.get.stats", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get(userid);
				if (pl is null) return;
				SocketStatsData json = JsonConvert.DeserializeObject<SocketStatsData>(obj[0].ToString());
				if (!Manager.Data.Users.TryGetValue(userid, out var data)) return;
				int oldlvl = data.lvl;
				data.xp = json.xp;
				data.lvl = json.lvl;
				data.to = json.to;
				data.money = json.money;
				if (oldlvl != json.lvl)
				{
					pl.Broadcast(10, $"<color=#fdffbb>Вы получили {json.lvl} уровень!\nДо следующего уровня вам не хватает {json.to - json.xp}xp.</color>");
					Levels.SetPrefix(pl);
				}
			});
		}
		internal void Add(Player pl, int xp, int money)
		{
			Plugin.Socket.Emit("database.add.stats", new object[] { pl.UserId.Replace("@steam", "").Replace("@discord", ""),
				pl.UserId.Contains("discord"), xp, money, pl.UserId});
		}
		internal void AddMoney(Player pl, int money) => Add(pl, 0, money);
		internal void AddXP(Player pl, int xp) => Add(pl, xp, 0);
	}
}