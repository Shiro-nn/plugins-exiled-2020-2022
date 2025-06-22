using Qurre.API.Addons.Models;
using UnityEngine;
namespace Loli.Textures.Models
{
    public class Pizza
    {
        private readonly static Color32 Testo = new(255, 215, 161, 255);
        private readonly static Color32 Sier = new(231, 178, 60, 255);
        private readonly static Color32 Peperoni = new(219, 0, 0, 255);
        public Pizza(Vector3 position, Vector3 rotation, float scale = 1)
        {
            Model = new Model("Pizza", position, rotation);

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Testo, Vector3.zero, Vector3.zero, scale * new Vector3(0.8f, 0.025f, 0.8f)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Sier, Vector3.up * 0.025f, Vector3.zero, scale * new Vector3(0.77f, 0.001f, 0.77f)));

            SpawnPeperonis(scale);

            Model.GameObject.transform.localScale = Vector3.one * scale;
        }

        private void SpawnPeperonis(float scale)
        {
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.307f, 0.02728271f, 0), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.247f, 0.02728271f, 0.152f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.119f, 0.02728271f, 0.267f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.03299999f, 0.0272f, 0.299f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.178f, 0.02728271f, 0.2460001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.2869999f, 0.02728271f, 0.1340001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.309f, 0.02728271f, -0.01199996f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.265f, 0.02728271f, -0.152f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.152f, 0.02728271f, -0.265f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.008000016f, 0.02728271f, -0.3150001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.1570001f, 0.02728271f, -0.265f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.271f, 0.02728271f, -0.1570001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.06900001f, 0.02728271f, -0.1570001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.001000047f, 0.02728271f, 0.156f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(-0.146f, 0.02728271f, 0.01600003f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.143f, 0.02728271f, 0.06200004f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.107f, 0.02728271f, -0.1110001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cylinder, Peperoni, new Vector3(0.01699996f, 0.02728271f, -0.01800001f), Vector3.zero, scale * new Vector3(0.1f, 0.001f, 0.1f)));
        }

        public Model Model { get; private set; }
    }
}