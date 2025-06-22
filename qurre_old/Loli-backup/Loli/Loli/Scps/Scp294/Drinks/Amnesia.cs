using Qurre.API;
using Qurre.API.Objects;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using MEC;
namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Amnesia : IDrink
    {
        public string Name { get; } = "Амнезия";

        public string Description { get; } = "Жидкая амнезия";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.EnableEffect(EffectType.Amnesia, 60);
            pl.Hint(new(0, -4, "Что я?", 3, true));
            Timing.CallDelayed(4, () => pl.Hint(new(0, -4, "Где я?", 3, true)));
            Timing.CallDelayed(20, () => pl.Hint(new(0, -4, "Зачем я здесь?", 5, true)));
            Timing.CallDelayed(35, () => pl.Hint(new(0, -4, "Что это?", 4, true)));
            Timing.CallDelayed(50, () => pl.Hint(new(0, -4, "ААААААААААААААА", 3, true)));
            Timing.CallDelayed(53, () => pl.Hint(new(0, -4, "Кажется вспоминаю", 4, true)));
        }
    }
}