using System;
using System.Collections.Generic;
using Grenades;
using MEC;
using System.Linq;
using UnityEngine;
using Mirror;
using System.Text;
using System.Text.RegularExpressions;
using Exiled.Events;
using Exiled.API.Features;

namespace PlayerXP
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>(); 
		private readonly Regex regexSmartSiteReplacer = new Regex(@"#"+Configs.Pn);

		public void OnWaitingForPlayers()
		{
			Stats.Clear();
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
		}

		public void OnRoundStart()
		{
		}

		public void OnRoundEnd(Exiled.Events.EventArgs.RoundEndedEventArgs ev)
		{
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
			try
			{
				foreach (Stats stats in Stats.Values)
					Methods.SaveStats(stats);
			}
			catch (Exception)
			{
			}
		}

		public void OnPlayerJoin(Exiled.Events.EventArgs.JoinedEventArgs ev)
		{
			Timing.CallDelayed(100f, () => ev.Player.Broadcast(15, Configs.Jm));
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			
			if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
				Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Methods.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));
			Timing.CallDelayed(1.5f, () => {
				addp(ev.Player.ReferenceHub);
			});
		}
		public void OnPlayerSpawn(Exiled.Events.EventArgs.SpawningEventArgs ev)
		{
			Timing.CallDelayed(100f, () => ev.Player.Broadcast(15, Configs.Jm));
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
				Stats.Add(ev.Player.ReferenceHub.characterClassManager.UserId, Methods.LoadStats(ev.Player.ReferenceHub.characterClassManager.UserId));
			Timing.CallDelayed(1.5f, () => {
				addp(ev.Player.ReferenceHub);
			});
		}
		public void AddXP(ReferenceHub player)
		{
			Stats[player.characterClassManager.UserId].to = Stats[player.characterClassManager.UserId].lvl * 250 + 750;
			if (Stats[player.characterClassManager.UserId].xp >= Stats[player.characterClassManager.UserId].to)
			{
				Stats[player.characterClassManager.UserId].xp -= Stats[player.characterClassManager.UserId].to;
				Stats[player.characterClassManager.UserId].lvl++;
				Stats[player.characterClassManager.UserId].to = Stats[player.characterClassManager.UserId].lvl * 250 + 750;
				player.Broadcast(Configs.Lvlup.Replace("%lvl%", $"{Stats[player.characterClassManager.UserId].lvl}").Replace("%to.xp%", ((Stats[player.characterClassManager.UserId].to) - Stats[player.characterClassManager.UserId].xp).ToString()), 10);
				foreach (Stats stats in Stats.Values)
					Methods.SaveStats(stats);

				addp(player);
			}
		}
		public void addp(ReferenceHub player)
		{
			if (Stats[player.characterClassManager.UserId].lvl == 1)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "green");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 2)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "crimson");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 3)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "cyan");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 4)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "deep_pink");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 5)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "yellow");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 6)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "orange");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 7)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "lime");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 8)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "pumpkin");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 9)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "red");
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 10)
			{
				if (20 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "green");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 20)
			{
				if (30 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "crimson");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 30)
			{
				if (40 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "cyan");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 40)
			{
				if (50 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "deep_pink");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 50)
			{
				if (60 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "yellow");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 60)
			{
				if (70 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "orange");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 70)
			{
				if (80 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "lime");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 80)
			{
				if (90 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "pumpkin");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 90)
			{
				if (100 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "red");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 100)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", "red");
			}
			if (player.adminsearch())
			{
				player.SetRank($"{ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeText} " + Stats[player.characterClassManager.UserId].lvl + $" {Configs.Lvl}", ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeColor);
			}
		}
		public void OnCheckEscape(Exiled.Events.EventArgs.EscapingEventArgs ev)
		{
			ev.Player.ClearBroadcasts();
			Stats[ev.Player.ReferenceHub.characterClassManager.UserId].xp += 100;
			AddXP(ev.Player.ReferenceHub);
			ev.Player.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(ev.Player.ReferenceHub.scp079PlayerScript.connectionToClient, Configs.Eb.Replace("%xp%", "100"), 10, 0);
			string nick = ev.Player?.Nickname;
			MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
			if (matches.Count > 0)
			{
				ev.Player.ClearBroadcasts();
				Stats[ev.Player.ReferenceHub.characterClassManager.UserId].xp += 100;
				AddXP(ev.Player.ReferenceHub);
				ev.Player.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(ev.Player.ReferenceHub.scp079PlayerScript.connectionToClient, Configs.Eb.Replace("%xp%", "200"), 10, 0);
			}
		}

		public void OnPlayerDeath(Exiled.Events.EventArgs.DiedEventArgs ev)
		{
			ReferenceHub target = ev.Target.ReferenceHub;
			ReferenceHub killer = ev.Killer.ReferenceHub;
			string targetname = ev.Target?.Nickname;
			addp(target);
			List<Team> pList = Player.List.Select(x => Extensions.GetTeam2(x.ReferenceHub)).ToList();
			if (target == null || string.IsNullOrEmpty(target.characterClassManager.UserId))
				return;
			

			if (killer == null || string.IsNullOrEmpty(killer.characterClassManager.UserId))
				return;

			if (Stats.ContainsKey(killer.characterClassManager.UserId))
			{
				if (killer != target)
				{
					ev.Killer.ClearBroadcasts();
					if (killer == pList.Contains(Team.CHI))
					{
						if (target == pList.Contains(Team.MTF))
						{
							Stats[killer.characterClassManager.UserId].xp += 50;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.RSC))
						{
							Stats[killer.characterClassManager.UserId].xp += 25;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							Stats[killer.characterClassManager.UserId].xp += 75;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "75").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.TUT))
						{
							Stats[killer.characterClassManager.UserId].xp += 50;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.TUT))
					{
						if (target == pList.Contains(Team.CDP))
						{
							Stats[killer.characterClassManager.UserId].xp += 25;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.RSC))
						{
							Stats[killer.characterClassManager.UserId].xp += 25;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.MTF))
						{
							Stats[killer.characterClassManager.UserId].xp += 50;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.CHI))
						{
							Stats[killer.characterClassManager.UserId].xp += 50;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							Stats[killer.characterClassManager.UserId].xp += 10;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "10").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.RSC))
					{
						if (target == pList.Contains(Team.CDP))
						{
							Stats[killer.characterClassManager.UserId].xp += 50;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.CHI))
						{
							Stats[killer.characterClassManager.UserId].xp += 100;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.TUT))
						{
							Stats[killer.characterClassManager.UserId].xp += 100;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							Stats[killer.characterClassManager.UserId].xp += 200;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.CDP))
					{
						if (target == pList.Contains(Team.RSC))
						{
							Stats[killer.characterClassManager.UserId].xp += 50;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.MTF))
						{
							Stats[killer.characterClassManager.UserId].xp += 100;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							Stats[killer.characterClassManager.UserId].xp += 200;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.TUT))
						{
							Stats[killer.characterClassManager.UserId].xp += 100;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.SCP))
					{
						Stats[killer.characterClassManager.UserId].xp += 25;
						AddXP(killer);
						killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
					}
				}
				string nick = ev.Killer?.Nickname;
				MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0)
				{
					if (killer != target)
					{
						ev.Killer.ClearBroadcasts();
						if (killer == pList.Contains(Team.CHI))
						{
							if (target == pList.Contains(Team.MTF))
							{
								Stats[killer.characterClassManager.UserId].xp += 50;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.RSC))
							{
								Stats[killer.characterClassManager.UserId].xp += 25;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								Stats[killer.characterClassManager.UserId].xp += 75;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "150").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.TUT))
							{
								Stats[killer.characterClassManager.UserId].xp += 50;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.TUT))
						{
							if (target == pList.Contains(Team.CDP))
							{
								Stats[killer.characterClassManager.UserId].xp += 25;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.RSC))
							{
								Stats[killer.characterClassManager.UserId].xp += 25;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.MTF))
							{
								Stats[killer.characterClassManager.UserId].xp += 50;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.CHI))
							{
								Stats[killer.characterClassManager.UserId].xp += 50;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								Stats[killer.characterClassManager.UserId].xp += 10;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "20").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.RSC))
						{
							if (target == pList.Contains(Team.CDP))
							{
								Stats[killer.characterClassManager.UserId].xp += 50;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.CHI))
							{
								Stats[killer.characterClassManager.UserId].xp += 100;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.TUT))
							{
								Stats[killer.characterClassManager.UserId].xp += 100;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								Stats[killer.characterClassManager.UserId].xp += 200;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "400").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.CDP))
						{
							if (target == pList.Contains(Team.RSC))
							{
								Stats[killer.characterClassManager.UserId].xp += 50;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.MTF))
							{
								Stats[killer.characterClassManager.UserId].xp += 100;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								Stats[killer.characterClassManager.UserId].xp += 200;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "400").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.TUT))
							{
								Stats[killer.characterClassManager.UserId].xp += 100;
								AddXP(killer);
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.SCP))
						{
							Stats[killer.characterClassManager.UserId].xp += 25;
							AddXP(killer);
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
				}
			}
		}
		public void OnPocketDimensionDie(Exiled.Events.EventArgs.FailingEscapePocketDimensionEventArgs ev)
		{
			Player pscp106 = Player.List.Where(x => x.ReferenceHub.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			ReferenceHub scp106 = pscp106.ReferenceHub;
			Stats[scp106.characterClassManager.UserId].xp += 25;
			AddXP(scp106);
			pscp106.ClearBroadcasts();
			scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "25").Replace("%player%", $"{ev.Player?.Nickname}"), 10, 0);
			string nick = pscp106?.Nickname;
			MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
			if (matches.Count > 0)
			{
				pscp106.ClearBroadcasts();
				Stats[scp106.characterClassManager.UserId].xp += 25;
				AddXP(scp106);
				scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, Configs.Kb.Replace("%xp%", "50").Replace("%player%", $"{ev.Player?.Nickname}"), 10, 0);
			}
		}
		public void OnConsoleCommand(Exiled.Events.EventArgs.SendingConsoleCommandEventArgs ev)
		{
			string cmd = ev.Name.ToLower();
			if (cmd.StartsWith("xp"))
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n"+ Stats[ev.Player.ReferenceHub.characterClassManager.UserId].xp +"/"+ Stats[ev.Player.ReferenceHub.characterClassManager.UserId].to + "\nlvl:\n" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
			if (cmd.StartsWith("lvl"))
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].xp + "/" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].to + "\nlvl:\n" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
			if (cmd.StartsWith(Configs.Cc))
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].xp + "/" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].to + "\nlvl:\n" + Stats[ev.Player.ReferenceHub.characterClassManager.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
		}
	}
}