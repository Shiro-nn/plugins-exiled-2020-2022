using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using PlayerXP.discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace PlayerXP.events
{
	public class mtfvsci
	{
		private readonly Plugin plugin;
		public mtfvsci(Plugin plugin) => this.plugin = plugin;
		private static Dictionary<string, RoleType> role = new Dictionary<string, RoleType>();
		private int chaosint = 0;
		private int mtfint = 0;
		public Random Gen = new Random();
		internal bool Ga = true;
		public bool GamemodeEnabled;
		internal static bool GamemodEnabled;
		internal bool RoundStarted;
		private string pingPongRoles;
		public void OnWaitingForPlayers()
		{
			RoundStarted = false;
			if (Server.Port == 7777)
			{
				Tessage = "fydne ff:off";
			}
		}
		internal void roundstart()
		{
			role.Clear();
			chaosint = 0;
			mtfint = 0;
			ServerConsole._serverName = ServerConsole._serverName.Replace("<color=#0089c7>Запущен ивент</color> <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color>", "");
			if (!Ga)
			{
				plugin.spawns.spawnwork(new Vector3(20f, 989.4f, -66f), 10);
				plugin.spawns.spawnwork(new Vector3(148.7f, 995, -46.4f), 2);
				ServerConsole._serverName += $"<color=#0089c7>Запущен ивент</color> <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color>";
				GamemodeEnabled = true;
				GamemodEnabled = true;
				Ga = true;
				RoundStarted = true;
				Timing.CallDelayed(1.0f, () =>
				{
					foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
						item.Delete();
					List<ReferenceHub> players = Extensions.GetHubs().ToList();
					foreach (ReferenceHub player in players)
					{
						Timing.CallDelayed(0.5f, () => player.characterClassManager.SetPlayersClass(RoleType.Spectator, player.gameObject));
						Timing.CallDelayed(1.5f, () => spawnp(player));
					}
				});
			}
		}
		internal void roundend(RoundEndedEventArgs ev)
		{
			role.Clear();
			chaosint = 0;
			mtfint = 0;
			RoundStarted = false;
			if (Ga) GamemodeEnabled = false;
			if (Ga) ServerConsole._serverName = ServerConsole._serverName.Replace("<color=#0089c7>Запущен ивент</color> <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color>", "");
			if (ev.LeadingTeam == Exiled.API.Enums.LeadingTeam.ChaosInsurgency)
			{
				foreach (ReferenceHub player in Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ChaosInsurgency).ToList())
				{
					plugin.donate.money[player.characterClassManager.UserId] += 25;
					player.Broadcast("<color=#fdffbb>Вы получили <color=red>25 монет</color> за победу на ивенте!</color>", 5);
				}
			}
			else if (ev.LeadingTeam == Exiled.API.Enums.LeadingTeam.FacilityForces)
			{
				foreach (ReferenceHub player in Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.NtfCommander).ToList())
				{
					plugin.donate.money[player.characterClassManager.UserId] += 25;
					player.Broadcast("<color=#fdffbb>Вы получили <color=red>25 монет</color> за победу на ивенте!</color>", 5);
				}
			}
		}
		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (GamemodeEnabled)
			{
				ev.Player.ReferenceHub.Broadcast("<color=lime>Сейчас запущен ивент</color> <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color>\n<color=red>.<color=#006dff>mvc info</color>-</color><color=#03d2b0>узнать про ивент</color>", 10);
				if (RoundStarted) Timing.CallDelayed(0.5f, () => spawnp(ev.Player.ReferenceHub));
			}
		}
		public void spawnp(ReferenceHub player)
		{
			if (!role.ContainsKey(player.characterClassManager.UserId))
			{
				if (mtfint >= chaosint)
				{
					chaosint++;
					player.characterClassManager.SetClassID(RoleType.ChaosInsurgency);
					role.Add(player.characterClassManager.UserId, RoleType.ChaosInsurgency);
					Timing.CallDelayed(0.5f, () =>
					{
						player.inventory.Clear();
						player.AddItem(ItemType.KeycardNTFCommander);
						player.inventory.AddNewItem(ItemType.GunE11SR, 999);
						player.inventory.AddNewItem(ItemType.GunLogicer, 999);
						player.AddItem(ItemType.GrenadeFlash);
						player.AddItem(ItemType.GrenadeFrag);
						player.AddItem(ItemType.Medkit);
						player.AddItem(ItemType.Adrenaline);
						player.AddItem(ItemType.Radio);
					});
				}
				else
				{
					mtfint++;
					player.characterClassManager.SetClassID(RoleType.NtfCommander);
					role.Add(player.characterClassManager.UserId, RoleType.NtfCommander);
					Timing.CallDelayed(0.5f, () =>
					{
						player.inventory.Clear();
						player.AddItem(ItemType.KeycardNTFCommander);
						player.inventory.AddNewItem(ItemType.GunE11SR, 999);
						player.inventory.AddNewItem(ItemType.GunLogicer, 999);
						player.AddItem(ItemType.GrenadeFlash);
						player.AddItem(ItemType.GrenadeFrag);
						player.AddItem(ItemType.Medkit);
						player.AddItem(ItemType.Adrenaline);
						player.AddItem(ItemType.Radio);
					});
				}
			}
			else
			{
				Timing.CallDelayed(0.5f, () =>
				{
					player.characterClassManager.SetClassID(role[player.characterClassManager.UserId]);
					Timing.CallDelayed(0.5f, () =>
					{
						player.inventory.Clear();
						player.AddItem(ItemType.KeycardNTFCommander);
						player.inventory.AddNewItem(ItemType.GunE11SR, 999);
						player.inventory.AddNewItem(ItemType.GunLogicer, 999);
						player.AddItem(ItemType.GrenadeFlash);
						player.AddItem(ItemType.GrenadeFrag);
						player.AddItem(ItemType.Medkit);
						player.AddItem(ItemType.Adrenaline);
						player.AddItem(ItemType.Radio);
					});
				});
			}
		}
		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			if (GamemodeEnabled)
			{
				if (Warhead.IsDetonated)
				{
					ev.LeadingTeam = Exiled.API.Enums.LeadingTeam.FacilityForces;
					ev.IsRoundEnded = true;
				}
				else if (Round.ElapsedTime.Minutes == 15)
				{
					ev.LeadingTeam = Exiled.API.Enums.LeadingTeam.ChaosInsurgency;
					ev.IsRoundEnded = true;
				}
				else
				{
					ev.IsAllowed = false;
					ev.IsRoundEnded = false;
				}
				foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
                {
					if(item.itemId != ItemType.KeycardO5 && item.itemId != ItemType.KeycardFacilityManager && item.itemId != ItemType.KeycardContainmentEngineer && item.itemId != ItemType.KeycardNTFCommander &&
						item.itemId != ItemType.GrenadeFlash && item.itemId != ItemType.GrenadeFrag && item.itemId != ItemType.Medkit && item.itemId != ItemType.Adrenaline)
					item.Delete();
				}
				foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
				{
					NetworkServer.Destroy(doll.gameObject);
				}
			}
		}
		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (GamemodeEnabled)
			{
				ev.Target.ReferenceHub.inventory.Clear();
				spawnp(ev.Target.ReferenceHub);
			}
		}
		public void OnRaCommand(SendingRemoteAdminCommandEventArgs ev)
		{
			if (ev.Name == "mvc")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "Успешно";
				Ga = false;
				PlayerManager.localPlayer.GetComponent<Broadcast>().RpcAddElement("<color=red>Ивент</color> <color=#15ff00>^<color=#000fff>mtf</color> <color=#00ffff>vs</color> <color=#0c8700>chaos</color>^</color> <color=red>будет включен в следующем раунде!</color>\n<color=red>.<color=#006dff>mvc info</color>-</color><color=#03d2b0>узнать про ивент</color>", 10, 0);
				List<Embed> listEmbed = new List<Embed>();
				EmbedFooter reporterName = new EmbedFooter();
				reporterName.Text = ae;
				Webhook webhk = new Webhook(webhook);
				EmbedField by = new EmbedField();
				by.Name = c;
				if (ev.Sender.Nickname == "Dedicated Server")
				{
					by.Value = "Запущен голосованием";

				}
				else
				{
					by.Value = ev.Sender.Nickname;
				}
				by.Inline = true;
				EmbedField server = new EmbedField();
				server.Name = s;
				server.Value = $"{Tessage}";
				server.Inline = true;
				Embed embed = new Embed();
				embed.Title = e;
				embed.Description = $"`{has}`\n" +
					$"{nr}";
				embed.Fields.Add(by);
				embed.Fields.Add(server);
				embed.Footer = reporterName;
				listEmbed.Add(embed);
				if (string.IsNullOrWhiteSpace(RoleIDsToPing)) webhk.Send(null, null, null, false, embeds: listEmbed);
				else
				{
					if (!RoleIDsToPing.Contains(','))
					{
						webhk.Send("<@&" + RoleIDsToPing + "> " + null, null, null, false, embeds: listEmbed);
					}
					else
					{
						string[] split = RoleIDsToPing.Split(',');
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
		public string RoleIDsToPing { get; set; } = "654616522513448960";
		public string Tessage { get; set; } = "fydne ff:on";
		public string webhook { get; set; } = "https://discordapp.com/api/webhooks/-";
		public string ae { get; set; } = "Авто-ивент";
		public string c { get; set; } = "Проводит:";
		public string s { get; set; } = "Сервер:";
		public string e { get; set; } = "Ивент:";
		public string has { get; set; } = "mtf vs ci";
		public string nr { get; set; } = "В следующем раунде";
	}
}