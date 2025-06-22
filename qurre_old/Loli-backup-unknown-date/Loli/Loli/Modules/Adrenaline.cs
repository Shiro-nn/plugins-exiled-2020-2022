using System.Collections.Generic;
using Qurre.API;
using Qurre.API.Events;
using MEC;
using Qurre.API.Objects;
namespace Loli.Modules
{
    public partial class EventHandlers
    {
        private Dictionary<string, int> AdrData = new();
        internal void AdrenalineRefresh()
        {
            AdrData.Clear();
            AlphaEnabled = false;
            AlphaDiscusion = false;
        }
        internal void Adrenaline(ItemUsedEvent ev)
        {
            if (ev.Item.TypeId != ItemType.Adrenaline) return;
            if (!AdrData.ContainsKey(ev.Player.UserId)) AdrData.Add(ev.Player.UserId, 0);
            AdrData[ev.Player.UserId] += 1;
            if (AdrData[ev.Player.UserId] == 5) Timing.RunCoroutine(PostFix(Round.CurrentRound, ev.Player), $"Adrenaline-{ev.Player.UserId}");
        }
        internal void Spawn(RoleChangeEvent ev)
        {
            if (!AdrData.ContainsKey(ev.Player.UserId)) return;
            AdrData[ev.Player.UserId] = 0;
            try { Timing.KillCoroutines($"Adrenaline-{ev.Player.UserId}"); } catch { }
        }
        internal void Spawn(SpawnEvent ev)
        {
            if (!AdrData.ContainsKey(ev.Player.UserId)) return;
            AdrData[ev.Player.UserId] = 0;
            try { Timing.KillCoroutines($"Adrenaline-{ev.Player.UserId}"); } catch { }
        }
        internal void Scp500(ItemUsedEvent ev)
        {
            if (ev.Item.TypeId != ItemType.SCP500) return;
            if (!AdrData.ContainsKey(ev.Player.UserId)) return;
            AdrData[ev.Player.UserId] = 0;
            try { Timing.KillCoroutines($"Adrenaline-{ev.Player.UserId}"); } catch { }
        }
        private IEnumerator<float> PostFix(int round, Player pl)
        {
            var role = pl.GetCustomRole();
            yield return Timing.WaitForSeconds(Extensions.Random.Next(10, 15));
            if (round != Round.CurrentRound || pl == null) yield break;
            pl.ShowHint("<b><color=red>Вы употребили слишком много адреналина.</color></b>\n<color=#0089c7>У вас начались проблемы с сердцем.</color>", 10);
            pl.EnableEffect(EffectType.Asphyxiated, 15, true);
            pl.EnableEffect(EffectType.Hemorrhage, 10, true);
            yield return Timing.WaitForSeconds(15);
            yield return Timing.WaitForSeconds(Extensions.Random.Next(30, 45));
            if (round != Round.CurrentRound || pl == null) yield break;
            pl.EnableEffect(EffectType.Asphyxiated, 30, true);
            pl.EnableEffect(EffectType.Hemorrhage, 20, true);
            yield return Timing.WaitForSeconds(30);
            yield return Timing.WaitForSeconds(Extensions.Random.Next(100, 150));
            if (round != Round.CurrentRound || pl == null) yield break;
            pl.EnableEffect(EffectType.Asphyxiated, 120, true);
            pl.EnableEffect(EffectType.Hemorrhage, 100, true);
            yield return Timing.WaitForSeconds(120);
            if (round != Round.CurrentRound || pl == null) yield break;
            if (pl.GetCustomRole() == role) pl.Kill("Остановка сердца");
            yield break;
        }
    }
}