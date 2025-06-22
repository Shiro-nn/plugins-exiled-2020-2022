using Qurre.API;
using SchematicUnity.API.Objects;
using System.IO;
using UnityEngine;

namespace Loli.Builds.Models.Rooms
{
    static class RadarLoc
    {
        static internal void Load()
        {
            var Scheme = SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Pathes.Plugins, "Schemes", "RadarLoc.json"), Vector3.zero, default);

            new Lamp(new(32.043f, 1001.955f, -47.62f), Vector3.zero);
            new Radar(new(24.87f, 1004.12f, -41.46f), Vector3.zero, null, new(0.6f, 0.6f, 0.6f));

            foreach (var _obj in Scheme.Objects)
                findObjects(_obj);

            static void findObjects(SObject obj)
            {
                if (obj is null)
                    return;

                switch (obj.Name)
                {
                    case "RedColor":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color(10, 0, 0);
                            }
                            break;
                        }
                }

                if (obj.Childrens is null)
                    return;

                foreach (var _obj in obj.Childrens)
                    findObjects(_obj);
            }
        }
    }
}