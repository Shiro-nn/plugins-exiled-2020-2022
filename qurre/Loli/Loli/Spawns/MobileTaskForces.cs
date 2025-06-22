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
		static internal DateTime LastCall = DateTime.Now;
		static internal int Squads = 0;

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh()
		{
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
					Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#0089c7>Разведгруппа</color>");
					foreach (Player pl in list)
					{
						count++;
						try
						{
							if (count == 1) SpawnFirstOne(pl, Type.Commander);
							else if (count < 7) SpawnFirstOne(pl, Type.Lieutenant);
							else SpawnFirstOne(pl, Type.Cadet);
							pl.UnitName = "<color=#0089c7>Разведгруппа</color>";
						}
						catch { }
					}
				}
				else if (Squads == 2)
				{
					Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#ff8f00>Аварийный отряд</color>");
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
				if (Squads == 1) Cassie.Send("ATTENTION TO ALL PERSONNEL . . arrival of the first mobile task force group at the facility");
				else if (Squads == 2) Cassie.Send("ATTENTION TO ALL PERSONNEL . . An emergency mobile task force squad arrival at the facility");
				else
				{
					if (Respawning.NamingRules.UnitNamingRules.TryGetNamingRule(TeamType, out Respawning.NamingRules.UnitNamingRule unitNamingRule))
					{
						unitNamingRule.GenerateNew(TeamType, out string text);
						foreach (Player pl in list)
						{
							pl.ClassManager.NetworkCurSpawnableTeamType = (byte)TeamType;
							pl.ClassManager.NetworkCurUnitName = text;
						}
						unitNamingRule.PlayEntranceAnnouncement(text);
					}
				}
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, TeamType);
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
					BcRec(pl, "<color=#0033ff>Командир</color>");
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
					BcRec(pl, "<color=#0d6fff>Лейтенант</color>");
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
					BcRec(pl, "<color=#00bdff>Кадет</color>");
					break;
			}
			static void BcRec(Player pl, string umm)
			{
				pl.Client.Broadcast($"<size=30%><color=#6f6f6f>Вы - {umm} <color=#0089c7>разведгруппы <color=#0047ec>МОГ</color></color>\n" +
					"Ваша задача - разведать ситуацию в комплексе.</color></size>", 10, true);
			}
		}
		static public void SpawnSecondOne(Player pl, SecondType type)
		{
			SpawnManager.SpawnProtect(pl);
			Timing.CallDelayed(0.5f, () => pl.UnitName = "<color=#ff8f00>Аварийный отряд</color>");
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
					});
					BcRec(pl, "<color=#0033ff>Командир</color>", "отдавать высокоуровневые приказы");
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
						//pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff4640>Инженер</color>";
						pl.UserInfomation.CustomInfo = "Инженер";
						pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					});
					BcRec(pl, "<color=#ff4640>Инженер</color>", "починить неисправности в комплексе");
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
						//pl.NicknameSync.Network_customPlayerInfoString = "<color=#94ff00>Снайпер</color>";
						pl.UserInfomation.CustomInfo = "Снайпер";
						pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					});
					BcRec(pl, "<color=#94ff00>Снайпер</color>", "устранить дальние цели, до которых не долетают обычные пули");
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
						//pl.NicknameSync.Network_customPlayerInfoString = "<color=#415261>Бесшумный</color> <color=#94ff00>Снайпер</color>";
						pl.UserInfomation.CustomInfo = "Бесшумный Снайпер";
						pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					});
					BcRec(pl, "<color=#415261>Бесшумный</color> <color=#94ff00>Снайпер</color>", "устранить цели максимально незаметно");
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
						//pl.NicknameSync.Network_customPlayerInfoString = "<color=#0ac067>Пулеметчик</color>";
						pl.UserInfomation.CustomInfo = "Пулеметчик";
						pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					});
					BcRec(pl, "<color=#0ac067>Пулеметчик</color>", "устранять цели на ближней дистанции");
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
						//pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff2222>Врач</color>";
						pl.UserInfomation.CustomInfo = "Врач";
						pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					});
					BcRec(pl, "<color=#ff2222>Врач</color>", "лечить союзников");
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
						//pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff3b00>Разрушитель</color>";
						pl.UserInfomation.CustomInfo = "Разрушитель";
						pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					});
					BcRec(pl, "<color=#ff3b00>Разрушитель</color>", "уничтожить цели с высоким уровнем защиты");
					break;
				case SecondType.Lieutenant:
					pl.RoleInfomation.Role = RoleTypeId.NtfSergeant;
					pl.UserInfomation.CustomInfo = "Лейтенант";
					pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					BcRec(pl, "<color=#0d6fff>Лейтенант</color>", "исполнять приказы высших по рангу");
					break;
				case SecondType.Cadet:
					pl.RoleInfomation.Role = RoleTypeId.NtfPrivate;
					pl.UserInfomation.CustomInfo = "Кадет";
					pl.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
						PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
					BcRec(pl, "<color=#00bdff>Кадет</color>", "исполнять приказы высших по рангу");
					break;
			}
			static void BcRec(Player pl, string umm, string desc)
			{
				pl.Client.Broadcast($"<size=30%><color=#6f6f6f>Вы - {umm} <color=#ff8f00>аварийного</color> <color=#0089c7>отряда</color> <color=#0047ec>МОГ</color>\n" +
					$"Ваша задача - {desc}.</color></size>", 10, true);
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