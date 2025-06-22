using EXILED;
using EXILED.Extensions;
using MEC;
using scp035.API;
using System.Collections.Generic;
using UnityEngine;

namespace CISpy
{
	partial class EventHandlers
	{
		internal static void MakeSpy(ReferenceHub player, bool isVulnerable = false, bool full = true)
		{
			if (!Configs.spawnWithGrenade && full)
			{
				for (int i = player.inventory.items.Count - 1; i >= 0; i--)
				{
					if (player.inventory.items[i].id == ItemType.GrenadeFrag)
					{
						player.inventory.items.RemoveAt(i);
					}
				}
			}
			spies.Add(player, isVulnerable);
			player.Broadcast(Configs.spawnbc, Configs.spawnbct);
			player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, Configs.cm, Configs.cmc);
		}

		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}

		private void RevealSpies()
		{
			foreach (KeyValuePair<ReferenceHub, bool> spy in spies)
			{
				Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
				foreach (var item in spy.Key.inventory.items) items.Add(item);
				Vector3 pos = spy.Key.transform.position;
				Quaternion rot = spy.Key.transform.rotation;
				int health = (int)spy.Key.playerStats.health;
				string ammo = spy.Key.ammoBox.Networkamount;

				spy.Key.SetRole(RoleType.ChaosInsurgency);

				Timing.CallDelayed(0.3f, () =>
				{
					spy.Key.plyMovementSync.OverridePosition(pos, 0f);
					spy.Key.SetRotation(rot.x, rot.y);
					spy.Key.inventory.items.Clear();
					foreach (var item in items) spy.Key.inventory.AddNewItem(item.id);
					spy.Key.playerStats.health = health;
					spy.Key.ammoBox.Networkamount = ammo;
				});

				spy.Key.Broadcast(Configs.acdb, Configs.acdbt);
			}
			spies.Clear();
		}

		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = true;
			ffPlayers.Add(player);
		}

		private void RemoveFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player);
		}
	}
}