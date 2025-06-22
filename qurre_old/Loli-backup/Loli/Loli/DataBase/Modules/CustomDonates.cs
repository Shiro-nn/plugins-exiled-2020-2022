using Loli.DataBase.Modules.Controllers;
using Qurre.API;
namespace Loli.DataBase.Modules
{
    internal static class CustomDonates
    {
        internal static void CheckGetDonate(Player pl, int id)
        {
            try
            {
                if (id == 3437 && Plugin.ServerID == 4)
                {
                    //new Glow(pl, new UnityEngine.Color32(190, 194, 203, 255));
                    new Nimb(pl);
                    pl.RaLogin();
                }
                if (ThisYt(id))
                {
                    pl.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("yt"), false, true, false);
                    if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(pl.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(pl.UserId);
                    ServerStatic.GetPermissionsHandler()._members.Add(pl.UserId, "yt");
                    Levels.SetPrefix(pl);
                }
            }
            catch { }
        }
        internal static bool ThisDonater(string userId)
        {
            if (!Manager.Static.Data.Users.TryGetValue(userId, out var _data)) return false;
            return ThisDonater(_data);
        }
        internal static bool ThisDonater(UserData data) => data.id == 3437 && Plugin.ServerID == 4;
        internal static bool ThisYt(string userId)
        {
            if (!Manager.Static.Data.Users.TryGetValue(userId, out var _data)) return false;
            return ThisYt(_data.id);
        }
        internal static bool ThisYt(UserData data) => ThisYt(data.id);
        internal static bool ThisYt(int id)
            => (Plugin.YTAcess && (id == 1943 || id == 13 || id == 3658 || id == 1385)) ||
            (Plugin.YouTubersServer && (id == 2749 || id == 3953 || id == 3958 || id == 4074));
        internal static bool TryGetPlayerListPrefix(int id, out PlayerListPrefix prefix)
        {
            if (id == 3437 && Plugin.ServerID == 4)
            {
                prefix = new("НЕ АДМИН, а Донатер", "light_green");
                return true;
            }
            prefix = default;
            return false;
        }
        internal static bool TryGetRemoteAdminPrefix(int id, out PlayerListPrefix prefix)
        {
            if (id == 3437 && Plugin.ServerID == 4)
            {
                prefix = new("___", "#00b235");
                return true;
            }
            if (ThisYt(id))
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