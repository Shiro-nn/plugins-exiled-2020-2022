using System.Collections.Generic;
using MEC;
using Qurre.API;

namespace Loli.Addons
{
	static class BroadCasts
	{
		static internal IEnumerator<float> Send()
		{
			Timing.WaitForSeconds(1f);
			for (; ; )
			{
				int random = UnityEngine.Random.Range(1, 100);
				if (random < 20)
				{
					Map.Broadcast("<size=70%><b><color=#ff1f00>Хотите вступить в клан</color><color=#0089c7>?</color></b>\n" +
						"<color=#bb00ff>Вы можете вступить в клан на сайте <color=#fdffbb>scpsl<color=red>.</color>store</color></color></size>", 20);
				}
				else if (random < 32)
				{
					Map.Broadcast("<b><color=#00ffff>Хотите узнать <color=red>TPS</color> серверов?</color></b>\n" +
						"<size=70%><color=#0089c7>Вы можете узнать <color=red>TPS</color> серверов за\nпоследнее время на сайте </color>" +
						"<color=#fdffbb>scpsl<color=#0089c7>.</color>store</color></size>", 20);
				}
				else if (random < 44)
				{
					Map.Broadcast("<size=70%><b><color=#00ffff>Хотите узнать консольные команды</color>?</b>\n" +
						"<color=#0089c7>Напишите <color=#0089c7>.</color><color=#ff0>help</color> в консоли на <color=#f47fff>[<color=red>ё</color>]</color></color></size>", 10);
				}
				else if (random < 56)
				{
					Map.Broadcast("<size=70%><color=#fdffbb>Хотите поддержать проект</color><color=red>?</color>\n" +
						"<color=#f47fff>Вы можете приобрести <color=red>Донат</color> на сайте</color> <color=#0089c7>scpsl<color=red>.</color>store</color></size>", 20);
				}
				else if (random < 70)
				{
					Map.Broadcast("<size=70%><b><color=#00ffff>Есть <color=red>Жалобы</color>? <color=#15ff00>Предложения</color>? <color=#006dff>Вопросы</color>? " +
						"<color=#ffb500>Проблемы с сервером</color>?\n<color=#9bff00>Хотите подать заявку на должность</color>?</color></b>\n" +
						"<color=#fdffbb>Откройте тикет в <color=#0089c7>Discord</color>'e</color></size>", 15);
				}
				else if (random < 85)
				{
					Map.Broadcast("<b><color=#eb1bff>Хотите разнообразия</color><color=#0089c7>?</color></b>\n" +
						"<size=70%><color=#00ff00>Вы можете <color=#0089c7>кастомизировать</color> своего персонажа на сайте\n" +
						"<color=#fdffbb>scpsl<color=#0089c7>.</color>store</color></color></size>", 20);
				}
				else
				{
					Map.Broadcast("<size=70%><b><color=#ff1f00>Хотите вступить в клан</color><color=#0089c7>?</color></b>\n" +
						"<color=#bb00ff>Вы можете вступить в клан на сайте <color=#fdffbb>scpsl<color=red>.</color>store</color></color></size>", 20);
				}
				yield return Timing.WaitForSeconds(500f);
			}
		}
	}
}