using System.Collections.Generic;
using System.Linq;
using Loli.Spawns;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using Respawning;
using Respawning.NamingRules;
using UnityEngine;
namespace Loli.Addons.RolePlay
{
    internal class Modules
    {
        internal void RealEscape() => Server.RealEscape = Plugin.RolePlay;
        internal void MainGuard()
        {
            if (!Plugin.RolePlay) return;
            var list = Player.List.Where(x => x.Role == RoleType.FacilityGuard).ToArray();
            if (list.Count() == 0) return;
            list.Shuffle();
            Player guard = list.First();
            Timing.CallDelayed(0.5f, () =>
            {
                guard.Hp = 150;
                guard.ResetInventory(new List<ItemType>
                {
                    ItemType.KeycardNTFLieutenant,
                    ItemType.ArmorCombat,
                    ItemType.GunE11SR,
                    ItemType.SCP500,
                    ItemType.Medkit,
                    ItemType.GrenadeHE,
                    ItemType.Radio,
                    ItemType.Flashlight,
                });
                guard.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#afafa1>Начальник СБ</color>\n" +
                    "Вся охрана комплекса подчиняется вашим приказам</color></size>", 10, true);
                guard.NicknameSync.Network_customPlayerInfoString = "Начальник СБ";
                guard.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
                guard.GetAmmo();
            });
        }
        internal void BetterRadio(RadioUsingEvent ev)
        {
            if (!Plugin.RolePlay) return;
            ev.Consumption *= 0.3f;
        }
        internal void RealEscape(EscapeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            ev.NewRole = RoleType.Spectator;
        }
        internal void RealUnits()
        {
            if (!Plugin.RolePlay) return;
            UnitNamingManager.RolesWithEnforcedDefaultName[RoleType.Scientist] = SpawnableTeamType.NineTailedFox;
        }
        internal void RealHid(ItemChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player.Role == RoleType.Scientist) return;
            if (ev.Player.Tag.Contains("DontHid")) return;
            if (ev.OldItem?.Type == ItemType.MicroHID) ev.Allowed = false;
        }
        internal void RealHid(DroppingItemEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.Player.Role == RoleType.Scientist) return;
            if (ev.Item.Type != ItemType.MicroHID) return;
            ev.Player.Tag += " DontHid";
            Timing.CallDelayed(1f, () => ev.Player.Tag = ev.Player.Tag.Replace(" DontHid", ""));
        }
        internal void RealHid(PickupItemEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.Player.Role == RoleType.Scientist) return;
            if (ev.Pickup.Type != ItemType.MicroHID) return;
            ev.Allowed = false;
            if (ev.Player.Tag.Contains(" DontHid")) return;
            ev.Allowed = true;
            Timing.CallDelayed(0.5f, () => ev.Player.Inventory.ServerSelectItem(ev.Pickup.Serial));
        }
        internal void Real914(UpgradeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            var list = ev.Players.ToArray();
            Timing.CallDelayed(0.1f, () =>
            {
                foreach (var pl in list) try { pl.Damage(Random.Range(30, 120), "Похоже, был переработан в SCP-914"); } catch { }
            });
        }
        internal void Door(InteractDoorEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if ((ev.Door.Type == DoorType.LCZ_Cafe || ev.Door.Type == DoorType.GR18) &&
                ev.Player.AllItems.Where(x => x.Category == ItemCategory.Keycard).Count() == 0) ev.Allowed = false;
        }
        internal void FixRecharge(RechargeWeaponEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            var team = ev.Player.GetTeam();
            if (team != Team.RSC && team != Team.CDP) return;
            if (ev.Player.Tag.Contains(Roles.FacilityManager.Tag)) return;
            if (ev.Player.Tag.Contains(Roles.FacilityManager.TagSpy)) return;
            if (ev.Item.Type == ItemType.GunCOM15 || ev.Item.Type == ItemType.GunCOM18 || ev.Item.Type == ItemType.GunRevolver) return;
            ev.Allowed = false;
        }
        internal void Spawn(RoleChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.NewRole == RoleType.Spectator) return;
            if (ev.NewRole == RoleType.ClassD) return;
            if (ev.NewRole.GetTeam() == Team.SCP) return;
            if (ev.NewRole == RoleType.Scientist) return;
            Timing.CallDelayed(0.5f, () =>
            {
                float scale = Random.Range(90, 110);
                ev.Player.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
            });
        }
        internal void Spawn(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.RoleType == RoleType.Spectator) return;
            if (ev.RoleType == RoleType.ClassD) return;
            if (ev.RoleType.GetTeam() == Team.SCP) return;
            if (ev.RoleType == RoleType.Scientist) return;
            float scale = Random.Range(90, 110);
            ev.Player.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
        }
        internal static float WaitingSweep => 1800f;
        internal static string SweepTag => "SweepGroup";
        internal void RoundStart()
        {
            if (!Plugin.RolePlay) return;
            int round = Round.CurrentRound;
            Timing.RunCoroutine(DoRun());
            IEnumerator<float> DoRun()
            {
                yield return Timing.WaitForSeconds(WaitingSweep);
                bool spawned = false;
                while (!spawned)
                {
                    if (round != Round.CurrentRound) yield break;
                    Timing.RunCoroutine(SpawnSwapGroup());
                    yield return Timing.WaitForSeconds(30f);
                }
                IEnumerator<float> SpawnSwapGroup()
                {
                    if (Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).Count() == 0) yield break;
                    var TeamType = SpawnableTeamType.NineTailedFox;
                    RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, TeamType);
                    yield return Timing.WaitForSeconds(15f);
                    List<Player> list = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToList();
                    if (list.Count == 0) yield break;
                    list.Shuffle();
                    spawned = true;
                    Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#ff0000>Группа зачистки</color>");
                    foreach (Player pl in list) SpawnOne(pl);
                    Cassie.Send("ATTENTION TO ALL PERSONNEL . . a sweep group has arrival at the complex . my pity");
                    RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, TeamType);
                    Timing.RunCoroutine(CheckSwapGroup());
                    Timing.RunCoroutine(LightsOff());
                    void SpawnOne(Player pl)
                    {
                        SpawnManager.SpawnProtect(pl);
                        pl.SetRole(RoleType.NtfCaptain);
                        Timing.CallDelayed(0.5f, () =>
                        {
                            pl.ClearInventory();
                            pl.GetAmmo();
                            pl.AddItem(ItemType.KeycardFacilityManager);
                            pl.AddItem(ItemType.GunE11SR);
                            pl.AddItem(ItemType.Radio);
                            pl.AddItem(ItemType.GrenadeHE);
                            pl.AddItem(ItemType.SCP500);
                            pl.AddItem(ItemType.SCP500);
                            pl.AddItem(ItemType.Flashlight);
                            pl.AddItem(ItemType.ArmorHeavy);
                            pl.Tag += SweepTag;
                            pl.MaxHp = 200;
                            pl.Hp = 200;
                        });
                        pl.Broadcast($"<size=30%><color=#6f6f6f>Вы из <color=#ff0000>Группы зачистки</color> <color=#0047ec>МОГ</color>\n" +
                            "Ваша задача - зачистить комплекс, уничтожив всё живое в нем.\n(Кроме своих товарищей, конечно)</color></size>", 10, true);
                        pl.UnitName = "<color=#ff0000>Группа зачистки</color>";
                    }
                }
                IEnumerator<float> LightsOff()
                {
                    Lights.TurnOff(5);
                    yield return Timing.WaitForSeconds(1f);
                    Lights.ChangeColor(new Color(0, 0, 0));
                    yield return Timing.WaitForSeconds(1f);
                    while (Round.CurrentRound == round)
                    {
                        Lights.ChangeColor(new Color(0, 0, 0));
                        Lights.TurnOff(31);
                        yield return Timing.WaitForSeconds(30f);
                    }
                    yield break;
                }
                IEnumerator<float> CheckSwapGroup()
                {
                    yield return Timing.WaitForSeconds(1f);
                    while (Player.List.Where(x => x.Tag.Contains(SweepTag)).Count() > 2)
                        yield return Timing.WaitForSeconds(1f);
                    if (round != Round.CurrentRound) yield break;
                    yield return Timing.WaitForSeconds(300f);
                    bool spawnedO5 = false;
                    while (!spawnedO5)
                    {
                        if (round != Round.CurrentRound) yield break;
                        Timing.RunCoroutine(SpawnO5Group());
                        yield return Timing.WaitForSeconds(30f);
                    }
                    IEnumerator<float> SpawnO5Group()
                    {
                        if (Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).Count() == 0) yield break;
                        var TeamType = SpawnableTeamType.NineTailedFox;
                        RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, TeamType);
                        yield return Timing.WaitForSeconds(15f);
                        List<Player> list = Player.List.Where(x => x.Role == RoleType.Spectator && !x.Overwatch).ToList();
                        if (list.Count == 0) yield break;
                        list.Shuffle();
                        spawnedO5 = true;
                        Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#000000>Группа совета О5</color>");
                        foreach (Player pl in list) SpawnOne(pl);
                        Cassie.Send("ATTENTION TO ALL PERSONNEL . . a group of under the personal command of the o5 arrival at the complex");
                        RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, TeamType);
                        Timing.RunCoroutine(CheckSwapGroup());
                        void SpawnOne(Player pl)
                        {
                            SpawnManager.SpawnProtect(pl);
                            pl.SetRole(RoleType.NtfCaptain);
                            Timing.CallDelayed(0.5f, () =>
                            {
                                pl.ClearInventory();
                                pl.GetAmmo();
                                pl.AddItem(ItemType.KeycardFacilityManager);
                                pl.AddItem(ItemType.GunE11SR);
                                pl.AddItem(ItemType.Radio);
                                pl.AddItem(ItemType.GrenadeHE);
                                pl.AddItem(ItemType.SCP500);
                                pl.AddItem(ItemType.SCP500);
                                pl.AddItem(ItemType.GunRevolver);
                                pl.AddItem(ItemType.ArmorHeavy);
                                pl.MaxAhp = 500;
                                pl.MaxHp = 350;
                                pl.Ahp = 500;
                                pl.Hp = 350;
                            });
                            pl.Broadcast($"<size=30%><color=#6f6f6f>Вы из <color=#000000>Группы совета О5</color>\n" +
                                "Ваша задача - зачистить комплекс, уничтожив всё живое в нем.\n(Кроме своих товарищей, конечно)</color></size>", 10, true);
                            pl.UnitName = "<color=#000000>Группа совета О5</color>";
                        }
                    }
                }
            }
        }
        internal void SweepFixTag(DeadEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Target == null) return;
            if (!ev.Target.Tag.Contains(SweepTag)) return;
            ev.Target.Tag = ev.Target.Tag.Replace(SweepTag, "");
        }
        public void SweepFixTag(RoleChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player == null) return;
            if (ev.NewRole.GetTeam() == Team.MTF) return;
            if (!ev.Player.Tag.Contains(SweepTag)) return;
            ev.Player.Tag = ev.Player.Tag.Replace(SweepTag, "");
        }
        public void SweepFixTag(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player == null) return;
            if (ev.RoleType.GetTeam() == Team.MTF) return;
            if (!ev.Player.Tag.Contains(SweepTag)) return;
            ev.Player.Tag = ev.Player.Tag.Replace(SweepTag, "");
        }
    }
}