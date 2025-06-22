using Exiled.API.Features;
using Grenades;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameCore;
using MongoDB.scp035.API;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;
using Exiled.Events.EventArgs;
using CustomPlayerEffects;
namespace MongoDB.Patches
{
	[HarmonyPatch(typeof(FragGrenade), nameof(FragGrenade.ServersideExplosion))]
	static class FragGrenadeServersideExplosionPatch
	{
		public static void Postfix(FragGrenade __instance)
		{
			Player thrower = Player.Get(__instance.thrower.gameObject);
			Player scp035 = Player.Get(Scp035Data.GetScp035());
			if (thrower != null && scp035 != null && thrower.Id != scp035.Id && thrower.Team == scp035.Team)
			{
				Vector3 position = __instance.transform.position;
				PlayerStats component = scp035.ReferenceHub.GetComponent<PlayerStats>();
				float amount = (float)(__instance.damageOverDistance.Evaluate(Vector3.Distance(position, component.transform.position)) * (component.ccm.IsHuman() ? ConfigFile.ServerConfig.GetFloat("human_grenade_multiplier", 0.7f) : ConfigFile.ServerConfig.GetFloat("scp_grenade_multiplier", 1f)));
				if (amount > __instance.absoluteDamageFalloff)
					component.HurtPlayer(new PlayerStats.HitInfo(amount, (__instance.thrower != null) ? __instance.thrower.hub.LoggedNameFromRefHub() : "(UNKNOWN)", DamageTypes.Grenade, __instance.thrower.hub.queryProcessor.PlayerId), scp035.GameObject, false);
			}
		}
	}
	[HarmonyPatch(typeof(FlashGrenade), nameof(FlashGrenade.ServersideExplosion))]
	static class FlashGrenadeServersideExplosionPatch
	{
		public static void Postfix(FlashGrenade __instance)
		{
			Player thrower = Player.Get(__instance.thrower.gameObject);
			Player scp035 = Player.Get(Scp035Data.GetScp035());
			if (thrower != null && scp035 != null && thrower.Id != scp035.Id && thrower.Team == scp035.Team)
			{
				GameObject gameObject = scp035.GameObject;
				Vector3 position = __instance.transform.position;
				ReferenceHub hub = ReferenceHub.GetHub(gameObject);
				CustomPlayerEffects.Flashed effect = hub.playerEffectsController.GetEffect<CustomPlayerEffects.Flashed>();
				CustomPlayerEffects.Deafened effect2 = hub.playerEffectsController.GetEffect<CustomPlayerEffects.Deafened>();
				if (effect != null && __instance.thrower != null && Flashable(thrower.ReferenceHub, scp035.ReferenceHub, position, __instance._ignoredLayers))
				{
					float num = __instance.powerOverDistance.Evaluate(Vector3.Distance(gameObject.transform.position, position) / ((position.y > 900f) ? __instance.distanceMultiplierSurface : __instance.distanceMultiplierFacility)) * __instance.powerOverDot.Evaluate(Vector3.Dot(hub.PlayerCameraReference.forward, (hub.PlayerCameraReference.position - position).normalized));
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num * 10f * __instance.maximumDuration), 1, 255);
					if (b >= effect.Intensity && num > 0f)
					{
						hub.playerEffectsController.ChangeEffectIntensity<CustomPlayerEffects.Flashed>(b);
						if (effect2 != null)
						{
							hub.playerEffectsController.EnableEffect(effect2, num * __instance.maximumDuration, true);
						}
					}
				}
			}
		}
		private static bool Flashable(ReferenceHub throwerPlayerHub, ReferenceHub targetPlayerHub, Vector3 sourcePosition, int ignoreMask)
		{
			return targetPlayerHub != throwerPlayerHub && !Physics.Linecast(sourcePosition, targetPlayerHub.PlayerCameraReference.position, ignoreMask);
		}
		[HarmonyPatch(typeof(PlayableScps.Scp096), nameof(PlayableScps.Scp096.OnDamage))]
		static class Scp096Patches
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
			{
				var newInstructions = instructions.ToList();

				var returnLabel = newInstructions.First(i => i.opcode == OpCodes.Brfalse_S).operand;
				var continueLabel = generator.DefineLabel();
				var firstOffset = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldloc_0) + 1;

				var player = generator.DeclareLocal(typeof(Player));
				var scp035 = generator.DeclareLocal(typeof(Player));

				newInstructions.InsertRange(firstOffset, new CodeInstruction[]
				{
				new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(UnityEngine.GameObject) })),
				new CodeInstruction(OpCodes.Stloc_1, player.LocalIndex),
				new CodeInstruction(OpCodes.Call, Method(typeof(Scp035Data), nameof(Scp035Data.GetScp035))),
				new CodeInstruction(OpCodes.Stloc_2, scp035.LocalIndex),
				new CodeInstruction(OpCodes.Ldloc_0)
				});

				var secondOffset = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldnull) + 3;

				newInstructions.InsertRange(secondOffset, new CodeInstruction[]
				{
				new CodeInstruction(OpCodes.Ldloc_2),
				new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
				new CodeInstruction(OpCodes.Ldloc_1),
				new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
				new CodeInstruction(OpCodes.Ldloc_2),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.Id))),
				new CodeInstruction(OpCodes.Ldloc_1),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.Id))),
				new CodeInstruction(OpCodes.Beq_S, returnLabel)
				});

				newInstructions[secondOffset + 9].labels.Add(continueLabel);

				foreach (var code in newInstructions)
					yield return code;
			}
		}
		[HarmonyPatch(typeof(Scp106PlayerScript), nameof(Scp106PlayerScript.CallCmdMovePlayer))]
		static class Scp106Patch
		{
			public static bool Prefix(Scp106PlayerScript __instance, GameObject ply, int t)
			{
				try
				{
					if (!__instance._iawRateLimit.CanExecute(true) || ply == null)
						return false;

					ReferenceHub hub = ReferenceHub.GetHub(ply);
					CharacterClassManager ccm = hub != null ? hub.characterClassManager : null;

					if (ccm == null)
						return false;

					if (!ServerTime.CheckSynchronization(t)
						|| !__instance.iAm106
						|| Vector3.Distance(hub.playerMovementSync.RealModelPosition, ply.transform.position) >= 3f
						|| !ccm.IsHuman()
						|| ccm.GodMode
						|| ccm.CurRole.team == Team.SCP)
					{
						return false;
					}

					var instanceHub = ReferenceHub.GetHub(__instance.gameObject);
					instanceHub.characterClassManager.RpcPlaceBlood(ply.transform.position, 1, 2f);
					__instance.TargetHitMarker(__instance.connectionToClient);

					if (Scp106PlayerScript._blastDoor.isClosed)
					{
						instanceHub.characterClassManager.RpcPlaceBlood(ply.transform.position, 1, 2f);
						instanceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(500f, instanceHub.LoggedNameFromRefHub(), DamageTypes.Scp106, instanceHub.playerId), ply);
					}
					else
					{
						Scp079Interactable.ZoneAndRoom otherRoom = hub.scp079PlayerScript.GetOtherRoom();
						Scp079Interactable.InteractableType[] filter = new Scp079Interactable.InteractableType[]
						{
							Scp079Interactable.InteractableType.Door, Scp079Interactable.InteractableType.Light,
							Scp079Interactable.InteractableType.Lockdown, Scp079Interactable.InteractableType.Tesla,
							Scp079Interactable.InteractableType.ElevatorUse,
						};

						foreach (Scp079PlayerScript scp079PlayerScript in Scp079PlayerScript.instances)
						{
							bool flag = false;

							foreach (Scp079Interaction scp079Interaction in scp079PlayerScript.ReturnRecentHistory(12f, filter))
							{
								foreach (Scp079Interactable.ZoneAndRoom zoneAndRoom in scp079Interaction.interactable
									.currentZonesAndRooms)
								{
									if (zoneAndRoom.currentZone == otherRoom.currentZone &&
										zoneAndRoom.currentRoom == otherRoom.currentRoom)
									{
										flag = true;
									}
								}
							}

							if (flag)
							{
								scp079PlayerScript.RpcGainExp(ExpGainType.PocketAssist, ccm.CurClass);
							}
						}

						var ev = new EnteringPocketDimensionEventArgs(Player.Get(ply), Vector3.down * 1998.5f, Player.Get(instanceHub));

						Exiled.Events.Handlers.Player.OnEnteringPocketDimension(ev);

						if (!ev.IsAllowed)
							return false;

						hub.playerMovementSync.OverridePosition(ev.Position, 0f, true);

						instanceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(40f, instanceHub.LoggedNameFromRefHub(), DamageTypes.Scp106, instanceHub.playerId), ply);

						PlayerEffectsController effectsController = hub.playerEffectsController;
						effectsController.GetEffect<Corroding>().IsInPd = true;
						effectsController.EnableEffect<Corroding>(0.0f, false);
					}
					return false;
				}
				catch
				{
					return true;
				}
			}
		}
	}
}