using EXILED;
using EXILED.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FloatingItems
{
    public class FIEvents
    {
        public static List<string> activeUsersFitems = new List<string>();
        public static List<string> activeUsersSitems = new List<string>();

        internal void ItemDropped(ItemDroppedEvent ev)
        {
            PickupUpdatePatch.canBeFloating.Add(ev.Item);
        }

        internal void RoundStart()
        {
            PickupUpdatePatch.canBeFloating.Clear();
            PickupUpdatePatch.haveBeenMoved.Clear();
        }

        internal void PlayerShoot(ref ShootEvent ev)
        {
                RaycastHit info;
            if (Physics.Linecast(ev.Shooter.GetComponent<Scp049PlayerScript>().plyCam.transform.position, ev.TargetPos, out info))
            {
                Collider[] arr = Physics.OverlapSphere(info.point, FloatingItems.rangeShoot);
                foreach (Collider collider in arr)
                {
                    if (collider.GetComponent<Pickup>() != null)
                    {
                        PickupStartPatch.Postfix(collider.GetComponent<Pickup>());
                        collider.GetComponent<Pickup>().Rb.AddExplosionForce(FloatingItems.forceShoot, info.point, FloatingItems.rangeShoot);
                    }
                }
            }
            else
            {
                Collider[] arr = Physics.OverlapSphere(ev.TargetPos, FloatingItems.rangeShoot);
                foreach (Collider collider in arr)
                {
                    if (collider.GetComponent<Pickup>() != null)
                    {
                        PickupStartPatch.Postfix(collider.GetComponent<Pickup>());
                        collider.GetComponent<Pickup>().Rb.AddExplosionForce(FloatingItems.forceShoot, ev.TargetPos, FloatingItems.rangeShoot);
                    }
                }
            }
        }
    }
}