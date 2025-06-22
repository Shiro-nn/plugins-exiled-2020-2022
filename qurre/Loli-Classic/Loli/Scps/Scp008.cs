using System.Collections.Generic;
using Qurre.API.Objects;
using Qurre.API;
using MEC;
using Qurre.Events.Structs;
using Qurre.API.Attributes;
using Qurre.Events;
using PlayerRoles;

namespace Loli.Scps
{
	static internal class Scp008
	{
		internal const string Tag = "Infected008";
		internal const string Coroutine = "Scp008Coroutine-";

		static internal void Cure(Player pl)
		{
			pl.Tag = pl.Tag.Replace(Tag, "");
			Timing.KillCoroutines(Coroutine + pl.UserInfomation.UserId);
		}

		[EventMethod(PlayerEvents.Dead)]
		static internal void Dead(DeadEvent ev)
		{
			if (ev.DamageType != DamageTypes.Scp049 && ev.DamageType != DamageTypes.Zombie && !ev.Target.Tag.Contains(Tag))
				return;

			Cure(ev.Target);

			var pos = ev.Attacker.MovementState.Position;

			ev.Target.Tag += "Scp008Invisible";
			ev.Target.GamePlay.BlockSpawnTeleport = true;
			ev.Target.RoleInfomation.SetNew(RoleTypeId.Scp0492, RoleChangeReason.Respawn);

			Timing.CallDelayed(0.5f, () =>
			{
				ev.Target.MovementState.Position = pos;
				ev.Target.Tag = ev.Target.Tag.Replace("Scp008Invisible", "");
				Scp0492Better.SpawnZombieRandom(ev.Target);
			});
			Timing.CallDelayed(0.3f, () =>
			{
				foreach (var rb in Map.Ragdolls)
				{
					try
					{
						if (rb != null && rb.Owner.UserInfomation.UserId == ev.Target.UserInfomation.UserId &&
							UnityEngine.Vector3.Distance(rb.Position, pos) < 2f)
							rb.Destroy();
					}
					catch { }
				}
			});
		}

		[EventMethod(PlayerEvents.Leave)]
		[EventMethod(PlayerEvents.ChangeRole)]
		[EventMethod(PlayerEvents.Spawn)]
		static internal void Cure(IBaseEvent @event)
		{
            switch (@event)
            {
				case LeaveEvent ev:
					Cure(ev.Player);
					break;
				case ChangeRoleEvent ev:
					Cure(ev.Player);
					break;
				case SpawnEvent ev:
					Cure(ev.Player);
					break;
				default: break;
			}
		}

		[EventMethod(PlayerEvents.UsedItem)]
		static internal void Medical(UsedItemEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag))
				return;
			if (ev.Item.Type != ItemType.Painkillers &&
				ev.Item.Type != ItemType.Medkit &&
				ev.Item.Type != ItemType.SCP500)
				return;
			Cure(ev.Player);
		}

		[EventMethod(PlayerEvents.Attack)]
		static internal void Attack(AttackEvent ev)
		{
			if (!ev.Allowed)
				return;
			if (ev.Target.Tag.Contains(Tag))
				return;
			if (ev.DamageType != DamageTypes.Zombie)
				return;
			Timing.RunCoroutine(Proc(), Coroutine + ev.Target.UserInfomation.UserId);
			IEnumerator<float> Proc()
			{
				var _role = ev.Target.RoleInfomation.Role;
				ev.Target.Tag += Tag;
				byte wait = 30;
				var bc = ev.Target.Client.Broadcast($"<size=70%><color=#737885>Вы заражены <color=red>SCP 008</color>\n" +
				$"Вы станете <color=red>SCP 049-2</color> через {wait} секунд</color></size>", wait, true);

				yield return Timing.WaitForSeconds(1f);

				wait -= 1;
				for (; wait > 1; wait--)
				{
					if (!ev.Target.Tag.Contains(Tag))
						yield break;
					bc.Message = "<size=70%><color=#737885>Вы заражены <color=red>SCP 008</color>\n" +
						$"Вы станете <color=red>SCP 049-2</color> через {wait} секунд</color></size>";
					yield return Timing.WaitForSeconds(1f);
				}

				if (ev.Target.RoleInfomation.Role != _role)
					yield break;

				var pos = ev.Target.MovementState.Position;

				Timing.CallDelayed(0.5f, () =>
				{
					ev.Target.Tag += "Scp008Invisible";
					ev.Target.GamePlay.BlockSpawnTeleport = true;
					ev.Target.RoleInfomation.SetNew(RoleTypeId.Scp0492, RoleChangeReason.Respawn);
					Timing.CallDelayed(0.5f, () =>
					{
						ev.Target.MovementState.Position = pos;
						ev.Target.Tag = ev.Target.Tag.Replace("Scp008Invisible", "");
						Scp0492Better.SpawnZombieRandom(ev.Target);
					});
				});

				ev.Target.Tag = ev.Target.Tag.Replace(Tag, "");

				Timing.CallDelayed(0.3f, () =>
				{
					foreach (var rb in Map.Ragdolls)
					{
						try
						{
							if (rb != null && rb.Owner.UserInfomation.UserId == ev.Target.UserInfomation.UserId &&
								UnityEngine.Vector3.Distance(rb.Position, pos) < 2f)
								rb.Destroy();
						}
						catch { }
					}
				});

				ev.Target.HealthInfomation.Kill("SCP-008");
				yield break;
			}
		}
	}
}