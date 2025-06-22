using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.AutoEvents
{
    public class TeamDeathmatch
    {
        public static bool Enabled = false;
        internal readonly Main main;
        public TeamDeathmatch(Main main) => this.main = main;
        internal Dictionary<Player, RoleType> Role = new Dictionary<Player, RoleType>();
        internal int KillsMtf = 0;
        internal int KillsChaos = 0;
        internal void Enable()
        {
            Enabled = true;
            LockDoor(DoorType.Surface_Gate);
            LockDoor(DoorType.Checkpoint_EZ_HCZ);
            KillsMtf = 0;
            KillsChaos = 0;
            Round.Lock = true;
            Timing.RunCoroutine(HintList(), "TeamDeathmatchHints");
            if (Round.Started)
            {
                Timing.RunCoroutine(WaitRestart(), "TeamDeathmatchWaitRestart");
                foreach (Player pl in Player.List) pl.Role = RoleType.Tutorial;
            }
            Map.Broadcast("<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Командная битва насмерть</color>.</color>\n<color=#494f61>Ваша задача - убить противников.</color></size>", 15);
            static void LockDoor(DoorType type)
            {
                Door door = Qurre.API.Extensions.GetDoor(type);
                door.Open = false;
                door.Locked = true;
            }
        }
        internal void Waiting() => main.UnRegisterTeamDeathmatch();
        internal void RoundStart() => Timing.RunCoroutine(WaitRestart(), "TeamDeathmatchWaitRestart");
        internal void Ragdolls(RagdollSpawnEvent ev) => ev.Allowed = false;
        internal void Drop(DroppingItemEvent ev) => ev.Allowed = false;
        internal void Spawn(SpawnEvent ev)
        {
            ev.Player.Tag = "";
            if (!Role.ContainsKey(ev.Player))
            {
                if (Role.Where(x => x.Value == RoleType.NtfSergeant).Count() < Role.Where(x => x.Value == RoleType.ChaosRepressor).Count())
                {
                    Role.Add(ev.Player, RoleType.NtfSergeant);
                    ev.Player.Role = RoleType.NtfSergeant;
                }
                else
                {
                    Role.Add(ev.Player, RoleType.ChaosRepressor);
                    ev.Player.Role = RoleType.ChaosRepressor;
                }
            }
            else
            {
                if (ev.RoleType != RoleType.NtfSergeant && ev.RoleType != RoleType.ChaosRepressor)
                {
                    ev.Player.Role = Role[ev.Player];
                }
                else
                {
                    ev.Player.Invisible = true;
                    Timing.CallDelayed(0.5f, () =>
                    {
                        ItemType KeyCard = ItemType.KeycardNTFCommander;
                        if (ev.RoleType == RoleType.ChaosRepressor) KeyCard = ItemType.KeycardChaosInsurgency;
                        ev.Player.ClearInventory();
                        ev.Player.GetAmmo();
                        ev.Player.AddItem(KeyCard);
                        ev.Player.AddItem(ItemType.GunE11SR);
                        ev.Player.AddItem(ItemType.GunLogicer);
                        ev.Player.AddItem(ItemType.Adrenaline);
                        ev.Player.AddItem(ItemType.Medkit);
                        ev.Player.AddItem(ItemType.Medkit);
                        ev.Player.AddItem(ItemType.GrenadeFlash);
                        ev.Player.AddItem(ItemType.Flashlight);
                        if (Round.ElapsedTime.TotalSeconds > 60) ev.Player.Position = Map.GetRandomSpawnPoint(RoleType.FacilityGuard) + Vector3.up;
                        else ev.Player.Position = Map.GetRandomSpawnPoint(ev.RoleType) + Vector3.up;
                        ev.Player.UnitName = "#fydne";
                        ev.Player.Hp = 150;
                        ev.Player.MaxHp = 150;
                        ev.Player.Invisible = false;
                    });
                }
            }
        }
        internal void Leave(LeaveEvent ev)
        {
            if (Role.ContainsKey(ev.Player)) Role.Remove(ev.Player);
        }
        internal void Join(JoinEvent ev)
        {
            Timing.CallDelayed(1f, () =>
            {
                if (!Role.ContainsKey(ev.Player))
                {
                    if (Role.Where(x => x.Value == RoleType.NtfSergeant).Count() < Role.Where(x => x.Value == RoleType.ChaosRepressor).Count())
                    {
                        Role.Add(ev.Player, RoleType.NtfSergeant);
                        ev.Player.Role = RoleType.NtfSergeant;
                    }
                    else
                    {
                        Role.Add(ev.Player, RoleType.ChaosRepressor);
                        ev.Player.Role = RoleType.ChaosRepressor;
                    }
                }
                else ev.Player.Role = Role[ev.Player];
                ev.Player.Broadcast("<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Командная битва насмерть</color>.</color>\n<color=#494f61>Ваша задача - убить противников.</color></size>", 15);
            });
        }
        internal void Dead(DeadEvent ev)
        {
            try
            {
                if (ev.Killer != ev.Target)
                {
                    if (Role[ev.Killer] == RoleType.ChaosRepressor) KillsChaos++;
                    else if (Role[ev.Killer] == RoleType.NtfSergeant) KillsMtf++;
                }
            }
            catch { }
            if (!Role.ContainsKey(ev.Target)) return;
            ev.Target.Role = Role[ev.Target];
        }
        public IEnumerator<float> WaitRestart()
        {
            int thisRound = Round.CurrentRound;
            Round.Lock = true;
            yield return Timing.WaitForSeconds(600f);
            if (thisRound != Round.CurrentRound) yield break;
            Round.Lock = false;
            Round.End();
            LeadingTeam leader = LeadingTeam.Draw;
            RoleType WinnerRole = RoleType.None;
            if (KillsChaos > KillsMtf)
            {
                leader = LeadingTeam.ChaosInsurgency;
                WinnerRole = RoleType.ChaosRepressor;
            }
            else if (KillsChaos < KillsMtf)
            {
                leader = LeadingTeam.FacilityForces;
                WinnerRole = RoleType.NtfSergeant;
            }
            Round.ShowRoundSummary(new RoundSummary.SumInfo_ClassList(), leader);
            Timing.CallDelayed(1f, () => Round.ShowRoundSummary(new RoundSummary.SumInfo_ClassList(), leader));
            if (WinnerRole == RoleType.None)
            {
                Map.Broadcast("<size=30%><color=#fdffbb>Все участники ивента получают <color=red>250 опыта</color> & <color=red>25 монет</color>.</color></size>", 15);
                foreach (var pl in Role)
                {
                    DataBase.Manager.Static.Stats.Add(pl.Key, 250, 25);
                }
            }
            else
            {
                Map.Broadcast("<size=30%><color=#fdffbb>Победители ивента получают <color=red>500 опыта</color> & <color=red>50 монет</color>.</color></size>", 15);
                foreach (var pl in Role.Where(x => x.Value == WinnerRole))
                {
                    DataBase.Manager.Static.Stats.Add(pl.Key, 500, 50);
                }
            }
            yield break;
        }
        public IEnumerator<float> HintList()
        {
            for (; ; )
            {
                string date = $"{9 - Round.ElapsedTime.Minutes}:{60 - Round.ElapsedTime.Seconds}";
                if (Round.ElapsedTime.Minutes > 9) date = "0:0";
                else if (Round.ElapsedTime.Seconds == 0) date = $"{10 - Round.ElapsedTime.Minutes}:0";
                string text = $"<align=right><size=70%><alpha=#FF><b><color=#00ff88>До конца ивента - {date}</color></b></size></align>\n";
                text += $"<align=right><size=70%><alpha=#FF><b><color=#ff0000>Счетчик убийств</color></b></size></align>\n";
                text += $"<align=right><size=70%><alpha=#FF><b><color=#0089c7>Мог - {KillsMtf}</color></b></size></align>\n";
                text += $"<align=right><size=70%><alpha=#FF><b><color=#1aad3c>Хаос - {KillsChaos}</color></b></size></align>\n";
                Map.ShowHint(text.Trim(), 1.5f);
                var pks = Map.Pickups;
                foreach (var pk in pks) pk.Destroy();
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}