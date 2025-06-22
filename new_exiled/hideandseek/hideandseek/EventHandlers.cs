using EXILED;
using EXILED.Extensions;
using MEC;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
namespace hideandseek
{
	public class EventHandlers
	{
		private readonly Massacre plugin;
		public EventHandlers(Massacre plugin) => this.plugin = plugin;

		private static System.Random rand = new System.Random();
		public void OnWaitingForPlayers()
		{
			plugin.RoundStarted = false;
			plugin.ReloadConfig();
		}

		public void OnRoundStart()
		{
			if (!plugin.Ga)
			{
				plugin.GamemodeEnabled = true;
				Massacre.GamemodEnabled = true;
				plugin.Ga = true;
				plugin.RoundStarted = true;
				plugin.Ga = true;
				plugin.Functions.DoSetup();
				plugin.sd = true;
				plugin.rp = false;
				plugin.ew = false; 
				Timing.CallDelayed(120f, () =>
				{
					List<ReferenceHub> playerssl = Player.GetHubs();
					foreach (ReferenceHub scp173 in playerssl)
					{
						if (scp173.GetRole() == RoleType.Scp173)
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
							scp173.playerMovementSync.OverridePosition(randomSP, 0f);
						}
						if (scp173.characterClassManager.IsHuman())
						{
							PlayerEffectsController componentInParent2 = scp173.GetComponentInParent<PlayerEffectsController>();
							componentInParent2.GetEffect<CustomPlayerEffects.Deafened>();
							componentInParent2.EnableEffect<CustomPlayerEffects.Deafened>();
						}
					}
					Generator079.generators[0].RpcCustomOverchargeForOurBeautifulModCreators(666666666666, false);
					Cassie.CassieMessage("generator .g3 automatic reactivation started .g3 .g4", false, false);
					plugin.sd = false;
					plugin.sp = true;
				});
				Timing.CallDelayed(1.0f, () =>
				{
					List<ReferenceHub> players = Massacre.GetHubs();
					List<ReferenceHub> nuts = new List<ReferenceHub>();
					foreach (ReferenceHub play in players)
						play.characterClassManager.SetPlayersClass(RoleType.Spectator, play.gameObject);
					Timing.CallDelayed(1.0f, () =>
					{
						for (int i = 0; i < plugin.MaxPeanuts; i++)
						{
							int r = plugin.Gen.Next(players.Count);
							players[r].characterClassManager.SetPlayersClass(RoleType.Scp173, players[r].gameObject);
							players.Remove(players[r]);
							nuts.Add(players[r]);
						}

						foreach (ReferenceHub player in players)
							player.characterClassManager.SetPlayersClass(RoleType.ClassD, player.gameObject);

						Timing.CallDelayed(1.0f, () =>
						{
							foreach (ReferenceHub hub in players)
							{
								if (hub.characterClassManager.IsHuman())
								{
									Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp096);
									hub.playerMovementSync.OverridePosition(randomSP, 0f);
									hub.inventory.Clear();
									hub.AddItem(ItemType.Flashlight);
								}
							}
						});

						Timing.CallDelayed(120f, () =>
						{

							foreach (ReferenceHub nut in players)
							{
								if (!nut.characterClassManager.IsHuman())
								{
									Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
									nut.playerMovementSync.OverridePosition(randomSP, 0f);
								}
								if (nut.characterClassManager.IsHuman())
								{
									PlayerEffectsController componentInParent2 = nut.GetComponentInParent<PlayerEffectsController>();
									componentInParent2.GetEffect<CustomPlayerEffects.Deafened>();
									componentInParent2.EnableEffect<CustomPlayerEffects.Deafened>();
								}
							}
						});
					});
				});
			}
		}

		public void OnRoundEnd()
		{
			plugin.sd = false;
			plugin.sp = false;
			plugin.RoundStarted = false;
			plugin.rp = false;
			plugin.ew = false;
			if (plugin.Ga) plugin.GamemodeEnabled = false;
			if (plugin.Ga) Massacre.GamemodEnabled = false;
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			PlayerManager.localPlayer.GetComponent<Broadcast>().RpcClearElements();
			if (plugin.RoundStarted)
			{
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(plugin.jbct, plugin.jbc, false);
				if (plugin.sd)
				{
					Timing.CallDelayed(3.0f, () =>
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp096);
						ev.Player.characterClassManager.SetClassID(RoleType.ClassD);
						Timing.CallDelayed(0.5f, () =>
						{
							Vector3 randomP = Map.GetRandomSpawnPoint(RoleType.Scp096);
							ev.Player.playerMovementSync.OverridePosition(randomSP, 0f);
							ev.Player.playerMovementSync.OverridePosition(randomP, 0f);
							ev.Player.inventory.Clear();
							ev.Player.AddItem(ItemType.Flashlight);
						}
						);
					});
				}
				if (plugin.sp)
				{
					Timing.CallDelayed(3.0f, () =>
					{
						Vector3 randoSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
						ev.Player.characterClassManager.SetClassID(RoleType.Scp173);
						ev.Player.playerMovementSync.OverridePosition(randoSP, 0f);
						Timing.CallDelayed(0.5f, () =>
						{
							Vector3 andoSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
							ev.Player.playerMovementSync.OverridePosition(andoSP, 0f);
						}
						);
					});
				}
			}
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			if (ev.Door.DoorName.Contains("CHK") || ev.Door.DoorName.Contains("ARMORY") || ev.Door.DoorName.Contains("173") || ev.Door.DoorName.Contains("CHECKPOINT")) return;
			ev.Allow = true;
		}
		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			if (plugin.sd)
			{
				ev.ForceEnd = false;
				ev.Allow = false;
				return;
			}
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub d = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD).FirstOrDefault();
			if (plugin.sd)
			{
				ev.ForceEnd = false;
				ev.Allow = false;
				return;
			}
			if (pList.Count != 1)
			{
				ev.ForceEnd = false;
			}
			if (pList.Count == 1)
			{
				ev.ForceEnd = false;
				if (!plugin.rp)
				{
					d.Broadcast(plugin.wpbct, plugin.wpbc, false);
					Map.Broadcast(plugin.wbc.Replace("%player%", $"{d.GetNickname()}"), plugin.wbct);
					Map.StartNuke();
					plugin.Functions.checkopen();
					plugin.rp = true;
				}
				if (!plugin.ew)
				{
					plugin.ew = true;
				}
			}
			if (pList.Count == 0)
			{
				ev.ForceEnd = true;
			}
		}
		public void OnWarheadCancel(WarheadCancelEvent ev)
		{
			if (plugin.ew)
			{
				ev.Allow = false;
			}
		}
		public void OnTriggerTesla(ref TriggerTeslaEvent ev)
		{
			if (plugin.ew)
			{
				ev.Triggerable = false;
			}
		}
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			spawnuser(ev.Player);
		}
		public void spawnuser(ReferenceHub player)
		{
			if (plugin.sd)
			{
				Timing.CallDelayed(3.0f, () =>
				{
					Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp096);
					player.characterClassManager.SetClassID(RoleType.ClassD);
					Timing.CallDelayed(0.5f, () =>
					{
						player.playerMovementSync.OverridePosition(randomSP, 0f);
						player.playerMovementSync.OverridePosition(randomSP, 0f);
						player.inventory.Clear();
						player.AddItem(ItemType.Flashlight);
					}
					);
				});
			}
			if (plugin.sp)
			{
				Timing.CallDelayed(3.0f, () =>
				{
					Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
					player.characterClassManager.SetClassID(RoleType.Scp173);
					player.playerMovementSync.OverridePosition(randomSP, 0f);
					Timing.CallDelayed(0.5f, () =>
					{
						player.playerMovementSync.OverridePosition(randomSP, 0f);
					}
					);
					PlayerEffectsController componentInParent2 = player.GetComponentInParent<PlayerEffectsController>(); 
					componentInParent2.DisableEffect<CustomPlayerEffects.Deafened>();
				});
			}
		}
	}
}