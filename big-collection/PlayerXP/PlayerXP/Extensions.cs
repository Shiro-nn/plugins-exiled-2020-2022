using MEC;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using EXILED;
using EXILED.Extensions;
using Mirror;
using RemoteAdmin;
namespace PlayerXP
{
	public static class Extensions
	{

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
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
			player.serverRoles.GlobalSet = false;
			player.serverRoles.RefreshHiddenTag();
		}
		public static float GenerateRandomNumber(float min, float max)
		{
			if (max + 1 <= min) return min;
			return (float)new System.Random().NextDouble() * ((max + 1) - min) + min;
		}
		public static void Broadcast(this ReferenceHub player, string message, uint time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, monospaced);
		}
		public static string GetGroupName(this UserGroup group)
			=> ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key)
				.FirstOrDefault();

		public static bool IshelperTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.helperRoles.Contains(group);
		}
		public static bool IsgoodTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.goodRoles.Contains(group);
		}
		public static bool IseliteTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.eliteRoles.Contains(group);
		}
		public static bool IspremiumTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.premiumRoles.Contains(group);
		}
		public static bool IsvipTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.vipRoles.Contains(group);
		}
		public static bool IsvipplusTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.vipplusRoles.Contains(group);
		}
		public static bool IsstaTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.staRoles.Contains(group);
		}
		public static bool IsviphelperTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.viphelperRoles.Contains(group);
		}
		public static bool IsadminTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.adminRoles.Contains(group);
		}
		public static bool IslgbtTagUser(this ReferenceHub hub)
		{
			string group = ServerStatic.GetPermissionsHandler().GetUserGroup(hub.characterClassManager.UserId)
				.GetGroupName();

			return !string.IsNullOrEmpty(group) && Plugin.lgbtRoles.Contains(group);
		}
	}
}