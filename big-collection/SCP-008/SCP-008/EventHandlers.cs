using System.Collections.Generic;
using EXILED;
using MEC;
using UnityEngine;
using EXILED.Extensions;
using GameCore;
using System;
using System.Linq;
using scp682343.API;
using scp035.API;
namespace SCP008
{
	public class EventHandlers
	{
		internal bool sdo = false;
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		private ReferenceHub TryGet035()
		{
			return Scp035Data.GetScp035();
		}
		private ReferenceHub TryGet343()
		{
			return scp682Data.GetScp343();
		}
		public void OnWaitingForPlayers()
		{
			foreach (CoroutineHandle handle in plugin.Coroutines)
				Timing.KillCoroutines(handle);
		}
		public void OnRoundStart()
		{
			sdo = true;
			Timing.CallDelayed(90f, () => sdo = false);
		}
		public void OnRoundEnd()
		{
			foreach (CoroutineHandle handle in plugin.Coroutines)
				Timing.KillCoroutines(handle);
			GameCore.Console.singleton.TypeCommand($"/cfgr config_reload");
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ev.Info.Amount >= ev.Player.playerStats.health)
				if (plugin.InfectedPlayers.Contains(ev.Player))
				{
					plugin.Functions.CurePlayer(ev.Player);
					if (ev.Player.characterClassManager.CurClass != RoleType.Scp0492 && plugin.TurnInfectedOnDeath)
					{
						Vector3 pos = ev.Player.gameObject.transform.position;
						plugin.Functions.TurnIntoZombie(ev.Player, new Vector3(pos.x, pos.y, pos.z));
						Timing.RunCoroutine(plugin.Functions.TurnIntoZombie(ev.Player, new Vector3(pos.x, pos.y, pos.z)));
						ev.Info = new PlayerStats.HitInfo(0f, ev.Info.Attacker, ev.Info.GetDamageType(), ev.Info.PlyId);
					}
				}

			if (ev.Attacker == null || string.IsNullOrEmpty(ev.Attacker.characterClassManager.UserId))
			{
				return;
			}

			if (ev.Player.queryProcessor.PlayerId == TryGet035()?.queryProcessor.PlayerId)
			{
				return;
			}
			if (ev.Player.queryProcessor.PlayerId == TryGet343()?.queryProcessor.PlayerId)
			{
				return;
			}
			if (ev.Attacker.characterClassManager.CurClass == RoleType.Scp0492 && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.SCP && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.TUT)
			{
				plugin.Functions.InfectPlayer(ev.Player);
			}
		}
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Killer.characterClassManager.CurClass == RoleType.Scp049 && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.SCP && Plugin.GetTeam(ev.Player.characterClassManager.CurClass) != Team.TUT)
			{
				Vector3 pos = ev.Killer.transform.position;
				ev.Player.characterClassManager.SetClassID(RoleType.Scp0492);
				ev.Player.plyMovementSync.OverridePosition(pos, 0f);
				return;
			}
			if (sdo)
			{
				ReferenceHub plr = ev.Player;
				Timing.CallDelayed(0.5f, () =>
				{
					plr.characterClassManager.SetClassID(RoleType.ClassD);
					plr.ClearInventory();
					plr.AddItem(ItemType.Coin);
					plr.AddItem(ItemType.Flashlight);
					plr.AddItem(ItemType.KeycardJanitor);
				});
			}
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{

			if (sdo)
			{
				Timing.CallDelayed(3f, () =>
				{
					ev.Player.characterClassManager.SetClassID(RoleType.ClassD);
				});
			}
		}
		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (!plugin.InfectedPlayers.Contains(ev.Player))
				return;
			
			if (ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500)
				plugin.Functions.CurePlayer(ev.Player);
		}
	}
}