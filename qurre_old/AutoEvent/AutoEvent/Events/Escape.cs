using AutoEvent.Functions;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Qurre;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AutoEvent.Functions.MainFunctions;
using Random = UnityEngine.Random;

namespace AutoEvent
{
    internal class Escape : Interfaces.IEvent
    {
        public string Name => "Атомный Побег";
        public string Description => "";
        public string Color => "FFFF00";
        public static TimeSpan EventTime { get; set; }
        //public static Dictionary<Player, TimeSpan> EscapedSCP { get; set; } = new Dictionary<Player, TimeSpan>();
        public int Votes { get; set; }

        public void OnStart()
        {
            Plugin.IsEventRunning = true;
            Qurre.Events.Alpha.Stopping += OnNukeDisable;
            OnEventStarted();
        }
        public void OnStop()
        {
            Plugin.IsEventRunning = false;
            Qurre.Events.Alpha.Stopping -= OnNukeDisable;
            EventEnd();
        }

        static void OnNukeDisable(AlphaStopEvent ev)
        {
            ev.Allowed = false;
        }

        public static void OnEventStarted() // сделать ЛГБТ цвет
        {
            // Делаем всех д классами
            Player.List.ToList().ForEach(player =>
            {
                player.Role = RoleType.Scp173;
                player.EnableEffect(EffectType.Ensnared);
            });
            // Запуск боеголовки
            Alpha.Start();
            Alpha.TimeToDetonation = 80f;
            // Запуск музыки
            PlayAudio("Bomba_haus1.f32le", 20, false, "Escape");
            // Запуск ивента
            Timing.RunCoroutine(Cycle(), "cycle_time");
        }
        public static IEnumerator<float> Cycle()
        {
            // Обнуление таймера
            EventTime = new TimeSpan(0, 0, 0);
            // Отсчет обратного времени
            for (int time = 10; time > 0; time--)
            {
                Map.ClearBroadcasts();
                Map.Broadcast($"Атомный Побег\n" +
                    $"Успейте сбежать с комплекса пока он не взоврался!\n" +
                    $"<color=red>До начала побега: {(int)time} секунд</color>", 1);
                yield return Timing.WaitForSeconds(1f);
                EventTime += TimeSpan.FromSeconds(1f);
            }
            // Открываем все двери
            foreach (Door door in Map.Doors) door.Open = true;
            // Выключаем остановку
            Player.List.ToList().ForEach(player => player.DisableEffect(EffectType.Ensnared));
            // Отсчет времени
            while (Alpha.TimeToDetonation != 0)
            {
                foreach(Player player in Player.List)
                {
                    if (player.Room.Type == RoomType.EzGateA || player.Room.Type == RoomType.EzGateB) player.TeleportToRoom(RoomType.Surface);
                }
                Map.ClearBroadcasts();
                Map.Broadcast($"Атомный Побег\n" +
                    $"До взрыва: <color=red>{(int)Alpha.TimeToDetonation}</color> секунд", 1);
                yield return Timing.WaitForSeconds(1f);
                EventTime += TimeSpan.FromSeconds(1f);
            }
            EventEnd();
            yield break;
        }
        // Подведение итогов ивента и возврат в лобби
        public static void EventEnd()
        {
            /*
            // Сортировка
            EscapedSCP.ToList().OrderBy(e => e.Value.TotalSeconds);
            // Вывод результатов за раунд:
            int i = 1;
            string text = string.Empty;
            foreach (var scp in EscapedSCP)
            {
                if (i == 4) break;
                text += $"<color=yellow><color=red>{i}</color> Место: <color=red><b><i>{scp.Key} {scp.Value}</i></b></color></color>\n";
                i++;
            }
            */
            Map.ClearBroadcasts();
            Map.Broadcast($"Атомный Побег\n" +
                $"<color=red>ПОБЕДА SCP</color>", 10);
            // Ожидание рестарта лобби допустим внезапный рестарт негативно встретится, а тут подведение итогов ивента
            Timing.CallDelayed(10f, () =>
            {
                // Выключение музыки
                StopAudio();
                // Рестарт Лобби
                EventManager.Init();
            });
        }
        // Ивенты
        public static void OnJoin(JoinEvent ev)
        {
            ev.Player.Role = RoleType.Scp173;
        }
    }
}
