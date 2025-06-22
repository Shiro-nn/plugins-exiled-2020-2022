using Loli.Addons;
using MEC;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Loli.Scps
{
    static class Scp079Better
    {
        static Scp079Better()
        {
            CommandsSystem.RegisterConsole("079", Console);
        }

        [EventMethod(PlayerEvents.Spawn)]
        static void Spawn(SpawnEvent ev)
        {
            if (ev.Player.RoleInfomation.Role is RoleTypeId.Scp079)
                ev.Player.Client.Broadcast("<size=25%><color=#6f6f6f>Вы можете посмотреть способности " +
                "<color=red>SCP 079</color>, написав <color=#ffa300>.079</color> в консоли на " +
                "<color=#0089c7>[<color=#00ff00>ё</color>]</color>.</color></size>", 10, true);
        }

        static void Console(GameConsoleCommandEvent ev)
        {
            if (ev.Name != "079") return;
            ev.Allowed = false;
            if (ev.Args == null || ev.Args.Length == 0)
            {
                ZeroArgs();
                return;
            }
            if (!ev.Player.RoleInfomation.Scp079.IsWork)
            {
                ev.Reply = "Вы не являетесь SCP 079";
                ev.Color = "red";
                return;
            }
            switch (ev.Args[0])
            {
                case "1":
                    {
                        float mana = 15;
                        if (ev.Player.RoleInfomation.Scp079.Energy < mana)
                        {
                            ev.Player.Client.SendConsole($"Недостаточно энергии — {(int)ev.Player.RoleInfomation.Scp079.Energy}/{mana}", "red");
                            return;
                        }
                        var scps = Player.List.Where(x => x.RoleInfomation.Team == Team.SCPs && x.RoleInfomation.Role != RoleTypeId.Scp079 &&
                            ev.Player.GamePlay.Room.Cameras.Count > 0).ToList();
                        if (scps.Count == 0)
                        {
                            ev.Player.Client.SendConsole("SCP не найдены", "red");
                            return;
                        }
                        scps.Shuffle();
                        ev.Player.Client.SendConsole("Телепортация..", "green");
                        ev.Player.RoleInfomation.Scp079.Energy -= mana;
                        ev.Player.RoleInfomation.Scp079.Camera = scps[0].GamePlay.Room.Cameras.First();
                        break;
                    }
                case "2":
                    {
                        float mana = 30;
                        if (ev.Player.RoleInfomation.Scp079.Energy < mana)
                        {
                            ev.Player.Client.SendConsole($"Недостаточно энергии — {(int)ev.Player.RoleInfomation.Scp079.Energy}/{mana}", "red");
                            return;
                        }
                        ev.Player.Client.SendConsole("Выключение..", "green");
                        ev.Player.RoleInfomation.Scp079.Energy -= mana;
                        ev.Player.RoleInfomation.Scp079.Camera.Room.LightsOff(30);
                        break;
                    }
                case "3":
                    {
                        float mana = 100;
                        byte level = 2;
                        if (ev.Player.RoleInfomation.Scp079.Lvl < level)
                        {
                            ev.Player.Client.SendConsole($"Недостаточный уровень — {ev.Player.RoleInfomation.Scp079.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.RoleInfomation.Scp079.Energy < mana)
                        {
                            ev.Player.Client.SendConsole($"Недостаточно энергии — {(int)ev.Player.RoleInfomation.Scp079.Energy}/{mana}", "red");
                            return;
                        }
                        ev.Player.Client.SendConsole("Выключение..", "green");
                        ev.Player.RoleInfomation.Scp079.Energy -= mana;
                        GlobalLights.TurnOff(30);
                        break;
                    }
                case "4":
                    {
                        float mana = 40;
                        byte level = 2;
                        if (ev.Player.RoleInfomation.Scp079.Lvl < level)
                        {
                            ev.Player.Client.SendConsole($"Недостаточный уровень — {ev.Player.RoleInfomation.Scp079.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.RoleInfomation.Scp079.Energy < mana)
                        {
                            ev.Player.Client.SendConsole($"Недостаточно энергии — {(int)ev.Player.RoleInfomation.Scp079.Energy}/{mana}", "red");
                            return;
                        }
                        var room = ev.Player.RoleInfomation.Scp079.Camera.Room;
                        if (room.Lights.Override)
                        {
                            ev.Player.Client.SendConsole("В данной комнате уже изменен цвет освещения", "red");
                            return;
                        }
                        ev.Player.Client.SendConsole("Изменение..", "green");
                        ev.Player.RoleInfomation.Scp079.Energy -= mana;
                        room.Lights.Color = new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f);
                        Timing.CallDelayed(40f, () =>
                        {
                            room.Lights.Color = FlickerableLightController.DefaultWarheadColor;
                            room.Lights.Override = false;
                        });
                        break;
                    }
                case "5":
                    {
                        float mana = 20;
                        byte level = 1;
                        if (ev.Player.RoleInfomation.Scp079.Lvl < level)
                        {
                            ev.Player.Client.SendConsole($"Недостаточный уровень — {ev.Player.RoleInfomation.Scp079.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.RoleInfomation.Scp079.Energy < mana)
                        {
                            ev.Player.Client.SendConsole($"Недостаточно энергии — {(int)ev.Player.RoleInfomation.Scp079.Energy}/{mana}", "red");
                            return;
                        }
                        var room = ev.Player.RoleInfomation.Scp079.Camera.Room;
                        if (room.Lights.Override)
                        {
                            ev.Player.Client.SendConsole("В данной комнате уже изменен цвет освещения", "red");
                            return;
                        }
                        ev.Player.Client.SendConsole("Изменение..", "green");
                        ev.Player.RoleInfomation.Scp079.Energy -= mana;
                        byte to = (byte)Random.Range(0, 255);
                        room.Lights.Color = new Color32(to, to, to, 255);
                        Timing.CallDelayed(40f, () =>
                        {
                            room.Lights.Color = FlickerableLightController.DefaultWarheadColor;
                            room.Lights.Override = false;
                        });
                        break;
                    }
                case "6":
                    {
                        float mana = 75;
                        byte level = 3;
                        if (ev.Player.RoleInfomation.Scp079.Lvl < level)
                        {
                            ev.Player.Client.SendConsole($"Недостаточный уровень — {ev.Player.RoleInfomation.Scp079.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.RoleInfomation.Scp079.Energy < mana)
                        {
                            ev.Player.Client.SendConsole($"Недостаточно энергии — {(int)ev.Player.RoleInfomation.Scp079.Energy}/{mana}", "red");
                            return;
                        }
                        var room = ev.Player.RoleInfomation.Scp079.Camera.Room;
                        if (room is null || room.Type is RoomType.Pocket or RoomType.Surface)
                        {
                            ev.Player.Client.SendConsole("В данной местности отравление не работает", "red");
                            return;
                        }
                        double ls = Math.Round((DateTime.Now - LastGas).TotalSeconds);
                        if (ls < 45)
                        {
                            ev.Player.Client.SendConsole($"Отравить комнату можно раз в 45 секунд ({ls}/45)", "red");
                            return;
                        }
                        ev.Player.Client.SendConsole("Комната отравляется..", "green");
                        ev.Player.RoleInfomation.Scp079.Energy -= mana;
                        Timing.RunCoroutine(GasRoom(room, ev.Player));
                        break;
                    }
                default:
                    {
                        ZeroArgs();
                        break;
                    }
            }

            void ZeroArgs()
            {
                ev.Player.Client.SendConsole("Список способностей SCP 079:\n" +
                    ".079 1 - Телепортироваться к SCP; Требуется 15 энергии & 1 уровень\n" +
                    ".079 2 - Отключить свет в комнате; Требуется 30 энергии & 1 уровень\n" +
                    ".079 3 - Отключить свет в комплексе; Требуется 100 энергии & 2 уровень\n" +
                    ".079 4 - Изменить цвет в комнате; Требуется 40 энергии & 2 уровень\n" +
                    ".079 5 - Изменить яркость освещения в комнате; Требуется 20 энергии & 1 уровень\n" +
                    ".079 6 - Отравить людей в комнате; Требуется 60 энергии & 3 уровень", "red");
            }
        }
        static DateTime LastGas = DateTime.Now;
        static IEnumerator<float> GasRoom(Room room, Player scp)
        {
            LastGas = DateTime.Now;
            var doors = room.Doors;
            foreach (var door in doors)
            {
                if (!(door.Type is DoorType.HCZ_079_First or DoorType.HCZ_079_Second) || Round.ActiveGenerators > 2)
                {
                    door.Open = true;
                    door.Lock = true;
                }
            }
            room.Lights.Color = new Color32(255, 163, 0, 255);
            yield return Timing.WaitForSeconds(5f);
            foreach (var door in doors)
            {
                if (!(door.Type is DoorType.HCZ_079_First or DoorType.HCZ_079_Second) || Round.ActiveGenerators > 2)
                {
                    door.Open = false;
                    door.Lock = true;
                }
            }
            foreach (var pl in Player.List.Where(pl => pl.RoleInfomation.Team != Team.SCPs && pl.RoleInfomation.Team != Team.Dead &&
                pl.GamePlay.Room != null && pl.GamePlay.Room.Transform == room.Transform))
            {
                pl.Client.Broadcast(10, $"<size=25%><color=#6f6f6f>Данная комната <color=#ffa300>отравлена</color>.</color></size>", true);
                pl.Effects.Enable(EffectType.Asphyxiated, 20);
            }
            for (int i = 0; i < 25; i++)
            {
                foreach (var pl in Player.List.Where(pl => pl.RoleInfomation.Team != Team.SCPs && pl.RoleInfomation.Team != Team.Dead &&
                    pl.GamePlay.Room != null && pl.GamePlay.Room.Transform == room.Transform))
                {
                    pl.HealthInfomation.Damage(10, PlayerStatsSystem.DeathTranslations.Decontamination, scp);
                    if (pl.RoleInfomation.Role == RoleTypeId.Spectator) scp.RoleInfomation.Scp079.Exp += 35;
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
            foreach (var door in doors)
            {
                if (!(door.Type is DoorType.HCZ_079_First or DoorType.HCZ_079_Second) || Round.ActiveGenerators > 2)
                    door.Lock = false;
            }
            room.Lights.Color = Color.white;
            room.Lights.Override = false;
            yield break;
        }
    }
}