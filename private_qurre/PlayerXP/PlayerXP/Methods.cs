using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
namespace PlayerXP
{
	[Serializable]
	public class Stats
	{
		public string userId;
		public int lvl;
		public int xp;
		public int to;
	}
	public class Data
	{
		public ObjectId Id { get; set; }
		public string UserId { get; set; } = "";
		public int lvl { get; set; } = 1;
		public int xp { get; set; } = 0;
		public int to { get; set; } = 750;
	}
	public class Methods
	{
		public static Stats LoadStats(string userId)
		{
			var database = Plugin.Client.GetDatabase("PlayerXP");
			var collection = database.GetCollection<Data>("stats");
			var list = collection.Find(new BsonDocument("UserId", userId)).ToList();
			if(list.Count == 0)
			{
				collection.InsertOneAsync(new Data { UserId = userId });
				return new Stats()
				{
					userId = userId,
					lvl = 1,
					xp = 1,
					to = 750,
				};
			}
			var document = list.First();
			return new Stats()
			{
				userId = userId,
				lvl = document.lvl,
				xp = document.xp,
				to = document.to,
			};
		}
		public static void SaveStats(Stats stats)
		{
			var database = Plugin.Client.GetDatabase("PlayerXP");
			var collection = database.GetCollection<Data>("stats");
			var filter = Builders<Data>.Filter.Eq("UserId", stats.userId);
			collection.UpdateOne(filter, Builders<Data>.Update.Set("lvl", stats.lvl).Set("xp", stats.xp).Set("to", stats.to));
		}
	}
}