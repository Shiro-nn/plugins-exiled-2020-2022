using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
using Qurre.API.Objects;
using CustomPlayerEffects;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Juice : IDrink
    {
        public string Name { get; } = "Сок";

        public string Description { get; } = "Фруктовый сок";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "оооо, такооой вкусный", 5, true));
            pl.ResetStamina();
            if (pl.TryGetEffect(EffectType.MovementBoost, out PlayerEffect playerEffect))
            {
                playerEffect.Intensity = 7;
                pl.EnableEffect(playerEffect, 10);
            }
        }
    }
}