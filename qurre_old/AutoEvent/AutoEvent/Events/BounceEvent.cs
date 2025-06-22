using AutoEvent.Functions;
using MEC;
using Qurre;
using Qurre.API;
using Qurre.API.Addons.Models;
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
    internal class BounceEvent : Interfaces.IEvent
    {
        /// </inheritdoc>
        public string Name => "Вышибалы с Мячиком";

        /// </inheritdoc>
        public string Description => "";

        /// </inheritdoc>
        public string Color => "FFFF00";
        public static Model Model { get; set; }
        public static TimeSpan EventTime { get; set; }
        // Отряд Мога
        public static List<Player> MTFList { get; set; } = new List<Player>();
        // Отряд Хаоса
        public static List<Player> ChaosList { get; set; } = new List<Player>();
        public int Votes { get; set; }

        /// </inheritdoc>
        public void OnStart()
        {
            Plugin.IsEventRunning = true;
            Qurre.Events.Player.Join += OnJoin;
            Qurre.Events.Player.Damage += OnDamage;
            Qurre.Events.Player.RoleChange += OnChangeRole;
            Qurre.Events.Player.ThrowItem += OnThrowItem;
            Qurre.Events.Player.Leave += OnPlayerLeave;
            OnEventStarted();
        }

        /// </inheritdoc>
        public void OnStop()
        {
            Plugin.IsEventRunning = false;
            Qurre.Events.Player.Join -= OnJoin;
            Qurre.Events.Player.Damage -= OnDamage;
            Qurre.Events.Player.RoleChange -= OnChangeRole;
            Qurre.Events.Player.ThrowItem -= OnThrowItem;
            Qurre.Events.Player.Leave -= OnPlayerLeave;
            EventEnd();
        }
        public static void OnEventStarted()
        {
            // Обнуление Таймера
            EventTime = new TimeSpan(0, 0, 0);
            // Создание карты
            CreatingMapFromJson("Bounce.json", new Vector3(145.18f, 945.26f, -122.97f), out var model);
            Model = model;
            // Запуск музыки
            PlayAudio("PVZ_Moongrains.f32le", 20, true, "Bounce");

            // Создание и телепорт отрядов
            var count = 0;
            foreach (Player player in Player.List)
            {
                if (count % 2 == 0)
                {
                    MTFList.Add(player);

                    player.Role = RoleType.Scientist;
                    Timing.CallDelayed(2f, () =>
                    {
                        player.Position = Model.GameObject.transform.position + RandomPosition(true);
                        foreach (var item in player.AllItems.ToList())
                        {
                            player.RemoveItem(item);
                        }
                    });
                }
                else
                {
                    ChaosList.Add(player);

                    player.Role = RoleType.ClassD;
                    Timing.CallDelayed(2f, () =>
                    {
                        player.Position = Model.GameObject.transform.position + RandomPosition(false);
                    });
                    foreach (var item in player.AllItems.ToList())
                    {
                        player.RemoveItem(item);
                    }
                }
                count++;
            }
            // Запуск ивента
            Timing.RunCoroutine(TimingBeginEvent($"Вышибалы", 15), "OnEventStarted");
        }
        // Отсчет до начала ивента
        public static IEnumerator<float> TimingBeginEvent(string eventName, float time) // не используется но нужно
        {
            // Эффект - стоять на месте
            EnablePlayersEffect(Player.List.ToList(), EffectType.Ensnared);
            // Отсчёт
            for (float _time = time; _time > 0; _time--)
            {
                Map.ClearBroadcasts();
                Map.Broadcast($"<color=#D71868><b><i>{eventName}</i></b></color>\n<color=#ABF000>До начала ивента осталось <color=red>{_time}</color> секунд.</color>", 1);

                yield return Timing.WaitForSeconds(1f);
            }
            // Эффект отключаем
            DisablePlayersEffect(Player.List.ToList(), EffectType.Ensnared);
            // Запуск корутины начала ивента
            Timing.RunCoroutine(BallManager(), "BallStarted");
            yield break;
        }
        // Тайминг, который каждую секунду выдает мячик, а также считает время
        public static IEnumerator<float> BallManager()
        {
            while (MTFList.ToList().Count > 0 && ChaosList.ToList().Count > 0)
            {
                BroadcastPlayers($"<color=#D71868><b><i>Вышибалы</i></b></color>\n" +
                $"<color=yellow><color=blue>{MTFList.Count}</color> VS <color=green>{ChaosList.Count}</color></color>\n" +
                $"<color=yellow>Время ивента <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 2);

                foreach (Player player in Player.List)
                {
                    if ((MTFList.Contains(player) || ChaosList.Contains(player)) && EventTime.Seconds % 5 == 0)
                    {
                        player.AddItem(ItemType.SCP018);
                    }
                }
                EventTime += TimeSpan.FromSeconds(1f);
                yield return Timing.WaitForSeconds(1f);
            }
            EventEnd();
            yield break;
        }
        // Подведение итогов ивента и возврат в лобби
        public static void EventEnd()
        {
            if (MTFList.ToList().Count == 0)
            {
                BroadcastPlayers($"<color=#D71868><b><i>Вышибалы</i></b></color>\n" +
                $"<color=yellow>ПОБЕДИЛИ - <color=green>{ChaosList.Count} ХАОС</color></color>\n" +
                $"<color=yellow>Конец ивент: <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 10);
            }
            else
            {
                BroadcastPlayers($"<color=#D71868><b><i>Вышибалы</i></b></color>\n" +
                $"<color=yellow>ПОБЕДИЛИ - <color=blue>{MTFList.Count} МОГ</color></color>\n" +
                $"<color=yellow>Конец ивент: <color=red>{EventTime.Minutes}:{EventTime.Seconds}</color></color>", 10);
            }
            // Ожидание рестарта лобби допустим внезапный рестарт негативно встретится, а тут подведение итогов ивента
            Timing.CallDelayed(10f, () =>
            {
                // Чистим лист
                MTFList.Clear();
                ChaosList.Clear();
                // Выключение музыки
                StopAudio();
                // Рестарт Лобби
                EventManager.Init();
                // Очистка карты Ивента
                Model.Destroy();
            });
        }
        public static Vector3 RandomPosition(bool isMTF)
        {
            Vector3 position = new Vector3(0, 0, 0);
            var rand = Random.Range(0, 5);
            if (isMTF)
            {
                switch (rand)
                {
                    case 0: position = new Vector3(28.6f, 5.31f, 0.99f); break;
                    case 1: position = new Vector3(9.66f, 1.19f, 21.11f); break;
                    case 2: position = new Vector3(9.66f, 1.19f, 8.93f); break;
                    case 3: position = new Vector3(9.66f, 1.19f, -3.65f); break;
                    case 4: position = new Vector3(9.66f, 1.19f, -20.08f); break;

                }
            }
            else switch (rand)
                {
                    case 0: position = new Vector3(-27.46f, 5.59f, 0.99f); break;
                    case 1: position = new Vector3(-11.38f, 1.19f, -20.08f); break;
                    case 2: position = new Vector3(-11.38f, 1.19f, -3.65f); break;
                    case 3: position = new Vector3(-11.38f, 1.19f, 8.93f); break;
                    case 4: position = new Vector3(-11.38f, 1.19f, 21.11f); break;
                }
            return position;
        }
        // Ивенты
        public static void OnDamage(DamageEvent ev)
        {
            if (ev.DamageType == DamageTypes.Scp018)
            {
                ev.Target.AllItems.ToList().Clear();
                ev.Allowed = false;
                DamageTeleport(ev);
            }
        }
        public static void OnThrowItem(ThrowItemEvent ev)
        {
            //ev.Player.RemoveHandItem();
            //GrenadeFrag grenade = new GrenadeFrag(ItemType.SCP018);
            //grenade.FuseTime = 5f;
            //grenade.MaxRadius = 1f;
            //grenade.Throw(true);
            //grenade.Scale = new Vector3(3f, 3f, 3f);
            //grenade.Base.transform.localScale = new Vector3(3f, 3f, 3f);
        }
        public static void DamageTeleport(DamageEvent ev)
        {
            // Убираем из листа
            if (MTFList.Contains(ev.Target))
            {
                MTFList.Remove(ev.Target);
            }
            if (ChaosList.Contains(ev.Target))
            {
                ChaosList.Remove(ev.Target);
            }
            // Изменяем роль при смерти и тепаем на вышку
            if (ev.Target.Role == RoleType.Scientist) ev.Target.Position = Model.GameObject.transform.position + new Vector3(26.35f, 20.72f, 0.99f);
            else ev.Target.Position = Model.GameObject.transform.position + new Vector3(-27.46f, 20.72f, 0.99f);
        }
        public static void OnJoin(JoinEvent ev)
        {
            ev.Player.Role = RoleType.ClassD;
            Timing.CallDelayed(2f, () =>
            {
                ev.Player.Position = Model.GameObject.transform.position + new Vector3(-27.46f, 20.72f, 0.99f);
            });
        }
        public static void OnPlayerLeave(LeaveEvent ev)
        {
            if (MTFList.Contains(ev.Player))
            {
                MTFList.Remove(ev.Player);
            }
            if (ChaosList.Contains(ev.Player))
            {
                ChaosList.Remove(ev.Player);
            }
        }
        public static void OnChangeRole(RoleChangeEvent ev)
        {
            if (ev.NewRole == RoleType.Scientist)
            {
                if (ChaosList.Contains(ev.Player)) ChaosList.Remove(ev.Player);
                if (MTFList.Contains(ev.Player)) MTFList.Add(ev.Player);

                ev.Player.Role = RoleType.Scientist;
                Timing.CallDelayed(2f, () =>
                {
                    ev.Player.Position = Model.GameObject.transform.position + new Vector3(26.35f, 20.72f, 0.99f);
                });
            }
            else if (ev.NewRole == RoleType.ClassD)
            {
                if (MTFList.Contains(ev.Player)) MTFList.Remove(ev.Player);
                if (ChaosList.Contains(ev.Player)) ChaosList.Add(ev.Player);

                ev.Player.Role = RoleType.ClassD;
                Timing.CallDelayed(2f, () =>
                {
                    ev.Player.Position = Model.GameObject.transform.position + new Vector3(-27.46f, 20.72f, 0.99f);
                });
            }
        }
    }
}
