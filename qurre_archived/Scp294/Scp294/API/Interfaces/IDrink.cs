using Qurre.API;

namespace Scp294.API.Interfaces
{
    public interface IDrink
    {
        string Name { get; }
        string Description { get; }
        ItemType[] Items { get; }

        /// <returns>IsAllowed</returns>
        bool OnStartDrinking(Player player);
        void OnDrank(Player player);
    }
}