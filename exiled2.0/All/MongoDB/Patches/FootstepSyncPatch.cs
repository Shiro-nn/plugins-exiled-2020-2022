using HarmonyLib;

namespace MongoDB.Patches
{
	[HarmonyPatch(typeof(FootstepSync), nameof(FootstepSync.PlayFootstepSound))]
	class FootstepSyncPatch
	{
		private static int count = 0;

		public static void Prefix(FootstepSync __instance)
		{
			ReferenceHub player = Extensions.GetPlayer(__instance.gameObject);
			count++;
			if (player.queryProcessor.PlayerId == MongoDB.scp035.EventHandlers.scpPlayer?.queryProcessor.PlayerId && count >= 5)
			{
				player.PlaceCorrosion();
				count = 0;
			}
		}
	}
}
