using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
using PlayerStatsSystem;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Perfume : IDrink
    {
        public string Name { get; } = "Яд";

        public string Description { get; } = "Самый настоящий";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Kill(DeathTranslations.Poisoned);
        }
    }
}