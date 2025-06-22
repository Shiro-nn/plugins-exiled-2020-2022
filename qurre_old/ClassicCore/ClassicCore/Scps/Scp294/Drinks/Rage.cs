using Qurre.API;
using Qurre.API.Objects;
using ClassicCore.Scps.Scp294.API.Interfaces;
namespace ClassicCore.Scps.Scp294.Drinks
{
    public sealed class Rage : IDrink
    {
        public string Name { get; } = "Ярость";

        public string Description { get; } = "Жидкая ярость";

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player pl)
        {
            pl.EnableEffect(EffectType.Invigorated, 30);
            pl.Ahp = 150;
            int _hp = (int)pl.Hp;
            pl.Hp = _hp > 50 ? _hp - 50 : 1;
        }
    }
}