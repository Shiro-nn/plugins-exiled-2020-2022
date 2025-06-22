using Qurre.API.Addons.Models;
using UnityEngine;
namespace Loli.Textures.Models
{
    public class Turret
    {
        public Turret(Vector3 position, Vector3 rotation, Color bodyColor = default, float scale = default)
        {
            bodyColor = bodyColor == default ? Color.red : bodyColor;
            scale = scale is 0 ? 1.25f : scale;

            Model = new Model("Turret", position, rotation);

            // Head
            Model_Head = new Model("Head", Vector3.zero, Vector3.zero, Model);

            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, Color.black, new Vector3(0, 1.108887f, -0.187f), Vector3.zero, scale * new Vector3(0.3f, 0.005f, 0.3f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, bodyColor, new Vector3(0, 1.07998f, -0.187f), Vector3.zero, scale * new Vector3(0.425f, 0.025f, 0.425f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, bodyColor, new Vector3(0, 1.05f, -0.187f), Vector3.zero, scale * new Vector3(0.4f, 0.007f, 0.4f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, bodyColor, new Vector3(0, 0.9709961f, -0.187f), Vector3.zero, scale * new Vector3(0.425f, 0.075f, 0.425f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, bodyColor, new Vector3(0, 0.8910156f, -0.187f), Vector3.zero, scale * new Vector3(0.4f, 0.007f, 0.4f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, bodyColor, new Vector3(0, 0.8600098f, -0.187f), Vector3.zero, scale * new Vector3(0.425f, 0.025f, 0.425f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cube, bodyColor, new Vector3(0, 0.9709961f, -0.0387497f), Vector3.zero, scale * new Vector3(0.35f, 0.26f, 0.2975f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cube, new Color32(80, 80, 80, 255), new Vector3(0, 0.9709961f, 0.2587511f), Vector3.zero, scale * new Vector3(0.35f, 0.26f, 0.2975f)));
            var lamp = new ModelPrimitive(Model_Head, PrimitiveType.Sphere, bodyColor, new Vector3(0.09999996f, 0.9339844f, 0.4100001f), Vector3.zero, scale * new Vector3(0.03f, 0.03f, 0.03f));
            Model_Head.AddPart(lamp);
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, Color.black, new Vector3(-0.05000005f, 0.95f, 0.4750002f), Vector3.right * 90, scale * new Vector3(0.125f, 0.1f, 0.125f)));
            Model_Head.AddPart(new ModelPrimitive(Model_Head, PrimitiveType.Cylinder, Color.black, new Vector3(0.07250057f, 0.8915039f, 0.4133002f), Vector3.right * 90, scale * new Vector3(0.02999999f, 0.02f, 0.03f)));

            var _pos = lamp.GameObject.transform.position;
            Model_Head.AddPart(new ModelLight(Model_Head, bodyColor, new Vector3(_pos.x + 2.384186e-05f, _pos.y, _pos.z + 1.103372f), 1, scale, false));


            // Body
            Model_Body = new Model("Body", Vector3.zero, Vector3.zero, Model);

            Model_Body.AddPart(new ModelPrimitive(Model_Body, PrimitiveType.Cylinder, Color.black, new Vector3(2.384186e-08f, 0.585f, -0.02000003f), Vector3.zero, scale * new Vector3(0.2f, 0.4f, 0.2f)));
            Model_Body.AddPart(new ModelPrimitive(Model_Body, PrimitiveType.Cube, Color.black, new Vector3(2.384186e-08f, 0.17f, -0.425f), Vector3.left * 30, scale * new Vector3(0.35f, 0.075f, 1)));
            Model_Body.AddPart(new ModelPrimitive(Model_Body, PrimitiveType.Cube, Color.black, new Vector3(0.333f, 0.095f, 0.3339999f), new Vector3(20, 40, 0), scale * new Vector3(0.1f, 0.075f, 1)));
            Model_Body.AddPart(new ModelPrimitive(Model_Body, PrimitiveType.Cube, Color.black, new Vector3(-0.304f, 0.112f, 0.306f), new Vector3(20, 320, 0), scale * new Vector3(0.1f, 0.075f, 1)));

            Model.GameObject.transform.localScale = Vector3.one * scale;
            Model_Head.GameObject.AddComponent<TurretScript>();
        }

        public Model Model;
        public Model Model_Head;
        public Model Model_Body;
    }
    public class TurretScript : MonoBehaviour
    {
        bool one;
        private float startRotationY;

        void Start()
        {
            startRotationY = transform.rotation.eulerAngles.y;
        }

        void Update()
        {
            Vector3 direction = (one ? Vector3.down : Vector3.up) * 0.5f;
            transform.Rotate(direction);

            if (Difference(startRotationY, transform.rotation.eulerAngles.y) >= 60) one = !one;
        }

        public static float Difference(float first, float second)
        {
            if (first > second) return first - second;
            else return second - first;
        }
    }
}