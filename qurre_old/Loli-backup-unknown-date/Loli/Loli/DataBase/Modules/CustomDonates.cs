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
                if (id == 3437 && Plugin.ServerID == 3)
                {
                    new Glow(pl, new UnityEngine.Color32(190, 194, 203, 255));
                    pl.RaLogin();
                }
            }
            catch { }
        }
        internal static bool ThisDonater(string userId)
        {
            if(!Manager.Static.Data.Users.TryGetValue(userId, out var _data)) return false;
            return _data.id == 3437 && Plugin.ServerID == 3;
        }
        internal static bool TryGetPlayerListPrefix(int id, out PlayerListPrefix prefix)
        {
            if (id == 3437 && Plugin.ServerID == 3)
            {
                prefix = new("КЕК", "light_green");
                return true;
            }
            prefix = default;
            return false;
        }
        internal static bool TryGetRemoteAdminPrefix(int id, out PlayerListPrefix prefix)
        {
            if (id == 3437 && Plugin.ServerID == 3)
            {
                prefix = new("___", "#00b235");
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
        internal class RemoteAdminPrefix
        {
            internal readonly string Name;
            internal readonly string Color;
            internal RemoteAdminPrefix(string name, string color)
            {
                Name = name;
                Color = color;
            }
        }
    }
}