using MEC;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers.Items;
using Qurre.API.Objects;
using UnityEngine;
namespace ClansWars.Objects
{
    internal class Raketa
    {
        internal readonly Model Model;
        internal Raketa(Vector3 pos)
        {
            Model = new("Raketa", pos);

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Capsule, Color.black, Vector3.zero, Vector3.zero, new(5, 5, 5), false));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, new Color32(101, 91, 91, 255), new(0, 2.73f), Vector3.zero, new(5.1f, 5, 5.1f), false));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, new Color32(29, 29, 29, 255), new(0, -0.37f), Vector3.zero, new(5.2f, 0.5f, 5.2f), false));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, new Color32(29, 29, 29, 255), new(0, 1.76f), Vector3.zero, new(5.2f, 0.5f, 5.2f), false));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, new Color32(29, 29, 29, 255), new(0, 4.09f), Vector3.zero, new(5.2f, 0.5f, 5.2f), false));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, new Color32(101, 91, 91, 255), new(0, 5.7f), Vector3.zero, new(10, 1, 0.2f), false));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, new Color32(101, 91, 91, 255), new(0, 5.7f), new(0, 90), new(10, 1, 0.2f), false));

            var script = Model.GameObject.AddComponent<FlyController>();
            script.Model = Model;
        }
        public class FlyController : MonoBehaviour
        {
            private Rigidbody _rigidbody;
            private bool _collided;

            public Model Model;

            private void Start()
            {
                ChangeLayers(transform, 6);

                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.mass = 200;
                _rigidbody.drag = 3;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }

            private void Update()
            {
                if (!_collided)
                    transform.localEulerAngles += Vector3.up * Time.deltaTime * 100;
            }

            private void OnCollisionEnter(Collision _)
            {

                if (_collided) return;

                _collided = true;
                Destroy(gameObject.GetComponent<Rigidbody>());

                Vector3 pos = transform.position;

                foreach (var prim in Model.Primitives)
                {
                    if (prim.Primitive.Type == PrimitiveType.Capsule)
                    {
                        Destroy(prim.GameObject);
                        continue;
                    }
                    var r = prim.GameObject.AddComponent<Rigidbody>();
                    r.AddExplosionForce(100, pos, 20);
                }

                for (int i = 0; i < 15; i++)
                {
                    var gre = new GrenadeFrag(ItemType.GrenadeHE) { FuseTime = 0.1f };
                    gre.Spawn(pos + Vector3.up + (Vector3.left * Random.Range(-30, 30)) + (Vector3.forward * Random.Range(-30, 30)));
                }

                Timing.CallDelayed(0.1f, () =>
                {//Concussed Deafened Burned
                    foreach (var pl in Player.List)
                    {
                        if (Physics.Linecast(pos, pl.Position, pl.Movement.CollidableSurfaces)) continue;
                        var dist = Vector3.Distance(pos, pl.Position);
                        switch (dist)
                        {
                            case < 23:
                                {
                                    pl.Kill(DeathTranslations.Explosion);
                                    break;
                                }
                            case < 60:
                                {
                                    pl.ShakeScreen();
                                    pl.EnableEffect(EffectType.Concussed, 60 - dist, true);
                                    pl.EnableEffect(EffectType.Deafened, 60 - dist, true);
                                    pl.EnableEffect(EffectType.Burned, 60 - dist, true);
                                    pl.Damage((60 - dist) * 3, DeathTranslations.Explosion);
                                    break;
                                }
                        }
                    }
                });

                Timing.CallDelayed(5, () => Destroy(gameObject));
            }

            private static void ChangeLayers(Transform t, int layer)
            {
                t.gameObject.layer = layer;
                foreach (Transform child in t)
                {
                    ChangeLayers(child, layer);
                }
            }
        }
    }
}