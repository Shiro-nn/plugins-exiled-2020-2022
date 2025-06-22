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
    internal class Lava : Interfaces.IEvent
    {
        public string Name => "Пол - это ЛАВА";
        public string Description => "";
        public string Color => "FFFF00";
        public static Model Model { get; set; }
        public static Model LavaModel { get; set; }
        public static TimeSpan EventTime { get; set; }
        public int Votes { get; set; }

        public void OnStart()
        {
            Qurre.Log.Info("e");
            Plugin.IsEventRunning = true;
            Qurre.Events.Player.Damage += OnDamage;
            OnEventStarted();
        }
        public void OnStop()
        {
            Qurre.Log.Info("e4");
            Plugin.IsEventRunning = false;
            Qurre.Events.Player.Damage -= OnDamage;
            //EventEnd();
        }
        public static void OnEventStarted()
        {
            Qurre.Log.Info("e2");
            // Обнуление Таймера
            EventTime = new TimeSpan(0, 0, 0);
            // Создание карты
            CreatingMapFromJson("Lava.json", new Vector3(145.18f, 930f, -122.97f), out var model);
            Model = model;
            // Создание лавы
            LavaModel = new Model("lava", Model.GameObject.transform.position);
            LavaModel.AddPart(new ModelPrimitive(LavaModel, PrimitiveType.Cube, new Color32(255, 0, 0, 255), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(100, 1, 100)));
            foreach(var prim in LavaModel.Primitives)
            {
                prim.GameObject.AddComponent<LavaComponent>();
            }
            // фф включаем
            Server.FriendlyFire = true;
            // Запуск музыки
            PlayAudio("FallGuys_DnB.f32le", 10, true, "LavaAudio");
            Timing.CallDelayed(2f, () =>
            {
                // Делаем всех д классами
                foreach (var player in Qurre.API.Player.List)
                {
                    Qurre.Log.Info("e3");
                    player.Role = RoleType.ClassD;
                    player.EnableEffect(EffectType.Ensnared);
                    Timing.CallDelayed(2f, () =>
                    {
                        player.Position = Model.GameObject.transform.position + RandomPlayerPosition();
                        player.GameObject.AddComponent<BoxCollider>();
                        player.GameObject.AddComponent<BoxCollider>().size = new Vector3(1f, 3f, 1f);
                    });
                }
            });
            foreach(var gun in RandomGunPosition)
            {
                var item = ItemType.GunCOM15;
                var rand = Random.Range(0, 100);
                if (rand < 40) item = ItemType.GunCOM15;
                else if (rand >= 40 && rand < 80) item = ItemType.GunCOM18;
                else if (rand >= 80 && rand < 90) item = ItemType.ArmorHeavy;
                else if (rand >= 90 && rand < 100) item = ItemType.GunFSP9;
                Pickup pickup = new Item(item).Spawn(Model.GameObject.transform.position + gun);
            }
            // Запуск ивента
            Timing.RunCoroutine(Cycle(), "cycle_time");
        }
        public static IEnumerator<float> Cycle()
        {
            // Отсчет обратного времени
            for (int time = 10; time > 0; time--)
            {
                Map.ClearBroadcasts();
                Map.Broadcast($"<size=100><color=red>{time}</color></size>", 1);
                yield return Timing.WaitForSeconds(1f);
            }
            Player.List.ToList().ForEach(player => player.DisableEffect(EffectType.Ensnared));
            while (Player.List.ToList().Count(r => r.Role != RoleType.Spectator) > 1) // >
            {
                Map.ClearBroadcasts();
                string text = string.Empty;
                if (EventTime.TotalSeconds % 2 == 0)
                {
                    text = "<size=90><color=red><b>《 ! 》</b></color></size>\n";
                }
                else
                {
                    text = "<size=90><color=red><b>  !  </b></color></size>\n";
                }
                Map.Broadcast(text + $"<size=20><color=red><b>Живых: {Player.List.ToList().Count(r => r.Role != RoleType.Spectator)} Игроков</b></color></size>", 1);
                LavaModel.GameObject.transform.position += new Vector3(0, 0.1f, 0);
                yield return Timing.WaitForSeconds(1f);
                EventTime += TimeSpan.FromSeconds(1f);
            }
            if (Player.List.ToList().Count(r => r.Role != RoleType.Spectator) == 1)
            {
                Map.Broadcast($"<size=80><color=red><b>Победитель\n{Player.List.ToList().First(r => r.Role != RoleType.Spectator).Nickname}</b></color></size>", 10);
            }
            else if (Player.List.ToList().Count(r => r.Role != RoleType.Spectator) == 0)
            {
                Map.Broadcast($"<size=70><color=red><b>Все утонули в Лаве)))))</b></color></size>", 10);
            }
            EventEnd();
            yield break;
        }
        // Подведение итогов ивента и возврат в лобби
        public static void EventEnd()
        {
            // Ожидание рестарта лобби допустим внезапный рестарт негативно встретится, а тут подведение итогов ивента
            Timing.CallDelayed(10f, () =>
            {
                // фф выключаем
                Server.FriendlyFire = false;
                // Выключение музыки
                StopAudio();
                // Очистка карты Ивента
                Model.Destroy();
                // Очистка лавы
                LavaModel.Destroy();
                // Очистка оружия
                Map.Pickups.ForEach(x => x.Destroy());
                // Рестарт Лобби
                EventManager.Init();
            });
        }
        // Рандомная позиция игрока
        public static Vector3 RandomPlayerPosition()
        {
            Vector3 pos = new Vector3(0f, 0f, 0f);
            switch (Random.Range(0, 10))
            {
                case 0: pos = new Vector3(-13.43136f, 2.6f, 25.6032f); break; // 2.52
                case 1: pos = new Vector3(2.91f, 2.6f, 16.35f); break;
                case 2: pos = new Vector3(18.59f, 2.6f, 23.39f); break;
                case 3: pos = new Vector3(27.92f, 2.6f, 7.71f); break;
                case 4: pos = new Vector3(15.34f, 2.6f, -0.27f); break;
                case 5: pos = new Vector3(18.02f, 2.6f, -23.75f); break;
                case 6: pos = new Vector3(9.88f, 2.6f, -12.95f); break;
                case 7: pos = new Vector3(0.11f, 2.6f, -16.04f); break;
                case 8: pos = new Vector3(-13.26f, 2.6f, -14.57f); break;
                case 9: pos = new Vector3(-26.03f, 2.6f, -9.07f); break;
                case 10: pos = new Vector3(-14.85f, 2.6f, 4.35f); break;
                case 11: pos = new Vector3(-24.42f, 2.6f, 16.25f); break;
                case 12: pos = new Vector3(-3.49f, 2.6f, 29.02f); break;
            }
            return pos;
        }
        // Рандомная позиция пушек
        public static List<Vector3> RandomGunPosition { get; set; } = new List<Vector3>()
        {
            new Vector3(-17.12973f, 4.0342f, 21.97289f),
            new Vector3(4.282532f, 8.69206f, 20.4628f),
            new Vector3(22.10068f, 1.9663f, 8.774416f),
            new Vector3(18.5771f, 7.605f, 10.29191f),
            new Vector3(0.2160301f, 11.899f, 0.1276894f),
            new Vector3(8.975021f, 16.92524f, -25.19261f),
            new Vector3(-16.69451f, 1.90302f, -24.33174f),
            new Vector3(-21.3225f, 1.90302f, -7.356604f),
            new Vector3(26.95424f, 5.43766f, -6.992086f),
            new Vector3(24.92152f, 11.3792f, -11.712f),
            new Vector3(9.444767f, 11.27524f, -23.41049f),
            new Vector3(-24.58f, 4.33f, 4.68f)
        };
        // Ивенты
        public static void OnDamage(DamageEvent ev)
        {
            ev.Amount = 3.5f;
        }
        public static void OnJoin(JoinEvent ev)
        {
            ev.Player.Role = RoleType.Spectator;
            ev.Player.ClearBroadcasts();
            ev.Player.Broadcast("<color=yellow>Привет, Игрок!\n" +
                "Сейчас проходит ивент <color=red>'Пол - это ЛАВА'</color>" +
                "Ты мёртв, подожди некоторое время.</color>", 10);
        }
    }
}
