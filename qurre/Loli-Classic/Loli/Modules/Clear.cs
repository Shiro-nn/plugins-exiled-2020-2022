using InventorySystem.Items.Pickups;
using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loli.Modules
{
	static class Clear
	{
		static internal readonly List<ushort> Pickups = new();

		[EventMethod(RoundEvents.Start)]
		static void StartRefresh()
		{
			Timing.RunCoroutine(ClearWarhead(), "ClearWarheadThread");
			Timing.RunCoroutine(ClearManyItems(), "ClearManyItemsThread");
			Pickups.Clear();
			Timing.CallDelayed(5f, () => { foreach (var pick in Map.Pickups) if (!Pickups.Contains(pick.Serial)) Pickups.Add(pick.Serial); });
		}

		[EventMethod(RoundEvents.End)]
		static void ClearEndRefresh()
		{
			Timing.KillCoroutines("ClearWarheadThread");
			Timing.KillCoroutines("ClearManyItemsThread");
			Pickups.Clear();
		}

		static IEnumerator<float> ClearWarhead()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(10);

				if (Alpha.Detonated)
					Timing.RunCoroutine(_clear());
			}

			static IEnumerator<float> _clear()
			{
				var items = new List<Pickup>();
				foreach (Pickup item in Map.Pickups.Where(x => x.Position.y.Difference(980) > 50))
				{
					items.Add(item);
					yield return Timing.WaitForSeconds(0.001f);
				}
				foreach (var item in items)
				{
					try { item.Destroy(); } catch { }
					yield return Timing.WaitForSeconds(0.01f);
				}
				items.Clear();
				var dolls = new List<Ragdoll>();
				foreach (var doll in Map.Ragdolls.Where(x => x.Position.y.Difference(980) > 50))
				{
					dolls.Add(doll);
					yield return Timing.WaitForSeconds(0.001f);
				}
				foreach (var doll in dolls)
				{
					try { doll.Destroy(); } catch { }
					yield return Timing.WaitForSeconds(0.01f);
				}
				dolls.Clear();
				yield break;
			}
		}

		static DateTime ClearManyItemsLast = DateTime.Now;
		static IEnumerator<float> ClearManyItems()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(15);
				var picks = Map.Pickups.Where(x => !Pickups.Contains(x.Serial));
				bool b1 = picks.Count() > 1000;
				bool b2 = Map.Ragdolls.Count > 100;
				if (b1) Timing.RunCoroutine(ClearItems(picks));
				if (b2) Timing.RunCoroutine(ClearDolls());
				if ((b1 || b2) && (DateTime.Now - ClearManyItemsLast).TotalSeconds > 60)
				{
					ClearManyItemsLast = DateTime.Now;
					string t = "неизвестно чего";
					if (b1) t = "вещей";
					else if (b2) t = "всех трупов";
					if (b1 && b2) t = "вещей, а также трупов";
					Map.Broadcast($"<size=65%><color=#6f6f6f>Cовет О5 активировал <color=red>молекулярное уничтожение</color> <color=#0089c7>{t}</color>," +
					 "\n<color=#00ff22>ввиду сохранности секретности комплекса</color>.</color></size>", 10);
				}
			}

			static IEnumerator<float> ClearItems(IEnumerable<Pickup> picks)
			{
				var items = new List<Pickup>();
				foreach (Pickup item in picks)
				{
					items.Add(item);
					yield return Timing.WaitForSeconds(0.001f);
				}
				foreach (var item in items)
				{
					try { item.Destroy(); } catch { }
					yield return Timing.WaitForSeconds(0.005f);
				}
				items.Clear();
				yield break;
			}

			static IEnumerator<float> ClearDolls()
			{
				var dolls = new List<Ragdoll>();
				foreach (var doll in Map.Ragdolls)
				{
					dolls.Add(doll);
					yield return Timing.WaitForSeconds(0.001f);
				}
				foreach (var doll in dolls)
				{
					try { doll.Destroy(); } catch { }
					yield return Timing.WaitForSeconds(0.005f);
				}
				dolls.Clear();
				yield break;
			}
		}
	}
}