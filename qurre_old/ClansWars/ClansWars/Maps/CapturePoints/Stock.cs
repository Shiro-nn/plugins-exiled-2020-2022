using ClansWars.Objects;
using MEC;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ClansWars.Maps.CapturePoints
{
    internal static class Stock
    {
        internal readonly static List<Model> Flags = new();
        private static bool _loaded = false;
        private static Vector3 _pos = new(83, 1028, -57);
        internal static void Gen(Qurre.API.Events.GeneratorActivateEvent _)
        {
            if (Round.ActiveGenerators == 4) Timing.RunCoroutine(RaketaActivate());
            static IEnumerator<float> RaketaActivate()
            {
                while (Round.Started)
                {
                    new Raketa(new(_pos.x + Random.Range(50, -50), _pos.y + 50, _pos.z + Random.Range(65, -65)));
                    yield return Timing.WaitForSeconds(Random.Range(15, 40));
                }
            }
        }
        internal static void DoorEvents(Qurre.API.Events.DoorDamageEvent ev)
        {
            ev.Allowed = false;
        }
        internal static void Load()
        {
            if (_loaded) throw new Exception("Map \"Stock\" already loaded");
            _loaded = true;
            Flags.Clear();
            try { DestroyGenerators(); } catch { }
            SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Init.MapsPath, "stock.json"), _pos);
            Model model = new("ClansWars.Stock", _pos);
            Color32 _mc = new(128, 128, 128, 255);
            LoadBaza(new(-65, 10), Vector3.zero);
            LoadBaza(new(65, 10), new(0, 180));

            LoadFlag(new(0, 12.46f, 60), Vector3.zero, model);
            LoadFlag(new(0, 12.46f, -60), new(0, 180), model);

            LoadTower(Vector3.zero, new(0, 0));

            void LoadBaza(Vector3 pos, Vector3 rot)
            {
                Model baza = new("Baza", pos, rot, model);

                baza.AddPart(new ModelGenerator(baza, new(-10, -9.5f, -9), new(0, 90), Vector3.one));
                baza.AddPart(new ModelGenerator(baza, new(-10, -9.5f, 9), new(0, 90), Vector3.one));

                baza.AddPart(new ModelLocker(baza, LockerPrefabs.LargeGun, new(-10, -9.5f), new(0, 90), Vector3.one));
                baza.AddPart(new ModelLocker(baza, LockerPrefabs.Pedestal500, new(-9.75f, -9.5f, -4.5f), new(0, 90), Vector3.one));
                baza.AddPart(new ModelLocker(baza, LockerPrefabs.Pedestal207, new(-9.75f, -9.5f, 4.5f), new(0, 90), Vector3.one));

                baza.AddPart(new ModelWorkStation(baza, new(9, -9.5f, -12f), new(0, -90), Vector3.one));
                baza.AddPart(new ModelWorkStation(baza, new(9, -9.5f, 12f), new(0, -90), Vector3.one));

                baza.AddPart(new ModelDoor(baza, DoorPrefabs.DoorEZ, new(9.5f, -9.519f), new(0, 90), Vector3.one));
            }
            void LoadTower(Vector3 pos, Vector3 rot)
            {
                Model tower = new("Tower", pos, rot, model);

                new Objects.Lift(new(tower.GameObject.transform, new(0, 3, -1.5f), Vector3.zero, false),
                    new(tower.GameObject.transform, new(0, 28, -1.5f), Vector3.zero, true), _mc);

                tower.AddPart(new ModelDoor(tower, DoorPrefabs.DoorHCZ, new(7.34f, 0.5f, 0.55f), new(0, 90), Vector3.one));
                tower.AddPart(new ModelDoor(tower, DoorPrefabs.DoorHCZ, new(-7.34f, 0.5f, 0.55f), new(0, 90), Vector3.one));

                tower.AddPart(new ModelGenerator(tower, new(7.18f, 0.35f, 4.27f), new(0, 270), Vector3.one));

                LoadFlag(new(0, 30, 3), new(0, 90), tower);


                //tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(), new(), new()));
            }
            void LoadFlag(Vector3 pos, Vector3 rot, Model root)
            {
                Model flag = new("Flag", pos, rot, root);
                flag.AddPart(new ModelPrimitive(flag, PrimitiveType.Cylinder, _mc, new(0, 1), Vector3.zero, new(1, 6, 1)));
                flag.AddPart(new ModelPrimitive(flag, PrimitiveType.Cube, _mc, new(-3.48f, 5f), Vector3.zero, new(7, 4, 0.5f)));
                flag.GameObject.AddComponent<RotateScript>();
                flag.GameObject.AddComponent<FixPrimitiveSmoothing>().Model = flag;
                flag.Primitives.ForEach(prim => prim.Primitive.MovementSmoothing = 64);
                Flags.Add(flag);
            }
            void DestroyGenerators()
            {
                while (Map.Generators.Count > 0) try { Map.Generators[0].Destroy(); } catch { }
            }
        }
        internal class RotateScript : MonoBehaviour
        {
            void Update()
            {
                transform.Rotate(Vector3.down * 0.05f);
            }
        }
    }
}