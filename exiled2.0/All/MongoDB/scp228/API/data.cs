using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.scp228.API
{
	public static class Scp228Data
	{
		public static Pickup Getvodka()
		{
			return EventHandlers228.vodka;
		}
		public static ReferenceHub GetScp228()
		{
			return EventHandlers228.scp228ruj;
		}
		public static string vodkalocationbc()
		{
			return EventHandlers228.vodka1;
		}
		public static string vodkalocation()
		{
			return EventHandlers228.vodka2;
		}
		public static string vodkalocationcolor()
		{
			return EventHandlers228.vodkacolor;
		}
		public static void Spawn228(ReferenceHub player)
		{
			EventHandlers228.SpawnJG(player);
		}
	}
}
