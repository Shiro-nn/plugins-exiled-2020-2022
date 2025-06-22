using Hints;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerXP.player
{
	class hurte
	{
		public static float InfectionLength = 30;
		public static List<ReferenceHub> InfectedPlayers = new List<ReferenceHub>();
		internal static void hurt(Exiled.Events.EventArgs.HurtingEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId) return;
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId) return;
			if (ev.Target != ev.Attacker)
			{
                string str;
                if (ev.Target.ReferenceHub.playerStats.Health - ev.Amount > 0)
				{
					str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.ReferenceHub.playerStats.Health - ev.Amount)}</color><color=red>HP</color>.</b>";
                }
                else
				{
					str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>Убит</color><color=red>!</color></b>";
				}
				ev.Attacker.ReferenceHub.Hint(str, 1f);
			}
			if (ev.Amount >= ev.Target.ReferenceHub.playerStats.Health)
				if (InfectedPlayers.Contains(ev.Target.ReferenceHub))
				{
					Extensions.CurePlayer(ev.Target.ReferenceHub);
					if (ev.Target.ReferenceHub.characterClassManager.CurClass != RoleType.Scp0492)
					{
						Vector3 pos = ev.Target.ReferenceHub.gameObject.transform.position;
						Extensions.TurnIntoZombie(ev.Target.ReferenceHub, new Vector3(pos.x, pos.y, pos.z));
						Timing.RunCoroutine(Extensions.TurnIntoZombie(ev.Target.ReferenceHub, new Vector3(pos.x, pos.y, pos.z)));
					}
				}
			if (ev.Attacker.ReferenceHub.characterClassManager.CurClass == RoleType.Scp0492 && ev.Target.Team != Team.SCP && ev.Target.Team != Team.TUT)
			{
				Extensions.InfectPlayer(ev.Target.ReferenceHub);
			}
		}
	}
}