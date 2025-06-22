using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using Loli.BetterHints;
using MEC;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Iron : IDrink
    {
        public string Name { get; } = "Железо";

        public string Description { get; } = "Жидкое железо";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Ну круто наверное?", 5, true));
            Timing.CallDelayed(10f, () =>
            {
                pl.Hint(new(0, -4, "ай", 5, true));
                pl.HealthInfomation.Damage(25, "Окисление желудка");
            });
        }
    }
}