using Qurre.API.Controllers;
using Qurre.API.Events;
namespace cassie
{
	public class EventHandlers
	{
		public void WaitingForPlayers() => Cfg.Reload();
		public void AnnouncingMTF(MTFAnnouncementEvent ev)
		{
			ev.Allowed = false;
			Cassie.Send(Cfg.MTFCassie.Replace("%UnitName%", ev.UnitName).Replace("%UnitNumber%", $"{ev.UnitNumber}").Replace("%ScpsLeft%", $"{ev.ScpsLeft}"));
		}
		internal void TeamRespawn(TeamRespawnEvent ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency && ev.Allowed) Cassie.Send(Cfg.CICassie);
		}
	}
}