using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
namespace anti_abuse
{
	internal class Events
	{
		private readonly Plugin plugin;
		internal Events(Plugin plugin) => this.plugin = plugin;
		private Dictionary<string, int> giveway = new Dictionary<string, int>();
		private Dictionary<string, int> force = new Dictionary<string, int>();
		private Dictionary<string, bool> scp_play = new Dictionary<string, bool>();
		private Dictionary<string, DateTime> effect = new Dictionary<string, DateTime>();
		internal static List<string> WhiteList = new List<string>();
		private bool contain = false;
		private bool Waititng = false;
		public void WaitingForPlayers() => Waititng = true;
		public void RoundStarted()
		{
			contain = false;
			Waititng = false;
			try { giveway.Clear(); } catch { }
			try { force.Clear(); } catch { }
			try { scp_play.Clear(); } catch { }
			try { effect.Clear(); } catch { }
			try { WhiteList.Clear(); } catch { }
			foreach (Player pl in Player.List)
			{
				if (!force.ContainsKey(pl.UserId)) force.Add(pl.UserId, 0);
				if (!giveway.ContainsKey(pl.UserId)) giveway.Add(pl.UserId, 0);
				if (!scp_play.ContainsKey(pl.UserId)) scp_play.Add(pl.UserId, false);
				if (!effect.ContainsKey(pl.UserId)) effect.Add(pl.UserId, DateTime.Now);
			}
		}
		internal void Contain(ContainingEventArgs ev)
		{
			if (ev.IsAllowed) contain = true;
		}
		internal void Dead(DiedEventArgs ev)
		{
			if (ev.Target.Team == Team.SCP)
			{
				if (scp_play.ContainsKey(ev.Target.UserId)) scp_play[ev.Target.UserId] = true;
				else scp_play.Add(ev.Target.UserId, true);
			}
		}
		public void Ra(SendingRemoteAdminCommandEventArgs ev)
		{
			try
			{
				if (ev.Name == "awl")
				{
					ev.IsAllowed = false;
					if (ev.Sender.Group.BadgeText == Plugin.Cfg.GetString("anti_abuse_owner_role", ""))
					{
						if (ev.Arguments.Count > 0)
						{
							WhiteList.Add(ev.Arguments[0]);
							ev.ReplyMessage = "RA#Успешно";
						}
						else ev.ReplyMessage = "RA#Укажите UserID\nПример: \nawl 334893523331579916@discord\nawl -@steam";
					}
					else ev.ReplyMessage = "RA#Недостаточно прав.";
				}
				if (!force.ContainsKey(ev.Sender.UserId)) force.Add(ev.Sender.UserId, 0);
				if (!giveway.ContainsKey(ev.Sender.UserId)) giveway.Add(ev.Sender.UserId, 0);
				if (!scp_play.ContainsKey(ev.Sender.UserId)) scp_play.Add(ev.Sender.UserId, false);
				if (!effect.ContainsKey(ev.Sender.UserId)) effect.Add(ev.Sender.UserId, DateTime.Now);
				if (Waititng && Extensions.CheckRole(ev.Sender))
				{
					ev.ReplyMessage = "RA#увы, но сейчас - ожидание игроков";
					return;
				}
				string[] command = ev.Arguments.ToArray();
				CommandSender send = ev.CommandSender;
				if (ev.Name == "give")
				{
					try
					{
						if (Extensions.CheckRole(ev.Sender))
						{
							ev.IsAllowed = false;
							try
							{
								if (giveway[ev.Sender.UserId] < 3)
								{
									var item = -1;
									if (command.Length > 1)
									{
										try { item = Convert.ToInt32(command[1]); } catch { }
									}
									else
									{
										try { item = Convert.ToInt32(command[0]); } catch { }
									}
									if (item == -1 || item > 35)
									{
										ev.ReplyMessage = "GIVE#Либо ты криво написал команду, либо ты попытался выдать кривой предмет";
										return;
									}
									if (item == 16)
									{
										ev.ReplyMessage = "GIVE#Пылесос только 1";
										ev.IsAllowed = false;
										return;
									}
									else if (item == 11)
									{
										ev.ReplyMessage = "GIVE#Черная карта-слишком";
										ev.IsAllowed = false;
										return;
									}
									else if (ev.Sender.Role == RoleType.ClassD)
									{
										if (item == 13 || item == 17 || item == 20 || item == 21 || item == 23 || item == 24 || item == 25 || item == 30 || item == 31 || item == 32 || item == 6 || item == 7 || item == 8 || item == 9 || item == 10)
										{
											if (!RoundSummary.RoundInProgress() || 180 >= RoundSummary.roundTime)
											{
												ev.ReplyMessage = "GIVE#3 минуты не прошло";
												ev.IsAllowed = false;
												return;
											}
										}
									}
									else if (ev.Sender.Role == RoleType.Scientist)
									{
										if (item == 13 || item == 17 || item == 20 || item == 21 || item == 23 || item == 24 || item == 25 || item == 30 || item == 31 || item == 32 || item == 6 || item == 7 || item == 8 || item == 9 || item == 10)
										{
											if (!RoundSummary.RoundInProgress() || 300 >= RoundSummary.roundTime)
											{
												ev.ReplyMessage = "GIVE#5 минут не прошло";
												ev.IsAllowed = false;
												return;
											}
										}
									}
									ev.Sender.AddItem((ItemType)item);
									ev.ReplyMessage = "GIVE#Успешно";
									giveway[ev.Sender.UserId]++;
								}
								else
								{
									ev.ReplyMessage = "GIVE#Вы уже выдали 3 предмета";
									ev.IsAllowed = false;
									return;
								}
							}
							catch (Exception e)
							{
								ev.ReplyMessage = $"GIVE#Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
							}
						}
					}
					catch
					{
						if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
						if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
						try { if (!Extensions.CheckRole(ev.Sender)) return; } catch { }
						ev.IsAllowed = false;
						ev.ReplyMessage = "GIVE#Произошла ошибка, повторите попытку позже";
					}
				}
				if (ev.Name == "forceclass")
				{
					try
					{
						if (Extensions.CheckRole(ev.Sender))
						{
							ev.IsAllowed = false;
							try
							{
								if (force[ev.Sender.UserId] < 3)
								{
									var role = -1;
									if (command.Length > 1)
									{
										try { role = Convert.ToInt32(command[1]); } catch { }
									}
									else
									{
										try { role = Convert.ToInt32(command[0]); } catch { }
									}
									if (role == -1 || role > 17)
									{
										ev.ReplyMessage = "FORCECLASS#Либо ты криво написал команду, либо ты попытался заспавнить себя за кривую команду";
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
										if (Extensions.GetTeam((RoleType)role) == Team.SCP)
										{
											if (!scp_play.ContainsKey(ev.Sender.UserId)) scp_play.Add(ev.Sender.UserId, false);
											if (scp_play[ev.Sender.UserId])
											{
												ev.ReplyMessage = "FORCECLASS#Вы уже играли за SCP";
												return;
											}
										}
									}
									catch { }
									if ((RoleType)role == RoleType.Scp93953)
									{
										if (Scp93953 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
									}
									else if ((RoleType)role == RoleType.Scp93989)
									{
										if (Scp93989 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
									}
									else if ((RoleType)role == RoleType.Scp096)
									{
										if (Scp096 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
									}
									else if ((RoleType)role == RoleType.Scp079)
									{
										if (Scp079 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
									}
									else if ((RoleType)role == RoleType.Scp049)
									{
										if (Scp049 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
									}
									else if ((RoleType)role == RoleType.Scp106)
									{
										if (Scp106 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
										else if (contain)
										{
											ev.ReplyMessage = "FORCECLASS#Условия содержания SCP106 уже восстановлены";
											return;
										}
									}
									else if ((RoleType)role == RoleType.Scp173)
									{
										if (Scp173 > 0)
										{
											ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
											return;
										}
									}
									ev.ReplyMessage = "FORCECLASS#Не забывайте, что у вас есть только 3 спавна";
									ev.Sender.SetRole((RoleType)role);
									force[ev.Sender.UserId]++;
								}
								else
								{
									ev.ReplyMessage = "FORCECLASS#Спавниться более трех раз ЗАПРЕЩЕНО";
								}
							}
							catch (Exception e)
							{
								ev.ReplyMessage = $"FORCECLASS#Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
							}
						}
					}
					catch
					{
						if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
						if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
						try { if (!Extensions.CheckRole(ev.Sender)) return; } catch { }
						ev.IsAllowed = false;
						ev.ReplyMessage = "FORCECLASS#Произошла ошибка, повторите попытку позже";
					}
				}
				if (ev.Name == "effect")
				{
					try
					{
						if (Extensions.CheckRole(ev.Sender))
						{
							try
							{
								double CoolDown = Plugin.Cfg.GetDouble("anti_abuse_effect_cooldown", 3);
								if ((DateTime.Now - effect[ev.CommandSender.SenderId]).TotalSeconds > 0)
								{
									effect[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
								}
								else
								{
									ev.IsAllowed = false;
									ev.ReplyMessage = $"EFFECT#Эффекты можно использовать раз в {CoolDown} минут";
								}
							}
							catch (Exception e)
							{
								ev.IsAllowed = false;
								ev.ReplyMessage = $"EFFECT#Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
							}
						}
					}
					catch
					{
						if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
						if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
						try { if (!Extensions.CheckRole(ev.Sender)) return; } catch { }
						ev.IsAllowed = false;
						ev.ReplyMessage = "EFFECT#Произошла ошибка, повторите попытку позже";
					}
				}
			}
			catch (Exception e)
			{
				if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
				if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
				try { if (!Extensions.CheckRole(ev.Sender)) return; } catch { }
				ev.ReplyMessage = $"RA#хмм, ошибка.\n{e}";
				Log.Warn($"хмм, ошибка.\n{e}");
			}
		}
	}
}