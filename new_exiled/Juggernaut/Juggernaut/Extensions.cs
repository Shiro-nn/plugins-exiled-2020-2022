using MEC;
using UnityEngine;

namespace Juggernaut
{
	public static class Extensions
	{

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				Plugin.Info(pos.ToString());
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(EventHandlers.DelayAction(0.5f, () => player.playerMovementSync.OverridePosition(pos, 0)));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}
		public static void SetWeaponAmmo(this ReferenceHub rh, int amount)
		{
			rh.inventory.items.ModifyDuration(
			rh.inventory.items.IndexOf(rh.inventory.GetItemInHand()),
			amount);
		}
		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.RefreshHiddenTag();
		}

		public static void Broadcast(this ReferenceHub player, string message, ushort time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, 0);
		}

	}
}