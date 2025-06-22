using System.Collections.Generic;
using System.IO;

namespace PlayerXP
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
					lvl = 1,
					xp = 1,
					to = 750,
				};
			}
		}

		internal static void SaveStats(Stats stats)
		{
			string[] write = new[]
			{
				stats.UserId, 
				stats.lvl.ToString(), 
				stats.xp.ToString(),
				stats.to.ToString(),
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
				lvl = int.Parse(read[1]),//int
				xp = int.Parse(read[2]),
				to = int.Parse(read[3]),
			};
			return stats;
		}

	}
}