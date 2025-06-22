namespace scp228ruj.API
{
	public static class Scp228Data
	{
		public static Pickup Getvodka()
		{
			return EventHandlers.vodka;
		}
		public static ReferenceHub GetScp228()
		{
			return EventHandlers.scp228ruj;
		}
		public static string vodkalocationbc()
		{
			return EventHandlers.vodka1;
		}
		public static string vodkalocation()
		{
			return EventHandlers.vodka2;
		}
		public static string vodkalocationcolor()
		{
			return EventHandlers.vodkacolor;
		}
		public static void Spawn228(ReferenceHub player)
		{
			EventHandlers.SpawnJG(player);
		}
	}
}
