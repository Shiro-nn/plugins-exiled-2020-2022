using Authenticator;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Searching;
using PlayerRoles.Ragdolls;
using PlayerRoles.Voice;
using VoiceChat.Playbacks;

namespace HubServer
{
    [HarmonyPatch(typeof(ServerLogs), nameof(ServerLogs.StartLogging))]
    static class NotFileLogs1
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(ServerLogs), nameof(ServerLogs.AddLog))]
    static class NotFileLogs2
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(ServerLogs), nameof(ServerLogs.AppendLog))]
    static class NotFileLogs3
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(PersonalRadioPlayback), nameof(PersonalRadioPlayback.Update))]
    static class Path1
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(SearchCoordinator), nameof(SearchCoordinator.ReceiveRequestUnsafe))]
    static class Path2
    {
        [HarmonyPrefix]
        static bool Call(out SearchSession? session, out SearchCompletor completor, ref bool __result)
        {
            session = null;
            completor = null;
            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(Intercom), nameof(Intercom.Update))]
    static class Path3
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(FirearmBasicMessagesHandler), nameof(FirearmBasicMessagesHandler.ServerRequestReceived))]
    static class Path4
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.FixedUpdate))]
    static class Path5
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(RagdollManager), nameof(RagdollManager.ServerSpawnRagdoll))]
    static class Path6
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(AuthenticatorQuery), nameof(AuthenticatorQuery.SendData))]
    static class Path7
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(AuthenticatorQuery), nameof(AuthenticatorQuery.SaveNewToken))]
    static class Path8
    {
        [HarmonyPrefix]
        static bool Call()
        {
            return false;
        }
    }
}