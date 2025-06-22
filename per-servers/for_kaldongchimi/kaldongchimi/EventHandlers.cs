using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using Grenades;
namespace kaldongchimi
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static bool cw = false;
		public static float forceShoot = 100.0f;
		public static float rangeShoot = 7.0f;
		private readonly int grenade_pickup_mask = 1049088;
		public void Shoot(ref ShootEvent ev)
		{
			if (Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
			{
				var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
				if (pickup != null && pickup.Rb != null)
				{
					pickup.Rb.AddExplosionForce(Vector3.Distance(ev.TargetPos, ev.Shooter.GetPosition()), ev.Shooter.GetPosition(), 500f, 3f, ForceMode.Impulse);
				}

				var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
				if (grenade != null)
				{
					grenade.NetworkfuseTime = 0.1f;
				}
			}
		}
		public void Shot(ref ShootEvent ev)
		{
			RaycastHit info;
			if (Physics.Linecast(ev.Shooter.GetComponent<Scp049PlayerScript>().plyCam.transform.position, ev.TargetPos, out info))
			{
				Collider[] arr = Physics.OverlapSphere(info.point, rangeShoot);
				foreach (Collider collider in arr)
				{
					if (collider.GetComponent<Pickup>() != null)
					{
						collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, info.point, rangeShoot);
					}
				}
			}
			else
			{
				Collider[] arr = Physics.OverlapSphere(ev.TargetPos, rangeShoot);
				foreach (Collider collider in arr)
				{
					if (collider.GetComponent<Pickup>() != null)
					{
						collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, ev.TargetPos, rangeShoot);
					}
				}
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.TargetPos != Vector3.zero
				&& Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
			{
				var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
				if (pickup != null && pickup.Rb != null)
				{
					pickup.Rb.AddExplosionForce(Vector3.Distance(ev.TargetPos, ev.Shooter.GetPosition()), ev.Shooter.GetPosition(), 500f, 3f, ForceMode.Impulse);
				}

				var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
				if (grenade != null)
				{
					grenade.NetworkfuseTime = 0.1f;
				}
			}
		}
		public void RoundStart()
		{
			cw = false;
		}

		public void RoundEnd()
		{
			cw = false;
		}
		public void WarheadCancel(WarheadCancelEvent ev)
		{
			if (cw)
			{
				ev.Allow = false;
			}
		}
		public void PlayerDeath(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == ev.Killer?.queryProcessor.PlayerId) return;
			List<ReferenceHub> players = Plugin.GetHubs();
			foreach (ReferenceHub player in players)
			{
				if (player.owneruser())
				{
					player.Broadcast(Configs.killer.Replace("%killer.name%", $"{ev.Killer.GetNickname()}").Replace("%killer.role%", $"{ev.Killer.GetRole()}").Replace("%player.name%", $"{ev.Killer.GetNickname()}").Replace("%player.role%", $"{ev.Killer.GetRole()}"), Configs.killerdur);
					//player.Broadcast($"<size=20><color=aqua>{ev.Killer.GetNickname()}</color><color=red>(</color><color=aqua>{ev.Killer.GetRole()}</color><color=red>) killed</color> <color=aqua>{ev.Player.GetNickname()}</color><color=red>(</color><color=aqua>{ev.Player.GetRole()}</color><color=red>)</color></size>", 5);
				}
			}
		}
		public void CheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			if (!RoundSummary.RoundInProgress() || (double)(float)RoundSummary.roundTime >= (double)Configs.alphastart)
			{
				if (Configs.nuke)
				{
					if (!cw)
					{
						Map.StartNuke();
						Map.Broadcast(Configs.aabc, Configs.aabctime);
						cw = true;
					}
				}
			}
		}
		public void RunOnRACommandSent(ref RACommandEvent ev)
		{
			string[] command = ev.Command.Split(' ');
			ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(ev.Sender.SenderId);
			ReferenceHub playerRA = Plugin.GetPlayer(command[1]);
			if (command[0] == Configs.pc)
			{
				ev.Allow = false;
				if (!sender.CheckPermission("at.playersee"))
				{
					ev.Sender.RAMessage(Configs.ad);
					return;
				}
				List<ReferenceHub> players = Plugin.GetHubs();
				foreach (ReferenceHub player in players)
				{
					if (player == null) return;
					string pLister = Configs.pig;
					pLister += $"{player.GetNickname()}[{player.queryProcessor.PlayerId}] - {player.GetRole()}\n";
					ev.Sender.RAMessage(pLister);
				}
			}
			if (command[0] == Configs.am)
			{
				ev.Allow = false;
				if (!sender.CheckPermission("at.am"))
				{
					ev.Sender.RAMessage(Configs.ad);
					return;
				}
				if (EventPlugin.GhostedIds.Contains(sender.queryProcessor.PlayerId))
				{
					EventPlugin.GhostedIds.Remove(sender.queryProcessor.PlayerId);
					ev.Sender.RAMessage(Configs.amd);
					return;
				}

				EventPlugin.GhostedIds.Add(sender.queryProcessor.PlayerId);
				ev.Sender.RAMessage(Configs.ame);
				return;
			}
			if (command[0] == Configs.sl)
			{
				ev.Allow = false;
				if (!sender.CheckPermission("at.sl"))
				{
					ev.Sender.RAMessage(Configs.ad);
					return;
				}
				IEnumerable<ReferenceHub> SCPs = Player.GetHubs().Where(rh => rh.GetTeam() == Team.SCP);
				string response = Configs.asd;
				foreach (ReferenceHub scp in SCPs)
				{
					response += $"\n{scp.GetNickname()} - {scp.characterClassManager.Classes.SafeGet(scp.GetRole()).fullName}";
				}
				ev.Sender.RAMessage(response);
			}
		}
	}
}

