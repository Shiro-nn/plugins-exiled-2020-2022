namespace PlayerXP.scp343.API
{
    public static class scp343Data
	{
		public static ReferenceHub GetScp343()
		{
			return EventHandlers343.scp343;
		}

		public static void Spawn343(ReferenceHub player)
		{
			EventHandlers343.Spawn343(player);
		}
	}
}
