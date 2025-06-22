using Loli.Addons;
using Qurre.API;
using Qurre.API.Controllers;
using DateTime = System.DateTime;
using System.Linq;
using UnityEngine;
using MEC;
using Qurre.Events.Structs;
using Qurre.API.Attributes;
using Qurre.Events;
using PlayerRoles;
using Loli.DataBase.Modules;

namespace Loli.Spawns
{
	static class ChaosInsurgency
	{
		static ChaosInsurgency()
		{
			CommandsSystem.RegisterRemoteAdmin("hacker", HackerConsole);
		}

		static internal DateTime LastCall = DateTime.Now;
		static internal bool First => ChaosSquads == 0;
		static internal bool HackerSpawn => HackersSpawn < 7;
		static int HackersSpawn = 0;
		static int ChaosSquads = 0;


		[EventMethod(RoundEvents.Waiting)]
		static void WaitingForPlayers()
		{
			HackersSpawn = 0;
			ChaosSquads = 0;
		}

		[EventMethod(RoundEvents.Start)]
		static void RoundStart()
		{
			HackersSpawn = 0;
			ChaosSquads = 0;
		}

		static public void SpawnCI()
		{
			if (OmegaWarhead.InProgress) return;
			if (Alpha.Detonated) return;
			if ((DateTime.Now - LastCall).TotalSeconds < 30) return;
			LastCall = DateTime.Now;
			var pls = Player.List.Where(x => x.RoleInfomation.Role is RoleTypeId.Spectator && !x.GamePlay.Overwatch).ToList();
			if (!pls.Any()) return;
			if (First)
			{
				BackupPower.StartBackup(120f);
				ChaosSquads++;
			}
			pls.Shuffle();
			foreach (var pl in pls)
				SpawnOne(pl);
		}
		static public void SpawnOne(Player pl)
		{
			RoleTypeId _role = RoleTypeId.ChaosRifleman;
			var rand = Random.Range(0, 100);
			if (rand > 66) _role = RoleTypeId.ChaosRepressor;
			else if (rand > 33) _role = RoleTypeId.ChaosMarauder;
			SpawnManager.SpawnProtect(pl);
			if (HackerSpawn)
			{
				Roles.Hacker.SpawnHacker(pl);
				HackersSpawn++;
			}
			else if (pl.RoleInfomation.Role is RoleTypeId.Spectator)
				pl.RoleInfomation.SetNew(_role, RoleChangeReason.Respawn);
		}
		static void HackerConsole(RemoteAdminCommandEvent ev)
		{
			ev.Allowed = false;
			ev.Prefix = "SERVER_EVENT";
			if (!(ev.Sender.SenderId == "SERVER CONSOLE" || (Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var _d) && _d.administration.owner)))
			{
				ev.Reply = "Access denied";
				return;
			}
			string name = string.Join(" ", ev.Args);
			if (name.Contains("&"))
			{
				var names = name.Split('&');
				Player hacker = names[0].GetPlayer();
				Player guard = names[1].GetPlayer();
				if (hacker is null)
				{
					ev.Reply = "Hacker not found";
					return;
				}
				if (guard is null)
				{
					ev.Reply = "Guard not found";
					return;
				}
				ev.Reply = "Successfully";
				Roles.Hacker.SpawnHacker(hacker, guard);
			}
			else
			{
				Player player = name.GetPlayer();
				if (player is null)
				{
					ev.Reply = "Player not found";
					return;
				}
				ev.Reply = "Successfully";
				Roles.Hacker.SpawnHacker(player);
			}
		}
	}
}