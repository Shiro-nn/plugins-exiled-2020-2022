using System;
using Assets._Scripts.Dissonance;
using HarmonyLib;
using Exiled.API.Features;

namespace PlayerXP.Patches
{
	[HarmonyPatch(typeof(DissonanceUserSetup), "CallCmdAltIsActive")]
	public class TUTSpeak
	{
		public static bool Prefix(DissonanceUserSetup __instance, bool value)
		{
			try
			{
				CharacterClassManager component = __instance.gameObject.GetComponent<CharacterClassManager>();
				if (component.CurClass == RoleType.Tutorial)
				{
					__instance.SCPChat = true;
				}
				ReferenceHub refe = __instance.gameObject.GetComponent<ReferenceHub>();
				/*
				foreach (Player player in gate3Data.GetSHPlayers())
				{
					if (refe.queryProcessor.PlayerId == player.ReferenceHub.queryProcessor.PlayerId)
					{
						__instance.SpectatorChat = false;
						__instance.RadioAsHuman = false;
						__instance.NetworkspeakingFlags = SpeakingFlags.SCPChat;
						__instance.SCPChat = true;
						__instance.MimicAs939 = true;
						__instance.EnableListening(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
						__instance.EnableSpeaking(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
						__instance.EnableListening(TriggerType.Intercom, Assets._Scripts.Dissonance.RoleType.SCP);
						__instance.EnableSpeaking(TriggerType.Intercom, Assets._Scripts.Dissonance.RoleType.SCP);
					}
				}
				*/
				return true;
			}
			catch { return false; }
		}
	}
}