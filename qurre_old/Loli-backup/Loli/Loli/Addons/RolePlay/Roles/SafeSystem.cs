using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
namespace Loli.Addons.RolePlay.Roles
{
    internal class SafeSystem
    {
        internal const string Tag = "SafeSystem";
        internal SafeSystem()
        {
            CommandsSystem.RegisterConsole("ssa", Com);
            static void Com(SendingConsoleEvent ev)
            {
                if (!Plugin.RolePlay)
                {
                    ev.ReturnMessage = "Это не РП";
                    return;
                }
                if (ev.Player.Role != RoleType.Scp079)
                {
                    ev.ReturnMessage = "Вы не SCP-079";
                    return;
                }
                if (ev.Player.Tag.Contains(Tag))
                {
                    ev.ReturnMessage = "Вы уже являетесь Системой Безопасности";
                    return;
                }
                ev.Player.Tag += Tag;
                ev.Player.CustomInfo = "Система Безопасности";
                ev.Player.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo;
                Map.Broadcast("<size=30%><color=#6f6f6f>Активирована</color> <color=red>Система Безопасности</color></size>", 15);
                Cassie.Send("safety system .g4 activated");
            }
        }
        internal void FixTags(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player.Tag.Contains(Tag)) ev.Player.Tag = ev.Player.Tag.Replace(Tag, "");
        }
        internal void FixTags(RoleChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.Player.Tag.Contains(Tag)) ev.Player.Tag = ev.Player.Tag.Replace(Tag, "");
        }
        internal void FixTags(DeadEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Target.Tag.Contains(Tag)) ev.Target.Tag = ev.Target.Tag.Replace(Tag, "");
        }
    }
}