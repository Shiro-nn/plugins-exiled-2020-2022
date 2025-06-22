using System;
using HarmonyLib;
using PlayableScps;
using UnityEngine;
namespace PlayerXP.Patches
{
	[HarmonyPatch(typeof(Scp096), "ParseVisionInformation")]
	internal class Scp096ParseVision
	{
		public static bool Prefix(Scp096 __instance, VisionInformation info)
		{
			try
			{
				PlayableScpsController component = info.RaycastResult.transform.gameObject.GetComponent<PlayableScpsController>();
				if (!info.Looking || !info.RaycastHit || component == null || component.CurrentScp == null || component.CurrentScp != __instance)
				{
					return false;
				}
				CharacterClassManager component2 = info.Source.GetComponent<CharacterClassManager>();
				ReferenceHub component3 = info.Source.GetComponent<ReferenceHub>();
				if (component2 == null || component3 == null || component3.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId || component3.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId || component2.CurClass == RoleType.Tutorial)
				{
					return false;
				}
				float delay = (1f - info.DotProduct) / 0.25f * (Vector3.Distance(info.Source.transform.position, info.Target.transform.position) * 0.1f);
				if (!__instance.Calming)
				{
					__instance.AddTarget(info.Source);
				}
				if (__instance.CanEnrage && info.Source != null)
				{
					__instance.PreWindup(delay);
				}
				return false;
			}
			catch { return false; }
		}
	}
}