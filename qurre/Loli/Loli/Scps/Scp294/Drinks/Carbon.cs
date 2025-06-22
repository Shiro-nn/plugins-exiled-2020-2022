using Qurre.API;
using Loli.Scps.Scp294.API.Interfaces;

namespace Loli.Scps.Scp294.Drinks
{
    public sealed class Carbon : IDrink
    {
        public string Name { get; } = "Углерод";

        public string Description { get; } = "Жидкий углерод";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.HealthInfomation.Kill("Ожоги третьей степени");
        }
    }
}