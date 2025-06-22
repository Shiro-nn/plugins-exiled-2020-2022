using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LiteSCP999
{
    public class Plugin : Qurre.Plugin
    {
        public override string Name => "SCP999 Lite";
        public override string Developer => "fydne";
        public override Version NeededQurreVersion => new Version(1, 9, 9);
        public override Version Version => new Version(0, 0, 0);
        public override void Enable()
        {
            Qurre.Events.Round.Start += Started;
            Qurre.Events.Player.InteractDoor += Door;
            Qurre.Events.Player.Damage += Damage;
            Qurre.Events.Player.RoleChange += Spawn;
            Qurre.Events.Player.Spawn += Spawn;
            Qurre.Events.Player.Dead += Dead;
            Qurre.Events.Player.Dies += Dies;
            Qurre.Events.Player.PickupItem += Pickup;
            Qurre.Events.Server.SendingRA += Ra;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Start -= Started;
            Qurre.Events.Player.InteractDoor -= Door;
            Qurre.Events.Player.Damage -= Damage;
            Qurre.Events.Player.RoleChange -= Spawn;
            Qurre.Events.Player.Spawn -= Spawn;
            Qurre.Events.Player.Dead -= Dead;
            Qurre.Events.Player.Dies -= Dies;
            Qurre.Events.Player.PickupItem -= Pickup;
            Qurre.Events.Server.SendingRA -= Ra;
        }
        public static string ScpTag => "SCP999Lite";
        private void Started()
        {
            MEC.Timing.CallDelayed(1f, () =>
            {
                if (Player.List.Where(x => x.Tag.Contains(ScpTag)).Count() != 0) return;
                List<Player> pList = Player.List.Where(x => x.Role == RoleType.ClassD && x.UserId != null && x.UserId != string.Empty && !x.Overwatch).ToList();
                if (pList.Count == 0) return;
                Player pl = pList[Extensions.Random.Next(pList.Count)];
                Spawn(pl);
            });
        }
        private void Spawn(Player pl)
        {
            pl.BlockSpawnTeleport = true;
            pl.Role = RoleType.Tutorial;
            pl.Position = RoomType.EzCafeteria.GetRoom().Position;
            pl.GodMode = true;
            pl.Tag += ScpTag;
            pl.ClearInventory();
            pl.AddItem(ItemType.Flashlight);
        }
        private void Door(InteractDoorEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            if (ev.Door.Type == DoorType.Gate_A || ev.Door.Type == DoorType.Gate_B) return;
            ev.Allowed = true;
        }
        private void Damage(DamageEvent ev)
        {
            if (!ev.Attacker.Tag.Contains(ScpTag)) return;
            ev.Amount = 0;
            ev.Allowed = false;
        }
        private void Dead(DeadEvent ev) => ev.Target.Tag = ev.Target.Tag.Replace(ScpTag, "");
        private void Dies(DiesEvent ev)
        {
            if (!ev.Target.Tag.Contains(ScpTag)) return;
            ev.Allowed = false;
        }
        private void Pickup(PickupItemEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            ev.Allowed = false;
        }
        private void Spawn(RoleChangeEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            if (ev.NewRole != RoleType.Tutorial) ev.Allowed = false;
        }
        private void Spawn(SpawnEvent ev)
        {
            if (!ev.Player.Tag.Contains(ScpTag)) return;
            if (ev.RoleType != RoleType.Tutorial) ev.Player.Tag = ev.Player.Tag.Replace(ScpTag, "");
        }
        private void Ra(SendingRAEvent ev)
        {
            if (ev.Name == "scp999")
            {
                ev.Prefix = "SCP999";
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