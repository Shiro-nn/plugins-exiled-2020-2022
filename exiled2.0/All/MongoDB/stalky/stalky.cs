using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MongoDB.stalky
{
	public class stalky
	{
		private readonly Plugin plugin;
		public stalky(Plugin plugin) => this.plugin = plugin;
		private float cooldown = 0;
		internal void SetClass(ChangingRoleEventArgs ev)
		{
			if (ev.NewRole == RoleType.Scp106)
			{
				cooldown = 25;
				ev.Player.Broadcast(10, "<size=30%><color=#6f6f6f>Дважды нажав на создание портала, вы телепортируетесь к рандомной цели</color></size>");
			}
		}
		internal void CreatePortal(CreatingPortalEventArgs ev) => ev.IsAllowed = Stalk(ev.Player);
		internal bool Stalk(Player player)
		{
			if (cooldown < 1)
			{
				cooldown = 30;
				MEC.Timing.RunCoroutine(StalkCoroutine(player));
				return false;
			}
			else if (25 >= cooldown)
			{
				int i = 0;
				for (; i < 5 && cooldown > i; i++) player.Broadcast(1, "<size=30%><color=#6f6f6f>Подождите <color=red>$time</color> секунд</color></size>".Replace("$time", (cooldown - i).ToString("00")));
				return true;
			}
			else return false;
		}
		public void CooldownUpdate()
		{
			if (cooldown >= 0) cooldown--;
		}
		public IEnumerator<float> StalkCoroutine(Player player)
		{
			List<Player> list = Player.List.Where(x =>
			x.UserId != scp343.EventHandlers343.scp343?.characterClassManager.UserId &&
			x.UserId != scp228.EventHandlers228.scp228ruj?.characterClassManager.UserId &&
			x.UserId != scp035.EventHandlers.scpPlayer?.characterClassManager.UserId &&
			x.Role != RoleType.Tutorial && x.Role != RoleType.Spectator && x.Team != Team.SCP && x.Team != Team.CHI).ToList();
			Scp106PlayerScript scp106Script = player.GameObject.GetComponent<Scp106PlayerScript>();
			if (list.IsEmpty())
			{
				player.Broadcast(4, "<size=30%><color=#6f6f6f>Увы, таргеты не найдены</color></size>");
				yield break;
			}
			yield return MEC.Timing.WaitForOneFrame;
			Player target = FindTarget(list, scp106Script.teleportPlacementMask, out Vector3 portalPosition);
			if (target == default || (Vector3.Distance(portalPosition, new Vector3(0f, -1998f, 0f)) < 40f))
			{
				player.Broadcast(4, "<size=30%><color=#6f6f6f>Увы, таргеты не найдены</color></size>");
				yield break;
			}
			if (portalPosition.Equals(Vector3.zero))
			{
				player.Broadcast(4, "<size=30%><color=#6f6f6f>Попробуй снова</color></size>");
				yield break;
			}
			yield return MEC.Timing.WaitForOneFrame;
			scp106Script.NetworkportalPosition = portalPosition - Vector3.up;
			yield return MEC.Timing.WaitForSeconds(0.2f);
			scp106Script.CallCmdUsePortal();
			string color = "#ffffff";
			if (target.ReferenceHub.GetTeam() == Team.CDP) color = "#FF8E00";
			else if (target.ReferenceHub.GetTeam() == Team.RSC) color = "#fdffbb";
			else if (target.Role == RoleType.NtfCadet) color = "#00a5ff";
			else if (target.Role == RoleType.NtfCommander) color = "#0200ff";
			else if (target.Role == RoleType.NtfLieutenant) color = "#0074ff";
			else if (target.Role == RoleType.NtfScientist) color = "#1f7fff";
			player.Broadcast(6, $"<size=30%><color=#6f6f6f>Ваша жертва - <color=red>{target?.Nickname}</color></color></size>\n" +
				$"<size=30%><color=#6f6f6f>Похоже, что он <color={color}>{target?.ReferenceHub.GetCustomRole()}</color></color></size>");
		}
		private Player FindTarget(List<Player> validPlayerList, LayerMask teleportPlacementMask, out Vector3 portalPosition)
		{
			Player target;
			do
			{
				int index = Random.Range(0, validPlayerList.Count);
				target = validPlayerList[index];
				Physics.Raycast(new Ray(target.GameObject.transform.position, -Vector3.up), out RaycastHit raycastHit, 10f, teleportPlacementMask);
				portalPosition = raycastHit.point;
				validPlayerList.RemoveAt(index);
			} while ((portalPosition.Equals(Vector3.zero) || Vector3.Distance(portalPosition, new Vector3(0f, -1998f, 0f)) < 40f) && validPlayerList.Count > 0);
			return target;
		}
	}
}