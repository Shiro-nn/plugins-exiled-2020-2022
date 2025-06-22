using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerXP.items
{
    public class spawns
	{
		private readonly Plugin plugin;
		public spawns(Plugin plugin) => this.plugin = plugin;
		internal int scp012hp = 50;
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (ev.Door.DoorName == "012_BOTTOM")
			{
				ev.IsAllowed = false;
				if (ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId) return;
				if (ev.Player.Team == Team.SCP) return;
				if (scp012hp == -1) return;
				else if (scp012hp == 0)
				{
					scp012hp = -1;
					ev.Door.NetworkisOpen = true;
					ev.Door.Networklocked = true;
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast("<color=lime>УРА!</color>\n<color=#00ffff>Вы успешно открыли <color=red>SCP 012</color>, теперь вы можете взять предметы</color>", 10);
				}
				else
				{
					ev.Player.ClearBroadcasts();
					ev.Player.ReferenceHub.Broadcast($"<color=red>Осталось {scp012hp} открытий</color>", 5);
					ev.Player.ReferenceHub.Damage(10, DamageTypes.Scp049);
					scp012hp--;
				}
				return;
			}
		}
		public void OnRoundStart()
		{
			scp012hp = 50;

			Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(180, 986, 32));

			if (Random.Range(0, 100) > 50)
			{
				if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(-6.3f, -1995, 6.5f));
				}
				else if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(-5.5f, -1995, 5.5f));
				}
				else if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(4, -1995, 4));
				}
				else if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(7, -1995, 7));
				}
				else
				{
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(0, -1995, 0));
				}
			}
			else
			{
				if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(-5.5f, -1995, 5.5f));
				}
				else if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(-6.3f, -1995, 6.5f));
				}
				else if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(4, -1995, 4));
				}
				else if (Random.Range(0, 100) > 50)
				{
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(7, -1995, 7));
				}
				else
				{
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(0, -1995, 0));
				}
			}
			foreach (Scp079Interactable camera in Object.FindObjectsOfType<Scp079Interactable>())
			{
				if (camera.type == Scp079Interactable.InteractableType.Speaker)
				{
					Extensions.SpawnItem(ItemType.KeycardScientistMajor, 1, new Vector3(camera.transform.position.x, camera.transform.position.y + 1f, camera.transform.position.z));
				}
			}

			foreach (Door door in Object.FindObjectsOfType<Door>())
			{
				if (Vector3.Distance(door.transform.position, Map.GetRandomSpawnPoint(RoleType.Scp106)) <= 8.2f)
				{
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 1, new Vector3(door.transform.position.x + 8.13326f, door.transform.position.y + 25, door.transform.position.z + 1));
				}
				if (door.DoorName == "012_BOTTOM")
				{
					Vector3 scp1 = new Vector3(door.transform.position.x + 12, door.transform.position.y + 2, door.transform.position.z + 3.3f);
					Vector3 scp2 = new Vector3(door.transform.position.x - 12, door.transform.position.y + 2, door.transform.position.z - 3.3f);
					Vector3 scp3 = new Vector3(door.transform.position.x - 3.3f, door.transform.position.y + 2, door.transform.position.z + 12);
					Vector3 scp4 = new Vector3(door.transform.position.x + 3.3f, door.transform.position.y + 2, door.transform.position.z - 12);
					int hehe = 999999999;
					Extensions.SpawnItem(ItemType.KeycardO5, hehe, new Vector3(scp1.x, scp1.y, scp1.z - 0.7f));
					Extensions.SpawnItem(ItemType.GunProject90, hehe, new Vector3(scp1.x, scp1.y, scp1.z + 1));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp1.x, scp1.y - 1, scp1.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp1.x, scp1.y - 1, scp1.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp1.x, scp1.y - 1, scp1.z));
					spawnwork(scp1, 1);

					Extensions.SpawnItem(ItemType.KeycardFacilityManager, hehe, new Vector3(scp2.x, scp2.y, scp2.z + 1));
					Extensions.SpawnItem(ItemType.GunLogicer, hehe, new Vector3(scp2.x, scp2.y, scp2.z - 0.7f));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp2.x, scp2.y - 1, scp2.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp2.x, scp2.y - 1, scp2.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp2.x, scp2.y - 1, scp2.z));
					spawnwork(scp2, 2);

					Extensions.SpawnItem(ItemType.KeycardNTFCommander, hehe, new Vector3(scp3.x - 0.7f, scp3.y, scp3.z));
					Extensions.SpawnItem(ItemType.GunE11SR, hehe, new Vector3(scp3.x + 1, scp3.y, scp3.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp3.x, scp3.y - 1, scp3.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp3.x, scp3.y - 1, scp3.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp3.x, scp3.y - 1, scp3.z));
					spawnwork(scp3, 3);

					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, hehe, new Vector3(scp4.x - 0.7f, scp4.y, scp4.z));
					Extensions.SpawnItem(ItemType.GunE11SR, hehe, new Vector3(scp4.x + 1, scp4.y, scp4.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp4.x, scp4.y - 1, scp4.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp4.x, scp4.y - 1, scp4.z));
					Extensions.SpawnItem(ItemType.Medkit, hehe, new Vector3(scp4.x, scp4.y - 1, scp4.z));
					spawnwork(scp4, 4);
				}
			}
			spawnwork(new Vector3(-142.4f, 990f, -71.9f), 5);
		}
		public void spawnwork(Vector3 workp, int ids)
		{
			MapObjectLoaded objLoaded = new MapObjectLoaded();
			objLoaded.workStation = Object.Instantiate(NetworkManager.singleton.spawnPrefabs.Find(p => p.gameObject.name == "Work Station"));
			objLoaded.id = ids;
			objLoaded.name = "Custom Work Station";
			objLoaded.workStation.name = "Work Station";
			Offset offset = new Offset();
			objLoaded.position = new Vector3(workp.x, workp.y - 1, workp.z);
			if (ids == 1) objLoaded.rotation = new Vector3(0, 270, 0);
			if (ids == 3) objLoaded.rotation = new Vector3(0, 180, 0);
			if (ids == 2) objLoaded.rotation = new Vector3(0, 90, 0);
			if (ids == 5) objLoaded.rotation = new Vector3(0, 270, 0);
			objLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(objLoaded.rotation);
			offset.position = objLoaded.position;
			offset.rotation = objLoaded.rotation;
			offset.scale = Vector3.one;
			objLoaded.workStation.gameObject.transform.localScale = objLoaded.scale;
			objLoaded.workStation.AddComponent<WorkStationUpgrader>();
			NetworkServer.Spawn(objLoaded.workStation);
			objLoaded.workStation.GetComponent<WorkStation>().Networkposition = offset;
			objLoaded.workStation.GetComponent<WorkStation>().NetworkisTabletConnected = true;
			objLoaded.workStation.GetComponent<WorkStation>().Network_playerConnected = objLoaded.workStation;
		}
	}
}
