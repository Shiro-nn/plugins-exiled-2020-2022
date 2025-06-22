using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Respawning;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		internal void ChopperRefresh() => Timing.RunCoroutine(ChopperThread(), "ChopperThread");
		internal void ChopperRefresh(RoundEndEvent _) => Timing.KillCoroutines("ChopperThread");
		public IEnumerator<float> ChopperThread()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(600);
				if (Round.Started && !(AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled || Plugin.ClansWars))
				{
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
					Map.Broadcast("<size=25%><color=#6f6f6f>Cовет О5 отправил <color=lime>вертолет с припасами</color>.</color></size>", 5);
					yield return Timing.WaitForSeconds(15f);
					Vector3 randomSpawnPoint = Map.GetRandomSpawnPoint(RoleType.NtfCaptain);
					foreach (KeyValuePair<ItemType, int> keyValuePair in allowedItems)
					{
						for (int i = 0; i < keyValuePair.Value; i++)
						{
							new Item(keyValuePair.Key).Spawn(randomSpawnPoint);
						}
					}
					yield return Timing.WaitForSeconds(15f);
				}
			}
		}
		public Dictionary<ItemType, int> allowedItems = new Dictionary<ItemType, int>
		{
			{ItemType.GunE11SR,1},
			{ItemType.Medkit,3},
			{ItemType.Adrenaline,2},
			{ItemType.Ammo556x45,2}
		};
	}
}