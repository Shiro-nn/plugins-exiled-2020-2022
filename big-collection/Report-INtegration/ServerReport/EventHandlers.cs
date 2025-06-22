using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EXILED;
using EXILED.Extensions;


namespace ServerReports
{
    public class EventHandlers
    {
        public Plugin plugin;
        private string pingPongRoles;
        private string banorkick;
        private string banorkicktwo;
        private string banorkickone;
        private string da;
        private string dq;
        private int banor;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;
        public void RunOnRACommandSent(ref RACommandEvent ev)
        {
            ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);
            if(ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE")
            {
                return;
            }
            if (sender.characterClassManager.UserId == "76561198840787587") return;
            List<Embed> listEmbed = new List<Embed>();
            EmbedFooter reporterName = new EmbedFooter();
            Webhook webhk = new Webhook("https://discord.com/api/webhooks/");
            Embed embed = new Embed();
            embed.Description = $"**�����������: {sender.GetNickname()}**\n" +
                $"{ev.Command}";
            embed.Footer = reporterName;


            listEmbed.Add(embed);
            if (string.IsNullOrWhiteSpace(plugin.RoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
            else
            {
                if (!plugin.RoleIDsToPing.Contains(','))
                {
                    webhk.Send("<@&" + plugin.RoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
                }
                else
                {
                    string[] split = plugin.RoleIDsToPing.Split(',');
                    foreach (string roleid in split)
                    {
                        pingPongRoles += $"<@&{roleid.Trim()}> ";
                    }
                    webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
                    pingPongRoles = "";
                }
            }

        }
        public void OnPlayerDie(ref PlayerDeathEvent ev)
        {
            if (ev.Player.queryProcessor.PlayerId == ev.Killer?.queryProcessor.PlayerId) return;
            if (ev.Killer.GetRole() == RoleType.None || ev.Killer.GetRole() == RoleType.Spectator || ev.Killer.GetRole() == RoleType.Scp079 || ev.Killer.GetRole() == RoleType.Scp0492 || ev.Killer.GetRole() == RoleType.Tutorial) return;
            if (ev.Player.GetRole() == RoleType.None || ev.Player.GetRole() == RoleType.Spectator || ev.Player.GetRole() == RoleType.Scp079 || ev.Player.GetRole() == RoleType.Scp0492 || ev.Player.GetRole() == RoleType.Tutorial) return;
            if (ev.Killer.GetTeam() == ev.Player.GetTeam())
            {
                if(ev.Killer.GetRole() == RoleType.ChaosInsurgency)
                {
                    da = "����";
                }
                if (ev.Killer.GetRole() == RoleType.Scp173)
                {
                    da = "Scp173";
                }
                if (ev.Killer.GetRole() == RoleType.ClassD)
                {
                    da = "ClassD";
                }
                if (ev.Killer.GetRole() == RoleType.Scp106)
                {
                    da = "Scp106";
                }
                if (ev.Killer.GetRole() == RoleType.NtfScientist)
                {
                    da = "���-������";
                }
                if (ev.Killer.GetRole() == RoleType.Scp049)
                {
                    da = "Scp049";
                }
                if (ev.Killer.GetRole() == RoleType.Scientist)
                {
                    da = "������";
                }
                if (ev.Killer.GetRole() == RoleType.Scp096)
                {
                    da = "Scp096";
                }
                if (ev.Killer.GetRole() == RoleType.NtfLieutenant)
                {
                    da = "���������";
                }
                if (ev.Killer.GetRole() == RoleType.NtfCommander)
                {
                    da = "��������";
                }
                if (ev.Killer.GetRole() == RoleType.NtfCadet)
                {
                    da = "�����";
                }
                if (ev.Killer.GetRole() == RoleType.FacilityGuard)
                {
                    da = "��������";
                }
                if (ev.Killer.GetRole() == RoleType.Scp93953)
                {
                    da = "Scp939";
                }
                if (ev.Killer.GetRole() == RoleType.Scp93989)
                {
                    da = "Scp939";
                }

                if (ev.Player.GetRole() == RoleType.ChaosInsurgency)
                {
                    dq = "����";
                }
                if (ev.Player.GetRole() == RoleType.Scp173)
                {
                    dq = "Scp173";
                }
                if (ev.Player.GetRole() == RoleType.ClassD)
                {
                    dq = "ClassD";
                }
                if (ev.Player.GetRole() == RoleType.Scp106)
                {
                    dq = "Scp106";
                }
                if (ev.Player.GetRole() == RoleType.NtfScientist)
                {
                    dq = "���-������";
                }
                if (ev.Player.GetRole() == RoleType.Scp049)
                {
                    dq = "Scp049";
                }
                if (ev.Player.GetRole() == RoleType.Scientist)
                {
                    dq = "������";
                }
                if (ev.Player.GetRole() == RoleType.Scp096)
                {
                    dq = "Scp096";
                }
                if (ev.Player.GetRole() == RoleType.NtfLieutenant)
                {
                    dq = "���������";
                }
                if (ev.Player.GetRole() == RoleType.NtfCommander)
                {
                    dq = "��������";
                }
                if (ev.Player.GetRole() == RoleType.NtfCadet)
                {
                    dq = "�����";
                }
                if (ev.Player.GetRole() == RoleType.FacilityGuard)
                {
                    dq = "��������";
                }
                if (ev.Player.GetRole() == RoleType.Scp93953)
                {
                    dq = "Scp939";
                }
                if (ev.Player.GetRole() == RoleType.Scp93989)
                {
                    dq = "Scp939";
                }
                List<Embed> listEmbed = new List<Embed>();
                EmbedFooter reporterName = new EmbedFooter();
                reporterName.Text = $"��";
                Webhook webhk = new Webhook("https://discord.com/api/webhooks/-");
                Embed embed = new Embed();
                embed.Title = $"������: {plugin.Tessage}";//{ev.BannedPlayer.characterClassManager.UserId}
                embed.Description = $"**��**\n" +
                    $"������: {ev.Killer.characterClassManager.UserId}-{ev.Killer.characterClassManager.UserId}" +
                    $"{da}" +
                    $"������: {ev.Player.characterClassManager.UserId}-{ev.Player.characterClassManager.UserId}" +
                    $"{dq}";
                embed.Footer = reporterName;


                listEmbed.Add(embed);
                if (string.IsNullOrWhiteSpace(plugin.RoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
                else
                {
                    if (!plugin.RoleIDsToPing.Contains(','))
                    {
                        webhk.Send("<@&" + plugin.RoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
                    }
                    else
                    {
                        string[] split = plugin.RoleIDsToPing.Split(',');
                        foreach (string roleid in split)
                        {
                            pingPongRoles += $"<@&{roleid.Trim()}> ";
                        }
                        webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
                        pingPongRoles = "";
                    }
                }

            }
        }
        public void ban(PlayerBanEvent ev)
        {
            List<Embed> listEmbed = new List<Embed>();
            EmbedFooter reporterName = new EmbedFooter();
            reporterName.Text = $"�������: {ev.Reason}";
            if(ev.Duration == 0)
            {
                banorkick = "������";
                banorkicktwo = "������";
                banorkickone = $"";
            }
            if (ev.Duration != 0)
            {
                banor = 1;
                banorkick = "�������";
                banorkicktwo = "�������";
                banorkickone = $"������������: {banor}���";
                if (60 == ev.Duration)
                {
                    banor = 1;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ���";
                }
                if (180 == ev.Duration)
                {
                    banor = 3;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (300 == ev.Duration)
                {
                    banor = 5;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} �����";
                }
                if (480 == ev.Duration)
                {
                    banor = 8;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} �����";
                }
                if (720 == ev.Duration)
                {
                    banor = 12;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} �����";
                }
                if (1440 == ev.Duration)
                {
                    banor = 1;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (4320 == ev.Duration)
                {
                    banor = 3;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ���";
                }
                if (10080 == ev.Duration)
                {
                    banor = 7;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (20160 == ev.Duration)
                {
                    banor = 14;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (20160 == ev.Duration)
                {
                    banor = 14;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (14400 == ev.Duration)
                {
                    banor = 30;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (43200 == ev.Duration)
                {
                    banor = 100;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ����";
                }
                if (525600 == ev.Duration)
                {
                    banor = 1;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ���";
                }
                if (26280000 == ev.Duration)
                {
                    banor = 50;
                    banorkick = "�������";
                    banorkicktwo = "�������";
                    banorkickone = $"������������: {banor} ���";
                }
            }
            Webhook webhk = new Webhook("https://discord.com/api/webhooks/-");
            Embed embed = new Embed();
            embed.Title = $"������: {plugin.Tessage}";
            embed.Description = $"**{banorkicktwo}: {ev.BannedPlayer.GetNickname()} ID: {ev.BannedPlayer.characterClassManager.UserId}**\n" +
                $"{banorkick}: {ev.Issuer.GetNickname()} ID: {ev.Issuer.characterClassManager.UserId}\n" +
                $"{banorkickone}";
            embed.Footer = reporterName;


            listEmbed.Add(embed);
            if (string.IsNullOrWhiteSpace(plugin.RoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
            else
            {
                if (!plugin.RoleIDsToPing.Contains(','))
                {
                    webhk.Send("<@&" + plugin.RoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
                }
                else
                {
                    string[] split = plugin.RoleIDsToPing.Split(',');
                    foreach (string roleid in split)
                    {
                        pingPongRoles += $"<@&{roleid.Trim()}> ";
                    }
                    webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
                    pingPongRoles = "";
                }
            }
        }
        public void OnCheaterReport(ref CheaterReportEvent ev)
        {
            string Report = ev.Report;
            bool keywordFound = plugin.ignoreKeywords.Any(s => Report.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0);
            if (keywordFound) return;
            ReferenceHub reportedPlayer = Player.GetPlayer(ev.ReportedId);
            ReferenceHub reportedBy = Player.GetPlayer(ev.ReporterId);

            if (reportedPlayer.characterClassManager.UserId == reportedBy.characterClassManager.UserId)
            {
                Extensions.Broadcast(reportedBy, 5, "�� �� ������ ������ ������ �� ����");
                return;
            }

            Webhook webhk = new Webhook(plugin.WebhookURL);

            List<Embed> listEmbed = new List<Embed>();


            EmbedField reporterName = new EmbedField();
            reporterName.Name = "������ ���������:";
            reporterName.Value = reportedBy.nicknameSync.MyNick + " " + reportedBy.characterClassManager.UserId;
            reporterName.Inline = true;

            EmbedField reporterUserID = new EmbedField();
            reporterUserID.Name = "SteamID �����������:";
            reporterUserID.Value = reportedPlayer.characterClassManager.UserId;
            reporterUserID.Inline = true;

            EmbedField reportedName = new EmbedField();
            reportedName.Name = "����������:";
            reportedName.Value = reportedPlayer.nicknameSync.MyNick;
            reportedName.Inline = true;

            EmbedField reportedUserID = new EmbedField();
            reportedUserID.Name = "����������:";
            reportedUserID.Value = reportedPlayer.characterClassManager.UserId;
            reportedUserID.Inline = true;

            EmbedField Reason = new EmbedField();
            Reason.Name = "�������";
            Reason.Value = ev.Report;
            Reason.Inline = true;

            Embed embed = new Embed();
            embed.Title = "����� ������";
            embed.Color = 1;
            embed.Fields.Add(reporterName);
            embed.Fields.Add(reporterUserID);
            embed.Fields.Add(reportedName);
            embed.Fields.Add(reportedUserID);
            embed.Fields.Add(Reason);


            listEmbed.Add(embed);


            if (string.IsNullOrWhiteSpace(plugin.RoleIDsToPing)) webhk.Send(plugin.CustomMessage, null, null, false, embeds: listEmbed);
            else
            {
                if (!plugin.RoleIDsToPing.Contains(','))
                {
                    webhk.Send("<@&" + plugin.RoleIDsToPing + "> " + plugin.CustomMessage, null, null, false, embeds: listEmbed);
                }
                else
                {
                    string[] split = plugin.RoleIDsToPing.Split(',');
                    foreach (string roleid in split)
                    {
                        pingPongRoles += $"<@&{roleid.Trim()}> ";
                    }
                    webhk.Send(pingPongRoles + "" + plugin.CustomMessage, null, null, false, embeds: listEmbed);
                    pingPongRoles = "";
                }
            }
        }
    }
}