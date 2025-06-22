using System.Collections.Generic;
using Qurre.API.Objects;
using Qurre.API.Events;
using Qurre.API;
using MEC;
namespace Loli.Scps
{
	static internal class Scp008
	{
		internal const string Tag = "Infected008";
		internal const string Coroutine = "Scp008Coroutine-";
		static internal void Cure(Player pl)
		{
			pl.Tag = pl.Tag.Replace(Tag, "");
			Timing.KillCoroutines(Coroutine + pl.UserId);
		}
		static internal void Dead(DeadEvent ev)
		{
			if (ev.DamageType != DamageTypes.Scp049 && ev.DamageType != DamageTypes.Scp0492 && !ev.Target.Tag.Contains(Tag)) return;
			Cure(ev.Target);
			var pos = ev.Killer.Position;
			ev.Target.Tag += "Scp008Invisible";
			ev.Target.BlockSpawnTeleport = true;
			ev.Target.SetRole(RoleType.Scp0492);
			Timing.CallDelayed(0.5f, () =>
			{
				ev.Target.Position = pos;
				ev.Target.Tag = ev.Target.Tag.Replace("Scp008Invisible", "");
				Scp0492Better.SpawnZombieRandom(ev.Target);
			});
			Timing.CallDelayed(0.3f, () =>
			{
				foreach (var rb in Map.Ragdolls)
				{
					try
					{
						if (rb != null && rb.Owner.UserId == ev.Target.UserId &&
						UnityEngine.Vector3.Distance(rb.Position, pos) < 2f)
							rb.Destroy();
					}
					catch { }
				}
			});
		}
		static internal void Leave(LeaveEvent ev)
		{
			Cure(ev.Player);
		}
		static internal void Spawn(RoleChangeEvent ev)
		{
			Cure(ev.Player);
		}
		static internal void Spawn(SpawnEvent ev)
		{
			Cure(ev.Player);
		}
		static internal void Medical(ItemUsedEvent ev)
		{
			if (!ev.Player.Tag.Contains(Tag)) return;
			if (ev.Item.TypeId != ItemType.Painkillers && ev.Item.TypeId != ItemType.Medkit && ev.Item.TypeId != ItemType.SCP500) return;
			Cure(ev.Player);
		}
		static internal void Damage(DamageProcessEvent ev)
		{
			if (!ev.Allowed) return;
			if (ev.Target.Tag.Contains(Tag)) return;
			if (ev.DamageType != DamageTypes.Scp0492) return;
			Timing.RunCoroutine(Proc(), Coroutine + ev.Target.UserId);
			IEnumerator<float> Proc()
			{
				var _role = ev.Target.Role;
				ev.Target.Tag += Tag;
				byte wait = 30;
				var bc = ev.Target.Broadcast($"<size=30%><color=#737885>Вы заражены <color=red>SCP 008</color>\n" +
				$"Вы станете <color=red>SCP 049-2</color> через {wait} секунд</color></size>", wait, true);
				yield return Timing.WaitForSeconds(1f);
				wait -= 1;
				for (; wait > 1; wait--)
				{
					if (!ev.Target.Tag.Contains(Tag)) yield break;
					bc.Message = "<size=30%><color=#737885>Вы заражены <color=red>SCP 008</color>\n" +
						$"Вы станете <color=red>SCP 049-2</color> через {wait} секунд</color></size>";
					yield return Timing.WaitForSeconds(1f);
				}
				if (ev.Target.Role != _role) yield break;
				var pos = ev.Target.Position;
				Timing.CallDelayed(0.5f, () =>
				{
					ev.Target.Tag += "Scp008Invisible";
					ev.Target.BlockSpawnTeleport = true;
					ev.Target.SetRole(RoleType.Scp0492);
					Timing.CallDelayed(0.5f, () =>
					{
						ev.Target.Position = pos;
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
							if (rb != null && rb.Owner.UserId == ev.Target.UserId &&
							UnityEngine.Vector3.Distance(rb.Position, pos) < 2f)
								rb.Destroy();
						}
						catch { }
					}
				});
				ev.Target.Kill("SCP-008");
				yield break;
			}
		}
	}
}