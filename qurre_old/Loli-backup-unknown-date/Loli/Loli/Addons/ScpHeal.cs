using Qurre.API;
using Qurre.API.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Addons
{
	public class ScpHeal
	{
		private static Dictionary<string, Vector3> Positions = new Dictionary<string, Vector3>();
		internal void Heal()
		{
			if (!Alpha.Detonated)
			{
				foreach (Player player in Player.List.Where(x => x.Team == Team.SCP))
				{
					if (player.MaxHp > player.Hp)
					{
						if (player.Role == RoleType.Scp049)
							Heal(player, 3);
						else if (player.Role == RoleType.Scp93953 || player.Role == RoleType.Scp93989 || player.Role == RoleType.Scp106 ||
							player.Role == RoleType.Scp0492 || player.Role == RoleType.Scp096 || player.Role == RoleType.Scp173)
							Heal(player, 1);
					}
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