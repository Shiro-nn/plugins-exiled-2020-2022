using Exiled.API.Features;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace scp035
{
    public partial class EventHandlers
	{
		private static void RemovePossessedItems()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				Pickup p = scpPickups.ElementAt(i).Key;
				if (p != null) p.Delete();
			}
			scpPickups.Clear();
		}
		internal void RefreshItems()
		{
			RemovePossessedItems();
			for(int i = 0; i < plugin.config.MaskCount; i++)
			{
				Vector3 m = Map.Rooms[UnityEngine.Random.Range(0, Map.Rooms.Count - 1)].transform.position + Vector3.up;
				int it = UnityEngine.Random.Range(6, 33);
				Pickup p = Extensions.SpawnItem((ItemType)it, dur, m);
				scpPickups.Add(p, dur);
			}
		}
		internal void KillScp035(bool setRank = true)
		{
			Cassie.Message(plugin.config.Cassie_dead);
			if (setRank)
				scpPlayer.SetRank("");
			scpPlayer.playerStats.maxHP = maxHP;
			scpPlayer = null;
			isRotating = true;
			RefreshItems();
		}
		public static void Spawn035(ReferenceHub p035)
		{
			maxHP = splugin.config.Hp;
			p035.playerStats.maxHP = splugin.config.Hp;
			p035.playerStats.Health = splugin.config.Hp;
			p035.Broadcast(splugin.config.Spawn_bc, splugin.config.Spawn_bc_time);
			Cassie.Message(splugin.config.Cassie, false, false);
			scpPlayer = p035;
			p035.SetRank(splugin.config.Role, "red");
			Player.Get(p035).IsFriendlyFireEnabled = true;
		}
		public void InfectPlayer(ReferenceHub player, Pickup pItem)
		{
			pItem.Delete();
			isRotating = false;
			Timing.CallDelayed(3f, () => Spawn035(player));
			RemovePossessedItems();
		}
		internal IEnumerator<float> CorrodeUpdate()
		{
			for (; ; )
			{
				try
				{
					if (scpPlayer != null && Round.IsStarted)
					{
						IEnumerable<Player> pList = Player.List.Where(x => x.Id != scpPlayer.queryProcessor.PlayerId);
						pList = pList.Where(x => x.Team != Team.SCP);
						pList = pList.Where(x => x.Team != Team.TUT);
						pList = pList.Where(x => x.Health < 770);
						foreach (Player player in pList)
						{
							if (player != null && Vector3.Distance(scpPlayer.transform.position, player.Position) <= 1.5f)
							{
								player.Broadcast(1, plugin.config.Damage_bc);
								CorrodePlayer(player.ReferenceHub);
							}
							else if (player != null && Vector3.Distance(scpPlayer.transform.position, player.Position) <= 15f)
								player.Broadcast(1, plugin.config.Distance_bc);
						}
					}
				}
				catch { }
				yield return Timing.WaitForSeconds(1f);
			}
		}
		private void CorrodePlayer(ReferenceHub player)
		{
			if (scpPlayer != null)
			{
				int currHP = (int)scpPlayer.playerStats.Health;
				scpPlayer.playerStats.Health = currHP + 5 > plugin.config.Hp ? plugin.config.Hp : currHP + 5;
			}
			if (player.playerStats.Health - 5 > 0)
				player.playerStats.Health -= 5;
			else
			{
				player.Damage(55555, DamageTypes.None);
			}
		}
		internal IEnumerator<float> CorrodeHost()
		{
			for (; ; )
			{
				if (scpPlayer != null)
				{
					scpPlayer.playerStats.Health -= 1;
					if (scpPlayer.playerStats.Health <= 0)
					{
						scpPlayer.SetRole(RoleType.Spectator);
						KillScp035();
					}
				}
				yield return Timing.WaitForSeconds(5);
			}
		}
	}
}