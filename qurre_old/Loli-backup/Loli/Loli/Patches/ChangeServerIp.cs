/*using Authenticator;
using HarmonyLib;
using System.Collections.Generic;
namespace Loli.Patches
{
    [HarmonyPatch(typeof(AuthenticatorQuery), nameof(AuthenticatorQuery.SendData))]
    internal static class ChangeServerIp
    {
        internal static void Prefix(ref IEnumerable<string> data)
        {
            List<string> list = new();
            foreach (var _d in data)
            {
                if (_d.Contains("ip=") && Plugin.ServerDomain != "") list.Add($"ip={Plugin.ServerDomain}");
                else list.Add(_d);
            }
            data = list;
        }
    }
    [HarmonyPatch(typeof(AuthenticatorQuery), nameof(AuthenticatorQuery.SaveNewToken))]
    internal static class AntiChangeToken
    {
        internal static bool Prefix()
        {
            return false;
        }
    }
}*/