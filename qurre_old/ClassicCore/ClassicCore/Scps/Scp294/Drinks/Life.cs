using Qurre.API;
using ClassicCore.Scps.Scp294.API.Interfaces;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Life : IDrink
    {
        public string Name { get; } = "Жизнь";

        public string Description { get; } = "Жидкая жизнь";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.Hp = pl.MaxHp;
        }
    }
}