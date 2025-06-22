using MEC;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace PlayerXP.events.hideandseek
{
	public class EventHandlers
	{
		private readonly Massacre plugin;
		public EventHandlers(Massacre plugin) => this.plugin = plugin;

		private static System.Random rand = new System.Random();
		public void OnWaitingForPlayers()
		{
			plugin.RoundStarted = false;
			if(Server.Port == 7777)
			{
				plugin.Tessage = "fydne ff:off";
				plugin.botid = 739166429273915424;
			}
		}

		public void OnRoundStart()
		{
			if (!plugin.Ga)
			{
				ServerConsole._serverName = ServerConsole._serverName.Replace("<color=#0089c7>������� �����</color> <color=#15ff00>^<color=#ff0000>������</color>^</color>", "");
				ServerConsole._serverName += $"<color=#0089c7>������� �����</color> <color=#15ff00>^<color=#ff0000>������</color>^</color>";
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
					List<ReferenceHub> playerssl = Extensions.GetHubs().ToList();
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
					Map.TurnOffAllLights(666666666666, false);
					Cassie.Message("generator .g3 automatic reactivation started .g3 .g4", false, false);
					plugin.sd = false;
					plugin.sp = true;
				});
				Timing.CallDelayed(1.0f, () =>
				{
					List<ReferenceHub> players = Extensions.GetHubs().ToList();
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

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			plugin.sd = false;
			plugin.sp = false;
			plugin.RoundStarted = false;
			plugin.rp = false;
			plugin.ew = false;
			if (plugin.Ga) plugin.GamemodeEnabled = false;
			if (plugin.Ga) Massacre.GamemodEnabled = false;
			if (plugin.Ga) ServerConsole._serverName = ServerConsole._serverName.Replace("<color=#0089c7>������� �����</color> <color=#15ff00>^<color=#ff0000>������</color>^</color>", "");
		}

		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			PlayerManager.localPlayer.GetComponent<Broadcast>().RpcClearElements();
			if (plugin.RoundStarted)
			{
				ev.Player.ClearBroadcasts();
				ev.Player.ReferenceHub.Broadcast(plugin.jbc, plugin.jbct);
				if (plugin.sd)
				{
					Timing.CallDelayed(3.0f, () =>
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp096);
						ev.Player.ReferenceHub.characterClassManager.SetClassID(RoleType.ClassD);
						Timing.CallDelayed(0.5f, () =>
						{
							Vector3 randomP = Map.GetRandomSpawnPoint(RoleType.Scp096);
							ev.Player.ReferenceHub.playerMovementSync.OverridePosition(randomSP, 0f);
							ev.Player.ReferenceHub.playerMovementSync.OverridePosition(randomP, 0f);
							ev.Player.ReferenceHub.inventory.Clear();
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
						ev.Player.ReferenceHub.characterClassManager.SetClassID(RoleType.Scp173);
						ev.Player.ReferenceHub.playerMovementSync.OverridePosition(randoSP, 0f);
						Timing.CallDelayed(0.5f, () =>
						{
							Vector3 andoSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
							ev.Player.ReferenceHub.playerMovementSync.OverridePosition(andoSP, 0f);
						}
						);
					});
				}
			}
		}
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			if (ev.Door.DoorName.Contains("CHK") || ev.Door.DoorName.Contains("ARMORY") || ev.Door.DoorName.Contains("173") || ev.Door.DoorName.Contains("CHECKPOINT")) return;
			ev.IsAllowed = true;
		}
		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			if (plugin.sd)
			{
				ev.IsRoundEnded = false;
				ev.IsAllowed = false;
				return;
			}
			List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			ReferenceHub d = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD).FirstOrDefault();
			if (plugin.sd)
			{
				ev.IsRoundEnded = false;
				ev.IsAllowed = false;
				return;
			}
			if (pList.Count != 1)
			{
				ev.IsRoundEnded = false;
			}
			if (pList.Count == 1)
			{
				ev.IsRoundEnded = false;
				if (!plugin.rp)
				{
					plugin.plugin.donate.money[d.characterClassManager.UserId] += 50;
					d.Broadcast(plugin.wpbc, plugin.wpbct);
					d.Broadcast("<color=#fdffbb>�� �������� <color=red>50 �����</color> �� ������ �� ������!</color>", 5);
					Map.Broadcast(plugin.wbct, plugin.wbc.Replace("%player%", $"{d.GetNickname()}"));
					Extensions.StartNuke();
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
				ev.LeadingTeam = Exiled.API.Enums.LeadingTeam.ChaosInsurgency;
				ev.IsRoundEnded = true;
			}
		}
		public void OnWarheadCancel(StoppingEventArgs ev)
		{
			if (plugin.ew)
			{
				ev.IsAllowed = false;
			}
		}
		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (!plugin.GamemodeEnabled)
				return;
			spawnuser(ev.Target.ReferenceHub);
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