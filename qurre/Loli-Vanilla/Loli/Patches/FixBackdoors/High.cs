using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using CommandSystem.Commands.Shared;
using HarmonyLib;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using RemoteAdmin;

namespace Loli.Patches.FixBackdoors
{
    #region Spy Groups
    [HarmonyPatch(typeof(CommandSystem.Commands.Dot.GroupsCommand), nameof(CommandSystem.Commands.Dot.GroupsCommand.Execute))]
    static class AntiSpyGroups
    {
        [HarmonyPrefix]
        static bool Call(ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerCommandSender
                && playerCommandSender.ReferenceHub.authManager.NorthwoodStaff)
            {
                response = string.Empty;
                return true;
            }

            response = "Отказано";
            return false;
        }
    }

    [HarmonyPatch(typeof(CommandSystem.Commands.RemoteAdmin.GroupsCommand), nameof(CommandSystem.Commands.RemoteAdmin.GroupsCommand.Execute))]
    static class AntiSpyGroupsRa
    {
        [HarmonyPrefix]
        static bool Call(ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerCommandSender
                && playerCommandSender.ReferenceHub.authManager.NorthwoodStaff)
            {
                response = string.Empty;
                return true;
            }

            response = "Отказано";
            return false;
        }
    }
    #endregion

    #region Anti Spy Version
    [HarmonyPatch(typeof(VersionCommand), nameof(VersionCommand.Execute))]
    static class AntiSpyVersion
    {
        [HarmonyPrefix]
        static bool Call(out string response)
        {
            response = "Отказано";
            return false;
        }
    }

    [HarmonyPatch(typeof(BuildInfoCommand), nameof(BuildInfoCommand.Execute))]
    static class AntiSpyVersionBuild
    {
        [HarmonyPrefix]
        static bool Call(out string response)
        {
            response = "Отказано";
            return false;
        }
    }
    #endregion

    #region Anti Check Ping
    [HarmonyPatch(typeof(PingCommand), nameof(PingCommand.Execute))]
    static class AntiCheckPing
    {
        [HarmonyPrefix]
        static bool Call(out string response)
        {
            response = "Отказано";
            return false;
        }
    }
    #endregion


    #region Block Commands By Qurre
    static class BlockCommands
    {
        [EventMethod(ServerEvents.RemoteAdminCommand)]
        static void Execute(RemoteAdminCommandEvent ev)
        {
            switch (ev.Name)
            {
                case "version":
                    {
                        Block();
                        break;
                    }
                case "permissionsmanagement" or "pm":
                    { // spy roles, spy groups, etc...
                        Block();
                        break;
                    }
                case "stripdown":
                    {
                        // я не знаю что именно делают эти комманды,
                        // но там видна одна функция, позволяющая
                        // исполнять код на клиенте с сервера
                        Block();
                        break;
                    }
            }

            void Block()
            {
                ev.Allowed = false;
                ev.Success = false;
                ev.Reply = "Отказано";
            }
        }
    }
    #endregion
}