using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Addons
{
    static class AdrenalineInsult
    {
        static readonly Dictionary<string, int> AdrData = new();

        [EventMethod(RoundEvents.Waiting)]
        [EventMethod(RoundEvents.Start)]
        static void Refresh()
        {
            AdrData.Clear();
        }

        [EventMethod(PlayerEvents.UsedItem)]
        static void Adrenaline(UsedItemEvent ev)
        {
            if (ev.Item.Type != ItemType.Adrenaline)
                return;

            if (!AdrData.ContainsKey(ev.Player.UserInfomation.UserId))
            {
                AdrData.Add(ev.Player.UserInfomation.UserId, 1);
                return;
            }

            AdrData[ev.Player.UserInfomation.UserId] += 1;
            if (AdrData[ev.Player.UserInfomation.UserId] == 5)
                Timing.RunCoroutine(PostFix(Round.CurrentRound, ev.Player), $"Adrenaline-{ev.Player.UserInfomation.UserId}");
        }

        [EventMethod(PlayerEvents.ChangeRole)]
        static void Spawn(ChangeRoleEvent ev)
        {
            if (!AdrData.ContainsKey(ev.Player.UserInfomation.UserId))
                return;

            AdrData[ev.Player.UserInfomation.UserId] = 0;
            try { Timing.KillCoroutines($"Adrenaline-{ev.Player.UserInfomation.UserId}"); } catch { }
        }

        [EventMethod(PlayerEvents.Spawn)]
        static void Spawn(SpawnEvent ev)
        {
            if (!AdrData.ContainsKey(ev.Player.UserInfomation.UserId))
                return;

            AdrData[ev.Player.UserInfomation.UserId] = 0;
            try { Timing.KillCoroutines($"Adrenaline-{ev.Player.UserInfomation.UserId}"); } catch { }
        }

        [EventMethod(PlayerEvents.UsedItem)]
        static void Scp500(UsedItemEvent ev)
        {
            if (ev.Item.Type != ItemType.SCP500)
                return;

            if (!AdrData.ContainsKey(ev.Player.UserInfomation.UserId))
                return;

            AdrData[ev.Player.UserInfomation.UserId] = 0;
            try { Timing.KillCoroutines($"Adrenaline-{ev.Player.UserInfomation.UserId}"); } catch { }
        }
        static IEnumerator<float> PostFix(int round, Player pl)
        {
            var role = pl.GetCustomRole();

            yield return Timing.WaitForSeconds(Random.Range(10, 15));

            if (round != Round.CurrentRound || pl == null)
                yield break;

            pl.Client.ShowHint("<b><color=red>Вы употребили слишком много адреналина.</color></b>\n<color=#0089c7>У вас начались проблемы с сердцем.</color>", 10);
            pl.Effects.Enable(EffectType.Asphyxiated, 15, true);
            pl.Effects.Enable(EffectType.Hemorrhage, 10, true);

            yield return Timing.WaitForSeconds(15);
            yield return Timing.WaitForSeconds(Random.Range(30, 45));

            if (round != Round.CurrentRound || pl == null)
                yield break;

            pl.Effects.Enable(EffectType.Asphyxiated, 30, true);
            pl.Effects.Enable(EffectType.Hemorrhage, 20, true);

            yield return Timing.WaitForSeconds(30);
            yield return Timing.WaitForSeconds(Random.Range(100, 150));

            if (round != Round.CurrentRound || pl == null)
                yield break;

            pl.Effects.Enable(EffectType.Asphyxiated, 120, true);
            pl.Effects.Enable(EffectType.Hemorrhage, 100, true);

            yield return Timing.WaitForSeconds(120);

            if (round != Round.CurrentRound || pl == null)
                yield break;

            if (pl.GetCustomRole() == role)
                pl.HealthInfomation.Kill("Остановка сердца");

            yield break;
        }
    }
}