using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using InventorySystem.Items.Usables.Scp244.Hypothermia;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class LiquidNitrogen : IDrink
    {
        public string Name { get; } = "Жидкий азот";

        public string Description { get; } = "";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "З-зачем я э-эт-то с-сделал", 5, true));
            if (pl.Effects.Controller.TryGetEffect(out Hypothermia playerEffect))
            {
                playerEffect.Intensity = 11;
                pl.Effects.Enable(playerEffect, 30);
            }
        }
    }
}