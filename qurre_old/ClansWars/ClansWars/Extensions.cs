using Qurre.API.Addons.Models;
using UnityEngine;
namespace ClansWars
{
	internal static class Extensions
	{
	}
	internal class FixPrimitiveSmoothing : MonoBehaviour
	{
		internal Model Model;
		internal void Update()
		{
			if (Model is null) return;
			for (int i = 0; i < Model.Primitives.Count; i++)
			{
				var prim = Model.Primitives[i];
				prim.Primitive.Base.NetworkMovementSmoothing = prim.Primitive.MovementSmoothing;
				prim.Primitive.Base.NetworkRotation = new LowPrecisionQuaternion(prim.GameObject.transform.rotation);
				prim.Primitive.Base.NetworkPosition = prim.GameObject.transform.position;
			}
		}
	}
}