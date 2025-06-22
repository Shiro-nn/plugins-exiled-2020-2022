using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;
using UnityEngine;

namespace MongoDB.auto_events
{
    public class storm_base
    {
        internal readonly Main main;
        public static bool Enabled = false;
        private bool TextureLoaded = false;
        internal Dictionary<string, RoleType> Role = new Dictionary<string, RoleType>();
        private int CI = 0;
        private bool RoundEnding = false;
        private bool CIWin = false;
        private string HidPlayer = "";
        private bool Reload = false;
        private int HidCount = 1;
        private int HidPlatformCount = 0;
        private bool MTFCall = false;
        private bool AgainMTFCall = false;
        private int dur = 6574356;
        public storm_base(Main main) => this.main = main;
        internal void Elevator(InteractingElevatorEventArgs ev)
        {
            if (Enabled) ev.IsAllowed = false;
        }
        internal void Waiting()
        {
            if (Enabled)
            {
                GameCore.Console.singleton.TypeCommand($"/mapeditor load event");
                TextureLoaded = true;
                Role.Clear();
                CI = 0;
                RoundEnding = false;
                CIWin = false;
                HidPlayer = "";
                Reload = false;
                HidCount = 1;
                HidPlatformCount = 0;
                MTFCall = false;
                AgainMTFCall = false;
            }
        }
        internal void Started()
        {
            if (Enabled)
            {
                if (!TextureLoaded)
                {
                    GameCore.Console.singleton.TypeCommand($"/mapeditor load event");
                    TextureLoaded = true;
                    Role.Clear();
                    CI = 0;
                    RoundEnding = false;
                    CIWin = false;
                    HidPlayer = "";
                    Reload = false;
                    HidCount = 1;
                    HidPlatformCount = 0;
                    MTFCall = false;
                    AgainMTFCall = false;
                }
                Timing.CallDelayed(1f, () =>
                {
                    foreach (Player pl in Player.List)
                        pl.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.Tutorial, false);
                });
                RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, SpawnableTeamType.NineTailedFox);
                for (int i = -5; i > -24; i -= 2)
                {
                    Pickup p = Extensions.SpawnItem(ItemType.Flashlight, dur, new Vector3(i, 1016, -66));
                    p.transform.rotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f);
                }
                for (int i = -5; i > -24; i -= 2)
                {
                    Pickup p = Extensions.SpawnItem(ItemType.Flashlight, dur, new Vector3(i, 1016, -67));
                    p.transform.rotation = new Quaternion(-0.5f, 0.5f, -0.5f, -0.5f);
                }
                for (int i = -5; i > -18; i -= 2)
                {
                    Pickup p = Extensions.SpawnItem(ItemType.Flashlight, dur, new Vector3(i, 1009, -41));
                    p.transform.rotation = new Quaternion(-0.5f, 0.5f, -0.5f, -0.5f);
                }
                double nn = Player.List.Count() / 5;
                HidCount = System.Convert.ToInt32(System.Math.Round(nn));
                int x = -15;
                for (int i = 0; i < HidCount; i++)
                {
                    Extensions.SpawnItem(ItemType.MicroHID, dur, new Vector3(x, 1017, -68));
                    x -= 2;
                }
                Timing.CallDelayed(1f, () =>
                {
                    Map.ClearBroadcasts();
                    Map.Broadcast(10, $"<size=25%><color=#6f6f6f>Совет о5 сообщает.</color>\n<color=#0089c7>Было украдено <color=red>{HidCount}</color> пылесосов.</color></size>");
                });
            }
        }
        internal void Spawned(SpawningEventArgs ev)
        {
            if (Enabled)
            {
                if (!Role.ContainsKey(ev.Player.UserId))
                {
                    if (CI * 3 < Player.List.Count())
                    {
                        Role.Add(ev.Player.UserId, RoleType.ChaosInsurgency);
                        CI++;
                        ev.Player.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.ChaosInsurgency, false);
                    }
                    else
                    {
                        Role.Add(ev.Player.UserId, RoleType.NtfLieutenant);
                        ev.Player.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.NtfLieutenant, false);
                    }
                }
                else
                {
                    if (ev.RoleType != RoleType.NtfLieutenant && ev.RoleType != RoleType.ChaosInsurgency)
                    {
                        ev.Player.ReferenceHub.characterClassManager.SetClassIDAdv(Role[ev.Player.UserId], false);
                    }
                    else
                    {
                        Timing.CallDelayed(0.5f, () =>
                        {
                            ev.Player.ClearInventory();
                            ev.Player.ReferenceHub.ammoBox.ResetAmmo();
                            ev.Player.AddItem(ItemType.GunE11SR);
                            ev.Player.AddItem(ItemType.GunMP7);
                            ev.Player.AddItem(ItemType.GrenadeFlash);
                            ev.Player.AddItem(ItemType.SCP207);
                            ev.Player.AddItem(ItemType.Adrenaline);
                            ev.Player.AddItem(ItemType.Medkit);
                            ev.Player.AddItem(ItemType.SCP500);
                            ev.Player.AddItem(ItemType.Flashlight);
                            if (ev.RoleType == RoleType.ChaosInsurgency) ev.Player.Position = new Vector3(-16, 1010, -42);
                            else ev.Player.Position = Map.GetRandomSpawnPoint(RoleType.NtfLieutenant) + Vector3.up;
                            ev.Player.ReferenceHub.characterClassManager.NetworkCurUnitName = "#fydne";
                        });
                    }
                }
            }
        }
        internal void Left(LeftEventArgs ev)
        {
            if (Enabled)
            {
                if (Role.ContainsKey(ev.Player.UserId))
                {
                    if (Role[ev.Player.UserId] == RoleType.ChaosInsurgency) CI--;
                    Role.Remove(ev.Player.UserId);
                }
            }
        }
        internal void Join(JoinedEventArgs ev)
        {
            if (Enabled)
            {
                if (!Role.ContainsKey(ev.Player.UserId))
                {
                    if (CI * 3 < Player.List.Count())
                    {
                        Role.Add(ev.Player.UserId, RoleType.ChaosInsurgency);
                        CI++;
                        ev.Player.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.ChaosInsurgency, false);
                    }
                    else
                    {
                        Role.Add(ev.Player.UserId, RoleType.NtfLieutenant);
                        ev.Player.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.NtfLieutenant, false);
                    }
                }
                else
                {
                    ev.Player.ReferenceHub.characterClassManager.SetClassIDAdv(Role[ev.Player.UserId], false);
                }
                ev.Player.Broadcast(15, "<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Штурм базы Хаоса</color>.</color>\n<color=#494f61>Подробности в консоли на <color=lime>[<color=aqua>ё</color>]</color>.</color></size>");
                ev.Player.SendConsoleMessage("\nШтурм базы Хаоса\nХаос забрал из комплекса пылесос, совет о5 дал приказ: *вернуть пылесос*.\nЗадача мог - забрать пылесос из базы хаоса.\nЗадача хаоса - защитить пылесос.\nПылесос могут подбирать и мог, и хаос. Если кто-то подобрал пылесос, то об этом узнают другие, поэтому будьте осторожны.", "red");
            }
        }
        internal void Pickup(PickingUpItemEventArgs ev)
        {
            if (Enabled)
            {
                //if (ev.Player.Nickname == "hmm")  ev.Player.Broadcast(1, $"{ev.Pickup.rotation.x}, {ev.Pickup.rotation.y}, {ev.Pickup.rotation.z}, {ev.Pickup.rotation.w}");
                if (ev.Pickup.durability == dur)
                {
                    if (ev.Pickup.itemId == ItemType.Flashlight)
                    {
                        ev.IsAllowed = false;
                        return;
                    }
                    string PickRole = "<color=#000001>Неизвестна</color>";
                    if (ev.Player.Role == RoleType.ChaosInsurgency) PickRole = "<color=green>Хаос</color>";
                    else if (ev.Player.Role == RoleType.NtfLieutenant) PickRole = "<color=#006dff>Мтф</color>";
                    Map.Broadcast(3, $"<size=30%><color=#ff0000><color=#0089c7>{ev.Player.Nickname}</color> подобрал пылесос.</color>\n<color=#fdffbb>Его роль: {PickRole}</color>.</size>");
                    ev.Player.ReferenceHub.characterClassManager.NetworkCurUnitName = "<color=red>Несёт пылесос</color>";
                    HidPlayer = ev.Player.UserId;
                    Vector3 s1 = new Vector3(180, 0, -57);
                    Vector3 s2 = new Vector3(186, 1111, -53);
                    if (ev.Pickup.position.x > s1.x &&
                        ev.Pickup.position.y > s1.y &&
                        ev.Pickup.position.z > s1.z &&
                        ev.Pickup.position.x < s2.x &&
                        ev.Pickup.position.y < s2.y &&
                        ev.Pickup.position.z < s2.z)
                    {
                        HidPlatformCount--;
                        Map.ClearBroadcasts();
                        Map.Broadcast(15, $"<size=30%><color=#ffb500><color=#0089c7>{ev.Player.Nickname}</color>({PickRole}) забрал пылесос с базы <color=#000fff>MTF</color>.</color></size>");
                    }
                }
            }
        }
        internal void Drop(ItemDroppedEventArgs ev)
        {
            if (Enabled)
            {
                if (ev.Pickup.ItemId == ItemType.MicroHID && ev.Pickup.durability != dur && ev.Player.UserId == HidPlayer) ev.Pickup.durability = dur;
                if (ev.Pickup.durability == dur)
                {
                    string PickRole = "<color=#000001>Неизвестна</color>";
                    if (ev.Player.Role == RoleType.ChaosInsurgency) PickRole = "<color=green>Хаос</color>";
                    else if (ev.Player.Role == RoleType.NtfLieutenant) PickRole = "<color=#006dff>Мтф</color>";
                    Map.Broadcast(3, $"<size=30%><color=#00ff88><color=#0089c7>{ev.Player.Nickname}</color> выкинул пылесос.</color>\n<color=#fdffbb>Его роль: {PickRole}</color>.</size>");
                    ev.Player.ReferenceHub.characterClassManager.NetworkCurUnitName = "#fydne";
                    HidPlayer = "";
                    Vector3 s1 = new Vector3(180, 0, -57);
                    Vector3 s2 = new Vector3(186, 1111, -53);
                    if (ev.Pickup.position.x > s1.x &&
                        ev.Pickup.position.y > s1.y &&
                        ev.Pickup.position.z > s1.z &&
                        ev.Pickup.position.x < s2.x &&
                        ev.Pickup.position.y < s2.y &&
                        ev.Pickup.position.z < s2.z)
                    {
                        HidPlatformCount++;
                        if (!MTFCall)
                        {
                            AgainMTFCall = false;
                            Map.ClearBroadcasts();
                            Map.Broadcast(15, $"<size=30%><color=#fdffbb><color=#0089c7>{ev.Player.Nickname}</color>({PickRole}) доставил пылесос на базу <color=#000fff>MTF</color>.</color>\n<color=#6f6f6f>Совет о5 заберет его через <color=red>15 секунд</color>.</color></size>");
                            SpawnVert();
                        }
                        else
                        {
                            Timing.CallDelayed(15f, () =>
                            {
                                if (AgainMTFCall && !MTFCall)
                                {
                                    if (HidPlatformCount > 0) SpawnVert();
                                    AgainMTFCall = false;
                                }
                            });
                            Map.ClearBroadcasts();
                            Map.Broadcast(15, $"<size=30%><color=#fdffbb><color=#0089c7>{ev.Player.Nickname}</color>({PickRole}) доставил пылесос на базу <color=#000fff>MTF</color>.</color>\n<color=#6f6f6f>Совет о5 скоро заберет его.</color></size>");
                        }
                    }
                }
            }
        }
        internal void Dead(DiedEventArgs ev)
        {
            if (Enabled)
            {
                Vector3 pos = ev.Target.Position;
                string UI = ev.Target.UserId;
                Player.Get(UI).ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.Tutorial, false);
                if (UI == HidPlayer)
                    foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.itemId == ItemType.MicroHID))
                        Drop(new ItemDroppedEventArgs(Player.Get(UI), item));
            }
        }
        internal void Ending(EndingRoundEventArgs ev)
        {
            if (Enabled)
            {
                if (RoundEnding)
                {
                    ev.IsAllowed = true;
                    ev.IsRoundEnded = true;
                    Map.Broadcast(15, "<size=30%><color=#fdffbb>Победители ивента получают <color=red>500 опыта</color> и <color=red>50 монет</color>.</color></size>");
                    if (CIWin)
                    {
                        ev.LeadingTeam = Exiled.API.Enums.LeadingTeam.ChaosInsurgency;
                        foreach (Player pl in Player.List.Where(x => x.Role == RoleType.ChaosInsurgency))
                        {
                            main.plugin.donate.main[pl.UserId].xp += 500;
                            main.plugin.donate.main[pl.UserId].money += 50;
                            main.plugin.donate.AddXP(pl.ReferenceHub);
                        }
                    }
                    else
                    {
                        ev.LeadingTeam = Exiled.API.Enums.LeadingTeam.FacilityForces;
                        foreach (Player pl in Player.List.Where(x => x.Role == RoleType.NtfLieutenant))
                        {
                            main.plugin.donate.main[pl.UserId].xp += 500;
                            main.plugin.donate.main[pl.UserId].money += 50;
                            main.plugin.donate.AddXP(pl.ReferenceHub);
                        }
                    }
                    Enabled = false;
                    TextureLoaded = false;
                    Role.Clear();
                    CI = 0;
                    RoundEnding = false;
                    CIWin = false;
                    HidPlayer = "";
                    Reload = false;
                    HidCount = 1;
                    HidPlatformCount = 0;
                    MTFCall = false;
                    AgainMTFCall = false;
                }
                else
                {
                    ev.IsAllowed = false;
                    if (Round.ElapsedTime.TotalMinutes >= 10)
                    {
                        CIWin = true;
                        RoundEnding = true;
                    }
                    foreach (Player pl in Player.List.Where(x => x.Role == RoleType.Spectator))
                        pl.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.Tutorial, false);
                    foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.durability != dur && x.itemId != ItemType.Flashlight && x.itemId != ItemType.MicroHID))
                        item.Delete();
                    foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
                        Mirror.NetworkServer.Destroy(doll.gameObject);
                }
            }
        }
        internal void Refresh()
        {
            if (!Round.IsStarted && Enabled)
            {
                if (5 >= GameCore.RoundStart.singleton.NetworkTimer && !Reload)
                {
                    List<Player> data = new List<Player>();
                    foreach (var s in Player.List)
                    {
                        int j = Extensions.Random.Next(data.Count + 1);
                        if (j == data.Count)
                        {
                            data.Add(s);
                        }
                        else
                        {
                            data.Add(data[j]);
                            data[j] = s;
                        }
                    }
                    foreach (Player pl in data)
                    {
                        pl.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.Tutorial, false);
                        Timing.CallDelayed(0.8f, () =>
                        {
                            if (main.plugin.EventHandlers.randomspawn < 25) pl.ReferenceHub.SetPosition(new Vector3(181, 991, 29));
                            else if (main.plugin.EventHandlers.randomspawn < 50) pl.ReferenceHub.SetPosition(new Vector3(152, 1019.5f, -17));
                            else if (main.plugin.EventHandlers.randomspawn < 75) pl.ReferenceHub.SetPosition(new Vector3(220, 1027, -18));
                            else pl.ReferenceHub.SetPosition(new Vector3(187, 998, -3));
                        });
                    }
                    Reload = true;
                }
            }
        }
        internal void SpawnVert()
        {
            Vector3 s1 = new Vector3(180, 0, -57);
            Vector3 s2 = new Vector3(186, 1111, -53);
            RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
            MTFCall = true;
            Timing.CallDelayed(15f, () =>
            {
                AgainMTFCall = true;
                int HidBack = 0;
                foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x =>
                x.position.x > s1.x && x.position.y > s1.y && x.position.z > s1.z &&
                x.position.x < s2.x && x.position.y < s2.y && x.position.z < s2.z &&
                x.durability == dur))
                {
                    item.Delete();
                    HidBack++;
                    HidCount--;
                    HidPlatformCount--;
                }
                Timing.CallDelayed(0.5f, () =>
                {
                    if (HidBack > 0)
                    {
                        Map.Broadcast(15, $"<size=30%><color=#6f6f6f>Совет о5 успешно забрал <color=red>{HidBack}</color> <color=#006dff>пылесосов</color></color></size>");
                        if (HidCount == 0)
                        {
                            CIWin = false;
                            RoundEnding = true;
                        }
                    }
                });
                Timing.CallDelayed(15f, () =>
                {
                    MTFCall = false;
                    AgainMTFCall = false;
                });
            });
        }
    }
}