using MEC;
using Qurre.API;
using Qurre.API.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Addons.RolePlay.Roles
{
    internal class Scientists
    {
        internal static void SpawnMajor(Player major)
        {
            if (!Plugin.RolePlay) return;
            major.Tag += "MajorScientist";
            if (major.Role != RoleType.Scientist) major.SetRole(RoleType.Scientist);
            Timing.CallDelayed(0.5f, () =>
            {
                major.ClearInventory();
                major.GetAmmo();
                major.AddItem(ItemType.KeycardResearchCoordinator);
                major.AddItem(ItemType.Medkit);
                major.AddItem(ItemType.SCP500);
                major.AddItem(ItemType.Radio);
                major.AddItem(ItemType.Flashlight);
                major.AddItem(ItemType.ArmorLight);
                major.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#fdffbb>Координатор исследований</color>\n" +
                    "Другие ученные подчиняются вашим приказам</color></size>", 10, true);
                major.UnitName = "<color=#fdffbb>Координатор исследований</color>";
                //major.NicknameSync.Network_customPlayerInfoString = "<color=#fdffbb>Координатор исследований</color>";
                major.NicknameSync.Network_customPlayerInfoString = "Координатор исследований";
                major.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
            });
        }
        internal static void SpawnRandom(Player pl)
        {
            var random = Random.Range(1, 100);
            if (pl.Tag.Contains("MajorScientist")) return;
            if (pl.Tag.Contains(FacilityManager.Tag)) return;
            if (pl.Tag.Contains(FacilityManager.TagSpy)) return;
            if (random < 15)
            {
                pl.AddItem(ItemType.KeycardZoneManager);
                pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#217879>Менеджер зон содержания</color>\n" +
                    "Подчиняйтесь приказам <color=#fdffbb>координатора исследований</color>,\nлибо эвакуируйтесь самостоятельно</color></size>", 10, true);
                pl.UnitName = "<color=#217879>Менеджер зон содержания</color>";
                //pl.NicknameSync.Network_customPlayerInfoString = "<color=#217879>Менеджер зон содержания</color>";
                pl.NicknameSync.Network_customPlayerInfoString = "Менеджер зон содержания";
                pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
            }
            else
            {
                pl.AddItem(ItemType.KeycardScientist);
                pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#e2e26d>Научный сотрудник</color>\n" +
                    "Подчиняйтесь приказам <color=#fdffbb>координатора исследований</color>,\nлибо эвакуируйтесь самостоятельно</color></size>", 10, true);
                pl.UnitName = "<color=#e2e26d>Научный сотрудник</color>";
                pl.NicknameSync.Network_customPlayerInfoString = "Научный сотрудник";
                pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
            }
            float scale = Random.Range(90, 110);
            pl.Scale = new Vector3(scale / 100, scale / 100, scale / 100);
        }
        internal void RoundStart()
        {
            if (!Plugin.RolePlay) return;
            var list = Player.List.Where(x => x.Role == RoleType.Scientist).ToArray();
            if (list.Count() == 0) return;
            list.Shuffle();
            for (int i = 0; i < list.Count(); i++)
            {
                var pl = list[i];
                if (i == 0) SpawnMajor(pl);
                else if (i == 1 && Random.Range(0, 100) < 40) FacilityManager.Spawn(pl);
                else SpawnRandom(pl);
            }
        }
        internal void Spawn(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.RoleType != RoleType.Scientist) return;
            Timing.CallDelayed(0.5f, () => SpawnRandom(ev.Player));
        }
        internal void FixTags(SpawnEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.RoleType == RoleType.Scientist) return;
            if (ev.Player.Tag.Contains("MajorScientist")) ev.Player.Tag = ev.Player.Tag.Replace("MajorScientist", "");
            if (ev.Player.Tag.Contains(FacilityManager.Tag)) ev.Player.Tag = ev.Player.Tag.Replace(FacilityManager.Tag, "");
            if (ev.Player.Tag.Contains(FacilityManager.TagSpy)) ev.Player.Tag = ev.Player.Tag.Replace(FacilityManager.TagSpy, "");
        }
        internal void FixTags(RoleChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.NewRole == RoleType.Scientist) return;
            if (ev.Player.Tag.Contains("MajorScientist")) ev.Player.Tag = ev.Player.Tag.Replace("MajorScientist", "");
            if (ev.Player.Tag.Contains(FacilityManager.Tag)) ev.Player.Tag = ev.Player.Tag.Replace(FacilityManager.Tag, "");
            if (ev.Player.Tag.Contains(FacilityManager.TagSpy)) ev.Player.Tag = ev.Player.Tag.Replace(FacilityManager.TagSpy, "");
        }
        internal void FixTags(DeadEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Target.Tag.Contains("MajorScientist")) ev.Target.Tag = ev.Target.Tag.Replace("MajorScientist", "");
            if (ev.Target.Tag.Contains(FacilityManager.Tag)) ev.Target.Tag = ev.Target.Tag.Replace(FacilityManager.Tag, "");
            if (ev.Target.Tag.Contains(FacilityManager.TagSpy)) ev.Target.Tag = ev.Target.Tag.Replace(FacilityManager.TagSpy, "");
        }
        internal void BetterHid(PickupItemEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.Player.Role != RoleType.Scientist) return;
            if (ev.Pickup.Type != ItemType.MicroHID) return;
            ev.Allowed = false;
            if (ev.Player.Tag.Contains(" DontHid")) return;
            ev.Allowed = true;
            Timing.CallDelayed(0.5f, () => ev.Player.Inventory.ServerSelectItem(ev.Pickup.Serial));
            Timing.RunCoroutine(PostFix(), $"BetterHid-{ev.Player.UserId}");
            IEnumerator<float> PostFix()
            {
                var round = Round.CurrentRound;
                yield return Timing.WaitForSeconds(ev.Player.Scale.y * 60);
                if (round != Round.CurrentRound || ev.Player == null || ev.Player.ItemInHand == null || ev.Player.ItemTypeInHand != ItemType.MicroHID) yield break;
                ev.Player.DropItem(ev.Player.ItemInHand);
                ev.Player.ShowHint("<b><color=red>Отдыхайте...</color></b>", 10);
                ev.Player.Stamina.RemainingStamina = 0;
                ev.Player.Tag += " DontHid";
                yield return Timing.WaitForSeconds(10);
                if (round != Round.CurrentRound || ev.Player == null) yield break;
                ev.Player.Tag = ev.Player.Tag.Replace(" DontHid", "");
                yield break;
            }
        }
        internal void BetterHid(DropItemEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Player.Role != RoleType.Scientist) return;
            if (ev.Item.Type != ItemType.MicroHID) return;
            ev.Player.Tag += " DontHid";
            Timing.RunCoroutine(PostFix());
            Timing.KillCoroutines($"BetterHid-{ev.Player.UserId}");
            IEnumerator<float> PostFix()
            {
                var round = Round.CurrentRound;
                yield return Timing.WaitForSeconds(10);
                if (round != Round.CurrentRound || ev.Player == null) yield break;
                ev.Player.Tag = ev.Player.Tag.Replace(" DontHid", "");
                yield break;
            }
        }
        internal void BetterHid(ItemChangeEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ev.Allowed) return;
            if (ev.OldItem == null) return;
            if (ev.Player.Tag.Contains("DontHid")) return;
            if (ev.Player.Role != RoleType.Scientist) return;
            if (ev.OldItem.Type != ItemType.MicroHID) return;
            ev.Allowed = false;
        }
    }
}