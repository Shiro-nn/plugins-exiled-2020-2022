using HarmonyLib;
using Loli.Scps.Api;
using Qurre.API;
namespace Loli.Patches
{
	[HarmonyPatch(typeof(FootstepSync), nameof(FootstepSync.PlayFootstepSound))]
	internal static class FootstepSyncPatch
	{
		private static int count = 0;
		public static void Prefix(FootstepSync __instance)
		{
			Player player = Player.Get(__instance.gameObject);
			if (player == null) return;
			count++;
			if (player.ItsScp035() && count >= 5)
			{
				player.PlaceBlood(player.Position, 1, 2f);
				count = 0;
			}
		}
	}
}