using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace scp035
{
    public static class Extensions
	{
		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}
		public static void SetRole(this ReferenceHub player, RoleType newRole) => player.characterClassManager.SetPlayersClass(newRole, player.gameObject);
		public static void SetRotation(this ReferenceHub player, Vector2 rotations) => player.SetRotation(rotations.x, rotations.y);
		public static void SetRotation(this ReferenceHub player, float x, float y) => player.playerMovementSync.NetworkRotationSync = new Vector2(x, y);
		public static void Broadcast(this ReferenceHub player, string message, ushort time)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}
		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		private static Inventory _hostInventory;
		public static Inventory HostInventory
		{
			get
			{
				if (_hostInventory == null)
					_hostInventory = ReferenceHub.GetHub(PlayerManager.localPlayer).inventory;

				return _hostInventory;
			}
		}
		public static Pickup SpawnItem(ItemType itemType, float durability, Vector3 position, Quaternion rotation = default, int sight = 0, int barrel = 0, int other = 0)
			=> HostInventory.SetPickup(itemType, durability, position, rotation, sight, barrel, other);
		public static RoleType GetRole(this ReferenceHub player) => player.characterClassManager.CurClass;
		public static Team GetTeam(this ReferenceHub player) => GetTeam(GetRole(player));
		public static Team GetTeam(this RoleType roleType)
		{
			switch (roleType)
			{
				case RoleType.ChaosInsurgency:
					return Team.CHI;
				case RoleType.Scientist:
					return Team.RSC;
				case RoleType.ClassD:
					return Team.CDP;
				case RoleType.Scp049:
				case RoleType.Scp93953:
				case RoleType.Scp93989:
				case RoleType.Scp0492:
				case RoleType.Scp079:
				case RoleType.Scp096:
				case RoleType.Scp106:
				case RoleType.Scp173:
					return Team.SCP;
				case RoleType.Spectator:
					return Team.RIP;
				case RoleType.FacilityGuard:
				case RoleType.NtfCadet:
				case RoleType.NtfLieutenant:
				case RoleType.NtfCommander:
				case RoleType.NtfScientist:
					return Team.MTF;
				case RoleType.Tutorial:
					return Team.TUT;
				default:
					return Team.RIP;
			}
		}
		public static void TeleportTo106(Player player)
		{
			Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
				player.ReferenceHub.playerMovementSync.OverridePosition(scp106.ReferenceHub.transform.position, 0f);
			else player.Position = Map.GetRandomSpawnPoint(RoleType.Scp096);
		}
	}
}