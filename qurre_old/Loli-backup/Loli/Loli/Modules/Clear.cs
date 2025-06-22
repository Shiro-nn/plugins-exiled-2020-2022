using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DateTime = System.DateTime;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		private static readonly List<ushort> Pickups = new();
		internal void ClearRefresh()
		{
			Timing.RunCoroutine(ClearWarhead(), "ClearWarheadThread");
			Timing.RunCoroutine(ClearManyItems(), "ClearManyItemsThread");
			Pickups.Clear();
			Timing.CallDelayed(5f, () => { foreach (var pick in Map.Pickups) if (!Pickups.Contains(pick.Serial)) Pickups.Add(pick.Serial); });
		}
		internal void ClearRefresh(RoundEndEvent _)
		{
			Timing.KillCoroutines("ClearWarheadThread");
			Timing.KillCoroutines("ClearManyItemsThread");
			Pickups.Clear();
		}
		internal IEnumerator<float> ClearWarhead()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(10);
				if (Alpha.Detonated) Timing.RunCoroutine(_clear());
			}
			static IEnumerator<float> _clear()
			{
				var items = new List<Pickup>();
				foreach (Pickup item in Map.Pickups.Where(x => x.Position.y.Difference(980) > 50 && !x.Base.ItsNeededItem() &&
				Vector3.Distance(x.Position, Textures.Models.Rooms.ServersManager.ServersRoom.Model.GameObject.transform.position) > 20f))
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
				var dolls = new List<Qurre.API.Controllers.Ragdoll>();
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
		internal DateTime ClearManyItemsLast = DateTime.Now;
		internal IEnumerator<float> ClearManyItems()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(15);
				var picks = Map.Pickups.Where(x => !Pickups.Contains(x.Serial) && !DataBase.Shop.ItsShop(x) && !x.Base.ItsNeededItem());
				bool b1 = picks.Count() > 500;
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
					Map.Broadcast($"<size=25%><color=#6f6f6f>Cовет О5 активировал <color=red>молекулярное уничтожение</color> <color=#0089c7>{t}</color>," +
					 "\n<color=lime>ввиду сохранности секретности комплекса</color>.</color></size>", 10);
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
				var dolls = new List<Qurre.API.Controllers.Ragdoll>();
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