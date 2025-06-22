using Interactables.Interobjects.DoorUtils;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Textures.Models.Rooms
{
    internal static class Bashni
    {
        internal static Vector3 ChaosSpawnPoint = Respawn.GetPosition(RoleTypeId.ChaosConscript);
        internal static Vector3 MTFSpawnPoint = Respawn.GetPosition(RoleTypeId.NtfCaptain);

        private static Model NewZone;
        private static Model Zero;
        internal static void Load()
        {
            Zero = new("Zero Position", Vector3.zero);
            LoadZone();
            LoadBashni();
        }
        private static void LoadZone()
        {
            Model model = new("NewZone", new(-300, -884.8f, -1100), Vector3.zero);
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, Vector3.zero, new(90, 0), new(40, 20, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(0, 2.5f, 9.8f), Vector3.zero, new(5, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(0, 2.5f, -9.8f), new(0, 180), new(5, 5, 1)));

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(-7.96f, 2.5f, -5.49f), Vector3.zero, new(0.4f, 5, 9)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(-7.96f, 2.5f, 5.49f), Vector3.zero, new(0.4f, 5, 9)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(-7.96f, 4, -0.01f), Vector3.zero, new(0.4f, 2, 2)));

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-14, 2.5f, -10), new(0, 180), new(12, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-14, 2.5f, 10), Vector3.zero, new(12, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-19.97f, 2.5f), new(0, 270), new(20, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(13.65f, 2.5f, 6.25f), new(0, 30), new(14.3f, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(13.65f, 2.5f, -6.25f), new(0, 150), new(14.3f, 5, 1)));

            model.AddPart(new ModelLocker(model, LockerPrefabs.Pedestal500, new(16.223f, 0, 4.728f), new(0, 209), Vector3.one));
            model.AddPart(new ModelLocker(model, LockerPrefabs.Pedestal500, new(16.223f, 0, -4.728f), new(0, -29), Vector3.one));

            {
                ModelPrimitive light = new(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(0, 5), new(90, 0), new(40, 20, 0.1f));
                model.AddPart(light);
                Timing.RunCoroutine(NeonLight(light), "NeonLightModel");
            }

            new Lift(new(model.GameObject.transform, new(5, 2.5f, -10), Vector3.zero, true),
                new(model.GameObject.transform, new(5, 50, -10), Vector3.zero, true), Textures.Load.WhiteColor);
            {
                var sp = new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(5.02f, 47.5f, -4.81f), new(90, 0), new(6, 10, 1));
                model.AddPart(sp);
                ChaosSpawnPoint = sp.GameObject.transform.position + (Vector3.up * 2);
            }
            model.AddPart(new ModelLight(model, Color.blue, new(5.02f, 49.5f, -4.81f), shadows: false));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(7.7f, 50.4f, -4.88f), new(0, 90), new(10, 6, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(2.26f, 50.4f, -4.88f), new(0, -90), new(10, 6, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(5.02f, 50.4f, -0.042f), Vector3.zero, new(6, 6, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(5.02f, 52.4f, -4.81f), new(-90, 0), new(6, 10, 1)));

            new Lift(new(model.GameObject.transform, new(-5, 2.5f, -10), Vector3.zero, true),
                new(model.GameObject.transform, new(-3.9f, -68.8f, -10), Vector3.zero, true), Textures.Load.WhiteColor);
            {
                var sp = new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-3.88f, -71.3f, -4.81f), new(90, 0), new(6, 10, 1));
                model.AddPart(sp);
                MTFSpawnPoint = sp.GameObject.transform.position + (Vector3.up * 2);
            }
            model.AddPart(new ModelLight(model, Color.blue, new(-3.88f, -69.3f, -4.81f), shadows: false));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-1.2f, -68.4f, -4.88f), new(0, 90), new(10, 6, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-6.64f, -68.4f, -4.88f), new(0, -90), new(10, 6, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-3.88f, -68.4f, -0.042f), Vector3.zero, new(6, 6, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-3.88f, -66.4f, -4.81f), new(-90, 0), new(6, 10, 1)));

            new Lift(new(model.GameObject.transform, new(-5, 2.5f, 10), new(0, 180), true),
                new(Zero.GameObject.transform, new(15.75f, 994.92f, -10.92f), new(0, -90)), Textures.Load.WhiteColor, true);
            new Lift(new(model.GameObject.transform, new(5, 2.5f, 10), new(0, 180), true),
                new(Range.Static.Model.GameObject.transform, new(-25.2f, 2.51f, 9.71f), new(0, 90), true), Textures.Load.WhiteColor);

            NewZone = model;

            Color32 kamin = new(178, 34, 34, 255);
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, kamin, new(0, 0.463f), Vector3.zero, Vector3.one));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, kamin, new(0.45f, 1.197f), Vector3.zero, new(0.1f, 0.5f, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, kamin, new(-0.45f, 1.197f), Vector3.zero, new(0.1f, 0.5f, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, kamin, new(0, 1.197f, -0.45f), Vector3.zero, new(1, 0.5f, 0.1f)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, kamin, new(0, 1.197f, 0.45f), Vector3.zero, new(1, 0.5f, 0.1f)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, kamin, new(0, 1.197f), Vector3.zero, new(0.3f, 0.3f, 0.3f)));
            model.AddPart(new ModelLight(model, kamin, new(0, 1.7f), shadows: false));

            Color32 door_color = new(150, 150, 150, 255);
            CreateDoor(new(-7.957f, 0), new(0, 90));
            void CreateDoor(Vector3 pos, Vector3 rot)
            {
                ModelDoor obj = new(NewZone, DoorPrefabs.DoorEZ, pos, rot, Vector3.one);
                NewZone.AddPart(obj);
                obj.Door.Name = "NewZone_Door";
                if (obj.Door.DoorVariant.TryGetComponent<DoorNametagExtension>(out var dnametag)) dnametag.UpdateName(obj.Door.Name);
                obj.Door.Permissions.RequiredPermissions = KeycardPermissions.ArmoryLevelTwo;
                var DoArmoryDoor = new Model("DoArmoryDoor", pos + Vector3.up * 2.5f, rot, NewZone);
                var ArmoryDoor = new Model("ArmoryDoor", Vector3.zero, Vector3.zero, DoArmoryDoor);
                ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, door_color, new(0, -1.56f), Vector3.zero, new(2, 2, 0.1f)));
                ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, door_color, new(0, 0.466f), Vector3.zero, new(2, 0.8f, 0.1f)));
                ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, door_color, new(0.914f, -0.244f), Vector3.zero, new(0.2f, 0.7f, 0.1f)));
                ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, door_color, new(-0.914f, -0.244f), Vector3.zero, new(0.2f, 0.7f, 0.1f)));
                ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, Range.Steklo, new(0, -0.244f), Vector3.zero, new(1.7f, 0.7f, 0.08f)));
                CreateButton(new(-1.277f, -0.713f, 0.209f), Vector3.zero);
                CreateButton(new(1.267f, -0.713f, -0.21f), new(0, 180));
                void CreateButton(Vector3 pos, Vector3 rot)
                {
                    Model Button = new("Button", pos, rot, DoArmoryDoor);
                    Button.AddPart(new ModelPrimitive(Button, PrimitiveType.Cube, new Color32(60, 60, 60, 255), Vector3.zero, Vector3.zero, new(0.25f, 0.25f, 0.1f)));
                    Button.AddPart(new ModelPrimitive(Button, PrimitiveType.Cylinder, Color.red, new(0, 0, 0.05f), new(90, 0), new(0.15f, 0.01f, 0.15f)));
                }
                Range.CustomDoors.Add(new(obj.Door.DoorVariant, ArmoryDoor.GameObject.transform));
            }
        }
        private static void LoadBashni()
        {
            Model model = new("Bashni", new(0.5f, 1000, -70), Vector3.zero);

            new Lift(new(NewZone.GameObject.transform, new(20, 2.5f, 0), new(0, -90), true),
                new(model.GameObject.transform, new(124.5f, 27.42f, 40.5f), new(0, -90), true), Textures.Load.WhiteColor, true);

            new Lift(new(model.GameObject.transform, new(0, 2.5f, 4.66f), Vector3.zero),
                new(model.GameObject.transform, new(0, 27.42f, 4.66f), Vector3.zero), Textures.Load.WhiteColor);

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(0, 14.94f, 0.36f), Vector3.zero, new(9, 20, 9)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(3.605f, 2.5f, 4.864f), new(0, 180), new(1.8f, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-3.605f, 2.5f, 4.864f), new(0, 180), new(1.8f, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(0, 2.5f, -4.14f), Vector3.zero, new(9, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-4.5f, 2.5f, 0.36f), new(0, 90), new(9, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(4.5f, 2.5f, 0.36f), new(0, -90), new(9, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(3.605f, 27.42f, 4.864f), new(0, 180), new(1.8f, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-3.605f, 27.42f, 4.864f), new(0, 180), new(1.8f, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(0, 27.42f, -4.14f), Vector3.zero, new(9, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(-4.5f, 27.42f, 0.36f), new(0, 90), new(9, 5, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Quad, Textures.Load.WhiteColor, new(4.5f, 27.42f, 0.36f), new(0, -90), new(9, 5, 1)));

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(25.5f, 24.84f, 9.35f), Vector3.zero, new(60, 0.1f, 9)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(60, 24.84f, 24.85f), Vector3.zero, new(9, 0.1f, 40)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(94.5f, 24.84f, 40.35f), Vector3.zero, new(60, 0.1f, 9)));

            {
                var light = new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(25.5f, 29.85f, 9.35f), Vector3.zero, new(60, 0.1f, 9));
                model.AddPart(light);
                Timing.RunCoroutine(NeonLight(light), "NeonLightModel");
            }
            {
                var light = new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(60, 29.85f, 24.85f), Vector3.zero, new(9, 0.1f, 40));
                model.AddPart(light);
                Timing.RunCoroutine(NeonLight(light), "NeonLightModel");
            }
            {
                var light = new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(94.5f, 29.85f, 40.35f), Vector3.zero, new(60, 0.1f, 9));
                model.AddPart(light);
                Timing.RunCoroutine(NeonLight(light), "NeonLightModel");
            }

            Color32 steklo = new(0, 133, 155, 150);
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(-4.46f, 27.34f, 9.34f), Vector3.zero, new(0.1f, 5, 9)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(64.45f, 27.34f, 20.355f), Vector3.zero, new(0.1f, 5f, 31)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(55.55f, 27.34f, 29.34f), Vector3.zero, new(0.1f, 5f, 31)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(34.48f, 27.34f, 4.81f), new(0, 90), new(0.1f, 5, 60)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(25.53f, 27.34f, 13.86f), new(0, 90), new(0.1f, 5, 60)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(90, 27.34f, 44.8f), new(0, 90), new(0.1f, 5, 69)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(94.5f, 27.34f, 35.9f), new(0, 90), new(0.1f, 5, 60)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(124.4f, 27.34f, 36.85f), Vector3.zero, new(0.1f, 5, 1.8f)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, steklo, new(124.4f, 27.34f, 43.982f), Vector3.zero, new(0.1f, 5, 1.8f)));

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(-4, 12.33f, 13.35f), Vector3.zero, new(1, 25, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(3.95f, 12.33f, 13.35f), Vector3.zero, new(1, 25, 1)));

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(56, 17.3f, 13.35f), Vector3.zero, new(1, 15, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(64, 17.3f, 13.35f), Vector3.zero, new(1, 15, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(56, 17.3f, 5.35f), Vector3.zero, new(1, 15, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(64, 17.3f, 5.35f), Vector3.zero, new(1, 15, 1)));

            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(56, 12.35f, 44.35f), Vector3.zero, new(1, 25, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(51.5f, 0.25f, 44.35f), Vector3.zero, new(10, 1, 1)));
            model.AddPart(new ModelPrimitive(model, PrimitiveType.Cube, Textures.Load.WhiteColor, new(127.52f, 0.5f, 40.5f), Vector3.zero, new(5.5f, 49, 5.5f)));
        }
        private static IEnumerator<float> NeonLight(ModelPrimitive prim)
        {
            bool plus = false;
            Color color = new(0, 0, 3);
            for (; ; )
            {
                if (plus) color.b += 0.1f;
                else color.b -= 0.1f;
                if (color.b < 1) plus = true;
                else if (color.b > 3) plus = false;
                prim.Primitive.Color = color;
                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }
}