using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MongoDB.scp228
{
	public partial class EventHandlers228
	{
		private void escapescp228ruj()
		{
			ReferenceHub player = scp228ruj;
			scp228ruj = null;
			Cassie.Message(".g1 .g4 . last scp 2 2 8 j escape .g1 .g1 .g5 pitch_0.1 .g6", false, false);
			scp228ruj.inventory.Clear();
			scp228ruj.ChangeRole(RoleType.Spectator);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.ebt, Configs.eb);
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			ds = false;
			vodka1 = Configs.error1;
			vodka2 = Configs.error2;
			vodkacolor = Configs.error3;
			try { plugin.donate.setprefix(player); } catch { player.SetRank("[data deleted] уровень", "green"); }
		}
		internal void Killscp228ruj()
		{
			ReferenceHub rh = scp228ruj;
			ds = false;
			Cassie.Message(".g1 .g4 . last scp 2 2 8 j is dead .g1 .g1 .g5 pitch_0.1 .g6", false, false);
			scp228ruj.inventory.Clear();
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.dbt, Configs.db);
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			scp228ruj = null;
			vodka1 = Configs.error1;
			vodka2 = Configs.error2;
			vodkacolor = Configs.error3;
			try { plugin.donate.setprefix(rh); } catch { rh.SetRank("[data deleted] уровень", "green"); }
		}

		public static void SpawnJG(ReferenceHub JG)
        {
			try
			{
				if (JG != null && Extensions.TryGet343().queryProcessor.PlayerId != JG.queryProcessor.PlayerId)
				{
					scp228ruj = JG;
					JG.ChangeRole(RoleType.ClassD);
					Timing.CallDelayed(1f, () =>
					{
						JG.inventory.Clear();
					});
					JG.ClearBroadcasts();
					JG.Broadcast(Configs.sb, Configs.sbt);
					Timing.CallDelayed(0.5f, () =>
					{
						GameCore.Console.singleton.TypeCommand($"/doortp {JG.GetPlayerId()}. 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/open 079_FIRST");
						GameCore.Console.singleton.TypeCommand($"/lock 079_FIRST");
					});
					ds = true;
					JG.characterClassManager.TargetConsolePrint(JG.scp079PlayerScript.connectionToClient, $"\n----------------------------------------------------------- \n{Configs.sc}\n ----------------------------------------------------------- ", Configs.scc);
					Cassie.Message(".g1 .g4 . all scp 2 2 8 j is dead .g1 .g1 .g5 . . o no . 1 scp 2 2 8 j escape", false, false);
					int random = Extensions.Random.Next(1, 100);
					if (random < 15)
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, 327, randomSP);
							vodka = a;
						});
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 939</color>";
						vodka2 = $"{Configs.s} SCP 939";
						vodkacolor = "red";
					}
					else if (random < 30)
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp173);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, dur, randomSP);
							vodka = a;
						});
						checkopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 173</color>";
						vodka2 = $"{Configs.s} SCP 173";
						vodkacolor = "red";
					}
					else if (random < 45)
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.ClassD);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, dur, randomSP);
							vodka = a;
						});
						checkopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=#ff5a00>D class'a</color>";
						vodka2 = $"{Configs.s} D class'a";
						vodkacolor = "yellow";
					}
					else if (random < 60)
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp096);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, dur, randomSP);
							vodka = a;
						});
						aopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 096</color>";
						vodka2 = $"{Configs.s} SCP 096";
						vodkacolor = "red";
					}
					else if (random < 75)
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.ChaosInsurgency);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, dur, randomSP);
							vodka = a;
						});
						checkopen = true;
						gateopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=green>Chaos Insurgency</color>";
						vodka2 = $"{Configs.s} Chaos Insurgency";
						vodkacolor = "green";
					}
					else if (random < 90)
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.NtfCadet);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, dur, randomSP);
							vodka = a;
						});
						checkopen = true;
						gateopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=#006dff>MTF</color>";
						vodka2 = $"{Configs.s} MTF";
						vodkacolor = "blue";
					}
					else
					{
						Timing.CallDelayed(5f, () =>
						{
							Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
							Pickup a = Extensions.SpawnItem(ItemType.SCP207, dur, randomSP);
							vodka = a;
						});
						checkopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=#949494>Охраны</color>";
						vodka2 = $"{Configs.s} Охраны";
						vodkacolor = "grey";
					}
				}
				else
				{
					selectspawnJG();
				}
            }
            catch
			{
				selectspawnJG();
			}
		}
		public static void selectspawnJG()
		{
			List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();

			if (Player.List.Count() > 4 && scp228ruj == null)
			{
				SpawnJG(pList[Extensions.Random.Next(pList.Count)]);
			}
		}
		private void GrantFF(ReferenceHub player)
		{
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}
	}
}