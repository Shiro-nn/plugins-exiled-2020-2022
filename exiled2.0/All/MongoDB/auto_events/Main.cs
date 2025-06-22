using Exiled.API.Features;
namespace MongoDB.auto_events
{
    public class Main
    {
        internal readonly Plugin plugin;
        public Main(Plugin plugin) => this.plugin = plugin;
        public storm_base storm_base;
        public void Register()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += RA;
            RegisterStormBase();
        }
        public void UnRegister()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= RA;
            UnRegisterStormBase();
        }
        public void RegisterStormBase()
        {
            storm_base = new storm_base(this);
            Exiled.Events.Handlers.Player.InteractingElevator += storm_base.Elevator;
            Exiled.Events.Handlers.Server.WaitingForPlayers += storm_base.Waiting;
            Exiled.Events.Handlers.Server.RoundStarted += storm_base.Started;
            Exiled.Events.Handlers.Player.Spawning += storm_base.Spawned;
            Exiled.Events.Handlers.Player.Left += storm_base.Left;
            Exiled.Events.Handlers.Player.Joined += storm_base.Join;
            Exiled.Events.Handlers.Player.PickingUpItem += storm_base.Pickup;
            Exiled.Events.Handlers.Player.ItemDropped += storm_base.Drop;
            Exiled.Events.Handlers.Player.Died += storm_base.Dead;
            Exiled.Events.Handlers.Server.EndingRound += storm_base.Ending;
        }
        public void UnRegisterStormBase()
        {
            Exiled.Events.Handlers.Player.InteractingElevator -= storm_base.Elevator;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= storm_base.Waiting;
            Exiled.Events.Handlers.Server.RoundStarted -= storm_base.Started;
            Exiled.Events.Handlers.Player.Spawning -= storm_base.Spawned;
            Exiled.Events.Handlers.Player.Left -= storm_base.Left;
            Exiled.Events.Handlers.Player.Joined -= storm_base.Join;
            Exiled.Events.Handlers.Player.PickingUpItem -= storm_base.Pickup;
            Exiled.Events.Handlers.Player.ItemDropped -= storm_base.Drop;
            Exiled.Events.Handlers.Player.Died -= storm_base.Dead;
            Exiled.Events.Handlers.Server.EndingRound -= storm_base.Ending;
            storm_base = null;
        }
        private void RA(Exiled.Events.EventArgs.SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name == "auto_event" || ev.Name == "aa")
            {
                ev.IsAllowed = false;
                if (Event_Access(ev.CommandSender.SenderId))
                {
                    if (ev.Arguments.Count > 0)
                    {
                        string event_name = ev.Arguments[0].ToLower();
                        if (event_name == "storm" || event_name == "storm_base" || event_name == "sb")
                        {
                            storm_base.Enabled = true;
                            foreach (Player pl in Player.List)
                            {
                                pl.Broadcast(15, "<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Штурм базы Хаоса</color>.</color>\n<color=#494f61>Подробности в консоли на <color=lime>[<color=aqua>ё</color>]</color>.</color></size>");
                                pl.SendConsoleMessage("\nШтурм базы Хаоса\nХаос забрал из комплекса пылесос, совет о5 дал приказ: *вернуть пылесос*.\nЗадача мог - забрать пылесос из базы хаоса.\nЗадача хаоса - защитить пылесос.\nПылесос могут подбирать и мог, и хаос. Если кто-то подобрал пылесос, то об этом узнают другие, поэтому будьте осторожны.", "red");
                            }
                            ev.ReplyMessage = "Успешно";
                            if (EventHandlers.RoundStarted) storm_base.Started();
                            logs.send.sendralog($"🎚️ Запущен ивент - mtf vs ci");
                            logs.send.sendra($"🎚️ Запущен ивент - mtf vs ci");
                        }
                    }
                    else
                    {
                        ev.ReplyMessage = "auto_event storm_base";
                    }
                }
                else
                {
                    ev.ReplyMessage = "Нет прав, увы.";
                }
            }
        }
        private bool Event_Access(string iD)
        {
            bool _ = false;
            if (iD == "-@steam" || iD == "SERVER CONSOLE") _ = true;
            try
            {
                var main = plugin.donate.main[iD];
                if (main.hr || main.ghr || main.ar || main.gar) _ = true;
            }
            catch { }
            return _;
        }
    }
}