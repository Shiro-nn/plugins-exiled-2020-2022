using System.Linq;
using System.Collections.Generic;
using Exiled.Events.EventArgs;

namespace hideandseek
{
    public class Commands
    {
        private readonly Massacre plugin;
        public Commands(Massacre plugin) => this.plugin = plugin;

        private string pingPongRoles;
        private long authe;
        public void OnRaCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name == plugin.c1 || ev.Name == plugin.c2)
            {
                ev.IsAllowed = false;
                ev.ReplyMessage = plugin.ge;
                plugin.Functions.EnableGamemode();
                authe = plugin.botid;
                List<Embed> listEmbed = new List<Embed>();
                EmbedFooter reporterName = new EmbedFooter();
                reporterName.Text = plugin.ae;
                Webhook webhk = new Webhook(plugin.webhook);
                EmbedField by = new EmbedField();
                by.Name = plugin.c;
                by.Value = ev.Sender.Nickname;
                by.Inline = true;
                EmbedField server = new EmbedField();
                server.Name = plugin.s;
                server.Value = $"{plugin.Tessage}";
                server.Inline = true;
                Embed embed = new Embed();
                embed.Title = plugin.e;
                embed.Description = $"`{plugin.has}`\n" +
                    $"{plugin.nr}";
                embed.Fields.Add(by);
                embed.Fields.Add(server);
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
                            pingPongRoles += $"<@&" + roleid.Trim() + "> ";
                        }
                        webhk.Send(pingPongRoles + "" + null, null, null, false, embeds: listEmbed);
                        pingPongRoles = "";
                    }
                }
            }
        }
    }
}