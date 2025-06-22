using Loli.BetterHints;
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
		private float cooldown = 55;
		private System.DateTime LastClick = System.DateTime.Now;
		internal void SetClass(RoleChangeEvent ev)
		{
			if (Round.ElapsedTime.TotalSeconds > 2) return;
			if (ev.NewRole == RoleType.Scp106)
			{
				ev.Player.Tag += "StalkUseAllow";
				cooldown = 55;
				ev.Player.Hint(new(20, 6, "<align=right><color=#6f6f6f>Дважды нажав на создание портала,</color></align>", 10, false));
				ev.Player.Hint(new(20, 6, "<align=right><color=#6f6f6f>вы телепортируетесь к рандомной цели</color></align>", 10, false));
			}
		}
		internal void CreatePortal(PortalCreateEvent ev) => ev.Allowed = Stalk(ev.Player);
		internal bool Stalk(Player player)
		{
			if (!player.Tag.Contains("StalkUseAllow")) return true;
			if ((System.DateTime.Now - LastClick).TotalSeconds > 5)
			{
				LastClick = System.DateTime.Now;
				return true;
			}
			if (cooldown < 1)
			{
				cooldown = 60;
				StalkCoroutine(player);
				return false;
			}
			else if (55 >= cooldown)
			{
				Timing.RunCoroutine(TimeBC(player));
				return true;
			}
			else return false;
		}
		public IEnumerator<float> TimeBC(Player pl)
		{
			HintStruct hint = new(20, 6, $"<align=right><color=#6f6f6f>Подождите <color=red>{cooldown:00}</color> секунд</color></align>", 6, false);
			pl.Hint(hint);
			for (int i = 0; i < 5 && cooldown > 0; i++)
			{
				hint.Message = $"<align=right><color=#6f6f6f>Подождите <color=red>{cooldown:00}</color> секунд</color></align>";
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
				player.Hint(new(20, 6, "<align=right><color=#6f6f6f>Таргеты не найдены</color></align>", 5, false));
				return;
			}
			Timing.CallDelayed(0.1f, () =>
			{
				Player target = FindTarget(list, scp106Script.teleportPlacementMask, out Vector3 portalPosition);
				if (target == default || (Vector3.Distance(portalPosition, new Vector3(0f, -1998f, 0f)) < 40f))
				{
					player.Hint(new(20, 6, "<align=right><color=#6f6f6f>Таргеты не найдены</color></align>", 5, false));
					return;
				}
				if (portalPosition.Equals(Vector3.zero))
				{
					player.Hint(new(20, 6, "<align=right><color=#6f6f6f>Попробуй снова</color></align>", 5, false));
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
						string color = target.Role switch
						{
							RoleType.ClassD => "#FF8E00",
							RoleType.Scientist => "#fdffbb",
							RoleType.ChaosConscript or RoleType.ChaosMarauder
								or RoleType.ChaosRepressor or RoleType.ChaosRifleman => "#00ad11",
							RoleType.NtfPrivate => "#00a5ff",
							RoleType.NtfCaptain => "#0200ff",
							RoleType.NtfSergeant => "#0074ff",
							RoleType.NtfSpecialist => "#1f7fff",
							_ => "#ffffff",
						};
						player.Hint(new(20, 6, $"<align=right><color=#6f6f6f>Ваша жертва - <color=red>{target.Nickname}</color></color></align>", 10, false));
						player.Hint(new(20, 6, $"<align=right><color=#6f6f6f>Похоже, что он <color={color}>{target.GetCustomRole()}</color></color></align>", 10, false));
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