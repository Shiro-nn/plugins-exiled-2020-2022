using Loli.Addons;
using Loli.DataBase;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using DateTime = System.DateTime;
using System.Linq;
using UnityEngine;
using MEC;

namespace Loli.Spawns
{
	public class ChaosInsurgency
	{
		private readonly SpawnManager SpawnManager;
		public ChaosInsurgency(SpawnManager SpawnManager)
		{
			this.SpawnManager = SpawnManager;
			CommandsSystem.RegisterRemoteAdmin("hacker", HackerConsole);
		}
		internal bool First => ChaosSquads == 0;
		internal bool HackerSpawn => !Plugin.YouTubersServer && HackersSpawn < 7;
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
		internal void FixDies(DiesEvent ev)
		{
			if (Addons.RealSpawn.Chaos.AllowSpawn && ev.Target.Tag.Contains("ChaosSpawningInCar"))
				ev.Allowed = false;
		}
		internal void DoSpawn(DeadEvent ev)
		{
			Timing.CallDelayed(2f, () =>
			{
				if (Addons.RealSpawn.Chaos.AllowSpawn && ev.Target.Role == RoleType.Spectator)
				{
					ev.Target.Tag += "ChaosSpawningInCar";
					SpawnOne(ev.Target);
				}
			});
		}
		internal void DoSpawn(JoinEvent ev)
		{
			if (Addons.RealSpawn.Chaos.AllowSpawn)
			{
				ev.Player.Tag += "ChaosSpawningInCar";
				SpawnOne(ev.Player);
			}
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (Addons.RealSpawn.Chaos.AllowSpawn && ev.Player.Tag.Contains("ChaosSpawningInCar"))
				ev.Position = Addons.RealSpawn.Chaos.SpawnPoint.position;
		}
		internal DateTime LastCall = DateTime.Now;
		public void SpawnCI()
		{
			if (OmegaWarhead.InProgress) return;
			if (Alpha.Detonated) return;
			if (Plugin.ClansWars) return;
			if (Plugin.RolePlay && ChaosSquads > 5)
			{
				SpawnManager.MobileTaskForces.SpawnMtf();
				return;
			}
			if ((DateTime.Now - LastCall).TotalSeconds < 30) return;
			LastCall = DateTime.Now;
			var pls = Player.List.Where(x => x.Role is RoleType.Spectator && !x.Overwatch).ToList();
			if (!pls.Any()) return;
			if (First)
			{
				BackupPower.StartBackup(120f);
				ChaosSquads++;
			}
			pls.Shuffle();
			Addons.RealSpawn.Chaos.OpenGate(() =>
			{
				if (Plugin.RolePlay)
				{
					Cassie.Send("Attention to all personnel . .g6 . a security malfunction has been noticed pitch_.2 .g4 .g4 pitch_.8 " +
						"Attention . . the Chaos Insurgency have entered the facility . . All remaining ClassD personnel please wait as " +
						"the Chaos come to escort you out of the facility", false, false, true);
				}
				foreach (var pl in Player.List)
				{
					try
					{
						if (!pl.Tag.Contains("ChaosSpawningInCar")) continue;
						pl.Tag = pl.Tag.Replace("ChaosSpawningInCar", "");
						if (pl.GetTeam() != Team.CHI) continue;
						if ((pl.Tag.Contains(Roles.Hacker.HackerTag) || pl.Tag.Contains(Roles.Hacker.HackerGuardTag)) &&
						Vector3.Distance(pl.Position, Addons.RealSpawn.Chaos.SpawnPoint.position) > 10) continue;
						if (Random.Range(0, 100) > 50) pl.Position = new(Random.Range(-183, -176), Addons.RealSpawn.Chaos.SpawnPoint.position.y + 2, Random.Range(-56.5f, -55));
						else pl.Position = new(Random.Range(-183, -176), Addons.RealSpawn.Chaos.SpawnPoint.position.y + 3, Random.Range(-65, -66.5f));
					}
					catch (System.Exception e) { Qurre.Log.Error(e); }
				}
				Timing.CallDelayed(20, () =>
				{
					foreach (var pl in Player.List)
					{
						try
						{
							if (Vector3.Distance(pl.Position, Addons.RealSpawn.Chaos.SpawnPoint.position) > 7) continue;
							pl.Position = new(Random.Range(-183, -176), Addons.RealSpawn.Chaos.SpawnPoint.position.y + 3, Random.Range(-65, -66.5f));
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
			else if (pl.Role is RoleType.Spectator) pl.SetRole(_role);
		}
		private void HackerConsole(SendingRAEvent ev)
		{
			ev.Allowed = false;
			ev.Prefix = "SERVER_EVENT";
			if (!(ev.CommandSender.SenderId == "SERVER CONSOLE" || (Manager.Static.Data.Users.TryGetValue(ev.Player.UserId, out var _d) && _d.id == 1)))
			{
				ev.ReplyMessage = "Отказано в доступе";
				return;
			}
			string name = string.Join(" ", ev.Args);
			if (name.Contains("&"))
			{
				var names = name.Split('&');
				Player hacker = Player.Get(names[0]);
				Player guard = Player.Get(names[1]);
				if (hacker is null)
				{
					ev.ReplyMessage = "Хакер не найден";
					return;
				}
				if (guard is null)
				{
					ev.ReplyMessage = "Охранник не найден";
					return;
				}
				ev.ReplyMessage = "Успешно";
				Roles.Hacker.SpawnHacker(hacker, guard);
			}
			else
			{
				Player player = Player.Get(name);
				if (player is null)
				{
					ev.ReplyMessage = "Игрок не найден";
					return;
				}
				ev.ReplyMessage = "Успешно";
				Roles.Hacker.SpawnHacker(player);
			}
		}
	}
}