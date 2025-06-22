using Exiled.Events.EventArgs;
using MEC;

namespace PlayerXP.cspawn
{
    public class spawn
	{
		private readonly Plugin plugin;
		public spawn(Plugin plugin) => this.plugin = plugin;
		public void nspawn(SpawningEventArgs ev)
		{
			Timing.CallDelayed(0.1f, () =>
			{
				if (ev.Player.ReferenceHub.GetRole() == RoleType.ClassD)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 130;
					ev.Player.ReferenceHub.playerStats.Health = 130;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardJanitor);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.ChaosInsurgency)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 175;
					ev.Player.ReferenceHub.playerStats.Health = 175;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardChaosInsurgency);
					ev.Player.AddItem(ItemType.GunLogicer);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Painkillers);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.NtfCadet)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 125;
					ev.Player.ReferenceHub.playerStats.Health = 125;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardNTFLieutenant);
					ev.Player.AddItem(ItemType.GunProject90);
					ev.Player.AddItem(ItemType.WeaponManagerTablet);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.NtfCommander)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 175;
					ev.Player.ReferenceHub.playerStats.Health = 175;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardNTFCommander);
					ev.Player.AddItem(ItemType.GunUSP);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Player.AddItem(ItemType.Adrenaline);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.NtfLieutenant)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 150;
					ev.Player.ReferenceHub.playerStats.Health = 150;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardNTFLieutenant);
					ev.Player.AddItem(ItemType.GunUSP);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Player.AddItem(ItemType.Adrenaline);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.NtfScientist)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 200;
					ev.Player.ReferenceHub.playerStats.Health = 200;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardNTFCommander);
					ev.Player.AddItem(ItemType.MicroHID);
					ev.Player.AddItem(ItemType.GunE11SR);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Player.AddItem(ItemType.Adrenaline);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.Scientist)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 150;
					ev.Player.ReferenceHub.playerStats.Health = 150;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardScientist);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.Flashlight);
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.FacilityGuard)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 150;
					ev.Player.ReferenceHub.playerStats.Health = 150;
					ev.Player.ClearInventory();
					ev.Player.AddItem(ItemType.KeycardSeniorGuard);
					ev.Player.AddItem(ItemType.GunProject90);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Medkit);
					ev.Player.AddItem(ItemType.Radio);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					ev.Player.AddItem(ItemType.Disarmer);
					ev.Player.AddItem(ItemType.Flashlight);
				}


				else if (ev.Player.ReferenceHub.GetRole() == RoleType.Scp106)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 1000;
					ev.Player.ReferenceHub.playerStats.Health = 1000;
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.Scp049)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 2000;
					ev.Player.ReferenceHub.playerStats.Health = 2000;
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.Scp049)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 2000;
					ev.Player.ReferenceHub.playerStats.Health = 2000;
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.Scp0492)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 1000;
					ev.Player.ReferenceHub.playerStats.Health = 1000;
				}
				else if (ev.Player.ReferenceHub.GetRole() == RoleType.Scp93953 || ev.Player.ReferenceHub.GetRole() == RoleType.Scp93989)
				{
					ev.Player.ReferenceHub.playerStats.maxHP = 3000;
					ev.Player.ReferenceHub.playerStats.Health = 3000;
				}
			});
		}
	}
}
