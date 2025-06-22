using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using ClassicCore.BetterHints;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Cacao : IDrink
    {
        public string Name { get; } = "Какао";

        public string Description { get; } = "Горячее...";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            if (UnityEngine.Random.Range(0, 100) > 95)
            {
                pl.Hint(new(0, -4, "аааай", 5, true));
                pl.Damage(10, "Ожоги в районе полости рта");
                return;
            }
            pl.Hint(new(0, -4, "ммм, вкусно", 5, true));
        }
    }
}