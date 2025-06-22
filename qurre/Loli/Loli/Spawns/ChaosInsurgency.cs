using Loli.Addons;
using Loli.DataBase;
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

		[EventMethod(PlayerEvents.Dies)]
		static void FixDies(DiesEvent ev)
		{
			if (Addons.RealSpawn.Chaos.AllowSpawn && ev.Target.Tag.Contains("ChaosSpawningInCar"))
				ev.Allowed = false;
		}

		[EventMethod(PlayerEvents.Dead)]
		static void DoSpawn(DeadEvent ev)
		{
			Timing.CallDelayed(2f, () =>
			{
				if (Addons.RealSpawn.Chaos.AllowSpawn && ev.Target.RoleInfomation.Role == RoleTypeId.Spectator)
				{
					ev.Target.Tag += "ChaosSpawningInCar";
					SpawnOne(ev.Target);
				}
			});
		}

		[EventMethod(PlayerEvents.Join)]
		static void DoSpawn(JoinEvent ev)
		{
			if (Addons.RealSpawn.Chaos.AllowSpawn)
			{
				ev.Player.Tag += "ChaosSpawningInCar";
				SpawnOne(ev.Player);
			}
		}

		[EventMethod(PlayerEvents.Spawn)]
		static void Spawn(SpawnEvent ev)
		{
			if (Addons.RealSpawn.Chaos.AllowSpawn && ev.Player.Tag.Contains("ChaosSpawningInCar"))
				ev.Position = Addons.RealSpawn.Chaos.SpawnPoint.position;
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
			Addons.RealSpawn.Chaos.OpenGate(() =>
			{
				foreach (var pl in Player.List)
				{
					try
					{
						if (!pl.Tag.Contains("ChaosSpawningInCar")) continue;
						pl.Tag = pl.Tag.Replace("ChaosSpawningInCar", "");
						if (pl.GetTeam() != Team.ChaosInsurgency) continue;
						if ((pl.Tag.Contains(Roles.Hacker.HackerTag) || pl.Tag.Contains(Roles.Hacker.HackerGuardTag)) &&
						Vector3.Distance(pl.MovementState.Position, Addons.RealSpawn.Chaos.SpawnPoint.position) > 10) continue;
						if (Random.Range(0, 100) > 50) pl.MovementState.Position = new(Random.Range(-183, -176), Addons.RealSpawn.Chaos.SpawnPoint.position.y + 2, Random.Range(-56.5f, -55));
						else pl.MovementState.Position = new(Random.Range(-183, -176), Addons.RealSpawn.Chaos.SpawnPoint.position.y + 3, Random.Range(-65, -66.5f));
					}
					catch (System.Exception e) { Log.Error(e); }
				}
				Timing.CallDelayed(20, () =>
				{
					foreach (var pl in Player.List)
					{
						try
						{
							if (Vector3.Distance(pl.MovementState.Position, Addons.RealSpawn.Chaos.SpawnPoint.position) > 7) continue;
							pl.MovementState.Position = new(Random.Range(-183, -176), Addons.RealSpawn.Chaos.SpawnPoint.position.y + 3, Random.Range(-65, -66.5f));
						}
						catch { }
					}
				});
			});
			foreach (var pl in pls)
			{
				pl.Tag += "ChaosSpawningInCar";
				SpawnOne(pl);
			}
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
			if (!(ev.Sender.SenderId == "SERVER CONSOLE" || (Data.Users.TryGetValue(ev.Player.UserInfomation.UserId, out var _d) && _d.id == 1)))
			{
				ev.Reply = "Отказано в доступе";
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
					ev.Reply = "Хакер не найден";
					return;
				}
				if (guard is null)
				{
					ev.Reply = "Охранник не найден";
					return;
				}
				ev.Reply = "Успешно";
				Roles.Hacker.SpawnHacker(hacker, guard);
			}
			else
			{
				Player player = name.GetPlayer();
				if (player is null)
				{
					ev.Reply = "Игрок не найден";
					return;
				}
				ev.Reply = "Успешно";
				Roles.Hacker.SpawnHacker(player);
			}
		}
	}
}