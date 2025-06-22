using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Loli.Spawns
{
	public class MobileTaskForces
	{
		private readonly SpawnManager SpawnManager;
		public MobileTaskForces(SpawnManager SpawnManager) => this.SpawnManager = SpawnManager;
		internal int Squads = 0;
		internal void Refresh()
		{
			Squads = 0;
		}
		internal void Dead(DeadEvent ev)
		{
			if (ev.Target.Tag.Contains(" Gunner"))
			{
				ev.Target.Tag = ev.Target.Tag.Replace(" Gunner", "");
				ev.Target.AhpActiveProcesses.ForEach(x => x.DecayRate = 0);
			}
			ev.Target.Tag = ev.Target.Tag.Replace(" Shiper", "").Replace(" Engineer", "");
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (ev.Attacker.Tag.Contains("Shiper") && ev.DamageType == DamageTypes.E11SR)
				ev.Amount *= 1.25f;
		}
		internal DateTime LastCall = DateTime.Now;
		public void SpawnMtf()
		{
			if (Alpha.Detonated) return;
			if (Plugin.ClansWars) return;
			if (Round.ElapsedTime.TotalSeconds > Addons.RolePlay.Modules.WaitingSweep) return;
			if ((DateTime.Now - LastCall).TotalSeconds < 30) return;
			LastCall = DateTime.Now;
			var TeamType = SpawnableTeamType.NineTailedFox;
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, TeamType);
			Timing.CallDelayed(15f, () =>
			{
				if (Alpha.Detonated) return;
				List<Player> list = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToList();
				list.Shuffle();
				if (list.Count == 0) return;
				int count = 0;
				Squads++;
				if (Squads == 1)
				{
					Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#0089c7>Разведгруппа</color>");
					foreach (Player pl in list)
					{
						count++;
						if (count == 1) SpawnFirstOne(pl, Type.Commander);
						else if (count < 7) SpawnFirstOne(pl, Type.Lieutenant);
						else SpawnFirstOne(pl, Type.Cadet);
						pl.UnitName = "<color=#0089c7>Разведгруппа</color>";
					}
				}
				else if (Squads == 2)
				{
					Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#ff8f00>Аварийный отряд</color>");
					foreach (Player pl in list)
					{
						count++;
						if (count == 1) SpawnSecondOne(pl, SecondType.Commander);
						else if (count == 2) SpawnSecondOne(pl, SecondType.Engineer);
						else if (count == 3) SpawnSecondOne(pl, SecondType.Sniper);
						else if (count == 4) SpawnSecondOne(pl, SecondType.QuietSniper);
						else if (count == 5) SpawnSecondOne(pl, SecondType.Gunner);
						else if (count == 6) SpawnSecondOne(pl, SecondType.Physician);
						else if (count == 7) SpawnSecondOne(pl, SecondType.Destroyer);
						else if (count < 13) SpawnSecondOne(pl, SecondType.Lieutenant);
						else SpawnSecondOne(pl, SecondType.Cadet);
					}
				}
				else
				{
					foreach (Player pl in list)
					{
						count++;
						if (count == 1) SpawnOne(pl, Type.Commander);
						else if (count < 7) SpawnOne(pl, Type.Lieutenant);
						else SpawnOne(pl, Type.Cadet);
					}
				}
				Qurre.Events.Invoke.Round.TeamRespawn(new TeamRespawnEvent(list, count, TeamType));
				RespawnManager.Singleton.ForceSpawnTeam(TeamType);
				if (Squads == 1) Cassie.Send("ATTENTION TO ALL PERSONNEL . . arrival of the first mobile task force group at the complex");
				else if (Squads == 2) Cassie.Send("ATTENTION TO ALL PERSONNEL . . An emergency mobile task force squad arrival at the complex");
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
		public void SpawnOne(Player pl, Type type)
		{
			SpawnManager.SpawnProtect(pl);
			if (type == Type.Commander) pl.SetRole(RoleType.NtfCaptain);
			else if (type == Type.Lieutenant) pl.SetRole(RoleType.NtfSergeant);
			else if (type == Type.Cadet) pl.SetRole(RoleType.NtfPrivate);
		}
		public void SpawnFirstOne(Player pl, Type type)
		{
			SpawnManager.SpawnProtect(pl);
			if (type == Type.Commander)
			{
				pl.SetRole(RoleType.NtfCaptain);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFCommander);
					pl.AddItem(ItemType.GunE11SR);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.GrenadeFlash);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Painkillers);
					pl.AddItem(ItemType.Flashlight);
					pl.AddItem(ItemType.ArmorCombat);
				});
				BcRec(pl, "<color=#0033ff>Командир</color>");
			}
			else if (type == Type.Lieutenant)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunE11SR);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.GrenadeFlash);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Painkillers);
					pl.AddItem(ItemType.Flashlight);
					pl.AddItem(ItemType.ArmorCombat);
				});
				BcRec(pl, "<color=#0d6fff>Лейтенант</color>");
			}
			else if (type == Type.Cadet)
			{
				pl.SetRole(RoleType.NtfPrivate);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunCrossvec);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.GrenadeFlash);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Painkillers);
					pl.AddItem(ItemType.Flashlight);
					pl.AddItem(ItemType.ArmorCombat);
				});
				BcRec(pl, "<color=#00bdff>Кадет</color>");
			}
			void BcRec(Player pl, string umm)
			{
				pl.Broadcast($"<size=30%><color=#6f6f6f>Вы - {umm} <color=#0089c7>разведгруппы <color=#0047ec>МОГ</color></color>\n" +
					"Ваша задача - разведать ситуацию в комплексе.</color></size>", 10, true);
			}
		}
		public void SpawnSecondOne(Player pl, SecondType type)
		{
			SpawnManager.SpawnProtect(pl);
			Timing.CallDelayed(0.5f, () => pl.UnitName = "<color=#ff8f00>Аварийный отряд</color>");
			if (type == SecondType.Commander)
			{
				pl.SetRole(RoleType.NtfCaptain);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFCommander);
					pl.AddItem(ItemType.GunE11SR);
					pl.AddItem(ItemType.SCP500);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.GrenadeHE);
					pl.AddItem(ItemType.Adrenaline);
					pl.AddItem(ItemType.Flashlight);
					pl.AddItem(ItemType.ArmorCombat);
				});
				BcRec(pl, "<color=#0033ff>Командир</color>", "отдавать высокоуровневые приказы");
			}
			else if (type == SecondType.Engineer)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardContainmentEngineer);
					pl.AddItem(ItemType.GunCrossvec);
					pl.AddItem(ItemType.Adrenaline);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.GrenadeFlash);
					pl.AddItem(ItemType.Flashlight);
					pl.AddItem(ItemType.ArmorHeavy);
					pl.Tag = pl.Tag.Replace(" Engineer", "") + " Engineer";
					//pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff4640>Инженер</color>";
					pl.NicknameSync.Network_customPlayerInfoString = "Инженер";
					pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				});
				BcRec(pl, "<color=#ff4640>Инженер</color>", "починить неисправности в комплексе");
			}
			else if (type == SecondType.Sniper)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunE11SR);//, 40, 4, 3, 1);
					pl.AddItem(ItemType.GrenadeFlash);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.Adrenaline);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.ArmorHeavy);
					pl.Tag = pl.Tag.Replace(" Shiper", "") + " Shiper";
					//pl.NicknameSync.Network_customPlayerInfoString = "<color=#94ff00>Снайпер</color>";
					pl.NicknameSync.Network_customPlayerInfoString = "Снайпер";
					pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				});
				BcRec(pl, "<color=#94ff00>Снайпер</color>", "устранить дальние цели, до которых не долетают обычные пули");
			}
			else if (type == SecondType.QuietSniper)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunE11SR);//, 40, 3, 1, 3);
					pl.AddItem(ItemType.SCP268);
					pl.AddItem(ItemType.GrenadeFlash);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.Adrenaline);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.ArmorHeavy);
					pl.Tag = pl.Tag.Replace(" Shiper", "") + " Shiper";
					//pl.NicknameSync.Network_customPlayerInfoString = "<color=#415261>Бесшумный</color> <color=#94ff00>Снайпер</color>";
					pl.NicknameSync.Network_customPlayerInfoString = "Бесшумный Снайпер";
					pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				});
				BcRec(pl, "<color=#415261>Бесшумный</color> <color=#94ff00>Снайпер</color>", "устранить цели максимально незаметно");
			}
			else if (type == SecondType.Gunner)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunLogicer);
					pl.AddItem(ItemType.SCP500);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.Adrenaline);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Flashlight);
					pl.AddItem(ItemType.ArmorHeavy);
					pl.AhpActiveProcesses.ForEach(x => x.DecayRate = 0);
					pl.MaxAhp = 250;
					pl.Ahp = 250;
					pl.Tag = pl.Tag.Replace(" Gunner", "") + " Gunner";
					//pl.NicknameSync.Network_customPlayerInfoString = "<color=#0ac067>Пулеметчик</color>";
					pl.NicknameSync.Network_customPlayerInfoString = "Пулеметчик";
					pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				});
				BcRec(pl, "<color=#0ac067>Пулеметчик</color>", "устранять цели на ближней дистанции");
			}
			else if (type == SecondType.Physician)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunAK);
					pl.AddItem(ItemType.SCP500);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Medkit);
					pl.AddItem(ItemType.Adrenaline);
					pl.AddItem(ItemType.Radio);
					//pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff2222>Врач</color>";
					pl.NicknameSync.Network_customPlayerInfoString = "Врач";
					pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				});
				BcRec(pl, "<color=#ff2222>Врач</color>", "лечить союзников");
			}
			else if (type == SecondType.Destroyer)
			{
				pl.SetRole(RoleType.NtfSergeant);
				Timing.CallDelayed(0.5f, () =>
				{
					pl.ClearInventory();
					pl.GetAmmo();
					pl.AddItem(ItemType.KeycardNTFLieutenant);
					pl.AddItem(ItemType.GunLogicer);
					pl.AddItem(ItemType.GrenadeHE);
					pl.AddItem(ItemType.GrenadeHE);
					pl.AddItem(ItemType.GrenadeHE);
					pl.AddItem(ItemType.GrenadeHE);
					pl.AddItem(ItemType.Radio);
					pl.AddItem(ItemType.ArmorHeavy);
					//pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff3b00>Разрушитель</color>";
					pl.NicknameSync.Network_customPlayerInfoString = "Разрушитель";
					pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				});
				BcRec(pl, "<color=#ff3b00>Разрушитель</color>", "уничтожить цели с высоким уровнем защиты");
			}
			else if (type == SecondType.Lieutenant)
			{
				pl.SetRole(RoleType.NtfSergeant);
				pl.NicknameSync.Network_customPlayerInfoString = "Лейтенант";
				pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
				PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				BcRec(pl, "<color=#0d6fff>Лейтенант</color>", "исполнять приказы высших по рангу");
			}
			else if (type == SecondType.Cadet)
			{
				pl.SetRole(RoleType.NtfPrivate);
				pl.NicknameSync.Network_customPlayerInfoString = "Кадет";
				pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | 
					PlayerInfoArea.Role | PlayerInfoArea.PowerStatus | PlayerInfoArea.UnitName;
				BcRec(pl, "<color=#00bdff>Кадет</color>", "исполнять приказы высших по рангу");
			}
			void BcRec(Player pl, string umm, string desc)
			{
				pl.Broadcast($"<size=30%><color=#6f6f6f>Вы - {umm} <color=#ff8f00>аварийного</color> <color=#0089c7>отряда</color> <color=#0047ec>МОГ</color>\n" +
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