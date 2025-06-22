using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Gold : IDrink
    {
        public string Name { get; } = "Золото";

        public string Description { get; } = "Жидкое золото";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Что это?", 5, true));
        }
    }
}