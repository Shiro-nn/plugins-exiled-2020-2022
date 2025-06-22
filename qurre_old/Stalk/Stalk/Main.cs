using MEC;
using Qurre.API;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Stalk
{
	internal static class Main
	{
		private static float cooldown = 0;
		private static System.DateTime LastClick = System.DateTime.Now;
		internal static void ChangeRole(RoleChangeEvent ev)
		{
			if (ev.NewRole == RoleType.Scp106)
			{
				cooldown = Cfg.CoolDown;
				ev.Player.Broadcast(10, Cfg.SpawnBC);
			}
		}
		internal static void CreatePortal(PortalCreateEvent ev) => ev.Allowed = Stalk(ev.Player);
		internal static bool Stalk(Player player)
		{
			if ((System.DateTime.Now - LastClick).TotalSeconds > 5)
			{
				LastClick = System.DateTime.Now;
				return true;
			}
			if (cooldown < 1)
			{
				cooldown = Cfg.CoolDown + 5;
				StalkCoroutine(player);
				return false;
			}
			else if (Cfg.CoolDown >= cooldown)
			{
				Timing.RunCoroutine(TimeBC(player));
				return true;
			}
			else return false;
		}
		internal static IEnumerator<float> TimeBC(Player pl)
		{
			var bc = pl.Broadcast(6, Cfg.Cooldown.Replace("%cooldown%", $"{cooldown:00}"), true);
			for (int i = 0; i < 5 && cooldown > 0; i++)
			{
				yield return Timing.WaitForSeconds(1f);
				bc.Message = Cfg.Cooldown.Replace("%cooldown%", $"{cooldown:00}");
			}
		}
		internal static IEnumerator<float> CooldownUpdate()
		{
			for (; ; )
			{
				if (cooldown > 0) cooldown--;
				yield return Timing.WaitForSeconds(1f);
			}
		}
		internal static void StalkCoroutine(Player player)
		{
			List<Player> list = Player.List.Where(x => !x.Tag.Contains("scp035") &&
			x.Role != RoleType.Tutorial && x.Role != RoleType.Spectator && x.Team != Team.SCP && x.Team != Team.CHI).ToList();
			Scp106PlayerScript scp106Script = player.GameObject.GetComponent<Scp106PlayerScript>();
			if (list.IsEmpty())
			{
				player.Broadcast(4, Cfg.TargetsZero);
				return;
			}
			Timing.CallDelayed(0.1f, () =>
			{
				Player target = FindTarget(list, scp106Script.teleportPlacementMask, out Vector3 portalPosition);
				if (target == default || (Vector3.Distance(portalPosition, new Vector3(0f, -1998f, 0f)) < 40f))
				{
					player.Broadcast(4, Cfg.TargetsZero);
					return;
				}
				if (portalPosition.Equals(Vector3.zero))
				{
					player.Broadcast(4, Cfg.TryAgain);
					return;
				}
				Timing.CallDelayed(0.1f, () =>
				{
					if (player.Role != RoleType.Scp106) return;
					scp106Script.NetworkportalPosition = portalPosition - Vector3.up;
					Timing.CallDelayed(0.2f, () =>
					{
						if (player.Role != RoleType.Scp106) return;
						scp106Script.NetworkportalPosition = portalPosition - Vector3.up;
						player.Scp106Controller.UsePortal();
						string color = "#ffffff";
						if (target.Team == Team.CDP) color = "#FF8E00";
						else if (target.Team == Team.RSC) color = "#fdffbb";
						else if (target.Role == RoleType.NtfPrivate) color = "#00a5ff";
						else if (target.Role == RoleType.NtfCaptain) color = "#0200ff";
						else if (target.Role == RoleType.NtfSergeant) color = "#0074ff";
						else if (target.Role == RoleType.NtfSpecialist) color = "#1f7fff";
						player.Broadcast(6, Cfg.Target.Replace("%target.Role%", $"{target.Role}").Replace("%target.Nickname%", target.Nickname).Replace("%color%", color), true);
					});
				});
			});
		}
		private static Player FindTarget(List<Player> validPlayerList, LayerMask teleportPlacementMask, out Vector3 portalPosition)
		{
			Player target;
			do
			{
				int index = Extensions.Random.Next(0, validPlayerList.Count);
				target = validPlayerList[index];
				Physics.Raycast(new Ray(target.GameObject.transform.position, -Vector3.up), out RaycastHit raycastHit, 10f, teleportPlacementMask);
				portalPosition = raycastHit.point;
				validPlayerList.RemoveAt(index);
			} while ((portalPosition.Equals(Vector3.zero) || Vector3.Distance(portalPosition, new Vector3(0f, -1998f, 0f)) < 40f) && validPlayerList.Count > 0);
			return target;
		}
	}
}