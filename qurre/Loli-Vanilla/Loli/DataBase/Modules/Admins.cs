using MEC;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;

namespace Loli.DataBase.Modules
{
	static class Admins
	{
		static readonly HashSet<string> UserIds = new();

		static internal void Call()
		{
			Core.Socket.On("SCPServerInit", _ =>
			{
				Core.Socket.Emit("database.get.adm.steams", new object[] { });
			});

			Core.Socket.On("database.get.adm.steams", data =>
			{
				string[] userIds = JsonConvert.DeserializeObject<string[]>(data[0].ToString());

				UserIds.Clear();

				foreach (string userId in userIds)
				{
					UserIds.Add(userId + "@steam");
				}

			});

			Timing.RunCoroutine(UpdateInterval());
			static IEnumerator<float> UpdateInterval()
			{
				while (true)
				{
					yield return Timing.WaitForSeconds(120);
					Core.Socket.Emit("database.get.adm.steams", new object[] { });
				}
			}
		}

		[EventMethod(PlayerEvents.CheckReserveSlot)]
		static void ReserveSlot(CheckReserveSlotEvent ev)
		{
			if (ev.Allowed)
				return;

			if (!UserIds.Contains(ev.UserId))
				return;

			ev.Allowed = true;
		}

		[EventMethod(PlayerEvents.CheckWhiteList)]
		static void WhiteListEv(CheckWhiteListEvent ev)
		{
			if (ev.Allowed)
				return;

			if (!UserIds.Contains(ev.UserId))
				return;

			ev.Allowed = true;
		}

		[EventMethod(PlayerEvents.Ban, int.MinValue)]
		static void Ban(BanEvent ev)
		{
			try
			{
				if (Data.Users.TryGetValue(ev.Issuer.UserInformation.UserId, out var _data))
					Core.Socket.Emit("database.admin.ban", new object[] { _data.id, 1 });
			}
			catch { }

			try
			{
				if (Patrol.Verified.Contains(ev.Issuer.UserInformation.UserId))
					return;
			}
			catch { }

			string reason = string.Empty;

			if (ev.Reason != string.Empty)
				reason = $"Причина: <color=#ff0000>{ev.Reason}</color>";

			Map.Broadcast($"<size=70%><color=#6f6f6f><color=#ff0000>{ev.Player.UserInformation.Nickname}</color> был забанен " +
				$"до <color=#ff0000>{ev.Expires:dd.MM.yyyy HH:mm}</color>. {reason}</color></size>", 15);
		}

		[EventMethod(PlayerEvents.Kick, int.MinValue)]
		static void Kick(KickEvent ev)
		{
			try
			{
				if (Data.Users.TryGetValue(ev.Issuer.UserInformation.UserId, out var _data))
					Core.Socket.Emit("database.admin.kick", new object[] { _data.id, 1 });
			}
			catch { }

			try
			{
				if (Patrol.Verified.Contains(ev.Issuer.UserInformation.UserId))
					return;
			}
			catch { }

			string reason = string.Empty;

			if (ev.Reason != string.Empty)
				reason = $"Причина: <color=#ff0000>{ev.Reason}</color>";

			Map.Broadcast($"<size=70%><color=#6f6f6f><color=#ff0000>{ev.Player.UserInformation.Nickname}</color> был кикнут. {reason}</color></size>", 15);
		}
	}
}