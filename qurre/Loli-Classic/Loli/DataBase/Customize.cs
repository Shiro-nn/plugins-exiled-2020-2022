using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Usables;
using MEC;
using Newtonsoft.Json;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.DataBase
{
	[HarmonyPatch(typeof(Adrenaline), "OnEffectsActivated")]
	static class Customize_Patch
	{
		[HarmonyPrefix]
		internal static bool Prefix(Adrenaline __instance)
		{
			var pl = __instance.Owner.GetPlayer();
			if (!Customize.Customizes.ContainsKey(pl.UserInfomation.UserId)) return true;
			var gens = Customize.GetGens(pl);
			if (!gens.adrenaline_compatible) return true;
			Customize.UseAdrenaline(pl, gens.adrenaline_compatible);
			return false;
		}
	}
	static class Customize
	{
		static Customize()
		{
			Init();
		}
		internal static readonly Dictionary<string, BdData> Customizes = new();
		static void Init()
		{
			Core.Socket.On("database.get.donate.customize", obj =>
			{
				string userid = obj[1].ToString();
				var pl = userid.GetPlayer();
				if (pl is null) return;
				BdData json = JsonConvert.DeserializeObject<BdData>(obj[0].ToString());
				if (Customizes.ContainsKey(pl.UserInfomation.UserId)) Customizes.Remove(pl.UserInfomation.UserId);
				Customizes.Add(pl.UserInfomation.UserId, json);
			});
		}

		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (!Customizes.ContainsKey(ev.Player.UserInfomation.UserId)) return;
			Customizes.Remove(ev.Player.UserInfomation.UserId);
		}

		[EventMethod(PlayerEvents.Spawn)]
		static void Spawn(SpawnEvent ev)
		{
			if (!Customizes.ContainsKey(ev.Player.UserInfomation.UserId)) return;
			var scale = GetScale(ev.Player, ev.Role);
			ev.Player.MovementState.Scale = scale;
			Timing.CallDelayed(0.5f, () => ev.Player.MovementState.Scale = scale);
		}

		[EventMethod(PlayerEvents.ChangeRole)]
		static void Spawn(ChangeRoleEvent ev)
		{
			if (!Customizes.ContainsKey(ev.Player.UserInfomation.UserId)) return;
			Timing.CallDelayed(0.5f, () => ev.Player.MovementState.Scale = GetScale(ev.Player, ev.Role));
		}

		[EventMethod(PlayerEvents.Attack)]
		static void Damage(AttackEvent ev)
		{
			if (ev.FriendlyFire && !Server.FriendlyFire) return;
			if (!ev.Allowed) return;
			if (ev.Damage < 0) return;
			if (!Customizes.ContainsKey(ev.Target.UserInfomation.UserId)) return;
			var gens = GetGens(ev.Target);
			if (gens.native_armor) ev.Damage *= Random.Range(0.85f, 1);
			if (!gens.adrenaline_rush) return;
			if (ev.Damage > 100) return;
			if (ev.Target.Effects.CheckActive<CustomPlayerEffects.Invigorated>()) return;
			if (Random.Range(0, 100) < 20) return;
			if (ev.Target.HealthInfomation.Hp - ev.Damage < 2)
			{
				ev.Damage = 0;
				ev.Allowed = false;
				ev.Target.HealthInfomation.Hp = 1;
				UseAdrenaline(ev.Target, gens.adrenaline_compatible);
			}
		}

		internal static void UseAdrenaline(Player pl, bool best)
		{
			float d1 = 50;
			float d2 = 8;
			if (best)
			{
				d1 = 75;
				d2 = 16;
			}
			pl.HealthInfomation.Stamina = 100f;
			pl.HealthInfomation.Ahp = d1;
			pl.Effects.Enable<CustomPlayerEffects.Invigorated>(d2, true);
			pl.Effects.Controller.UseMedicalItem(Server.InventoryHost.CreateItemInstance(new(ItemType.Adrenaline, ItemSerialGenerator.GenerateNext()), false));
			if (best) Timing.RunCoroutine(PostFix(pl));
			static IEnumerator<float> PostFix(Player pl)
			{
				if (pl == null) yield break;
				var rand = Random.Range(15, 20);
				for (int i = 0; i < rand; i++)
				{
					yield return Timing.WaitForSeconds(1);
					pl.HealthInfomation.Heal(Random.Range(2, 4), false);
				}
				yield break;
			}
		}
		static internal Vector3 GetScale(Player pl, RoleTypeId role)
		{
			if (!Customizes.ContainsKey(pl.UserInfomation.UserId)) return Vector3.one;
			var data = Customizes[pl.UserInfomation.UserId].scales;
			if (role == RoleTypeId.ClassD || role == RoleTypeId.ChaosConscript)
			{
				float cmz = data.ClassD > 80 ? data.ClassD : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role == RoleTypeId.Scientist || role == RoleTypeId.NtfSpecialist)
			{
				float cmz = data.Scientist > 80 ? data.Scientist : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role == RoleTypeId.FacilityGuard)
			{
				float cmz = data.Guard > 80 ? data.Guard : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role == RoleTypeId.Tutorial)
			{
				float cmz = data.Serpents > 80 ? data.Serpents : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}

			var team = role.GetTeam();
			if (team == Team.FoundationForces)
			{
				float cmz = data.Mtf > 80 ? data.Mtf : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (team == Team.ChaosInsurgency)
			{
				float cmz = data.Chaos > 80 ? data.Chaos : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			return Vector3.one;
		}
		internal static GensMod GetGens(Player pl)
		{
			if (!Customizes.ContainsKey(pl.UserInfomation.UserId)) return new();
			var data = Customizes[pl.UserInfomation.UserId].genetics;
			if (pl.GetCustomRole() == RoleType.ClassD || pl.GetCustomRole() == RoleType.ChaosConscript) return data.ClassD;
			if (pl.GetCustomRole() == RoleType.Scientist || pl.GetCustomRole() == RoleType.NtfSpecialist) return data.Scientist;
			if (pl.GetCustomRole() == RoleType.FacilityGuard) return data.Guard;
			if (pl.GetTeam() == Team.FoundationForces) return data.Mtf;
			if (pl.GetTeam() == Team.ChaosInsurgency) return data.Chaos;
			return new();
		}
#pragma warning disable IDE1006
		internal class BdData
		{
			[JsonProperty("genetics")]
			public Gens genetics { get; set; } = new();

			[JsonProperty("scales")]
			public Scales scales { get; set; } = new();
		}
		internal class GensMod
		{
			[JsonProperty("adrenaline_compatible")]
			public bool adrenaline_compatible { get; set; } = false;

			[JsonProperty("adrenaline_rush")]
			public bool adrenaline_rush { get; set; } = false;

			[JsonProperty("native_armor")]
			public bool native_armor { get; set; } = false;
		}
#pragma warning restore IDE1006
		internal class Scales
		{
			public float ClassD { get; set; } = 100;
			public float Scientist { get; set; } = 100;
			public float Guard { get; set; } = 100;
			public float Mtf { get; set; } = 100;
			public float Chaos { get; set; } = 100;
			public float Serpents { get; set; } = 100;
		}
		internal class Gens
		{
			public GensMod ClassD { get; set; } = new();
			public GensMod Scientist { get; set; } = new();
			public GensMod Guard { get; set; } = new();
			public GensMod Mtf { get; set; } = new();
			public GensMod Chaos { get; set; } = new();
			public GensMod Serpents { get; set; } = new();
		}
	}
}