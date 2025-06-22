using UnityEngine;
namespace SLCustomObjects
{
	public static class Extensions
	{
		public static Vector3 GetJsonVector(this JsonVector3 vc)
		{
			return new Vector3
			{
				x = vc.x,
				y = vc.y,
				z = vc.z
			};
		}
	}
	public class CustomSchematicSpawn
	{
		public string SpecialID { get; set; } = "Schem0";
		public JsonVector3 Position { get; set; } = new JsonVector3() { x = 0f, y = 0f, z = 0f };
		public JsonVector3 Rotation { get; set; } = new JsonVector3() { x = 0f, y = 0f, z = 0f };
	}
}