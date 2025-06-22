using Loli.Scps.Api;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Scps
{
	public class Scp008
	{
		public float InfectionLength = 30;
		public List<Player> InfectedPlayers = new();
		internal void AntiFlood(DiesEvent ev)
        {
			if (Plugin.RolePlay) return;
			if(ev.Target.Role == RoleType.Scp0492)
			{
				CurePlayer(ev.Target);
				Timing.CallDelayed(0.2f, () =>
				{
					CurePlayer(ev.Target);
					if (ev.Target.Role != RoleType.Spectator) ev.Target.Role = RoleType.Spectator;
					Timing.CallDelayed(0.5f, () =>
					{
						CurePlayer(ev.Target);
						if (ev.Target.Role != RoleType.Spectator) ev.Target.Role = RoleType.Spectator;
					});
				});
            }
        }
		private readonly Dictionary<Player, Vector3> PositionCached = new();
		internal void Clear() => PositionCached.Clear();
		internal void SavePos(DiesEvent ev)
		{
			if (PositionCached.ContainsKey(ev.Target)) PositionCached[ev.Target] = ev.Target.Position;
			else PositionCached.Add(ev.Target, ev.Target.Position);
		}
		internal void Spawn(SpawnEvent ev)
		{
			if (ev.RoleType != RoleType.Scp0492) return;
			if (PositionCached.TryGetValue(ev.Player, out var pos))
				ev.Position = pos;
			else
			{
				var s049s = Player.List.Where(x => x.Role == RoleType.Scp049).ToList();
				if (s049s.Count > 0) ev.Position = pos;
				else ev.Position = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
			}
		}
		internal void Dies(DeadEvent ev)
		{
			if (Plugin.RolePlay) return;
			try
			{
				if (ev.Target.ItsScp035()) return;
				if (InfectedPlayers.Contains(ev.Target) && ev.Target.GetTeam() != Team.SCP)
				{
					Timing.CallDelayed(0.5f, () =>
					{
						var troops = Map.Ragdolls.Where(x => x.Owner?.Id == ev.Target.Id);
						foreach (var doll in troops) doll.Destroy();
					});
					CurePlayer(ev.Target);
					if (ev.Target.Role != RoleType.Scp0492) Timing.RunCoroutine(TurnIntoZombie(ev.Target));
				}
				if (ev.Killer.Role == RoleType.Scp0492 && ev.Target.GetTeam() != Team.SCP && ev.Target.Team != Team.TUT)
					InfectPlayer(ev.Target);
			}
            catch { }
		}
		internal void Medical(ItemUsedEvent ev)
		{
			if (Plugin.RolePlay) return;
			if (ev.Item.TypeId == ItemType.Painkillers || ev.Item.TypeId == ItemType.Medkit || ev.Item.TypeId == ItemType.SCP500)
				CurePlayer(ev.Player);
		}
		internal void Dead(DeadEvent ev)
		{
			if (Plugin.RolePlay) return;
			if (ev.DamageType == DamageTypes.Scp049 && ev.Target.Team != Team.SCP && ev.Target.Team != Team.TUT)
			{
				if (Plugin.RolePlay) return;
				ev.Target.Role = RoleType.Scp0492;
				Timing.CallDelayed(0.5f, () =>
				{
					var troops = Map.Ragdolls.Where(x => x.Owner?.Id == ev.Target.Id);
					foreach (var doll in troops) doll.Destroy();
					int random = Extensions.Random.Next(0, 100);
					if (30 >= random) Scp0492Better.SpawnZombie(ev.Target, "BigZombie");
					else if (60 >= random) Scp0492Better.SpawnZombie(ev.Target, "SpeedZombie");
				});
			}
			else if (ev.DamageType == DamageTypes.Scp0492 || InfectedPlayers.Contains(ev.Target))
			{
				ev.Target.Role = RoleType.Scp0492;
				Timing.CallDelayed(0.5f, () =>
				{
					var troops = Map.Ragdolls.Where(x => x.Owner?.Id == ev.Target.Id);
					foreach (var doll in troops) doll.Destroy();
					int random = Extensions.Random.Next(0, 100);
					if (30 >= random) Scp0492Better.SpawnZombie(ev.Target, "BigZombie");
					else if (60 >= random) Scp0492Better.SpawnZombie(ev.Target, "SpeedZombie");
				});
			}
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (Plugin.RolePlay) return;
			if (ev.DamageType == DamageTypes.Scp0492 && !InfectedPlayers.Contains(ev.Target))
				InfectPlayer(ev.Target);
        }
		internal void InfectPlayer(Player player)
		{
			if (Plugin.RolePlay) return;
			if (InfectedPlayers.Contains(player)) return;
			if (player.GetTeam() == Team.SCP || player.GetTeam() == Team.TUT) return;
			InfectedPlayers.Add(player);
			player.Broadcast(2, $"<size=30%><color=#737885>Вы заражены <color=red>SCP 008</color>.\n" +
				$"Вы станете <color=red>SCP 049-2</color> через {InfectionLength} секунд!</color></size>", true);
			Timing.RunCoroutine(DoInfectionTimer(player), $"Scp008-{player.UserId}");
		}
		private IEnumerator<float> DoInfectionTimer(Player player)
		{
			for (int i = 0; i < InfectionLength; ++i)
			{
				if (!InfectedPlayers.Contains(player)) yield break;
				else if(player.Team == Team.SCP)
                {
					CurePlayer(player);
					yield break;
				}
				else
				{
					player.Broadcast($"<size=30%><color=#737885>Вы заражены <color=red>SCP 008</color>.\n" +
						$"Вы станете <color=red>SCP 049-2</color> через {InfectionLength - i} секунд!</color></size>", 2, true);
					yield return Timing.WaitForSeconds(1f);
				}
			}
			Timing.RunCoroutine(TurnIntoZombie(player));
			yield break;
		}
		internal void CurePlayer(Player player)
		{
			if (Plugin.RolePlay) return;
			try
			{
				if (InfectedPlayers.Contains(player) && player.Team != Team.SCP)
				{
					InfectedPlayers.Remove(player);
					player.Broadcasts.FirstOrDefault().End();
				}
				Timing.KillCoroutines($"Scp008-{player.UserId}");
			}
			catch { }
		}
		internal IEnumerator<float> TurnIntoZombie(Player player)
		{
			CurePlayer(player);
			if (player.Role == RoleType.Scp0492)
			{
				CurePlayer(player);
				yield break;
			}
			yield return Timing.WaitForSeconds(0.3f);
			CurePlayer(player);
			player.DropItems();
			player.Role = RoleType.Scp0492;
			yield return Timing.WaitForSeconds(0.5f);
			player.Hp = player.MaxHp;
			yield return Timing.WaitForSeconds(0.5f);
			int random = Extensions.Random.Next(0, 100);
			if (30 >= random) Scp0492Better.SpawnZombie(player, "BigZombie");
			else if (60 >= random) Scp0492Better.SpawnZombie(player, "SpeedZombie");
			yield break;
		}
	}
}