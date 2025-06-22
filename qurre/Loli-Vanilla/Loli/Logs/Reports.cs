using System.Linq;
using Loli.Webhooks;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Logs;

internal static class Reports
{
    [EventMethod(ServerEvents.CheaterReport)]
    private static void CheaterReport(CheaterReportEvent ev)
    {
        string hook =
            "https://discord.com/api/webhooks";

        if (ev.Target.UserInformation.UserId == ev.Issuer.UserInformation.UserId)
        {
            ev.Issuer.Client.ShowHint(
                "<align=left><color=#737885>Вы не можете отправить репорт на себя</color></align>", 10);
            ev.Allowed = false;
            return;
        }

        SendReport(hook, true, ev.Issuer, ev.Target, ev.Reason);
    }

    [EventMethod(ServerEvents.LocalReport)]
    private static void LocalReport(LocalReportEvent ev)
    {
        string hook =
            "https://discord.com/api/webhooks";

        if (ev.Target.UserInformation.UserId == ev.Issuer.UserInformation.UserId)
        {
            ev.Issuer.Client.ShowHint(
                "<align=left><color=#737885>Вы не можете отправить репорт на себя</color></align>", 10);
            ev.Allowed = false;
            return;
        }

        SendReport(hook, false, ev.Issuer, ev.Target, ev.Reason);
    }

    private static void SendReport(string hook, bool isCheater, Player issuer, Player target, string reason)
    {
        new Dishook(hook).Send(string.Empty, Core.ServerName, null, embeds:
        [
            new Embed
            {
                Title = isCheater ? "Жалоба на читера" : "Жалоба на игрока",
                Color = isCheater ? 16732754 : 16750418,
                Description =
                    $"**Игрок `{issuer.UserInformation.Nickname}` подал жалобу на `{target.UserInformation.Nickname}` за {(isCheater ? "читы" : "нарушение правил")}.**\n" +
                    $"### Причина: ```{reason}```\n" +
                    $"### Жалобу подал:\n```{issuer.UserInformation.Nickname} - {issuer.UserInformation.UserId}\n" +
                    $"{issuer.StatsInformation.KillsCount} убийств, {issuer.StatsInformation.DeathsCount} смертей```\n" +
                    $"### Нарушитель:\n```{target.UserInformation.Nickname} - {target.UserInformation.UserId}\n" +
                    $"{target.StatsInformation.KillsCount} убийств, {target.StatsInformation.DeathsCount} смертей```\n" +
                    $"### Последние 5 убийств нарушителя:\n{
                        string.Join("\n",
                            target.StatsInformation.Kills.TakeLast(5).Select(
                                kill => $"```Убил игрока {kill.Target.Player.UserInformation.Nickname}, " +
                                        $"который был {kill.Target.Role}, будучи {kill.Killer.Role}.\n" +
                                        $"Инвентарь убитого: {kill.Target.InventoryHash}\n" +
                                        $"Инвентарь нарушителя: {kill.Killer.InventoryHash}\n" +
                                        $"Время: {kill.Time:hh:mm:ss zz}\n" +
                                        $"Длительность раунда: {kill.Time - Round.StartedTime:hh\\:mm\\:ss}```"
                            )
                        )
                    }"
            }
        ]);

    }
}