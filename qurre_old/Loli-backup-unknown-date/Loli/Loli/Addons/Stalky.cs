using Loli.Scps.Api;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Addons
{
	public class Stalky
	{
		private float cooldown = 0;
		private System.DateTime LastClick = System.DateTime.Now;
		internal void SetClass(RoleChangeEvent ev)
		{
			if (ev.NewRole == RoleType.Scp106)
			{
				cooldown = 25;
				ev.Player.Broadcast(10, "<size=30%><color=#6f6f6f>Дважды нажав на создание портала, вы телепортируетесь к рандомной цели</color></size>");
			}
		}
		internal void CreatePortal(PortalCreateEvent ev) => ev.Allowed = Stalk(ev.Player);
		internal bool Stalk(Player player)
		{
			if ((System.DateTime.Now - LastClick).TotalSeconds > 5)
			{
				LastClick = System.DateTime.Now;
				return true;
			}
			if (cooldown < 1)
			{
				cooldown = 30;
				StalkCoroutine(player);
				return false;
			}
			else if (25 >= cooldown)
			{
				Timing.RunCoroutine(TimeBC(player));
				return true;
			}
			else return false;
		}
		public IEnumerator<float> TimeBC(Player pl)
		{
			int i = 0;
			var bc = pl.Broadcast(6, $"<size=30%><color=#6f6f6f>Подождите <color=red>{cooldown:00}</color> секунд</color></size>", true);
			for (; i < 5 && cooldown > 0; i++)
			{
				bc.Message = $"<size=30%><color=#6f6f6f>Подождите <color=red>{cooldown:00}</color> секунд</color></size>";
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void CooldownUpdate()
		{
			if (cooldown >= 0) cooldown--;
		}
		public void StalkCoroutine(Player player)
		{
			List<Player> list = Player.List.Where(x => !x.ItsScp035() &&
			x.Role != RoleType.Tutorial && x.Role != RoleType.Spectator && x.Team != Team.SCP && x.Team != Team.CHI).ToList();
			Scp106PlayerScript scp106Script = player.GameObject.GetComponent<Scp106PlayerScript>();
			if (list.IsEmpty())
			{
				player.Broadcast(4, "<size=30%><color=#6f6f6f>Увы, таргеты не найдены</color></size>");
				return;
			}
			Timing.CallDelayed(0.1f, () =>
			{
				Player target = FindTarget(list, scp106Script.teleportPlacementMask, out Vector3 portalPosition);
				if (target == default || (Vector3.Distance(portalPosition, new Vector3(0f, -1998f, 0f)) < 40f))
				{
					player.Broadcast(4, "<size=30%><color=#6f6f6f>Увы, таргеты не найдены</color></size>");
					return;
				}
				if (portalPosition.Equals(Vector3.zero))
				{
					player.Broadcast(4, "<size=30%><color=#6f6f6f>Попробуй снова</color></size>");
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
						if (target.GetTeam() == Team.CDP) color = "#FF8E00";
						else if (target.GetTeam() == Team.RSC) color = "#fdffbb";
						else if (target.Role == RoleType.NtfPrivate) color = "#00a5ff";
						else if (target.Role == RoleType.NtfCaptain) color = "#0200ff";
						else if (target.Role == RoleType.NtfSergeant) color = "#0074ff";
						else if (target.Role == RoleType.NtfSpecialist) color = "#1f7fff";
						player.Broadcast(6, $"<size=30%><color=#6f6f6f>Ваша жертва - <color=red>{target.Nickname}</color></color></size>\n" +
							$"<size=30%><color=#6f6f6f>Похоже, что он <color={color}>{target.GetCustomRole()}</color></color></size>");
					});
				});
			});
		}
		private Player FindTarget(List<Player> validPlayerList, LayerMask teleportPlacementMask, out Vector3 portalPosition)
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