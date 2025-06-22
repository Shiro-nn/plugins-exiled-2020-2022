using Qurre.API;
using Qurre.API.Objects;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Amnesia : IDrink
    {
        public string Name { get; } = "Амнезия";

        public string Description { get; } = "Жидкая амнезия";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            player.EnableEffect(EffectType.Amnesia);
            player.ShowHint("Даниэль, это ты? Что ты делаешь?"); //Отсылка к Amnesia
        }
    }
}