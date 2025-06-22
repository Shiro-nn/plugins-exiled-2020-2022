using MEC;
using Mirror;
using PlayerXP.force;
using UnityEngine;

namespace PlayerXP.player
{
    public class died
	{
		public static void die(Exiled.Events.EventArgs.DiedEventArgs ev)
		{
			if (ev.Killer.ReferenceHub.characterClassManager.CurClass == RoleType.Scp049 && ev.Target.Team != Team.SCP && ev.Target.Team != Team.TUT)
			{
				bool fgdg = false;
				Timing.CallDelayed(0.5f, () =>
				{
					if (!fgdg)
					{
						fgdg = true;
						Vector3 pos = ev.Killer.ReferenceHub.transform.position;
						ev.Target.ReferenceHub.characterClassManager.SetClassID(RoleType.Scp0492);
						ev.Target.ReferenceHub.playerMovementSync.OverridePosition(pos, 0f);
						foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
						{
							if (doll.owner.PlayerId == ev.Target.ReferenceHub.queryProcessor.PlayerId)
							{
								NetworkServer.Destroy(doll.gameObject);
							}
						}
					}
				});
			}
			if (forceclass.sdo)
			{
				ReferenceHub plr = ev.Target.ReferenceHub;
				bool fgdg = false;
				Timing.CallDelayed(0.5f, () =>
				{
					if (!fgdg)
					{
						fgdg = true;
						plr.characterClassManager.SetClassID(RoleType.ClassD);
					}
				});
			}
		}
	}
}
