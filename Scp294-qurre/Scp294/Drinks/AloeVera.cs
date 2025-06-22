using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class AloeVera : IDrink
    {
        public string Name { get; } = "Алоэ вера";

        public string Description { get; } = "Жидая алоэ вера";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            
        }
    }
}