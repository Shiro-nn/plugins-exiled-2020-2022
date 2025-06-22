using Qurre.API;

namespace Loli.DataBase.Modules
{
    internal static class CustomDonates
    {
        internal static void CheckGetDonate(Player pl, string steam)
        {
            try
            {
                if (ThisYtInternal(steam))
                {
                    pl.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("yt"), false, true, false);
                    if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(pl.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(pl.UserInfomation.UserId);
                    ServerStatic.GetPermissionsHandler()._members.Add(pl.UserInfomation.UserId, "yt");
                    Levels.SetPrefix(pl);
                }
            }
            catch { }
        }
        internal static bool ThisYt(string userId) => ThisYtInternal(userId.Replace("@steam", ""));
        internal static bool ThisYt(UserData data) => ThisYtInternal(data.steam);
        internal static bool ThisYtInternal(string id)
            => Core.YTAcess && (id == "29289299292929");
        internal static bool TryGetRemoteAdminPrefix(string id, out PlayerListPrefix prefix)
        {
            if (ThisYtInternal(id))
            {
                prefix = new("YouTube", "#ff0000");
                return true;
            }
            prefix = default;
            return false;
        }
        internal class PlayerListPrefix
        {
            internal readonly string Name;
            internal readonly string Color;
            internal PlayerListPrefix(string name, string color)
            {
                Name = name;
                Color = color;
            }
        }
    }
}