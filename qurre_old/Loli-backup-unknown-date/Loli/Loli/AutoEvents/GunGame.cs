using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.AutoEvents
{
    public class GunGame
    {
        public static bool Enabled = false;
        internal readonly Main main;
        public GunGame(Main main) => this.main = main;
        internal Dictionary<Player, int> Kills = new Dictionary<Player, int>();
        internal void Enable()
        {
            Kills.Clear();
            Round.Lock = true;
            Enabled = true;
            foreach (Player pl in Player.List) AddPlayer(pl);
            if (Round.Started) foreach (Player pl in Player.List) pl.Role = RoleType.ClassD;
            Timing.RunCoroutine(HintList(), "GunGameCor");
        }
        internal void Waiting() => main.UnRegisterGunGame();
        internal void Join(JoinEvent ev)
        {
            Timing.CallDelayed(1f, () =>
            {
                AddPlayer(ev.Player);
                ev.Player.Broadcast("<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Игра с оружием</color>.</color>\n<color=#494f61>Ваша задача - убить других.</color></size>", 15);
                ev.Player.Role = RoleType.ClassD;
            });
        }
        internal void Leave(LeaveEvent ev)
        {
            RemovePlayer(ev.Player);
        }
        internal void Dead(DeadEvent ev)
        {
            AddKillsPlayer(ev.Killer);
            RefreshPlayer(ev.Target);
            AddWeapon(ev.Killer);
            ev.Target.Role = RoleType.ClassD;
        }
        internal void Spawn(RoleChangeEvent ev) => DoSpawn(ev.Player);
        internal void Spawn(SpawnEvent ev)
        {
            DoSpawn(ev.Player);
            if (Extensions.Random.Next(1, 2) == 1) ev.Position = Map.GetRandomSpawnPoint(RoleType.Scientist) + Vector3.up;
            else ev.Position = Qurre.API.Extensions.GetRoom((RoomType)Extensions.Random.Next(5, 12)).Position;
        }
        internal void DoSpawn(Player pl)
        {
            if(pl.Role != RoleType.ClassD)
            {
                pl.Role = RoleType.ClassD;
                return;
            }
            RefreshPlayer(pl);
            pl.FriendlyFire = true;
            pl.EnableEffect(EffectType.Invisible);
            Timing.CallDelayed(0.5f, () => AddWeapon(pl));
            Timing.CallDelayed(3f, () => pl.DisableEffect(EffectType.Invisible));
        }
        internal void AddGrenade(ThrowItemEvent ev) => ev.Player.AddItem(ev.Item.Type);
        internal void Pickup(PickupItemEvent ev)
        {
            if(ev.Pickup.Type != ItemType.Adrenaline && ev.Pickup.Type != ItemType.Painkillers && ev.Pickup.Type != ItemType.SCP500
                && ev.Pickup.Type != ItemType.Medkit) ev.Allowed = false;
        }
        internal void Drop(DroppingItemEvent ev)
        {
            if (ev.Item.Type != ItemType.Adrenaline && ev.Item.Type != ItemType.Painkillers && ev.Item.Type != ItemType.SCP500 && ev.Item.Type != ItemType.Medkit) ev.Allowed = false;
        }
        internal void Ragdolls(RagdollSpawnEvent ev) => ev.Allowed = false;
        internal void Bullets(PlaceBulletHoleEvent ev) => ev.Allowed = false;
        internal void Door(InteractDoorEvent ev)
        {
            if (ev.Door.Type == DoorType.Checkpoint_LCZ_A || ev.Door.Type == DoorType.Checkpoint_LCZ_B) ev.Allowed = false;
            else ev.Allowed = true;
        }
        internal void LczFix(LczDeconEvent ev) => ev.Allowed = false;
        internal void UpgradeFix(ActivatingEvent ev) => ev.Allowed = false;
        public IEnumerator<float> HintList()
        {
            for (; ; )
            {
                string text = "";
                var _data = Kills;
                var __data = _data.OrderBy(x => x.Value).Reverse();
                foreach (var data in __data) text += $"<align=right><size=70%><alpha=#FF><b><color=#ff0000>{data.Key.Nickname} - {data.Value}</color></b></size></align>\n";
                Map.ShowHint(text.Trim(), 1.5f);
                var pks = Map.Pickups.Where(x => x.Category != ItemCategory.Medical);
                foreach (var pk in pks) pk.Destroy();
                yield return Timing.WaitForSeconds(1f);
            }
        }
        internal void AddPlayer(Player pl)
        {
            if (Kills.ContainsKey(pl)) return;
            Kills.Add(pl, 0);
        }
        internal void RemovePlayer(Player pl)
        {
            if (!Kills.ContainsKey(pl)) return;
            Kills.Remove(pl);
        }
        internal void RefreshPlayer(Player pl)
        {
            if (!Kills.ContainsKey(pl)) Kills.Add(pl, 0);
            else Kills[pl] = 0;
        }
        internal void AddKillsPlayer(Player pl)
        {
            if (!Kills.ContainsKey(pl)) Kills.Add(pl, 0);
            Kills[pl] += 1;
            if(Kills[pl] > 5 && !Round.Ended)
            {
                Round.Lock = false;
                Round.End();
                Map.Broadcast($"<size=30%><color=#fdffbb><color=red>{pl.Nickname}</color> победил в ивенте, он получает" +
                    $" <color=red>500 опыта</color> & <color=red>50 монет</color>.</color></size>", 15);
                DataBase.Manager.Static.Stats.Add(pl, 500, 50);
            }
        }
        internal void AddWeapon(Player pl)
        {
            if (!Kills.ContainsKey(pl)) Kills.Add(pl, 0);
            int kills = Kills[pl];
            pl.ClearInventory();
            pl.GetAmmo();
            if (kills == 0) pl.AddItem(ItemType.GunCOM15);
            else if (kills == 1) pl.AddItem(ItemType.GunCOM18);
            else if (kills == 2) pl.AddItem(ItemType.GunCrossvec);
            else if (kills == 3) pl.AddItem(ItemType.GunRevolver);
            else if (kills == 4) pl.AddItem(ItemType.GunE11SR);
            else if (kills == 5) pl.AddItem(ItemType.GrenadeHE);
            else
            {
                RefreshPlayer(pl);
                pl.AddItem(ItemType.GunCOM15);
            }
        }
    }
}