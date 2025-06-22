using Qurre.API;
namespace ClassicCore.DataBase.Modules
{
    internal static class CustomDonates
    {
        internal static void CheckGetDonate(Player pl, int id)
        {
            try
            {
                /*
                if (id == -1)
                {
                    pl.RaLogin();
                }
                */
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
        internal static bool ThisDonater(UserData data) => false;// data.id == -1;
        internal static bool ThisYt(string userId)
        {
            if (!Manager.Static.Data.Users.TryGetValue(userId, out var _data)) return false;
            return ThisYt(_data.id);
        }
        internal static bool ThisYt(UserData data) => ThisYt(data.id);
        internal static bool ThisYt(int id) => id == 1943 || id == 13;
        internal static bool TryGetPlayerListPrefix(int id, out PlayerListPrefix prefix)
        {
            /*
            if (id == -1)
            {
                prefix = new("Крутой префикс", "light_green");
                return true;
            }
            */
            prefix = default;
            return false;
        }
        internal static bool TryGetRemoteAdminPrefix(int id, out PlayerListPrefix prefix)
        {
            /*
            if (id == -1)
            {
                prefix = new("Админка да", "#00b235");
                return true;
            }
            */
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