using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Cold : IDrink
    {
        public string Name { get; } = "Мороз";

        public string Description { get; } = "Холодное...";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Х-х-холодно..", 5, true));
        }
    }
}