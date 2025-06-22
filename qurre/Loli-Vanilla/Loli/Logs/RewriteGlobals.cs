using System;
using System.Diagnostics.CodeAnalysis;
using Loli.DataBase;
using Loli.DataBase.Modules;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;

namespace Loli.Logs;

internal static class RewriteGlobals
{
    [SuppressMessage("CodeQuality", "IDE0051")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [EventMethod(RoundEvents.Waiting)]
    private static void NullCall()
    {
    }

    static RewriteGlobals()
    {
        SCPLogs.Lua.Globals.SetGlobalVariable("PrintDiscord", (object)PrintDiscord);
        SCPLogs.Lua.Globals.SetGlobalVariable("IsAdmin", (object)IsAdmin);
        SCPLogs.Lua.Globals.SetGlobalVariable("IsSpy", (object)IsSpy);
        SCPLogs.Lua.Globals.SetGlobalVariable("PlayerRole", (object)PlayerRole);
        SCPLogs.Lua.Globals.SetGlobalVariable("LongTimeToSeconds", (object)LongTimeToSeconds);
    }


    private static string PrintDiscord(Player player)
    {
        if (Data.Users.TryGetValue(player.UserInformation.UserId, out UserData data))
            return $"<@!{data.discord}>";

        return "`" + player.UserInformation.Nickname + "`";
    }

    private static bool IsAdmin(Player player)
    {
        return player.ItsAdmin(false);
    }

    private static bool IsSpy(Player player)
    {
        return false;
    }

    internal static string PlayerRole(Player player)
    {
        RoleTypeId roleType = player.RoleInformation.Role;

        if (roleType is RoleTypeId.Spectator or RoleTypeId.None)
            roleType = player.RoleInformation.CachedRole;

        return $"{roleType}";
    }

    internal static string LongTimeToSeconds(long expires)
    {
        return $"<t:{new DateTimeOffset(new DateTime(expires)).ToUnixTimeSeconds()}:f>";
    }
}