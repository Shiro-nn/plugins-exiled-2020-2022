using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;

namespace SCP008
{
	public class Methods
	{
		private readonly Plugin plugin;
		public Methods(Plugin plugin) => this.plugin = plugin;

		public void InfectPlayer(ReferenceHub player)
		{
			if (plugin.InfectedPlayers.Contains(player))
			{
				return;
			}

			if (player.characterClassManager.IsAnyScp())
			{
				return;
			}
			plugin.InfectedPlayers.Add(player);
			plugin.Coroutines.Add(Timing.RunCoroutine(DoInfectionTimer(player), $"{player.characterClassManager.UserId}"));
		}

		private IEnumerator<float> DoInfectionTimer(ReferenceHub player)
		{
			Broadcast broadcast = (Broadcast)((Component)player).GetComponent<Broadcast>();
			for (int i = 0; (double)i < (double)this.plugin.InfectionLength; ++i)
			{
				if (!this.plugin.InfectedPlayers.Contains(player))
				{
					yield break;
				}
				else
				{
					player.ClearBroadcasts();
					player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, string.Format("Вы заражены SCP-008. Вы станете SCP 049-2 через {0} секунд!", (object)(float)((double)this.plugin.InfectionLength - (double)i)), 1U, false);
					//broadcast.RpcAddElement(string.Format("Вы заражены SCP-008. Вы станете SCP 049-2 через {0} секунд!", (object)(float)((double)this.plugin.InfectionLength - (double)i)), 1U, false);
					yield return Timing.WaitForSeconds(1f);
				}
			}

			GameObject gameObject = player.gameObject;
			Vector3 pos = gameObject.transform.position;

			Timing.RunCoroutine(TurnIntoZombie(player, pos));

			yield return Timing.WaitForSeconds(0.6f);

			foreach (ReferenceHub hub in EXILED.Plugin.GetHubs())
				if (Vector3.Distance(hub.gameObject.transform.position, player.gameObject.transform.position) < 10f && hub.characterClassManager.IsHuman() && hub != player)
					InfectPlayer(hub);
			CurePlayer(player);
		}

		public IEnumerator<float> TurnIntoZombie(ReferenceHub player, Vector3 position)
		{
			CurePlayer(player);
			if (player.characterClassManager.CurClass == RoleType.Scp0492)
			{
				yield break;
			}
			yield return Timing.WaitForSeconds(0.3f);
			CurePlayer(player);
			player.characterClassManager.SetClassIDAdv(RoleType.Scp0492, false);
			yield return Timing.WaitForSeconds(0.5f);
			CurePlayer(player);
			player.playerStats.health = player.playerStats.maxHP;
			player.plyMovementSync.OverridePosition(position, player.gameObject.transform.rotation.y);
			CurePlayer(player);
		}

		public void CurePlayer(ReferenceHub player)
		{
			if (plugin.InfectedPlayers.Contains(player))
				plugin.InfectedPlayers.Remove(player);

			Timing.KillCoroutines($"{player.characterClassManager.UserId}");
		}
	}
}