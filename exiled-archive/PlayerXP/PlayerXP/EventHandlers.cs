using Exiled.Events.EventArgs;
using Hints;
using MEC;
using System.Linq;

namespace PlayerXP
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		internal bool first = true;
		public void OnWaitingForPlayers()
		{
			try
			{
				if (first)
				{
					first = false;
					ServerConsole._serverName = ServerConsole._serverName.Replace("<size=1>SM119.0.0</size>", "").Replace("<size=1>Exiled 2.1.2</size>", "");
					ServerConsole._serverName += $"<color=#00000000><size=1>Qurre 1.0.1 ^beta^</size></color>";
				}
			}
			catch { }
		}
		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			string str = $"\n<color=#00fffb>Добро пожаловать на сервер</color> <color=#ffa600>f</color><color=#ffff00>y<color=#1eff00>d</color><color=#0004ff>n</color><color=#9d00ff>e</color>\n<color=#09ff00>Приятной игры!</color>";
			ev.Player.ReferenceHub.hints.Show(new TextHint(str.Trim(), new HintParameter[]
			{
					new StringHintParameter("")
			}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 10f));
		}
		public void tesla(TriggeringTeslaEventArgs ev)
		{
			ev.IsTriggerable = false;
		}
		internal void roundend(RoundEndedEventArgs ev)
		{
			Timing.CallDelayed(0.5f, () =>
			{
				foreach (ReferenceHub player in Extensions.GetHubs().ToList())
					plugin.donate.setprefix(player);
			});
		}
	}
}