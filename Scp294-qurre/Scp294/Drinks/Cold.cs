using Qurre.API;
using Qurre.API.Objects;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class Cold : IDrink
    {
        public string Name { get; } = "Мороз";

        public string Description { get; } = "Холодное...";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            player.ShowHint("Ваша голова болит от замерзания холодной жидкостью.");
            player.EnableEffect(EffectType.Blinded);
        }
    }
}