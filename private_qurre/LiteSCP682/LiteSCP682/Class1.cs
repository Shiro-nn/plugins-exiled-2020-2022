using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LiteSCP682
{
    public class Plugin : Qurre.Plugin
    {
        public override string Name => "SCP682 Lite";
        public override string Developer => "fydne";
        public override Version NeededQurreVersion => new Version(1, 9, 9);
        public override Version Version => new Version(0, 0, 0);
        public override void Enable()
        {
            Qurre.Events.Round.Start += Started;
            Qurre.Events.Player.InteractDoor += Destroy;
            Qurre.Events.Player.Damage += Damage;
            Qurre.Events.Player.RoleChange += Spawn;
            Qurre.Events.Player.Spawn += Spawn;
            Qurre.Events.Player.Dead += Dead;
            Qurre.Events.Server.SendingRA += Ra;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Start -= Started;
            Qurre.Events.Player.InteractDoor -= Destroy;
            Qurre.Events.Player.Damage -= Damage;
            Qurre.Events.Player.RoleChange -= Spawn;
            Qurre.Events.Player.Spawn -= Spawn;
            Qurre.Events.Player.Dead -= Dead;
            Qurre.Events.Server.SendingRA -= Ra;
        }
        public static string ScpTag => "SCP682Lite";
        private void Started()
        {
            if (Player.List.Where(x => x.Tag.Contains(ScpTag)).Count() != 0) return;
            List<Player> pList = Player.List.Where(x => x.Role == RoleType.ClassD && x.UserId != null && x.UserId != string.Empty && !x.Overwatch).ToList();
            if (pList.Count == 0) return;
            Player pl = pList[Extensions.Random.Next(pList.Count)];
            Spawn(pl);
        }
        private void Spawn(Player pl)
        {
            pl.Role = RoleType.Scp93989;
            pl.Hp = 20000;
            pl.Tag += ScpTag;
            pl.CustomInfo = "SCP-682";
            pl.NicknameSync.ShownPlayerInfo = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo;
        }
        private DateTime LastBreak = DateTime.Now;
        private void Destroy(InteractDoorEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            if ((DateTime.Now - LastBreak).TotalSeconds < 20) return;
            ev.Door.Destroyed = true;
            LastBreak = DateTime.Now;
        }
        private void Damage(DamageEvent ev)
        {
            if (!ev.Attacker.Tag.Contains(ScpTag)) return;
            ev.Amount = 1000;
        }
        private void Dead(DeadEvent ev)
        {
            ev.Target.Tag = ev.Target.Tag.Replace(ScpTag, "");
            ev.Target.NicknameSync.ShownPlayerInfo = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;

        }
        private void Spawn(RoleChangeEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            if (ev.NewRole != RoleType.Scp93989)
            {
                ev.Player.Tag = ev.Player.Tag.Replace(ScpTag, "");
                ev.Player.NicknameSync.ShownPlayerInfo = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
            }
        }
        private void Spawn(SpawnEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            if (ev.RoleType != RoleType.Scp93989)
            {
                ev.Player.Tag = ev.Player.Tag.Replace(ScpTag, "");
                ev.Player.NicknameSync.ShownPlayerInfo = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
            }
        }
        public void Ra(SendingRAEvent ev)
        {
            if (ev.Name == "scp682")
            {
                ev.Prefix = "SCP682";
                ev.Allowed = false;
                try
                {
                    string name = string.Join(" ", ev.Args);
                    Player player = Player.Get(name);
                    if (player == null)
                    {
                        ev.Success = false;
                        ev.ReplyMessage = "Игрок не найден";
                        return;
                    }
                    ev.ReplyMessage = "Успешно";
                    Spawn(player);
                }
                catch
                {
                    ev.Success = false;
                    ev.ReplyMessage = "Произошла ошибка";
                    return;
                }
            }
        }
    }
}