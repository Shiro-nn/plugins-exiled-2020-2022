using Qurre.API;
using System.Linq;
using UnityEngine;
namespace scp035
{
	internal static class Extensions
	{
		internal static void TeleportTo106(this Player player)
		{
			try
			{
				Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
				Vector3 toded = scp106.Position;
				player.Position = toded;
			}
			catch
			{
				player.Position = Map.GetRandomSpawnPoint(RoleType.Scp096);
			}
		}
	}
}