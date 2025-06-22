namespace scp682343.API
{
	public static class scp682Data
	{
		public static ReferenceHub GetScp682()
		{
			return EventHandlers.scp682;
		}

		public static void Spawn682(ReferenceHub player)
		{
			EventHandlers.SpawnJG(player);
		}
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
