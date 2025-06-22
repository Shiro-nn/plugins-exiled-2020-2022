using Qurre.API;
using Qurre.API.Objects;
using Loli.Scps.Scp294.API.Interfaces;

namespace Loli.Scps.Scp294.Drinks
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
            pl.Effects.Enable(EffectType.Invigorated, 30);
            pl.HealthInfomation.Ahp = 150;
            int _hp = (int)pl.HealthInfomation.Hp;
            pl.HealthInfomation.Hp = _hp > 50 ? _hp - 50 : 1;
        }
    }
}