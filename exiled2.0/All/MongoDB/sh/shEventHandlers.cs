using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using MongoDB.logs;
using MongoDB.scp035.API;
using Respawning;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MongoDB.sh
{
    public class shEventHandlers
	{
		private readonly Plugin plugin;
		public shEventHandlers(Plugin plugin) => this.plugin = plugin;
		internal int newspawnint = 0;
		public static List<int> shPlayers = new List<int>();
		internal int spawnshtime = 15;
		internal bool alphaon = false;
		internal bool alphadis = false;
		internal bool spawntim = false;
		internal void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string effort = ev.Name;
			foreach (string s in ev.Arguments)
				effort += $" {s}";
			string[] args = effort.Split(' ');
			try
			{
				var main = plugin.donate.main[ev.Sender.ReferenceHub.characterClassManager.UserId];
				if (main.pr || main.vr || main.vpr || main.sr || (main.don && !main.or && !main.gar && !main.ar && !main.ghr && !main.hr)) return;
			}
			catch { }
			if (ev.Name == "server_event")
			{
				if (args[1] == "force_mtf_respawn" || args[1] == "FORCE_MTF_RESPAWN")
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = "SERVER_EVENT#Успешно";
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
					bool yes = false;
					Timing.CallDelayed(15f, () =>
					{
						if (!yes)
						{
							yes = true;
							RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
						}
					});
				}
				else if (args[1] == "force_ci_respawn" || args[1] == "FORCE_CI_RESPAWN")
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = "SERVER_EVENT#Успешно";
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
					bool yes = false;
					Timing.CallDelayed(15f, () =>
					{
						if (!yes)
						{
							yes = true;
							RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency);
						}
					});
				}
			}
			if (ev.Name == "ci")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "SERVER_EVENT#Успешно";
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			}
			if (ev.Name == "mtf")
			{
				ev.IsAllowed = false;
				if (auto_events.storm_base.Enabled) return;
				ev.ReplyMessage = "SERVER_EVENT#Успешно";
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
			}
			if (ev.Name == "sh")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "SERVER_EVENT#Успешно";
				spawnsh();
			}
			if (ev.Name == "onesh")
			{
				spawnonesh(ev.Sender);
			}
		}
		public void OnWaitingForPlayers()
		{
			alphadis = false;
			alphaon = false;
			spawnshtime = 15;
		}
		public void OnRoundStart()
		{
			alphadis = false;
			alphaon = false;
			shPlayers.Clear();

			newspawnint = 0;
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			shPlayers.Clear();
		}
		public void spawn()
		{
			if (Round.IsStarted)
			{
				int random = Random.Range(0, 100);
				if (40 >= random)
				{
					spawnci();
				}
				else if (80 >= random)
				{
					spawnmtf();
				}
				else
				{
					spawnsh();
				}
			}
		}
		public Vector3 getrandomspawnsh
		{
			get
			{
				return Spawnpoint(Random.Range(0, 18));
			}
		}
		private Vector3 Spawnpoint(int id)
		{
			if (id == 1)
			{
				return new Vector3(-56, 989, -49);
			}
			else if (id == 2)
			{
				return new Vector3(-56, 989, -50);
			}
			else if (id == 3)
			{
				return new Vector3(-56, 989, -51);
			}
			else if (id == 4)
			{
				return new Vector3(-56, 984, -52.2f);
			}
			else if (id == 5)
			{
				return new Vector3(-56, 984, -53);
			}
			else if (id == 6)
			{
				return new Vector3(-56, 984, -54);
			}
			else if (id == 7)
			{
				return new Vector3(-56, 984, -55);
			}
			else if (id == 8)
			{
				return new Vector3(-56, 984, -56);
			}
			else if (id == 9)
			{
				return new Vector3(-56, 984, -57);
			}
			else if (id == 10)
			{
				return new Vector3(-56, 984, -58);
			}
			else if (id == 11)
			{
				return new Vector3(-56, 984, -59);
			}
			else if (id == 12)
			{
				return new Vector3(-56, 984, -60);
			}
			else if (id == 13)
			{
				return new Vector3(-56, 984, -61);
			}
			else if (id == 14)
			{
				return new Vector3(-56, 984, -62);
			}
			else if (id == 15)
			{
				return new Vector3(-56, 984, -63);
			}
			else if (id == 16)
			{
				return new Vector3(-56, 984, -64);
			}
			else if (id == 17)
			{
				return new Vector3(-56, 984, -65);
			}
			return new Vector3(-49, 985, -58);
		}
		internal void shtimee()
		{
			if (spawntim)
			{
				if (Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList().Count != 0)
				{
					Map.ClearBroadcasts();
					Map.Broadcast(2, $"<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Замечен отряд <color=#15ff00>Длань Змеи</color></color>\n<color=#0089c7>Они будут на территории комплекса через {spawnshtime} секунд</color></size>");
				}
				if (0 >= spawnshtime)
				{
					spawntim = false;
					spawnshtime = 15;
					if (Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList().Count != 0)
					{
						Cassie.Message("SERPENTS HAND HASENTERED");
						Map.ClearBroadcasts();
						Map.Broadcast(10, $"<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Отряд <color=#15ff00>Длань Змеи</color></color> <color=#0089c7>замечен на территории комплекса</color></size>");
					}
					try { send.sendmsg($"<:help:671334152565424129> Приехал отряд длань змеи в кол-ве {Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList().Count} человек."); } catch { }
					List<Player> newsh = Player.List.Where(x => x.Role == RoleType.Spectator && !x.IsOverwatchEnabled).ToList();
					foreach (Player sh in newsh)
					{
						spawnonesh(sh);
					}
				}
				spawnshtime--;
			}
		}
		public void spawnsh()
		{
			spawntim = true;
		}
		public void spawnonesh(Player sh)
		{
			shPlayers.Add(sh.ReferenceHub.queryProcessor.PlayerId);
			sh.ReferenceHub.characterClassManager.SetClassID(RoleType.Tutorial);
			sh.ClearBroadcasts();
			//sh.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(sh.ReferenceHub.scp079PlayerScript.connectionToClient, "<size=30%><color=red>Вы</color>-<color=#15ff00>Длань змеи</color>\n<color=#00ffdc>Ваша задача убить всех, кроме <color=red>SCP</color></color>\n<color=#fdffbb>На <color=red>[<color=#00ffff>Q</color>]</color> разговаривать с <color=red>scp</color><color=#ff0>,</color> на <color=red>[<color=#00ffff>V</color>]</color> с людьми</color></size>", 10, 0);
			sh.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(sh.ReferenceHub.scp079PlayerScript.connectionToClient, "<size=30%><color=red>Вы</color>-<color=#15ff00>Длань змеи</color>\n<color=#00ffdc>Ваша задача убить всех, кроме <color=red>SCP</color></color></size>", 10, 0);
			sh.ReferenceHub.inventory.items.ToList().Clear();
			sh.ReferenceHub.inventory.AddNewItem(ItemType.KeycardChaosInsurgency);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.GunE11SR);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Radio);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Medkit);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Adrenaline);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.GrenadeFlash);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.GrenadeFrag);
			sh.ReferenceHub.inventory.AddNewItem(ItemType.Flashlight);
			sh.ReferenceHub.playerStats.Health = 150;
			sh.ReferenceHub.playerStats.maxHP = 150;
			bool spawnone = false;
			Timing.CallDelayed(0.3f, () =>
			{
				if (!spawnone)
				{
					spawnone = true;
					sh.ReferenceHub.playerMovementSync.OverridePosition(getrandomspawnsh, 0f);
				}
			});
		}
		public void spawnci()
		{
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			bool yes = false;
			Timing.CallDelayed(15f, () => { if (!yes) { yes = true; RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.ChaosInsurgency); } });
		}
		public void spawnmtf()
		{
			GameCore.Console.singleton.TypeCommand($"/mtf");
			bool yes = false;
			Timing.CallDelayed(15f, () =>
			{
				if (!yes)
				{
					yes = true;
					RespawnManager.Singleton.ForceSpawnTeam(SpawnableTeamType.NineTailedFox);
				}
			});
		}









		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
			{
				shPlayers.Remove(ev.Player.Id);
			}
		}
		public void Leave(LeftEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
			{
				shPlayers.Remove(ev.Player.Id);
			}
		}
		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPocketDimensionEscaping(EscapingPocketDimensionEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
				TeleportTo106(ev.Player);
			}
		}
		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
			}
		}
		private void TeleportTo106(Player player)
		{
			Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
			if (scp106 != null)
			{
				player.ReferenceHub.playerMovementSync.OverridePosition(scp106.ReferenceHub.transform.position, 0f);
			}
			else
			{
				foreach (DoorVariant door in Map.Doors)
                {
					try
					{
						if (door.GetComponent<DoorNametagExtension>().GetName == "106_PRIMARY")
						{
							player.ReferenceHub.playerMovementSync.OverridePosition(new Vector3(door.transform.position.x, door.transform.position.y + 1, door.transform.position.z), 0f);
						}
                    }
                    catch { }
				}
			}
		}
		internal void hurt(HurtingEventArgs ev)
		{
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == 0) return;
			ReferenceHub scp035 = null;
			ReferenceHub target = ev.Target.ReferenceHub;
			ReferenceHub attacker = ev.Attacker.ReferenceHub;
			try { scp035 = Player.Get(Scp035Data.GetScp035()).ReferenceHub; } catch { }
			if (((shPlayers.Contains(target.queryProcessor.PlayerId) && Player.Get(target).Role == RoleType.Tutorial && (ev.Attacker.Team == Team.SCP || ev.DamageType == DamageTypes.Pocket)) ||
				(shPlayers.Contains(attacker.queryProcessor.PlayerId) && Player.Get(attacker).Role == RoleType.Tutorial && (ev.Target.Team == Team.SCP || (scp035 != null && attacker.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId))) ||
				(shPlayers.Contains(target.queryProcessor.PlayerId) && Player.Get(target).Role == RoleType.Tutorial && Player.Get(attacker).Role == RoleType.Tutorial && shPlayers.Contains(attacker.queryProcessor.PlayerId) &&
				target.queryProcessor.PlayerId != attacker.queryProcessor.PlayerId)))
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
			else if (shPlayers.Contains(target.queryProcessor.PlayerId) && Player.Get(target).Role == RoleType.Tutorial && ev.Attacker.Team == Team.SCP)
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
			else if (shPlayers.Contains(attacker.queryProcessor.PlayerId) && Player.Get(attacker).Role == RoleType.Tutorial && ev.Target.Team == Team.SCP)
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
		}
		internal void died(DiedEventArgs ev)
		{
			if (shPlayers.Contains(ev.Target.ReferenceHub.queryProcessor.PlayerId))
			{
				shPlayers.Remove(ev.Target.ReferenceHub.queryProcessor.PlayerId);
			}
		}
		public void setrole(ChangingRoleEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.Id))
			{
				if (ev.NewRole != RoleType.Tutorial)
				{
					if (shPlayers.Contains(ev.Player.Id))
					{
						shPlayers.Remove(ev.Player.Id);
					}
				}
			}
		}

		public void scpzeroninesixe(EnragingEventArgs ev)
		{
			if (shPlayers.Contains(ev.Player.ReferenceHub.queryProcessor.PlayerId) && ev.Player.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
			}
		}

		public void scpzeroninesixeadd(AddingTargetEventArgs ev)
		{
			if (shPlayers.Contains(ev.Target.ReferenceHub.queryProcessor.PlayerId) && ev.Target.Role == RoleType.Tutorial)
			{
				ev.IsAllowed = false;
			}
		}

	}
}
