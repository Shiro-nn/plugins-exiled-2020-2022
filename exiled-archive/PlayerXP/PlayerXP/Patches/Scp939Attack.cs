using System;
using CustomPlayerEffects;
using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace PlayerXP.Patches
{
	[HarmonyPatch(typeof(Scp939PlayerScript), "CallCmdShoot")]
	internal class Scp939Attack
	{
		public static void Postfix(Scp939PlayerScript __instance, GameObject target)
		{
			try
			{
				Player player = Player.Get(target);
				if (player.Role == RoleType.Tutorial || player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId || player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
				{
					player.ReferenceHub.playerEffectsController.DisableEffect<Amnesia>();
				}
			}
			catch { }
		}
	}
}
