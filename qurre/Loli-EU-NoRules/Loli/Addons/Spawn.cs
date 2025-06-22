using MEC;
using PlayerRoles;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Addons
{
	static class Spawn
	{
		[EventMethod(PlayerEvents.Spawn)]
		static void Update(SpawnEvent ev)
		{
			if (ev.Role is not RoleTypeId.Scp939)
				ev.Player.Effects.DisableAll();

			ev.Player.UserInfomation.CustomInfo = "";
			ev.Player.UserInfomation.InfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo |
				PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;

			Timing.CallDelayed(0.2f, () =>
			{
				var hp = ev.Player.GetMaxHp();
				if(hp > 0)
				{
					ev.Player.HealthInfomation.MaxHp = (int)hp;
					ev.Player.HealthInfomation.Hp = hp;
				}

				switch (ev.Player.RoleInfomation.Role)
				{
					case RoleTypeId.Tutorial: break;
					case RoleTypeId.ClassD:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardJanitor);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							break;
						}
					case RoleTypeId.Scientist:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardScientist);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Radio);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							break;
						}
					case RoleTypeId.FacilityGuard:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardNTFOfficer);
							ev.Player.Inventory.AddItem(ItemType.GunFSP9);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Radio);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorLight);
							break;
						}
					case RoleTypeId.ChaosConscript:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
							ev.Player.Inventory.AddItem(ItemType.GunAK);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Painkillers);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.GrenadeFlash);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorCombat);
							break;
						}
					case RoleTypeId.ChaosMarauder:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
							ev.Player.Inventory.AddItem(ItemType.GunLogicer);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Adrenaline);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorHeavy);
							break;
						}
					case RoleTypeId.ChaosRepressor:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
							ev.Player.Inventory.AddItem(ItemType.GunShotgun);
							ev.Player.Inventory.AddItem(ItemType.GunRevolver);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Painkillers);
							ev.Player.Inventory.AddItem(ItemType.GrenadeFlash);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorCombat);
							break;
						}
					case RoleTypeId.ChaosRifleman:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardChaosInsurgency);
							ev.Player.Inventory.AddItem(ItemType.GunAK);
							ev.Player.Inventory.AddItem(ItemType.GunCOM18);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Painkillers);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorCombat);
							break;
						}
					case RoleTypeId.NtfPrivate:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
							ev.Player.Inventory.AddItem(ItemType.GunCrossvec);
							ev.Player.Inventory.AddItem(ItemType.Painkillers);
							ev.Player.Inventory.AddItem(ItemType.Radio);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.GrenadeFlash);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorCombat);
							break;
						}
					case RoleTypeId.NtfCaptain:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardNTFCommander);
							ev.Player.Inventory.AddItem(ItemType.GunRevolver);
							ev.Player.Inventory.AddItem(ItemType.GunE11SR);
							ev.Player.Inventory.AddItem(ItemType.Radio);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorCombat);
							break;
						}
					case RoleTypeId.NtfSergeant:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardNTFLieutenant);
							ev.Player.Inventory.AddItem(ItemType.GunCOM18);
							ev.Player.Inventory.AddItem(ItemType.GunE11SR);
							ev.Player.Inventory.AddItem(ItemType.Radio);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorCombat);
							break;
						}
					case RoleTypeId.NtfSpecialist:
						{
							ev.Player.Inventory.Clear();
							ev.Player.GetAmmo();
							ev.Player.Inventory.AddItem(ItemType.KeycardNTFCommander);
							ev.Player.Inventory.AddItem(ItemType.GunE11SR);
							ev.Player.Inventory.AddItem(ItemType.SCP330);
							ev.Player.Inventory.AddItem(ItemType.Radio);
							ev.Player.Inventory.AddItem(ItemType.GrenadeHE);
							ev.Player.Inventory.AddItem(ItemType.Medkit);
							ev.Player.Inventory.AddItem(ItemType.Flashlight);
							ev.Player.Inventory.AddItem(ItemType.ArmorHeavy);
							break;
						}
				}
			});
		}
	}
}