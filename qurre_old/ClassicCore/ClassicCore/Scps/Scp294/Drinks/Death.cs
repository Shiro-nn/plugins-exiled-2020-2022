using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Death : IDrink
    {
        public string Name { get; } = "Смерть";

        public string Description { get; } = "Жидкая смерть";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Kill("Что?");
        }
    }
}