using System.Collections.Generic;
using System.Linq;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace Loli.ClansWars.Capturing
{
    internal class Map2
    {
        internal const float DistanceSave = 10f;
        internal const float DistanceUp = 4.5f;
        internal static bool Enabled { get; private set; } = false;
        internal static string Hint { get; private set; } = "";
        internal static Map2 Static { get; private set; }
        internal Map2(CapturingManager mngr)
        {
            Manager = mngr;
            Static = this;
        }
        internal CapturingManager Manager;
        internal Progresses Progress1 { get; private set; }
        internal Progresses Progress2 { get; private set; }
        internal Progresses Progress3 { get; private set; }
        internal RolesCl Roles { get; private set; }
        internal int MinusCounts => Player.List.Where(x => x.Tag.Contains(MinusTag)).Count();
        internal int PlusCounts => Player.List.Where(x => x.Tag.Contains(PlusTag)).Count();
        private string MinusTag => "MinusCapturing";
        private string PlusTag => "PlusCapturing";
        internal void Disable()
        {
            if (!Enabled) return;
            Enabled = false;
            Round.Lock = false;
            Round.End();
            Map.Broadcast("<size=30%><color=#fdffbb>Ивент окончен.</color></size>", 15);
        }
        internal void Enable()
        {
            Enabled = true;
            foreach (var rm in Map.Rooms)
            {
                if (rm.Zone == ZoneType.Heavy)
                {
                    if (rm.Type == RoomType.HczCrossing)
                    {
                        rm.LightColor = new Color(0.98f, 0.6f, 0.6f);
                    }
                    else
                    {
                        rm.LightColor = new Color(0.5f, 0.5f, 0.5f);
                    }
                }
                else
                {
                    rm.LightColor = new Color(0, 0, 0);
                    rm.LightsOff(10000);
                }
            }
            var rooms = Map.Rooms.Where(x => x.Type == RoomType.HczCrossing).ToArray();
            Progress1 = new(rooms[0]);
            Progress2 = new(rooms[1]);
            Progress3 = new(rooms[2]);
            Roles = new(RoleType.ClassD, RoleType.Scientist);
            Qurre.Events.Map.UseLift += Perms;
            Qurre.Events.Map.DoorOpen += Perms;
            Qurre.Events.Player.InteractDoor += Perms;
            Qurre.Events.Player.Join += Join;
            Qurre.Events.Player.RagdollSpawn += Ragdolls;
            Qurre.Events.Map.PlaceBulletHole += Bullets;
            Qurre.Events.Player.Dies += Dies;
            Qurre.Events.Player.Dead += Dead;
            Qurre.Events.Player.DroppingItem += Drop;
            Qurre.Events.Player.Spawn += Spawn;
            Qurre.Events.Player.RoleChange += Spawn;
            Round.Lock = true;
            Timing.RunCoroutine(Сycle());
            IEnumerator<float> Сycle()
            {
                for (; ; )
                {
                    try { CheckPoints(); } catch { }
                    yield return Timing.WaitForSeconds(1f);
                }
            }
            GameCore.Console.singleton.TypeCommand($"/mapeditor load map2", new GameCoreSender("MapEditor"));
            var __list = Player.List.ToList();
            __list.Shuffle();
            foreach (var pl in __list) RefreshPlayer(pl);
        }
        internal static string GetHint(Player pl)
        {
            if (Round.Ended) return "";
            string text;
            if (Vector3.Distance(pl.Position, Static.Progress1.Room.Position) < DistanceSave)
            {
                if (Vector3.Distance(pl.Position, Static.Progress1.Room.Position) >= DistanceUp) text = "Вы находитесь в зоне точки #<color=red>1</color>";
                else text = "Вы находитесь в зоне <color=#00ff19>захвата</color> точки #<color=red>1</color>";
            }
            else if (Vector3.Distance(pl.Position, Static.Progress2.Room.Position) < DistanceSave)
            {
                if (Vector3.Distance(pl.Position, Static.Progress2.Room.Position) >= DistanceUp) text = "Вы находитесь в зоне точки #<color=red>2</color>";
                else text = "Вы находитесь в зоне <color=#00ff19>захвата</color> точки #<color=red>2</color>";
            }
            else if (Vector3.Distance(pl.Position, Static.Progress3.Room.Position) < DistanceSave)
            {
                if (Vector3.Distance(pl.Position, Static.Progress3.Room.Position) >= DistanceUp) text = "Вы находитесь в зоне точки #<color=red>3</color>";
                else text = "Вы находитесь в зоне <color=#00ff19>захвата</color> точки #<color=red>3</color>";
            }
            else text = "Вы не находитесь на точке";
            return Hint + $"<align=left><pos=-20%><b><size=70%><color=#0eadff>{text}</color></size></b></pos></align>\n\n" +
                "<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#ff0000>Суть данного ивента - захват точек." +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=60%><alpha=#FF><b>" +
                    "Точки - квадратные развилки в тяжелой зоне</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#00ff19>Для захвата точки, стойте в зоне</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#00ff19>захвата - в 5-ти метрах от центра</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#ffb500>Вы можете предложить идеи для карты" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "в <color=#0089c7>Discord</color> сервере</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#f47fff>discord.gg/UCUBU2z</color>" +
                    "</b></size></pos></align>\n";
        }
        private void CheckPoints()
        {
            try { Check1(); } catch { }
            try { Check2(); } catch { }
            try { Check3(); } catch { }
            var bar1 = GetBar(Progress1);
            var bar2 = GetBar(Progress2);
            var bar3 = GetBar(Progress3);
            Hint = $"" +
                $"<align=left><pos=-20%><b><size=150%><color=#ea00ff>Захват точек</color></size></b></pos></align>\n\n" +
                $"<align=left><pos=-20%><b><size=70%><color=#00ff19>{Progress1?.Sum / 2}%</color></size></b></pos></align>\n" +
                $"<align=left><pos=-20%><size=10>{bar1}</size></pos></align>\n\n" +
                $"<align=left><pos=-20%><b><size=70%><color=#00ff19>{Progress2?.Sum / 2}%</color></size></b></pos></align>\n" +
                $"<align=left><pos=-20%><size=10>{bar2}</size></pos></align>\n\n" +
                $"<align=left><pos=-20%><b><size=70%><color=#00ff19>{Progress3?.Sum / 2}%</color></size></b></pos></align>\n" +
                $"<align=left><pos=-20%><size=10>{bar3}</size></pos></align>\n\n" +
                $"";
            if((Progress1.Sum == -200 && Progress2.Sum == -200 && Progress3.Sum == -200) || (Progress1.Sum == 200 && Progress2.Sum == 200 && Progress3.Sum == 200))
            {
                Disable();
            }
            string GetBar(Progresses pr)
            {
                string PlusColor = Roles.PlusColor;
                string MinusColor = Roles.MinusColor;
                string bar = "";
                if (pr == null) return bar;
                if (pr.Sum == 0) bar = $"<color={MinusColor}>████████████████████</color><color={PlusColor}>████████████████████</color>";
                else if (pr.Sum > 0)
                {
                    if (pr.Sum < 10) bar = $"<color={MinusColor}>█████████████████████</color><color={PlusColor}>███████████████████</color>";
                    else if (pr.Sum < 20) bar = $"<color={MinusColor}>██████████████████████</color><color={PlusColor}>██████████████████</color>";
                    else if (pr.Sum < 30) bar = $"<color={MinusColor}>███████████████████████</color><color={PlusColor}>█████████████████</color>";
                    else if (pr.Sum < 40) bar = $"<color={MinusColor}>████████████████████████</color><color={PlusColor}>████████████████</color>";
                    else if (pr.Sum < 50) bar = $"<color={MinusColor}>█████████████████████████</color><color={PlusColor}>███████████████</color>";
                    else if (pr.Sum < 60) bar = $"<color={MinusColor}>██████████████████████████</color><color={PlusColor}>███████████████</color>";
                    else if (pr.Sum < 70) bar = $"<color={MinusColor}>███████████████████████████</color><color={PlusColor}>██████████████</color>";
                    else if (pr.Sum < 80) bar = $"<color={MinusColor}>████████████████████████████</color><color={PlusColor}>█████████████</color>";
                    else if (pr.Sum < 90) bar = $"<color={MinusColor}>█████████████████████████████</color><color={PlusColor}>████████████</color>";
                    else if (pr.Sum < 100) bar = $"<color={MinusColor}>██████████████████████████████</color><color={PlusColor}>███████████</color>";
                    else if (pr.Sum < 110) bar = $"<color={MinusColor}>███████████████████████████████</color><color={PlusColor}>██████████</color>";
                    else if (pr.Sum < 120) bar = $"<color={MinusColor}>████████████████████████████████</color><color={PlusColor}>█████████</color>";
                    else if (pr.Sum < 130) bar = $"<color={MinusColor}>█████████████████████████████████</color><color={PlusColor}>████████</color>";
                    else if (pr.Sum < 140) bar = $"<color={MinusColor}>██████████████████████████████████</color><color={PlusColor}>███████</color>";
                    else if (pr.Sum < 150) bar = $"<color={MinusColor}>███████████████████████████████████</color><color={PlusColor}>██████</color>";
                    else if (pr.Sum < 160) bar = $"<color={MinusColor}>████████████████████████████████████</color><color={PlusColor}>█████</color>";
                    else if (pr.Sum < 170) bar = $"<color={MinusColor}>█████████████████████████████████████</color><color={PlusColor}>████</color>";
                    else if (pr.Sum < 180) bar = $"<color={MinusColor}>██████████████████████████████████████</color><color={PlusColor}>███</color>";
                    else if (pr.Sum < 190) bar = $"<color={MinusColor}>███████████████████████████████████████</color><color={PlusColor}>██</color>";
                    else if (pr.Sum < 200) bar = $"<color={MinusColor}>████████████████████████████████████████</color><color={PlusColor}>█</color>";
                    else bar = $"<color={MinusColor}>█████████████████████████████████████████</color>";
                }
                else if (pr.Sum < 0)
                {
                    if (pr.Sum > -10) bar = $"<color={MinusColor}>███████████████████</color><color={PlusColor}>█████████████████████</color>";
                    else if (pr.Sum > -20) bar = $"<color={MinusColor}>██████████████████</color><color={PlusColor}>██████████████████████</color>";
                    else if (pr.Sum > -30) bar = $"<color={MinusColor}>█████████████████</color><color={PlusColor}>███████████████████████</color>";
                    else if (pr.Sum > -40) bar = $"<color={MinusColor}>████████████████</color><color={PlusColor}>████████████████████████</color>";
                    else if (pr.Sum > -50) bar = $"<color={MinusColor}>███████████████</color><color={PlusColor}>█████████████████████████</color>";
                    else if (pr.Sum > -60) bar = $"<color={MinusColor}>██████████████</color><color={PlusColor}>██████████████████████████</color>";
                    else if (pr.Sum > -70) bar = $"<color={MinusColor}>█████████████</color><color={PlusColor}>███████████████████████████</color>";
                    else if (pr.Sum > -80) bar = $"<color={MinusColor}>████████████</color><color={PlusColor}>████████████████████████████</color>";
                    else if (pr.Sum > -90) bar = $"<color={MinusColor}>███████████</color><color={PlusColor}>█████████████████████████████</color>";
                    else if (pr.Sum > -100) bar = $"<color={MinusColor}>██████████</color><color={PlusColor}>██████████████████████████████</color>";
                    else if (pr.Sum > -110) bar = $"<color={MinusColor}>█████████</color><color={PlusColor}>███████████████████████████████</color>";
                    else if (pr.Sum > -120) bar = $"<color={MinusColor}>████████</color><color={PlusColor}>████████████████████████████████</color>";
                    else if (pr.Sum > -130) bar = $"<color={MinusColor}>███████</color><color={PlusColor}>█████████████████████████████████</color>";
                    else if (pr.Sum > -140) bar = $"<color={MinusColor}>██████</color><color={PlusColor}>██████████████████████████████████</color>";
                    else if (pr.Sum > -150) bar = $"<color={MinusColor}>█████</color><color={PlusColor}>███████████████████████████████████</color>";
                    else if (pr.Sum > -160) bar = $"<color={MinusColor}>████</color><color={PlusColor}>████████████████████████████████████</color>";
                    else if (pr.Sum > -170) bar = $"<color={MinusColor}>███</color><color={PlusColor}>█████████████████████████████████████</color>";
                    else if (pr.Sum > -180) bar = $"<color={MinusColor}>██</color><color={PlusColor}>██████████████████████████████████████</color>";
                    else if (pr.Sum > -200) bar = $"<color={MinusColor}>█</color><color={PlusColor}>███████████████████████████████████████</color>";
                    else bar = $"<color={PlusColor}>████████████████████████████████████████</color>";
                }
                return bar;
            }
            void Check1()
            {
                var pls9 = Player.List.Where(x => Vector3.Distance(x.Position, Progress1.Room.Position) < DistanceSave);
                if (pls9.Count() == 0)
                {
                    if (Progress1.Sum == -200 || Progress1.Sum == 200) return;
                    if (Progress1.Sum > 0) Progress1.Sum--;
                    else if (Progress1.Sum < 0) Progress1.Sum++;
                    return;
                }
                var pls = Player.List.Where(x => Vector3.Distance(x.Position, Progress1.Room.Position) < DistanceUp);
                if (pls.Count() == 0)
                {
                    if (Progress1.Sum == -200 || Progress1.Sum == 200) return;
                    if (pls9.Where(x => x.Role == Roles.Minus).Count() == 0 && Progress1.Sum > 0) Progress1.Sum--;
                    if (pls9.Where(x => x.Role == Roles.Plus).Count() == 0 && Progress1.Sum < 0) Progress1.Sum++;
                    return;
                }
                var pPlus = pls.Where(x => x.Role == Roles.Plus).Count();
                var pMinus = pls.Where(x => x.Role == Roles.Minus).Count();
                if (pPlus > pMinus)
                {
                    var dd = pPlus - pMinus;
                    Progress1.Sum -= (dd == 1 ? 1 : dd / 2);
                }
                else if (pPlus < pMinus)
                {
                    var dd = pMinus - pPlus;
                    Progress1.Sum += (dd == 1 ? 1 : dd / 2);
                }
            }
            void Check2()
            {
                var pls9 = Player.List.Where(x => Vector3.Distance(x.Position, Progress2.Room.Position) < DistanceSave);
                if (pls9.Count() == 0)
                {
                    if (Progress2.Sum == -200 || Progress2.Sum == 200) return;
                    if (Progress2.Sum > 0) Progress2.Sum--;
                    else if (Progress2.Sum < 0) Progress2.Sum++;
                    return;
                }
                var pls = Player.List.Where(x => Vector3.Distance(x.Position, Progress2.Room.Position) < DistanceUp);
                if (pls.Count() == 0)
                {
                    if (Progress2.Sum == -200 || Progress2.Sum == 200) return;
                    if (pls9.Where(x => x.Role == Roles.Minus).Count() == 0 && Progress2.Sum > 0) Progress2.Sum--;
                    if (pls9.Where(x => x.Role == Roles.Plus).Count() == 0 && Progress2.Sum < 0) Progress2.Sum++;
                    return;
                }
                var pPlus = pls.Where(x => x.Role == Roles.Plus).Count();
                var pMinus = pls.Where(x => x.Role == Roles.Minus).Count();
                if (pPlus > pMinus)
                {
                    var dd = pPlus - pMinus;
                    Progress2.Sum -= (dd == 1 ? 1 : dd / 2);
                }
                else if (pPlus < pMinus)
                {
                    var dd = pMinus - pPlus;
                    Progress2.Sum += (dd == 1 ? 1 : dd / 2);
                }
            }
            void Check3()
            {
                var pls9 = Player.List.Where(x => Vector3.Distance(x.Position, Progress3.Room.Position) < DistanceSave);
                if (pls9.Count() == 0)
                {
                    if (Progress3.Sum == -200 || Progress3.Sum == 200) return;
                    if (Progress3.Sum > 0) Progress3.Sum--;
                    else if (Progress3.Sum < 0) Progress3.Sum++;
                    return;
                }
                var pls = Player.List.Where(x => Vector3.Distance(x.Position, Progress3.Room.Position) < DistanceUp);
                if (pls.Count() == 0)
                {
                    if (Progress3.Sum == -200 || Progress3.Sum == 200) return;
                    if (pls9.Where(x => x.Role == Roles.Minus).Count() == 0 && Progress3.Sum > 0) Progress3.Sum--;
                    if (pls9.Where(x => x.Role == Roles.Plus).Count() == 0 && Progress3.Sum < 0) Progress3.Sum++;
                    return;
                }
                var pPlus = pls.Where(x => x.Role == Roles.Plus).Count();
                var pMinus = pls.Where(x => x.Role == Roles.Minus).Count();
                if (pPlus > pMinus)
                {
                    var dd = pPlus - pMinus;
                    Progress3.Sum -= (dd == 1 ? 1 : dd / 2);
                }
                else if (pPlus < pMinus)
                {
                    var dd = pMinus - pPlus;
                    Progress3.Sum += (dd == 1 ? 1 : dd / 2);
                }
            }
        }
        internal void Perms(UseLiftEvent ev) => ev.Allowed = false;
        internal void Perms(DoorOpenEvent ev)
        {
            if (ev.Door.Type == DoorType.Checkpoint_EZ_HCZ || ev.Door.Type == DoorType.HCZ_106_Primary || ev.Door.Type == DoorType.HCZ_106_Secondary) ev.Allowed = false;
        }
        internal void Perms(InteractDoorEvent ev)
        {
            if (ev.Door.Type == DoorType.Checkpoint_EZ_HCZ || ev.Door.Type == DoorType.HCZ_106_Primary || ev.Door.Type == DoorType.HCZ_106_Secondary) ev.Allowed = false;
        }
        internal void Join(JoinEvent ev) => RefreshPlayer(ev.Player);
        internal void Ragdolls(RagdollSpawnEvent ev) => ev.Allowed = false;
        internal void Bullets(PlaceBulletHoleEvent ev) => ev.Allowed = false;
        internal void Dies(DiesEvent ev) => ev.Target.ClearInventory();
        internal void Dead(DeadEvent ev)
        {
            if (ev.Target.Tag.Contains(PlusTag)) ev.Target.Role = Roles.Plus;
            else if (ev.Target.Tag.Contains(MinusTag)) ev.Target.Role = Roles.Minus;
            else RefreshPlayer(ev.Target);
        }
        internal void Drop(DroppingItemEvent ev)
        {
            ev.Allowed = false;
            ev.Player.RemoveItem(ev.Item);
        }
        internal void RefreshPlayer(Player pl)
        {
            if (PlusCounts > MinusCounts)
            {
                pl.Tag = MinusTag;
                pl.Role = Roles.Minus;
            }
            else
            {
                pl.Tag = PlusTag;
                pl.Role = Roles.Plus;
            }
        }
        internal void Spawn(RoleChangeEvent ev)
        {
            if (ev.Player.Tag.Contains(PlusTag))
            {
                if (ev.NewRole != Roles.Plus)
                {
                    ev.Allowed = false;
                    ev.Player.Role = Roles.Plus;
                }
                else
                {
                    ev.Player.BlockSpawnTeleport = true;
                    ev.Player.TeleportToRoom(RoomType.HczChkpA);
                }
            }
            else if (ev.Player.Tag.Contains(MinusTag))
            {
                if (ev.NewRole != Roles.Minus)
                {
                    ev.Allowed = false;
                    ev.Player.Role = Roles.Minus;
                }
                else
                {
                    ev.Player.BlockSpawnTeleport = true;
                    ev.Player.TeleportToRoom(RoomType.HczChkpB);
                }
            }
            else
            {
                RefreshPlayer(ev.Player);
                ev.Allowed = false;
            }
        }
        internal void Spawn(SpawnEvent ev)
        {
            if (ev.Player.Tag.Contains(PlusTag))
            {
                if (ev.RoleType != Roles.Plus) ev.Player.Role = Roles.Plus;
                else ev.Position = RoomType.HczChkpA.GetRoom().Position + Vector3.up * 2;
                AddItems(ev.Player);
            }
            else if (ev.Player.Tag.Contains(MinusTag))
            {
                if (ev.RoleType != Roles.Minus) ev.Player.Role = Roles.Minus;
                else ev.Position = RoomType.HczChkpB.GetRoom().Position + Vector3.up * 2;
                AddItems(ev.Player);
            }
            else RefreshPlayer(ev.Player);
        }
        private void AddItems(Player pl)
        {
            pl.ClearInventory();
            pl.GetAmmo();
            pl.AddItem(ItemType.KeycardNTFOfficer);
            pl.AddItem(ItemType.GunCrossvec);
            pl.AddItem(ItemType.Medkit);
            pl.AddItem(ItemType.Medkit);
            pl.AddItem(ItemType.Adrenaline);
            pl.AddItem(ItemType.Radio);
            pl.AddItem(ItemType.Flashlight);
        }


        internal class Progresses
        {
            internal Progresses(Room room) => Room = room;
            internal Room Room;
            internal int Sum
            {
                get => _sum;
                set
                {
                    if (value > 200) value = 200;
                    if (value < -200) value = -200;
                    _sum = value;
                }
            }
            private protected int _sum = 0;
        }
        internal class RolesCl
        {
            internal RolesCl(RoleType plus, RoleType minus)
            {
                Plus = plus;
                Minus = minus;
            }
            internal RoleType Plus;
            internal RoleType Minus;
            internal string PlusColor => RoleToColor(Plus);
            internal string MinusColor => RoleToColor(Minus);
            private string RoleToColor(RoleType rt)
            {
                if (rt.GetTeam() == Team.CDP) return "#ff9900";
                if (rt.GetTeam() == Team.SCP) return "#ff0000";
                if (rt.GetTeam() == Team.RSC) return "#e2e26d";
                if (rt.GetTeam() == Team.TUT) return "#00ff00";
                if (rt.GetTeam() == Team.CHI) return "#58be58";
                if (rt == RoleType.FacilityGuard) return "#afafa1";
                if (rt.GetTeam() == Team.MTF) return "#0074ff";
                return "#ffffff";
            }
        }
    }
}