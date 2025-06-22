using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loli.Spawns
{
	static class MobileTaskForces
	{
		static internal readonly List<string> _usedUnits = new();
		static internal DateTime LastCall = DateTime.Now;
		static internal int Squads = 0;

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh()
		{
			_usedUnits.Clear();
			Squads = 0;
		}

		[EventMethod(PlayerEvents.Dead)]
		static void Dead(DeadEvent ev)
		{
			if (ev.Target.Tag.Contains(" Gunner"))
			{
				ev.Target.Tag = ev.Target.Tag.Replace(" Gunner", "");
				ev.Target.HealthInfomation.AhpActiveProcesses.ForEach(x => x.DecayRate = 0);
			}
			ev.Target.Tag = ev.Target.Tag.Replace(" Shiper", "").Replace(" Engineer", "");
		}

		[EventMethod(PlayerEvents.Attack)]
		static void Attack(AttackEvent ev)
		{
			if (ev.Attacker.Tag.Contains("Shiper") && ev.DamageType == DamageTypes.E11SR)
				ev.Damage *= 1.25f;
		}

		static public void SpawnMtf()
		{
			if (Addons.OmegaWarhead.InProgress) return;
			if (Alpha.Detonated) return;
			if ((DateTime.Now - LastCall).TotalSeconds < 30) return;
			LastCall = DateTime.Now;
			var TeamType = SpawnableTeamType.NineTailedFox;
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, TeamType);
			Timing.CallDelayed(15f, () =>
			{
				if (Alpha.Detonated) return;
				List<Player> list = Player.List.Where(x => x.RoleInfomation.Role is RoleTypeId.Spectator && !x.GamePlay.Overwatch).ToList();
				if (list.Count == 0) return;
				list.Shuffle();
				int count = 0;
				Squads++;
				if (Squads == 1)
				{
					Addons.CustomUnits.AddUnit("Reconnaissance Group", "#0089c7");
					foreach (Player pl in list)
					{
						count++;
						try
						{
							if (count == 1) SpawnFirstOne(pl, Type.Commander);
							else if (count < 7) SpawnFirstOne(pl, Type.Lieutenant);
							else SpawnFirstOne(pl, Type.Cadet);
							Timing.CallDelayed(0.4f, () =>
							{
								pl.UserInfomation.CustomInfo = "Reconnaissance Group";
								pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
								PlayerInfoArea.Role | PlayerInfoArea.PowerStatus;
							});
						}
						catch { }
					}
				}
				else if (Squads == 2)
				{
					Addons.CustomUnits.AddUnit("Emergency Group", "#ff8f00");
					foreach (Player pl in list)
					{
						count++;
						try
						{
							switch (count)
							{
								case 1: SpawnSecondOne(pl, SecondType.Commander); break;
								case 2: SpawnSecondOne(pl, SecondType.Engineer); break;
								case 3: SpawnSecondOne(pl, SecondType.Sniper); break;
								case 4: SpawnSecondOne(pl, SecondType.QuietSniper); break;
								case 5: SpawnSecondOne(pl, SecondType.Gunner); break;
								case 6: SpawnSecondOne(pl, SecondType.Physician); break;
								case 7: SpawnSecondOne(pl, SecondType.Destroyer); break;
								case < 13: SpawnSecondOne(pl, SecondType.Lieutenant); break;
								default: SpawnSecondOne(pl, SecondType.Cadet); break;
							}
						}
						catch { }
					}
				}
				else
				{
					foreach (Player pl in list)
					{
						count++;
						try
						{
							if (count == 1) SpawnOne(pl, Type.Commander);
							else if (count < 7) SpawnOne(pl, Type.Lieutenant);
							else SpawnOne(pl, Type.Cadet);
						}
						catch { }
					}
				}
				RespawnManager.Singleton.ForceSpawnTeam(TeamType);
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, TeamType);
				if (Squads == 1) Cassie.Send("ATTENTION TO ALL PERSONNEL . . arrival of the first mobile task force group at the facility");
				else if (Squads == 2) Cassie.Send("ATTENTION TO ALL PERSONNEL . . An emergency mobile task force squad arrival at the facility");
				else
				{
					string unit = "";
					do
					{
						string code = Respawning.NamingRules.NineTailedFoxNamingRule.PossibleCodes[
							UnityEngine.Random.Range(0, Respawning.NamingRules.NineTailedFoxNamingRule.PossibleCodes.Length - 1)
							];
						int number = UnityEngine.Random.Range(1, 19);
						unit = $"{code}-{number}";
					}
					while (_usedUnits.Contains(unit));
					_usedUnits.Add(unit);
					Addons.CustomUnits.AddUnit(unit, "#0074ff");
					new Respawning.NamingRules.NineTailedFoxNamingRule().PlayEntranceAnnouncement(unit);
				}
			});
		}
		static public void SpawnOne(Player pl, Type type)
		{
			SpawnManager.SpawnProtect(pl);
			switch (type)
			{
				case Type.Commander:
					pl.RoleInfomation.Role = RoleTypeId.NtfCaptain;
					break;
				case Type.Lieutenant:
					pl.RoleInfomation.Role = RoleTypeId.NtfSergeant;
					break;
				case Type.Cadet:
					pl.RoleInfomation.Role = RoleTypeId.NtfPrivate;
					break;
			}
		}
		static public void SpawnFirstOne(Player pl, Type type)
		{
			SpawnManager.SpawnProtect(pl);
			switch (type)
			{
				case Type.Commander:
					pl.RoleInfomation.Role = RoleTypeId.NtfCaptain;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFCommander);
						pl.Inventory.AddItem(ItemType.GunE11SR);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.GrenadeFlash);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Painkillers);
						pl.Inventory.AddItem(ItemType.Flashlight);
						pl.Inventory.AddItem(ItemType.ArmorCombat);
					});
					BcRec(pl, "<color=#0033ff>Captain</color>");
					break;
				case Type.Lieutenant:
					pl.RoleInfomation.Role = RoleTypeId.NtfSergeant;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunE11SR);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.GrenadeFlash);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Painkillers);
						pl.Inventory.AddItem(ItemType.Flashlight);
						pl.Inventory.AddItem(ItemType.ArmorCombat);
					});
					BcRec(pl, "<color=#0d6fff>Lieutenant</color>");
					break;
				case Type.Cadet:
					pl.RoleInfomation.Role = RoleTypeId.NtfPrivate;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunCrossvec);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.GrenadeFlash);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Painkillers);
						pl.Inventory.AddItem(ItemType.Flashlight);
						pl.Inventory.AddItem(ItemType.ArmorCombat);
					});
					BcRec(pl, "<color=#00bdff>Cadet</color>");
					break;
			}
			static void BcRec(Player pl, string role)
			{
				pl.Client.Broadcast($"<size=70%><color=#6f6f6f>You are {role} of the MTF <color=#0089c7>reconnaissance group</color>\n" +
					"Your task is to scout the situation in the area</color></size>", 10, true);
			}
		}
		static public void SpawnSecondOne(Player pl, SecondType type)
		{
			SpawnManager.SpawnProtect(pl);
			Timing.CallDelayed(0.4f, () =>
			{
				pl.UserInfomation.CustomInfo = "Emergency Group";
				pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
				PlayerInfoArea.Role | PlayerInfoArea.PowerStatus;
			});
			switch (type)
			{
				case SecondType.Commander:
					pl.RoleInfomation.Role = RoleTypeId.NtfCaptain;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFCommander);
						pl.Inventory.AddItem(ItemType.GunE11SR);
						pl.Inventory.AddItem(ItemType.SCP500);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.GrenadeHE);
						pl.Inventory.AddItem(ItemType.Adrenaline);
						pl.Inventory.AddItem(ItemType.Flashlight);
						pl.Inventory.AddItem(ItemType.ArmorCombat);
						pl.UserInfomation.CustomInfo = "Captain | Emergency Group";
					});
					BcRec(pl, "<color=#0033ff>Captain</color>", "issue high-level orders");
					break;
				case SecondType.Engineer:
					pl.RoleInfomation.Role = RoleTypeId.NtfSpecialist;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardContainmentEngineer);
						pl.Inventory.AddItem(ItemType.GunCrossvec);
						pl.Inventory.AddItem(ItemType.Adrenaline);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.GrenadeFlash);
						pl.Inventory.AddItem(ItemType.Flashlight);
						pl.Inventory.AddItem(ItemType.ArmorHeavy);
						pl.Tag = pl.Tag.Replace(" Engineer", "") + " Engineer";
						pl.UserInfomation.CustomInfo = "Engineer | Emergency Group";
					});
					BcRec(pl, "<color=#ff4640>Engineer</color>", "fix the malfunctions in the area");
					break;
				case SecondType.Sniper:
					pl.RoleInfomation.Role = RoleTypeId.NtfSpecialist;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunE11SR);//, 40, 4, 3, 1);
						pl.Inventory.AddItem(ItemType.GrenadeFlash);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.Adrenaline);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.ArmorHeavy);
						pl.Tag = pl.Tag.Replace(" Shiper", "") + " Shiper";
						pl.UserInfomation.CustomInfo = "Sniper | Emergency Group";
					});
					BcRec(pl, "<color=#94ff00>Sniper</color>", "eliminate distant targets that ordinary bullets don't reach");
					break;
				case SecondType.QuietSniper:
					pl.RoleInfomation.Role = RoleTypeId.NtfSpecialist;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunE11SR);//, 40, 3, 1, 3);
						pl.Inventory.AddItem(ItemType.SCP207);
						pl.Inventory.AddItem(ItemType.GrenadeFlash);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.Adrenaline);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.ArmorHeavy);
						pl.Tag = pl.Tag.Replace(" Shiper", "") + " Shiper";
						pl.UserInfomation.CustomInfo = "Silent Sniper | Emergency Group";
					});
					BcRec(pl, "<color=#415261>Silent</color> <color=#94ff00>Sniper</color>", "eliminate the targets as unnoticed as possible");
					break;
				case SecondType.Gunner:
					pl.RoleInfomation.Role = RoleTypeId.NtfSpecialist;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunLogicer);
						pl.Inventory.AddItem(ItemType.SCP500);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.Adrenaline);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Flashlight);
						pl.Inventory.AddItem(ItemType.ArmorHeavy);
						pl.HealthInfomation.AhpActiveProcesses.ForEach(x => x.DecayRate = 0);
						pl.HealthInfomation.MaxAhp = 250;
						pl.HealthInfomation.Ahp = 250;
						pl.Tag = pl.Tag.Replace(" Gunner", "") + " Gunner";
						pl.UserInfomation.CustomInfo = "Gunner | Emergency Group";
					});
					BcRec(pl, "<color=#0ac067>Gunner</color>", "eliminate targets at near range");
					break;
				case SecondType.Physician:
					pl.RoleInfomation.Role = RoleTypeId.NtfSpecialist;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunAK);
						pl.Inventory.AddItem(ItemType.SCP500);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Medkit);
						pl.Inventory.AddItem(ItemType.Adrenaline);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.UserInfomation.CustomInfo = "Doctor | Emergency Group";
					});
					BcRec(pl, "<color=#ff2222>Doctor</color>", "heal allies");
					break;
				case SecondType.Destroyer:
					pl.RoleInfomation.Role = RoleTypeId.NtfSpecialist;
					Timing.CallDelayed(0.5f, () =>
					{
						pl.Inventory.Clear();
						pl.GetAmmo();
						pl.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
						pl.Inventory.AddItem(ItemType.GunLogicer);
						pl.Inventory.AddItem(ItemType.GrenadeHE);
						pl.Inventory.AddItem(ItemType.GrenadeHE);
						pl.Inventory.AddItem(ItemType.GrenadeHE);
						pl.Inventory.AddItem(ItemType.GrenadeHE);
						pl.Inventory.AddItem(ItemType.Radio);
						pl.Inventory.AddItem(ItemType.ArmorHeavy);
						pl.UserInfomation.CustomInfo = "Destroyer | Emergency Group";
					});
					BcRec(pl, "<color=#ff3b00>Destroyer</color>", "destroy targets with a high level of health");
					break;
				case SecondType.Lieutenant:
					pl.RoleInfomation.Role = RoleTypeId.NtfSergeant;
					Timing.CallDelayed(0.5f, () => pl.UserInfomation.CustomInfo = "Lieutenant | Emergency Group");
					BcRec(pl, "<color=#0d6fff>Lieutenant</color>", "carry out the orders of the highest rank");
					break;
				case SecondType.Cadet:
					pl.RoleInfomation.Role = RoleTypeId.NtfPrivate;
					Timing.CallDelayed(0.5f, () => pl.UserInfomation.CustomInfo = "Cadet | Emergency Group");
					BcRec(pl, "<color=#00bdff>Cadet</color>", "carry out the orders of the highest rank");
					break;
			}
			static void BcRec(Player pl, string role, string desc)
			{
				pl.Client.Broadcast($"<size=70%><color=#6f6f6f>You are {role} of the MTF <color=#ff8f00>Emergency</color> <color=#0089c7>Group</color>\n" +
					$"Your task is to {desc}</color></size>", 10, true);
			}
		}
		public enum Type : byte
		{
			Commander,
			Lieutenant,
			Cadet,
		}
		public enum SecondType : byte
		{
			Commander,
			Engineer,
			Sniper,
			QuietSniper,
			Gunner,
			Physician,
			Destroyer,
			Lieutenant,
			Cadet,
		}
	}
}