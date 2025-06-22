using System;
using System.Linq;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace Loli.Addons.RolePlay.Roles
{
    internal class RealDClass
    {
        internal void RealName(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.RoleType == RoleType.ClassD)
            {
                int number = UnityEngine.Random.Range(1000, 9999);
                ev.Player.DisplayNickname = $"D-class {number}";
                ev.Player.UnitName = $"Прозвище: <color=red>{ev.Player.Nickname.Replace("<", "").Replace(">", "")}</color>";
            }
            else ev.Player.DisplayNickname = "";
        }
        internal void RealСharacter(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.RoleType != RoleType.ClassD) return;
            var random = UnityEngine.Random.Range(1, 100);
            if (random < 5 && Round.ElapsedTime.TotalSeconds > 10 && Round.ElapsedTime.TotalMinutes < 1)
            {
                if (Extensions.Random.Next(1, 2) == 1) ev.Position = Map.GetRandomSpawnPoint(RoleType.Scientist) + Vector3.up;
                else ev.Position = Qurre.API.Extensions.GetRoom((RoomType)Extensions.Random.Next(5, 12)).Position;
                if (ev.Player.AllItems.Where(x => x.Type == ItemType.KeycardJanitor).Count() == 0)
                    ev.Player.AddItem(ItemType.KeycardJanitor);
                ev.Player.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#aebaf1>Уборщик</color> <color=#ff9900>D-класса</color></color></size>", 10, true);
                ev.Player.UnitName += "\nРоль: <color=#aebaf1>Уборщик</color>";
            }
            else if (random < 20)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.Scale = new Vector3(0.85f, 1, 0.85f);
                    ev.Player.Hp = 90;
                    ev.Player.MaxHp = 90;
                    ev.Player.Tag += "DSkinny";
                    ev.Player.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#ff5000>Слабый</color> <color=#ff9900>D-класс</color></color></size>", 10, true);
                });
            }
            else if (random < 40)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.Scale = new Vector3(1.1f, 0.9f, 1.1f);
                    ev.Player.Hp = 100;
                    ev.Player.MaxHp = 100;
                    ev.Player.Tag += "DThick";
                    ev.Player.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#ffbf00>Толстый</color> <color=#ff9900>D-класс</color></color></size>", 10, true);
                });
            }
            else if (random < 60)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    float scale = Extensions.Random.Next(82, 90);
                    ev.Player.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
                    ev.Player.Hp = 90;
                    ev.Player.MaxHp = 90;
                    ev.Player.Tag += "DSkinny";
                    ev.Player.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#60ff00>Маленький</color> <color=#ff9900>D-класс</color></color></size>", 10, true);
                });
            }
            else
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    float scale = Extensions.Random.Next(93, 115);
                    ev.Player.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
                });
            }
        }
        internal void FixTags(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.RoleType == RoleType.ClassD) return;
            if (ev.Player.Tag.Contains("DSkinny")) ev.Player.Tag = ev.Player.Tag.Replace("DSkinny", "");
            if (ev.Player.Tag.Contains("DThick")) ev.Player.Tag = ev.Player.Tag.Replace("DThick", "");
        }
        internal void FixTags(RoleChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.NewRole == RoleType.ClassD) return;
            if (ev.Player.Tag.Contains("DSkinny")) ev.Player.Tag = ev.Player.Tag.Replace("DSkinny", "");
            if (ev.Player.Tag.Contains("DThick")) ev.Player.Tag = ev.Player.Tag.Replace("DThick", "");
        }
        internal void FixTags(DeadEvent ev)
        {
            if (!Plugin.RolePlay) return;
            ev.Target.DisplayNickname = "";
            if (ev.Target.Tag.Contains("DSkinny")) ev.Target.Tag = ev.Target.Tag.Replace("DSkinny", "");
            if (ev.Target.Tag.Contains("DThick")) ev.Target.Tag = ev.Target.Tag.Replace("DThick", "");
        }
        internal void FixTags(EscapeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            ev.Player.DisplayNickname = "";
            if (ev.Player.Tag.Contains("DSkinny")) ev.Player.Tag = ev.Player.Tag.Replace("DSkinny", "");
            if (ev.Player.Tag.Contains("DThick")) ev.Player.Tag = ev.Player.Tag.Replace("DThick", "");
        }
        internal void PickReal(PickupItemEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player.Tag.Contains("DSkinny") && (ev.Pickup.Type == ItemType.GunAK || ev.Pickup.Type == ItemType.GunE11SR ||
                ev.Pickup.Type == ItemType.GunLogicer || ev.Pickup.Type == ItemType.GunShotgun || ev.Pickup.Type == ItemType.MicroHID))
            {
                ev.Allowed = false;
                ev.Player.ShowHint("<color=red><b>Данное оружие слишком тяжелое для вас</b></color>", 10);
            }
        }
    }
}