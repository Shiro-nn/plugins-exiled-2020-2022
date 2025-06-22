using HarmonyLib;
using InventorySystem.Items.Usables;
using Loli.Spawns;
using MEC;
using Newtonsoft.Json;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Events;
using System.Collections.Generic;
using UnityEngine;
using RT = Loli.Module.RoleType;
namespace Loli.DataBase
{
	[HarmonyPatch(typeof(Adrenaline), "OnEffectsActivated")]
	internal static class Customize_Patch
	{
		internal static bool Prefix(Adrenaline __instance)
		{
			var pl = Player.Get(__instance.Owner);
			if (!Customize.Customizes.ContainsKey(pl.UserId)) return true;
			var gens = Customize.GetGens(pl);
			if (!gens.adrenaline_compatible) return true;
			Customize.UseAdrenaline(pl, gens.adrenaline_compatible);
			return false;
		}
	}
	internal class Customize
	{
		internal static Customize Static { get; private set; }
		public Customize()
		{
			Static = this;
			Init();
		}
		internal static readonly Dictionary<string, BdData> Customizes = new();
		private void Init()
		{
			Plugin.Socket.On("database.get.donate.customize", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get(userid);
				if (pl is null) return;
				BdData json = JsonConvert.DeserializeObject<BdData>(obj[0].ToString());
				if (Customizes.ContainsKey(pl.UserId)) Customizes.Remove(pl.UserId);
				Customizes.Add(pl.UserId, json);
			});
		}
		internal void Leave(LeaveEvent ev)
		{
			if (!Customizes.ContainsKey(ev.Player.UserId)) return;
			Customizes.Remove(ev.Player.UserId);
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (Plugin.RolePlay || Plugin.ClansWars) return;
			if (!Customizes.ContainsKey(ev.Player.UserId)) return;
			var scale = GetScale(ev.Player, ev.RoleType);
			ev.Player.Scale = scale;
			Timing.CallDelayed(0.5f, () => ev.Player.Scale = scale);
		}
		internal void Spawn(RoleChangeEvent ev)
		{
			if (Plugin.RolePlay || Plugin.ClansWars) return;
			if (!Customizes.ContainsKey(ev.Player.UserId)) return;
			Timing.CallDelayed(0.5f, () => ev.Player.Scale = GetScale(ev.Player, ev.NewRole));
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (Plugin.RolePlay || Plugin.ClansWars) return;
			if (ev.FriendlyFire && !Server.FriendlyFire) return;
			if (!ev.Allowed) return;
			if (ev.Amount < 0) return;
			if (!Customizes.ContainsKey(ev.Target.UserId)) return;
			var gens = GetGens(ev.Target);
			if (gens.native_armor) ev.Amount *= Random.Range(0.85f, 1);
			if (!gens.adrenaline_rush) return;
			if (ev.Amount > 100) return;
			if (ev.Target.GetEffectActive<CustomPlayerEffects.Invigorated>()) return;
			if (Random.Range(0, 100) < 20) return;
			if (ev.Target.Hp - ev.Amount < 2)
			{
				ev.Amount = 0;
				ev.Allowed = false;
				ev.Target.Hp = 1;
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
			pl.ReferenceHub.fpc.ModifyStamina(100f);
			pl.PlayerStats.GetModule<AhpStat>().ServerAddProcess(d1);
			pl.PlayerEffectsController.EnableEffect<CustomPlayerEffects.Invigorated>(d2, true);
			pl.PlayerEffectsController.UseMedicalItem(ItemType.Adrenaline);
			if (best) Timing.RunCoroutine(PostFix(pl));
			static IEnumerator<float> PostFix(Player pl)
			{
				if (pl == null) yield break;
				var rand = Random.Range(15, 20);
				for (int i = 0; i < rand; i++)
				{
					yield return Timing.WaitForSeconds(1);
					pl.PlayerStats.GetModule<HealthStat>().ServerHeal(Random.Range(2, 4));
				}
				yield break;
			}
		}
		internal Vector3 GetScale(Player pl, RoleType role)
		{
			if (Plugin.RolePlay || Plugin.ClansWars) return Vector3.one;
			if (!Customizes.ContainsKey(pl.UserId)) return Vector3.one;
			var data = Customizes[pl.UserId].scales;
			if (role == RoleType.ClassD || role == RoleType.ChaosConscript)
			{
				float cmz = data.ClassD > 80 ? data.ClassD : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role == RoleType.Scientist || role == RoleType.NtfSpecialist)
			{
				float cmz = data.Scientist > 80 ? data.Scientist : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role == RoleType.FacilityGuard)
			{
				float cmz = data.Guard > 80 ? data.Guard : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role.GetTeam() == Team.MTF)
			{
				float cmz = data.Mtf > 80 ? data.Mtf : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role.GetTeam() == Team.CHI)
			{
				float cmz = data.Chaos > 80 ? data.Chaos : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			if (role == RoleType.Tutorial)
			{
				float cmz = data.Serpents > 80 ? data.Serpents : 80;
				return new Vector3(cmz / 100, cmz / 100, cmz / 100);
			}
			return Vector3.one;
		}
		internal static GensMod GetGens(Player pl)
		{
			if (Plugin.RolePlay || Plugin.ClansWars) return new();
			if (!Customizes.ContainsKey(pl.UserId)) return new();
			var data = Customizes[pl.UserId].genetics;
			if (pl.GetCustomRole() == RT.ClassD || pl.GetCustomRole() == RT.ChaosConscript) return data.ClassD;
			if (pl.GetCustomRole() == RT.Scientist || pl.GetCustomRole() == RT.NtfSpecialist) return data.Scientist;
			if (pl.GetCustomRole() == RT.FacilityGuard) return data.Guard;
			if (pl.GetTeam() == Team.MTF) return data.Mtf;
			if (pl.GetTeam() == Team.CHI) return data.Chaos;
			if (pl.Tag.Contains(SerpentsHand.HandTag)) return data.Serpents;
			return new();
		}
#pragma warning disable IDE1006
		internal class BdData
		{
			public Gens genetics { get; set; } = new();
			public Scales scales { get; set; } = new();
		}
		internal class GensMod
		{
			public bool adrenaline_compatible { get; set; } = false;
			public bool adrenaline_rush { get; set; } = false;
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