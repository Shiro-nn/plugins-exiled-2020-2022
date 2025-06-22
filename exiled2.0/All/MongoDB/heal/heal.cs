using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MongoDB.heal
{
	public class scpheal
	{
		private readonly Plugin plugin;
		public scpheal(Plugin plugin) => this.plugin = plugin;
		private static Dictionary<string, Vector3> svector = new Dictionary<string, Vector3>();
		internal void scpheals()
		{
            if (!Warhead.IsDetonated)
			{
				List<ReferenceHub> players = Extensions.GetHubs().Where(x => x.GetTeam() == Team.SCP).ToList();
				foreach (ReferenceHub player in players)
				{
					if (player.playerStats.maxHP > player.playerStats.Health)
					{
						if (player.GetRole() == RoleType.Scp049)
						{
							heal(player, 10);
						}
						else if (player.GetRole() == RoleType.Scp106 || player.GetRole() == RoleType.Scp0492 || player.GetRole() == RoleType.Scp096)
						{
							heal(player, 5);
						}
						else if (player.GetRole() == RoleType.Scp93953 || player.GetRole() == RoleType.Scp93989 || player.GetRole() == RoleType.Scp173)
						{
							heal(player, 15);
						}
					}
				}
			}
		}
		internal static void heal(ReferenceHub player, int hp)
		{
			if (svector.ContainsKey(player.characterClassManager.UserId))
			{
				if (Vector3.Distance(player.transform.position, svector[player.characterClassManager.UserId]) <= 2f)
				{
					if (player.playerStats.maxHP > player.playerStats.Health + hp)
					{
						player.playerStats.Health = player.playerStats.Health + hp;
                    }
                    else
					{
						player.playerStats.Health = player.playerStats.maxHP;
					}
				}
				svector[player.characterClassManager.UserId] = player.transform.position;
			}
			else
			{
				svector.Add(player.characterClassManager.UserId, player.transform.position);
			}
		}
	}
}