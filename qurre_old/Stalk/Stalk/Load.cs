using MEC;
namespace Stalk
{
    internal class Load : Qurre.Plugin
	{
		public override string Name => "Stalk";
		public override string Developer => "fydne";
		private const string CoroutinesName = "CoroutinesByStalkYes";
		public override void Enable()
		{
			Qurre.Events.Player.RoleChange += Main.ChangeRole;
			Qurre.Events.Scp106.PortalCreate += Main.CreatePortal;
			Qurre.Events.Round.Waiting += Cfg.Reload;
			Timing.RunCoroutine(Main.CooldownUpdate(), CoroutinesName);
		}
		public override void Disable()
		{
			Qurre.Events.Player.RoleChange -= Main.ChangeRole;
			Qurre.Events.Scp106.PortalCreate -= Main.CreatePortal;
			Qurre.Events.Round.Waiting -= Cfg.Reload;
			Timing.KillCoroutines(CoroutinesName);
		}
	}
}