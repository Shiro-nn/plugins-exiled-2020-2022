using PlayerRoles;
using Qurre.API;
using Qurre.API.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Addons
{
	static class ScpHeal
	{
		static readonly Dictionary<string, Vector3> Positions = new();
		static internal void Heal()
		{
			if (Alpha.Detonated) return;
			foreach (Player player in Player.List)
			{
				if (player.RoleInfomation.Team is not Team.SCPs) continue;
				if (player.HealthInfomation.MaxHp > player.HealthInfomation.Hp)
				{
					if (player.RoleInfomation.Role is RoleTypeId.Scp049)
						Heal(player, 3);
					else if (player.RoleInfomation.Role is not RoleTypeId.Scp079)
						Heal(player, 1);
				}
			}
		}
		static void Heal(Player player, int hp)
		{
			if (Positions.ContainsKey(player.UserInfomation.UserId))
			{
				if (Vector3.Distance(player.MovementState.Position, Positions[player.UserInfomation.UserId]) <= 2f)
				{
					if (player.HealthInfomation.MaxHp > player.HealthInfomation.Hp + hp) player.HealthInfomation.Hp += hp;
					else player.HealthInfomation.Hp = player.HealthInfomation.MaxHp;
				}
				Positions[player.UserInfomation.UserId] = player.MovementState.Position;
			}
			else Positions.Add(player.UserInfomation.UserId, player.MovementState.Position);
		}
	}
}