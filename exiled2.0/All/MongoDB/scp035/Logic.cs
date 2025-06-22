using Exiled.API.Features;
using MEC;
using Mirror;
using MongoDB.scp228.API;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MongoDB.scp035
{
    public partial class EventHandlers
	{
		private static void RemovePossessedItems()
		{
			foreach(Pickup p in UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.durability == dur)) p.Delete();
		}

		private static Pickup TryGetvodka()
		{
			return Scp228Data.Getvodka();
		}
		internal void RefreshItems()
		{
			RemovePossessedItems();
			for (int i = 0; i < 5; i++)
			{
				Vector3 m = Map.Rooms[UnityEngine.Random.Range(0, Map.Rooms.Count - 1)].Position + Vector3.up;
				int it = UnityEngine.Random.Range(6, 33);
				Pickup p = Extensions.SpawnItem((ItemType)it, dur, m);
			}
		}

		internal void KillScp035(bool setRank = true)
		{
			ReferenceHub player = scpPlayer;
			scpPlayer.playerStats.maxHP = maxHP;
			scpPlayer = null;
			Cassie.Message("scp 0 3 5 containment minute");
			isRotating = true;
			RefreshItems();
			if (setRank)
			{
				try { Main035.plugin.donate.setprefix(player); } catch { player.SetRank("[data deleted] уровень", "green"); }
			}
		}

		public static void Spawn035(ReferenceHub p035)
		{
			if (scpPlayer == null)
			{
				maxHP = 300;
				p035.playerStats.maxHP = 300;
				p035.playerStats.Health = 300;
				p035.Broadcast($"<size=60>Вы-<color=\"red\"><b>SCP-035</b></color></size>\nВы заразили тело и получили контроль над ним, используйте его, чтобы помочь другим SCP!", 10);
				Cassie.Message("ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE . ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B", false, false);
				Cassie.Message("SCP 0 3 5 ESCAPE", false, false);
				scpPlayer = p035;
				try { Main035.plugin.donate.setprefix(p035); } catch { p035.SetRank("SCP 035", "red"); }
			}
		}
		public void InfectPlayer(ReferenceHub player, Pickup pItem)
		{
			pItem.Delete();
			isRotating = false;
			Timing.CallDelayed(3f, () => Spawn035(player));
			RemovePossessedItems();
		}

		internal void CorrodeUpdate()
        {
            try
			{
				if (scpPlayer != null && isRoundStarted)
				{
					IEnumerable<ReferenceHub> pList = Extensions.GetHubs().Where(x => x.queryProcessor.PlayerId != scpPlayer.queryProcessor.PlayerId);
					pList = pList.Where(x => Extensions.GetTeam(x) != Team.SCP);
					pList = pList.Where(x => Extensions.GetTeam(x) != Team.TUT);
					pList = pList.Where(x => x.queryProcessor.PlayerId != Extensions.TryGet343()?.queryProcessor.PlayerId);
					pList = pList.Where(x => x.queryProcessor.PlayerId != Extensions.TryGet228()?.queryProcessor.PlayerId);
					foreach (ReferenceHub player in pList)
					{
						if (player != null && Vector3.Distance(scpPlayer.transform.position, player.transform.position) <= 1.5f)
						{
							player.Broadcast("<size=25%><color=#6f6f6f>Вас атакует <color=red>SCP 035</color></color></size>", 1);
							CorrodePlayer(player);
						}
						else if (player != null && Vector3.Distance(scpPlayer.transform.position, player.transform.position) <= 15f)
						{
							player.Broadcast("<size=25%><color=#f47fff>*<color=#0089c7>принюхивается</color>*</color>\n<color=#6f6f6f>Вы чувствуете запах гнили, похоже это <color=red>SCP 035</color></color></size>", 1);
						}
					}
				}
            } catch {}
		}

		private void CorrodePlayer(ReferenceHub player)
		{
			if (scpPlayer != null)
			{
				int currHP = (int)scpPlayer.playerStats.Health;
				scpPlayer.playerStats.Health = currHP + 5 > 300 ? 300 : currHP + 5;
			}
			if (player.playerStats.Health - 5 > 0)
			{
				player.playerStats.Health -= 5;
			}
			else
			{
				player.Damage(55555, DamageTypes.None);
				ReferenceHub spy = scpPlayer;
				{
					Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
					foreach (var item in spy.inventory.items) items.Add(item);
					Vector3 pos1 = player.transform.position;
					Quaternion rot = spy.transform.rotation;
					int health = (int)spy.playerStats.Health;

					spy.SetRole(player.GetRole());

					Timing.CallDelayed(0.3f, () =>
					{
						spy.playerMovementSync.OverridePosition(pos1, 0f);
						spy.SetRotation(rot.x, rot.y);
						spy.inventory.items.Clear();
						foreach (var item in items) spy.inventory.AddNewItem(item.id);
						spy.playerStats.Health = health;
						spy.ammoBox.ResetAmmo();
					});
				}
				player.Damage(5, DamageTypes.Nuke);
				foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
				{
					if (doll.owner.PlayerId == player.queryProcessor.PlayerId)
					{
						NetworkServer.Destroy(doll.gameObject);
					}
				}
			}
		}
	}
}
