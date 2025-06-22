using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace ExamplePrimitives
{
    internal static class Spawner
    {
        internal static void Initialize()
        {
            var parent = new ParentOnSomePrimitive(new Vector3(-1.15f, -3.9258f, 6.75f), Vector3.zero);
            parent.SpawnChild();//spawn new child

        }
    }
}