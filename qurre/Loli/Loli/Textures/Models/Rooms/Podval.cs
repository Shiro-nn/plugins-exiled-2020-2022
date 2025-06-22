using PlayerRoles;
using PlayerStatsSystem;
using Qurre.API.Addons.Models;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using UnityEngine;

namespace Loli.Textures.Models.Rooms
{
    internal class Podval
    {
        static GameObject TutorialSpawnPoint;

        [EventMethod(PlayerEvents.Spawn, 666)]
        static void SpawnChangePos(SpawnEvent ev)
        {
            if (ev.Role != RoleTypeId.Tutorial)
                return;

            ev.Position = TutorialSpawnPoint?.transform?.position ?? Vector3.zero;
        }

        public Model Model;
        private readonly Color32 Verevka = new(152, 138, 94, 255);
        private readonly Color32 Divan = new(245, 174, 71, 255);
        private readonly Color32 Box = new(51, 29, 13, 255);
        private readonly Color32 Metall = new(91, 91, 91, 255);
        private readonly Color32 Pol = new(160, 54, 35, 255);
        private readonly Color32 Stena = new(127, 99, 87, 255);
        private readonly Color32 Styl = new(123, 96, 61, 255);

        public Podval(Vector3 position, Vector3 rotation)
        {
            Model = new Model("Podval_Room", position, rotation);

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Pol, new Vector3(-2.8f, -2.25f, 12.17f), new Vector3(90, 0), new Vector3(10, 30, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Pol, new Vector3(-2.8f, 1.75f, 12.17f), new Vector3(270, 0), new Vector3(10, 30, 1)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Stena, new Vector3(-7.8f, -0.25f, 12.17f), new Vector3(180, 90, -270), new Vector3(4, 30, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Stena, new Vector3(2.2f, -0.25f, 12.17f), new Vector3(180, 270, -90), new Vector3(4, 30, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Stena, new Vector3(-2.8f, -0.25f, -2.83f), new Vector3(0, -180, 270), new Vector3(4, 10, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Stena, new Vector3(-2.8f, -0.25f, 27.15f), new Vector3(0, 0, 270), new Vector3(4, 10, 1)));

            Cell(new Vector3(-1.938f, 0, 18.413f), Vector3.zero);

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0.95f, -1.75f, 26.159f), new Vector3(0, -30), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(-0.97f, -1.75f, 26.159f), Vector3.zero, new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0, -1.75f, 25.19f), new Vector3(0, 30), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0, -0.781f, 26.159f), new Vector3(0, -15), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0.97f, -1.75f, 18.41f), new Vector3(0, 30), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0.97f, -1.75f, 16.47f), new Vector3(0, -30), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0, -1.75f, 17.44f), new Vector3(0, -105), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0, -1.75f, 15.5f), new Vector3(0, -15), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0.97f, -1.75f, 14.53f), new Vector3(0, 45), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0.97f, -0.78f, 15.499f), new Vector3(0, 135), new Vector3(1, 1, 1.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Box, new Vector3(0.97f, -0.78f, 17.437f), new Vector3(0, 45), new Vector3(1, 1, 1.2f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(0, -0.26f, 20.35f), Vector3.zero, new Vector3(0.4f, 4.1f, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(0, -0.26f, 11.63f), Vector3.zero, new Vector3(0.4f, 4.1f, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(0, -0.26f, 3.88f), Vector3.zero, new Vector3(0.4f, 4.1f, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(-5.81f, -0.26f, 11.63f), Vector3.zero, new Vector3(0.4f, 4.1f, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(-5.81f, -0.26f, 3.88f), Vector3.zero, new Vector3(0.4f, 4.1f, 0.4f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(1.178f, -1.795f, 20.35f), Vector3.zero, new Vector3(2.1f, 1, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(1.178f, -1.795f, 11.63f), Vector3.zero, new Vector3(2.1f, 1, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(1.178f, -1.795f, 3.88f), Vector3.zero, new Vector3(2.1f, 1, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(-6.85f, -1.795f, 11.63f), Vector3.zero, new Vector3(2.1f, 1, 0.4f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Stena, new Vector3(-6.85f, -1.795f, 3.88f), Vector3.zero, new Vector3(2.1f, 1, 0.4f)));

            SpawnDivan(new Vector3(-4.85f, -1.727f, 19.757f), Vector3.zero);

            AdminSucide(Vector3.zero, Vector3.zero);

            Color32 lightColor = new(160, 54, 35, 255);
            Model.AddPart(new ModelLight(Model, lightColor, new Vector3(-2.59f, 0, 2.91f), 1, 6, false));
            Model.AddPart(new ModelLight(Model, lightColor, new Vector3(-2.59f, 0, 13.57f), 1, 6, false));
            Model.AddPart(new ModelLight(Model, lightColor, new Vector3(-2.59f, 0, 24.23f), 1, 6, false));

            TutorialSpawnPoint = new Model("SpawnPoint", new(-2.59f, 0, 13.57f), Vector3.zero, Model).GameObject;
        }
        private void AdminSucide(Vector3 position, Vector3 rotation)
        {
            var _admin = new Model("Admin Sucide", position, rotation, Model);
            {
                var Stool = new Model("Stool", new Vector3(0.051f, -1.728f, -0.864f), new Vector3(-83.979f, 81.383f, -86.90701f), _admin);
                Stool.AddPart(new ModelPrimitive(Stool, PrimitiveType.Cube, Styl, Vector3.zero, Vector3.zero, new Vector3(1, 0.097207f, 1)));

                Stool.AddPart(new ModelPrimitive(Stool, PrimitiveType.Cube, Styl, new(-0.4337f, -0.3392f, 0.4327f), Vector3.zero, new Vector3(0.1325287f, 0.7761508f, 0.1325287f)));
                Stool.AddPart(new ModelPrimitive(Stool, PrimitiveType.Cube, Styl, new(-0.4337f, -0.3392f, -0.434f), Vector3.zero, new Vector3(0.1325287f, 0.7761508f, 0.1325287f)));
                Stool.AddPart(new ModelPrimitive(Stool, PrimitiveType.Cube, Styl, new(0.434f, -0.3392f, 0.4327f), Vector3.zero, new Vector3(0.1325287f, 0.7761508f, 0.1325287f)));
                Stool.AddPart(new ModelPrimitive(Stool, PrimitiveType.Cube, Styl, new(0.434f, -0.3392f, -0.434f), Vector3.zero, new Vector3(0.1325287f, 0.7761508f, 0.1325287f)));
            }
            {
                var pos = _admin.GameObject.transform.position;
                var rot = _admin.GameObject.transform.rotation.eulerAngles;
                new Ragdoll(RoleTypeId.Tutorial, new Vector3(pos.x + -1.708f, pos.y + -0.5f, pos.z + -0.023f),
                    Quaternion.Euler(new Vector3(rot.x + -46.6f, rot.y + -52.86f, rot.z + -40.5f)), new CustomReasonDamageHandler("По всей видимости, повесился"), "Главный Админ");
            }
            {
                var Petlya = new Model("Petlya", new Vector3(0.159f, 0.247f), new Vector3(0, 0, -11.321f), _admin);
                Petlya.AddPart(new ModelPrimitive(Petlya, PrimitiveType.Cube, Verevka, new(0.0999f, 0.7178f, 0.502f), Vector3.zero, new(0.067f, 1.744f, 0.076f)));
                Petlya.AddPart(new ModelPrimitive(Petlya, PrimitiveType.Cube, Verevka, new(0.1f, -0.16f, 0.5f), Vector3.zero, new(0.067f, 0.024f, 0.18f)));
                Petlya.AddPart(new ModelPrimitive(Petlya, PrimitiveType.Cube, Verevka, new(0.1f, -0.4f, 0.5f), Vector3.zero, new(0.067f, 0.024f, 0.18f)));
                Petlya.AddPart(new ModelPrimitive(Petlya, PrimitiveType.Cube, Verevka, new(0.09999f, -0.283f, 0.58f), Vector3.zero, new(0.067f, 0.259f, 0.025f)));
                Petlya.AddPart(new ModelPrimitive(Petlya, PrimitiveType.Cube, Verevka, new(0.09999f, -0.283f, 0.423f), Vector3.zero, new(0.067f, 0.259f, 0.025f)));
            }
        }
        private void SpawnDivan(Vector3 position, Vector3 rotation)
        {
            var _divan = new Model("Divan", position, rotation, Model);
            _divan.AddPart(new ModelPrimitive(_divan, PrimitiveType.Cube, Divan, new Vector3(0.008f, -0.203f, -0.053f), Vector3.zero, new Vector3(3, 0.67f, 1.348f)));
            _divan.AddPart(new ModelPrimitive(_divan, PrimitiveType.Cube, Divan, new Vector3(1.3486f, 0.002f, -0.129f), Vector3.zero, new Vector3(0.33f, 1.1f, 1.5f)));
            _divan.AddPart(new ModelPrimitive(_divan, PrimitiveType.Cube, Divan, new Vector3(-1.562f, 0.002f, -0.129f), Vector3.zero, new Vector3(0.33f, 1.1f, 1.5f)));
            _divan.AddPart(new ModelPrimitive(_divan, PrimitiveType.Cube, Divan, new Vector3(-0.103f, 0.01f, -0.75f), Vector3.zero, new Vector3(3.229f, 1.083f, 0.266f)));
        }
        private void Cell(Vector3 position, Vector3 rotation)
        {
            var Cell = new Model("Kletka", position, rotation, Model);
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Quad, Metall, new Vector3(-3.067f, -2.24f, 6.035f), new Vector3(90, 0), new Vector3(4, 4, 1)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Quad, Metall, new Vector3(-3.083f, 1.725f, 6.047f), new Vector3(270, 0), new Vector3(4, 4, 1)));

            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.169f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-2.14f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-3.11f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-4.08f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-4.994f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-4.57f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-3.65f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-2.67f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.68f, -0.24f, 4.126f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));

            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.169f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-2.14f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-3.11f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-4.08f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-4.994f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-4.57f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-3.65f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-2.67f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.68f, -0.24f, 7.963f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));

            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 6.986f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 6.016f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 5.046f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 4.556f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 5.476f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 6.456f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-1.163f, -0.24f, 7.446f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));

            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 6.986f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 6.016f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 5.046f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 4.556f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 5.476f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 6.456f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
            Cell.AddPart(new ModelPrimitive(Cell, PrimitiveType.Cube, Metall, new Vector3(-5, -0.24f, 7.446f), Vector3.zero, new Vector3(0.1f, 4, 0.1f)));
        }
    }
}