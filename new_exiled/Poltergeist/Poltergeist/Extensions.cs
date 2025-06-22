using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using MEC;
using EXILED.Extensions;
using EXILED;
namespace Poltergeist
{
	public static class Extensions
	{
		public static List<Pickup> haveBeenMoved = new List<Pickup>();
		public static List<Pickup> canBeFloating = new List<Pickup>();
		public static float maxOffset = 2.0f;
		public static float minForce = 0.0f;
		public static float maxForce = 100.0f;
		public static void Postfix(Pickup instance)
		{
			instance.Rb.useGravity = false;
			instance.GetComponent<Collider>().material = new PhysicMaterial() { bounciness = 100f, bounceCombine = PhysicMaterialCombine.Maximum };
			float rng = UnityEngine.Random.Range(0.1f, 0.5f);
			//float rng = UnityEngine.Random.Range(minForce, minForce);
			instance.Rb.AddForceAtPosition(Vector3.up * 40f, Vector3.up * 40f);
			Timing.CallDelayed(15f, () => {
				instance.Rb.useGravity = true;
			});
			Timing.CallDelayed(30f, () => {
				instance.Rb.AddForceAtPosition(Vector3.up * 140f, Vector3.up * 140f);
				instance.Rb.AddForceAtPosition(Vector3.left * 140f, Vector3.left * 240f);
			});
			Timing.CallDelayed(40f, () => {
				instance.Rb.AddForceAtPosition(Vector3.up * 540f, Vector3.up * 540f);
			});
		}
		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}
		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		public static bool HasLightSource(this ReferenceHub rh)
		{
			if (rh.inventory != null && rh.inventory.curItem == ItemType.Flashlight || EventHandlers.dd)
				return true;
			if (rh.inventory == null || rh.weaponManager == null || !rh.weaponManager.NetworksyncFlash ||
				rh.weaponManager.curWeapon < 0 ||
				rh.weaponManager.curWeapon >= rh.weaponManager.weapons.Length) return false;
			WeaponManager.Weapon weapon = rh.weaponManager.weapons[rh.weaponManager.curWeapon];
			Inventory.SyncItemInfo itemInHand = rh.inventory.GetItemInHand();
			if (weapon == null || itemInHand.modOther < 0 || itemInHand.modOther >= weapon.mod_others.Length)
				return false;
			WeaponManager.Weapon.WeaponMod modOther = weapon.mod_others[itemInHand.modOther];
			if (modOther != null && !string.IsNullOrEmpty(modOther.name) && (modOther.name.ToLower().Contains("flashlight") || modOther.name.Contains("night")))
				return true;
			return false;
		}
		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}
		public static void Checkopen()
		{
			foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				if (true)
				{
					Timing.CallDelayed(1.5f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(1.7f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(1.9f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(2.1f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(2.3f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(2.5f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(2.7f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(1.5f, () => Cassie.CassieMessage("pitch_0.1  .g1", false, false));
					Timing.CallDelayed(1.6f, () => Cassie.CassieMessage("pitch_1.1 .g1", false, false));
					Timing.CallDelayed(11.4f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(11.6f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(11.8f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(12.0f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(12.2f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(12.4f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(12.6f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(11.4f, () => Cassie.CassieMessage("pitch_0.1  .g5", false, false));
					Timing.CallDelayed(11.5f, () => Cassie.CassieMessage("pitch_1.1 .g1", false, false));
				}
		}
	}
}
