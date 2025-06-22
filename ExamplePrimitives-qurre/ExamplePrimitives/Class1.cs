using Qurre.API.Controllers;
using System.Threading;
using UnityEngine;
namespace ExamplePrimitives
{
    public class SomePrimitiveController
    {
        internal readonly Primitive Primitive;
        private Vector3 _startPosition;
        public SomePrimitiveController(Transform parent, PrimitiveType type, Vector3 position, Color color = default, Quaternion rotation = default, Vector3 size = default)
        {
            var prim = new Primitive(type, position, color, rotation, size);
            Primitive = prim;
            prim.Base.gameObject.transform.parent = parent;//parent needed for <unity documentation> ; not use if not needed
            prim.Base.gameObject.transform.localPosition = position;
            prim.Base.gameObject.transform.localRotation = rotation;
            _startPosition = position;
        }
        public void OpenAsDoor()
        {
            new Thread(() =>//or use coroutines
            {
                for (int i = 0; i < 10; i++)
                {
                    Primitive.Position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z + i * 0.5f);
                    Thread.Sleep(100);
                }
            }).Start();
        }
    }
}