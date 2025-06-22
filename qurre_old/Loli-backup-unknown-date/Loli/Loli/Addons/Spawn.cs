using Loli.Scps.Api;
using MEC;
using Qurre.API.Events;
using System.Linq;
namespace Loli.Addons
{
	public class Spawn
	{
		public void Update(SpawnEvent ev)
		{
			if (ev.RoleType != RoleType.Scp93953 && ev.RoleType != RoleType.Scp93989) ev.Player.DisableAllEffects();
			ev.Player.NicknameSync.Network_customPlayerInfoString = "";
			ev.Player.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | 
				PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
			if (Plugin.ClansWars) return;
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (ev.Player.GetCustomRole() == Module.RoleType.Scp035)
			{
				ev.Player.MaxHp = (int)ev.Player.GetMaxHp();
				return;
			}
			Timing.CallDelayed(0.2f, () =>
			{
				var hp = ev.Player.GetMaxHp();
				ev.Player.MaxHp = (int)hp;
				ev.Player.Hp = hp;
				if (ev.Player.Role == RoleType.ClassD)
				{
					if (Plugin.RolePlay)
					{
						if (ev.Player.AllItems.Where(x => x.Type == ItemType.Flashlight).Count() == 0)
							ev.Player.AddItem(ItemType.Flashlight);
					}
					else
					{
						ev.Player.ClearInventory();
						ev.Player.GetAmmo();
						ev.Player.AddItem(ItemType.KeycardJanitor);
						ev.Player.AddItem(ItemType.Flashlight);
					}
				}
				else if (ev.Player.Role == RoleType.ChaosConscript)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardChaosInsurgency);
					ev.Player.AddItem(ItemType.GunAK);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Painkillers);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorCombat);
				}
				else if (ev.Player.Role == RoleType.ChaosMarauder)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardChaosInsurgency);
					ev.Player.AddItem(ItemType.GunLogicer);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Adrenaline);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorHeavy);
				}
				else if (ev.Player.Role == RoleType.ChaosRepressor)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardChaosInsurgency);
					ev.Player.AddItem(ItemType.GunShotgun);
					ev.Player.AddItem(ItemType.GunRevolver);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Painkillers);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorCombat);
				}
				else if (ev.Player.Role == RoleType.ChaosRifleman)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardChaosInsurgency);
					ev.Player.AddItem(ItemType.GunAK);
					ev.Player.AddItem(ItemType.GunCOM18);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Painkillers);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorCombat);
				}
				else if (ev.Player.Role == RoleType.NtfPrivate)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardNTFLieutenant);
					ev.Player.AddItem(ItemType.GunCrossvec);
					ev.Player.AddItem(ItemType.Painkillers);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorCombat);
				}
				else if (ev.Player.Role == RoleType.NtfCaptain)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardNTFCommander);
					ev.Player.AddItem(ItemType.GunRevolver);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorCombat);
				}
				else if (ev.Player.Role == RoleType.NtfSergeant)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardNTFLieutenant);
					ev.Player.AddItem(ItemType.GunCOM18);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorCombat);
				}
				else if (ev.Player.Role == RoleType.NtfSpecialist)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					ev.Player.AddItem(ItemType.KeycardNTFCommander);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Player.AddItem(ItemType.GunRevolver);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorHeavy);
				}
				else if (ev.Player.Role == RoleType.Scientist)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					if (!Plugin.RolePlay) ev.Player.AddItem(ItemType.KeycardScientist);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.Role == RoleType.FacilityGuard)
				{
					ev.Player.ClearInventory();
					ev.Player.GetAmmo();
					var card = ItemType.KeycardNTFOfficer;
					if (Plugin.RolePlay) card = ItemType.KeycardGuard;
					ev.Player.AddItem(card);
					ev.Player.AddItem(ItemType.GunFSP9);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeHE);
					ev.Player.AddItem(ItemType.Flashlight);
					ev.Player.AddItem(ItemType.ArmorLight);
				}

				else if (ev.Player.Role == RoleType.Scp173)
				{
					ev.Player.Scp173Controller.IgnoredPlayers.Clear();
					foreach (var pl in Spawns.SerpentsHand.SerpentsHands) try { ev.Player.Scp173Controller.IgnoredPlayers.Add(pl); } catch { }
					foreach (var pl in Scp035.Get035()) try { ev.Player.Scp173Controller.IgnoredPlayers.Add(pl); } catch { }
				}
			});
		}
	}
}