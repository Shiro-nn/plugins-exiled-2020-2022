using Qurre.API;
using Qurre.API.Controllers;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.Addons
{
	public class ScpHeal
	{
		private readonly static Dictionary<string, Vector3> Positions = new();
		internal void Heal()
		{
			if (Alpha.Detonated) return;
			foreach (Player player in Player.List)
			{
				if (player.Team is not Team.SCP) continue;
				if (player.MaxHp > player.Hp)
				{
					if (player.Role is RoleType.Scp049)
						Heal(player, 3);
					else if (player.Role is not RoleType.Scp079)
						Heal(player, 1);
				}
			}
		}
		private void Heal(Player player, int hp)
		{
			if (Positions.ContainsKey(player.UserId))
			{
				if (Vector3.Distance(player.Position, Positions[player.UserId]) <= 2f)
				{
					if (player.MaxHp > player.Hp + hp) player.Hp += hp;
					else player.Hp = player.MaxHp;
				}
				Positions[player.UserId] = player.Position;
			}
			else Positions.Add(player.UserId, player.Position);
		}
	}
}