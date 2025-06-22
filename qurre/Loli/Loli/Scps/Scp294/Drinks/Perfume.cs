using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;
using PlayerStatsSystem;

namespace Loli.Scps.Scp294.Drinks
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
            pl.HealthInfomation.Kill(DeathTranslations.Poisoned);
        }
    }
}