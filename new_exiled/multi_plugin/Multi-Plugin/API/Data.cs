using System.Collections.Generic;

namespace MultiPlugin.API
{
	public static class scp682Data
	{
		public static ReferenceHub GetScp682()
		{
			return EventHandlers.scp682;
		}

		public static void Spawn682(ReferenceHub player)
		{
			EventHandlers.SpawnJG(player);
		}
		public static ReferenceHub GetScp343()
		{
			return EventHandlers.scp343;
		}

		public static void Spawn343(ReferenceHub player)
		{
			EventHandlers.Spawn343(player);
		}
	}
}
namespace MultiPlugin5.API
{
	public static class SerpentsHand
	{
		public static void SpawnPlayer(ReferenceHub player, bool full = true)
		{
			EventHandlers.SpawnPlayer(player, full);
		}

		public static void SpawnSquad(List<ReferenceHub> playerList)
		{
			EventHandlers.SpawnSquad(playerList);
		}

		public static void SpawnSquad(int size)
		{
			EventHandlers.CreateSquad(size);
		}

		public static List<int> GetSHPlayers()
		{
			return EventHandlers.shPlayers;
		}
	}
}
namespace MultiPlugin16.API
{
	public static class Scp035Data
	{
		public static ReferenceHub GetScp035()
		{
			return EventHandlers.scpPlayer;
		}

		public static void Spawn035(ReferenceHub player)
		{
			EventHandlers.Spawn035(player, null, false);
		}
	}
}
namespace MultiPlugin20.API
{
	public static class SpyData
	{
		public static Dictionary<ReferenceHub, bool> GetSpies()
		{
			return EventHandlers.spies;
		}

		public static void MakeSpy(ReferenceHub player, bool isVulenrable = false, bool full = true)
		{
			EventHandlers.MakeSpy(player, isVulenrable, full);
		}
	}
}
