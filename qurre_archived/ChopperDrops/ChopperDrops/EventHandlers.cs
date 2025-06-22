using System;
using System.Collections.Generic;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Respawning;
using UnityEngine;
namespace ChopperDrops
{
	internal class EventHandlers
	{
		public void RoundStart() => Timing.RunCoroutine(ChopperThread(), "ChopperDrops");
		public void WaitingForPlayers() => Timing.KillCoroutines("ChopperDrops");
		public IEnumerator<float> ChopperThread()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(Configs.DropDelay);
				if (Round.Started)
				{
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
					Map.Broadcast(Configs.DropText, Configs.DropTextTime);
					yield return Timing.WaitForSeconds(15f);
					Vector3 randomSpawnPoint = Map.GetRandomSpawnPoint(RoleType.NtfCaptain);
					foreach (string PrekeyValuePair in Configs.AllowedItems.Split(','))
					{
						try
						{
							var keyValuePair = PrekeyValuePair.Split(':');
							for (int i = 0; i < int.Parse(keyValuePair[1]); i++)
							{
								ItemType item = (ItemType)Enum.Parse(typeof(ItemType), keyValuePair[0]);
								new Item(item).Spawn(randomSpawnPoint);
							}
						}
						catch { }
					}
					yield return Timing.WaitForSeconds(15f);
				}
			}
		}
	}
}