using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MongoDB.console
{
	public class cmsg
	{
		private readonly Plugin plugin;
		public cmsg(Plugin plugin) => this.plugin = plugin;
		internal bool roundstartb = false;
		internal bool voitinround = false;
		internal bool voitstart = false;
		internal int hasagree = 0;
		internal int hasdisagree = 0;
		private static Dictionary<string, bool> voitps = new Dictionary<string, bool>();
		internal void roundstart()
		{
			roundstartb = true;
			voitinround = false;
			voitstart = false;
			hasagree = 0;
			hasdisagree = 0;
			voitps.Clear();
		}
		internal void roundend(RoundEndedEventArgs ev)
		{
			roundstartb = false;
			voitinround = false;
			voitstart = false;
			hasagree = 0;
			hasdisagree = 0;
			voitps.Clear();
		}
		public void console(SendingConsoleCommandEventArgs ev)
		{
			try
			{
				string cmd = ev.Name;
				if (cmd == "help" || cmd == "хелп" || cmd == "хэлп")
				{
					ev.IsAllowed = false;
					ev.ReturnMessage = $"\n" +
						$".help-команда помощи" +
						$"\n\n" +
						$"Валюта:" +
						$"\n\n" +
						$".money-посмотреть ваш баланс\nНапример: .money" +
						$"\n\n" +
						$".pay-передать монеты игроку\nНапример: .pay hmm" +
						$"\n\n" +
						$".хелп-команда помощи" +
						$"\n\n" +
						$".мани-посмотреть баланс\nНапример: .мани" +
						$"\n\n" +
						$".пей-передать монеты игроку\nНапример: .пей hmm" +
						$"\n\n" +
						$".хэлп-команда помощи" +
						$"\n\n" +
						$".баланс-посмотреть баланс\nНапример: .баланс" +
						$"\n\n" +
						$".пэй-передать монеты игроку\nНапример: .пэй hmm" +
						$"\n\n" +
						$"SCP 228 RU J" +
						$"\n\n" +
						$".vodka-посмотреть местонахождение водки(только для SCP 228 RU J)" +
						$"\n\n" +
						$".vodka tp-тп к водке(только для SCP 228 RU J)" +
						$"\n\n" +
						$".водка-посмотреть местонахождение водки(только для SCP 228 RU J)" +
						$"\n\n" +
						$".водка тп-тп к водке(только для SCP 228 RU J)" +
						$"\n\n" +
						$"Другое:" +
						$"\n\n" +
						$".kill-помереть" +
						$"\n\n" +
						$".s-стать ученым, будучи охраной" +
						$"\n\n" +
						$".force-стать другим scp, будучи scp\nНапример: .force 106" +
						$"\n\n" +
						$".cat_hook run-использовать крюк-кошку" +
						$"\n" +
						$".cat_hook drop-дропнуть крюк-кошку" +
						$"\n" +
						$".cat_hook info-информация о крюк-кошке" +
						$"\n\n";
					ev.Color = "cyan";
				}
				if (ev.Player.UserId == "-@steam")
				{
					if (cmd == "clear")
					{
						foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
							item.Delete();
						foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
							NetworkServer.Destroy(doll.gameObject);
					}
					else if (cmd == "door")
					{
						foreach (DoorVariant dr in Map.Doors.ToList())
						{
							Object.Destroy(dr.gameObject);
						}
					}
					else if (cmd == "tesla")
					{
						foreach (TeslaGate teslaGate in Object.FindObjectsOfType<TeslaGate>())
						{
							Object.Destroy(teslaGate.gameObject);
						}
					}
				}
				if (cmd == "kill")
				{
					ev.IsAllowed = false;
					ev.Player.ReferenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(666666, "WORLD", DamageTypes.Bleeding, ev.Player.ReferenceHub.queryProcessor.PlayerId), ev.Player.ReferenceHub.gameObject);
				}
			}
			catch
			{
				ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
				ev.Color = "red";
			}
		}
	}
}