using System.Collections.Generic;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using UnityEngine;
namespace Loli.Addons.RolePlay.Roles
{
    internal class FacilityManager
    {
        internal const string Tag = "FacilityManagerRole";
        internal const string TagSpy = "FacilityManagerSpy";
        internal static void Spawn(Player pl)
        {
            if (!Plugin.RolePlay) return;
            bool Spy = Random.Range(0, 100) > 65;
            pl.Tag += Spy ? TagSpy : Tag;
            if (pl.Role != RoleType.Scientist) pl.SetRole(RoleType.Scientist, true);
            Timing.CallDelayed(0.5f, () =>
            {
                pl.Hp = 200;
                pl.ResetInventory(new List<ItemType>
                {
                    ItemType.KeycardFacilityManager,
                    ItemType.ArmorHeavy,
                    ItemType.GunCOM18,
                    ItemType.SCP500,
                    ItemType.Adrenaline,
                    ItemType.Medkit,
                    ItemType.Radio,
                });
                pl.Position = RoomType.EzIntercom.GetRoom().Position + Vector3.up;
                pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#ff0000>Менеджер комплекса</color>\n" +
                    "Все в этом комплексе подчиняются вашим приказам</color></size>", 10, true);
                if (Spy) pl.ShowHint("<b><color=red>Вы состоите в группировке <color=#1ca71a>Повстанцев Хаоса</color>.</color></b>\n" +
                    "<b><size=90%><color=#db0027>Именно Вы устроили саботаж в комплексе, но, к вашему сожалению,</color></size></b>\n" +
                    "<b><color=#db0027>из-за подозрений <color=#494949>совета О5</color>, вы не смогли покинуть комплекс</color></b>\n" +
                    "<b><color=#5eb800>Ваша задача - помочь <color=red>Хакерам</color>  <color=#1ca71a>Повстанцев Хаоса</color>,</color></b>\n" +
                    "<b><color=#5eb800>и эвакуироваться из комплекса.</color></b>\n" +
                    "<b><color=#00d30c>У вас есть доступ к комнате управления</color></b>", 30);
                pl.UnitName = "<color=#ff0000>Менеджер комплекса</color>";
                //pl.NicknameSync.Network_customPlayerInfoString = "<color=#ff0000>Менеджер комплекса</color>";
                pl.NicknameSync.Network_customPlayerInfoString = "Менеджер комплекса";
                pl.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;
                pl.AddItem(ItemType.Ammo9x19, 20);
                Timing.CallDelayed(1f, () => pl.AddItem(ItemType.Coin));
            });
        }
        internal static void Damage(DamageProcessEvent ev)
        {
            if (!ev.Target.Tag.Contains(TagSpy)) return;
            if (ev.Attacker.Team != Team.CHI) return;
            if (ev.PrimitiveType != DamageTypesPrimitive.Firearm) return;
            ev.Attacker.ShowHint("<b><color=red>Менеджер комплекса является <color=#1ca71a>Шпионом Повстанцев Хаоса</color></color></b>");
            ev.Amount /= 50;
        }
        internal static void Escape(EscapeEvent ev)
        {
            if (ev.Player.Role != RoleType.Scientist) return;
            if (ev.Player.Tag.Contains(TagSpy) && Textures.Models.Rooms.Control.Status != HackMode.Hacked) ev.Allowed = false;
            else if (ev.Player.Tag.Contains(Tag) && (Round.ElapsedTime.TotalMinutes > 10 || Alpha.Active || Alpha.Detonated || OmegaWarhead.InProgress)) ev.Allowed = false;
        }
        internal static void Cuff(CuffEvent ev)
        {
            if (ev.Target.Role != RoleType.Scientist) return;
            if (ev.Cuffer.Team != Team.CHI) return;
            if (!ev.Target.Tag.Contains(TagSpy)) return;
            ev.Allowed = false;
        }
    }
}