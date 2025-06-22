using Exiled.API.Features;
using MEC;
using Mirror;
using MongoDB.force;
using UnityEngine;

namespace MongoDB.player
{
	public class died
	{
		public static void die(Exiled.Events.EventArgs.DiedEventArgs ev)
		{
			if (ev.HitInformations.GetDamageType() == DamageTypes.Scp049 && ev.Target.Team != Team.SCP && ev.Target.Team != Team.TUT)
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
			if (ev.HitInformations.GetDamageType() == DamageTypes.Scp0492 || hurte.InfectedPlayers.Contains(ev.Target.ReferenceHub))
			{
				Timing.CallDelayed(0.5f, () =>
				{
					foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
					{
						if (doll.owner.PlayerId == ev.Target.ReferenceHub.queryProcessor.PlayerId)
						{
							NetworkServer.Destroy(doll.gameObject);
						}
					}
				});
			}
			if (Round.ElapsedTime.Minutes == 0 && Round.IsStarted && ev.HitInformations.GetDamageType() != DamageTypes.Scp0492 && ev.HitInformations.GetDamageType() != DamageTypes.Scp049)
			{
				ReferenceHub plr = ev.Target.ReferenceHub;
				Timing.CallDelayed(0.5f, () =>
				{
					plr.characterClassManager.SetClassID(RoleType.ClassD);
				});
			}
		}
		public static void dying(Exiled.Events.EventArgs.DyingEventArgs ev)
		{
			Vector3 pos = ev.Target.Position;
			if (ev.HitInformation.GetDamageType() == DamageTypes.Scp049 && ev.IsAllowed)
			{
				ev.IsAllowed = false;
				ev.Target.ReferenceHub.inventory.ServerDropAll();
				ReferenceHub plr = ev.Target.ReferenceHub;
				plr.characterClassManager.SetClassID(RoleType.Scp0492);
				Timing.CallDelayed(0.3f, () => ev.Target.ReferenceHub.playerMovementSync.OverridePosition(pos, 0f));
			}
		}
	}
}