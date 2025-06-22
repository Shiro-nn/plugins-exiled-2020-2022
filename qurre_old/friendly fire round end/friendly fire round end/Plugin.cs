using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
namespace friendly_fire_round_end
{
    public class Plugin : Qurre.Plugin
    {
        #region override
        public override System.Version Version => new System.Version(1, 0, 6);
        public override System.Version NeededQurreVersion => new System.Version(1, 10, 4);
        public override string Developer => "fydne";
        public override string Name => "friendly fire round end";
        public override void Enable()
        {
            Qurre.Events.Round.End += RoundEnd;
            Qurre.Events.Player.DamageProcess += EndFF;
        }
        public override void Disable()
        {
            Qurre.Events.Round.End -= RoundEnd;
            Qurre.Events.Player.DamageProcess -= EndFF;
        }
        #endregion
        #region Event
        internal void RoundEnd(RoundEndEvent ev)
        {
            if (!Server.FriendlyFire)
            {
                Cassie.Send("ATTENTION TO ALL PERSONNEL . team fire ENABLE");
            }
        }
        internal void EndFF(DamageProcessEvent ev)
        {
            if (Round.Ended)
            {
                ev.FriendlyFire = false;
                ev.Amount = 10;
                ev.Allowed = true;
            }
        }
        #endregion
    }
}