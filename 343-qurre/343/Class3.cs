namespace _343
{
    internal class Class3 : Qurre.Plugin
    {
        private Scp343 _evs;
        internal static string _access = "-@steam";
        public override void Enable()
        {
            _access = Config.GetString("scp343_access", "-@steam", "те, у кого будут права к спавну 343 (через запятую, если несколько)");
            _evs = new Scp343();
            Qurre.Events.Round.Start += _evs.RoundStart;
            Qurre.Events.Round.End += _evs.RoundEnd;
            Qurre.Events.Player.Flashed += _evs.AntiGrenade;
            Qurre.Events.Player.ScpAttack += _evs.AntiScpAttack;
            Qurre.Events.Player.Dead += _evs.Dead;
            Qurre.Events.Player.Dies += _evs.UpdateDeaths;
            Qurre.Events.Player.Cuff += _evs.Cuff;
            Qurre.Events.Player.DamageProcess += _evs.Damage;
            Qurre.Events.Scp096.AddTarget += _evs.AddTarget;
            Qurre.Events.Player.Shooting += _evs.Shoot;
            Qurre.Events.Player.Escape += _evs.Escape;
            Qurre.Events.Player.RoleChange += _evs.ChangeRole;
            Qurre.Events.Player.Leave += _evs.Leave;
            Qurre.Events.Scp106.Contain += _evs.Contain;
            Qurre.Events.Scp106.PocketEnter += _evs.Pocket;
            Qurre.Events.Scp106.PocketEscape += _evs.Pocket;
            Qurre.Events.Scp106.PocketFailEscape += _evs.Pocket;
            Qurre.Events.Player.InteractDoor += _evs.Door;
            Qurre.Events.Player.DroppingItem += _evs.Drop;
            Qurre.Events.Scp106.FemurBreakerEnter += _evs.Femur;
            Qurre.Events.Player.PickupItem += _evs.Pickup;
            Qurre.Events.Alpha.Stopping += _evs.AlphaEv;
            Qurre.Events.Player.ItemUsing += _evs.Medical;
            Qurre.Events.Scp914.UpgradePlayer += _evs.Upgrade;
            Qurre.Events.Player.InteractLocker += _evs.Locker;
            Qurre.Events.Player.InteractGenerator += _evs.Generator;
            Qurre.Events.Server.SendingRA += _evs.Ra;
            MEC.Timing.RunCoroutine(_evs.UpdateTimes());
        }
        public override void Disable()
        {
        }
    }
}
