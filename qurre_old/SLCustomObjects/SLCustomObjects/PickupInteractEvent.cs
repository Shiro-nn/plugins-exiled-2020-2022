using System;
namespace SLCustomObjects.Args
{
	public class PickupInteractEvent : EventArgs
	{
		public string SchematicName;
		public Pickup Pickup;
		public string EventName;
	}
}