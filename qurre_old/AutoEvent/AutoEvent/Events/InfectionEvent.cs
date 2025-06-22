using AutoEvent.Functions;
using MEC;
using Qurre;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Events;
using Qurre.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AutoEvent.Functions.MainFunctions;
using Map = Qurre.API.Map;
using Player = Qurre.API.Player;
using Random = UnityEngine.Random;

namespace AutoEvent
{
    internal class InfectionEvent : Interfaces.IEvent
    {
        public string Name => "Заражение Зомби";
        public string Description => "";
        public string Color => "FF4242";
        public static Player Zombie { get; set; }
        public static Model Model { get; set; }
        public static TimeSpan EventTime { get; set; }
        public int Votes { get; set; }

        /// </inheritdoc>
        public void OnStart()
        {
            Plugin.IsEventRunning = true;
            Qurre.Events.Player.Join += OnJoin;
            Qurre.Events.Player.Dead += OnDead;
            Qurre.Events.Player.Damage += OnDamage;
            Qurre.Events.Round.Check += (CheckEvent e) => Qurre.Log.Info("Log");
            OnEventStarted();
            Log.Info($"{EventManager.CurrentEvent.Name}");
        }

        /// </inheritdoc>
        public void OnStop()
        {
            Plugin.IsEventRunning = false;
            Qurre.Events.Player.Join -= OnJoin;
            Qurre.Events.Player.Dead -= OnDead;
            Qurre.Events.Player.Damage -= OnDamage;
            EventEnd();
        }
        public static void OnEventStarted()
        {
            // Обнуление Таймера
            EventTime = new TimeSpan(0, 0, 0);
            // Создание карты
            CreatingMapFromJson("Zombie.json", new Vector3(145.18f, 945.26f, -122.97f), out var model);
            Model = model;
            // Запуск музыки
            PlayAudio("Zombie.f32le", 20, true, "Zombie");

            TeleportPlayersToPosition(Player.List.ToList(), Model.GameObject.transform.position + new Vector3(-25.44f, 1.51f, -0.74f));
            Timing.RunCoroutine(TimingBeginEvent($"Заражение", 15), "OnEventStarted");
        }
        // Отсчет до начала ивента
        public static IEnumerator<float> TimingBeginEvent(string eventName, float time)
        {
            for (float _time = time; _time > 0; _time--)
            {
                Map.ClearBroadcasts();
                Map.Broadcast($"<color=#D71868><b><i>{eventName}</i></b></color>\n<color=#ABF000>До начала ивента осталось <color=red>{_time}</color> секунд.</color>", 1);

                yield return Timing.WaitForSeconds(1f);
            }
            // Спавн зомби
            //SpawnZombie();
            //yield break;
        }
        // Спавн зомби
        public static void SpawnZombie()
        {
            Zombie = Player.List.ToList().RandomItem();
            BlockAndChangeRolePlayer(Zombie, RoleType.Scp0492);
            Timing.RunCoroutine(EventBeginning(), "SpawnZombie");
        }
        // Ивент начался - отсчет времени и колво людей
        public static IEnumerator<float> EventBeginning()
        {
            while (Player.List.Count(r => r.Role == RoleType.ClassD) > 1)
            {
                BroadcastPlayers($"<color=#D71868><b><i>Заражение</i></b></color>\n" +
                    $"<color=yellow>Осталось людей: <color=green>{Player.List.Count(r => r.Role == RoleType.ClassD)}</color></color>\n" +
                    $"<color=yellow>Время ивента <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 2);

                yield return Timing.WaitForSeconds(1f);
                EventTime += TimeSpan.FromSeconds(1f);
            }
            Timing.RunCoroutine(DopTime(), "EventBeginning");
            yield break;
        }
        // Если останется один человек, то обратный отсчет
        public static IEnumerator<float> DopTime() // дальше него не проходит!
        {
            for (int doptime = 30; doptime > 0; doptime--)
            {
                if (Player.List.Count(r => r.Role == RoleType.ClassD) == 0) break;

                BroadcastPlayers($"Дополнительное время: {doptime}\n" +
                $"<color=yellow>Остался <b><i>Последний</i></b> человек!</color>\n" +
                $"<color=yellow>Время ивента <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 2);

                yield return Timing.WaitForSeconds(1f);
                EventTime += TimeSpan.FromSeconds(1f);
            }
            EventManager.CurrentEvent.OnStop();
            yield break;
        }
        // Подведение итогов ивента и возврат в лобби
        public static void EventEnd()
        {
            if (Player.List.Count(r => r.Role == RoleType.ClassD) == 0)
            {
                BroadcastPlayers($"<color=red>Зомби Победили!</color>\n" +
                $"<color=yellow>Время ивента <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 10);
            }
            else
            {
                BroadcastPlayers($"<color=yellow><color=#D71868><b><i>Люди</i></b></color> Победили!</color>\n" +
                $"<color=yellow>Время ивента <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 10);
            }
            // Ожидание рестарта лобби допустим внезапный рестарт негативно встретится, а тут подведение итогов ивента
            Timing.CallDelayed(10f, () =>
            {
                // Выключение музыки
                StopAudio();
                // Рестарт Лобби
                EventManager.Init();
                // Очистка карты Ивента
                Model.Destroy();
            });
        }
        // Дальше идут ивенты ... для удобства думаю лучше их писать в конкретном ивенте
        public static void OnDamage(DamageEvent ev)
        {
            Log.Info("Проверка Дамага");
            if (ev.Attacker.Role == RoleType.Scp0492)
            {
                BlockAndChangeRolePlayer(ev.Target, RoleType.Scp0492);
            }
        }
        public static void OnDead(DeadEvent ev)
        {
            Log.Info("Проверка Смерти");
            ev.Target.Role = RoleType.Scp0492;
            Timing.CallDelayed(2f, () =>
            {
                ev.Target.Position = Model.GameObject.transform.position + new Vector3(-25.44f, 1.51f, -0.74f);
            });
        }
        public static void OnJoin(JoinEvent ev)
        {
            ev.Player.Role = RoleType.Scp0492;
            Timing.CallDelayed(2f, () =>
            {
                ev.Player.Position = Model.GameObject.transform.position + new Vector3(-25.44f, 1.51f, -0.74f);
            });
        }
    }
}
