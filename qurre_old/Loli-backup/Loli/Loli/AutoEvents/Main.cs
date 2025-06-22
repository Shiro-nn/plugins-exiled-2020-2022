using MEC;
using Qurre.API;
using System.Linq;
namespace Loli.AutoEvents
{
    public class Main
    {
        internal readonly Plugin plugin;
        public Main(Plugin plugin) => this.plugin = plugin;
        public StormBase StormBase;
        public GunGame GunGame;
        public TeamDeathmatch TeamDeathmatch;
        public void Register()
        {
            Qurre.Events.Server.SendingRA += RA;
        }
        public void UnRegister()
        {
            Qurre.Events.Server.SendingRA -= RA;
            UnRegisterStormBase();
            UnRegisterGunGame();
            UnRegisterTeamDeathmatch();
        }
        #region StormBase
        public void RegisterStormBase()
        {
            StormBase = new StormBase(this);
            Qurre.Events.Player.InteractLift += StormBase.Elevator;
            Qurre.Events.Player.Spawn += StormBase.Spawned;
            Qurre.Events.Player.Leave += StormBase.Left;
            Qurre.Events.Player.Join += StormBase.Join;
            Qurre.Events.Player.PickupItem += StormBase.Pickup;
            Qurre.Events.Player.DropItem += StormBase.Drop;
            Qurre.Events.Player.Dead += StormBase.Dead;
            Qurre.Events.Round.Check += StormBase.Ending;
        }
        public void UnRegisterStormBase()
        {
            Qurre.Events.Player.InteractLift -= StormBase.Elevator;
            Qurre.Events.Player.Spawn -= StormBase.Spawned;
            Qurre.Events.Player.Leave -= StormBase.Left;
            Qurre.Events.Player.Join -= StormBase.Join;
            Qurre.Events.Player.PickupItem -= StormBase.Pickup;
            Qurre.Events.Player.DropItem -= StormBase.Drop;
            Qurre.Events.Player.Dead -= StormBase.Dead;
            Qurre.Events.Round.Check -= StormBase.Ending;
            StormBase = null;
        }
        #endregion
        #region GunGame
        public void RegisterGunGame()
        {
            GunGame = new GunGame(this);
            Qurre.Events.Round.Waiting += GunGame.Waiting;
            Qurre.Events.Player.Join += GunGame.Join;
            Qurre.Events.Player.Leave += GunGame.Leave;
            Qurre.Events.Player.Dead += GunGame.Dead;
            Qurre.Events.Player.RoleChange += GunGame.Spawn;
            Qurre.Events.Player.Spawn += GunGame.Spawn;
            Qurre.Events.Player.ThrowItem += GunGame.AddGrenade;
            Qurre.Events.Player.PickupItem += GunGame.Pickup;
            Qurre.Events.Player.DroppingItem += GunGame.Drop;
            Qurre.Events.Player.RagdollSpawn += GunGame.Ragdolls;
            Qurre.Events.Map.PlaceBulletHole += GunGame.Bullets;
            Qurre.Events.Player.InteractDoor += GunGame.Door;
            Qurre.Events.Map.LczDecon += GunGame.LczFix;
            Qurre.Events.Scp914.Activating += GunGame.UpgradeFix;
        }
        public void UnRegisterGunGame()
        {
            Qurre.Events.Round.Waiting -= GunGame.Waiting;
            Qurre.Events.Player.Join -= GunGame.Join;
            Qurre.Events.Player.Leave -= GunGame.Leave;
            Qurre.Events.Player.Dead -= GunGame.Dead;
            Qurre.Events.Player.RoleChange -= GunGame.Spawn;
            Qurre.Events.Player.Spawn -= GunGame.Spawn;
            Qurre.Events.Player.ThrowItem -= GunGame.AddGrenade;
            Qurre.Events.Player.PickupItem -= GunGame.Pickup;
            Qurre.Events.Player.DroppingItem -= GunGame.Drop;
            Qurre.Events.Player.RagdollSpawn -= GunGame.Ragdolls;
            Qurre.Events.Map.PlaceBulletHole -= GunGame.Bullets;
            Qurre.Events.Player.InteractDoor -= GunGame.Door;
            Qurre.Events.Map.LczDecon -= GunGame.LczFix;
            Qurre.Events.Scp914.Activating -= GunGame.UpgradeFix;
            Timing.KillCoroutines("GunGameCor");
            GunGame.Kills.Clear();
            GunGame = null;
            GunGame.Enabled = false;
        }
        #endregion
        #region TeamDeathmatch
        public void RegisterTeamDeathmatch()
        {
            TeamDeathmatch = new TeamDeathmatch(this);
            Qurre.Events.Round.Waiting += TeamDeathmatch.Waiting;
            Qurre.Events.Round.Start += TeamDeathmatch.RoundStart;
            Qurre.Events.Player.RagdollSpawn += TeamDeathmatch.Ragdolls;
            Qurre.Events.Player.DroppingItem += TeamDeathmatch.Drop;
            Qurre.Events.Player.Spawn += TeamDeathmatch.Spawn;
            Qurre.Events.Player.Leave += TeamDeathmatch.Leave;
            Qurre.Events.Player.Join += TeamDeathmatch.Join;
            Qurre.Events.Player.Dead += TeamDeathmatch.Dead;
        }
        public void UnRegisterTeamDeathmatch()
        {
            Qurre.Events.Round.Waiting -= TeamDeathmatch.Waiting;
            Qurre.Events.Round.Start -= TeamDeathmatch.RoundStart;
            Qurre.Events.Player.RagdollSpawn -= TeamDeathmatch.Ragdolls;
            Qurre.Events.Player.DroppingItem -= TeamDeathmatch.Drop;
            Qurre.Events.Player.Spawn -= TeamDeathmatch.Spawn;
            Qurre.Events.Player.Leave -= TeamDeathmatch.Leave;
            Qurre.Events.Player.Join -= TeamDeathmatch.Join;
            Qurre.Events.Player.Dead -= TeamDeathmatch.Dead;
            Timing.KillCoroutines("TeamDeathmatchHints");
            Timing.KillCoroutines("TeamDeathmatchWaitRestart");
            TeamDeathmatch = null;
            TeamDeathmatch.Enabled = false;
        }
        #endregion
        private void RA(Qurre.API.Events.SendingRAEvent ev)
        {
            if (ev.Name == "auto_event" || ev.Name == "aa" || ev.Name == "ae")
            {
                ev.Prefix = "AutoEvents";
                ev.Allowed = false;
                if (Event_Access(ev.CommandSender.SenderId))
                {
                    if (ev.Args.Count() > 0)
                    {
                        string event_name = ev.Args[0].ToLower();
                        if (event_name == "storm" || event_name == "storm_base" || event_name == "sb")
                        {
                            ev.ReplyMessage = "Устарел";
                            return;/*
                            RegisterStormBase();
                            Map.Broadcast("<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Штурм базы Хаоса</color>.</color>\n" +
                                "<color=#494f61>Подробности в консоли на <color=lime>[<color=aqua>ё</color>]</color>.</color></size>", 15);
                            foreach (Player pl in Player.List) pl.SendConsoleMessage("\nШтурм базы Хаоса\nХаос забрал из комплекса MicroHID'ы, совет о5 дал приказ: *вернуть все MicroHID'ы*.\n" +
                                "Задача мог - забрать все MicroHID'ы с базы хаоса.\nЗадача хаоса - защитить MicroHID'ы.\n" +
                                "MicroHID'ы могут подбирать и мог, и хаос. Если кто-то подобрал MicroHID, то об этом узнают другие, поэтому будьте осторожны.", "red");
                            ev.ReplyMessage = "Успешно";
                            StormBase.Enable();
                            Logs.Api.SendRa($"🎚️ Запущен ивент - Штурм базы Хаоса");
                            SCPDiscordLogs.Api.SendMessage($"🎚️ Запущен ивент - Штурм базы Хаоса");
                            SCPDiscordLogs.Api.SendMessage($"🎚️ Запущен ивент - Штурм базы Хаоса", SCPDiscordLogs.Api.Status.RemoteAdmin);*/
                        }
                        else if (event_name == "gun" || event_name == "gun_game" || event_name == "gg")
                        {
                            RegisterGunGame();
                            Map.Broadcast("<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Игра с оружием</color>.</color>\n<color=#494f61>Ваша задача - убить других.</color></size>", 15);
                            ev.ReplyMessage = "Успешно";
                            GunGame.Enable();
                            Logs.Api.SendRa($"🎚️ Запущен ивент - Игра с оружием");
                            SCPDiscordLogs.Api.SendMessage($"🎚️ Запущен ивент - Игра с оружием");
                            SCPDiscordLogs.Api.SendMessage($"🎚️ Запущен ивент - Игра с оружием", SCPDiscordLogs.Api.Status.RemoteAdmin);
                        }
                        else if (event_name == "teamdeathmatch" || event_name == "tdm" || event_name == "td")
                        {
                            RegisterTeamDeathmatch();
                            ev.ReplyMessage = "Успешно";
                            TeamDeathmatch.Enable();
                            Logs.Api.SendRa($"🎚️ Запущен ивент - Командная битва насмерть");
                            SCPDiscordLogs.Api.SendMessage($"🎚️ Запущен ивент - Командная битва насмерть");
                            SCPDiscordLogs.Api.SendMessage($"🎚️ Запущен ивент - Командная битва насмерть", SCPDiscordLogs.Api.Status.RemoteAdmin);
                        }
                    }
                    else
                    {
                        ev.ReplyMessage = "Доступные авто-ивенты для запуска:\n" +
                            "Штурм базы хаоса - auto_event storm_base // ae sb\n" +
                            "Игра с оружием - auto_event gun_game // ae gg\n" +
                            "Командная битва насмерть - auto_event TeamDeathmatch // ae td";
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
                if (DataBase.Manager.Static.Data.Users.TryGetValue(iD, out var main) &&
                    (main.mainhelper || main.admin || main.mainadmin || main.owner || main.id == 1 ||
                    (Plugin.YouTubersServer && DataBase.Modules.CustomDonates.ThisYt(main)))) _ = true;
            }
            catch { }
            return _;
        }
    }
}