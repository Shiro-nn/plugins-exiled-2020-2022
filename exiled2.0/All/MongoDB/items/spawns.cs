using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MongoDB.items
{
	public class spawns
	{
		private readonly Plugin plugin;
		public spawns(Plugin plugin) => this.plugin = plugin;
		public void OnRoundStart()
		{
			Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(180, 986, 32));

			if (Random.Range(0, 100) > 50)
			{
				if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(-6.3f, -1995, 6.5f));
				else if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(-5.5f, -1995, 5.5f));
				else if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(4, -1995, 4));
				else if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(7, -1995, 7));
				else
					Extensions.SpawnItem(ItemType.KeycardFacilityManager, 10000, new Vector3(0, -1995, 0));
			}
			else
			{
				if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(-5.5f, -1995, 5.5f));
				else if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(-6.3f, -1995, 6.5f));
				else if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(4, -1995, 4));
				else if (Random.Range(0, 100) > 50)
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(7, -1995, 7));
				else
					Extensions.SpawnItem(ItemType.KeycardContainmentEngineer, 10000, new Vector3(0, -1995, 0));
			}
			foreach (Scp079Interactable camera in Object.FindObjectsOfType<Scp079Interactable>())
				if (camera.type == Scp079Interactable.InteractableType.Speaker)
					Extensions.SpawnItem(ItemType.KeycardScientistMajor, 10000, new Vector3(camera.transform.position.x, camera.transform.position.y + 1f, camera.transform.position.z));
		}
	}
}