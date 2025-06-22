using Grenades;
using Mirror;
using System.Linq;
using UnityEngine;

namespace PlayerXP.items
{
    public class physic
	{
		private readonly Plugin plugin;
		public physic(Plugin plugin) => this.plugin = plugin;
		private static readonly int grenade_pickup_mask = 1049088;
		public static float forceShoot = 100.0f;
		public static float rangeShoot = 7.0f;
		internal void Shoot(Exiled.Events.EventArgs.ShootingEventArgs ev)
		{
			if (Physics.Linecast(ev.Shooter.ReferenceHub.transform.position, ev.Position, out RaycastHit raycastHt, grenade_pickup_mask))
			{
				var cpickup = raycastHt.transform.GetComponentInParent<Pickup>();
				if (cpickup == shop.shop1 || cpickup == shop.shop2 || cpickup == shop.shop3 || cpickup == shop.shop4 || cpickup == shop.shop5 || cpickup == shop.shop6 || cpickup == shop.shop7 || cpickup == shop.shop8 || cpickup == shop.shop9 ||
					cpickup == shop.shop10 || cpickup == shop.shop11 || cpickup == shop.shop12 || cpickup == shop.shop13 || cpickup == shop.shop14 || cpickup == shop.shop15 || cpickup == shop.shop16 || cpickup == shop.shop17 || cpickup == shop.shop18 || cpickup == shop.shop19
					 || cpickup == shop.shop20 || cpickup == shop.shop21 || cpickup == shop.shop22 || cpickup == shop.shop23 || cpickup == shop.shop24 || cpickup == shop.shop25)
				{ }
				else
				{
					try
					{
						ReferenceHub hub = ev.Shooter.ReferenceHub;
						if (ev.Position != Vector3.zero
							&& Physics.Linecast(ev.Shooter.ReferenceHub.transform.position, ev.Position, out RaycastHit raycastHit, grenade_pickup_mask))
						{
							var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
							if (pickup != null && pickup.Rb != null)
							{
								pickup.Rb.AddExplosionForce(Vector3.Distance(ev.Position, ev.Shooter.ReferenceHub.transform.position), ev.Shooter.ReferenceHub.transform.position, 500f, 3f, ForceMode.Impulse);
							}

							var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
							if (grenade != null)
							{
								grenade.NetworkfuseTime = 0.1f;
							}
						}
					}
					catch
					{ }
					try
					{
						if (Physics.Linecast(ev.Shooter.ReferenceHub.transform.position, ev.Position, out RaycastHit raycastHit, grenade_pickup_mask))
						{
							var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
							if (pickup != null && pickup.Rb != null)
							{
								pickup.Rb.AddExplosionForce(Vector3.Distance(ev.Position, ev.Shooter.ReferenceHub.transform.position), ev.Shooter.ReferenceHub.transform.position, 500f, 3f, ForceMode.Impulse);
							}

							var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
							if (grenade != null)
							{
								grenade.NetworkfuseTime = 0.1f;
							}
						}
					}
					catch
					{ }
					try
					{
						RaycastHit info;
						if (Physics.Linecast(ev.Shooter.ReferenceHub.playerMovementSync.transform.position, ev.Position, out info))
						{
							Collider[] arr = Physics.OverlapSphere(info.point, rangeShoot);
							foreach (Collider collider in arr)
							{
								if (collider.GetComponent<Pickup>() != null)
								{
									collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, info.point, rangeShoot);
								}
							}
						}
						else
						{
							Collider[] arr = Physics.OverlapSphere(ev.Position, rangeShoot);
							foreach (Collider collider in arr)
							{
								if (collider.GetComponent<Pickup>() != null)
								{
									collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, ev.Position, rangeShoot);
								}
							}
						}
					}
					catch
					{ }
					try
					{
						if (Physics.Linecast(ev.Shooter.ReferenceHub.transform.position, ev.Position, out RaycastHit raycastHit, grenade_pickup_mask))
						{
							var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
							if (pickup.ItemId == ItemType.GrenadeFrag)
							{
								pickup.Delete();
								var pos = ev.Position;
								GrenadeManager gm = ev.Shooter.ReferenceHub.GetComponent<GrenadeManager>();
								GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFrag);
								FragGrenade granade = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FragGrenade>();
								granade.fuseDuration = 0.2f;
								granade.InitData(gm, Vector3.zero, Vector3.zero, 0f);
								granade.transform.position = pos;
								NetworkServer.Spawn(granade.gameObject);
							}
							if (pickup.ItemId == ItemType.GrenadeFlash)
							{
								pickup.Delete();
								var pos = ev.Position;
								GrenadeManager gm = ev.Shooter.ReferenceHub.GetComponent<GrenadeManager>();
								GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFlash);
								FlashGrenade flash = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FlashGrenade>();
								flash.fuseDuration = 0.1f;
								flash.InitData(gm, Vector3.zero, Vector3.zero, 0f);
								flash.transform.position = pos;
								NetworkServer.Spawn(flash.gameObject);
							}
						}
					}
					catch
					{ }
				}
			}
		}
	}
}
