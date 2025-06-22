using MEC;
using Qurre.API.Events;
namespace ClassicCore.Addons
{
	static internal class Spawn
	{
		static internal void Update(SpawnEvent ev)
		{
			if (ev.RoleType is not RoleType.Scp93953 and not RoleType.Scp93989) ev.Player.DisableAllEffects();
			ev.Player.NicknameSync.Network_customPlayerInfoString = "";
			ev.Player.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
				PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
			Timing.CallDelayed(0.2f, () =>
			{
				var hp = ev.Player.GetMaxHp();
				ev.Player.MaxHp = (int)hp;
				ev.Player.Hp = hp;
				switch (ev.Player.Role)
				{
					case RoleType.Tutorial: break;
					case RoleType.ClassD:
						{
							ev.Player.ClearInventory();
							ev.Player.GetAmmo();
							ev.Player.AddItem(ItemType.KeycardJanitor);
							ev.Player.AddItem(ItemType.Flashlight);
							break;
						}
					case RoleType.Scientist:
						{
							ev.Player.ClearInventory();
							ev.Player.GetAmmo();
							ev.Player.AddItem(ItemType.KeycardScientist);
							ev.Player.AddItem(ItemType.Medkit);
							ev.Player.AddItem(ItemType.Radio);
							ev.Player.AddItem(ItemType.Flashlight);
							break;
						}
					case RoleType.FacilityGuard:
						{
							ev.Player.ClearInventory();
							ev.Player.GetAmmo();
							ev.Player.AddItem(ItemType.KeycardNTFOfficer);
							ev.Player.AddItem(ItemType.GunFSP9);
							ev.Player.AddItem(ItemType.Medkit);
							ev.Player.AddItem(ItemType.Medkit);
							ev.Player.AddItem(ItemType.Radio);
							ev.Player.AddItem(ItemType.GrenadeHE);
							ev.Player.AddItem(ItemType.Flashlight);
							ev.Player.AddItem(ItemType.ArmorLight);
							break;
						}
					case RoleType.ChaosConscript:
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
							break;
						}
					case RoleType.ChaosMarauder:
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
							break;
						}
					case RoleType.ChaosRepressor:
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
							break;
						}
					case RoleType.ChaosRifleman:
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
							break;
						}
					case RoleType.NtfPrivate:
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
							break;
						}
					case RoleType.NtfCaptain:
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
							break;
						}
					case RoleType.NtfSergeant:
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
							break;
						}
					case RoleType.NtfSpecialist:
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
							break;
						}
				}
			});
		}
	}
}