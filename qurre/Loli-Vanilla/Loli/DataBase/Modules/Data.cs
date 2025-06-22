using Qurre.API.Attributes;
using Qurre.Events;
using System;
using System.Collections.Generic;

namespace Loli.DataBase.Modules
{
	static class Data
	{
		static internal readonly CustomDictionary<string, UserData> Users = new();
		static internal readonly Dictionary<string, DonateRoles> Roles = new();

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting()
		{
			try { Users.Clear(); } catch { }
		}
	}
}