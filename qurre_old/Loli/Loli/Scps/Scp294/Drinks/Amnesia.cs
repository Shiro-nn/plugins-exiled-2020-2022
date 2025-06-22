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
            var role = pl.GetCustomRole();
            Timing.CallDelayed(4, () => Hint("Где я?", 3));
            Timing.CallDelayed(20, () => Hint("Зачем я здесь?", 5));
            Timing.CallDelayed(35, () => Hint("Что это?", 4));
            Timing.CallDelayed(50, () => Hint("ААААААААААААААА", 3));
            Timing.CallDelayed(53, () => Hint("Кажется вспоминаю", 4));
            void Hint(string text, int time)
            {
                if (role != pl.GetCustomRole()) return;
                pl.Hint(new(0, -4, text, time, true));
            }
        }
    }
}