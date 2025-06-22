using System.Collections.Generic;
using UnityEngine;
namespace ExamplePrimitives
{
    internal class ParentOnSomePrimitive
    {
        private readonly GameObject InvisibleObject;
        private readonly List<SomePrimitiveController> Childrens = new List<SomePrimitiveController>();
        internal ParentOnSomePrimitive(Vector3 pos, Vector3 rot)
        {
            InvisibleObject = new GameObject("Parent");
            InvisibleObject.transform.position = pos;
            InvisibleObject.transform.rotation = Quaternion.Euler(rot);
            //children's primitives position == parent.position + localPosition ; Rotation & Scale like as position;
            SpawnChild(PrimitiveType.Cube, new Vector3(-1, 900, -10)/* == 0, 1000, 0 */, Vector3.zero, new Color32(0, 0, 0, 155)/* 155 - window effect */);
            SpawnChild(PrimitiveType.Cube, new Vector3(-1, 900, 0)/* == 0, 1000, 10 */, Vector3.zero, new Color32(0, 0, 0, 155)/* 155 - window effect */);
        }
        internal void SpawnChild(PrimitiveType type, Vector3 position, Vector3 rotation, Color color)
        {
            var child = new SomePrimitiveController(InvisibleObject.transform, type, position, color, Quaternion.Euler(rotation));
            Childrens.Add(child);
        }
        internal void OpenAllChildrenDoors()
        {
            foreach (var children in Childrens)
            {
                try { children.OpenAsDoor(); } catch { }
            }
        }
        internal void ChangeColors(Color32 color)
        {
            foreach (var children in Childrens)
            {
                try { children.Primitive.Color = color; } catch { }
            }
        }
    }
}