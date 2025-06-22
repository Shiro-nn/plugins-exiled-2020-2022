using UnityEngine;
using Qurre.API.Events;
using Loli.DataBase;
using InventorySystem.Items.Pickups;
using Qurre.API.Controllers.Items;
using Mirror;

namespace Loli.Items
{
    public class Physic
    {
        private static float ForceShoot => 100.0f;
        private static float RangeShoot => 7.0f;
        internal void Shoot(ShootingEvent ev)
        {
            if (Physics.Linecast(ev.Shooter.Position, ev.Message.TargetPosition, out RaycastHit raycastHit))
            {
                Vector3 pos = raycastHit.point;
                Collider[] array = Physics.OverlapSphere(raycastHit.point, RangeShoot);
                foreach (Collider collider in array) DoChange(collider, pos);
            }
            else
            {
                Vector3 pos = ev.Message.TargetPosition;
                Collider[] array = Physics.OverlapSphere(ev.Message.TargetPosition, RangeShoot);
                foreach (Collider collider in array) DoChange(collider, pos);
            }
            static void DoChange(Collider collider, Vector3 pos)
            {
                try
                {
                    var pick = collider.GetComponent<ItemPickupBase>();
                    if (pick == null || pick.Rb == null) return;
                    if (!Shop.ItsShop(Pickup.Get(pick))) pick.Rb.AddExplosionForce(ForceShoot, pos, RangeShoot);
                }
                catch { }
            }/*
            {
                if (Physics.Linecast(ev.Message.ShooterPosition, ev.Message.TargetPosition, out RaycastHit raycastHit5))
                {
                    Collider[] array = Physics.OverlapSphere(raycastHit5.point, RangeShoot);
                    foreach (Collider collider in array) DoGrenadeChange(collider);
                    void DoGrenadeChange(Collider collider)
                    {
                        var pick = collider.GetComponent<ItemPickupBase>();
                        Qurre.Log.Info(collider);
                        Qurre.Log.Info(pick);
                        if (pick == null || pick.Rb == null) return;
                        if (!Shop.ItsShop(Pickup.Get(pick)) && (pick.Info.ItemId == ItemType.GrenadeHE || pick.Info.ItemId == ItemType.GrenadeFlash))
                        {
                            pick.DestroySelf();
                            var item = new Throwable(pick.Info.ItemId);
                            var _pick = item.Spawn(ev.Message.TargetPosition);
                            var thrownProjectile = Object.Instantiate(item.Projectile);
                            if (thrownProjectile.TryGetComponent<Rigidbody>(out var rigidbody))
                            {
                                rigidbody.position = _pick.Base.Rb.position;
                                rigidbody.rotation = _pick.Base.Rb.rotation;
                                rigidbody.velocity = _pick.Base.Rb.velocity;
                                rigidbody.angularVelocity = rigidbody.angularVelocity;
                            }
                            _pick.Base.Info.Locked = true;
                            thrownProjectile.NetworkInfo = _pick.Base.Info;
                            NetworkServer.Spawn(thrownProjectile.gameObject);
                            thrownProjectile.InfoReceived(default, _pick.Base.Info);
                            thrownProjectile.ServerActivate();
                            _pick.Destroy();
                        }
                    }
                }
            }*/
        }
    }
}