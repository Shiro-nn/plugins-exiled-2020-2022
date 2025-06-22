using Exiled.API.Features;
using Exiled.Events.EventArgs;
namespace cassie
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public void WaitingForPlayers() => plugin.cfg1();
		public void AnnouncingMTF(AnnouncingNtfEntranceEventArgs ev)
		{
			ev.IsAllowed = false;
			Cassie.Message(plugin.config.MTFCassie.Replace("%UnitName%", ev.UnitName).Replace("%UnitNumber%", $"{ev.UnitNumber}").Replace("%ScpsLeft%", $"{ev.ScpsLeft}"));
		}
		internal void TeamRespawn(RespawningTeamEventArgs ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency && ev.IsAllowed)
				Cassie.Message(plugin.config.CICassie);
		}
	}
}