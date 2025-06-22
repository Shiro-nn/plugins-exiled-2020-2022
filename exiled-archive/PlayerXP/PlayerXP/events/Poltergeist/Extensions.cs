using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PlayerXP.events.Poltergeist
{
    public static class Extensions
	{
		public static List<Pickup> haveBeenMoved = new List<Pickup>();
		public static List<Pickup> canBeFloating = new List<Pickup>();
		public static float maxOffset = 2.0f;
		public static float minForce = 0.0f;
		public static float maxForce = 100.0f;
		public static void Postfix(Pickup instance)
		{
			instance.Rb.useGravity = false;
			instance.GetComponent<Collider>().material = new PhysicMaterial() { bounciness = 100f, bounceCombine = PhysicMaterialCombine.Maximum };
			float rng = UnityEngine.Random.Range(0.1f, 0.5f);
			//float rng = UnityEngine.Random.Range(minForce, minForce);
			instance.Rb.AddForceAtPosition(Vector3.up * 40f, Vector3.up * 40f);
			Timing.CallDelayed(15f, () => {
				instance.Rb.useGravity = true;
			});
			Timing.CallDelayed(30f, () => {
				instance.Rb.AddForceAtPosition(Vector3.up * 140f, Vector3.up * 140f);
				instance.Rb.AddForceAtPosition(Vector3.left * 140f, Vector3.left * 240f);
			});
			Timing.CallDelayed(40f, () => {
				instance.Rb.AddForceAtPosition(Vector3.up * 540f, Vector3.up * 540f);
			});
		}
		public static void Checkopen()
		{
			foreach(TeslaGate tesla in Map.TeslaGates)
			{
				Timing.CallDelayed(1.5f, () => tesla.RpcInstantTesla());
				Timing.CallDelayed(11.4f, () => tesla.RpcInstantTesla());
			}
			foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				if (door.DoorName != "012_BOTTOM")
				{
					Timing.CallDelayed(1.5f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(1.7f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(1.9f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(2.1f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(2.3f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(2.5f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(2.7f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(11.4f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(11.6f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(11.8f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(12.0f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(12.2f, () => door.NetworkisOpen = false);
					Timing.CallDelayed(12.4f, () => door.NetworkisOpen = true);
					Timing.CallDelayed(12.6f, () => door.NetworkisOpen = false);
				}
		}
	}
}
