using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerXP.scp228
{
    public partial class EventHandlers228
	{
		private void escapescp228ruj()
		{
			scp228ruj.SetRank("[data deleted] уровень", "green");
			Cassie.Message(".g1 .g4 . last scp 2 2 8 j escape .g1 .g1 .g5 pitch_0.1 .g6", false, false);
			scp228ruj.inventory.Clear();
			scp228ruj.ChangeRole(RoleType.Spectator);
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.ebt, Configs.eb);
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			pickupspawn = false;
			ds = false;
			vodka1 = Configs.error1;
			vodka2 = Configs.error2;
			vodkacolor = Configs.error3;
			scp228ruj = null;
		}
		private void Killscp228ruj()
		{
			ds = false;
			scp228ruj.SetRank("[data deleted] уровень", "green");
			Cassie.Message(".g1 .g4 . last scp 2 2 8 j is dead .g1 .g1 .g5 pitch_0.1 .g6", false, false);
			scp228ruj.inventory.Clear();
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.dbt, Configs.db);
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			scp228ruj = null;
			pickupspawn = false;
			vodka1 = Configs.error1;
			vodka2 = Configs.error2;
			vodkacolor = Configs.error3;
		}

		public static void SpawnJG(ReferenceHub JG)
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
				hasTag = !string.IsNullOrEmpty(JG.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(JG.serverRoles.HiddenBadge);
				if (isHidden) JG.RefreshTag();
				Timing.CallDelayed(0.5f, () =>
				{
					GameCore.Console.singleton.TypeCommand($"/doortp {JG.GetPlayerId()}. 079_FIRST");
					GameCore.Console.singleton.TypeCommand($"/open 079_FIRST");
					GameCore.Console.singleton.TypeCommand($"/lock 079_FIRST");
				});
				ds = true;
				JG.characterClassManager.TargetConsolePrint(JG.scp079PlayerScript.connectionToClient, $"\n----------------------------------------------------------- \n{Configs.sc}\n ----------------------------------------------------------- ", Configs.scc);
				Cassie.Message(".g1 .g4 . all scp 2 2 8 j is dead .g1 .g1 .g5 . . o no . 1 scp 2 2 8 j escape", false, false);
				coroutines.Add(Timing.RunCoroutine(Gopd()));
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 939</color>";
						vodka2 = $"{Configs.s} SCP 939";
						vodkacolor = "red";
						return;
					}
				}
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp173);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
						checkopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 173</color>";
						vodka2 = $"{Configs.s} SCP 173";
						vodkacolor = "red";
						return;
					}
				}
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp106);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
                        dedopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 106</color>";
						vodka2 = $"{Configs.s} SCP 106";
						vodkacolor = "red";
						return;
					}
				}
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.ClassD);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
						checkopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=#ff5a00>D class'a</color>";
						vodka2 = $"{Configs.s} D class'a";
						vodkacolor = "yellow";
						return;
					}
				}
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp096);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
						aopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=red>SCP 096</color>";
						vodka2 = $"{Configs.s} SCP 096";
						vodkacolor = "red";
						return;
					}
				}
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.ChaosInsurgency);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
						checkopen = true;
						gateopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=green>Chaos Insurgency</color>";
						vodka2 = $"{Configs.s} Chaos Insurgency";
						vodkacolor = "green";
						return;
					}
				}
				if (!pickupspawn)
				{
					if (rand.Next(1, 100) < 15)
					{
						Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.NtfCadet);
						Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
						vodka = a;
						a.durability = dur;
						pickupspawn = true;
						checkopen = true;
						gateopen = true;
						vodka1 = $"<color=aqua>{Configs.s}</color> <color=#006dff>MTF</color>";
						vodka2 = $"{Configs.s} MTF";
						vodkacolor = "blue";
						return;
					}
				}
				if (!pickupspawn)
				{
					Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.FacilityGuard);
					Pickup a = Extensions.SpawnItem(ItemType.SCP207, 100000, randomSP);
					vodka = a;
					a.durability = dur;
					pickupspawn = true;
					checkopen = true;
					vodka1 = $"<color=aqua>{Configs.s}</color> <color=#949494>Охраны</color>";
					vodka2 = $"{Configs.s} Охраны";
					vodkacolor = "grey";
					return;
				}
            }
            else
            {
				selectspawnJG();
			}
		}
		public static void selectspawnJG()
		{
			List<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();

			if (Configs.dsreplace)
			{
				pList = Extensions.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.FacilityGuard && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			}

			if (pList.Count > 4 && scp228ruj == null)
			{
				SpawnJG(pList[rand.Next(pList.Count)]);
			}
		}



		public IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}
		private void GrantFF(ReferenceHub player)
		{
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}
	}
}
