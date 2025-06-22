using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class SeaWater : IDrink
    {
        public string Name { get; } = "Морская вода";

        public string Description { get; } = "Просто морская вода";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hint(new(0, -4, "Как же захотелось пить..", 5, true));
            pl.ReferenceHub.fpc.ModifyStamina(0);
        }
    }
}