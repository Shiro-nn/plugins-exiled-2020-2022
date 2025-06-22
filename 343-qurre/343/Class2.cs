using Qurre.API;
using System.Collections.Generic;
using System.Linq;
namespace _343
{
	public static class Api
	{
		public static List<Player> Get343()
		{
			return Player.List.Where(x => x.Tag.Contains(Scp343.Tag)).ToList();
		}
		public static bool ItsScp343(this Player pl)
		{
			try
			{
				return pl.Tag.Contains(Scp343.Tag);
			}
			catch
			{
				return false;
			}
		}
	}
}