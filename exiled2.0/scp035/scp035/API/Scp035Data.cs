namespace scp035.API
{
	public static class Scp035Data
	{
		public static ReferenceHub GetScp035()
		{
			try
			{
				return EventHandlers.scpPlayer;
			}
			catch
			{
				return null;
			}
		}
		public static void Spawn035(ReferenceHub player)
		{
			EventHandlers.Spawn035(player);
		}
	}
}
