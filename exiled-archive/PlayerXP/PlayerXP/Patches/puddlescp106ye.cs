using System;
using Assets._Scripts.Dissonance;
using HarmonyLib;
using Exiled.API.Features;
using RemoteAdmin;
using CustomPlayerEffects;
using UnityEngine;
using Mirror;
using PlayerXP.scp343.API;
using PlayerXP.scp228.API;

namespace PlayerXP.Patches
{
	[HarmonyPatch(typeof(SinkholeEnvironmentalHazard), "DistanceChanged", new Type[]
	   {
		typeof(GameObject)
	   })]
	public class puddlescp106ye
	{
		public static bool Prefix(SinkholeEnvironmentalHazard __instance, GameObject player)
		{
			bool result;
			if (!NetworkServer.active)
			{
				result = false;
			}
			else
			{
				PlayerEffectsController componentInParent = player.GetComponentInParent<PlayerEffectsController>();
				if (componentInParent == null)
				{
					result = false;
				}
				else
				{
					componentInParent.GetEffect<SinkHole>();
					bool scpimmune = __instance.SCPImmune;
					if (scpimmune)
					{
						Player pl = Player.Get(player);
						CharacterClassManager component = player.GetComponent<CharacterClassManager>();
						bool flag3 = component == null || component.IsAnyScp() || component.CurClass == RoleType.Tutorial || pl.ReferenceHub.queryProcessor.PlayerId == scp343Data.GetScp343()?.queryProcessor.PlayerId || pl.ReferenceHub.queryProcessor.PlayerId == Scp228Data.GetScp228()?.queryProcessor.PlayerId;
						if (flag3)
						{
							return false;
						}
					}
					Player player2 = Player.Get(player);
					bool isGodModeEnabled = player2.IsGodModeEnabled;
					if (isGodModeEnabled)
					{
						result = false;
					}
					else
					{
						if (Vector3.Distance(player.transform.position, __instance.transform.position) > __instance.DistanceToBeAffected * 1.15f)
						{
							PlayerEffectsController playerEffectsController;
							if (player.TryGetComponent<PlayerEffectsController>(out playerEffectsController))
							{
								SinkHole effect = playerEffectsController.GetEffect<SinkHole>();
								if (effect != null && effect.Enabled)
								{
									componentInParent.DisableEffect<SinkHole>();
								}
								result = false;
							}
							else
							{
								result = false;
							}
						}
						else
						{
							if (Vector3.Distance(player.transform.position, __instance.transform.position) < __instance.DistanceToBeAffected * 0.7f)
							{
								componentInParent.DisableEffect<SinkHole>();
								ReferenceHub hub = ReferenceHub.GetHub(player);
								hub.playerMovementSync.OverridePosition(Vector3.down * 1998.5f, 0f, true);
								PlayerEffectsController playerEffectsController2 = hub.playerEffectsController;
								playerEffectsController2.GetEffect<Corroding>().IsInPd = true;
								playerEffectsController2.EnableEffect<Corroding>(0f, false);
								result = false;
							}
							else
							{
								componentInParent.EnableEffect<SinkHole>(0f, false);
								result = false;
							}
						}
					}
				}
			}
			return result;
		}
	}
}