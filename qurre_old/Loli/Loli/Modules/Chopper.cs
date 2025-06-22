using Loli.Addons;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using System.Collections.Generic;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		internal void ChopperRefresh() => Timing.RunCoroutine(ChopperThread(), "ChopperThread");
		internal void ChopperRefresh(RoundEndEvent _) => Timing.KillCoroutines("ChopperThread");
		public IEnumerator<float> ChopperThread()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(200);
				if (Round.Started)
				{
					Cassie.Send("The plane will supply the aid both within a minute", true, true);
					AirDrop.Call();
				}
			}
		}
	}
}