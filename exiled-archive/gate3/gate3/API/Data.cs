using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace gate3.API
{
	public static class gate3Data
	{
		public static List<Player> GetSHPlayers()
		{
			return (from x in EventHandlers.shPlayers
					select Player.Get(x)).ToList<Player>();
		}
	}
}
