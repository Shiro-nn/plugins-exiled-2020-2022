using Exiled.API.Features;
using Exiled.Events.EventArgs;
using HarmonyLib;
using MEC;
using MongoDB.Bson;
using MongoDB.Driver;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
namespace auto_donate
{
	[Serializable]
	public class Ra_Cfg
	{
		public string UserId;
		public bool force = false;
		public bool give = false;
		public bool effects = false;
		public bool players_roles = false;
		public DateTime now = DateTime.Now;
	}
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public Dictionary<string, bool> donater = new Dictionary<string, bool>();
		public Dictionary<string, Ra_Cfg> Donate = new Dictionary<string, Ra_Cfg>();
		public Dictionary<string, int> force = new Dictionary<string, int>();
		public Dictionary<string, int> giveway = new Dictionary<string, int>();
		private Dictionary<string, DateTime> effect = new Dictionary<string, DateTime>();
		public Dictionary<string, bool> giver = new Dictionary<string, bool>();
		public Dictionary<string, bool> forcer = new Dictionary<string, bool>();
		public Dictionary<string, bool> effecter = new Dictionary<string, bool>();
		public Dictionary<string, bool> scp_play = new Dictionary<string, bool>();
		internal bool contain = false;
		public void PlayerJoin(VerifiedEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			if (!force.ContainsKey(ev.Player.UserId)) force.Add(ev.Player.UserId, 0);
			if (!giveway.ContainsKey(ev.Player.UserId)) giveway.Add(ev.Player.UserId, 0);
			if (!effect.ContainsKey(ev.Player.UserId)) effect.Add(ev.Player.UserId, DateTime.Now);
			if (forcer.ContainsKey(ev.Player.UserId)) forcer[ev.Player.UserId] = false;
			else forcer.Add(ev.Player.UserId, false);
			if (giver.ContainsKey(ev.Player.UserId)) giver[ev.Player.UserId] = false;
			else giver.Add(ev.Player.UserId, false);
			if (effecter.ContainsKey(ev.Player.UserId)) effecter[ev.Player.UserId] = false;
			else effecter.Add(ev.Player.UserId, false);
			if (!scp_play.ContainsKey(ev.Player.UserId)) scp_play.Add(ev.Player.UserId, false);
			spawnpref(ev.Player);
		}
		public void spawnpref(Player pl)
		{
			string web_name = pl.UserId.Replace("@steam", "").Replace("@discord", "");
			var database = Plugin.Client.GetDatabase("auto_donate");
			var collection = database.GetCollection<BsonDocument>("donates");
			var list = collection.Find(new BsonDocument("owner", web_name)).ToList();
			foreach (var document in list)
			{
				Log.Debug($"Donate info =>\nforce: {document["force"]}\ngive: {document["give"]}\neffects: {document["effects"]}\nplayers roles: {document["players_roles"]}\n" +
					$"owner: {document["owner"]}\nto: {document["to"]}\nprefix: {document["prefix"]}\nserver: {document["server"]}\n\nyour id: {web_name}");
				if ((int)document["server"] == plugin.config.ServerID)
				{
					if (!Donate.ContainsKey(pl.UserId))
					{
						Donate.Add(pl.UserId, new Ra_Cfg());
						Donate[pl.UserId].force = (bool)document["force"];
						Donate[pl.UserId].give = (bool)document["give"];
						Donate[pl.UserId].effects = (bool)document["effects"];
						Donate[pl.UserId].players_roles = (bool)document["players_roles"];
					}
					else
					{
						if (!Donate[pl.UserId].force) Donate[pl.UserId].force = (bool)document["force"];
						if (!Donate[pl.UserId].give) Donate[pl.UserId].give = (bool)document["give"];
						if (!Donate[pl.UserId].effects) Donate[pl.UserId].effects = (bool)document["effects"];
						if (!Donate[pl.UserId].players_roles) Donate[pl.UserId].players_roles = (bool)document["players_roles"];
					}
					if ((bool)document["force"])
					{
						if (forcer.ContainsKey(pl.UserId)) forcer[pl.UserId] = true;
						else forcer.Add(pl.UserId, true);
					}
					if ((bool)document["give"])
					{
						if (giver.ContainsKey(pl.UserId)) giver[pl.UserId] = true;
						else giver.Add(pl.UserId, true);
					}
					if ((bool)document["effects"])
					{
						if (effecter.ContainsKey(pl.UserId)) effecter[pl.UserId] = true;
						else effecter.Add(pl.UserId, true);
					}
					if (!Module.Prefixs.ContainsKey(pl.UserId))
					{
						Module.Prefixs.Add(pl.UserId, new Module.ra_pref());
						Module.Prefixs[pl.UserId].prefix = (string)document["prefix"];
						Module.Prefixs[pl.UserId].color = (string)document["color"];
						Module.Prefixs[pl.UserId].gameplay_data = (bool)document["players_roles"];
					}
					else
					{
						Module.Prefixs[pl.UserId].prefix = (string)document["prefix"];
						Module.Prefixs[pl.UserId].color = (string)document["color"];
						Module.Prefixs[pl.UserId].gameplay_data = (bool)document["players_roles"];
					}
					if (donater.ContainsKey(pl.UserId)) effecter[pl.UserId] = true;
					else donater.Add(pl.UserId, true);
					pl.ReferenceHub.serverRoles.RemoteAdmin = true;
					pl.ReferenceHub.serverRoles.RemoteAdminMode = pl.ReferenceHub.serverRoles.RemoteAdminMode == ServerRoles.AccessMode.GlobalAccess ? ServerRoles.AccessMode.GlobalAccess : ServerRoles.AccessMode.PasswordOverride;
					pl.ReferenceHub.serverRoles.CallTargetOpenRemoteAdmin(pl.ReferenceHub.scp079PlayerScript.connectionToClient, false);
					Timing.CallDelayed(1f, () =>
					{
						var role_txt = "";
						try
						{
							string txt = (pl.ReferenceHub.serverRoles.NetworkMyText.Replace($" | {Module.Prefixs[pl.UserId].prefix}", "").Replace($"{Module.Prefixs[pl.UserId].prefix}", "") +
								$" | {Module.Prefixs[pl.UserId].prefix}").Trim();
							if (txt.Substring(0, 1) == "|") role_txt = txt.Substring(1);
							else role_txt = txt;
							pl.ReferenceHub.serverRoles.SetText(role_txt);
						}
						catch { }
						pl.ReferenceHub.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("test"), false, true, false);
						Timing.CallDelayed(1f, () => pl.ReferenceHub.serverRoles.SetText(role_txt));
					});
				}
			}
		}
		internal void Waiting()
		{
			plugin.cfg1();
			contain = false;
			try { donater.Clear(); } catch { }
			try { Donate.Clear(); } catch { }
			try { giveway.Clear(); } catch { }
			try { force.Clear(); } catch { }
			try { effect.Clear(); } catch { }
			try { giver.Clear(); } catch { }
			try { forcer.Clear(); } catch { }
			try { effecter.Clear(); } catch { }
			try { scp_play.Clear(); } catch { }
		}
		internal void Containing(ContainingEventArgs ev)
		{
			if (ev.IsAllowed) contain = true;
		}
		internal void RoleChange(ChangedRoleEventArgs ev) => ChangeRole(ev.Player, ev.Player.Team);
		internal void Spawned(SpawningEventArgs ev) => ChangeRole(ev.Player, ev.Player.Team);
		private void ChangeRole(Player pl, Team tm)
		{
			if (tm == Team.SCP)
			{
				if (scp_play.ContainsKey(pl.UserId)) scp_play[pl.UserId] = true;
				else scp_play.Add(pl.UserId, true);
			}
			if (Module.Prefixs.ContainsKey(pl.UserId))
			{
                try
				{
					if (Module.Prefixs.ContainsKey(pl.UserId))
					{
						Timing.CallDelayed(3f, () =>
						{
							string txt = (pl.ReferenceHub.serverRoles.NetworkMyText.Replace($" | {Module.Prefixs[pl.UserId].prefix}", "").Replace($"{Module.Prefixs[pl.UserId].prefix}", "") +
							$" | {Module.Prefixs[pl.UserId].prefix}").Trim();
							if (txt.Substring(0, 1) == "|") txt = txt.Substring(1);
							pl.ReferenceHub.serverRoles.SetText(txt);
						});
					}
				}
                catch { }
			}
		}
		public void Ra(SendingRemoteAdminCommandEventArgs ev)
		{
			CommandSender send = ev.CommandSender;
			if (ev.Name == "give")
			{
				try
				{
					if (giver[ev.CommandSender.SenderId])
					{
						ev.IsAllowed = false;
						if (!Round.IsStarted)
						{
							ev.ReplyMessage = "Увы, но сейчас - ожидание игроков";
							return;
						}
						try
						{
							if (giveway[ev.Sender.UserId] < 3)
							{
								var item = -1;
								if (ev.Arguments.Count() > 1)
								{
									try { item = Convert.ToInt32(ev.Arguments[1]); } catch { }
								}
								else
								{
									try { item = Convert.ToInt32(ev.Arguments[0]); } catch { }
								}
								if (item == -1 || item > 35)
								{
									ev.ReplyMessage = "Либо ты криво написал команду, либо ты попытался выдать кривой предмет";
									return;
								}
								if (item == 16)
								{
									ev.ReplyMessage = "Пылесос только 1";
									ev.IsAllowed = false;
									return;
								}
								else if (item == 11)
								{
									ev.ReplyMessage = "Черная карта-слишком";
									ev.IsAllowed = false;
									return;
								}
								else if ((ev.Sender.Role == RoleType.ClassD) &&
										(item == 13 || item == 17 || item == 20 || item == 21 || item == 23 || item == 24 || item == 25 || item == 30 ||
										item == 31 || item == 32 || item == 6 || item == 7 || item == 8 || item == 9 || item == 10) &&
										(!RoundSummary.RoundInProgress() || 180 >= RoundSummary.roundTime))
								{
									ev.ReplyMessage = "3 минуты не прошло";
									ev.IsAllowed = false;
									return;
								}
								else if ((ev.Sender.Role == RoleType.Scientist) &&
									(item == 13 || item == 17 || item == 20 || item == 21 || item == 23 || item == 24 || item == 25 || item == 30 ||
									item == 31 || item == 32 || item == 6 || item == 7 || item == 8 || item == 9 || item == 10) &&
									(!RoundSummary.RoundInProgress() || 300 >= RoundSummary.roundTime))
								{
									ev.ReplyMessage = "5 минут не прошло";
									ev.IsAllowed = false;
									return;
								}
								ev.Sender.AddItem((ItemType)item);
								ev.ReplyMessage = "Успешно";
								giveway[ev.Sender.UserId]++;
							}
							else
							{
								ev.ReplyMessage = "Вы уже выдали 3 предмета";
								ev.IsAllowed = false;
								return;
							}
						}
						catch (Exception e)
						{
							ev.ReplyMessage = $"Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
						}
					}
					else if (donater[ev.Sender.UserId])
					{
						ev.ReplyMessage = "Вы не приобрели эту функцию.";
						ev.IsAllowed = false;
						return;
					}
				}
				catch
				{/*
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					if (!giver.ContainsKey(ev.CommandSender.SenderId)) return;
					ev.IsAllowed = false;
					ev.ReplyMessage = "Произошла ошибка, повторите попытку позже";*/
				}
			}
			if (ev.Name == "forceclass")
			{
				try
				{
					if (forcer[ev.CommandSender.SenderId])
					{
						ev.IsAllowed = false;
						try
						{
							if (force[ev.Sender.UserId] < 3)
							{
								var role = -1;
								if (ev.Arguments.Count() > 1)
								{
									try { role = Convert.ToInt32(ev.Arguments[1]); } catch { }
								}
								else
								{
									try { role = Convert.ToInt32(ev.Arguments[0]); } catch { }
								}
								if (role == -1 || role > 17)
								{
									ev.ReplyMessage = "Либо ты криво написал команду, либо ты попытался заспавнить себя за кривую команду";
									return;
								}
								int Scp173 = Player.List.Where(x => x.Role == RoleType.Scp173).ToList().Count;
								int Scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).ToList().Count;
								int Scp049 = Player.List.Where(x => x.Role == RoleType.Scp049).ToList().Count;
								int Scp079 = Player.List.Where(x => x.Role == RoleType.Scp079).ToList().Count;
								int Scp096 = Player.List.Where(x => x.Role == RoleType.Scp096).ToList().Count;
								int Scp93989 = Player.List.Where(x => x.Role == RoleType.Scp93989).ToList().Count;
								int Scp93953 = Player.List.Where(x => x.Role == RoleType.Scp93953).ToList().Count;
								try
								{
									if (role == (int)Team.SCP)
									{
										if (!scp_play.ContainsKey(ev.Sender.UserId)) scp_play.Add(ev.Sender.UserId, false);
										if (scp_play[ev.Sender.UserId])
										{
											ev.ReplyMessage = "Вы уже играли за SCP";
											return;
										}
									}
								}
								catch { }
								if ((RoleType)role == RoleType.Scp93953 && Scp93953 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.Scp93989 && Scp93989 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.Scp096 && Scp096 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.Scp079 && Scp079 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.Scp049 && Scp049 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.Scp106 && Scp106 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.Scp106 && contain)
								{
									ev.ReplyMessage = "Условия содержания SCP106 уже восстановлены";
									return;
								}
								else if ((RoleType)role == RoleType.Scp173 && Scp173 > 0)
								{
									ev.ReplyMessage = "Этот SCP уже есть";
									return;
								}
								else if ((RoleType)role == RoleType.NtfScientist)
								{
									ev.ReplyMessage = "Увы, но нет";
									return;
								}
								ev.ReplyMessage = "Не забывайте, что у вас есть только 3 спавна";
								ev.Sender.SetRole((RoleType)role);
								force[ev.Sender.UserId]++;
							}
							else
							{
								ev.ReplyMessage = "Спавниться более трех раз ЗАПРЕЩЕНО";
							}
						}
						catch (Exception e)
						{
							ev.ReplyMessage = $"Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
						}
					}
					else if (donater[ev.Sender.UserId])
					{
						ev.ReplyMessage = "Вы не приобрели эту функцию.";
						ev.IsAllowed = false;
						return;
					}
				}
				catch
				{/*
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					if (!forcer.ContainsKey(ev.CommandSender.SenderId)) return;
					ev.IsAllowed = false;
					ev.ReplyMessage = "Произошла ошибка, повторите попытку позже";*/
				}
			}
			if (ev.Name == "effect")
			{
				try
				{
					if (effecter[ev.CommandSender.SenderId])
					{
						ev.IsAllowed = false;
						try
						{
							try
							{
								var effects = ev.Arguments[1].Split('=');
								if (effects[0].ToLower() == "scp207" && effects[1] != "0" && ev.Sender.Team == Team.SCP) ev.Arguments[1] = "scp207=1";
							}
							catch { }
							double CoolDown = 3;
							if ((DateTime.Now - effect[ev.CommandSender.SenderId]).TotalSeconds > 0)
							{
								effect[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
								var com = $"{ev.Name} {ev.Sender.Id}. {ev.Arguments[1]}";
								GameCore.Console.singleton.TypeCommand($"/{com}", new EffectsSender(ev.CommandSender.Nickname, com));
								ev.ReplyMessage = "Успешно";
							}
							else
							{
								var wait = Math.Round((effect[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
								ev.ReplyMessage = $"Эффекты можно использовать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							}
						}
						catch (Exception e)
						{
							ev.ReplyMessage = $"Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
						}
					}
					else if (donater[ev.Sender.UserId])
					{
						ev.ReplyMessage = "Вы не приобрели эту функцию.";
						ev.IsAllowed = false;
						return;
					}
				}
				catch
				{/*
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					if (ev.CommandSender.SenderId == "Effects Controller") return;
					if (!effecter.ContainsKey(ev.CommandSender.SenderId)) return;
					ev.IsAllowed = false;
					ev.ReplyMessage = "Произошла ошибка, повторите попытку позже";*/
				}
			}
		}
		internal static bool CheckPerms(CommandSender commandSender, PlayerPermissions perms)
		{
			return (ServerStatic.IsDedicated && commandSender.FullPermissions) ||
					PermissionsHandler.IsPermitted(commandSender.Permissions, perms);

		}
		internal static void Prefixs(RaRequestPlayerListEvent ev)
		{
			try
			{
				string text3 = "\n";
				bool gameplayData = Module.GD(ev.CommandSender) || CheckPerms(ev.CommandSender, PlayerPermissions.GameplayData);
				ev.Player.ReferenceHub.queryProcessor.GameplayData = gameplayData;
				bool flag2 = CheckPerms(ev.CommandSender, PlayerPermissions.ViewHiddenBadges);
				bool flag3 = CheckPerms(ev.CommandSender, PlayerPermissions.ViewHiddenGlobalBadges);
				if (ev.Player.ReferenceHub.serverRoles.Staff)
				{
					flag2 = true;
					flag3 = true;
				}
				foreach (GameObject gameObject4 in PlayerManager.players)
				{
					QueryProcessor component = gameObject4.GetComponent<QueryProcessor>();
					string text4 = string.Empty;
					bool flag4 = false;
					ServerRoles component2 = component.GetComponent<global::ServerRoles>();
					try
					{
						if (string.IsNullOrEmpty(component2.HiddenBadge) || (component2.GlobalHidden && flag3) || (!component2.GlobalHidden && flag2))
						{
							text4 = (component2.RaEverywhere ? "[~] " : (component2.Staff ? "[@] " : (component2.RemoteAdmin ? Module.Prefix(gameObject4) : string.Empty)));
						}
						flag4 = Player.Get(gameObject4).IsOverwatchEnabled;
					}
					catch
					{
					}
					text3 = string.Concat(new object[]
					{
																text3,
																text4,
																"(",
																component.PlayerId,
																") ",
																component.GetComponent<global::NicknameSync>().CombinedName.Replace("\n", string.Empty),
																flag4 ? "<OVRM>" : string.Empty
					});
					text3 += "\n";
				}
				ev.CommandSender.RaReply(ev.Name.ToUpper() + ":PLAYER_LIST#" + text3, true, ev.Args.Length < 2 || ev.Args[1].ToUpper() != "SILENT", "");
				ev.Allowed = false;
			}
			catch
			{
				ev.Allowed = true;
			}
		}
		internal void okp()
		{
			Timing.CallDelayed(5f, () => sendplayersinfo());
		}
		public static NetworkStream ss;
		internal void sendplayersinfo()
		{
			okp();
			try
			{
				TcpClient stcp = new TcpClient();
				stcp.Connect(plugin.config.WebIp, 421);
				ss = stcp.GetStream();
				string name = plugin.config.ServerName;
				int players = Player.List.ToList().Count;
				int maxplay = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
				string str = $"online=;={name}=;={players}=;={maxplay}=;={Server.IpAddress}=;={Server.Port}";
				byte[] ba = Encoding.UTF8.GetBytes(str);
				ss.Write(ba, 0, ba.Length);
				stcp.Close();
			}
			catch { }
		}
	}
	[HarmonyPatch(typeof(CommandProcessor), "ProcessQuery", new Type[] { typeof(string), typeof(CommandSender) })]
	internal static class RemoteAdminCommand
	{
		private static bool Prefix(string q, CommandSender sender)
		{
			try
			{
				string[] allarguments = q.Split(' ');
				string name = allarguments[0].ToLower();
				string[] args = allarguments.Skip(1).ToArray();
				IdleMode.SetIdleMode(false);
				if (q == "REQUEST_DATA PLAYER_LIST SILENT")
				{
					var _ev = new RaRequestPlayerListEvent(sender, string.IsNullOrEmpty(sender.SenderId) ? Server.Host : (Player.Get(sender.SenderId) ?? Server.Host), q, name, args);
					EventHandlers.Prefixs(_ev);
					return _ev.Allowed;
				}
				else return true;
			}
			catch (Exception e)
			{
				Log.Error($"umm, error in patching Server [RA]:\n{e}\n{e.StackTrace}");
				return true;
			}
		}
	}
	public class RaRequestPlayerListEvent : EventArgs
	{
		public RaRequestPlayerListEvent(CommandSender commandSender, Player player, string command, string name, string[] args, bool allowed = true)
		{
			CommandSender = commandSender;
			Player = player;
			Command = command;
			Name = name;
			Args = args;
			Allowed = allowed;
		}
		public CommandSender CommandSender { get; }
		public Player Player { get; }
		public string Command { get; }
		public string Name { get; }
		public string[] Args { get; }
		public bool Allowed { get; set; }
	}
	public static class Module
	{
		public static string Prefix(GameObject gm)
		{
			string userId = ReferenceHub.GetHub(gm).characterClassManager.UserId;
			if (Prefixs.ContainsKey(userId))
			{
				if (Prefixs[userId].color == "")
				{
					return $"[{Prefixs[userId].prefix}] ";
				}
				return $"<color={Prefixs[userId].color}>[{Prefixs[userId].prefix}]</color> ";
			}
			else
			{
				return "[RA] ";
			}
		}
		public static bool GD(CommandSender cs)
		{
			bool result = false;
			string senderId = cs.SenderId;
			if (Prefixs.ContainsKey(senderId))
			{
				result = Prefixs[senderId].gameplay_data;
			}
			return result;
		}
		public static Dictionary<string, ra_pref> Prefixs = new Dictionary<string, ra_pref>();
		[Serializable]
		public class ra_pref
		{
			public string prefix = "RA";
			public string color = "";
			public bool gameplay_data;
		}
	}
	public class EffectsSender : CommandSender
	{
		public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
		{
		}

		public override void Print(string text)
		{
		}

		public string Name;
		public string Command;
		public EffectsSender(string name, string com)
		{
			Name = name;
			Command = com;
		}
		public override string SenderId => "Effects Controller";
		public override string Nickname => Name;
		public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
		public override byte KickPower => byte.MinValue;
		public override bool FullPermissions => true;
	}
}
