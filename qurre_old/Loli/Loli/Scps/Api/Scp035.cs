using Qurre.API;
using System.Collections.Generic;
using System.Linq;
namespace Loli.Scps.Api
{
	public static class Scp035
	{
		public static List<Player> Get035()
		{
			return Player.List.Where(x => x.Tag.Contains(Scps.Scp035.Tag) && x.Role != RoleType.Spectator).ToList();
		}
		public static bool ItsScp035(this Player pl)
		{
			return pl.Tag.Contains(Scps.Scp035.Tag);
		}
	}
}