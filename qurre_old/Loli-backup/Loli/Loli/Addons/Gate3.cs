using System.Linq;
using Loli.Textures.Models;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace Loli.Addons
{
    internal static class Gate3
    {
        internal static void Load()
        {
            var prim = PrimitiveType.Quad;
            var col = new Color32(0, 0, 0, 1);
            var doroga = new Color32(59, 60, 62, 255);
            var stena_white = new Color32(199, 200, 201, 255);
            Model Model = new("Gate3", Vector3.zero, Vector3.zero);
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-86, 982.2375f, -58.65f), new(90, 0, 0), new(55, 15.3f, 1)));//0
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-68.09f, 982.2375f, -68.64f), new(90, 0, 0), new(18, 5, 1)));//1
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-80.18f, 982.2375f, -49.03f), new(90, 0, 0), new(6, 4, 1)));//2
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-80.18f, 984.83f, -47.04f), Vector3.zero, new(6, 5.2f, 1)));//3
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-80.18f, 987.47f, -49.03f), new(270, 0, 0), new(6, 4, 1)));//4
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-77.48f, 984.83f, -49.25f), new(0, 90, 0), new(4.1f, 5.2f, 1)));//5
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-83.048f, 984.83f, -49.2f), new(0, 270, 0), new(4.1f, 5.2f, 1)));//6
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-67.97f, 984.83f, -51.28f), Vector3.zero, new(19, 5.2f, 1)));//7
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-111.53f, 984.83f, -51.26f), Vector3.zero, new(57, 5.2f, 1)));//8
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-114.1f, 990, -51.93f), new(-15, 0, 0), new(55, 5.2f, 1)));//9
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-141.58f, 991.29f, -47.11f), new(0, 90, -15), new(10, 10, 1)));//10
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-141.58f, 990.99f, -71.94f), new(0, 90, 0), new(4, 7, 1)));//11
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-108.01f, 985, -66.34f), new(0, 180, 0), new(63, 5.6f, 1)));//12
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-161.5f, 987.326f, -66.34f), new(0, 180, 0), new(55, 1, 1)));//13
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-108.9f, 987.799f, -68.32f), new(90, 0, 0), new(65, 4, 1)));//14
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-164.8f, 987.799f, -70), new(90, 0, 0), new(48, 7.4f, 1)));//15
            Model.AddPart(new ModelPrimitive(Model, prim, Color.black, new(-80.38f, 987.799f, -75), new(90, 0, 0), new(8, 10, 1)));//16
            Model.AddPart(new ModelPrimitive(Model, prim, Color.black, new(-181.07f, 987.799f, -78.07f), new(90, 0, 0), new(16, 10.4f, 1)));//17
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-76.505f, 985, -68.85f), new(0, 270, 0), new(5, 5.6f, 1)));//18
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-164.79f, 987.472f, -58.3f), new(90, 0, 0), new(49, 16.8f, 1)));//19
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-126, 984.84f, -58.9f), new(79.58f, 270, 0), new(15, 29, 1)));//20
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-69.05f, 985, -70.92f), new(0, 180, 0), new(15, 5.6f, 1)));//21
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-60.67f, 994.82f, -60.99f), new(0, 90, 0), new(17, 5.6f, 1)));//22
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-64.91f, 989.55f, -68.43f), new(0, 155, 0), new(12, 15, 1)));//23
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-76.667f, 990.27f, -72.76f), new(0, 90, 15), new(5, 5.5f, 1)));//24
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-71.62f, 989.97f, -70.25f), new(-15, 180, 0), new(10, 5.2f, 1)));//25
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-112.9f, 990.13f, -70), new(0, 180, 0), new(57.5f, 5, 1)));//27
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-84.03f, 990.13f, -71.916f), new(0, 270, 0), new(4, 5, 1)));//28
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-160.07f, 987.326f, -50.15f), Vector3.zero, new(37.5f, 1, 1)));//29
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-160.07f, 987.799f, -46.4f), new(90, 0, 0), new(37.5f, 7.4f, 1)));//30
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-183.92f, 987.472f, -28.5f), new(90, 0, 0), new(11, 47, 1)));//31
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-129, 987.472f, -11), new(90, 0, 0), new(100, 11, 1)));//32
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-58, 989.7f, -11), new(85, 90, 0), new(11, 60, 1)));//33
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-157.4f, 990.99f, -73.5365f), new(0, 180, 0), new(33, 7, 1)));//35
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-173.66f, 990.99f, -78.2f), new(0, 90, 0), new(10, 7, 1)));//36
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-173.34f, 993.66f, -30.42f), new(0, 90, 0), new(28, 13, 1)));//37
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-157.6f, 990.99f, -44.746f), Vector3.zero, new(32, 7, 1)));//38
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-175.07f, 987.799f, -33.25f), new(90, 90, 0), new(33.7f, 7.4f, 1)));//39
            Model.AddPart(new ModelPrimitive(Model, prim, new Color32(0, 0, 0, 1), new(-188.72f, 993.66f, -43.7f), new(0, 270, 0), new(78, 13, 1)));//40
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-97.45f, 991.5f, -7.03f), Vector3.zero, new(140, 8.2f, 1)));//41
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-97.45f, 991.5f, -14.725f), new(0, 180, 0), new(140, 8.2f, 1)));//42
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-97.45f, 996.3f, -13.95f), new(-45, 180, 0), new(140, 2.3f, 1)));//43
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-97.45f, 996.3f, -7.8f), new(-45, 0, 0), new(140, 2.3f, 1)));//44
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-52.9f, 997.11f, -11), new(270, 0, 0), new(50, 7, 1)));//45
            Model.AddPart(new ModelPrimitive(Model, prim, doroga, new(-162.43f, 997.11f, -10.84f), new(270, 0, 0), new(10, 4.7f, 1)));//46
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-167.41f, 997.528f, -6.222f), new(0, 90, -45), new(4, 4, 1)));//47
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-167.41f, 997.528f, -15.58f), new(0, 90, 45), new(4, 4, 1)));//48
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-169.34f, 993.66f, -16.45f), new(0, 180, 0), new(8, 13, 1)));//49
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-178.36f, 993.66f, -5.42f), Vector3.zero, new(22, 13, 1)));//50
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-167.41f, 993.66f, -15.74f), new(0, 90, 0), new(2, 13, 1)));//51
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-167.41f, 993.66f, -6.03f), new(0, 90, 0), new(2, 13, 1)));//52
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-167.41f, 998.6f, -10.97f), new(0, 90, 0), new(8, 3, 1)));//53
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-176.85f, 993.2f, -82.56f), new(0, 180, 0), new(6.6f, 11, 1)));//54
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-185.49f, 993.2f, -82.56f), new(0, 180, 0), new(6.6f, 11, 1)));//55
            Model.AddPart(new ModelPrimitive(Model, prim, col, new(-181.15f, 994.71f, -82.56f), new(0, 180, 0), new(3, 7, 1)));//56
            Model.AddPart(new ModelPrimitive(Model, prim, new Color32(0, 0, 0, 200), new(-117.655f, 997.11f, -10.84f), new(270, 0, 0), new(79.55f, 4.7f, 1)));//57
            Model.AddPart(new ModelPrimitive(Model, prim, new Color32(0, 0, 0, 200), new(-77.64f, 990.26f, -73.55f), new(0, 180, 0), new(2, 4.9f, 0.48f)));//58
            Model.AddPart(new ModelPrimitive(Model, prim, Textures.Load.WhiteColor, new(15.56f, 994.79f, -7.612f), new(0, 90), new(1.2f, 5, 1)));//59
            Model.AddPart(new ModelPrimitive(Model, prim, Textures.Load.WhiteColor, new(15.56f, 994.79f, -14.29f), new(0, 90), new(1.2f, 5, 1)));//60
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-188.74f, 998.87f, -13), new(0, 270), new(16, 4, 1)));//61
            Model.AddPart(new ModelPrimitive(Model, prim, stena_white, new(-188.93f, 1000.25f, -13), new(-37, 270), new(16, 4, 1)));//62
            try
            {
                new Lamp(new(-185.52f, 988.46f, -9.5f), Vector3.zero);//1
                new Lamp(new(-185.52f, 988.46f, -65.3f), Vector3.zero);//2
                new Lamp(new(-143.431f, 988.8f, -72.12f), Vector3.zero);//3
                new Lamp(new(-143.431f, 988.8f, -46.36f), Vector3.zero);//4
                new Lamp(new(-77.701f, 988.3f, -72.433f), Vector3.zero, 0.5f);//5
                new Lamp(new(-61.39f, 983.22f, -52.3f), Vector3.zero);//6
                new Lamp(new(-98.6f, 983.22f, -52.45f), Vector3.zero);//7
                new Lamp(new(-12.3f, 993.36f, -16.39f), Vector3.zero);//8
                new Lamp(new(13.56f, 993.36f, -5.34f), Vector3.zero);//9
            }
            catch { }
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, new Color32(0,0,0,1), new(-181.15f, 996.11f, -82.56f), new(0, 180), new(7, 10, 0.6f)));
            CreateStolb(new(-76, 983.22f, -53), new(0, -50));

            MEC.Timing.CallDelayed(1f, () => Model.Primitives.ForEach(prim => { try { prim.Primitive.Break(); } catch { } }));

            void CreateStolb(Vector3 pos, Vector3 rot)
            {
                var _stolb = new Model($"Stolb", pos, rot);
                Model.AddPart(new ModelPrimitive(_stolb, PrimitiveType.Cube, Color.gray, Vector3.zero, Vector3.zero, new(0.2f, 2, 0.2f)));
                Model.AddPart(new ModelPrimitive(_stolb, PrimitiveType.Cube, Color.gray, new(0, 1.23f), Vector3.zero, new(0.2f, 0.5f, 1.5f)));
                Model.AddPart(new ModelPrimitive(_stolb, PrimitiveType.Cube, Color.gray, new(0, 1.213f, 0.924f), Vector3.zero, new(0.2f, 0.2f, 0.5f)));
                Color _red = new(3, 0, 0);
                Model.AddPart(new ModelPrimitive(_stolb, PrimitiveType.Cube, _red, new(0, 1.05f, 0.9f), new(-20, 0), new(0.21f, 0.1f, 1)));
                Model.AddPart(new ModelPrimitive(_stolb, PrimitiveType.Cube, _red, new(0, 1.37f, 0.9f), new(20, 0), new(0.21f, 0.1f, 1)));
            }
        }
        internal static void AntiMachineDead(DamageEvent ev)
        {
            if (ev.DamageType == DamageTypes.Wall && ev.Target.Position.y < 1000 && ev.Target.Position.y > 970) ev.Allowed = false;
        }
        private static Vector3 DoorEntrance_Pos1 = new(-58, 987, -51);
        private static Vector3 DoorEntrance_Pos2 = new(-55, 991, -48);
        private static Vector3 DoorEntrance_Tp = new(-80, 984, -52);
        private static Vector3 DoorExit_Pos1 = new(-84, 982, -50);
        private static Vector3 DoorExit_Pos2 = new(-77, 990, -44);
        private static Vector3 DoorExit_Tp = new(-51, 989, -50);
        internal static void DoorTeleport()
        {
            foreach (var pl in Player.List)
            {
                if (pl.Position.y < 992 && pl.Position.y > 980 &&
                    pl.Position.x <= DoorEntrance_Pos2.x && pl.Position.x >= DoorEntrance_Pos1.x &&
                    pl.Position.y <= DoorEntrance_Pos2.y && pl.Position.y >= DoorEntrance_Pos1.y &&
                    pl.Position.z <= DoorEntrance_Pos2.z && pl.Position.z >= DoorEntrance_Pos1.z)
                    pl.Position = DoorEntrance_Tp;
                else if (pl.Position.y < 992 && pl.Position.y > 980 &&
                    pl.Position.x <= DoorExit_Pos2.x && pl.Position.x >= DoorExit_Pos1.x &&
                    pl.Position.y <= DoorExit_Pos2.y && pl.Position.y >= DoorExit_Pos1.y &&
                    pl.Position.z <= DoorExit_Pos2.z && pl.Position.z >= DoorExit_Pos1.z)
                    pl.Position = DoorExit_Tp;
            }
        }
    }
}