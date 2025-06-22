using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace MongoDB.bc
{
	public class mec
	{
		public Plugin plugin;
		private static Random rand = new Random();
		public mec(Plugin plugin) => this.plugin = plugin;
		internal IEnumerator<float> randombc()
		{
			Timing.WaitForSeconds(1f);
			for (; ; )
			{
				int ri = rand.Next(1, 100);
				if (ri < 20)
				{
					Map.Broadcast(10, "<b><color=#0089c7>Discord</color> <color=#00ffff>проекта</color>:</b>\n<color=#f47fff>discord<color=red>.</color>gg<color=#006dff>/</color>UCUBU2z</color>", global::Broadcast.BroadcastFlags.Normal);
				}
				else if (ri < 40)
				{
					Map.Broadcast(10, "<b><color=#00ffff>Сайт</color>:</b>\n<color=#0089c7>scpsl<color=red>.</color>store</color>", global::Broadcast.BroadcastFlags.Normal);
				}
				else if (ri < 60)
				{
					Map.Broadcast(10, "<b><color=#00ffff>Хотите узнать консольные команды</color>?</b>\n<color=#0089c7>Напишите <color=#0089c7>.</color><color=#ff0>help</color> в консоли на <color=##f47fff>[<color=red>ё</color>]</color></color>", global::Broadcast.BroadcastFlags.Normal);
				}
				else if (ri < 80)
				{
					Map.Broadcast(10, "<color=#fdffbb>Хотите поддержать проект</color><color=red>?</color>\n<color=#f47fff>Вы можете купить <color=red>Донат</color> на сайте</color> <color=#0089c7>scpsl<color=red>.</color>store</color>", global::Broadcast.BroadcastFlags.Normal);
				}
				else
				{
					Map.Broadcast(10, "<size=30%><b><color=#00ffff>Есть <color=red>Жалобы</color>? <color=#15ff00>Предложения</color>? <color=#006dff>Вопросы</color>? <color=#ffb500>Проблемы с сервером</color>?\n<color=#9bff00>Хотите подать заявку на должность</color>?</color></b>\n<color=#fdffbb>Откройте тикет в <color=#0089c7>Discord</color>'e</color></size>", global::Broadcast.BroadcastFlags.Normal);
				}
				yield return Timing.WaitForSeconds(500f);
			}
		}
	}
}