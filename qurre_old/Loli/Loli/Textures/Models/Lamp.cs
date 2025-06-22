using Qurre.API.Addons.Models;
using UnityEngine;
using Light = Qurre.API.Controllers.Light;
namespace Loli.Textures.Models
{
    public class Lamp
    {
        public Lamp(Vector3 position, Vector3 rotation, float scale = 1)
        {
            Model = new CustomRoom("Lamp", position, rotation);

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.black, new Vector3(0, 3.52f, 0), Vector3.zero, scale * new Vector3(0.65f, 0.1f, 0.65f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.black, new Vector3(0, 3.02f, 0), Vector3.zero, scale * new Vector3(0.5f, 0.5f, 0.5f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.black, new Vector3(0, 1.02f, 0), Vector3.zero, scale * new Vector3(0.33f, 2, 0.33f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.black, new Vector3(0, -0.99f, 0), Vector3.zero, scale * new Vector3(1, 0.01f, 1)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.black, new Vector3(-0.485f, 3.725f, 0), Vector3.forward * 45, scale * new Vector3(0.05f, 1, 0.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.black, new Vector3(0.265f, 3.725f, -0.25f), new Vector3(0, 45, 315), scale * new Vector3(0.05f, 1, 0.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Color.black, new Vector3(0.265f, 3.725f, 0.25f), new Vector3(0, 315, 315), scale * new Vector3(0.05f, 1, 0.2f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Color.black, Vector3.up * 4.137f, Vector3.zero, scale * new Vector3(1.75f, 0.075f, 1.75f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, new Color(50, 50, 50), Vector3.up * 4.121f, Vector3.zero, scale * new Vector3(1.5f, 0.075f, 1.5f)));

            Model.AddPart(new ModelLight(Model, Color.white, Vector3.up * 0.221f, 1, 20, false));

            MEC.Timing.CallDelayed(1f, () => Model.Primitives.ForEach(prim => { try { prim.Primitive.Break(); } catch { } }));
        }

        public CustomRoom Model;
        public Light Light;
    }
}