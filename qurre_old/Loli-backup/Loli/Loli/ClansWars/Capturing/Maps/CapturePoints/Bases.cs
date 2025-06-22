using System.Collections.Generic;
using System.Linq;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using Qurre.API.Addons.Models;
using UnityEngine;
namespace Loli.ClansWars.Capturing.Maps.CapturePoints
{
    internal class Bases
    {
        internal static bool Enabled { get; private set; } = false;
        internal static Bases Static { get; private set; }
        internal Bases(CapturingManager mngr)
        {
            Manager = mngr;
            Static = this;
        }
        internal CapturingManager Manager;
        internal void Disable()
        {
            if (!Enabled) return;
            Enabled = false;
            Round.Lock = false;
            Round.End();
            Map.Broadcast("<size=30%><color=#fdffbb>Клановая Война окончена</color></size>", 15);
        }
        internal void Enable()
        {
            Enabled = true;
        }
        internal static Model MapMod { get; private set; }
        internal static void LoadMap()
        {
            Color32 _mc = new(200, 200, 200, 255);
            Color32 _st = new(0, 0, 0, 100);
            Model Map = new("ClansWars-Bases", new(83, 1028, -57));
            MapMod = Map;
            Map.AddPart(new ModelPrimitive(Map, PrimitiveType.Cube, _mc, Vector3.zero, Vector3.zero, new(150, 1, 150)));
            Map.AddPart(new ModelPrimitive(Map, PrimitiveType.Cube, _mc, new(75.5f, 14.5f), new(0, 0, 90), new(30, 1, 150)));
            Map.AddPart(new ModelPrimitive(Map, PrimitiveType.Cube, _mc, new(-75.5f, 14.5f), new(0, 0, 90), new(30, 1, 150)));
            Map.AddPart(new ModelPrimitive(Map, PrimitiveType.Cube, _mc, new(0, 14.5f, 75.5f), new(0, 90, 90), new(30, 1, 150)));
            Map.AddPart(new ModelPrimitive(Map, PrimitiveType.Cube, _mc, new(0, 14.5f, -75.5f), new(0, 90, 90), new(30, 1, 150)));

            LoadBaza(new(-65, 10), Vector3.zero);
            LoadBaza(new(65, 10), new(0, 180));

            LoadPlace(new(0, 0, 60), Vector3.zero);
            LoadPlace(new(0, 0, -60), new(0, 180));

            LoadTower(Vector3.zero, new(0,180));

            void LoadBaza(Vector3 pos, Vector3 rot)
            {
                Model baza = new("Baza", pos, rot, Map);

                baza.AddPart(new ModelGenerator(baza, new(-10, -9.5f, -9), new(0, 90), Vector3.one));
                baza.AddPart(new ModelGenerator(baza, new(-10, -9.5f, 9), new(0, 90), Vector3.one));

                baza.AddPart(new ModelLocker(baza, LockerPrefabs.LargeGun, new(-10, -9.5f), new(0, 90), Vector3.one));
                baza.AddPart(new ModelLocker(baza, LockerPrefabs.Pedestal500, new(-9.75f, -9.5f, -4.5f), new(0, 90), Vector3.one));
                baza.AddPart(new ModelLocker(baza, LockerPrefabs.Pedestal207, new(-9.75f, -9.5f, 4.5f), new(0, 90), Vector3.one));

                baza.AddPart(new ModelWorkStation(baza, new(9, -9.5f, -12f), new(0, -90), Vector3.one));
                baza.AddPart(new ModelWorkStation(baza, new(9, -9.5f, 12f), new(0, -90), Vector3.one));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Quad, _mc, new(-8, -7.25f, 22), new(0, 180, 90), new(4.5f, 4, 1)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Quad, _mc, new(-8, -7.25f, -22), new(0, 0, 90), new(4.5f, 4, 1)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, Vector3.zero, Vector3.zero, new(20, 1, 30)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(9.5f, 1.25f), new(0, 0, 90), new(1.5f, 1, 30)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(9.5f, 4.5f), new(0, 0, 90), new(5, 1, 30)));
                //^
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(1.5f, 1.25f, 14.5f), new(0, 90, 90), new(1.5f, 1, 15)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(1.5f, 4.5f, 14.5f), new(0, 90, 90), new(5, 1, 15)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(-8, -4.36f, 16.88f), new(0, 90, 90), new(1, 4, 4)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(-5.5f, -3.75f, 19f), new(0, 90, 90), new(11.5f, 8, 1)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(-5.5f, 4.5f, 19f), new(0, 90, 90), new(5, 8, 1)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(-8, -1.5f, 22.5f), new(0, 0, 90), new(7, 4, 1)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(-8, 4.5f, 22.5f), new(0, 0, 90), new(5, 4, 1)));
                //^
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(1.5f, 1.25f, -14.5f), new(0, 90, 90), new(1.5f, 1, 15)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(1.5f, 4.5f, -14.5f), new(0, 90, 90), new(5, 1, 15)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(-8, -4.36f, -16.88f), new(0, 90, 90), new(1, 4, 4)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(-5.5f, -3.75f, -19f), new(0, 90, 90), new(11.5f, 8, 1)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(-5.5f, 4.5f, -19f), new(0, 90, 90), new(5, 8, 1)));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(-8, -1.5f, -22.5f), new(0, 0, 90), new(7, 4, 1)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _st, new(-8, 4.5f, -22.5f), new(0, 0, 90), new(5, 4, 1)));

                baza.AddPart(new ModelDoor(baza, DoorPrefabs.DoorEZ, new(9.5f, -9.519f), new(0, 90), Vector3.one));

                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(0, -5, -14.5f), new(0, 90, 90), new(9, 1, 20)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(0, -5, 14.5f), new(0, 90, 90), new(9, 1, 20)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(9.5f, -3.25f), new(0, 0, 90), new(6, 1, 28)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(9.5f, -7.86f, -7.5f), new(0, 0, 90), new(3.25f, 1, 13)));
                baza.AddPart(new ModelPrimitive(baza, PrimitiveType.Cube, _mc, new(9.5f, -7.86f, 7.5f), new(0, 0, 90), new(3.25f, 1, 13)));
            }
            void LoadPlace(Vector3 pos, Vector3 rot)
            {
                Model place = new("Place", pos, rot, Map);

                place.AddPart(new ModelPrimitive(place, PrimitiveType.Cube, _mc, new(0, 7), Vector3.zero, new(30, 1, 30)));
                place.AddPart(new ModelPrimitive(place, PrimitiveType.Cube, _mc, new(0, 4.5f, -15.5f), new(0, 90, 90), new(8, 1, 60)));
                place.AddPart(new ModelPrimitive(place, PrimitiveType.Cube, _mc, new(-22.45f, 3.45f), new(0, 0, 25), new(17, 1, 30)));
                place.AddPart(new ModelPrimitive(place, PrimitiveType.Cube, _mc, new(22.45f, 3.45f), new(0, 0, -25), new(17, 1, 30)));

                LoadFlag(new(0, 12.46f), Vector3.zero, place);
            }
            void LoadTower(Vector3 pos, Vector3 rot)
            {
                Model tower = new("Tower", pos, rot, Map);

                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(0, 15.45f), Vector3.zero, new(15, 20, 15)));

                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Quad, _mc, new(4.966f, 3, -1.3f), new(0, 180), new(4.5f, 5, 0.1f)));
                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Quad, _mc, new(-4.966f, 3, -1.3f), new(0, 180), new(4.5f, 5, 0.1f)));

                new Textures.Models.Lift(new(tower.GameObject.transform, new(0, 3, -1.5f), Vector3.zero, false),
                    new(tower.GameObject.transform, new(0, 28, -1.5f), Vector3.zero, true), _mc);

                tower.AddPart(new ModelDoor(tower, DoorPrefabs.DoorHCZ, new(7.34f, 0.5f, 0.55f), new(0, 90), Vector3.one));
                tower.AddPart(new ModelDoor(tower, DoorPrefabs.DoorHCZ, new(-7.34f, 0.5f, 0.55f), new(0, 90), Vector3.one));

                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(7.345f, 3, -3.95f), Vector3.zero, new(0.3f, 5, 7.1f)));
                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(7.345f, 3, 4.5f), Vector3.zero, new(0.3f, 5, 6)));
                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(7.345f, 4.784f, 0.54f), Vector3.zero, new(0.3f, 2, 2)));

                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(-7.345f, 3, -3.95f), Vector3.zero, new(0.3f, 5, 7.1f)));
                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(-7.345f, 3, 4.5f), Vector3.zero, new(0.3f, 5, 6)));
                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(-7.345f, 4.784f, 0.54f), Vector3.zero, new(0.3f, 2, 2)));

                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _st, new(0,3, 7.34f), new(0, 90), new(0.3f, 5, 14.4f)));
                tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(0, 3, -7.45f), new(0, 90), new(0.1f, 5, 14.4f)));

                LoadFlag(new(0, 30, 3), new(0, 90), tower);


                //tower.AddPart(new ModelPrimitive(tower, PrimitiveType.Cube, _mc, new(), new(), new()));
            }
            void LoadFlag(Vector3 pos, Vector3 rot, Model root)
            {
                Model flag = new("Flag", pos, rot, root);
                flag.AddPart(new ModelPrimitive(flag, PrimitiveType.Cylinder, _mc, new(0, 1), Vector3.zero, new(1, 6, 1)));
                flag.AddPart(new ModelPrimitive(flag, PrimitiveType.Cube, _mc, new(-3.48f, 5f), Vector3.zero, new(7, 4, 0.5f)));
            }
        }
    }
}