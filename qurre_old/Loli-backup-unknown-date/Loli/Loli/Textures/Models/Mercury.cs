using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.Textures.Models
{
    internal class Mercury
    {
        internal static readonly List<Mercury> MercuryList = new();
        private static readonly Color32 Chulki = new(86, 74, 84, 255);
        private static readonly Color32 Botinok = new(145, 47, 0, 255);
        private static readonly Color32 Kozha = new(251, 218, 210, 255);
        private static readonly Color32 Yubka = new(165, 45, 46, 255);
        private static readonly Color32 Odezhda = new(45, 41, 56, 255);
        private static readonly Color32 Volosi = new(75, 74, 90, 255);
        private static readonly Color32 Eyes = new(244, 149, 177, 255);
        private static readonly Vector3 LocPos = new(-1.225f, 1.2f, -0.145f);
        private readonly FlyScript flyScript;
        internal Mercury(Player pl)
        {
            Player = pl;
            Model = new("Mercury", Vector3.zero, new(0,170));
            flyScript = Model.GameObject.AddComponent<FlyScript>();
            try { flyScript.Root = pl.Transform; } catch { }
            MercuryList.Add(this);
            {
                var Noga = new Model("Noga", new(-0.029f, 0, -0.189f), Vector3.zero, Model);
                {
                    var Systav = new Model("NizhniySystav", new(0.124f, -0.172f, 0.004f), new(0, 0, 35), Noga);
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(0.01f, 0.096f, 0.0925f), Vector3.zero, new(0.1f, 0.1f, 0.1f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(0.008f, 0.187f, 0.093f), new(0, 0, 3.91f), new(0.1f, 0.1f, 0.1f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Botinok, new(-0.04f, -0.128f, 0.0925f), new(0, 0, 14.08f), new(0.235f, 0.045f, 0.125f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Cylinder, Botinok, new(0.01f, -0.03f, 0.093f), Vector3.zero, new(0.115f, 0.075f, 0.115f), false));
                }
                {
                    var Systav = new Model("VerhnyiSystav", new(0.021f, 0.15f, 0.011f), new(0, -2.5f, -50), Noga);
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(0.0625f, -0.042f, 0.0887f), Vector3.zero, new(0.1f, 0.1f, 0.1f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(0.0625f, 0.063f, 0.088f), Vector3.zero, new(0.1025f, 0.015f, 0.1025f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Kozha, new(0.0625f, 0.095f, 0.0887f), Vector3.zero, new(0.1f, 0.125f, 0.1f), false));
                }
            }
            {
                var Noga = new Model("Noga", new(0.099f, -0.091f, 0.051f), Vector3.zero, Model);
                {
                    var Systav = new Model("NizhniySystav", new(0.027f, -0.232f, -0.014f), new(-5.31f, 0, 8.18f), Noga);
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(-0.039f, 0.168f, -0.0035f), Vector3.zero, new(0.1f, 0.1f, 0.1f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(-0.042f, 0.26f, -0.0035f), new(0, 0, 3.91f), new(0.1f, 0.1f, 0.1f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Botinok, new(-0.093f, -0.056f, -0.0035f), new(0, 0, 14.08f), new(0.235f, 0.045f, 0.125f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Cylinder, Botinok, new(-0.04f, 0.043f, -0.0035f), Vector3.zero, new(0.115f, 0.075f, 0.115f), false));
                }
                {
                    var Systav = new Model("VerhnyiSystav", new(0.021f, 0.15f, -0.015f), new(0, -30, -30), Noga);
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(-0.038f, -0.056f, 0.015f), Vector3.zero, new(0.1f, 0.1f, 0.1f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Chulki, new(-0.038f, 0.049f, 0.015f), Vector3.zero, new(0.1025f, 0.015f, 0.1025f), false));
                    Model.AddPart(new ModelPrimitive(Systav, PrimitiveType.Capsule, Kozha, new(-0.038f, 0.08f, 0.0145f), Vector3.zero, new(0.1f, 0.125f, 0.1f), false));
                }
            }
            {
                var Telo = new Model("Telo", new(0, 0.504f), Vector3.zero, Model);
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.169f, -0.252f, -0.004f), Vector3.zero, new(0.125f, 0.065f, 0.25f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.047f, -0.238f, -0.004f), new(0, 0, -30.68f), new(0.025f, 0.05f, 0.275f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.117f, -0.243f, 0.157f), new(0, 72, -30.68f), new(0.025f, 0.05f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.117f, -0.243f, -0.157f), new(0, -72, -30.68f), new(0.025f, 0.05f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.25f, -0.238f, -0.004f), new(0, 0, 30.68f), new(0.025f, 0.05f, 0.275f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.208f, -0.243f, -0.15f), new(0, -115.3f, -30.68f), new(0.025f, 0.05f, 0.1f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.208f, -0.243f, 0.15f), new(0, 115.3f, -30.68f), new(0.025f, 0.05f, 0.1f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.063f, -0.161f, -0.004f), new(0, 0, -30.68f), new(0.025f, 0.05f, 0.275f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.122f, -0.161f, 0.143f), new(0, 72, -30.68f), new(0.025f, 0.05f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.122f, -0.161f, -0.143f), new(0, -72, -30.68f), new(0.025f, 0.05f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.235f, -0.161f, -0.004f), new(0, 0, 30.68f), new(0.025f, 0.05f, 0.275f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.201f, -0.161f, -0.137f), new(0, -115.3f, -30.68f), new(0.025f, 0.05f, 0.1f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Yubka, new(0.201f, -0.161f, 0.137f), new(0, 115.3f, -30.68f), new(0.025f, 0.05f, 0.1f), false));

                Vector3 COS1 = new(0.005f, 0.075f, 0.035f);
                Vector3 COS2 = new(0.005f, 0.075f, 0.125f);
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.133f, -0.09f, 0.109f), new(0, 72, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.109f, -0.09f, 0.099f), new(0, 52.6f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.089f, -0.092f, 0.075f), new(0, 27.6f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.0775f, -0.092f), new(0, 0, -15f), COS2, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.089f, -0.092f, -0.075f), new(0, -27.6f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.109f, -0.09f, -0.099f), new(0, -52.6f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.133f, -0.09f, -0.109f), new(0, -72, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.1625f, -0.09f, 0.1115f), new(0, 101.5f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.191f, -0.09f, 0.1004f), new(0, 124.6f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.21f, -0.09f, 0.0775f), new(0, 166.3f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.21f, -0.092f), new(0, 0, 15f), COS2, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.1625f, -0.09f, -0.1115f), new(0, -101.5f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.191f, -0.09f, -0.1004f), new(0, -124.6f, -15), COS1, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.21f, -0.09f, -0.0775f), new(0, -166.3f, -15), COS1, false));

                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Odezhda, new(0.146f, -0.0025f, -0.004f), Vector3.zero, new(0.135f, 0.1f, 0.21f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Sphere, Odezhda, new(0.097f, 0.032f, 0.025f), new(0, 0, 90), new(0.075f, 0.075f, 0.075f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Sphere, Odezhda, new(0.097f, 0.032f, -0.025f), new(0, 0, 90), new(0.075f, 0.075f, 0.075f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.0873f, 0.051f), new(0, 0, 90), new(0.025f, 0.01f, 0.05f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Odezhda, new(0.146f, 0.065f, -0.004f), Vector3.zero, new(0.11f, 0.05f, 0.175f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.151f, 0.118f), Vector3.zero, new(0.07f, 0.05f, 0.07f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Sphere, Kozha, new(0.153f, 0.201f), new(0, 0, 90), new(0.15f, 0.15f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Sphere, Volosi, new(0.153f, 0.25f), new(0, 0, 90), new(0.15f, 0.15f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Kozha, new(0.153f, 0.223f), new(0, 90), new(0.15f, 0.0175f, 0.15f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Color.white, new(0.083f, 0.223f, -0.0175f), new(0, 0, 90), new(0.03f, 0.005f, 0.025f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Color.white, new(0.083f, 0.223f, 0.0175f), new(0, 0, 90), new(0.03f, 0.005f, 0.025f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Eyes, new(0.0829f, 0.223f, 0.0176f), new(8, 0, 90), new(0.02f, 0.005f, 0.015f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Eyes, new(0.0829f, 0.223f, -0.0176f), new(-8, 0, 90), new(0.02f, 0.005f, 0.015f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Odezhda, new(0.0828f, 0.223f, 0.0176f), new(8, 0, 90), new(0.01f, 0.005f, 0.0065f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Odezhda, new(0.0828f, 0.223f, -0.0176f), new(-8, 0, 90), new(0.01f, 0.005f, 0.0065f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Kozha, new(0.0955f, 0.223f), new(0, 90), new(0.085f, 0.0175f, 0.035f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.08f, 0.2354f, -0.0184f), new(90, 0, 90), new(0.02f, 0.005f, 0.004f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.08f, 0.2354f, 0.0184f), new(90, 0, 90), new(0.02f, 0.005f, 0.004f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.08f, 0.2317f, -0.029f), new(-140, 0, 90), new(0.01f, 0.005f, 0.0065f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cube, Odezhda, new(0.08f, 0.2317f, 0.029f), new(140, 0, 90), new(0.01f, 0.005f, 0.0065f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Eyes, new(0.083f, 0.176f, 0.005f), new(78, 0, 90), new(0.015f, 0.005f, 0.005f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Eyes, new(0.083f, 0.176f, -0.005f), new(-78, 0, 90), new(0.015f, 0.005f, 0.005f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Cylinder, Eyes, new(0.083f, 0.1964f, 0.00135f), new(0, 0, 90), new(0.015f, 0.005f, 0.005f), false));

                Vector3 VS = new(0.025f, 0.065f, 0.02f);
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.1165f, 0.218f, 0.059f), new(0, -26.09f), new(0.05f, 0.065f, 0.02f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.1410065f, 0.218f, 0.06624985f), new(0, -13), new(0.05f, 0.065f, 0.02f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.1814575f, 0.218f, 0.06499863f), new(0, 16), new(0.05f, 0.065f, 0.02f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.1165f, 0.218f, -0.059f), new(0, 26.09f), new(0.05f, 0.065f, 0.02f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.1410065f, 0.218f, -0.06624985f), new(0, 13), new(0.05f, 0.065f, 0.02f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.1814575f, 0.218f, -0.06499863f), new(0, -16), new(0.05f, 0.065f, 0.02f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2189941f, 0.2179849f, 0.05049896f), new(0, 55.12f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2235107f, 0.136564f, 0.05364609f), new(-7.44f, 55.12f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2324524f, 0.05636376f, 0.0598793f), new(-9.2f, 55.12f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2243958f, 0.2179849f, 0.02809906f), new(0, 83.2f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2298584f, 0.136564f, 0.02875137f), new(-7.44f, 83.2f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2406921f, 0.05636376f, 0.03004074f), new(-9.2f, 83.2f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2271576f, 0.2179849f, 0.005050659f), new(0, 88.9f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2326508f, 0.136564f, 0.005157471f), new(-7.44f, 88.9f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2435455f, 0.05636376f, 0.005363464f), new(-9.2f, 88.9f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.227005f, 0.2179849f, -0.01950073f), new(0, 106), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2322845f, 0.136564f, -0.02101517f), new(-7.44f, 106), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2427673f, 0.05636376f, -0.02402115f), new(-9.2f, 106), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2189941f, 0.2179849f, -0.04150009f), new(0, 118.9f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2238159f, 0.136564f, -0.04415894f), new(-7.44f, 118.9f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2333527f, 0.05636376f, -0.04942703f), new(-9.2f, 118.9f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.210495f, 0.2179849f, -0.0605011f), new(0, 145.4f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.213623f, 0.136564f, -0.06502914f), new(-7.44f, 145.4f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2198181f, 0.05636376f, -0.07400131f), new(-9.2f, 145.4f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.210495f, 0.2179849f, 0.0605011f), new(0, 29.2f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2131805f, 0.136564f, 0.0653f), new(-7.44f, 29.2f), VS, false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Volosi, new(0.2185059f, 0.05636376f, 0.07481766f), new(-9.2f, 29.2f), VS, false));

                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Chulki, new(0.151001f, 0.07150048f, -0.1080017f), new(25.17f, 23.5f, -18.82f), new(0.065f, 0.05f, 0.065f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Chulki, new(0.1295471f, 0.03121728f, -0.1193047f), new(25.17f, 23.5f, -18.82f), new(0.05f, 0.025f, 0.05f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.113739f, 0.001554191f, -0.1276398f), new(25.17f, 23.5f, -18.82f), new(0.045f, 0.045f, 0.045f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.100296f, -0.003206551f, -0.1242447f), new(25.17f, 23.5f, -152.42f), new(0.045f, 0.035f, 0.045f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Chulki, new(0.07989502f, 0.05648583f, -0.08478928f), new(25.17f, 23.5f, -152.42f), new(0.045f, 0.075f, 0.045f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Chulki, new(0.151001f, 0.07150048f, 0.1080017f), new(-27.1f, 28f, 22f), new(0.065f, 0.05f, 0.065f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Chulki, new(0.1758728f, 0.03268212f, 0.1172638f), new(-27.1f, 28f, 22f), new(0.05f, 0.025f, 0.05f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.1941986f, 0.004117668f, 0.1240921f), new(-27.1f, 28, 22), new(0.045f, 0.045f, 0.045f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Kozha, new(0.2204895f, -0.03555518f, 0.1346664f), new(-30.982f, 26.166f, 22.891f), new(0.045f, 0.035f, 0.045f), false));
                Model.AddPart(new ModelPrimitive(Telo, PrimitiveType.Capsule, Chulki, new(0.2620239f, -0.094271f, 0.1535721f), new(-30.982f, 26.166f, 22.891f), new(0.045f, 0.075f, 0.045f), false));
            }
        }
        private class FlyScript : MonoBehaviour
        {
            internal Transform Root;
            public const float repeat_time = 0.1f;
            private float curr_time;
            private Vector3 FlyVec = new();
            bool IsUp => par >= 25;
            int par = 0;
            bool down = false;
            void Start()
            {
                curr_time = repeat_time;
            }
            void Update()
            {
                curr_time -= Time.deltaTime;
                if (curr_time > 0) return;
                curr_time = repeat_time;
                if (!down)
                {
                    if (par < 50) par++;
                    else
                    {
                        down = true;
                        par--;
                    }
                }
                else if (down)
                {
                    if (par > 0) par--;
                    else
                    {
                        down = false;
                        par--;
                    }
                }
                FlyVec += (IsUp ? Vector3.up : Vector3.down) * 0.01f;
                try
                {
                    if (Root is null)
                    {
                        transform.position += (IsUp ? Vector3.up : Vector3.down) * 0.01f;
                        return;
                    }
                    transform.position = Root.position + LocPos + FlyVec;
                }
                catch { }
            }
        }
        internal readonly Player Player;
        internal readonly Model Model;
        internal void Destroy()
        {
            try { MercuryList.Remove(this); } catch { }
            try { Model.Destroy(); } catch { }
        }
        internal static void Update(EffectEnabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.flyScript.Root = null; } catch { }
        }
        internal static void Update(EffectDisabledEvent ev)
        {
            if (ev.Type != EffectType.Invisible) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.flyScript.Root = ev.Player.Transform; } catch { }
        }
        internal static void Leave(LeaveEvent ev)
        {
            if (!TryGet(ev.Player, out var obj)) return;
            MercuryList.Remove(obj);
            try { obj.Model.Destroy(); } catch { }
        }
        internal static void RoleChange(RoleChangeEvent ev)
        {
            if (ev.NewRole != RoleType.Spectator) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.flyScript.Root = null; } catch { }
        }
        internal static void RoleChange(SpawnEvent ev)
        {
            if (ev.RoleType == RoleType.Spectator || ev.RoleType == RoleType.None) return;
            if (!TryGet(ev.Player, out var obj)) return;
            try { obj.flyScript.Root = ev.Player.Transform; } catch { }
        }
        internal static void Waiting() => MercuryList.Clear();
        private static bool TryGet(Player pl, out Mercury nimb)
        {
            var list = MercuryList.ToArray();
            foreach (var item in list)
                if (item.Player == pl)
                {
                    nimb = item;
                    return true;
                }
            nimb = default;
            return false;
        }
    }
}