using Loli.Addons;
using Loli.DataBase;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Respawning;
using DateTime = System.DateTime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Spawns
{
	public class ChaosInsurgency
	{
		private readonly SpawnManager SpawnManager;
		public ChaosInsurgency(SpawnManager SpawnManager) => this.SpawnManager = SpawnManager;
		internal bool First => ChaosSquads == 0;
		internal bool HackerSpawn => HackersSpawn < 7;
		private int HackersSpawn = 0;
		private int ChaosSquads = 0;
		internal void WaitingForPlayers()
		{
			HackersSpawn = 0;
			ChaosSquads = 0;
		}
		internal void RoundStart()
		{
			HackersSpawn = 0;
			ChaosSquads = 0;
		}
		internal DateTime LastCall = DateTime.Now;
		public void SpawnCI()
		{
			if (Alpha.Detonated) return;
			if (Plugin.ClansWars) return;
			if (Plugin.RolePlay && ChaosSquads > 5)
			{
				SpawnManager.MobileTaskForces.SpawnMtf();
				return;
			}
			if ((DateTime.Now - LastCall).TotalSeconds < 30) return;
			LastCall = DateTime.Now;
			bool _backup_enabled = false;
			if (First && Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).Count() > 0)
			{
				BackupPower.StartBackup(60f);
				_backup_enabled = true;
				ChaosSquads++;
			}
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			Timing.CallDelayed(15f, () =>
			{
				if (Alpha.Detonated) return;
				List<Player> list = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToList();
				if (list.Count == 0) return;
				if (First) BackupPower.StartBackup(60f);
				if (!_backup_enabled) ChaosSquads++;
				list.Shuffle();
				Qurre.Events.Invoke.Round.TeamRespawn(new TeamRespawnEvent(list, 999, SpawnableTeamType.ChaosInsurgency));
				foreach (Player ci in list) SpawnOne(ci);
				if (Plugin.RolePlay)
				{
					Cassie.Send("Attention to all personnel . .g6 . a security malfunction has been noticed pitch_.2 .g4 .g4 pitch_.8 " +
						"Attention . . the Chaos Insurgency have entered the facility . . All remaining ClassD personnel please wait as " +
						"the Chaos come to escort you out of the facility", false, false, true);
				}
			});
		}
		public void SpawnOne(Player pl)
		{
			RoleType _role = RoleType.ChaosRifleman;
			var rand = Random.Range(0, 100);
			if (rand > 66) _role = RoleType.ChaosRepressor;
			else if (rand > 33) _role = RoleType.ChaosMarauder;
			SpawnManager.SpawnProtect(pl);
			if (HackerSpawn)
			{
				Roles.Hacker.SpawnHacker(pl);
				HackersSpawn++;
			}
			else if (pl.Role == RoleType.Spectator) pl.SetRole(_role);
		}
		public void Ra(SendingRAEvent ev)
		{
			if (ev.Name == "hacker")
			{
				ev.Allowed = false;
				ev.Prefix = "SERVER_EVENT";
				try
				{
					if (Manager.Static.Data.Users[ev.Player.UserId].or || ev.CommandSender.SenderId == "SERVER CONSOLE")
					{
						string name = string.Join(" ", ev.Args);
						if (name.Contains("&"))
						{
							var names = name.Split('&');
							Player hacker = Extensions.GetPlayer(names[0]);
							Player guard = Extensions.GetPlayer(names[1]);
							if (hacker == null)
							{
								ev.ReplyMessage = "Хакер не найден";
								return;
							}
							if (guard == null)
							{
								ev.ReplyMessage = "Охранник не найден";
								return;
							}
							ev.ReplyMessage = "Успешно";
							Roles.Hacker.SpawnHacker(hacker, guard);
						}
						else
						{
							Player player = Extensions.GetPlayer(name);
							if (player == null)
							{
								ev.ReplyMessage = "Игрок не найден";
								return;
							}
							ev.ReplyMessage = "Успешно";
							Roles.Hacker.SpawnHacker(player);
						}
					}
					else
					{
						ev.ReplyMessage = "Отказано в доступе";
					}
				}
				catch
				{
					ev.ReplyMessage = "Произошла ошибка";
					return;
				}
			}
		}
	}
}