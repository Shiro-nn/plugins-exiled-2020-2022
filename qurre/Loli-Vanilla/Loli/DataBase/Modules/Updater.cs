using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;

namespace Loli.DataBase.Modules
{
	static class Updater
	{
		static bool _endSaving = false;

		[EventMethod(RoundEvents.Waiting)]
		static void Waiting() => _endSaving = false;

		[EventMethod(RoundEvents.End)]
		static void End()
		{
			if (Core.BlockStats)
				return;

			_endSaving = true;
			double cf = 1;
			try
			{
				int hour = DateTime.UtcNow.Hour + 3;
				if (hour >= 3 && hour < 7)
					cf = Core.PreMorningStatsCf;
				else if (hour >= 7 && hour < 13)
					cf = Core.MorningStatsCf;
				else if (hour >= 13 && hour < 17)
					cf = Core.DayStatsCf;
				else if (hour >= 17 && hour < 21)
					cf = Core.AverageCf;
				else if (hour >= 21 && hour < 24)
					cf = Core.PreNightCf;
				else if (hour >= 0 && hour < 2)
					cf = Core.NightCf;
			}
			catch { }
			foreach (var pl in Player.List) try
				{
					if (Data.Users.TryGetValue(pl.UserInformation.UserId, out var data) && data.found)
					{
						int played = (int)(DateTime.Now - data.entered).TotalSeconds;
						int total = (int)Math.Round(played * cf);
						Core.Socket.Emit("database.add.time", new object[] { data.id, 1, total });
					}
				}
				catch { }
		}

		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (Core.BlockStats)
				return;

			if (Data.Users.TryGetValue(ev.Player.UserInformation.UserId, out var data))
			{
				Data.Users.Remove(ev.Player.UserInformation.UserId);
				if (_endSaving) return;
				if (data.found)
				{
					double cf = 1;
					try
					{
						int hour = DateTime.UtcNow.Hour + 3;
						if (hour >= 3 && hour < 7)
							cf = Core.PreMorningStatsCf;
						else if (hour >= 7 && hour < 13)
							cf = Core.MorningStatsCf;
						else if (hour >= 13 && hour < 17)
							cf = Core.DayStatsCf;
						else if (hour >= 17 && hour < 21)
							cf = Core.AverageCf;
						else if (hour >= 21 && hour < 24)
							cf = Core.PreNightCf;
						else if (hour >= 0 && hour < 2)
							cf = Core.NightCf;
					}
					catch { }
					int played = (int)(DateTime.Now - data.entered).TotalSeconds;
					int total = (int)Math.Round(played * cf);
					Core.Socket.Emit("database.add.time", new object[] { data.id, 1, total });
				}
			}
		}
	}
}