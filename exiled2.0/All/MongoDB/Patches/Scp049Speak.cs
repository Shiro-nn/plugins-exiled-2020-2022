using System;
using Assets._Scripts.Dissonance;
using HarmonyLib;
using Exiled.API.Features;

namespace MongoDB.Patches
{
	[HarmonyPatch(typeof(DissonanceUserSetup), "CallCmdAltIsActive")]
	public class Scp049Speak
	{
		public static bool Prefix(DissonanceUserSetup __instance, bool value)
		{
			CharacterClassManager component = __instance.gameObject.GetComponent<CharacterClassManager>();
			if (component.CurClass == RoleType.Scp049 || component.CurClass.Is939())
			{
				__instance.IntercomAsHuman = value;
				__instance.MimicAs939 = value;
			}
			return false;
		}
	}
}