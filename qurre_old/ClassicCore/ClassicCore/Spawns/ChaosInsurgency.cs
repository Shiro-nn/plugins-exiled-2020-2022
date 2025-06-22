using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using DateTime = System.DateTime;
using System.Linq;
using UnityEngine;
using MEC;
using Respawning;
namespace ClassicCore.Spawns
{
	internal class ChaosInsurgency
	{
		internal DateTime LastCall = DateTime.Now;
		public void SpawnCI()
		{
			if (Alpha.Detonated) return;
			if ((DateTime.Now - LastCall).TotalSeconds < 30) return;
			LastCall = DateTime.Now;
			if (!Player.List.Any(x => x.Role is RoleType.Spectator && !x.Overwatch)) return;
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			Timing.CallDelayed(15f, () =>
			{
				if (Alpha.Detonated) return;
				var pls = Player.List.Where(x => x.Role is RoleType.Spectator && !x.Overwatch).ToList();
				if (pls.Count == 0) return;
				pls.Shuffle();
				Qurre.Events.Invoke.Round.TeamRespawn(new TeamRespawnEvent(pls, 999, SpawnableTeamType.ChaosInsurgency));
				foreach (Player ci in pls) SpawnOne(ci);
			});
		}
		public void SpawnOne(Player pl)
		{
			RoleType _role = RoleType.ChaosRifleman;
			var rand = Random.Range(0, 100);
			if (rand > 66) _role = RoleType.ChaosRepressor;
			else if (rand > 33) _role = RoleType.ChaosMarauder;
			SpawnManager.SpawnProtect(pl);
			if (pl.Role is RoleType.Spectator) pl.SetRole(_role);
		}
	}
}