using Qurre.API;
using Scp294.API.Interfaces;

namespace Scp294.Drinks
{
    public sealed class CallOfDuty : IDrink
    {
        public string Name { get; } = "Call of duty";

        public string Description { get; } = "Описание отсутствует.";

        public ItemType[] Items { get; } = new ItemType[]
        {

        };

        public bool OnStartDrinking(Player _)
        {
            return true;
        }

        public void OnDrank(Player player)
        {
            player.ShowHint("На вкус как Дабстеп и твоя мамаша.");
        }
    }
}