using System.Collections.Generic;
using System.IO;

namespace Shop
{
	public class Methods
	{
		internal static Stats LoadStats(string userId)
		{
			string path = Path.Combine(Plugin.StatFilePath, $"{userId}.txt");

			if (File.Exists(path))
				return DeserializeStats(path);
			else
			{
				return new Stats()
				{
					UserId = userId,
					money = 0,
				};
			}
		}

		internal static void SaveStats(Stats stats)
		{
			string[] write = new[]
			{
				stats.UserId, 
				stats.money.ToString(), 
			};

			string path = Path.Combine(Plugin.StatFilePath, $"{stats.UserId}.txt");
			File.WriteAllLines(path, write);
		}

		private static Stats DeserializeStats(string path)
		{
			string[] read = File.ReadAllLines(path);
			Stats stats = new Stats
			{
				UserId = read[0],
				money = int.Parse(read[1]),
			};
			return stats;
		}

	}
}