using Newtonsoft.Json;
using Qurre.API;

namespace Loli.DataBase.Modules
{
	static class Stats
	{
		static Stats() => InitStats();

		static void InitStats()
		{
			Core.Socket.On("database.get.stats", obj =>
			{
				string userid = obj[1].ToString();
				var pl = userid.GetPlayer();
				if (pl is null) return;
				SocketStatsData json = JsonConvert.DeserializeObject<SocketStatsData>(obj[0].ToString());
				if (!Data.Users.TryGetValue(userid, out var data)) return;
				int oldlvl = data.lvl;
				data.xp = json.xp;
				data.lvl = json.lvl;
				data.to = json.to;
				data.money = json.money;
				if (oldlvl != json.lvl)
				{
					pl.Client.Broadcast(10, $"<color=#fdffbb>Вы получили {json.lvl} уровень!\nДо следующего уровня вам не хватает {json.to - json.xp}xp.</color>");
					Levels.SetPrefix(pl);
				}
			});
		}
		static internal void Add(Player pl, int xp, int money)
		{
			Core.Socket.Emit("database.add.stats", new object[] { pl.UserInfomation.UserId.Replace("@steam", "").Replace("@discord", ""),
				pl.UserInfomation.UserId.Contains("discord"), xp, money, pl.UserInfomation.UserId});
		}
		static internal void AddMoney(Player pl, int money) => Add(pl, 0, money);
		static internal void AddXP(Player pl, int xp) => Add(pl, xp, 0);
	}
}