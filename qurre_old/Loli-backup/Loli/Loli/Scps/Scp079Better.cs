using Loli.Addons;
using Loli.Addons.RolePlay.Roles;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Loli.Scps
{
    public class Scp079Better
    {
        internal Scp079Better()
        {
            CommandsSystem.RegisterConsole("079", Console);
        }
        internal void Spawn(SpawnEvent ev)
        {
            if (ev.Player.Role is RoleType.Scp079) ev.Player.Broadcast("<size=25%><color=#6f6f6f>Вы можете посмотреть способности " +
                "<color=red>SCP 079</color>, написав <color=#ffa300>.079</color> в консоли на " +
                "<color=#0089c7>[<color=#00ff00>ё</color>]</color>.</color></size>", 10, true);
        }
        internal void Console(SendingConsoleEvent ev)
        {
            if (ev.Name != "079") return;
            ev.Allowed = false;
            if (ev.Args == null || ev.Args.Length == 0)
            {
                ZeroArgs();
                return;
            }
            if (!ev.Player.Scp079Controller.Is079)
            {
                ev.ReturnMessage = "Вы не являетесь SCP 079";
                ev.Color = "red";
                return;
            }
            switch (ev.Args[0])
            {
                case "1":
                    {
                        float mana = 15;
                        if (ev.Player.Scp079Controller.Energy < mana)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточно энергии — {(int)ev.Player.Scp079Controller.Energy}/{mana}", "red");
                            return;
                        }
                        var scps = Player.List.Where(x => x.Team == Team.SCP && x.Role != RoleType.Scp079 && ev.Player.Room.Cameras.Count > 0).ToList();
                        if (scps.Count == 0)
                        {
                            ev.Player.SendConsoleMessage("SCP не найдены", "red");
                            return;
                        }
                        scps.Shuffle();
                        ev.Player.SendConsoleMessage("Телепортация..", "green");
                        ev.Player.Scp079Controller.Energy -= mana;
                        ev.Player.Scp079Controller.Camera = scps[0].Room.Cameras.First();
                        break;
                    }
                case "2":
                    {
                        float mana = 30;
                        if (ev.Player.Scp079Controller.Energy < mana)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточно энергии — {(int)ev.Player.Scp079Controller.Energy}/{mana}", "red");
                            return;
                        }
                        ev.Player.SendConsoleMessage("Выключение..", "green");
                        ev.Player.Scp079Controller.Energy -= mana;
                        ev.Player.Scp079Controller.Camera.Room.LightsOff(30);
                        break;
                    }
                case "3":
                    {
                        float mana = 100;
                        byte level = 2;
                        if (ev.Player.Scp079Controller.Lvl < level)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточный уровень — {ev.Player.Scp079Controller.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.Scp079Controller.Energy < mana)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточно энергии — {(int)ev.Player.Scp079Controller.Energy}/{mana}", "red");
                            return;
                        }
                        ev.Player.SendConsoleMessage("Выключение..", "green");
                        ev.Player.Scp079Controller.Energy -= mana;
                        GlobalLights.TurnOff(30);
                        break;
                    }
                case "4":
                    {
                        float mana = 40;
                        byte level = 2;
                        if (ev.Player.Scp079Controller.Lvl < level)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточный уровень — {ev.Player.Scp079Controller.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.Scp079Controller.Energy < mana)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточно энергии — {(int)ev.Player.Scp079Controller.Energy}/{mana}", "red");
                            return;
                        }
                        var room = ev.Player.Scp079Controller.Camera.Room;
                        if (room.Lights.Override)
                        {
                            ev.Player.SendConsoleMessage("В данной комнате уже изменен цвет освещения", "red");
                            return;
                        }
                        ev.Player.SendConsoleMessage("Изменение..", "green");
                        ev.Player.Scp079Controller.Energy -= mana;
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
                        if (ev.Player.Scp079Controller.Lvl < level)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточный уровень — {ev.Player.Scp079Controller.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.Scp079Controller.Energy < mana)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточно энергии — {(int)ev.Player.Scp079Controller.Energy}/{mana}", "red");
                            return;
                        }
                        var room = ev.Player.Scp079Controller.Camera.Room;
                        if (room.Lights.Override)
                        {
                            ev.Player.SendConsoleMessage("В данной комнате уже изменен цвет освещения", "red");
                            return;
                        }
                        ev.Player.SendConsoleMessage("Изменение..", "green");
                        ev.Player.Scp079Controller.Energy -= mana;
                        var to = Random.Range(0, 255);
                        room.Lights.Color = new Color(to, to, to);
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
                        if (ev.Player.Scp079Controller.Lvl < level)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточный уровень — {ev.Player.Scp079Controller.Lvl}/{level}", "red");
                            return;
                        }
                        if (ev.Player.Scp079Controller.Energy < mana)
                        {
                            ev.Player.SendConsoleMessage($"Недостаточно энергии — {(int)ev.Player.Scp079Controller.Energy}/{mana}", "red");
                            return;
                        }
                        if (ev.Player.Tag.Contains(SafeSystem.Tag))
                        {
                            ev.Player.SendConsoleMessage("Отравление недоступно для Системы Безопасности", "red");
                            return;
                        }
                        var room = ev.Player.Scp079Controller.Camera.Room;
                        if (room is null || room.Type is RoomType.Pocket or RoomType.Surface)
                        {
                            ev.Player.SendConsoleMessage("В данной местности отравление не работает", "red");
                            return;
                        }
                        double ls = Math.Round((DateTime.Now - LastGas).TotalSeconds);
                        if (ls < 45)
                        {
                            ev.Player.SendConsoleMessage($"Отравить комнату можно раз в 45 секунд ({ls}/45)", "red");
                            return;
                        }
                        ev.Player.SendConsoleMessage("Комната отравляется..", "green");
                        ev.Player.Scp079Controller.Energy -= mana;
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
                ev.Player.SendConsoleMessage("Список способностей SCP 079:\n" +
                    ".079 1 - Телепортироваться к SCP; Требуется 15 энергии & 1 уровень\n" +
                    ".079 2 - Отключить свет в комнате; Требуется 30 энергии & 1 уровень\n" +
                    ".079 3 - Отключить свет в комплексе; Требуется 100 энергии & 2 уровень\n" +
                    ".079 4 - Изменить цвет в комнате; Требуется 40 энергии & 2 уровень\n" +
                    ".079 5 - Изменить яркость освещения в комнате; Требуется 20 энергии & 1 уровень\n" +
                    ".079 6 - Отравить людей в комнате; Требуется 60 энергии & 3 уровень", "red");
            }
        }
        private DateTime LastGas = DateTime.Now;
        private IEnumerator<float> GasRoom(Room room, Player scp)
        {
            LastGas = DateTime.Now;
            var doors = room.Doors;
            foreach (var door in doors)
            {
                if (!(door.Type is DoorType.HCZ_079_First or DoorType.HCZ_079_Second) || Round.ActiveGenerators > 2)
                {
                    door.Open = true;
                    door.Locked = true;
                }
            }
            room.Lights.Color = new Color32(255, 163, 0, 255);
            yield return Timing.WaitForSeconds(5f);
            foreach (var door in doors)
            {
                if (!(door.Type is DoorType.HCZ_079_First or DoorType.HCZ_079_Second) || Round.ActiveGenerators > 2)
                {
                    door.Open = false;
                    door.Locked = true;
                }
            }
            foreach (var pl in Player.List.Where(pl => pl.Team != Team.SCP && pl.Team != Team.RIP && pl.Room != null && pl.Room.Transform == room.Transform))
            {
                pl.Broadcast(10, $"<size=25%><color=#6f6f6f>Данная комната <color=#ffa300>отравлена</color>.</color></size>", true);
                pl.EnableEffect(EffectType.Asphyxiated, 20);
            }
            for (int i = 0; i < 25; i++)
            {
                foreach (var pl in Player.List.Where(pl => pl.Team != Team.SCP && pl.Team != Team.RIP && pl.Room != null && pl.Room.Transform == room.Transform))
                {
                    pl.Damage(10, PlayerStatsSystem.DeathTranslations.Decontamination, scp);
                    if (pl.Role == RoleType.Spectator) scp.Scp079Controller.GiveExp(35);
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
            foreach (var door in doors)
            {
                if (!(door.Type is DoorType.HCZ_079_First or DoorType.HCZ_079_Second) || Round.ActiveGenerators > 2) door.Locked = false;
            }
            room.Lights.Color = Color.white;
            room.Lights.Override = false;
            yield break;
        }
    }
}