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
namespace GIVE
{
	public static class Extensions
	{
		public static string GetGroupName(this UserGroup group)
			=> ServerStatic.GetPermissionsHandler().GetAllGroups().Where(p => p.Value == group).Select(p => p.Key)
				.FirstOrDefault();

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
	}
}
