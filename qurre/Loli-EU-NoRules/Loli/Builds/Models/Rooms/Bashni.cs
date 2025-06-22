using MEC;
using PlayerRoles;
using Qurre.API;
using SchematicUnity.API.Objects;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Loli.Builds.Models.Rooms
{
    internal static class Bashni
    {
        internal static Vector3 ChaosSpawnPoint = Respawn.GetPosition(RoleTypeId.ChaosConscript);
        internal static Vector3 MTFSpawnPoint = Respawn.GetPosition(RoleTypeId.NtfCaptain);

        internal static void Load()
        {
            GameObject root = new();
            root.transform.position = new(0.2f, 1000f, -51.8f);

            new Lift(new(root.transform, new(-12.22f, 1.8125f, 0.087f), new(0, 90)),
                new(root.transform, new(-12.22f, 19.88f, 0.087f), new(0, 90)), Color.white);

            new Lift(new(null, new(-128.359f, 992.91f, -27.84f), new(0, 180), true),
                new(root.transform, new(45.077f, 19.898f, 6.727f), new(0, 270)), new Color32(59, 60, 62, 255));

            var Scheme = SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Pathes.Plugins, "Schemes", "Bashni.json"),
                root.transform.position, root.transform.rotation);
            foreach (var _obj in Scheme.Objects)
                findObjects(_obj);
            static void findObjects(SObject obj)
            {
                switch (obj.Name)
                {
                    case "Steklo":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color32(0, 133, 155, 150);
                            }
                            break;
                        }
                    case "Potolok":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                Timing.RunCoroutine(NeonLight(prm));
                            }
                            break;
                        }
                }
                foreach (var _obj in obj.Childrens)
                    findObjects(_obj);
            }
        }
        private static IEnumerator<float> NeonLight(PrimitiveParams prim)
        {
            bool plus = false;
            Color color = new(0, 0, 6);
            for (; ; )
            {
                if (plus) color.b += 0.2f;
                else color.b -= 0.2f;
                if (color.b < 1) plus = true;
                else if (color.b > 6) plus = false;
                prim.Color = color;
                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }
}