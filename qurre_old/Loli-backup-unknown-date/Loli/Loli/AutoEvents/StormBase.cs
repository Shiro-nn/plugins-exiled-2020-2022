using System.Collections.Generic;
using System.Linq;
using Loli.DataBase;
using MEC;
using Mirror;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Events;
using Respawning;
using RoundRestarting;
using UnityEngine;
namespace Loli.AutoEvents
{
    public class StormBase
    {
        internal readonly Main main;
        public static bool Enabled = false;
        internal Dictionary<string, RoleType> Role = new Dictionary<string, RoleType>();
        private int CI = 0;
        private bool RoundEnding = false;
        private bool CIWin = false;
        private int HidCount = 1;
        private int HidPlatformCount = 0;
        private bool MTFCall = false;
        private bool AgainMTFCall = false;
        private int dur = 6574356;
        public StormBase(Main main) => this.main = main;
        internal void Elevator(InteractLiftEvent ev)
        {
            ev.Allowed = false;
        }
        internal void Enable()
        {
            Enabled = true;
            GameCore.Console.singleton.TypeCommand($"/mapeditor load event", new GameCoreSender("MapEditor"));
            CI = 0;
            RoundEnding = false;
            CIWin = false;
            HidCount = 1;
            HidPlatformCount = 0;
            MTFCall = false;
            AgainMTFCall = false;
            RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, SpawnableTeamType.NineTailedFox);
            for (int i = -5; i > -24; i -= 2) new Item(ItemType.Flashlight).Spawn(new Vector3(i, 1016, -66));
            for (int i = -5; i > -24; i -= 2) new Item(ItemType.Flashlight).Spawn(new Vector3(i, 1016, -67));
            for (int i = -5; i > -18; i -= 2) new Item(ItemType.Flashlight).Spawn(new Vector3(i, 1009, -41));
            double nn = Player.List.Count() / 5;
            HidCount = System.Convert.ToInt32(System.Math.Round(nn));
            int x = -15;
            for (int i = 0; i < HidCount; i++)
            {
                var mh = new MicroHid(ItemType.MicroHID)
                {
                    Energy = dur
                };
                mh.Spawn(new Vector3(x, 1017, -68));
                x -= 2;
            }
            Timing.CallDelayed(1f, () =>
            {
                Map.Broadcast($"<size=25%><color=#6f6f6f>Совет О5 сообщает.</color>\n<color=#0089c7>Было украдено <color=red>{HidCount}</color> MicroHID'ов.</color></size>", 10, true);
            });
        }
        internal void Spawned(SpawnEvent ev)
        {
            ev.Player.Tag = "";
            if (!Role.ContainsKey(ev.Player.UserId))
            {
                if (CI * 3 < Player.List.Count())
                {
                    Role.Add(ev.Player.UserId, RoleType.ChaosRepressor);
                    CI++;
                    ev.Player.Role = RoleType.ChaosRepressor;
                }
                else
                {
                    Role.Add(ev.Player.UserId, RoleType.NtfSergeant);
                    ev.Player.Role = RoleType.NtfSergeant;
                }
            }
            else
            {
                if (ev.RoleType != RoleType.NtfSergeant && ev.RoleType != RoleType.ChaosRepressor)
                {
                    ev.Player.Role = Role[ev.Player.UserId];
                }
                else
                {
                    Timing.CallDelayed(0.5f, () =>
                    {
                        ev.Player.ClearInventory();
                        ev.Player.GetAmmo();
                        ev.Player.AddItem(ItemType.GunE11SR);
                        ev.Player.AddItem(ItemType.GunFSP9);
                        ev.Player.AddItem(ItemType.GrenadeFlash);
                        ev.Player.AddItem(ItemType.SCP207);
                        ev.Player.AddItem(ItemType.Adrenaline);
                        ev.Player.AddItem(ItemType.Medkit);
                        ev.Player.AddItem(ItemType.SCP500);
                        ev.Player.AddItem(ItemType.Flashlight);
                        if (ev.RoleType == RoleType.ChaosRepressor) ev.Player.Position = new Vector3(-16, 1010, -42);
                        else ev.Player.Position = Map.GetRandomSpawnPoint(RoleType.NtfSergeant) + Vector3.up;
                        ev.Player.UnitName = "#fydne";
                    });
                }
            }
        }
        internal void Left(LeaveEvent ev)
        {
            if (Role.ContainsKey(ev.Player.UserId))
            {
                if (Role[ev.Player.UserId] == RoleType.ChaosRepressor) CI--;
                Role.Remove(ev.Player.UserId);
            }
        }
        internal void Join(JoinEvent ev)
        {
            Timing.CallDelayed(1f, () =>
            {
                if (!Role.ContainsKey(ev.Player.UserId))
                {
                    if (CI * 3 < Player.List.Count())
                    {
                        Role.Add(ev.Player.UserId, RoleType.ChaosRepressor);
                        CI++;
                        ev.Player.Role = RoleType.ChaosRepressor;
                    }
                    else
                    {
                        Role.Add(ev.Player.UserId, RoleType.NtfSergeant);
                        ev.Player.Role = RoleType.NtfSergeant;
                    }
                }
                else
                {
                    ev.Player.Role = Role[ev.Player.UserId];
                }
                ev.Player.Broadcast(15, "<size=30%><color=#0089c7>Сейчас запущен ивент <color=red>Штурм базы Хаоса</color>.</color>\n" +
                    "<color=#494f61>Подробности в консоли на <color=lime>[<color=aqua>ё</color>]</color>.</color></size>");
                ev.Player.SendConsoleMessage("\nШтурм базы Хаоса\nХаос забрал из комплекса MicroHID'ы, совет о5 дал приказ: *вернуть все MicroHID'ы*.\n" +
                    "Задача мог - забрать все MicroHID'ы с базы хаоса.\nЗадача хаоса - защитить MicroHID'ы.\n" +
                    "MicroHID'ы могут подбирать и мог, и хаос. Если кто-то подобрал MicroHID, то об этом узнают другие, поэтому будьте осторожны.", "red");
            });
        }
        internal void Pickup(PickupItemEvent ev)
        {
            if (ev.Pickup.Type == ItemType.Flashlight)
            {
                ev.Allowed = false;
                return;
            }
            if (ev.Pickup.Type != ItemType.MicroHID)
            {
                return;
            }
            string PickRole = "<color=#000001>Неизвестна</color>";
            if (ev.Player.Role == RoleType.ChaosRepressor) PickRole = "<color=green>Хаос</color>";
            else if (ev.Player.Role == RoleType.NtfSergeant) PickRole = "<color=#006dff>Мтф</color>";
            Map.Broadcast($"<size=30%><color=#ff0000><color=#0089c7>{ev.Player.Nickname}</color> подобрал MicroHID.</color>\n<color=#fdffbb>Его роль: {PickRole}</color>.</size>", 3);
            ev.Player.UnitName = "<color=red>Несёт MicroHID</color>";
            ev.Player.Tag = "HidOwner";
            Vector3 s1 = new Vector3(180, 0, -57);
            Vector3 s2 = new Vector3(186, 1111, -53);
            if (ev.Pickup.Position.x > s1.x &&
                ev.Pickup.Position.y > s1.y &&
                ev.Pickup.Position.z > s1.z &&
                ev.Pickup.Position.x < s2.x &&
                ev.Pickup.Position.y < s2.y &&
                ev.Pickup.Position.z < s2.z)
            {
                HidPlatformCount--;
                Map.Broadcast($"<size=30%><color=#ffb500><color=#0089c7>{ev.Player.Nickname}</color>({PickRole}) забрал MicroHID с базы <color=#000fff>MTF</color>.</color></size>", 15, true);
            }
        }
        internal void Drop(DropItemEvent ev)
        {
            if (ev.Item.Type == ItemType.MicroHID)
            {
                string PickRole = "<color=#000001>Неизвестна</color>";
                if (ev.Player.Role == RoleType.ChaosRepressor) PickRole = "<color=green>Хаос</color>";
                else if (ev.Player.Role == RoleType.NtfSergeant) PickRole = "<color=#006dff>Мтф</color>";
                Map.Broadcast($"<size=30%><color=#00ff88><color=#0089c7>{ev.Player.Nickname}</color> выкинул MicroHID.</color>\n<color=#fdffbb>Его роль: {PickRole}</color>.</size>", 3);
                ev.Player.UnitName = "#fydne";
                ev.Player.Tag = "";
                Vector3 s1 = new Vector3(180, 0, -57);
                Vector3 s2 = new Vector3(186, 1111, -53);
                if (ev.Player.Position.x > s1.x &&
                    ev.Player.Position.y > s1.y &&
                    ev.Player.Position.z > s1.z &&
                    ev.Player.Position.x < s2.x &&
                    ev.Player.Position.y < s2.y &&
                    ev.Player.Position.z < s2.z)
                {
                    HidPlatformCount++;
                    if (!MTFCall)
                    {
                        AgainMTFCall = false;
                        Map.Broadcast($"<size=30%><color=#fdffbb><color=#0089c7>{ev.Player.Nickname}</color>({PickRole}) доставил MicroHID на базу <color=#000fff>MTF</color>.</color>\n<color=#6f6f6f>Совет О5 заберет его через <color=red>15 секунд</color>.</color></size>", 15, true);
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
                        Map.Broadcast($"<size=30%><color=#fdffbb><color=#0089c7>{ev.Player.Nickname}</color>({PickRole}) доставил MicroHID на базу <color=#000fff>MTF</color>.</color>\n<color=#6f6f6f>Совет О5 скоро заберет его.</color></size>", 15, true);
                    }
                }
            }
        }
        internal void Dead(DeadEvent ev)
        {
            Vector3 pos = ev.Target.Position;
            if (ev.Target.Tag.Contains("HidOwner"))
                foreach (Pickup item in Map.Pickups.Where(x => x.Type == ItemType.MicroHID))
                    Drop(new DropItemEvent(ev.Target, item));
            ev.Target.Role = Role[ev.Target.UserId];
        }
        internal void Ending(CheckEvent ev)
        {
            if (RoundEnding)
            {
                ev.RoundEnd = true;
                Map.Broadcast("<size=30%><color=#fdffbb>Победители ивента получают <color=red>500 опыта</color> & <color=red>50 монет</color>.</color></size>", 15);
                if (CIWin)
                {
                    ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
                    foreach (Player pl in Player.List.Where(x => x.Role == RoleType.ChaosRepressor))
                    {
                        Manager.Static.Stats.Add(pl, 500, 50);
                        Levels.SetPrefix(pl);
                    }
                }
                else
                {
                    ev.LeadingTeam = LeadingTeam.FacilityForces;
                    foreach (Player pl in Player.List.Where(x => x.Role == RoleType.NtfSergeant))
                    {
                        Manager.Static.Stats.Add(pl, 500, 50);
                        Levels.SetPrefix(pl);
                    }
                }
                Enabled = false;
                main.UnRegisterStormBase();
                Round.End();
                Timing.CallDelayed(30f, () =>
                {
                    foreach (Player pl in Player.List) pl.DimScreen();
                    Timing.CallDelayed(1f, () => NetworkServer.SendToAll(new RoundRestartMessage(RoundRestartType.FullRestart, 25, 0, true, true)));
                    Timing.CallDelayed(4.9f, () => Server.Restart());
                });
            }
            else
            {
                ev.RoundEnd = false;
                if (Round.ElapsedTime.TotalMinutes >= 10)
                {
                    CIWin = true;
                    RoundEnding = true;
                }
                foreach (Player pl in Player.List.Where(x => x.Role == RoleType.Spectator))
                    pl.ReferenceHub.characterClassManager.SetClassIDAdv(RoleType.NtfSergeant, false, CharacterClassManager.SpawnReason.Died);
                foreach (Pickup item in Map.Pickups.Where(x => x.Type != ItemType.Flashlight && x.Type != ItemType.MicroHID))
                    item.Destroy();
                foreach (Ragdoll doll in Object.FindObjectsOfType<Ragdoll>())
                    Mirror.NetworkServer.Destroy(doll.gameObject);
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
                foreach (Pickup item in Map.Pickups.Where(x =>
                x.Position.x > s1.x && x.Position.y > s1.y && x.Position.z > s1.z &&
                x.Position.x < s2.x && x.Position.y < s2.y && x.Position.z < s2.z &&
                x.Type == ItemType.MicroHID))
                {
                    item.Destroy();
                    HidBack++;
                    HidCount--;
                    HidPlatformCount--;
                }
                Timing.CallDelayed(0.5f, () =>
                {
                    if (HidBack > 0)
                    {
                        Map.Broadcast($"<size=30%><color=#6f6f6f>Совет О5 успешно забрал <color=red>{HidBack}</color> <color=#006dff>MicroHID(а/ов)</color></color></size>", 15);
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