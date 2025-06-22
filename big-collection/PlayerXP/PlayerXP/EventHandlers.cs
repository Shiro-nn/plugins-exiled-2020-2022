using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using Grenades;
using MEC;
using System.Linq;
using UnityEngine;
using Mirror;
using Log = EXILED.Log;
using System.Text;
using System.Text.RegularExpressions;
namespace PlayerXP
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>(); 
		private readonly Regex regexSmartSiteReplacer = new Regex(@"#fydne");

		public void OnWaitingForPlayers()
		{
			Stats.Clear();
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
		}

		public void OnRoundStart()
		{
			foreach (Stats stats in Stats.Values)
			Coroutines.Add(Timing.RunCoroutine(SecondCounter()));
		}

		private IEnumerator<float> SecondCounter()
		{
			for (;;)
			{
				foreach (Stats stats in Stats.Values)

				yield return Timing.WaitForSeconds(1f);
			}
		}

		public void OnRoundEnd()
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

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			Timing.CallDelayed(100f, () => ev.Player.Broadcast("<color=red>���� �� �������� � ����</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>�� �� ������ �������� � 2 ���� ������ �����</color>", 15));
			if (string.IsNullOrEmpty(ev.Player.characterClassManager.UserId) || ev.Player.characterClassManager.IsHost || ev.Player.nicknameSync.MyNick == "Dedicated Server")
				return;
			
			if (!Stats.ContainsKey(ev.Player.characterClassManager.UserId))
				Stats.Add(ev.Player.characterClassManager.UserId, Methods.LoadStats(ev.Player.characterClassManager.UserId));
			Timing.CallDelayed(1.5f, () => {
				if (Stats[ev.Player.characterClassManager.UserId].lvl == 1)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "green");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 2)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "crimson");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 3)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "cyan");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 4)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "deep_pink");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 5)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "yellow");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 6)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "orange");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 7)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 8)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "pumpkin");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl == 9)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 10)
				{
					if (20 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "green");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 20)
				{
					if (30 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "crimson");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 30)
				{
					if (40 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "cyan");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 40)
				{
					if (50 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "deep_pink");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 50)
				{
					if (60 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "yellow");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 60)
				{
					if (70 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "orange");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 70)
				{
					if (80 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 80)
				{
					if (90 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "pumpkin");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 90)
				{
					if (100 >= Stats[ev.Player.characterClassManager.UserId].lvl)
					{
						ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
					}
				}
				else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 100)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (ev.Player.IshelperTagUser())
				{
					ev.Player.SetRank("Helper " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "aqua");
				}
				if (ev.Player.IsgoodTagUser())
				{
					ev.Player.SetRank("��.����� " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (ev.Player.IseliteTagUser())
				{
					ev.Player.SetRank("ELITE " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (ev.Player.IspremiumTagUser())
				{
					ev.Player.SetRank("Premium " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
				}
				if (ev.Player.IsvipTagUser())
				{
					ev.Player.SetRank("Vip " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (ev.Player.IsvipplusTagUser())
				{
					ev.Player.SetRank("Vip+ " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "orange");
				}
				if (ev.Player.IsstaTagUser())
				{
					ev.Player.SetRank("������ " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
				}
				if (ev.Player.IsviphelperTagUser())
				{
					ev.Player.SetRank("������� ������ " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "blue_green");
				}
				if (ev.Player.IsadminTagUser())
				{
					ev.Player.SetRank("����� " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "yellow");
				}
			});
		}
		public void OnPlayerSpawn(PlayerSpawnEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.characterClassManager.UserId) || ev.Player.characterClassManager.IsHost || ev.Player.nicknameSync.MyNick == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.characterClassManager.UserId))
				Stats.Add(ev.Player.characterClassManager.UserId, Methods.LoadStats(ev.Player.characterClassManager.UserId));
			if (Stats[ev.Player.characterClassManager.UserId].lvl == 1)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "green");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 2)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "crimson");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 3)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "cyan");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 4)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "deep_pink");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 5)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "yellow");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 6)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "orange");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 7)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 8)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "pumpkin");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl == 9)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 10)
			{
				if (20 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "green");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 20)
			{
				if (30 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "crimson");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 30)
			{
				if (40 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "cyan");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 40)
			{
				if (50 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "deep_pink");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 50)
			{
				if (60 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "yellow");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 60)
			{
				if (70 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "orange");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 70)
			{
				if (80 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 80)
			{
				if (90 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "pumpkin");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 90)
			{
				if (100 >= Stats[ev.Player.characterClassManager.UserId].lvl)
				{
					ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
			}
			else if (Stats[ev.Player.characterClassManager.UserId].lvl >= 100)
			{
				ev.Player.SetRank(Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (ev.Player.IshelperTagUser())
			{
				ev.Player.SetRank("Helper " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "aqua");
			}
			if (ev.Player.IsgoodTagUser())
			{
				ev.Player.SetRank("��.����� " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (ev.Player.IseliteTagUser())
			{
				ev.Player.SetRank("ELITE " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (ev.Player.IspremiumTagUser())
			{
				ev.Player.SetRank("Premium " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
			}
			if (ev.Player.IsvipTagUser())
			{
				ev.Player.SetRank("Vip " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (ev.Player.IsvipplusTagUser())
			{
				ev.Player.SetRank("Vip+ " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "orange");
			}
			if (ev.Player.IsstaTagUser())
			{
				ev.Player.SetRank("������ " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "lime");
			}
			if (ev.Player.IsviphelperTagUser())
			{
				ev.Player.SetRank("������� ������ " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "blue_green");
			}
			if (ev.Player.IsadminTagUser())
			{
				ev.Player.SetRank("����� " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "yellow");
			}
		}
		public void AddXP(ReferenceHub player)
		{
			Stats[player.characterClassManager.UserId].to = Stats[player.characterClassManager.UserId].lvl * 250 + 750;
			if (Stats[player.characterClassManager.UserId].xp >= Stats[player.characterClassManager.UserId].to)
			{
				Stats[player.characterClassManager.UserId].xp -= Stats[player.characterClassManager.UserId].to;
				Stats[player.characterClassManager.UserId].lvl++;
				Stats[player.characterClassManager.UserId].to = Stats[player.characterClassManager.UserId].lvl * 250 + 750;
				player.Broadcast("<color=#fdffbb>�� �������� " + Stats[player.characterClassManager.UserId].lvl + " �������! �� ���������� ������ ��� �� ������� " + ((Stats[player.characterClassManager.UserId].to) - Stats[player.characterClassManager.UserId].xp).ToString() + "xp.</color>", 10);
				foreach (Stats stats in Stats.Values)
					Methods.SaveStats(stats);

				if (Stats[player.characterClassManager.UserId].lvl == 1)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "green");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 2)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "crimson");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 3)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "cyan");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 4)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "deep_pink");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 5)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "yellow");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 6)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "orange");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 7)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 8)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "pumpkin");
				}
				else if (Stats[player.characterClassManager.UserId].lvl == 9)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "red");
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 10)
				{
					if (20 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "green");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 20)
				{
					if (30 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "crimson");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 30)
				{
					if (40 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "cyan");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 40)
				{
					if (50 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "deep_pink");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 50)
				{
					if (60 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "yellow");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 60)
				{
					if (70 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "orange");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 70)
				{
					if (80 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 80)
				{
					if (90 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "pumpkin");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 90)
				{
					if (100 >= Stats[player.characterClassManager.UserId].lvl)
					{
						player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "red");
					}
				}
				else if (Stats[player.characterClassManager.UserId].lvl >= 100)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (player.IshelperTagUser())
				{
					player.SetRank("Helper " + Stats[player.characterClassManager.UserId].lvl + " �������", "aqua");
				}
				if (player.IsgoodTagUser())
				{
					player.SetRank("��.����� " + Stats[player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (player.IseliteTagUser())
				{
					player.SetRank("ELITE " + Stats[player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (player.IspremiumTagUser())
				{
					player.SetRank("Premium " + Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
				}
				if (player.IsvipTagUser())
				{
					player.SetRank("Vip " + Stats[player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (player.IsvipplusTagUser())
				{
					player.SetRank("Vip+ " + Stats[player.characterClassManager.UserId].lvl + " �������", "orange");
				}
				if (player.IsstaTagUser())
				{
					player.SetRank("������ " + Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
				}
				if (player.IsviphelperTagUser())
				{
					player.SetRank("������� ������ " + Stats[player.characterClassManager.UserId].lvl + " �������", "blue_green");
				}
				if (player.IsadminTagUser())
				{
					player.SetRank("����� " + Stats[player.characterClassManager.UserId].lvl + " �������", "yellow");
				}
			}
		}
		public void addp(ReferenceHub player)
		{
			if (Stats[player.characterClassManager.UserId].lvl == 1)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "green");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 2)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "crimson");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 3)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "cyan");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 4)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "deep_pink");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 5)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "yellow");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 6)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "orange");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 7)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 8)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "pumpkin");
			}
			else if (Stats[player.characterClassManager.UserId].lvl == 9)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "red");
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 10)
			{
				if (20 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "green");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 20)
			{
				if (30 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "crimson");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 30)
			{
				if (40 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "cyan");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 40)
			{
				if (50 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "deep_pink");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 50)
			{
				if (60 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "yellow");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 60)
			{
				if (70 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "orange");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 70)
			{
				if (80 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 80)
			{
				if (90 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "pumpkin");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 90)
			{
				if (100 >= Stats[player.characterClassManager.UserId].lvl)
				{
					player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "red");
				}
			}
			else if (Stats[player.characterClassManager.UserId].lvl >= 100)
			{
				player.SetRank(Stats[player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if(player.IshelperTagUser())
			{
				player.SetRank("Helper " +Stats[player.characterClassManager.UserId].lvl + " �������", "aqua");
			}
			if (player.IsgoodTagUser())
			{
				player.SetRank("��.����� " + Stats[player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (player.IseliteTagUser())
			{
				player.SetRank("ELITE " + Stats[player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (player.IspremiumTagUser())
			{
				player.SetRank("Premium " + Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
			}
			if (player.IsvipTagUser())
			{
				player.SetRank("Vip " + Stats[player.characterClassManager.UserId].lvl + " �������", "red");
			}
			if (player.IsvipplusTagUser())
			{
				player.SetRank("Vip+ " + Stats[player.characterClassManager.UserId].lvl + " �������", "orange");
			}
			if (player.IsstaTagUser())
			{
				player.SetRank("������ " + Stats[player.characterClassManager.UserId].lvl + " �������", "lime");
			}
			if (player.IsviphelperTagUser())
			{
				player.SetRank("������� ������ " + Stats[player.characterClassManager.UserId].lvl + " �������", "blue_green");
			}
			if (player.IsadminTagUser())
			{
				player.SetRank("����� " + Stats[player.characterClassManager.UserId].lvl + " �������", "yellow");
			}
		}
		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			List<Team> pList = Player.GetHubs().Select(x => Player.GetTeam(x)).ToList();
			ev.Player.ClearBroadcasts();
			if (ev.Player == pList.Contains(Team.RSC))
			{
				Stats[ev.Player.characterClassManager.UserId].xp += 100;
				AddXP(ev.Player);
				ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� �����</color>", 10, true);
			}
			if (ev.Player == pList.Contains(Team.CDP))
			{
				Stats[ev.Player.characterClassManager.UserId].xp += 100;
				AddXP(ev.Player);
				ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� �����</color>", 10, true);
			}
			string nick = ev.Player.GetNickname();
			MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
			if (matches.Count > 0)
			{
				ev.Player.ClearBroadcasts();
				if (ev.Player == pList.Contains(Team.RSC))
				{
					Stats[ev.Player.characterClassManager.UserId].xp += 100;
					AddXP(ev.Player);
					ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� �����</color>", 10, true);
				}
				if (ev.Player == pList.Contains(Team.CDP))
				{
					Stats[ev.Player.characterClassManager.UserId].xp += 100;
					AddXP(ev.Player);
					ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� �����</color>", 10, true);
				}
			}
		}
		public void OnPlayerDeath(ref PlayerDeathEvent ev)
		{
			addp(ev.Player);
			List<Team> pList = Player.GetHubs().Select(x => Player.GetTeam(x)).ToList();
			if (ev.Player == null || string.IsNullOrEmpty(ev.Player.characterClassManager.UserId))
				return;
			

			if (ev.Killer == null || string.IsNullOrEmpty(ev.Killer.characterClassManager.UserId))
				return;

			if (Stats.ContainsKey(ev.Killer.characterClassManager.UserId))
			{
				if (ev.Killer != ev.Player)
				{
					ev.Killer.ClearBroadcasts();
					if (ev.Killer == pList.Contains(Team.CHI))
					{
						if (ev.Player == pList.Contains(Team.MTF))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 50;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.RSC))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 25;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 25xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.SCP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 75;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 75xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.TUT))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 50;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
					}
					else if (ev.Killer == pList.Contains(Team.TUT))
					{
						if (ev.Player == pList.Contains(Team.CDP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 25;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 25xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.RSC))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 25;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 25xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.MTF))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 50;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.CHI))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 50;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.SCP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 10;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 10xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
					}
					else if (ev.Killer == pList.Contains(Team.RSC))
					{
						if (ev.Player == pList.Contains(Team.CDP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 50;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.CHI))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 100;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.TUT))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 100;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.SCP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 200;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
					}
					else if (ev.Killer == pList.Contains(Team.CDP))
					{
						if (ev.Player == pList.Contains(Team.RSC))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 50;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.MTF))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 100;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.SCP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 200;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
						else if (ev.Player == pList.Contains(Team.TUT))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 100;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
					}
					else if (ev.Killer == pList.Contains(Team.SCP))
					{
						Stats[ev.Killer.characterClassManager.UserId].xp += 25;
						AddXP(ev.Killer);
						ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 25xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
					}
				}
				string nick = ev.Killer.GetNickname();
				MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0)
				{
					if (ev.Killer != ev.Player)
					{
						ev.Killer.ClearBroadcasts();
						if (ev.Killer == pList.Contains(Team.CHI))
						{
							if (ev.Player == pList.Contains(Team.MTF))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 50;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.RSC))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 25;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.SCP))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 75;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 150xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.TUT))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 50;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
						}
						else if (ev.Killer == pList.Contains(Team.TUT))
						{
							if (ev.Player == pList.Contains(Team.CDP))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 25;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.RSC))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 25;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.MTF))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 50;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.CHI))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 50;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.SCP))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 10;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 20xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
						}
						else if (ev.Killer == pList.Contains(Team.RSC))
						{
							if (ev.Player == pList.Contains(Team.CDP))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 50;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.CHI))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 100;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.TUT))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 100;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.SCP))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 200;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 400xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
						}
						else if (ev.Killer == pList.Contains(Team.CDP))
						{
							if (ev.Player == pList.Contains(Team.RSC))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 50;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 100xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.MTF))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 100;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.SCP))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 200;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 400xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
							else if (ev.Player == pList.Contains(Team.TUT))
							{
								Stats[ev.Killer.characterClassManager.UserId].xp += 100;
								AddXP(ev.Killer);
								ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 200xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
							}
						}
						else if (ev.Killer == pList.Contains(Team.SCP))
						{
							Stats[ev.Killer.characterClassManager.UserId].xp += 25;
							AddXP(ev.Killer);
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
						}
					}
				}
			}
		}
		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
			Stats[scp106.characterClassManager.UserId].xp += 25;
			AddXP(scp106);
			scp106.ClearBroadcasts();
			scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 25xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
			string nick = scp106.GetNickname();
			MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
			if (matches.Count > 0)
			{
				scp106.ClearBroadcasts();
				Stats[scp106.characterClassManager.UserId].xp += 25;
				AddXP(scp106);
				scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 50xp �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 10, true);
			}
		}
		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			string cmd = ev.Command.ToLower();
			if (cmd.StartsWith("xp"))
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n"+ Stats[ev.Player.characterClassManager.UserId].xp +"/"+ Stats[ev.Player.characterClassManager.UserId].to + "\nlvl:\n" + Stats[ev.Player.characterClassManager.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
			if (cmd.StartsWith("lvl"))
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n" + Stats[ev.Player.characterClassManager.UserId].xp + "/" + Stats[ev.Player.characterClassManager.UserId].to + "\nlvl:\n" + Stats[ev.Player.characterClassManager.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
			if (cmd.StartsWith("�������"))
			{
				ev.ReturnMessage = "\n----------------------------------------------------------- \nXP:\n" + Stats[ev.Player.characterClassManager.UserId].xp + "/" + Stats[ev.Player.characterClassManager.UserId].to + "\nlvl:\n" + Stats[ev.Player.characterClassManager.UserId].lvl + "\n -----------------------------------------------------------";
				ev.Color = "red";
			}
			if (cmd.StartsWith("prefix"))
			{
				if(ev.Player.IslgbtTagUser())
				{
					string[] args = ev.Command.Split(' ');
					ev.Player.SetRank($"{args[1]} " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
				}
				if (!ev.Player.IslgbtTagUser())
				{
					ev.ReturnMessage = "��� ������� �������";
					ev.Color = "lime";
				}
			}
			if (cmd.StartsWith("rainbow"))
			{
				if (ev.Player.IslgbtTagUser())
				{
					string[] args = ev.Command.Split(' ');
					ev.Player.SetRank($"{args[1]} " + Stats[ev.Player.characterClassManager.UserId].lvl + " �������", "red");
					ev.ReturnMessage = "��� ������� �������";
					ev.Color = "lime";
				}
				if (!ev.Player.IslgbtTagUser())
				{
					ev.ReturnMessage = "� ��� ����������� �������� ����, ���������� �� �� ������ �� ������: \nhttps://fydne.xyz/donate";
					ev.Color = "red";
				}
			}
		}
	}
}