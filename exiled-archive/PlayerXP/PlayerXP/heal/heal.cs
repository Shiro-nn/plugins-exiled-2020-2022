using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerXP.heal
{
    public class scpheal
	{
		private readonly Plugin plugin;
		public scpheal(Plugin plugin) => this.plugin = plugin;
		private static Dictionary<string, Vector3> svector = new Dictionary<string, Vector3>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public void OnRoundStart()
		{
			Coroutines.Add(Timing.RunCoroutine(scpheals()));
		}
		public void RoundEnd(RoundEndedEventArgs ev)
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
		}
		public static IEnumerator<float> scpheals()
		{
			for (; ; )
			{
				List<ReferenceHub> players = Extensions.GetHubs().Where(x => x.GetTeam() == Team.SCP).ToList();
				foreach (ReferenceHub player in players)
				{
					if (player.playerStats.maxHP > player.playerStats.Health)
					{
						player.playerStats.Health++;
						if (svector.ContainsKey(player.characterClassManager.UserId))
						{
							if (Vector3.Distance(player.transform.position, svector[player.characterClassManager.UserId]) <= 2f)
							{
								player.playerStats.Health = player.playerStats.Health + 15;
							}
							svector[player.characterClassManager.UserId] = player.transform.position;
						}
						else
						{
							svector.Add(player.characterClassManager.UserId, player.transform.position);
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
}
