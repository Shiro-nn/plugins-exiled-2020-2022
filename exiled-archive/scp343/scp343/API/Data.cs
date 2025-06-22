namespace scp343.API
{
	public static class scp343Data
	{
		public static ReferenceHub GetScp343()
		{
			return EventHandlers.scp343;
		}

		public static void Spawn343(ReferenceHub player)
		{
			EventHandlers.Spawn343(player);
		}
	}
}
